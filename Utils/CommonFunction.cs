using Microsoft.Playwright;
using PlaywrightPoc.Pages;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightPoc.Utils
{
    public class CommonFunction
    {
        
        ScenarioContext _scenarioContext;
        IPage page;

        public CommonFunction(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            page = _scenarioContext.Get<IPage>("page");
        }


        public async Task IsTitleVisible(string titleValue)
        {
            await Assertions.Expect(page).ToHaveTitleAsync(titleValue);
        }
    }
}
