using AccountsValidation.Api.ValidationEndpoint;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/validate", ValidationEndpoint.Validate)
    .WithName("Validate accounts file")
    .DisableAntiforgery();

app.Run();
