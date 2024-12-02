namespace AccountsValidation.Service.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("word", "Word")]
    [InlineData("", "")]
    [InlineData(" word", " word")]
    [InlineData("0word", "0word")]
    public void WithValidString_ReturnsCapitalizedString(string input, string expected)
    {
        // Act
        var result = input.Capitalize();

        // Assert
        Assert.Equal(expected, result);
    }
}
