using MassTransit;
using ReportService;
using ReportService.Consumers;
using ReportService.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<ReportsService>();

builder.Services.AddMassTransit((x) =>
{
    x.AddConsumer<GenerateReportConsumer>()
        .Endpoint(e => e.Name = "generate-report");

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
