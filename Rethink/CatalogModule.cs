using Nancy;
using Nancy.ModelBinding;

namespace Rethink
{
    public class CatalogModule : NancyModule
    {
        public CatalogModule()
        {
            Delete["/catalog"] = _ =>
            {
                ProductCatalog.Instance.Clear();
                return HttpStatusCode.NoContent;
            };

            Put["/catalog/products/{productId:int}"] = parameters =>
            {
                decimal price = this.Bind<decimal>();
                ProductCatalog.Instance.SetPrice(parameters.productId, price);
                return HttpStatusCode.NoContent;
            };
        }
    }
}
