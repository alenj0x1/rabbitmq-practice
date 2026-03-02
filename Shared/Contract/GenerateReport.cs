using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contract
{
    public class GenerateReport
    {
        public required Guid ProcessId { get; set; }
        public required int UserId { get; set; }
        public required string Content { get; set; }
        public required ReportProgress InitialProgress { get; set; }
    }
}
