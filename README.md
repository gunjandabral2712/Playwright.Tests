Playwright .NET Tests (POM)
===========================

This repository contains Playwright-based UI tests written in .NET using the Page Object Model (POM).

Projects
- Playwright.Framework — class library containing page objects and test infrastructure (POM).
- Playwright.Tests — test project (NUnit) that references Playwright.Framework and contains test cases.

Running locally

Prerequisites:
- .NET 8 SDK
- npm is NOT required when using the Microsoft.Playwright.CLI tool, but the CLI will download browser binaries.

Steps:
1. Restore and build:
   dotnet restore
   dotnet build -c Release

2. Install Playwright browsers (required once):
   dotnet tool install --tool-path ./.tools Microsoft.Playwright.CLI --version 1.60.0
   ./.tools/playwright install --with-deps

3. Run tests:
   dotnet test Playwright.Tests/Playwright.Tests.csproj -c Release

CI

A GitHub Actions workflow is provided at .github/workflows/ci.yml. It restores, builds, installs Playwright browsers and runs tests on ubuntu-latest.

Notes
- If you open the solution in Visual Studio, add the Playwright.Framework project to the solution if it is not visible (right-click solution -> Add -> Existing Project -> Playwright.Framework\Playwright.Framework.csproj).
