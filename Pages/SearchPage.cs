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

        string searchBoxXpath = "//textarea[@name='q']";

        public async Task NavigateToPage(string url)
        {            
            await page.GotoAsync(url);
        }

        public async Task FillSearchBox(string value)
        {
            await page.FillAsync(searchBoxXpath, value);
            Thread.Sleep(1000);
            await page.Keyboard.PressAsync("Enter");
        }
    }
}
