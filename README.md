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

Environment-specific appsettings
- This repo supports environment-specific appsettings files. Files supported:
  - appsettings.json (defaults)
  - appsettings.Local.json (local development)
  - appsettings.CI.json (CI runs)
- The test runner reads ASPNETCORE_ENVIRONMENT or TEST_ENVIRONMENT to pick the environment-specific file. If neither is set, Local is used by default.
- To run locally with Local settings (headed by default): ensure TEST_ENVIRONMENT is not set or set it to Local.
- In CI the workflow sets TEST_ENVIRONMENT=CI so appsettings.CI.json is loaded automatically.
 - Note: Headless mode for CI is controlled by the workflow's MODE variable (and the Headless environment variable exposed to tests).
   For clarity, appsettings.CI.json does not set Headless; use the workflow input or env to switch headed vs headless runs.

CI

A GitHub Actions workflow is provided at .github/workflows/ci.yml. It restores, builds, installs Playwright browsers and runs tests on ubuntu-latest.

Notes
- If you open the solution in Visual Studio, add the Playwright.Framework project to the solution if it is not visible (right-click solution -> Add -> Existing Project -> Playwright.Framework\Playwright.Framework.csproj).

GitHub Pages report
- The CI workflow generates a Playwright HTML report (when traces are produced) and publishes it to the repository's gh-pages branch.
- The report will be available at: https://<owner>.github.io/<repo>/ (and /index.html). The workflow will post a direct link on pull requests when a report is deployed.
- Ensure GitHub Pages is enabled for the repository and that workflows are allowed to create commits (GITHUB_TOKEN needs push permission) so the report can be published.
