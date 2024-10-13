using WebSocketSharp;

namespace CryptoTrans.Interfaces
{
    public interface ISockerService
    {
        public WebSocket ConnectPermanentWebsocker(string streamUrl);
        public WebSocket ConnectTemporaryWebsocker(string streamUrl);
    }
}
