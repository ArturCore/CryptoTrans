using CryptoTrans.Configurations;
using CryptoTrans.Interfaces.Managers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using System.Reflection;
using WebSocketSharp;

namespace CryptoTrans.Services
{    
    public class BinanceOrderBookService : BackgroundService
    {
        string streamUrl;
        private ISocketManager _socketManager;
        public BinanceOrderBookService(IOptions<BinanceSettings> options, ISocketManager socketManager) 
        {
            streamUrl = options.Value.StreamUrl;
            _socketManager = socketManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _socketManager.Connect(streamUrl);
            _socketManager.Track();
        }
    }
}
