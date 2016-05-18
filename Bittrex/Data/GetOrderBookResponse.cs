namespace Bittrex.Data
{
    using System.Collections.Generic;

    public class GetOrderBookResponse
    {
        public List<OrderEntry> buy { get; set; }
        public List<OrderEntry> sell { get; set; }
    }
}
