namespace Bittrex {
    using System;
    using Data;

    public interface IExchange {
        void Initialise( ExchangeContext context );
        AccountBalance GetBalance( String market );
        GetBalancesResponse GetBalances();
        Decimal CalculateMinimumOrderQuantity( String market, Decimal price );
        dynamic GetMarkets();
        dynamic GetTicker( String market );
        GetOpenOrdersResponse GetOpenOrders( String market );
        OrderResponse PlaceBuyOrder( String market, Decimal quantity, Decimal price );
        OrderResponse PlaceSellOrder( String market, Decimal quantity, Decimal price );
        void CancelOrder( String uuid );

        /// <summary>
        ///     Used to retrieve the orderbook for a given market
        /// </summary>
        /// <param name="market"></param>
        /// <param name="type">The type of orderbook to return.</param>
        /// <param name="depth">How deep of an order book to retrieve. Max is 50</param>
        /// <returns></returns>
        GetOrderBookResponse GetOrderBook( String market, OrderBookType type, Int32 depth = 20 );

        /// <summary>
        ///     Used to retrieve the latest trades that have occured for a specific market.
        /// </summary>
        /// <param name="market"></param>
        /// <param name="count">a number between 1-50 for the number of entries to return</param>
        /// <returns></returns>
        GetMarketHistoryResponse GetMarketHistory( String market, Int32 count = 20 );

        GetMarketSummaryResponse GetMarketSummary( String market );

        GetOrderHistoryResponse GetOrderHistory( String market, Int32 count = 10 );
    }
}
