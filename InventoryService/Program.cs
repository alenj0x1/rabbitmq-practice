using InventoryService;
using InventoryService.Classes;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<RabbitMQConnection>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<RabbitMQConnection>());
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
