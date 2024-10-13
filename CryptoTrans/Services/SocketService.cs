using CryptoTrans.Interfaces;
using Microsoft.Extensions.Options;
using WebSocketSharp;

namespace CryptoTrans.Services
{
    public class SocketService : ISockerService
    {
        public SocketService(IOptions<SocketService> options) 
        { 
            
        }
        public WebSocket ConnectPermanentWebsocker(string streamUrl)
        {
            WebSocket webSocket = new(streamUrl);
            return webSocket;
        }

        public WebSocket ConnectTemporaryWebsocker(string streamUrl)
        {
            throw new NotImplementedException();
        }
    }
}
