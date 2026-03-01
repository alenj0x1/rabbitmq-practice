using MassTransit;
using Shared.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            // DLX created automatically, with prefix queue[_error]
            if (context.Message.DebugException)
            {
                throw new Exception();
            }

            Console.WriteLine(context.Message.Product);
        }
    }
}
