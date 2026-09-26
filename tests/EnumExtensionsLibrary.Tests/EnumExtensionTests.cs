using System;
using System.ComponentModel;
using System.Threading.Tasks;
using EnumExtensionsLibrary;
using Xunit;

namespace EnumExtensionsLibrary.Tests;

public enum OrderState
{
    [Description("Pedido Criado")]
    [EnumDescription(101, "Código 101 - Pedido Criado")]
    Created = 1,

    [Description("Pedido Pago")]
    [EnumDescription(102, "Código 102 - Pedido Pago")]
    Paid = 2,

    Shipped = 3
}

public enum ZeroIndexedState
{
    [Description("Estado Inicial Zero")]
    Initial = 0,

    [Description("Estado Ativo")]
    Active = 1
}

public class EnumExtensionTests
{
    [Fact]
    public void GetDescription_WithZeroValueEnum_ShouldReturnDescriptionInsteadOfEmpty()
    {
        Assert.Equal("Estado Inicial Zero", ZeroIndexedState.Initial.GetDescription());
        Assert.Equal("Estado Ativo", ZeroIndexedState.Active.GetDescription());
    }

    [Fact]
    public void GetDescription_ShouldReturnAttributeValue()
    {
        Assert.Equal("Pedido Criado", OrderState.Created.GetDescription());
        Assert.Equal("Pedido Pago", OrderState.Paid.GetDescription());
    }

    [Fact]
    public void GetDescription_WithoutAttribute_ShouldFallbackToEnumName()
    {
        Assert.Equal("Shipped", OrderState.Shipped.GetDescription());
    }

    [Fact]
    public void GetDescription_ByKey_ShouldReturnKeyedDescription()
    {
        Assert.Equal("Código 101 - Pedido Criado", OrderState.Created.GetDescription(101));
        Assert.Equal("Shipped", OrderState.Shipped.GetDescription(999));
    }

    [Fact]
    public void GetDescriptionByKeyOrDefault_ShouldReturnDefaultWhenNotFound()
    {
        var result = OrderState.Created.GetDescriptionByKeyOrDefault(999, "Padrão");
        Assert.Equal("Padrão", result);
    }

    [Fact]
    public void HasKey_ShouldReturnTrueForExistingKey()
    {
        Assert.True(OrderState.Created.HasKey(101));
        Assert.False(OrderState.Created.HasKey(999));
    }

    [Fact]
    public void ToDictionary_ShouldContainAllValues()
    {
        var dict = EnumExtension.ToDictionary<OrderState>();
        Assert.Equal(3, dict.Count);
        Assert.Equal("Created", dict[1]);
        Assert.Equal("Paid", dict[2]);
        Assert.Equal("Shipped", dict[3]);
    }

    [Fact]
    public void GetEnumByDescription_ShouldReturnCorrectEnumOrThrow()
    {
        var item = EnumExtension.GetEnumByDescription<OrderState>("Pedido Criado");
        Assert.Equal(OrderState.Created, item);

        Assert.Throws<ArgumentException>(() =>
            EnumExtension.GetEnumByDescription<OrderState>("Descrição Inexistente"));
    }

    [Fact]
    public void TryParse_ShouldHandleValidAndInvalidValues()
    {
        Assert.True(EnumExtension.TryParse<OrderState>("Created", out var valid));
        Assert.Equal(OrderState.Created, valid);

        Assert.False(EnumExtension.TryParse<OrderState>("InvalidValue", out _));
    }

    [Fact]
    public void GetDescription_WhenCached_ShouldReturnFastWithoutHeapAllocation()
    {
        var status = OrderState.Created;
        _ = status.GetDescription();

        var allocatedBefore = GC.GetAllocatedBytesForCurrentThread();

        for (var i = 0; i < 1000; i++)
        {
            _ = status.GetDescription();
        }

        var allocatedAfter = GC.GetAllocatedBytesForCurrentThread();
        var totalAllocated = allocatedAfter - allocatedBefore;

        Assert.Equal(0, totalAllocated);
    }

    [Fact]
    public void GetDescription_WhenExceedingCacheCapacity_ShouldFallbackSafelyWithoutExceedingBoundedLimit()
    {
        EnumDescriptionCache<OrderState>.Clear();

        const int totalItems = 2000;
        for (var i = 1; i <= totalItems; i++)
        {
            var dynamicValue = (OrderState)i;
            var description = dynamicValue.GetDescription();
            Assert.False(string.IsNullOrEmpty(description));
        }

        Assert.True(EnumDescriptionCache<OrderState>.Count <= EnumDescriptionCache<OrderState>.MaxCapacity);
        Assert.Equal(EnumDescriptionCache<OrderState>.MaxCapacity, EnumDescriptionCache<OrderState>.Count);
    }

    [Fact]
    public void GetDescription_Untyped_WhenExceedingCacheCapacity_ShouldFallbackSafely()
    {
        EnumDescriptionCache.Clear();

        const int totalItems = 2000;
        for (var i = 1; i <= totalItems; i++)
        {
            var dynamicValue = (OrderState)i;
            var description = EnumExtension.GetDescription((Enum)dynamicValue);
            Assert.False(string.IsNullOrEmpty(description));
        }

        Assert.True(EnumDescriptionCache.Count <= EnumDescriptionCache.MaxCapacity);
        Assert.Equal(EnumDescriptionCache.MaxCapacity, EnumDescriptionCache.Count);
    }

    [Fact]
    public void GetDescription_UnderHighConcurrency_ShouldBeThreadSafeAndRespectBoundedLimit()
    {
        EnumDescriptionCache<OrderState>.Clear();

        Parallel.For(0, 5000, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 }, i =>
        {
            var enumVal = (i % 3) switch
            {
                0 => OrderState.Created,
                1 => OrderState.Paid,
                _ => (OrderState)(100 + i)
            };

            var description = enumVal.GetDescription();
            Assert.False(string.IsNullOrEmpty(description));
        });

        Assert.True(EnumDescriptionCache<OrderState>.Count <= EnumDescriptionCache<OrderState>.MaxCapacity);
    }

    [Fact]
    public void GetDescription_Untyped_UnderHighConcurrency_ShouldBeThreadSafeAndRespectBoundedLimit()
    {
        EnumDescriptionCache.Clear();

        Parallel.For(0, 5000, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 }, i =>
        {
            var enumVal = (i % 3) switch
            {
                0 => OrderState.Created,
                1 => OrderState.Paid,
                _ => (OrderState)(100 + i)
            };

            var description = EnumExtension.GetDescription((Enum)enumVal);
            Assert.False(string.IsNullOrEmpty(description));
        });

        Assert.True(EnumDescriptionCache.Count <= EnumDescriptionCache.MaxCapacity);
    }

    [Fact]
    public void EnumDescriptionCache_Clear_ShouldResetCount()
    {
        _ = OrderState.Created.GetDescription();
        Assert.True(EnumDescriptionCache<OrderState>.Count > 0);

        EnumDescriptionCache<OrderState>.Clear();
        Assert.Equal(0, EnumDescriptionCache<OrderState>.Count);
    }
}