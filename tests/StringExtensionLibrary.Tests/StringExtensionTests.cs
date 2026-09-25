using StringExtensionLibrary;
using Xunit;

namespace StringExtensionLibrary.Tests;

public class StringExtensionTests
{
    [Fact]
    public void TryToInt_ShouldParseValidInteger()
    {
        bool success = "12345".TryToInt(out int value);
        Assert.True(success);
        Assert.Equal(12345, value);
    }

    [Fact]
    public void TryToInt_ShouldFailFastOnInvalidInput()
    {
        bool success = "abc".TryToInt(out int value);
        Assert.False(success);
        Assert.Equal(0, value);
    }

    [Fact]
    public void ToIntOrDefault_ShouldReturnDefaultValueOnInvalidInput()
    {
        Assert.Equal(-1, "invalido".ToIntOrDefault(-1));
        Assert.Equal(50, "50".ToIntOrDefault(-1));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("yes", true)]
    [InlineData("no", false)]
    [InlineData("y", true)]
    [InlineData("n", false)]
    public void ToBoolean_ShouldParseValidValues(string input, bool expected)
    {
        Assert.Equal(expected, input.ToBoolean());
    }

    [Fact]
    public void ParseStringToCsv_ShouldWrapInQuotesAndEscape()
    {
        string input = "Hello, \"World\"";
        string csv = input.ParseStringToCsv();
        Assert.Equal("\"Hello, \"\"World\"\"\"", csv);
    }

    [Fact]
    public void Left_And_Right_ShouldExtractSubstrings()
    {
        string text = "HelloWorld";
        Assert.Equal("Hello", text.Left(5));
        Assert.Equal("World", text.Right(5));
        Assert.Equal("Hello", text.LeftOf('W'));
        Assert.Equal("orld", text.RightOf('W'));
    }

    [Fact]
    public void Truncate_ShouldTruncateWithEllipsis()
    {
        string text = "DotNet Utility Library";
        Assert.Equal("DotN...", text.Truncate(4));
        Assert.Equal("DotNet Utility Library", text.Truncate(50));
    }

    [Fact]
    public void TrimStart_And_TrimEnd_ShouldTrimSpecificStrings()
    {
        string text = "###Title###";
        Assert.Equal("Title###", text.TrimStart("#"));
        Assert.Equal("Title", text.TrimStart("#").TrimEnd("#"));
    }

    [Fact]
    public void Regex_Validations_ShouldVerifyIPv4AndEmail()
    {
        Assert.True("192.168.1.1".IsValidIPv4());
        Assert.False("999.999.999.999".IsValidIPv4());
        Assert.True("test@example.com".IsEmailAddress());
        Assert.False("invalid-email".IsEmailAddress());
    }

    [Fact]
    public void Capitalize_And_Reverse_ShouldManipulateString()
    {
        Assert.Equal("Developer", "developer".Capitalize());
        Assert.Equal("dcba", "abcd".Reverse());
    }

    [Fact]
    public void ToEnum_ShouldParseEnumType()
    {
        var result = "Sunday".ToEnum<System.DayOfWeek>();
        Assert.Equal(System.DayOfWeek.Sunday, result);
    }

    [Fact]
    public void JsonToDictionary_ShouldParseValidJsonWithPrimitives()
    {
        string json = "{\"name\":\"Alice\",\"age\":30,\"active\":true}";
        var dictionary = json.JsonToDictionary();

        Assert.NotNull(dictionary);
        Assert.Equal("Alice", dictionary["name"]);
        Assert.Equal(30L, dictionary["age"]);
        Assert.Equal(true, dictionary["active"]);
    }

    [Fact]
    public void JsonToDictionary_WithNullOrEmpty_ShouldThrowArgumentNullException()
    {
        string? nullJson = null;
        string emptyJson = string.Empty;

        Assert.Throws<ArgumentNullException>(() => nullJson!.JsonToDictionary());
        Assert.Throws<ArgumentNullException>(() => emptyJson.JsonToDictionary());
    }

    [Fact]
    public void QueryStringToDictionary_ShouldParseUrlParameters()
    {
        string query = "?name=Alice&role=admin";
        var dict = query.QueryStringToDictionary();

        Assert.Equal("Alice", dict["name"]);
        Assert.Equal("admin", dict["role"]);
    }
}