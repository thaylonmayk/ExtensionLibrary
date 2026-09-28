using System;
using System.ComponentModel;
using EnumExtensionsLibrary;
using Xunit;

namespace EnumExtensionsLibrary.Tests
{
    public class EnumExtensionsParseTests
    {
        public enum OrderStatus
        {
            [Description("Pending Approval")]
            Pending = 1,

            [Description("Payment Approved")]
            Approved = 2,

            [Description("Order Shipped")]
            Shipped = 3,

            [Description("Order Cancelled")]
            Cancelled = 4
        }

        [Theory]
        [InlineData("Approved", OrderStatus.Approved)]
        [InlineData("approved", OrderStatus.Approved)]
        [InlineData("Pending", OrderStatus.Pending)]
        [InlineData("InvalidValue", OrderStatus.Cancelled)]
        [InlineData("", OrderStatus.Cancelled)]
        [InlineData(null, OrderStatus.Cancelled)]
        public void ToEnum_ShouldParseSafelyWithDefault(string input, OrderStatus expected)
        {
            var result = input.ToEnum(defaultValue: OrderStatus.Cancelled, ignoreCase: true);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Shipped", OrderStatus.Shipped)]
        [InlineData("shipped", OrderStatus.Shipped)]
        [InlineData("NonExistent", null)]
        [InlineData(null, null)]
        public void ToEnumOrNull_ShouldReturnNullOnFailure(string input, OrderStatus? expected)
        {
            var result = input.ToEnumOrNull<OrderStatus>(ignoreCase: true);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToEnum_FromInteger_ShouldReturnParsedOrFallback()
        {
            Assert.Equal(OrderStatus.Approved, 2.ToEnum(OrderStatus.Cancelled));
            Assert.Equal(OrderStatus.Cancelled, 999.ToEnum(OrderStatus.Cancelled));
        }

        [Fact]
        public void GetCachedDescriptions_ShouldReturnAllDescriptionsFromCache()
        {
            var descriptions1 = EnumExtension.GetCachedDescriptions<OrderStatus>();
            var descriptions2 = EnumExtension.GetCachedDescriptions<OrderStatus>();

            Assert.Same(descriptions1, descriptions2);
            Assert.Equal(4, descriptions1.Count);
            Assert.Equal("Pending Approval", descriptions1[OrderStatus.Pending]);
            Assert.Equal("Payment Approved", descriptions1[OrderStatus.Approved]);
            Assert.Equal("Order Shipped", descriptions1[OrderStatus.Shipped]);
            Assert.Equal("Order Cancelled", descriptions1[OrderStatus.Cancelled]);
        }
    }
}
