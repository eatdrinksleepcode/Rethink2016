using FakeItEasy;
using NUnit.Framework;

namespace Rethink.Tests
{
    [TestFixtureAttribute]
    public class PricingCalculatorTest
    {

        [Test]
        public void MultipleProducts_Mocks()
        {
            // Arrange
            var cart = A.Fake<ICart>();
            A.CallTo(() => cart.Products).Returns(new [] { 
                new ProductToPurchase { ProductId = 1, Quantity = 1 },
                new ProductToPurchase { ProductId = 42, Quantity = 7 },
            });

            IProductCatalog mockProductCatalog = A.Fake<IProductCatalog>();
            A.CallTo(() => mockProductCatalog.LookupPrice(1)).Returns(1m);
            A.CallTo(() => mockProductCatalog.LookupPrice(42)).Returns(1.5m);

            ITaxCalculator mockTaxCalculator = A.Fake<ITaxCalculator>();
            A.CallTo(() => mockTaxCalculator.CalculateTaxes(11.5m)).Returns(1.15m);

            // Act
            var calculator = new PricingCalculator(mockProductCatalog, mockTaxCalculator);
            PricingSummary price = calculator.PriceCart(cart);

            // Assert
            Assert.That(price.ProductPrices.Count, Is.EqualTo(2));
            Assert.That(price.SubTotal, Is.EqualTo(11.5m));
            Assert.That(price.Taxes, Is.EqualTo(1.15m));
            Assert.That(price.Total, Is.EqualTo(12.65m));
        }

        [Test]
        public void MissingPrice_Mocks()
        {
            var cart = A.Fake<ICart>();
            A.CallTo(() => cart.Products).Returns(new[] {
                new ProductToPurchase { ProductId = 1, Quantity = 1 },
                new ProductToPurchase { ProductId = 42, Quantity = 7 },
            });

            IProductCatalog mockCatalog = A.Fake<IProductCatalog>();
            A.CallTo(() => mockCatalog.LookupPrice(1)).Returns(1m);
            A.CallTo(() => mockCatalog.LookupPrice(42)).Returns(-1m);

            ITaxCalculator mockTaxCalculator = A.Fake<ITaxCalculator>();
            A.CallTo(() => mockTaxCalculator.CalculateTaxes(1m)).Returns(0.1m);

            var calculator = new PricingCalculator(mockCatalog, mockTaxCalculator);
            PricingSummary price = calculator.PriceCart(cart);

            Assert.That(price.ProductPrices.Count, Is.EqualTo(1));
            Assert.That(price.SubTotal, Is.EqualTo(1m));
            Assert.That(price.Taxes, Is.EqualTo(0.1m));
            Assert.That(price.Total, Is.EqualTo(1.1m));
        }

        private class PercentageTaxCalculatorStub : ITaxCalculator
        {
            private readonly decimal percentage;

            public PercentageTaxCalculatorStub(decimal percentage)
            {
                this.percentage = percentage;
            }

            public decimal CalculateTaxes(decimal subTotal)
            {
                return subTotal * percentage;
            }
        }

        [Test]
        public void MultipleProducts()
        {
            var cart = new Cart();
            cart.IncludeProduct(1, 1);
            cart.IncludeProduct(42, 7);

            var catalog = new ProductCatalog();
            catalog.SetPrice(1, 1m);
            catalog.SetPrice(42, 1.5m);

            var taxCalculator = new PercentageTaxCalculatorStub(0.1m);

            var calculator = new PricingCalculator(catalog, taxCalculator);
            PricingSummary price = calculator.PriceCart(cart);

            Assert.That(price.ProductPrices.Count, Is.EqualTo(2));
            Assert.That(price.SubTotal, Is.EqualTo(11.5m));
            Assert.That(price.Taxes, Is.EqualTo(1.15m));
            Assert.That(price.Total, Is.EqualTo(12.65m));
        }

        [Test]
        public void MissingPrice()
        {
            var cart = new Cart();
            cart.IncludeProduct(1, 1);
            cart.IncludeProduct(42, 7);

            var catalog = new ProductCatalog();
            catalog.SetPrice(1, 1m);

            var taxCalculator = new PercentageTaxCalculatorStub(0.1m);

            var calculator = new PricingCalculator(catalog, taxCalculator);
            PricingSummary price = calculator.PriceCart(cart);

            Assert.That(price.ProductPrices.Count, Is.EqualTo(1));
            Assert.That(price.SubTotal, Is.EqualTo(1m));
            Assert.That(price.Taxes, Is.EqualTo(0.1m));
            Assert.That(price.Total, Is.EqualTo(1.1m));
        }

        [Test]
        public void MultipleProducts2()
        {
            var cart = new Cart();
            cart.IncludeProduct(1, 1);
            cart.IncludeProduct(42, 7);

            var catalog = new ProductCatalog();
            catalog.SetPrice(1, 1m);
            catalog.SetPrice(42, 1.5m);

            var taxCalculator = new PercentageTaxCalculatorStub(0.1m);

            var calculator = new PricingCalculator(catalog, taxCalculator);
            PricingSummary price = calculator.PriceCart2(cart);

            Assert.That(price.ProductPrices.Count, Is.EqualTo(2));
            Assert.That(price.SubTotal, Is.EqualTo(11.5m));
            Assert.That(price.Taxes, Is.EqualTo(1.15m));
            Assert.That(price.Total, Is.EqualTo(12.65m));
        }

        [Test]
        public void MissingPrice2()
        {
            var cart = new Cart();
            cart.IncludeProduct(1, 1);
            cart.IncludeProduct(42, 7);

            var catalog = new ProductCatalog();
            catalog.SetPrice(1, 1m);

            var taxCalculator = new PercentageTaxCalculatorStub(0.1m);

            var calculator = new PricingCalculator(catalog, taxCalculator);
            PricingSummary price = calculator.PriceCart2(cart);

            Assert.That(price.ProductPrices.Count, Is.EqualTo(1));
            Assert.That(price.SubTotal, Is.EqualTo(1m));
            Assert.That(price.Taxes, Is.EqualTo(0.1m));
            Assert.That(price.Total, Is.EqualTo(1.1m));
        }
    }
}
