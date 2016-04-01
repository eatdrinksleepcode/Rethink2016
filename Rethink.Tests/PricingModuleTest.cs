using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;

namespace Rethink.Tests
{
    [TestFixture]
    public class PricingModuleTest
    {
        [Test]
        public void GetCartPricing()
        {
            StaticConfiguration.DisableErrorTraces = false;
            var browser = new Browser(new DefaultNancyBootstrapper());

            browser.Delete("/catalog/products");
            browser.Put("/catalog/products/1", c => c.Body("1"));
            browser.Put("/catalog/products/1", c => c.Body("1.5"));

            var response = browser.Post("/carts");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var cartId = response.Body.AsString();
            browser.Put("/carts/" + cartId + "/products/1", c => c.Body("1"));
            browser.Put("/carts/" + cartId + "/products/42", c => c.Body("7"));
            var result = browser.Get("/carts/" + cartId + "/pricing", c => c.Accept(new MediaRange("application/json")));

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var body = result.Body.DeserializeJson<PricingSummary>();
            Assert.That(body.ProductPrices.Count, Is.EqualTo(2));
            Assert.That(body.SubTotal, Is.EqualTo("11.5"));
            Assert.That(body.Taxes, Is.EqualTo("1.15"));
            Assert.That(body.Total, Is.EqualTo("12.65"));
        }
    }
}
