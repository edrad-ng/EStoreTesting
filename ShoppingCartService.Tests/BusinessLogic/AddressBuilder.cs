using ShoppingCartService.Models;

namespace ShoppingCartService.Tests.BusinessLogic
{
    public class AddressBuilder
    {
        private string street;
        private string country;
        private string city;

        public AddressBuilder()
        {
            street = "";
            country = "country";
            city = "city";
        }

        public AddressBuilder WithStreet(string street)
        {
            this.street = street;
            return this;
        }

        public AddressBuilder WithCountry(string country)
        {
            this.country = country;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            this.city = city;
            return this;
        }

        public Address Build() => new()
        {
            City = city,
            Country = country,
            Street = street
        };
    }
}
