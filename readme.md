# About

Web app that validates accounts list file.

Homework for Danske bank.

# Usage

Run server:

```sh
dotnet run --project ./AccountValidation.Api/
```

Swagger app is available at http://localhost:5138/swagger/index.html

Run tests:

```sh
dotnet test
```

# Features

Server has one endpoint `POST /validate`, that accepts a file to validate.

Server expects file where each line represents account name and account number. Fields are separated by semicolon ";".

In case of a valid file, server returns OK response code with JSON `{ fileValid: true }`

In case of an invalid file, server returns BAD_REQUEST with JSON body, that contains validation messages and performance statistics.

# Code considerations

Solution is split into several projects:

- `Api` - contains single endpoint that accepts input and formats JSON output.

- `Service` - parses and validates file, measures validation performance and formats human-readable output.

- `Service.Tests` - contains unit tests for files in `Service` project.

Validations are configured declaratively, with attributes, instead of imperatively.
