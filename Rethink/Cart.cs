using System.Collections.Generic;
using System.Linq;

namespace Rethink
{
	public class Cart
	{
		private readonly Dictionary<int, int> products = new Dictionary<int, int> ();
        private readonly IProductCatalog catalog;
        private readonly ITaxCalculator taxCalculator;

        public Cart(IProductCatalog catalogService, ITaxCalculator pricingService)
        {
            this.catalog = catalogService;
            this.taxCalculator = pricingService;
        }

		public void IncludeProduct (int productId, int quantity)
		{
			products [productId] = quantity;
		}

        public PricingSummary PriceCart()
        {
            var productTotals = (from p in products
                                 let price = catalog.LookupPrice(p.Key)
                                 where price != -1m
                                 select new { ProductId = p.Key, Price = price, Quantity = p.Value })
                .ToDictionary(p => p.ProductId, p => p.Price * p.Quantity);
            var subTotal = productTotals.Values.Sum();
            return new PricingSummary
            {
                ProductPrices = productTotals,
                SubTotal = subTotal,
                Taxes = taxCalculator.CalculateTaxes(subTotal),
            };
        }

        public PricingSummary PriceCart2()
        {
            var productPrices = PriceProducts(products.Keys);
            var summary = CalculatePricing(products, productPrices);
            summary.Taxes = taxCalculator.CalculateTaxes(summary.SubTotal);
            return summary;
        }

        public IDictionary<int, decimal> PriceProducts(IEnumerable<int> products)
        {
            return (from p in products
                    let price = catalog.LookupPrice(p)
                    select new { ProductId = p, Price = price }).ToDictionary(p => p.ProductId, p => p.Price);
        }

        public PricingSummary CalculatePricing(IDictionary<int, int> productsToPurchase, IDictionary<int, decimal> productPrices)
        {
            var productTotals = (from price in productPrices
                                 join product in productsToPurchase on price.Key equals product.Key
                                 where price.Value != -1
                                 select new { ProductId = price.Key, Quantity = product.Value, Price = price.Value })
                .ToDictionary(p => p.ProductId, p => p.Price * p.Quantity);
            var subTotal = productTotals.Values.Sum();
            return new PricingSummary
            {
                ProductPrices = productTotals,
                SubTotal = subTotal,
            };
        }
    }
}
