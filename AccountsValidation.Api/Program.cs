using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/validate", ValidateAccountsFile).WithName("GetWeatherForecast").DisableAntiforgery();

app.Run();

static Results<Ok<ValidFileResponse>, BadRequest<InvalidFileResponse>> ValidateAccountsFile(
    IFormFile file
)
{
    using var reader = new StreamReader(file.OpenReadStream());
    var invalidLines = AccountValidator.ValidateStream(reader);

    if (invalidLines.Any())
    {
        var response = new InvalidFileResponse(FileValid: false, InvalidLines: invalidLines);
        return TypedResults.BadRequest(response);
    }

    return TypedResults.Ok(new ValidFileResponse());
}

public record ValidFileResponse(bool FileValid = true);

public record InvalidFileResponse(bool FileValid, IList<string> InvalidLines);

public record Account(string Number, string Name);

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
