namespace AccountsValidation.Service.Tests;

public class AccountParserTests
{
    [Fact]
    public void WithValidInput_ReturnsAccount()
    {
        // Arrange
        string input = "1234567;Max";

        // Act
        Account account = AccountParser.Parse(input);

        // Assert
        Assert.Equal("1234567", account.Number);
        Assert.Equal("Max", account.Name);
    }

    [Fact]
    public void WithMultipleSemiColons_ReturnsAccount()
    {
        // Arrange
        string input = "1234567;Max;";

        // Act
        Account account = AccountParser.Parse(input);

        // Assert
        Assert.Equal("1234567", account.Number);
        Assert.Equal("Max", account.Name);
    }

    [Fact]
    public void WithInvalidInput_ThrowsError()
    {
        // Arrange
        string input = "1234567,Max";

        // Act
        var exception = Assert.Throws<ArgumentException>(() => AccountParser.Parse(input));

        // Assert
        Assert.Equal(
            "Fields must be separated by ';' character (Parameter 'input')",
            exception.Message
        );
    }

    [Fact]
    public void WithSpacesAroundFields_TrimsSpaces()
    {
        // Arrange
        string input = " 1234567  ;   Max    ";

        // Act
        Account account = AccountParser.Parse(input);

        // Assert
        Assert.Equal("1234567", account.Number);
        Assert.Equal("Max", account.Name);
    }
}
