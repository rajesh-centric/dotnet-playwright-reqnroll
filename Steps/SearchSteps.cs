using PlaywrightPoc.Pages;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightPoc.Steps
{
    [Binding]
    public class SearchSteps
    {
        public SearchPage sPage;
        ScenarioContext _scenarioContext;

        public SearchSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            sPage = new SearchPage(_scenarioContext);
        }

        [Given("User navigates to the url (.*)")]
        public async Task GivenUserNavigatesToTheUrl(string urlValue)
        {
            await sPage.NavigateToPage(urlValue);
        }

        [When("User enters the value (.*) on Search Page")]
        public async Task WhenUserEntersTheValuePlaywrightOnSearchPage(string value)
        {
            await sPage.FillSearchBox(value);
        }

    }
}
