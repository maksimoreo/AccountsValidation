using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AccountsValidation.Service;

public class AccountsStreamValidator
{
    public record Result(List<string> InvalidLines, Dictionary<int, TimeSpan> ExecutionTimePerLine);

    private List<string> invalidLines = [];
    private Dictionary<int, TimeSpan> executionTimePerLine = [];

    public Result ValidateStream(StreamReader inputStream)
    {
        ResetState();
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

            executionTimePerLine.Add(lineIndex, executionTime);
        }

        return new Result(InvalidLines: invalidLines, ExecutionTimePerLine: executionTimePerLine);
    }

    private void ResetState()
    {
        invalidLines = [];
        executionTimePerLine = [];
    }

    private void ProcessLine(string line, int lineIndex)
    {
        var account = AccountParser.Parse(line);

        var validationResults = ValidateAccount(account);

        if (validationResults.Count == 0)
            return;

        string errorMessage = FormatValidationMessage(account, validationResults, lineIndex);
        invalidLines.Add(errorMessage);
    }

    private static ICollection<ValidationResult> ValidateAccount(Account account)
    {
        ValidationContext validator = new(account);
        ICollection<ValidationResult> results = [];

        Validator.TryValidateObject(account, validator, results, true);

        return results;
    }

    private string FormatValidationMessage(
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

    private string GetPropertyDisplayName(string property)
    {
        var propertyInfo = typeof(Account).GetProperty(property);
        var displayNameAttribute = propertyInfo!.GetCustomAttribute<DisplayNameAttribute>();
        var displayName = displayNameAttribute!.DisplayName;

        return displayName!;
    }
}
