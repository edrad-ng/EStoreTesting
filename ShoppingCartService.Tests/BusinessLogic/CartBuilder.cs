using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using System.Collections.Generic;

namespace ShoppingCartService.Tests.BusinessLogic
{
    public class CartBuilder
    {
        private string customerId;
        private CustomerType customerTye;
        private string id;
        private List<Item> items;
        private Address shippingAddress;
        private ShippingMethod shippingMethod;

        public CartBuilder()
        {
            customerId = "";
            customerTye = new CustomerType();
            id = "";
            items = new List<Item>();
            shippingAddress = new Address();
            shippingMethod = new ShippingMethod();
        }

        public CartBuilder WithCustomerId(string customerId)
        {
            this.customerId = customerId;
            return this;
        }

        public CartBuilder WithCustomerType(CustomerType customerTye)
        {
            this.customerTye = customerTye;
            return this;
        }

        public CartBuilder WithItems(IEnumerable<Item> items)
        {
            this.items.AddRange(items);
            return this;
        }

        public CartBuilder WithShippingAddress(Address shippingAddress)
        {
            this.shippingAddress = shippingAddress;
            return this;
        }

        public CartBuilder WithShippingMethod(ShippingMethod shippingMethod)
        {
            this.shippingMethod = shippingMethod;
            return this;
        }

        public Cart Build() => new()
        {
            CustomerId = customerId,
            CustomerType = customerTye,
            Id = id,
            Items = items,
            ShippingAddress = shippingAddress,
            ShippingMethod = shippingMethod
        };
    }
}
