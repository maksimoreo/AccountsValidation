namespace AccountsValidation.Service.Tests;

public class StringUtilitiesTests
{
    [Theory]
    [InlineData(new string[] { }, "")]
    [InlineData(new string[] { "one" }, "One")]
    [InlineData(new string[] { "one", "two", "three" }, "One, two, three")]
    [InlineData(new string[] { "One", "Two", "Three" }, "One, two, three")]
    public void WithValidInput_ReturnsSetnence(string[] input, string expected)
    {
        // Act
        string result = StringUtilities.CreateSentence(input);

        // Assert
        Assert.Equal(expected, result);
    }
}
