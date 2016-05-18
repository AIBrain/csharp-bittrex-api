namespace Bittrex {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using Data;

    public class Exchange : IExchange {
        private const String ApiCallTemplate = "https://bittrex.com/api/{0}/{1}";
        private const String ApiVersion = "v1.1";
        private const String ApiCallGetMarkets = "public/getmarkets";
        private const String ApiCallGetTicker = "public/getticker";
        private const String ApiCallGetOrderBook = "public/getorderbook";
        private const String ApiCallGetMarketHistory = "public/getmarkethistory";
        private const String ApiCallGetMarketSummary = "public/getmarketsummary";

        private const String ApiCallGetBalances = "account/getbalances";
        private const String ApiCallGetBalance = "account/getbalance";
        private const String ApiCallGetOrderHistory = "account/getorderhistory";

        private const String ApiCallBuyLimit = "market/buylimit";
        private const String ApiCallSellLimit = "market/selllimit";
        private const String ApiCallGetOpenOrders = "market/getopenorders";
        private const String ApiCallCancel = "market/cancel";

        private SecureString ApiKey {
            get; set;
        }

        private SecureString Secret {
            get; set;
        }

        private String QuoteCurrency {
            get; set;
        }

        private Boolean Simulate {
            get; set;
        }

        private ApiCall ApiCall {
            get; set;
        }

        public void Initialise( ExchangeContext context ) {
            this.ApiKey = context.ApiKey;
            this.Secret = context.Secret;
            this.QuoteCurrency = context.QuoteCurrency;
            this.Simulate = context.Simulate;
            this.ApiCall = new ApiCall( this.Simulate );
        }

        public AccountBalance GetBalance( String market ) {
            return this.Call<AccountBalance>( ApiCallGetBalance, Tuple.Create( "currency", market ) );
        }

        public GetBalancesResponse GetBalances() {
            return this.Call<GetBalancesResponse>( ApiCallGetBalances );
        }

        public OrderResponse PlaceBuyOrder( String market, Decimal quantity, Decimal price ) {
            return this.Call<OrderResponse>( ApiCallBuyLimit, Tuple.Create( "market", GetMarketName( market ) ), Tuple.Create( "quantity", quantity.ToString() ), Tuple.Create( "rate", price.ToString() ) );
        }

        public OrderResponse PlaceSellOrder( String market, Decimal quantity, Decimal price ) {
            return this.Call<OrderResponse>( ApiCallSellLimit, Tuple.Create( "market", GetMarketName( market ) ), Tuple.Create( "quantity", quantity.ToString() ), Tuple.Create( "rate", price.ToString() ) );
        }

        public Decimal CalculateMinimumOrderQuantity( String market, Decimal price ) {
            var minimumQuantity = Math.Round( 0.00050000M / price, 1 ) + 0.1M;
            return minimumQuantity;
        }

        public dynamic GetMarkets() {
            return this.Call<dynamic>( ApiCallGetMarkets );
        }

        public dynamic GetTicker( String market ) {
            return this.Call<dynamic>( ApiCallGetTicker, Tuple.Create( "market", GetMarketName( market ) ) );
        }

        public GetOpenOrdersResponse GetOpenOrders( String market ) {
            return this.Call<GetOpenOrdersResponse>( ApiCallGetOpenOrders, Tuple.Create( "market", GetMarketName( market ) ) );
        }

        public void CancelOrder( String uuid ) {
            this.Call<dynamic>( ApiCallCancel, Tuple.Create( "uuid", uuid ) );
        }

        public GetOrderBookResponse GetOrderBook( String market, OrderBookType type, Int32 depth = 20 ) {
            if ( type == OrderBookType.Both ) {
                return this.Call<GetOrderBookResponse>( ApiCallGetOrderBook, Tuple.Create( "market", GetMarketName( market ) ), Tuple.Create( "type", type.ToString().ToLower() ), Tuple.Create( "depth", depth.ToString() ) );
            }
            var results = this.Call<List<OrderEntry>>( ApiCallGetOrderBook, Tuple.Create( "market", this.GetMarketName( market ) ), Tuple.Create( "type", type.ToString().ToLower() ), Tuple.Create( "depth", depth.ToString() ) );

            switch ( type ) {
                case OrderBookType.Buy:
                    return new GetOrderBookResponse { buy = results };
            }
            return new GetOrderBookResponse { sell = results };
        }

        public GetMarketHistoryResponse GetMarketHistory( String market, Int32 count = 20 ) {
            return this.Call<GetMarketHistoryResponse>( ApiCallGetMarketHistory, Tuple.Create( "market", GetMarketName( market ) ), Tuple.Create( "count", count.ToString() ) );
        }

        public GetMarketSummaryResponse GetMarketSummary( String market ) {
            return this.Call<GetMarketSummaryResponse[]>( ApiCallGetMarketSummary, Tuple.Create( "market", GetMarketName( market ) ) ).Single();
        }

        public GetOrderHistoryResponse GetOrderHistory( String market, Int32 count = 20 ) {
            return this.Call<GetOrderHistoryResponse>( ApiCallGetOrderHistory, Tuple.Create( "market", GetMarketName( market ) ), Tuple.Create( "count", count.ToString() ) );
        }

        private static String HashHmac( String message, SecureString secret ) {
            using ( var hmac = new HMACSHA512( Encoding.UTF8.GetBytes( secret.ToString() ) ) ) {
                var msg = Encoding.UTF8.GetBytes( message );
                var hash = hmac.ComputeHash( msg );
                return BitConverter.ToString( hash ).ToLower().Replace( "-", String.Empty );
            }
        }

        private String GetMarketName( String market ) {
            return this.QuoteCurrency + "-" + market;
        }

        private T Call<T>( String method, params Tuple<String, String>[] parameters ) {
            if ( method.StartsWith( "public" ) ) {
                var uri = String.Format( ApiCallTemplate, ApiVersion, method );
                if ( parameters != null && parameters.Any() ) {
                    var extraParameters = new StringBuilder();
                    foreach ( var item in parameters ) {
                        extraParameters.Append( ( extraParameters.Length == 0 ? "?" : "&" ) + item.Item1 + "=" + item.Item2 );
                    }

                    if ( extraParameters.Length > 0 ) {
                        uri = uri + extraParameters;
                    }
                }

                return this.ApiCall.CallWithJsonResponse<T>( uri, false );
            }
            else {
                var nonce = DateTime.UtcNow.Ticks;
                var uri = String.Format( ApiCallTemplate, ApiVersion, $"{method}?apikey={this.ApiKey}&nonce={nonce}" );

                if ( parameters != null ) {
                    var extraParameters = new StringBuilder();
                    foreach ( var item in parameters ) {
                        extraParameters.Append( "&" + item.Item1 + "=" + item.Item2 );
                    }

                    if ( extraParameters.Length > 0 ) {
                        uri = uri + extraParameters;
                    }
                }

                var sign = HashHmac( uri, this.Secret );
                return this.ApiCall.CallWithJsonResponse<T>( uri, !method.StartsWith( "market/get" ) && !method.StartsWith( "account/get" ), Tuple.Create( "apisign", sign ) );
            }
        }
    }
}
