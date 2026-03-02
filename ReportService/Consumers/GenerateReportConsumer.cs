using MassTransit;
using ReportService.Services;
using Shared.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Consumers
{
    public class GenerateReportConsumer(ReportsService reportsService) : IConsumer<GenerateReport>
    {
        public async Task Consume(ConsumeContext<GenerateReport> context)
        {
            await reportsService.Generate(context.Message);

            Console.WriteLine("taka");
        }
    }
}
