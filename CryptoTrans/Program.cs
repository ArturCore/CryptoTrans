using CryptoTrans.Configurations;
using CryptoTrans.Interfaces.Managers;
using CryptoTrans.Interfaces.Services;
using CryptoTrans.Managers;
using CryptoTrans.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddSingleton<ISocketService, SocketService>();
builder.Services.AddTransient<ISocketManager, SocketManager>();

builder.Services.Configure<BinanceSettings>(builder.Configuration.GetSection("Binance"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<BinanceSocketService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();