using Nancy;
using Nancy.ModelBinding;

namespace Rethink
{
    public class PricingModule : NancyModule
    {
        private readonly Repository repository = new Repository();

        public PricingModule()
        {
            Post["/carts"] = _ =>
            {
                return repository.NewCart().ToString();
            };
            
            Put["/carts/{cartId:int}/products/{productId:int}"] = parameters =>
            {
                var quantity = this.Bind<int>();
                repository.LoadCart(parameters.cartId).IncludeProduct(parameters.productId, quantity);
                return HttpStatusCode.NoContent;
            };

            Get["/carts/{cartId:int}/pricing"] = parameters =>
            {
                var cart = this.repository.LoadCart(parameters.cartId);
                PricingSummary pricing = cart.PriceCart();
                return Response.AsJson(pricing);
            };
        }
    }
}
