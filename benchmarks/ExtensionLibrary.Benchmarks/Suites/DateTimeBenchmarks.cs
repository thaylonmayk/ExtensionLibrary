using System;
using BenchmarkDotNet.Attributes;
using DateTimeExtensionsLibrary;
using ExtensionLibrary.Benchmarks.Common;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria de benchmarks para manipulação de datas e cálculo vetorial de dias úteis.
/// </summary>
public class DateTimeBenchmarks : BenchmarkBase
{
    private DateTime _startDate;
    private DateTime _endDate;
    private DateTime _utcDate;

    [GlobalSetup]
    public void Setup()
    {
        _startDate = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _endDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _utcDate = DateTime.UtcNow;
    }

    [Benchmark(Baseline = true, Description = "1. BusinessDays: Iterative Day-by-Day Loop (5 Years)")]
    public int BusinessDays_IterativeDayByDay()
    {
        return CalculateBusinessDaysIterative(_startDate, _endDate);
    }

    [Benchmark(Description = "1. BusinessDays: TL.ExtensionLibrary Vectorized Formula")]
    public int BusinessDays_ExtensionLibrary()
    {
        return _startDate.BusinessDaysBetween(_endDate);
    }

    [Benchmark(Description = "2. EnsureUtc: Unconditional ToUniversalTime (Baseline)")]
    public DateTime EnsureUtc_UnconditionalConversion()
    {
        return _utcDate.ToUniversalTime();
    }

    [Benchmark(Description = "2. EnsureUtc: TL.ExtensionLibrary Defensive Kind Check")]
    public DateTime EnsureUtc_ExtensionLibrary()
    {
        return _utcDate.EnsureUtc();
    }

    [Benchmark(Description = "3. StartOfMonth: Manual DateTime Instantiation (Baseline)")]
    public DateTime StartOfMonth_Manual()
    {
        return new DateTime(_startDate.Year, _startDate.Month, 1, 0, 0, 0, 0, _startDate.Kind);
    }

    [Benchmark(Description = "3. StartOfMonth: TL.ExtensionLibrary")]
    public DateTime StartOfMonth_ExtensionLibrary()
    {
        return _startDate.StartOfMonth();
    }

    private static int CalculateBusinessDaysIterative(DateTime start, DateTime end)
    {
        if (start > end) return 0;
        int count = 0;
        var current = start.Date;
        var final = end.Date;

        while (current <= final)
        {
            if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
            {
                count++;
            }
            current = current.AddDays(1);
        }

        return count;
    }
}
