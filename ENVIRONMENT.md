# Environment Profiles (runtime selection) ✅

This project supports multiple environment profiles so you can select which environment the tests run against at runtime.

Files:
- `TestData/EnvironmentConfig.json` — list of environment profiles (Name, AppUrl, UserName, Password).
- `Utils/EnvironmentManager.cs` — loads the selected profile (NUnit parameter or environment variable) and exposes `EnvironmentManager.Current`.

How to select an environment at runtime:

- Using an environment variable (PowerShell):
  - $env:TEST_ENVIRONMENT = 'qa'
  - dotnet test

- Using NUnit test parameter (when running with NUnit console runner):
  - --params:ENV=qa

Notes:
- The default environment is `dev` if none is provided.
- Add or update profiles in `TestData/EnvironmentConfig.json` following the JSON structure.

Example profile entry:

{
  "Name": "qa",
  "AppUrl": "https://qa.example.com/web/index.php/auth/login",
  "UserName": "qauser",
  "Password": "qapass"
}
