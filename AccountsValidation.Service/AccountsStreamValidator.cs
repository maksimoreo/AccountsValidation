using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AccountsValidation.Service;

public class AccountsStreamValidator
{
    private readonly List<string> invalidLines = [];

    public IList<string> ValidateStream(StreamReader inputStream)
    {
        invalidLines.Clear();

        for (int i = 0; inputStream.Peek() > 0; i++)
        {
            var line = inputStream.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            ProcessLine(line, i);
        }

        return invalidLines;
    }

    public void ProcessLine(string line, int index)
    {
        var account = AccountParser.Parse(line);

        var validationResults = ValidateAccount(account);

        if (validationResults.Count == 0)
            return;

        string errorMessage = FormatValidationMessage(
            account,
            validationResults,
            lineIndex: index + 1
        );
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
