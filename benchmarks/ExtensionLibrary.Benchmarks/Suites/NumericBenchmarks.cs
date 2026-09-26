using System;
using BenchmarkDotNet.Attributes;
using ExtensionLibrary.Benchmarks.Common;
using NumericExtensionLibrary;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria de benchmarks para cálculos numéricos seguros e arredondamento financeiro.
/// </summary>
public class NumericBenchmarks : BenchmarkBase
{
    private decimal _numerator;
    private decimal _zeroDivisor;
    private decimal _decimalValue;
    private decimal _part;
    private decimal _total;

    [GlobalSetup]
    public void Setup()
    {
        _numerator = 98450.75m;
        _zeroDivisor = 0m;
        _decimalValue = 1234.56789m;
        _part = 450.0m;
        _total = 1800.0m;
    }

    [Benchmark(Baseline = true, Description = "1. SafeDivide: Defensive Exception Unwinding")]
    public decimal SafeDivide_DefensiveException()
    {
        try
        {
            return _numerator / _zeroDivisor;
        }
        catch (DivideByZeroException)
        {
            return 0m;
        }
    }

    [Benchmark(Description = "1. SafeDivide: TL.ExtensionLibrary Zero-Check Guard")]
    public decimal SafeDivide_ExtensionLibrary()
    {
        return _numerator.SafeDivide(_zeroDivisor, 0m);
    }

    [Benchmark(Description = "2. RoundFinancial: BCL Math.Round Standard (Baseline)")]
    public decimal RoundFinancial_BclMathRound()
    {
        return Math.Round(_decimalValue, 2, MidpointRounding.ToEven);
    }

    [Benchmark(Description = "2. RoundFinancial: TL.ExtensionLibrary Guarded")]
    public decimal RoundFinancial_ExtensionLibrary()
    {
        return _decimalValue.RoundFinancial(2);
    }

    [Benchmark(Description = "3. Percentage: Ternary Division (Baseline)")]
    public decimal Percentage_TernaryDivision()
    {
        return _total == 0m ? 0m : (_part / _total) * 100m;
    }

    [Benchmark(Description = "3. Percentage: TL.ExtensionLibrary CalculatePercentageOf")]
    public decimal Percentage_ExtensionLibrary()
    {
        return _part.CalculatePercentageOf(_total);
    }
}
