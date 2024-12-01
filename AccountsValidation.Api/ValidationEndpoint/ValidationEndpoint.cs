using AccountsValidation.Api.ValidationEndpoint.Responses;
using AccountsValidation.Service;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AccountsValidation.Api.ValidationEndpoint;

public static class ValidationEndpoint
{
    public static Results<Ok<ValidFileResponse>, BadRequest<InvalidFileResponse>> Validate(
        IFormFile file
    )
    {
        using var reader = new StreamReader(file.OpenReadStream());

        var validator = new AccountsStreamValidator();
        var invalidLines = validator.ValidateStream(reader);

        var performance = validator.ExecutionTimePerLine.Select(
            (pair) =>
                new LinePerformance(
                    pair.Key,
                    ExecutionTimeInMilliseconds: pair.Value.TotalMilliseconds
                )
        );

        if (invalidLines.Any())
        {
            var response = new InvalidFileResponse(
                InvalidLines: invalidLines,
                Performance: performance
            );
            return TypedResults.BadRequest(response);
        }

        return TypedResults.Ok(new ValidFileResponse(Performance: performance));
    }
}
