using NotificationService.Classes;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contract;
using System.Text.Json;

namespace NotificationService
{
    public class Worker(RabbitMQConnection rabbitMQConnection) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var channel = await rabbitMQConnection.Connection.CreateChannelAsync(cancellationToken: stoppingToken);

            // exchanges
            await channel.ExchangeDeclareAsync("orders.exchange", ExchangeType.Fanout, cancellationToken: stoppingToken);

            // queues
            await channel.QueueDeclareAsync(
                queue: "orders.notifications.queue", 
                durable: true, 
                autoDelete: false, 
                exclusive: false, 
                cancellationToken: stoppingToken);

            // bind exchanges with queues
            await channel.QueueBindAsync("orders.notifications.queue", "orders.exchange", "", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                OrderCreated? data = JsonSerializer.Deserialize<OrderCreated>(ea.Body.ToArray());
                var taka = "";
            };
            await channel.BasicConsumeAsync("orders.notifications.queue", autoAck: true, consumerTag: "taka", noLocal: false, exclusive: false, null, consumer, cancellationToken: stoppingToken);

            await Task.Delay(-1, stoppingToken);
        }
    }
}
