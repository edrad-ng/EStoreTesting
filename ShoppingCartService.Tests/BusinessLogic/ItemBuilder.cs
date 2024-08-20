using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartService.Tests.BusinessLogic
{
    public class ItemBuilder
    {
        private double price;
        private string productId;
        private string productName;
        private uint quantity;

        public ItemBuilder()
        {
            price = 0.0;
            productId = "";
            productName = "";
            quantity = 0;
        }

        public ItemBuilder WithPrice(double price)
        {
            this.price = price;
            return this;
        }

        public ItemBuilder WithProductId(string productId)
        {
            this.productId = productId;
            return this;
        }

        public ItemBuilder WithProductName(string productName)
        {
            this.productName = productName;
            return this;
        }

        public ItemBuilder WithQuantity(uint quantity)
        {
            this.quantity = quantity;
            return this;
        }

        public Item Build() => new()
        {
            Price = price,
            ProductId = productId,
            ProductName = productName,
            Quantity = quantity
        };
    }
}
