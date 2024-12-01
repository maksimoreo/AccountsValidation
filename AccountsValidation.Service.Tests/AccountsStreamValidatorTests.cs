using System.Text;

namespace AccountsValidation.Service.Tests;

public class AccountsStreamValidatorTests
{
    [Fact]
    public void WithAllValidLines_ReturnsEmpty()
    {
        // Arrange
        string input =
            @"3123456;Thomas
3123456;Richard
        ";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using var inputStreamReader = new StreamReader(inputStream);

        // Act
        var validationMessages = new AccountsStreamValidator().ValidateStream(inputStreamReader);

        // Assert
        Assert.Empty(validationMessages);
    }

    [Fact]
    public void WithInvalidLines_ReturnsValidationMessages()
    {
        // Arrange
        string input =
            @"31234567;Thomas
3123456;richard
        ";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using var inputStreamReader = new StreamReader(inputStream);

        // Act
        var validationMessages = new AccountsStreamValidator().ValidateStream(inputStreamReader);

        // Assert
        List<string> expectedMessages =
        [
            "Account number - not valid for 1 line 'Thomas 31234567'",
            "Account name - not valid for 2 line 'richard 3123456'",
        ];
        Assert.Equal(expectedMessages, validationMessages);
    }

    [Fact]
    public void WithMultipleInvalidFields_ReturnsFormattedListOfFields()
    {
        // Arrange
        string input = "31234567;thomas";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using var inputStreamReader = new StreamReader(inputStream);

        // Act
        var validationMessages = new AccountsStreamValidator().ValidateStream(inputStreamReader);

        // Assert
        List<string> expectedMessages =
        [
            "Account name, account number - not valid for 1 line 'thomas 31234567'",
        ];
        Assert.Equal(expectedMessages, validationMessages);
    }

    [Fact]
    public void WithEmptyFields_ReturnsErrors()
    {
        // Arrange
        string input = ";";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using var inputStreamReader = new StreamReader(inputStream);

        // Act
        var validationMessages = new AccountsStreamValidator().ValidateStream(inputStreamReader);

        // Assert
        List<string> expectedMessages = ["Account name, account number - not valid for 1 line ' '"];
        Assert.Equal(expectedMessages, validationMessages);
    }

    [Fact]
    public void WithEmptyLines_ReturnsCorrectLineIndex()
    {
        // Arrange
        string input =
            @"
31234567;Thomas


3123456;richard

        ";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using var inputStreamReader = new StreamReader(inputStream);

        // Act
        var validationMessages = new AccountsStreamValidator().ValidateStream(inputStreamReader);

        // Assert
        List<string> expectedMessages =
        [
            "Account number - not valid for 2 line 'Thomas 31234567'",
            "Account name - not valid for 5 line 'richard 3123456'",
        ];
        Assert.Equal(expectedMessages, validationMessages);
    }

    [Fact]
    public void WithGivenInputFromRequirements_ReturnsExpectedMessages()
    {
        // Arrange
        string input =
            @"32999921;Thomas
3293982acc;Richard
8293982;xAEA-12
329a982;Rose
329398.;Bob
3113902;michael
3113902acc;Rob;
        ";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using var inputStreamReader = new StreamReader(inputStream);

        // Act
        var validationMessages = new AccountsStreamValidator().ValidateStream(inputStreamReader);

        // Assert
        List<string> expectedMessages =
        [
            "Account number - not valid for 1 line 'Thomas 32999921'",
            "Account name, account number - not valid for 3 line 'xAEA-12 8293982'",
            "Account number - not valid for 4 line 'Rose 329a982'",
            "Account number - not valid for 5 line 'Bob 329398.'",
            "Account name - not valid for 6 line 'michael 3113902'",
        ];
        Assert.Equal(expectedMessages, validationMessages);
    }
}
