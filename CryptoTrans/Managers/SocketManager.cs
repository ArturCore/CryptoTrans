using CryptoTrans.Interfaces.Managers;
using CryptoTrans.Interfaces.Services;
using CryptoTrans.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using WebSocketSharp;

namespace CryptoTrans.Managers
{
    public class SocketManager : ISocketManager
    {
        private ISocketService _socketService;
        public WebSocket Socket {  get; set; }

        public SocketManager(IServiceProvider services)
        {
            _socketService = services.GetService<ISocketService>();
        }
        public void Connect(string streamUrl)
        {
            Socket = new WebSocket(streamUrl);
            Socket.Connect();
        }

        public void Track()
        {            
            Socket.OnMessage += (sender, e) =>
            {
                Console.WriteLine($"{e.Data}\n\n\n");
            };

            Socket.OnClose += (sender, e) =>
            {
                Console.WriteLine("Account webhook closed at " + DateTime.Now.ToString());
            };

            while (true)
            {
                if (Socket.IsAlive)
                {
                    Console.WriteLine("WebSocket is still alive.\n");
                }
                else
                {
                    Console.WriteLine("DEEEEEEAD \n");
                }
                Thread.Sleep(500);
            }
        }
    }
}
