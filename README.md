# Stock Compliance API

## Overview

Stock Compliance API is a .NET-based backend application designed to manage users, stocks, and financial data while evaluating stock compliance based on ethical investment principles. The API integrates external financial data providers and applies a set of custom rules to determine whether a stock is compliant or non-compliant.

## Features

- Authentication & Authorization
  - User registration and login
  - Built using ASP.NET Identity
  - Role-based access control (RBAC)
- User Management
  - Users, Roles, Permissions, Modules
- Banking Module
  - Bank management
  - User-bank mapping (UserBanks)
- Stock Data
  - Fetch stock data using external APIs
  - Support for real-time and historical data
  - Chart integration for stock trends
- Compliance Engine
  - Sector screening
  - Debt ratios, cash ratios, asset composition
  - Revenue allocation checks
  - Outputs: ✅ Compliant / ❌ Non-Compliant

## Tech Stack

- .NET 9 (ASP.NET Core)
- ASP.NET Identity
- Entity Framework Core
- External APIs: Chart service, Financial Modeling Prep (FMP)

## External Integrations

The project integrates with one or more external services for financial and charting data. Configure API keys and endpoints in your configuration (appsettings.json or environment variables):

- FMP (Financial Modeling Prep) API key
- Charts API key / endpoint

## Getting Started (Local Development)

Prerequisites:
- .NET 9 SDK
- A supported database (configured in appsettings.json)

Common commands:

1. Restore dependencies
   - dotnet restore
2. Build
   - dotnet build
3. Apply EF Core migrations (if using migrations)
   - dotnet ef database update --project StockTradingApplication
4. Run
   - dotnet run --project StockTradingApplication
5. Run tests
   - dotnet test

Note: Adjust the project path and parameters to match your environment. The project may expose HTTPS endpoints (check launchSettings.json or Program.cs for configured URLs).

## Configuration

Use appsettings.json and environment variables to configure:
- Connection string for the database
- External API keys (FMP, Charts)
- Logging and other application settings

Example environment variables:
- ConnectionStrings__DefaultConnection
- FMP__ApiKey
- CHARTS__ApiKey
- ASPNETCORE_ENVIRONMENT

## Project Structure (high level)

- Controllers/ — HTTP endpoints and route definitions
- Models/ — EF entities, request and response models
- Services/ — Business logic and external API integration
- Services/IServices/ — Service interfaces
- Tests/IntergrationTests/ — Integration tests covering controller routes and flows

## Compliance Engine

The Compliance Engine evaluates stocks using a series of financial checks and business rules, including:
- Sector exclusions (industry-level screening)
- Financial ratios (debt, cash, assets)
- Revenue composition checks

Result categories:
- Compliant (Halal)
- Non-compliant (Haram)

## License

Specify project license in the repository root (e.g., LICENSE file).

----

This README provides a concise overview and common development steps. Refer to the repository source code and controller XML comments for detailed API behavior and parameter details.
