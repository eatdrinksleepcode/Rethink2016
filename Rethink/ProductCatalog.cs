using System.Collections.Generic;

namespace Rethink
{
    public class ProductCatalog : IProductCatalog
    {
        public static readonly ProductCatalog Instance = new ProductCatalog();

        private readonly Dictionary<int, decimal> prices = new Dictionary<int, decimal>();

        public decimal LookupPrice(int productId)
        {
            decimal price;
            if (!prices.TryGetValue(productId, out price))
            {
                price = -1;
            }
            return price;
        }

        public void SetPrice(int productId, decimal price)
        {
            prices[productId] = price;
        }

        public void Clear()
        {
            prices.Clear();
        }
    }
}
