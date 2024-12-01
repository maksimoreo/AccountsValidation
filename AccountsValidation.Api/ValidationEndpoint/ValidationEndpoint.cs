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
        var validationResult = validator.ValidateStream(reader);

        var performance = validationResult.ExecutionTimePerLine.Select(
            (pair) =>
                new LinePerformance(
                    pair.Key,
                    ExecutionTimeInMilliseconds: pair.Value.TotalMilliseconds
                )
        );

        if (validationResult.InvalidLines.Count == 0)
        {
            return TypedResults.Ok(new ValidFileResponse(Performance: performance));
        }

        var response = new InvalidFileResponse(
            InvalidLines: validationResult.InvalidLines,
            Performance: performance
        );
        return TypedResults.BadRequest(response);
    }
}
