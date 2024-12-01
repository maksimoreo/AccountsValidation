using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AccountsValidation.Service;

public class AccountsStreamValidator
{
    private readonly List<string> invalidLines = [];

    public Dictionary<int, TimeSpan> ExecutionTimePerLine { get; } = [];

    public IList<string> ValidateStream(StreamReader inputStream)
    {
        invalidLines.Clear();
        ExecutionTimePerLine.Clear();

        for (int i = 0; inputStream.Peek() > 0; i++)
        {
            var line = inputStream.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            int lineIndex = i + 1;

            var executionTime = PerformanceUtilities.WithMeasuredTime(() =>
            {
                ProcessLine(line, lineIndex);
            });

            ExecutionTimePerLine.Add(lineIndex, executionTime);
        }

        return invalidLines;
    }

    public void ProcessLine(string line, int lineIndex)
    {
        var account = AccountParser.Parse(line);

        var validationResults = ValidateAccount(account);

        if (validationResults.Count == 0)
            return;

        string errorMessage = FormatValidationMessage(account, validationResults, lineIndex);
        invalidLines.Add(errorMessage);
    }

    public static ICollection<ValidationResult> ValidateAccount(Account account)
    {
        ValidationContext validator = new(account);
        ICollection<ValidationResult> results = [];

        Validator.TryValidateObject(account, validator, results, true);

        return results;
    }

    public static string FormatValidationMessage(
        Account account,
        ICollection<ValidationResult> validationResults,
        int lineIndex
    )
    {
        var properties = validationResults
            .SelectMany(result => result.MemberNames)
            .Distinct()
            .Select(GetPropertyDisplayName);

        var propertiesSentence = StringUtilities.CreateSentence(properties);

        return $"{propertiesSentence} - not valid for {lineIndex} line '{account}'";
    }

    public static string GetPropertyDisplayName(string property)
    {
        var propertyInfo = typeof(Account).GetProperty(property);
        var displayNameAttribute = propertyInfo!.GetCustomAttribute<DisplayNameAttribute>();
        var displayName = displayNameAttribute!.DisplayName;

        return displayName!;
    }
}
