namespace AccountsValidation.Service;

public static class StringExtensions
{
    public static string Capitalize(this string value) =>
        value.Length switch
        {
            0 => "",
            _ => string.Concat(value[0].ToString().ToUpper(), value[1..]),
        };
}
