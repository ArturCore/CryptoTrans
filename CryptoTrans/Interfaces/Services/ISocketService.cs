using WebSocketSharp;

namespace CryptoTrans.Interfaces.Services
{
    public interface ISocketService
    {
        public WebSocket Connect(string streamUrl);
        public void Close(WebSocket webSocket);
    }
}
