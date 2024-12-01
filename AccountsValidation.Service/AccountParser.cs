namespace AccountsValidation.Service;

public static class AccountParser
{
    public static Account Parse(string input)
    {
        var parts = input.Split(';');

        if (parts.Length < 2)
        {
            throw new ArgumentException("Fields must be separated by ';' character", nameof(input));
        }

        var number = parts[0].Trim();
        var name = parts[1].Trim();

        Account account = new(number: number, name: name);

        return account;
    }
}
