namespace AccountsValidation.Service.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void WithValidString_ReturnsCapitalizedString()
    {
        // Arrange
        var input = "word";

        // Act
        var result = input.Capitalize();

        // Assert
        Assert.Equal("Word", result);
    }

    [Fact]
    public void WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var input = "";

        // Act
        var result = input.Capitalize();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void WhenStringBeginsWithSpace_ReturnsUnchangedString()
    {
        // Arrange
        var input = " word";

        // Act
        var result = input.Capitalize();

        // Assert
        Assert.Equal(" word", result);
    }

    [Fact]
    public void WhenStringBeginsWithDigit_ReturnsUnchangedString()
    {
        // Arrange
        var input = "0word";

        // Act
        var result = input.Capitalize();

        // Assert
        Assert.Equal("0word", result);
    }
}
