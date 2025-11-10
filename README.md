## Old Phone Pad

A clean, production-ready C# solution that simulates the behavior of classic mobile phone keypads translating numeric keypress sequences (e.g. `4433555 555666#`) into human-readable text (`HELLO`).

The **OldPhonePad** solution implements a modular, testable, and extensible architecture for decoding numeric keypress inputs into text. It follows **SOLID principles**, **clean code practices**, and a **three-tier layered structure** that’s ready for continuous integration, Docker deployment, and scaling.

---

## Architecture Overview

### Project Structure

```
old-phone-pad/
│
├── src/
│ ├── OldPhonePad.Lib/ # Core decoding library (business logic)
│ ├── OldPhonePad.Console/ # Console interface for manual interaction
│ └── OldPhonePad.Api/ # Minimal ASP.NET Web API exposing /decode endpoint
│
├── tests/
│ └── OldPhonePad.Tests/ # Unit & edge case tests (xUnit)
│
├── Dockerfile # Docker image for API
├── old-phone-pad.sln # Solution file
└── README.md # Documentation
```

### Architecture Layers

| Layer | Description | Key Technologies |
|--------|--------------|------------------|
| **Library** | Core decoding logic and business rules | C#, OOP, SOLID |
| **Console** | Command-line UI for manual testing | .NET Console, DI |
| **API** | Minimal REST API exposing `/decode` endpoint | ASP.NET Minimal API, Swagger, Logging |
| **Tests** | xUnit-based unit tests for correctness & robustness | xUnit, .NET Test SDK |

### Design Principles

| Principle | Implementation |
|------------|----------------|
| **Single Responsibility** | Each class and interface handles exactly one concern. |
| **Open/Closed** | Adding new keypad layouts or rules doesn’t require modifying core logic. |
| **Liskov Substitution** | All abstractions (`IKeyMapping`, `IInputTokenizer`, etc.) can be safely substituted. |
| **Interface Segregation** | Small, focused contracts instead of “god interfaces”. |
| **Dependency Inversion** | High-level logic depends on abstractions, not concrete types. |

Also applies:
- **DRY** - shared logic lives in the library.
- **Fail Fast** - explicit exceptions for invalid input.
- **Dependency Injection** - used across Console and API via `Microsoft.Extensions.DependencyInjection`.

### Core Logic

The decoding engine interprets sequences of digits (2–9) according to multi-tap SMS rules.

#### Example Inputs / Outputs

| Input | Output |
|--------|--------|
| `33#` | `E` |
| `227*#` | `B` |
| `4433555 555666#` | `HELLO` |
| `8 88777444666*664#` | `TURING` |

#### Features
- Handles **backspaces (`*`)** correctly.
- Ignores **invalid or stray characters** safely.
- Applies **guardrails** (max input length).
- Uses **dependency injection** for flexibility.
- Supports **logging** via `ILogger<T>` (no-op in console).
- Robust unit testing and Dockerized API endpoint.

---

## Testing

### Framework
- [xUnit](https://xunit.net/)
- Built-in `Assert` syntax and clear test naming.

### Coverage
✅ 10 core functional tests  
✅ 5 extended robustness tests (edge and negative cases)

### Example test categories
- Valid decoding (`HELLO`, `TURING`, etc.)
- Backspace handling (`*`)
- Invalid input ignoring
- Missing `#` termination
- Guardrail overflow rejection

### Running Locally

#### Requirements
- .NET SDK 9.0+
- Docker (optional for API container)
- macOS, Windows, or Linux

#### 1) Console Application
```
dotnet build
dotnet test
dotnet run --project src/OldPhonePad.Console
```
#### Interactive Run Example:
```
Old Phone Pad Decoder
Enter keypresses ending with '#' (or press Enter to run sample inputs):
4433555 555666#
Decoded: HELLO
```

#### 2) API Application
```
dotnet run --project src/OldPhonePad.Api
```
#### Test with curl:
```
curl -X POST http://localhost:5000/decode \
  -H "Content-Type: application/json" \
  -d '{"input":"4433555 555666#"}'
```
#### Expected response:
```
{
  "input": "4433555 555666#",
  "output": "HELLO"
}
```

#### 3) Run with Docker
```
docker build -t oldphoneapi ./src/OldPhonePad.Api
docker run --rm -p 5000:80 oldphoneapi
```
Then call the API using the above curl and verify the expected response.

---

## CI/CD Readiness

### Compatible with modern CI/CD workflows

✅ Build and test via dotnet build / dotnet test

✅ Dockerized API for deployment

✅ Ready for GitHub Actions, Azure Pipelines, or Jenkins

#### Example GitHub Action
```
name: Build & Test

on:
  push:
    branches: [ main, master ]
  pull_request:
    types: [opened, synchronize, reopened]

env:
  DOTNET_SDK: '9.0.x'

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_SDK }}

      - name: Cache NuGet packages
        uses: actions/cache@v4
        with:
          path: ~/.nuget/packages
          key: nuget-packages-${{ runner.os }}-${{ hashFiles('**/*.csproj') }}
          restore-keys: |
            nuget-packages-${{ runner.os }}-

      - name: Restore dependencies
        run: dotnet restore --no-cache

      - name: Build (Release)
        run: dotnet build --no-restore --configuration Release

      - name: Run unit tests
        run: dotnet test --no-build --verbosity normal
```

## Engineering Highlights
- Architected a clean, modular .NET solution (Library, API, Console, Tests) applying SOLID and Clean Architecture principles for scalability and maintainability.
- Implemented Dependency Injection, Logging, and Unit Testing using standard .NET patterns (ILogger<T>, DI containers, xUnit).
- Developed robust core logic with strong input validation, error handling, and predictable decoding behavior for reliability under production conditions.
- Enabled full CI/CD pipeline with GitHub Actions for automated build, restore, test, and (optional) Docker image validation.
- Containerized the API with Docker for reproducible and environment-independent deployment.
- Built comprehensive test coverage (functional + edge cases) ensuring quality, regression safety, and confidence in delivery.
- Designed with DevOps and observability in mind, using structured logging and defensive programming principles.
- Delivered production-ready code and documentation, with a professional README, .gitignore, and CI-ready repo hygiene.
