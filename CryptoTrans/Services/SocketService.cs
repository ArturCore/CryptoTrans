using CryptoTrans.Interfaces.Services;
using Microsoft.Extensions.Options;
using WebSocketSharp;

namespace CryptoTrans.Services
{
    public class SocketService : ISocketService
    {
        public WebSocket Connect(string streamUrl)
        {
            WebSocket webSocket = new(streamUrl);
            return webSocket;
        }

        public void Close(WebSocket webSocket)
        {
            webSocket.Close();
        }
    }
}
