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
        public async Task<IActionResult> Create([FromBody] OrderCreated order)
        {
            using var channel = await rabbitMQConnection.Connection.CreateChannelAsync();

            // note: Fanout, for multiple queues // multiple microservices
            await channel.ExchangeDeclareAsync("orders.exchange", ExchangeType.Fanout);

            order.OrderId = Guid.NewGuid();
            var body = JsonSerializer.SerializeToUtf8Bytes(order);

            // note: add here, in BasicProperties, Persistent property, "message save on disk".
            // note: at here, routingKey value, is not necessary, because it's using a exchange and it's fanout, not direct
            // interesting fact: when rabbitmq, not has memory, it's messages, are saved in the disk, and categorized as Paged out
            await channel.BasicPublishAsync("orders.exchange", "", false, new BasicProperties(), body);

            return Ok("hello!");
        }
    }
}
