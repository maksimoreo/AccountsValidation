using System.Text.Json.Serialization;

namespace AccountsValidation.Api.ValidationEndpoint.Responses;

public record BaseResponse(
    [property: JsonPropertyOrder(-1)] bool FileValid,
    IEnumerable<LinePerformance> Performance
);
