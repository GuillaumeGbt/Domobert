using Common;
using Dal = DAL.Models;
using Bll = BLL.Models;
using sDal = DAL.Services;
using sBll = BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IDeviceRepository<Bll.Device>, BLL.Services.DeviceService>();
builder.Services.AddScoped<IDeviceRepository<Dal.Device>, DAL.Services.DeviceService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// MQTT service
builder.Services.AddControllers();
builder.Services.AddSingleton(
    builder.Configuration.GetSection("Mqtt").Get<sDal.MqttService.Configuration>()
    ?? throw new Exception("Mqtt Config is missing")
);
builder.Services.AddTransient<sBll.MqttService>();


// TODO CORS MODIFICATION
builder.Services.AddCors(opt => opt.AddPolicy(name: "myPolicy", blder => {
    blder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));


var app = builder.Build();

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
