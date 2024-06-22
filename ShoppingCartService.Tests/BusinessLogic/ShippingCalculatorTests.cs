using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using System.Collections.Generic;
using Xunit;

namespace ShoppingCartService.Tests.BusinessLogic
{
    public class ShippingCalculatorTests
    {
        [Fact]
        public void ShippingCost_Equals_BaseCost_When_CustomerPremium_And_ShippingMethodPriority()
        {
            // arrange
            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Premium,
                ShippingMethod = Models.ShippingMethod.Priority,
                ShippingAddress = new Models.Address { City = "Roissy-en-Brie", Country = "France", Street = "22 avenue du Bois" },
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var baseCost = ShippingCalculator.InternationalShippingRate * 5;

            var calculator = new ShippingCalculator();

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(baseCost));
        }

        [Fact]
        public void ShippingCost_Equals_BaseCost_When_CustomerPremium_And_ShippingMethodExpedited()
        {
            // arrange
            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Premium,
                ShippingMethod = Models.ShippingMethod.Expedited,
                ShippingAddress = new Models.Address { City = "Roissy-en-Brie", Country = "France", Street = "22 avenue du Bois" },
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var baseCost = ShippingCalculator.InternationalShippingRate * 5;

            var calculator = new ShippingCalculator();

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(baseCost));
        }

        [Fact]
        public void ShippingCost_BasedOnShippingMethod_When_CustomerPremium_And_ShippingMethod_Express()
        {
            // arrange
            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Premium,
                ShippingMethod = Models.ShippingMethod.Express,
                ShippingAddress = new Models.Address { City = "Roissy-en-Brie", Country = "France", Street = "22 avenue du Bois" },
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var travelCost = ShippingCalculator.InternationalShippingRate * 5 * 2.5;

            var calculator = new ShippingCalculator();

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(travelCost));
        }

        [Fact]
        public void ShippingCost_BasedOnShippingMethod_When_CustomerPremium_And_ShippingMethod_Standard()
        {
            // arrange
            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Premium,
                ShippingMethod = Models.ShippingMethod.Standard,
                ShippingAddress = new Models.Address { City = "Roissy-en-Brie", Country = "France", Street = "22 avenue du Bois" },
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var travelCost = ShippingCalculator.InternationalShippingRate * 5 * 1.0;

            var calculator = new ShippingCalculator();

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(travelCost));
        }

        [Fact]
        public void ShippingCost_BasedOnShippingMethod_When_CustomerNotPremium_ShippingMethodPriority()
        {
            // arrange
            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Standard,
                ShippingMethod = Models.ShippingMethod.Priority,
                ShippingAddress = new Models.Address { City = "Roissy-en-Brie", Country = "France", Street = "22 avenue du Bois" },
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var travelCost = ShippingCalculator.InternationalShippingRate * 5 * 2.0;

            var calculator = new ShippingCalculator();

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(travelCost));
        }

        [Fact]
        public void ShippingCost_BasedOnShippingMethod_When_CustomerNotPremium_ShippingMethodExpedited()
        {
            // arrange
            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Standard,
                ShippingMethod = Models.ShippingMethod.Expedited,
                ShippingAddress = new Models.Address { City = "Roissy-en-Brie", Country = "France", Street = "22 avenue du Bois" },
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var travelCost = ShippingCalculator.InternationalShippingRate * 5 * 1.2;

            var calculator = new ShippingCalculator();

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(travelCost));
        }

        [Fact]
        public void ShippingCost_BasedOnShippingMethod_When_CustomerNotPremium_ShippingMethodExpress_SameCountry()
        {
            // arrange
            var address = new Models.Address
            {
                City = "Roissy-en-Brie",
                Country = "France",
                Street = "22 avenue du Bois"
            };

            var shippingAddress = new Models.Address
            {
                City = "Pontault-Combault",
                Country = "France",
                Street = "40 rue des Berchères"
            };

            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Standard,
                ShippingMethod = Models.ShippingMethod.Express,
                ShippingAddress = shippingAddress,
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var travelCost = ShippingCalculator.SameCountryRate * 5 * 2.5;

            var calculator = new ShippingCalculator(address);

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(travelCost));
        }

        [Fact]
        public void ShippingCost_BasedOnShippingMethod_When_CustomerNotPremium_ShippingMethodExpress_SameCity()
        {
            // arrange
            var address = new Models.Address
            {
                City = "Roissy-en-Brie",
                Country = "France",
                Street = "22 avenue du Bois"
            };

            var shippingAddress = new Models.Address
            {
                City = "Roissy-en-Brie",
                Country = "France",
                Street = "40 avenue Ancel de Garlande"
            };

            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Standard,
                ShippingMethod = Models.ShippingMethod.Express,
                ShippingAddress = shippingAddress,
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };
            var travelCost = ShippingCalculator.SameCityRate * 5 * 2.5;

            var calculator = new ShippingCalculator(address);

            // act
            var cost = calculator.CalculateShippingCost(cart);

            // assert
            Assert.True(cost.Equals(travelCost));
        }
    }
}
