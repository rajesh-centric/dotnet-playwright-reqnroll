using PlaywrightPoc.Pages;
using PlaywrightPoc.Utils;
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

     
        [When("I perform a search for \"(.*)\"")]
        public async Task WhenIPerformASearchFor(string value)
        {
            // record search term used for reporting
            StepDataCollector.Add(_scenarioContext, "Search", "query", value ?? string.Empty);
            await sPage.PerformSearchOnOrangeHRM(value);
        }

        [Then("I should see search results contain \"(.*)\"")]
        public async Task ThenIShouldSeeSearchResultsContain(string expectedText)
        {
            await sPage.AssertSearchResultsContain(expectedText);
        }

    }
}
