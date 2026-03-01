using MassTransit;
using Shared.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Consumers
{
    public class CheckStockConsumer : IConsumer<CheckStock>
    {
        public async Task Consume(ConsumeContext<CheckStock> context)
        {
            await context.RespondAsync(new StockResponse
            {
                IsAvailable = true,
                CurrentStock = 10
            });
        }
    }
}
