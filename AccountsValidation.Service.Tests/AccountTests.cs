using System.ComponentModel.DataAnnotations;

namespace AccountsValidation.Service.Tests;

public class AccountTests
{
    [Fact]
    public void WhenValid_ReturnsNoErrors()
    {
        // Arrange
        var account = new Account(name: "Max", number: "3123456");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void WithEmptyName_ReturnsErrors()
    {
        // Arrange
        var account = new Account(name: "", number: "3123456");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Single(validationResults);
        Assert.Equal("The Name field is required.", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void WithLowercaseFirstLetterName_ReturnsErrors()
    {
        // Arrange
        var account = new Account(name: "max", number: "3123456");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Single(validationResults);
        Assert.Equal(
            "Must begin with capital alphabetical letter.",
            validationResults[0].ErrorMessage
        );
    }

    [Fact]
    public void WithEmptyAccountNumber_ReturnsErrors()
    {
        // Arrange
        var account = new Account(name: "Max", number: "");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Single(validationResults);
        Assert.Equal("The Number field is required.", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void WhenAccountNumberBeginsWith4_ReturnsNoErrors()
    {
        // Arrange
        var account = new Account(name: "Max", number: "4123456");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void WhenAccountNumberBeginsWithOtherDigitsThan3Or4_ReturnsErrors()
    {
        // Arrange
        var account = new Account(name: "Max", number: "0123456");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Single(validationResults);
        Assert.Equal(
            "Must be a 7 digit number, starting with 3 or 4, with optional 'acc' at the end.",
            validationResults[0].ErrorMessage
        );
    }

    [Fact]
    public void WhenAccountNumberHasMoreThan7Digits_ReturnsErrors()
    {
        // Arrange
        var account = new Account(name: "Max", number: "31234567");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Single(validationResults);
        Assert.Equal(
            "Must be a 7 digit number, starting with 3 or 4, with optional 'acc' at the end.",
            validationResults[0].ErrorMessage
        );
    }

    [Fact]
    public void WhenAccountNumberHasAccAtTheEnd_ReturnsNoErrors()
    {
        // Arrange
        var account = new Account(name: "Max", number: "4123456acc");

        // Act
        var validationResults = Validate(account);

        // Assert
        Assert.Empty(validationResults);
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
        Assert.Equal(
            "Must begin with capital alphabetical letter.",
            validationResults[0].ErrorMessage
        );
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
