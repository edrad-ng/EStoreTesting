﻿using System.Collections.Generic;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
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

        [Fact]
        public void CalculateShippingCost_SameCountryStandardShippingNoItems_Return0()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>(),
                ShippingAddress = new Address { Country = "country", City = "city 2", Street = "street 2" }
            };

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

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>
                {
                    new() {Quantity = 5},
                    new() {Quantity = 3}
                },
                ShippingAddress = new Address { Country = "country", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(8 * ShippingCalculator.SameCountryRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingStandardShippingNoItems_return0()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>(),
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

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

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>
                {
                    new() {Quantity = 5},
                    new() {Quantity = 3}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(8 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityExpeditedShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Expedited,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { City = "city", Country = "country", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryExpeditedShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Expedited,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpeditedShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Expedited,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityPriorityShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Priority,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { City = "city", Country = "country", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryPriorityShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Priority,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingPriorityShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Priority,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityExpressShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Express,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { City = "city", Country = "country", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryExpressShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Express,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpressShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Express,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingPriorityShippingPremiumCustomerOneItemsQuantity1_DoNotPayShippingRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                ShippingMethod = ShippingMethod.Priority,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpeditedShippingPremiumCustomerOneItemsQuantity1_DoNotPayShippingRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                ShippingMethod = ShippingMethod.Expedited,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpressShippingVIPCustomerOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Express,
                Items = new List<Item>
                {
                    new() {Quantity = 1}
                },
                ShippingAddress = new Address { Country = "country 2", City = "city 2", Street = "street 2" }
            };

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.5, result);
        }
    }
}