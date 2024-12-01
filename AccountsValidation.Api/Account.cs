using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AccountsValidation.Api;

public class Account(string name, string number)
{
    [DisplayName("Account name")]
    [RegularExpression(@"^[A-Z].*$", ErrorMessage = "Must begin with capital alphabetical letter")]
    public string Name { get; set; } = name;

    [DisplayName("Account number")]
    // Note: Cannot use multiple regular expressions per field / prop
    [RegularExpression(
        @"^[34]\d{6}(acc){0,1}$",
        ErrorMessage = "Must be a 7 digit number, starting with 3 or 4, with optional 'acc' at the end"
    )]
    public string Number { get; set; } = number;

    public override string ToString() => $"{Name} {Number}";
}
