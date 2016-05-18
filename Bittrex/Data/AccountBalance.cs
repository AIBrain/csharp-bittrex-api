namespace Bittrex.Data
{
    using System;

    public class AccountBalance
    {
        public String Currency { get; set; }
        public Decimal Balance { get; set; }
        public Decimal Available { get; set; }
        public Decimal Pending { get; set; }
        public String CryptoAddress { get; set; }
        public Boolean Requested { get; set; }
        public String Uuid { get; set; }
    }
}
