using WebSocketSharp;

namespace CryptoTrans.Interfaces.Managers
{
    public interface ISocketManager
    {
        public WebSocket Socket { get; set; }
        public void Connect(string streamUrl);
        public void Track();
    }
}
