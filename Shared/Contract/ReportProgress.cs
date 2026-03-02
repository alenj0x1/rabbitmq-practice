using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contract
{
    public class ReportProgress
    {
        public required Guid ProcessId { get; set; }
        public required int UserId { get; set; }
        public Guid? ReportId { get; set; }
        public int Percentage { get; set; } = 0;
        public string PercentageMessage { get; set; } = "";
        public string? FilePath { get; set; }
    }
}
