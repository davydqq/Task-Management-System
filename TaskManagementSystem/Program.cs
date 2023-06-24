using API.Hosted;
using DatabaseContext;
using TaskManagement;
using Transportation;
using Transportation.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add BI
var dbConn = builder.Configuration.GetSection("DatabaseConnection").Value;
var rabbitMqConn = builder.Configuration.GetSection("RabbitMQConnection").Value;

builder.Services.ApplyDataBaseDI(dbConn);
builder.Services.ApplyTransportationModules(rabbitMqConn);
builder.Services.ApplyTaskManagementModules();

builder.Services.AddHostedService<SetupServiceBusHosted>();

var unlockConfig = builder.Configuration.GetSection("RabbitSettings").Get<RabbitSettings>();
builder.Services.AddSingleton(x => unlockConfig);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
