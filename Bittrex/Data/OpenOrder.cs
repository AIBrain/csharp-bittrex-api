namespace Bittrex.Data
{
    using System;

    public class OpenOrder
    {
		public String Uuid{get;set;}
		public String OrderUuid{get;set;}
		public String Exchange{get;set;}
		public OpenOrderType OrderType{get;set;}
		public Decimal Quantity{get;set;}
		public Decimal QuantityRemaining{get;set;}
		public Decimal Limit{get;set;}
		public Decimal CommissionPaid{get;set;}
		public Decimal Price{get;set;}
        //public decimal? PricePerUnit{get;set;}
		public DateTime Opened{get;set;}
		//public string Closed" : null,
		public Boolean CancelInitiated{get;set;}
		public Boolean ImmediateOrCancel{get;set;}
		public Boolean IsConditional{get;set;}
		public String Condition{get;set;}
        public String ConditionTarget { get; set; }
    }
}
