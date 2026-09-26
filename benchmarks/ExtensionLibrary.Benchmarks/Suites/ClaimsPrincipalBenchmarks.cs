using System;
using System.Security.Claims;
using BenchmarkDotNet.Attributes;
using ClaimsPrincipalExtensionsLibrary;
using ExtensionLibrary.Benchmarks.Common;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria de benchmarks para extração e tipagem segura de claims de identidade.
/// </summary>
public class ClaimsPrincipalBenchmarks : BenchmarkBase
{
    private ClaimsPrincipal _principalWithGuid = null!;
    private ClaimsPrincipal _principalWithInt = null!;
    private const string ExpectedGuidString = "6f81f185-985f-4a0f-90e9-bfaea8123456";
    private const string ExpectedIntString = "987452";

    [GlobalSetup]
    public void Setup()
    {
        _principalWithGuid = CreatePrincipal(ExpectedGuidString);
        _principalWithInt = CreatePrincipal(ExpectedIntString);
    }

    [Benchmark(Baseline = true, Description = "1. GetUserId<Guid>: Manual FindFirst + Guid.Parse")]
    public Guid GetUserIdGuid_ManualParse()
    {
        return ParseGuidManual(_principalWithGuid);
    }

    [Benchmark(Description = "1. GetUserId<Guid>: TL.ExtensionLibrary Typed")]
    public Guid GetUserIdGuid_ExtensionLibrary()
    {
        return _principalWithGuid.GetUserId<Guid>();
    }

    [Benchmark(Description = "2. GetUserId<int>: Manual FindFirst + int.TryParse (Baseline)")]
    public int GetUserIdInt_ManualParse()
    {
        return ParseIntManual(_principalWithInt);
    }

    [Benchmark(Description = "2. GetUserId<int>: TL.ExtensionLibrary Typed")]
    public int GetUserIdInt_ExtensionLibrary()
    {
        return _principalWithInt.GetUserId<int>();
    }

    private static ClaimsPrincipal CreatePrincipal(string subValue)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim("sub", subValue),
            new Claim(ClaimTypes.Name, "Alice Engineer"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Email, "alice@enterprise.org")
        }, "TestAuth");

        return new ClaimsPrincipal(identity);
    }

    private static Guid ParseGuidManual(ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst("sub");
        if (claim is null || string.IsNullOrWhiteSpace(claim.Value))
        {
            return Guid.Empty;
        }

        return Guid.TryParse(claim.Value, out var guid) ? guid : Guid.Empty;
    }

    private static int ParseIntManual(ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst("sub");
        if (claim is null || string.IsNullOrWhiteSpace(claim.Value))
        {
            return 0;
        }

        return int.TryParse(claim.Value, out var id) ? id : 0;
    }
}
