namespace AccountsValidation.Api;

class AccountValidator
{
    public static IList<string> ValidateStream(StreamReader inputStream)
    {
        List<string> invalidLines = [];

        for (int i = 0; inputStream.Peek() > 0; i++)
        {
            var line = inputStream.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
                break;

            var accountEntry = ParseAccountEntry(line);

            var invalidFields = ValidateAccountEntry(accountEntry);

            if (invalidFields.Any())
            {
                var invalidFieldsSerialized = string.Join(", ", invalidFields);
                var accountEntrySerialized = $"{accountEntry.Name} {accountEntry.Number}";
                var lineIndex = i + 1;

                var lineExplanation =
                    $"{invalidFieldsSerialized} - not valid for {lineIndex} line '{accountEntrySerialized}'";

                invalidLines.Add(lineExplanation);
            }
        }

        return invalidLines;
    }

    public static Account ParseAccountEntry(string input)
    {
        var parts = input.Split(';');
        var number = parts[0].Trim();
        var name = parts[1].Trim();
        var account = new Account(Number: number, Name: name);

        return account;
    }

    public static IEnumerable<string> ValidateAccountEntry(Account accountEntry)
    {
        HashSet<string> invalidFields = [];

        if (!char.IsUpper(accountEntry.Name[0]))
        {
            invalidFields.Add("Account name");
        }

        return invalidFields;
    }
}
