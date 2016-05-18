using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bittrex
{
    public class CompletedOrder
    {
        public String OrderUuid { get; set; }
        public String Exchange { get; set; }
        public DateTime TimeStamp { get; set; }
        public OpenOrderType OrderType { get; set; }
        public Decimal Limit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityRemaining { get; set; }
        public Decimal Commission { get; set; }
        public Decimal Price { get; set; }
        public Decimal PricePerUnit { get; set; }
        public Boolean IsConditional { get; set; }
        public String Condition { get; set; }
        public String ConditionTarget { get; set; }
        public Boolean ImmediateOrCancel { get; set; }
    }
}
