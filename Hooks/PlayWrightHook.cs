using Allure.Net.Commons;
using Microsoft.Playwright;
using PlaywrightPoc.Utils;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PlaywrightPoc.Hooks
{
    [Binding]
    public class PlayWrightHook
    {
        ScenarioContext _scenarioContext;
        IBrowser browser;

        public object AttachToAllure { get; private set; }

        public PlayWrightHook(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "playwrightConfig.json");
            List<PlaywrightConfig>configData=DataReaderFile.GetJsonData(path);

            // Load selected environment (from NUnit params `ENV` or env var `TEST_ENVIRONMENT`)
            string envPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestData", "EnvironmentConfig.json");
            EnvironmentManager.Load(envPath);
            // No hard-coded values: store selected Playwright config in scenario context for access in AfterScenario
            var cfg = configData[0];
            _scenarioContext.Add("playwrightConfig", cfg);

            var playW = await Playwright.CreateAsync();
            var Request =await playW.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL=cfg.BaseURL
                 //BaseURL=cfg.TokenBaseURL
            });
            var browserOptions = new BrowserTypeLaunchOptions
            {
                Channel= cfg.Channel,
                Headless = cfg.Headless,
                Args=new List<string> { cfg.Args }
            };
           
            browser = await playW.Chromium.LaunchAsync(browserOptions);
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = ViewportSize.NoViewport
            });

            // Start tracing on the context if configured (trace will be stopped on failure in AfterScenario)
            if (cfg.TraceOnFailure)
            {
                await context.Tracing.StartAsync(new TracingStartOptions
                {
                    Screenshots = true,
                    Snapshots = true,
                    Sources = true
                });
            }

            var page = await context.NewPageAsync();

            // Save context and page so AfterScenario can access them without hard-coded paths or values
            _scenarioContext.Add("context", context);
            _scenarioContext.Add("page", page);
            _scenarioContext.Add("apiRequest", Request);

            // Initialize step-data collector for this scenario (config-driven masking will be handled there)
            StepDataCollector.Init(_scenarioContext);
        }
        [AfterScenario]
        public async Task AfterScenario()
        {
            var page = _scenarioContext.Get<IPage>("page");
            var context = _scenarioContext.Get<IBrowserContext>("context");
            var cfg = _scenarioContext.Get<PlaywrightConfig>("playwrightConfig");
            var apiRequest = _scenarioContext.Get<IAPIRequestContext>("apiRequest");

            string name = _scenarioContext.ScenarioInfo.Title;
            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // If scenario failed, collect artifacts according to configuration (no hard-coded directories)
            if (_scenarioContext.TestError != null)
            {
                // Screenshots
                if (cfg.ScreenshotsOnFailure)
                {
                    var screenshotsDir = cfg.ScreenshotDir ?? "Screen_shots";
                    Directory.CreateDirectory(Path.Combine(baseDir, screenshotsDir));
                    string screenshotFormat = cfg.ScreenshotFileNameFormat ?? "{scenario}_{timestamp}.png";
                    string screenshotName = screenshotFormat.Replace("{scenario}", SanitizeFileName(name)).Replace("{timestamp}", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                    string screenshotPath = Path.Combine(baseDir, screenshotsDir, screenshotName);

                    await page.ScreenshotAsync(new PageScreenshotOptions
                    {
                        Path = screenshotPath,
                        FullPage = true
                    });

                    AllureApi.AddAttachment("Failure Screenshot", "image/png", screenshotPath);
                }

                // Tracing
                if (cfg.TraceOnFailure && context != null)
                {
                    var tracesDir = cfg.TracesDir ?? "Traces";
                    Directory.CreateDirectory(Path.Combine(baseDir, tracesDir));
                    string traceFormat = cfg.TraceFileNameFormat ?? "{scenario}_{timestamp}.zip";
                    string traceName = traceFormat.Replace("{scenario}", SanitizeFileName(name)).Replace("{timestamp}", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                    string tracePath = Path.Combine(baseDir, tracesDir, traceName);

                    // Stop tracing and save to the configured path
                    await context.Tracing.StopAsync(new TracingStopOptions
                    {
                        Path = tracePath
                    });

                    AllureApi.AddAttachment("Playwright Trace", "application/zip", tracePath);
                }

                // Step data collection: attach recorded step inputs as JSON (if any)
                try
                {
                    var stepJson = StepDataCollector.ToJson(_scenarioContext);
                    if (!string.IsNullOrWhiteSpace(stepJson) && stepJson != "[]")
                    {
                        var artifactsDir = cfg.TracesDir ?? "Traces";
                        Directory.CreateDirectory(Path.Combine(baseDir, artifactsDir));
                        string stepDataName = SanitizeFileName(name) + "_stepdata_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
                        string stepDataPath = Path.Combine(baseDir, artifactsDir, stepDataName);
                        File.WriteAllText(stepDataPath, stepJson);
                        AllureApi.AddAttachment("Step Data", "application/json", stepDataPath);
                    }
                }
                catch { /* non-fatal - do not break teardown on reporting errors */ }
            }

            // Close browser context and browser instance gracefully
            if (context != null)
                await context.CloseAsync();

            if (browser != null)
                await browser.CloseAsync();
            
            if (apiRequest != null)
                await apiRequest.DisposeAsync();
        }

        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "artifact";
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Replace(' ', '_');
        }
    }
}
