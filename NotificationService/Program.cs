using MassTransit;
using NotificationService;
using NotificationService.Consumers;
var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>()
        .Endpoint(e => e.Name = "notification-order-created");

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
