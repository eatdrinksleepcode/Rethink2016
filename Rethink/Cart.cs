using System.Collections.Generic;
using System.Linq;

namespace Rethink
{
	public class Cart : ICart
	{
		private readonly Dictionary<int, int> products = new Dictionary<int, int> ();
		
        public IEnumerable<ProductToPurchase> Products {
            get { return products.Select(p => new ProductToPurchase { ProductId = p.Key, Quantity = p.Value }); }
		}

		public void IncludeProduct (int productId, int quantity)
		{
			products [productId] = quantity;
		}
	}
}
