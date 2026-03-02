using MassTransit;
using Microsoft.AspNetCore.SignalR;
using OrderService.Hubs;
using Shared.Contract;

namespace OrderService.Consumers
{
    public class ReportProgressConsumer(IHubContext<ReportHub> hubContext) : IConsumer<ReportProgress>
    {
        public async Task Consume(ConsumeContext<ReportProgress> context)
        {
            var progress = context.Message;
            await hubContext.Clients.Group(progress.UserId.ToString()).SendAsync("ProgressChanged", progress);
        }
    }
}
