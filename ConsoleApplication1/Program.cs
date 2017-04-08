using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLSharp.Core;
using TeleSharp.TL;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleIoC ioc = new SimpleIoC();
            #region Register Block
            ioc.Register<IServiceTL, Service>();
            #endregion
            var service = ioc.Resolve<IServiceTL>();
            //var service = new Service();
            //service.Connect().Wait();
            //service.Authorize().Wait();
            Console.ReadKey();
        }
    }
    interface IServiceTL
    {
        Task Connect();
        Task Authorize();
        Task SendMessage(string phone, string message);
    }

    public class Service : IServiceTL
    {
        private TelegramClient client;
        public Service()
        {
            client = new TelegramClient(158868, "ece1a6343e8c1ec89502987b89c68e28", null, "session", (address, port) => new System.Net.Sockets.TcpClient("149.154.167.40", 443));
        }
        public async Task Connect()
        {
            await client.ConnectAsync(true);
        }
        public async Task Authorize()
        {
            var hash = await client.SendCodeRequestAsync("+79538909739");

            //var user = await client.MakeAuthAsync("+79538909739", hash, "1234");
        }

        public async Task SendMessage(string phone, string message)
        {
            var contacts = await client.GetContactsAsync();
            var recipient = contacts.users.lists.Where(x => x.GetType() == typeof(TLUser)).Cast<TLUser>().FirstOrDefault(x => x.phone == phone);
            await client.SendMessageAsync(new TLInputPeerUser() { user_id = recipient.id }, message);
        }
    }
}
