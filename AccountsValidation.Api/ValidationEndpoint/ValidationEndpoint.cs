using AccountsValidation.Service;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AccountsValidation.Api.ValidationEndpoint;

public static class ValidationEndpoint
{
    public static Results<
        Ok<Responses.ValidFileResponse>,
        BadRequest<Responses.InvalidFileResponse>
    > Validate(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var invalidLines = new AccountsStreamValidator().ValidateStream(reader);

        if (invalidLines.Any())
        {
            var response = new Responses.InvalidFileResponse(
                FileValid: false,
                InvalidLines: invalidLines
            );
            return TypedResults.BadRequest(response);
        }

        return TypedResults.Ok(new Responses.ValidFileResponse());
    }
}
