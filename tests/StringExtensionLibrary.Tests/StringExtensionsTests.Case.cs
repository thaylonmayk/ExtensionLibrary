using System;
using StringExtensionLibrary;
using Xunit;

namespace StringExtensionLibrary.Tests
{
    public class StringExtensionsCaseTests
    {
        [Theory]
        [InlineData("PascalCaseString", "pascal_case_string")]
        [InlineData("camelCaseString", "camel_case_string")]
        [InlineData("already_snake_case", "already_snake_case")]
        [InlineData("Some HTML Text", "some_html_text")]
        [InlineData("ID", "id")]
        [InlineData("UserID", "user_id")]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("   ", "")]
        public void ToSnakeCase_ShouldFormatExpectedValues(string input, string expected)
        {
            string result = input.ToSnakeCase();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("PascalCaseString", "pascal-case-string")]
        [InlineData("camelCaseString", "camel-case-string")]
        [InlineData("already-kebab-case", "already-kebab-case")]
        [InlineData("Some HTML Text", "some-html-text")]
        [InlineData("ID", "id")]
        [InlineData("UserID", "user-id")]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("   ", "")]
        public void ToKebabCase_ShouldFormatExpectedValues(string input, string expected)
        {
            string result = input.ToKebabCase();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Aprenda C# em 21 Dias com Eficiência!", "aprenda-c-em-21-dias-com-eficiencia")]
        [InlineData("Olá Mundo! Como você está?", "ola-mundo-como-voce-esta")]
        [InlineData("   Espaços no Início e Fim   ", "espacos-no-inicio-e-fim")]
        [InlineData("Multiple---Dashes___Underscores", "multiple-dashes-underscores")]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("   ", "")]
        public void ToSlug_ShouldNormalizeAndProduceValidSlug(string input, string expected)
        {
            string result = input.ToSlug();
            Assert.Equal(expected, result);
        }
    }
}
