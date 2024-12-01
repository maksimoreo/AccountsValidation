namespace AccountsValidation.Service.Tests;

public class StringUtilitiesTests
{
    [Fact]
    public void WithValidInput_ReturnsSetnence()
    {
        // Arrange
        List<string> words = ["one", "two", "three"];

        // Act
        string result = StringUtilities.CreateSentence(words);

        // Assert
        Assert.Equal("One, two, three", result);
    }

    [Fact]
    public void WithEmptyInput_ReturnsEmptyString()
    {
        // Arrange
        List<string> words = [];

        // Act
        string result = StringUtilities.CreateSentence(words);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void WithSingleWord_ReturnsSingleWordString()
    {
        // Arrange
        List<string> words = ["one"];

        // Act
        string result = StringUtilities.CreateSentence(words);

        // Assert
        Assert.Equal("One", result);
    }

    [Fact]
    public void WhenAllWordsAreCapitalized_ReturnsCorrectSentence()
    {
        // Arrange
        List<string> words = ["One", "Two", "Three"];

        // Act
        string result = StringUtilities.CreateSentence(words);

        // Assert
        Assert.Equal("One, two, three", result);
    }
}
