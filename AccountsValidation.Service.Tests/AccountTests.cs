using System.ComponentModel.DataAnnotations;

namespace AccountsValidation.Service.Tests;

public class AccountTests
{
    [Theory]
    [InlineData("Max", "3123456")]
    [InlineData("Max", "4123456")]
    [InlineData("Max", "4123456acc")]
    public void WhenValid_ReturnsNoErrors(string name, string number)
    {
        // Arrange
        var account = new Account(name, number);

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData("", "The Name field is required.")]
    [InlineData("max", "Must begin with capital alphabetical letter.")]
    public void WithInvalidName_ReturnsErrors(string accountName, string expectedMessage)
    {
        // Arrange
        var account = new Account(name: accountName, number: "3123456");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Single(validationResults);
        Assert.Equal(expectedMessage, validationResults[0].ErrorMessage);
    }

    [Theory]
    [InlineData("", "The Number field is required.")]
    [InlineData("0123456", "Must be a 7 digit number, starting with 3 or 4, with optional 'acc' at the end.")]
    [InlineData("31234567", "Must be a 7 digit number, starting with 3 or 4, with optional 'acc' at the end.")]
    [InlineData("3123456ac", "Must be a 7 digit number, starting with 3 or 4, with optional 'acc' at the end.")]
    public void WithInvalidAccountNumber_ReturnsErrors(string accountNumber, string expectedMessage)
    {
        // Arrange
        var account = new Account(name: "Max", number: accountNumber);

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Single(validationResults);
        Assert.Equal(expectedMessage, validationResults[0].ErrorMessage);
    }

    [Fact]
    public void WithMultipleInvalidFields_ReturnsErrors()
    {
        // Arrange
        var account = new Account(name: "max", number: "31234567");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Equal(2, validationResults.Count);
        Assert.Equal("Must begin with capital alphabetical letter.", validationResults[0].ErrorMessage);
        Assert.Equal(
            "Must be a 7 digit number, starting with 3 or 4, with optional 'acc' at the end.",
            validationResults[1].ErrorMessage
        );
    }

    private static List<ValidationResult> Validate(Account account)
    {
        ValidationContext validator = new(account);
        List<ValidationResult> validationResults = [];

        Validator.TryValidateObject(account, validator, validationResults, true);

        return validationResults;
    }
}
