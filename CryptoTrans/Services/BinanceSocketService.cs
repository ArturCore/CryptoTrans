using CryptoTrans.Configurations;
using Microsoft.Extensions.Options;
using WebSocketSharp;

namespace CryptoTrans.Services
{
    public class BinanceSocketService : BackgroundService
    {
        private DateTime _lastMessageReceived;

        private WebSocket _socket;
        private WebSocket _accountSocket;
        private FileService _fileService;
        private CancellationTokenSource _cts;

        private readonly string? _apiKey;
        private readonly string? _apiSecret;
        private readonly string? _testNetUrl;
        private readonly string? _accountStreamUrl;

        public BinanceSocketService(IOptions<BinanceSettings> options)
        {
            _lastMessageReceived = DateTime.Now;

            _socket = new WebSocket(options.Value.StreamUrl);
            _fileService = new FileService();
            _cts = new CancellationTokenSource();

            _apiKey = options.Value.ApiKey;
            _apiSecret = options.Value.ApiSecret;
            _testNetUrl = options.Value.TestNetUrl;
            _accountStreamUrl = options.Value.AccountStreamUrl;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetWebhook();
            //SetAccountWebhook();
            while (!_cts.IsCancellationRequested && !stoppingToken.IsCancellationRequested)
            {
                if ((DateTime.Now - _lastMessageReceived).TotalSeconds > 10)
                {
                    Console.WriteLine("No messages received for 10 seconds. Closing WebSocket.");
                    _socket.Close();
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private void SetWebhook()
        {
            _socket.Connect();

            _socket.OnMessage += (sender, e) =>
            {
                _fileService.WriteToFile(e.Data);
                Console.WriteLine("String is recorded!");
                _lastMessageReceived = DateTime.Now;
            };

            _socket.OnClose += (sender, e) =>
            {
                Console.WriteLine("OnClose at " + DateTime.Now.ToString());
                _cts.Cancel();
                _fileService.OnClose();
                Console.WriteLine("On more console log.");

                Environment.Exit(0);
            };
        }

        private async void SetAccountWebhook()
        {
            string listenKey = await GetListenKey();

            string socketUrl = $@"{_accountStreamUrl}{listenKey}";
            using (var ws = new WebSocket(socketUrl))
            {
                ws.OnMessage += (sender, e) =>
                {
                    Console.WriteLine($"{e.Data}\n\n\n");
                };

                ws.OnClose += (sender, e) =>
                {
                    Console.WriteLine("Account webhook closed at " + DateTime.Now.ToString());
                    _cts.Cancel();
                };

                ws.Connect();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private async Task<string> GetListenKey()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-MBX-APIKEY", _apiKey);

                var response = await client.PostAsync($"{_testNetUrl}/fapi/v1/listenKey", null);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                return jsonResponse.listenKey;
            }
        }
    }
}