using Microsoft.Playwright;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightPoc.Pages
{
    public class SearchPage
    {
        ScenarioContext _scenarioContext;
        IPage page;

        public SearchPage(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            page = _scenarioContext.Get<IPage>("page");

        }

        // Perform a search on OrangeHRM if a search input is available; otherwise do nothing.
        string orangeSearchXpath = "//input[contains(@placeholder,'Search') or contains(@aria-label,'Search')]";
        public async Task PerformSearchOnOrangeHRM(string value)
        {
            var locator = page.Locator(orangeSearchXpath);
            if (await locator.CountAsync() > 0)
            {
                await locator.FillAsync(value);
                await page.Keyboard.PressAsync("Enter");
            }
            // If the search control is not available, leave the page as-is; assertion below will fail deliberately.
        }

        public async Task AssertSearchResultsContain(string expectedText)
        {
            // Intentionally assert that the page body contains the text to force a failing test.
            await Assertions.Expect(page.Locator("body")).ToContainTextAsync(expectedText);
        }
    }
}
