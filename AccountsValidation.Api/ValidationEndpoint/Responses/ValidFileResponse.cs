namespace AccountsValidation.Api.ValidationEndpoint.Responses;

public record ValidFileResponse(IEnumerable<LinePerformance> Performance)
    : BaseResponse(FileValid: true, Performance: Performance);
