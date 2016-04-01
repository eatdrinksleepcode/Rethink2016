using System.Collections.Generic;

namespace Rethink
{
	public interface ICart
	{
        IEnumerable<ProductToPurchase> Products { get; }
	}
}
