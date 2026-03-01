using InventoryService;
using InventoryService.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>(c =>
        {
            // by consumer reply
            c.UseMessageRetry(r => r.Intervals(
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(30)
                ));
        })
        .Endpoint(e => e.Name = "inventory-order-created");

    x.AddConsumer<CheckStockConsumer>()
        .Endpoint(e => e.Name = "check-stock");

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(ctx);

        // general retry
        cfg.UseMessageRetry(r => r.Intervals(
                TimeSpan.FromSeconds(5)
        ));
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
