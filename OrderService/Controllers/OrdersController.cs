using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Shared.Contract;
using System.Text;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IPublishEndpoint publishEndpoint, IRequestClient<CheckStock> requestClientCheckStock) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreated order)
        {
            order.OrderId = Guid.NewGuid();
            await publishEndpoint.Publish(order);

            return Ok("hello!");
        }

        [HttpGet("validate-stock/:productName")]
        public async Task<IActionResult> ValidateStock(string productName, [FromQuery] int quantity)
        {
            var response = await requestClientCheckStock.GetResponse<StockResponse>(new CheckStock
            {
                Product = productName,
                Quantity = quantity
            });

            return Ok(response);
        }
    }
}
