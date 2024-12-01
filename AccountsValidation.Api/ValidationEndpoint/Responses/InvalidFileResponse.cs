namespace AccountsValidation.Api.ValidationEndpoint.Responses;

public record InvalidFileResponse(
    IList<string> InvalidLines,
    IEnumerable<LinePerformance> Performance
) : BaseResponse(FileValid: false, Performance: Performance);
