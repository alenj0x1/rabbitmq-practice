using InventoryService.Classes;
using RabbitMQ.Client;
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

            // exchanges
            await channel.ExchangeDeclareAsync("orders.exchange", ExchangeType.Fanout, cancellationToken: stoppingToken);
            await channel.ExchangeDeclareAsync("orders.inventory.dead.exchange", ExchangeType.Direct, cancellationToken: stoppingToken); // DLX Dead Letter Exchange

            // queues
            // note: property durable, match with basic property "Persistent".
            await channel.QueueDeclareAsync(
                queue:"orders.inventory.queue", 
                durable: true, autoDelete: false, 
                exclusive: false, 
                cancellationToken: stoppingToken, 
                arguments:
                    new Dictionary<string, object?>()
                    {
                        { "x-dead-letter-exchange", "orders.inventory.dead.exchange" }
                    });
            await channel.QueueDeclareAsync("orders.inventory.dead.queue", durable: true, autoDelete: false, exclusive: false, cancellationToken: stoppingToken);

            // bind exchanges with queues
            await channel.QueueBindAsync("orders.inventory.queue", "orders.exchange", "", cancellationToken: stoppingToken);
            await channel.QueueBindAsync("orders.inventory.dead.queue", "orders.inventory.dead.exchange", "", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    OrderCreated? data = JsonSerializer.Deserialize<OrderCreated>(ea.Body.ToArray())
                        ?? throw new Exception("unknown data");

                    if (data.DebugException)
                    {
                        throw new Exception("debug exception");
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception)
                {

                    await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };
            await channel.BasicConsumeAsync("orders.inventory.queue", autoAck: false, consumerTag: "taka", noLocal: false, exclusive: false, null, consumer, cancellationToken: stoppingToken);

            await Task.Delay(-1, stoppingToken);
        }
    }
}
