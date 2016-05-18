using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bittrex
{
    public class ExchangeContext
    {
        public String ApiKey { get; set; }
        public String Secret { get; set; }
        public String QuoteCurrency  { get; set; }
        public Boolean Simulate { get; set; }
    }
}
