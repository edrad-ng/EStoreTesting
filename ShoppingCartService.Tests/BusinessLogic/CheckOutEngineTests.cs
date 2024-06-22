using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartService.Tests.BusinessLogic
{
    public class CheckOutEngineTests
    {
        [Fact]
        public void CalculateTotals_ThrowsException_When_ShippingCalculator_NotProvided()
        {
            // arrange
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Cart, ShoppingCartDto>());
            var engine = new CheckOutEngine(null, new Mapper(config));

            // act & assert
            Assert.Throws<NullReferenceException>(() => engine.CalculateTotals(new Cart()));
        }

        [Fact]
        public void CalculateTotals_Total_Is_90Percent_For_PremiumCustomer()
        {
            // arrange 
            var shippingCalculator = new ShippingCalculator();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<ItemDto, Item>();
                cfg.CreateMap<CreateCartDto, Cart>()
                    .ForMember(dest => dest.CustomerId,
                        opt => opt.MapFrom(src => src.Customer.Id))
                    .ForMember(dest => dest.CustomerType,
                        opt => opt.MapFrom(src => src.Customer.CustomerType))
                    .ForMember(dest => dest.ShippingAddress,
                    opt => opt.MapFrom(src => src.Customer.Address));

                cfg.CreateMap<Item, ItemDto>();
                cfg.CreateMap<Cart, ShoppingCartDto>();
            });
            var mapper = config.CreateMapper();
            var engine = new CheckOutEngine(shippingCalculator, mapper);
            var cart = new Cart
            {
                CustomerType = Models.CustomerType.Premium,
                ShippingMethod = Models.ShippingMethod.Expedited,
                ShippingAddress = new Models.Address { City = "Roissy-en-Brie", Country = "France", Street = "22 avenue du Bois" },
                Items = new List<Item>() { new() { Quantity = 2 }, new() { Quantity = 3 } }
            };

            var shippingCost = ShippingCalculator.InternationalShippingRate * 5;
            var total = cart.Items.Sum(item => item.Price * item.Quantity) + shippingCost;
            var totalForPremium = total * 90 / 100.0;

            // act
            var checkoutDto = engine.CalculateTotals(cart);

            // assert
            Assert.True(checkoutDto.CustomerDiscount.Equals(10.0));
            Assert.True(checkoutDto.Total.Equals(totalForPremium));

        }
    }
}
