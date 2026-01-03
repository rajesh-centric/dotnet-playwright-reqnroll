using Microsoft.Playwright;
using PlaywrightPoc.Utils;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PlaywrightPoc.Hooks
{
    [Binding]
    public class PlayWrightHook
    {
        ScenarioContext _scenarioContext;

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
            var playW = await Playwright.CreateAsync();
            var browserOptions = new BrowserTypeLaunchOptions
            {
                Channel= configData[0].Channel,
                Headless = configData[0].Headless,
                Args=new List<string> { configData[0].Args }
            };
           
            var browser = await playW.Chromium.LaunchAsync(browserOptions);
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = ViewportSize.NoViewport
            });
            var page = await context.NewPageAsync();
            _scenarioContext.Add("page", page);
        }

    }
}
