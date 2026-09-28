using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CollectionExtensionsLibrary;
using ExtensionLibrary.Benchmarks.Common;
using MoreLinq;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria comparativa de algoritmos de coleções entre BCL, TL.ExtensionLibrary e MoreLINQ.
/// </summary>
public class CollectionBenchmarks : BenchmarkBase
{
    private List<BenchmarkUser> _userList = null!;
    private const int ItemCount = 1000;

    [GlobalSetup]
    public void Setup()
    {
        _userList = GenerateUsers(ItemCount);
    }

    [Benchmark(Baseline = true, Description = "1. IsNullOrEmpty: LINQ !Any()")]
    public bool CheckEmpty_LinqAny()
    {
        return _userList is null || !_userList.Any();
    }

    [Benchmark(Description = "1. IsNullOrEmpty: TL.ExtensionLibrary O(1) Check")]
    public bool CheckEmpty_ExtensionLibrary()
    {
        return _userList.IsNullOrEmpty();
    }

    [Benchmark(Description = "2. DistinctBy: LINQ GroupBy First (Baseline)")]
    public List<BenchmarkUser> DistinctBy_LinqGroupBy()
    {
        return _userList.GroupBy(user => user.Role).Select(group => group.First()).ToList();
    }

    [Benchmark(Description = "2. DistinctBy: TL.ExtensionLibrary (.NET 8 Native)")]
    public List<BenchmarkUser> DistinctBy_ExtensionLibrary()
    {
        return CollectionExtensionsLibrary.CollectionExtensions.DistinctBy(_userList, user => user.Role).ToList();
    }

    [Benchmark(Description = "2. DistinctBy: MoreLINQ")]
    public List<BenchmarkUser> DistinctBy_MoreLinq()
    {
        return MoreEnumerable.DistinctBy(_userList, user => user.Role).ToList();
    }

    [Benchmark(Description = "3. Shuffle: LINQ OrderBy Guid (Baseline)")]
    public List<BenchmarkUser> Shuffle_LinqOrderByGuid()
    {
        return _userList.OrderBy(_ => Guid.NewGuid()).ToList();
    }

    [Benchmark(Description = "3. Shuffle: TL.ExtensionLibrary Fisher-Yates")]
    public List<BenchmarkUser> Shuffle_ExtensionLibrary()
    {
        return CollectionExtensionsLibrary.CollectionExtensions.Shuffle(_userList).ToList();
    }

    [Benchmark(Description = "3. Shuffle: MoreLINQ")]
    public List<BenchmarkUser> Shuffle_MoreLinq()
    {
        return MoreEnumerable.Shuffle(_userList).ToList();
    }

    [Benchmark(Description = "4. Chunk: Skip/Take Nested Partitioning (Baseline)")]
    public List<List<BenchmarkUser>> Chunk_SkipTake()
    {
        return PartitionViaSkipTake(_userList, 50);
    }

    [Benchmark(Description = "4. Chunk: TL.ExtensionLibrary (.NET 8 Chunk)")]
    public List<BenchmarkUser[]> Chunk_ExtensionLibrary()
    {
        return _userList.Chunk(50).ToList();
    }

    [Benchmark(Description = "5. Partition: Double LINQ Where (Baseline)")]
    public (List<BenchmarkUser> Matches, List<BenchmarkUser> NonMatches) Partition_DoubleWhere()
    {
        var matches = _userList.Where(u => u.Id % 2 == 0).ToList();
        var nonMatches = _userList.Where(u => u.Id % 2 != 0).ToList();
        return (matches, nonMatches);
    }

    [Benchmark(Description = "5. Partition: TL.ExtensionLibrary Single Pass O(N)")]
    public (List<BenchmarkUser> Matches, List<BenchmarkUser> NonMatches) Partition_ExtensionLibrary()
    {
        return CollectionExtensionsLibrary.CollectionExtensions.Partition(_userList, u => u.Id % 2 == 0);
    }

    private static List<BenchmarkUser> GenerateUsers(int count)
    {
        var roles = new[] { "Admin", "User", "Manager", "Auditor", "Guest" };
        var list = new List<BenchmarkUser>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(new BenchmarkUser
            {
                Id = i + 1,
                Name = $"User_{i + 1}",
                Email = $"user_{i + 1}@enterprise.org",
                Role = roles[i % roles.Length],
                CreatedAt = DateTime.UtcNow.AddMinutes(-i)
            });
        }
        return list;
    }

    private static List<List<BenchmarkUser>> PartitionViaSkipTake(List<BenchmarkUser> source, int chunkSize)
    {
        var result = new List<List<BenchmarkUser>>();
        int total = source.Count;
        for (int i = 0; i < total; i += chunkSize)
        {
            result.Add(source.Skip(i).Take(chunkSize).ToList());
        }
        return result;
    }
}
