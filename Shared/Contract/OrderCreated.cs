using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contract
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }
        [DefaultValue("computer")]
        public string Product { get; set; }
        [DefaultValue(1)]
        public int Quantity { get; set; }
        [DefaultValue(true)]
        public bool DebugException { get; set; }
    }
}
