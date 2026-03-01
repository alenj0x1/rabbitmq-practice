using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contract;
using System.Text.Json;

namespace InventoryService
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(-1, stoppingToken);
        }
    }
}
