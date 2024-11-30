namespace AccountsValidation.Api.ValidationEndpoint.Responses;

public record InvalidFileResponse(bool FileValid, IList<string> InvalidLines);
