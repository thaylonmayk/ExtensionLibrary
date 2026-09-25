using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using ExtensionLibrary.Benchmarks.Common;
using QueryableExtensionsLibrary;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria de testes de performance para consultas IQueryable e paginação Keyset.
/// </summary>
public class QueryableBenchmarks : BenchmarkBase
{
    private List<BenchmarkUser> _sourceData = null!;
    private IQueryable<BenchmarkUser> _queryable = null!;
    private PropertyInfo _sortProperty = null!;
    private const int DatasetSize = 10000;
    private const int DeepPageOffset = 9500;
    private const int PageSize = 20;

    [GlobalSetup]
    public void Setup()
    {
        _sourceData = GenerateUsers(DatasetSize);
        _queryable = _sourceData.AsQueryable();
        _sortProperty = typeof(BenchmarkUser).GetProperty(nameof(BenchmarkUser.CreatedAt))!;
    }

    [Benchmark(Baseline = true, Description = "1. Paging: Deep Offset Skip/Take (O(N))")]
    public List<BenchmarkUser> DeepPaging_OffsetSkipTake()
    {
        return _queryable.Skip(DeepPageOffset).Take(PageSize).ToList();
    }

    [Benchmark(Description = "1. Paging: Keyset Cursor Seek (O(1))")]
    public KeysetPagedList<BenchmarkUser, int> DeepPaging_KeysetSeek()
    {
        return _queryable.ToKeysetPagedList(user => user.Id, cursor: DeepPageOffset, pageSize: PageSize);
    }

    [Benchmark(Description = "2. Order: Uncached Reflection OrderBy (Baseline)")]
    public List<BenchmarkUser> DynamicOrder_UncachedReflection()
    {
        return OrderByUncachedReflection(_queryable, _sortProperty).Take(PageSize).ToList();
    }

    [Benchmark(Description = "2. Order: TL.ExtensionLibrary Cached Expression Tree")]
    public List<BenchmarkUser> DynamicOrder_ExtensionLibrary()
    {
        return _queryable.Order(nameof(BenchmarkUser.CreatedAt), ascending: true).Take(PageSize).ToList();
    }

    private static List<BenchmarkUser> GenerateUsers(int count)
    {
        var roles = new[] { "Admin", "User", "Manager", "Auditor", "Guest" };
        var list = new List<BenchmarkUser>(count);
        var baseDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        for (int i = 0; i < count; i++)
        {
            list.Add(new BenchmarkUser
            {
                Id = i + 1,
                Name = $"User_{i + 1}",
                Email = $"user_{i + 1}@enterprise.org",
                Role = roles[i % roles.Length],
                CreatedAt = baseDate.AddMinutes(i)
            });
        }
        return list;
    }

    private static IQueryable<BenchmarkUser> OrderByUncachedReflection(IQueryable<BenchmarkUser> source, PropertyInfo property)
    {
        return source.OrderBy(user => property.GetValue(user, null));
    }
}
