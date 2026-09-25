using System;
using System.ComponentModel;
using BenchmarkDotNet.Attributes;

namespace ExtensionLibrary.Benchmarks.Common;

/// <summary>
/// Classe base para suítes de benchmark com diagnóstico de alocação de memória.
/// </summary>
[MemoryDiagnoser]
public abstract class BenchmarkBase
{
}

/// <summary>
/// Modelo representativo para testes de ordenação, clonagem e paginação.
/// </summary>
public class BenchmarkUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Enum para avaliação de cache de descrições e atributos.
/// </summary>
public enum BenchmarkStatus
{
    [Description("Pending Approval")]
    Pending = 1,

    [Description("In Progress Active")]
    InProgress = 2,

    [Description("Completed Successfully")]
    Completed = 3,

    [Description("Canceled by User")]
    Canceled = 4
}
