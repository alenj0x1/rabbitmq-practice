using MassTransit;
using OrderService.Consumers;
using OrderService.Hubs;
using Scalar.AspNetCore;
using Shared.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReportProgressConsumer>()
        .Endpoint(e => e.Name = "report-progress");

    // This is not necessary, mass transit add this automatically, when this called from an constructor
    x.AddRequestClient<CheckStock>();

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

builder.Services.AddCors((options) =>
{
    options.AddPolicy("client", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("client");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ReportHub>("/hubs/reports");

app.Run();
