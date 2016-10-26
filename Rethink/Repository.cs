using System.Collections.Generic;
namespace Rethink
{
	public class Repository
	{
        private int nextCartId = 0;
		private static readonly Dictionary<int, Cart> carts = new Dictionary<int, Cart> ();

        public int NewCart()
        {
            var newId = ++nextCartId;
            carts.Add(newId, new Cart(ProductCatalog.Instance, new TaxCalculator()));
            return newId;
        }

		public Cart LoadCart (int cartId)
		{
			return carts [cartId];
		}
	}
}
