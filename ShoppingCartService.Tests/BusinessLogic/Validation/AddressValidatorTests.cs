using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartService.Tests.BusinessLogic.Validation
{
    public class AddressValidatorTests
    {
        [Fact]
        public void IsValid_Returns_False_When_Address_IsNull()
        {
            // arrange
            Address address = null;
            var validator = new AddressValidator();

            // act
            var isValid = validator.IsValid(address);

            // assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_Returns_False_When_Country_IsEmpty()
        {
            // arrange
            var address = new Address { City = "City", Street = "street" };
            var validator = new AddressValidator();

            // act
            var isValid = validator.IsValid(address);

            // assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_Returns_False_When_City_IsEmpty()
        {
            // arrange
            var address = new Address { Country = "Country", Street = "street" };
            var validator = new AddressValidator();

            // act
            var isValid = validator.IsValid(address);

            // assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_Returns_False_When_Street_IsEmpty()
        {
            // arrange
            var address = new Address { Country = "Country", City = "city" };
            var validator = new AddressValidator();

            // act
            var isValid = validator.IsValid(address);

            // assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_Returns_True_When_FieldsFilled()
        {
            // arrange
            var address = new Address { Country = "Country", City = "city", Street="street" };
            var validator = new AddressValidator();

            // act
            var isValid = validator.IsValid(address);

            // assert
            Assert.True(isValid);
        }
    }
}
