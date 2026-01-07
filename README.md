# Reqnroll + Playwright Test Project

A .NET test automation project using **Reqnroll** (BDD), **NUnit**, and **Microsoft Playwright**, with **Allure** reporting and **Coverlet** code coverage.

---

## Tech Stack

- **Target Framework:** `net10.0`
- **Language Version:** `latest`
- **Nullable Reference Types:** enabled
- **Implicit Usings:** enabled
- **Test Framework:** NUnit (`NUnit`, `NUnit3TestAdapter`, `NUnit.Analyzers`)
- **BDD:** Reqnroll (`Reqnroll`, `Reqnroll.NUnit`)
- **UI Automation:** Microsoft.Playwright
- **Reporting:** Allure.Reqnroll
- **Coverage:** coverlet.collector
- **Test SDK:** Microsoft.NET.Test.Sdk

Package versions (from the project file):

| Package | Version |
|---|---|
| Allure.Reqnroll | 2.14.1 |
| coverlet.collector | 6.0.4 |
| Microsoft.NET.Test.Sdk | 17.14.0 |
| Microsoft.Playwright | 1.57.0 |
| NUnit | 4.3.2 |
| NUnit.Analyzers | 4.7.0 |
| NUnit3TestAdapter | 5.0.0 |
| Reqnroll | 3.3.0 |
| Reqnroll.NUnit | 3.3.0 |

---

## Project Structure

```
<repo-root>/
  Features/            # Gherkin feature files (.feature)
  Screen_shots/        # Saved screenshots
  playwrightConfig.json
  TestData/
    ApplicationData.json
```

> `playwrightConfig.json` and `TestData/ApplicationData.json` are copied to the output directory during build/run.

---

## Prerequisites

- **.NET SDK** matching `net10.0` (or newer that supports `net10.0`).
- **Microsoft Playwright** browsers installed for the project.
- (Optional) **Allure Commandline** for generating interactive HTML reports.

---

## Getting Started

1. **Restore dependencies**

```bash
dotnet restore
```

2. **Install Playwright browsers** (once per machine):

```bash
dotnet tool restore  # if you use local tools
pwsh -c "npx playwright install"  # or
playwright install                 # if Playwright CLI is available
```

> Alternatively, use `pwsh -c "npx playwright install --with-deps"` on Linux containers.

3. **Run tests**

```bash
dotnet test -v normal
```

### Run tests with code coverage (Coverlet)

```bash
dotnet test \
  --collect:"XPlat Code Coverage" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
```

Coverage results will be placed under `TestResults/<guid>/` as `coverage.cobertura.xml` (or chosen format).

### Run tests with Allure reporting

Allure.Reqnroll integrates with your Reqnroll/NUnit executions to produce results in `allure-results/`.

```bash
dotnet test
```

Then generate and open the report:

```bash
# If Allure is installed globally
allure generate --clean allure-results -o allure-report
allure open allure-report
```

---

## Configuration

- **`playwrightConfig.json`** – Playwright project configuration (e.g., browser, viewport, timeout).
- **`TestData/ApplicationData.json`** – App/test data consumed by steps.

Both files are marked with `CopyToOutputDirectory=Always`, ensuring they are available next to the test binaries.

---

## NUnit & Reqnroll

- Add step definitions under your test project, and Gherkin scenarios under `Features/`.
- NUnit attributes (e.g., `[Test]`, `[SetUp]`, `[TearDown]`) are available via `using NUnit.Framework;`.
- Reqnroll hooks (`[BeforeTestRun]`, `[AfterScenario]`, etc.) can be used to manage Playwright lifecycle (browser/page creation, screenshots on failure, etc.).

---

## Playwright Tips

- Use a shared **Browser**/\*new-context-per-scenario\* pattern to isolate tests.
- Capture screenshots to `Screen_shots/` on failures for quick triage.
- Keep selectors resilient; prefer `getByRole`, `getByTestId`, etc.

---

## Scripts & Examples (suggested)

You may add the following helper scripts:

- **Windows (PowerShell):** `scripts/run-tests.ps1`
- **Linux/macOS (bash):** `scripts/run-tests.sh`

Each can orchestrate Playwright install, test execution, coverage, and Allure report generation.

---

## Troubleshooting

- **Playwright browsers not found:** run `playwright install` (or `npx playwright install`).
- **Allure report not generated:** ensure Allure CLI is installed and that `allure-results/` has files after test run.
- **Coverage file missing:** confirm `--collect:"XPlat Code Coverage"` is used and check `TestResults/`.
- **Target framework issues:** ensure your .NET SDK supports `net10.0`.

---

## License

This project is currently **not packable** (`IsPackable=false`); add licensing information here as applicable.

---

## Credits

- Reqnroll BDD framework
- NUnit testing framework
- Microsoft Playwright for browser automation
- Allure for reporting
- Coverlet for coverage

