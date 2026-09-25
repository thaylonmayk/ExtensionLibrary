using System;
using System.Collections.Generic;
using System.Linq;
using QueryableExtensionsLibrary;
using Xunit;

namespace QueryableExtensionsLibrary.Tests;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class QueryableExtensionTests
{
    [Fact]
    public void Filter_ShouldFilterByValidProperty()
    {
        var query = new List<Product>
        {
            new() { Id = 1, Name = "Notebook", Price = 3000 },
            new() { Id = 2, Name = "Mouse", Price = 100 }
        }.AsQueryable();

        var result = query.Filter("Price", ">=", 1000).ToList();

        Assert.Single(result);
        Assert.Equal("Notebook", result[0].Name);
    }

    [Fact]
    public void Filter_ShouldThrowOnInvalidProperty()
    {
        var query = new List<Product> { new() { Id = 1, Name = "Item" } }.AsQueryable();

        Assert.Throws<ArgumentException>(() => query.Filter("PropriedadeInexistente", "Valor"));
    }

    [Fact]
    public void ToKeysetPagedList_FirstPage_ShouldReturnPageAndCursors()
    {
        var query = Enumerable.Range(1, 30)
            .Select(i => new Product { Id = i, Name = $"Product {i}", Price = i * 10 })
            .AsQueryable();

        var page = query.ToKeysetPagedList(x => x.Id, pageSize: 10);

        Assert.Equal(10, page.Items.Count);
        Assert.Equal(1, page.Items[0].Id);
        Assert.Equal(10, page.Items[9].Id);
        Assert.True(page.HasNextPage);
        Assert.False(page.HasPreviousPage);
        Assert.Equal(10, page.NextCursor);
    }

    [Fact]
    public void ToKeysetPagedList_SeekForward_ShouldReturnNextPage()
    {
        var query = Enumerable.Range(1, 30)
            .Select(i => new Product { Id = i, Name = $"Product {i}", Price = i * 10 })
            .AsQueryable();

        var page = query.ToKeysetPagedList(x => x.Id, cursor: 10, pageSize: 10, SeekDirection.Forward);

        Assert.Equal(10, page.Items.Count);
        Assert.Equal(11, page.Items[0].Id);
        Assert.Equal(20, page.Items[9].Id);
        Assert.True(page.HasNextPage);
        Assert.True(page.HasPreviousPage);
    }

    [Fact]
    public void ToKeysetPagedList_SeekBackward_ShouldReturnPreviousPageInOriginalOrder()
    {
        var query = Enumerable.Range(1, 30)
            .Select(i => new Product { Id = i, Name = $"Product {i}", Price = i * 10 })
            .AsQueryable();

        var page = query.ToKeysetPagedList(x => x.Id, cursor: 21, pageSize: 10, SeekDirection.Backward);

        Assert.Equal(10, page.Items.Count);
        Assert.Equal(11, page.Items[0].Id);
        Assert.Equal(20, page.Items[9].Id);
    }

    [Fact]
    public void ToKeysetPagedList_InvalidPageSize_ShouldThrowArgumentOutOfRangeException()
    {
        var query = new List<Product>().AsQueryable();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            query.ToKeysetPagedList(x => x.Id, pageSize: -1));
    }

    [Fact]
    public void Order_ShouldSortAscendingAndDescendingDynamically()
    {
        var query = new List<Product>
        {
            new() { Id = 3, Name = "Monitor", Price = 800 },
            new() { Id = 1, Name = "Notebook", Price = 3000 },
            new() { Id = 2, Name = "Mouse", Price = 100 }
        }.AsQueryable();

        var ascending = query.Order("Price", ascending: true).ToList();
        Assert.Equal(100, ascending[0].Price);
        Assert.Equal(800, ascending[1].Price);
        Assert.Equal(3000, ascending[2].Price);

        var descending = query.Order("Price", ascending: false).ToList();
        Assert.Equal(3000, descending[0].Price);
        Assert.Equal(800, descending[1].Price);
        Assert.Equal(100, descending[2].Price);
    }
}