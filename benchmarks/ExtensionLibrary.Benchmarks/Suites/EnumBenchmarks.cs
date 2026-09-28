using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EnumExtensionsLibrary;
using ExtensionLibrary.Benchmarks.Common;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria comparativa de resolução de atributos e descrições de Enums.
/// </summary>
public class EnumBenchmarks : BenchmarkBase
{
    private BenchmarkStatus _status;

    [GlobalSetup]
    public void Setup()
    {
        _status = BenchmarkStatus.InProgress;
        _ = _status.GetDescription();
    }

    [Benchmark(Baseline = true, Description = "1. Description: Reflection Direct Field Lookup")]
    public string Description_ReflectionDirect()
    {
        return ResolveDescriptionViaReflection(_status);
    }

    [Benchmark(Description = "1. Description: TL.ExtensionLibrary Zero-Alloc Generic Cache")]
    public string Description_ExtensionLibrary()
    {
        return _status.GetDescription();
    }

    [Benchmark(Description = "1. Description: TL.ExtensionLibrary Untyped Bounded Cache")]
    public string Description_ExtensionLibraryUntyped()
    {
        return EnumExtension.GetDescription((Enum)_status);
    }

    [Benchmark(Description = "2. MultiThread Contention: 16 Parallel Reads on Generic Cache")]
    public void Description_ParallelContentionGeneric()
    {
        Parallel.For(0, 16, static i =>
        {
            var desc = BenchmarkStatus.InProgress.GetDescription();
            GC.KeepAlive(desc);
        });
    }

    [Benchmark(Description = "2. MultiThread Contention: 16 Parallel Reads on Untyped Cache")]
    public void Description_ParallelContentionUntyped()
    {
        Parallel.For(0, 16, static i =>
        {
            var desc = EnumExtension.GetDescription((Enum)BenchmarkStatus.InProgress);
            GC.KeepAlive(desc);
        });
    }

    [Benchmark(Description = "3. Parse: Enum.Parse with TryCatch (Baseline)")]
    public BenchmarkStatus Parse_EnumParseTryCatch()
    {
        try
        {
            return (BenchmarkStatus)Enum.Parse(typeof(BenchmarkStatus), "InProgress", true);
        }
        catch
        {
            return BenchmarkStatus.Pending;
        }
    }

    [Benchmark(Description = "3. Parse: TL.ExtensionLibrary (Safe TryParse)")]
    public BenchmarkStatus Parse_ExtensionLibrary()
    {
        return EnumExtensionsLibrary.EnumExtension.ToEnum<BenchmarkStatus>("InProgress", BenchmarkStatus.Pending, true);
    }

    private static string ResolveDescriptionViaReflection(BenchmarkStatus status)
    {
        var field = typeof(BenchmarkStatus).GetField(status.ToString());
        if (field is null)
        {
            return status.ToString();
        }

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute is null ? status.ToString() : attribute.Description;
    }
}
