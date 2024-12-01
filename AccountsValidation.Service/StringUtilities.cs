namespace AccountsValidation.Service;

public static class StringUtilities
{
    /// <summary>
    /// Joins words with comma
    /// </summary>
    /// <param name="words">Words to make a sentence from</param>
    /// <returns>String of words separated by comma</returns>
    public static string CreateSentence(IEnumerable<string> words)
    {
        if (!words.Any())
            return "";

        var firstWord = words.First().Capitalize();
        var correctedCaseWords = words.Skip(1).Select(word => word.ToLower()).Prepend(firstWord);
        return string.Join(", ", correctedCaseWords);
    }
}
