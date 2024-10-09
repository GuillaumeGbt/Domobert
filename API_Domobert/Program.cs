using Common;
using Dal = DAL.Models;
using Bll = BLL.Models;
using sDal = DAL.Services;
using sBll = BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using BLL.BackgroundServices;
using DAL.Models;
using BLL.Services;
using BLL.Initializers;
using Microsoft.Extensions.Hosting;
using API_Domobert.Hubs;
using API_Domobert.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IDeviceRepository<Bll.Device>, sBll.DeviceService>();
builder.Services.AddScoped<IDeviceRepository<Dal.Device>, sDal.DeviceService>();

builder.Services.AddScoped<sBll.HistoryTempService>();
builder.Services.AddScoped<sDal.HistoryTempService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// MQTT service

var mqttConfig = builder.Configuration.GetSection("Mqtt").Get<DAL.Services.MqttService.Configuration>()
                  ?? throw new Exception("Mqtt Config is missing");
builder.Services.AddSingleton<sDal.MqttService>(sp =>
{
    return new DAL.Services.MqttService(mqttConfig);
});

builder.Services.AddSingleton(
    builder.Configuration.GetSection("Mqtt").Get<DAL.Services.MqttService.Configuration>()
    ?? throw new Exception("Mqtt Config is missing")
);

builder.Services.AddTransient<BLL.Services.MqttService>();

builder.Services.AddSingleton<MqttBackgroundService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MqttBackgroundService>());



builder.Services.AddSingleton<MqttConfigService>();
builder.Services.AddSingleton<MqttConfigInitializer>();

//SIGNALR
builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationService, NotificationService>();


// TODO CORS MODIFICATION
builder.Services.AddCors(opt => opt.AddPolicy(name: "myPolicy", blder => {
    blder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));


var app = builder.Build();

// Initialiser la configuration MQTT au démarrage
var mqttConfigInitializer = app.Services.GetRequiredService<MqttConfigInitializer>();
mqttConfigInitializer.InitializeConfig();

//Configurer SignalR
app.MapHub<DeviceHub>("/deviceHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//TODO
app.UseCors("myPolicy");

app.Run();
