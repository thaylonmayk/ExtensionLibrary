using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using ExtensionLibrary.Benchmarks.Common;
using ObjectExtensionsLibrary;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria de testes para clonagem profunda e inspeção genérica de objetos.
/// </summary>
public class ObjectBenchmarks : BenchmarkBase
{
    private BenchmarkUser _user = null!;
    private BenchmarkUser _otherUser = null!;
    private PropertyInfo[] _userProperties = null!;

    [GlobalSetup]
    public void Setup()
    {
        _user = new BenchmarkUser
        {
            Id = 101,
            Name = "John Doe Administrator",
            Email = "johndoe@enterprise.org",
            Role = "PlatformEngineer",
            CreatedAt = new DateTime(2026, 3, 15, 10, 30, 0, DateTimeKind.Utc)
        };

        _otherUser = new BenchmarkUser
        {
            Id = 101,
            Name = "John Doe Administrator",
            Email = "johndoe@enterprise.org",
            Role = "PlatformEngineer",
            CreatedAt = new DateTime(2026, 3, 15, 10, 30, 0, DateTimeKind.Utc)
        };

        _userProperties = typeof(BenchmarkUser).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    [Benchmark(Baseline = true, Description = "1. Clone: Reflection Property-by-Property")]
    public BenchmarkUser Clone_Reflection()
    {
        return CloneViaReflection(_user, _userProperties);
    }

    [Benchmark(Description = "1. Clone: TL.ExtensionLibrary (System.Text.Json)")]
    public BenchmarkUser? Clone_ExtensionLibrary()
    {
        return _user.Clone();
    }

    [Benchmark(Description = "2. Equality: Manual Property Comparison (Baseline)")]
    public bool Equality_ManualComparison()
    {
        return _user.Id == _otherUser.Id
            && _user.Name == _otherUser.Name
            && _user.Email == _otherUser.Email
            && _user.Role == _otherUser.Role
            && _user.CreatedAt == _otherUser.CreatedAt;
    }

    [Benchmark(Description = "2. Equality: TL.ExtensionLibrary PropertiesEqual")]
    public bool Equality_ExtensionLibrary()
    {
        return _user.PropertiesEqual(_otherUser);
    }

    [Benchmark(Description = "3. IsDefault: Boxing object.Equals (Baseline)")]
    public bool IsDefault_BoxingEquals()
    {
        return object.Equals(_user, default(BenchmarkUser));
    }

    [Benchmark(Description = "3. IsDefault: TL.ExtensionLibrary EqualityComparer")]
    public bool IsDefault_ExtensionLibrary()
    {
        return _user.IsDefault();
    }

    private static BenchmarkUser CloneViaReflection(BenchmarkUser source, PropertyInfo[] properties)
    {
        var target = new BenchmarkUser();
        for (int i = 0; i < properties.Length; i++)
        {
            var prop = properties[i];
            prop.SetValue(target, prop.GetValue(source));
        }
        return target;
    }
}
