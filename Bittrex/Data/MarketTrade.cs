namespace Bittrex.Data
{
    using System;

    public class MarketTrade
    {
        public Int32 Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Price { get; set; }
        public Decimal Total { get; set; }
        public FillType FillType { get; set; }
        public OrderType OrderType { get; set; }
    }
}
