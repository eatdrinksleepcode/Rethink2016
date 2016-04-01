using System.Collections.Generic;

namespace Rethink
{
    public class PricingSummary
    {
        public PricingSummary()
        {
            ProductPrices = new Dictionary<int, decimal>();
        }
        
        public Dictionary<int, decimal> ProductPrices { get; set; }
		public decimal SubTotal { get; set; }
		public decimal Taxes { get; set; }
		public decimal Total { get { return SubTotal + Taxes; } }
	}
}
