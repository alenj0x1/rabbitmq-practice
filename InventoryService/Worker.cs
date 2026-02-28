using InventoryService.Classes;
using RabbitMQ.Client.Events;
using Shared.Contract;
using System.Text.Json;

namespace InventoryService
{
    public class Worker(RabbitMQConnection rabbitMQConnection) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var channel = await rabbitMQConnection.Connection.CreateChannelAsync(cancellationToken: stoppingToken);
            var queue = await channel.QueueDeclareAsync("order.queue", durable: true, autoDelete: false, exclusive: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                OrderCreated? data = JsonSerializer.Deserialize<OrderCreated>(ea.Body.ToArray());
                var taka = "";
            };
            await channel.BasicConsumeAsync("order.queue", autoAck: true, consumerTag: "taka", noLocal: false, exclusive: false, null, consumer, cancellationToken: stoppingToken);

            await Task.Delay(-1, stoppingToken);
        }
    }
}
