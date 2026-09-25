using System;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using ExtensionLibrary.Benchmarks.Common;
using Humanizer;
using StringExtensionLibrary;

namespace ExtensionLibrary.Benchmarks.Suites;

/// <summary>
/// Bateria comparativa de operações comuns de strings entre BCL, TL.ExtensionLibrary e Humanizer.
/// </summary>
public class StringBenchmarks : BenchmarkBase
{
    private string _longText = null!;
    private string _logInput = null!;
    private string _invalidNumberString = null!;
    private Regex _compiledRegex = null!;

    [GlobalSetup]
    public void Setup()
    {
        _longText = "A arquitetura de software orientada a microsservicos e servicos distribuidos exige observabilidade avancada e baixo consumo de memoria.";
        _logInput = "Usuario admin realizou logon com sucesso\r\nTentativa em host nao autorizado\nSessao terminada\r";
        _invalidNumberString = "425980_INVALID_SUFFIX";
        _compiledRegex = new Regex(@"[\r\n]", RegexOptions.Compiled);
    }

    [Benchmark(Baseline = true, Description = "1. Truncate: Substring Concatenation")]
    public string Truncate_Baseline()
    {
        return TruncateViaSubstring(_longText, 25);
    }

    [Benchmark(Description = "1. Truncate: TL.ExtensionLibrary (string.Create)")]
    public string Truncate_ExtensionLibrary()
    {
        return StringExtensionLibrary.StringExtensions.Truncate(_longText, 25);
    }

    [Benchmark(Description = "1. Truncate: Humanizer")]
    public string Truncate_Humanizer()
    {
        return Humanizer.TruncateExtensions.Truncate(_longText, 25, "...", Truncator.FixedLength);
    }

    [Benchmark(Description = "2. SanitizeLog: Regex Replace Compiled (Baseline)")]
    public string SanitizeLog_RegexCompiled()
    {
        return _compiledRegex.Replace(_logInput, "_");
    }

    [Benchmark(Description = "2. SanitizeLog: TL.ExtensionLibrary (Double Replace)")]
    public string SanitizeLog_ExtensionLibrary()
    {
        return _logInput.SanitizeForLog();
    }

    [Benchmark(Description = "3. ToInt: Defensive Try-Catch Exception (Baseline)")]
    public int ParseInt_DefensiveTryCatch()
    {
        try
        {
            return int.Parse(_invalidNumberString);
        }
        catch (FormatException)
        {
            return 0;
        }
    }

    [Benchmark(Description = "3. ToInt: TL.ExtensionLibrary (Invariant TryParse)")]
    public int ParseInt_ExtensionLibrary()
    {
        return _invalidNumberString.ToIntOrDefault(0);
    }

    private static string TruncateViaSubstring(string text, int length)
    {
        if (string.IsNullOrEmpty(text) || length <= 0)
        {
            return string.Empty;
        }

        if (text.Length > length)
        {
            return text.Substring(0, length) + "...";
        }

        return text;
    }
}
