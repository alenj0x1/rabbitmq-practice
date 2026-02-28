using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Classes;
using RabbitMQ.Client;
using Shared.Contract;
using System.Text;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(RabbitMQConnection rabbitMQConnection) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create()
        {
            using var channel = await rabbitMQConnection.Connection.CreateChannelAsync();

            var queue = await channel.QueueDeclareAsync("order.queue", durable: true, autoDelete: false, exclusive: false);

            var order = new OrderCreated
            {
                OrderId = Guid.NewGuid(),
                Product = "computer",
                Quantity = 1
            };
            var body = JsonSerializer.SerializeToUtf8Bytes(order);
            await channel.BasicPublishAsync("", "order.queue", false, new BasicProperties(), body);

            return Ok("hello!");
        }
    }
}
