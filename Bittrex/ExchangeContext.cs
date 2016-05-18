
namespace Bittrex
{
    using System;
    using System.Security;

    public class ExchangeContext
    {
        public SecureString ApiKey { get; set; }
        public SecureString Secret { get; set; }
        public String QuoteCurrency  { get; set; }
        public Boolean Simulate { get; set; }
    }
}
