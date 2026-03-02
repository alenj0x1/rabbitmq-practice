using MassTransit;
using Shared.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public class ReportsService(IPublishEndpoint publishEndpoint)
    {
        public async Task Generate(GenerateReport model)
        {
            var progress = model.InitialProgress;

            await Task.Delay(1000);
            progress.Percentage = 25;
            progress.PercentageMessage = "Generating";
            await publishEndpoint.Publish(progress);

            await Task.Delay(1000);
            progress.Percentage = 50;
            progress.PercentageMessage = "Saving on disk";
            await publishEndpoint.Publish(progress);

            await Task.Delay(1000);
            progress.Percentage = 100;
            progress.PercentageMessage = "Completed";
            progress.ReportId = Guid.NewGuid();
            progress.FilePath = "/";
            await publishEndpoint.Publish(progress);
        }
    }
}
