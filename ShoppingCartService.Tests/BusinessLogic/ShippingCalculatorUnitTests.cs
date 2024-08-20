using System.Collections.Generic;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.Tests.BusinessLogic;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class ShippingCalculatorUnitTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void CalculateShippingCost_SameCityStandardShippingItems_ReturnNumberOfItems(uint numberOfItems)
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>() { new Item { Quantity = numberOfItems } },
                ShippingAddress = new Address { City = "city", Country = "country", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(numberOfItems, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityStandardShippingTwoItems_ReturnSumOfItemsQuantity()
        {
            var address = new Address { Country = "country", City = "city", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>
                {
                    new() {Quantity = 5},
                    new() {Quantity = 3}
                },
                ShippingAddress = new Address { Country = "country", City = "city", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(8 * ShippingCalculator.SameCityRate, result);
        }

        [InlineData("country", 0)]
        [InlineData("country", 1)]
        [InlineData("country", 5)]
        [Theory]
        public void CalculateShippingCost_SameCountryStandardShippingItemsQuantity_ReturnQuantityTimesRate(string country, uint quantity)
        {
            var address = new Address { Country = country, City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { quantity });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Standard, items, new Address { Country = country, City = "city 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void CalculateShippingCost_SameCountryStandardShippingItemsQuantity_ReturnItemsQuantityTimesRate(uint itemsQuantity)
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>
                {
                    new() {Quantity = itemsQuantity}
                },
                ShippingAddress = new Address { Country = "country", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(itemsQuantity * ShippingCalculator.SameCountryRate, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryStandardShippingTwoItems_ReturnSumOfItemsQuantityTimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 5,3 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Standard, items, new Address { Country = "country", City = "city 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(8 * ShippingCalculator.SameCountryRate, result);
        }

        [InlineData("country 1", "country 2", 0)]
        [InlineData("country 1", "country 2", 1)]
        [InlineData("country 1", "country 2", 5)]
        [Theory]
        public void CalculateShippingCost_InternationalShippingStandardShippingItemsQuantity_Return5QuantitytimesRate(string firstCountry, string secondCountry, uint quantity)
        {
            var address = new Address { Country = firstCountry, City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { quantity });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Standard, items, new Address { Country = secondCountry, City = "city 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void CalculateShippingCost_InternationalShippingStandardShippingItemsQuantity_returnItemsQuantityTimesRate(uint itemsQuantity)
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>
                {
                    new() {Quantity = itemsQuantity}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(itemsQuantity * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingStandardShippingTwoItems_ReturnSumOfItemsQuantity()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 5,3 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Standard, items, new Address { Country = "country 2", City = "city 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(8 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityExpeditedShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Expedited, items, new Address { City = "city", Country = "country", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryExpeditedShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Expedited, items, new Address { Country = "country", City = "city 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpeditedShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Expedited, items, new Address { Country = "country 2", City = "city 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityPriorityShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Priority, items, new Address { Country = "country", City = "city", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryPriorityShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Priority, items, new Address { Country = "country", City = "city 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingPriorityShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Priority, items, new Address { City = "city 2", Country = "country 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityExpressShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Express, items, new Address { City = "city", Country = "country", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryExpressShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Express, items, new Address { City = "city 2", Country = "country", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpressShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Express, items, new Address { City = "city 2", Country = "country 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingPriorityShippingPremiumCustomerOneItemsQuantity1_DoNotPayShippingRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Premium, ShippingMethod.Priority, items, new Address { City = "city 2", Country = "country 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpeditedShippingPremiumCustomerOneItemsQuantity1_DoNotPayShippingRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Premium, ShippingMethod.Expedited, items, new Address { City = "city 2", Country = "country 2", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpressShippingVIPCustomerOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var items = CreateItems(new uint[] { 1 });
            var cart = CreateCart(CustomerType.Standard, ShippingMethod.Express, items, new Address { City="city 2", Country="country 2", Street="street 2"});

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.5, result);
        }

        private static Cart CreateCart(CustomerType customerType, ShippingMethod shippingMethod, IEnumerable<Item> items, Address address)
        {
            return new CartBuilder()
                        .WithItems(items)
                        .WithShippingMethod(shippingMethod)
                        .WithCustomerType(customerType)
                        .WithShippingAddress(address)
                        .Build();
        }

        private static IEnumerable<Item> CreateItems(IEnumerable<uint> quantities)
        {
            foreach (var q in quantities)
            {
                yield return new ItemBuilder()
                                    .WithQuantity(q)
                                    .Build();
            }
        }
    }
}