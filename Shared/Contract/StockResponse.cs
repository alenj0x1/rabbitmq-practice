using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contract
{
    public class StockResponse
    {
        public bool IsAvailable { get; set; }
        public int CurrentStock { get; set; }
    }
}
