using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Contract;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController(IPublishEndpoint publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Generate([FromQuery] string content, [FromQuery] int userId)
        {
            var processId = Guid.NewGuid();
            var progress = new ReportProgress
            {
                ProcessId = processId,
                UserId = userId,
                Percentage = 0,
                PercentageMessage = "Started"
            };

            await publishEndpoint.Publish(new GenerateReport
            {
                ProcessId = processId,
                UserId = userId,
                Content = content,
                InitialProgress = progress
            });

            return Ok(progress);
        }
    }
}
