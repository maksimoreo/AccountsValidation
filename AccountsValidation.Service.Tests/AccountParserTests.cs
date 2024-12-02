namespace AccountsValidation.Service.Tests;

public class AccountParserTests
{
    [Theory]
    [InlineData("1234567;Max", "Max", "1234567")]
    [InlineData("1234567;Max;", "Max", "1234567")]
    [InlineData(" 1234567  ;   Max    ", "Max", "1234567")]
    public void WithValidInput_ReturnsAccount(
        string input,
        string expectedAccountName,
        string expectedAccountNumber
    )
    {
        // Act
        Account account = AccountParser.Parse(input);

        // Assert
        Assert.Equal(expectedAccountNumber, account.Number);
        Assert.Equal(expectedAccountName, account.Name);
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
}
