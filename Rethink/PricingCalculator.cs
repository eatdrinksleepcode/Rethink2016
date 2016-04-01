using System.Linq;
using System.Collections.Generic;

namespace Rethink
{
    public class PricingCalculator
    {
        private readonly IProductCatalog catalog;
        private readonly ITaxCalculator taxCalculator;

        public PricingCalculator(IProductCatalog catalogService, ITaxCalculator pricingService)
        {
            this.catalog = catalogService;
            this.taxCalculator = pricingService;
        }

        public PricingSummary PriceCart(ICart cart)
        {
            var productTotals = (from p in cart.Products
                                 let price = catalog.LookupPrice(p.ProductId)
                                 where price != -1m
                                 select new { ProductId = p.ProductId, Price = price, Quantity = p.Quantity })
                .ToDictionary(p => p.ProductId, p => p.Price * p.Quantity);
            var subTotal = productTotals.Values.Sum();
            return new PricingSummary
            {
                ProductPrices = productTotals,
                SubTotal = subTotal,
                Taxes = taxCalculator.CalculateTaxes(subTotal),
            };
        }

        public PricingSummary PriceCart2(ICart cart)
        {
            var productPrices = PriceProducts(cart.Products.Select(p => p.ProductId));
            var summary = CalculatePricing(cart.Products, productPrices);
            summary.Taxes = taxCalculator.CalculateTaxes(summary.SubTotal);
            return summary;
        }

        public IDictionary<int, decimal> PriceProducts(IEnumerable<int> products)
        {
            return (from p in products
                    let price = catalog.LookupPrice(p)
                    select new { ProductId = p, Price = price }).ToDictionary(p => p.ProductId, p => p.Price);
        }

        public PricingSummary CalculatePricing(IEnumerable<ProductToPurchase> productsToPurchase, IDictionary<int, decimal> productPrices)
        {
            var productTotals = (from price in productPrices
                                 join product in productsToPurchase on price.Key equals product.ProductId
                                 where price.Value != -1
                                 select new { ProductId = price.Key, Quantity = product.Quantity, Price = price.Value })
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
