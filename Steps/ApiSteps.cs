using System;
using System.Reflection;
using PlaywrightPoc.Pages;
using PlaywrightPoc.TestData;
using PlaywrightPoc.Utils;
using Reqnroll;

namespace PlaywrightPoc.Steps;

[Binding]
public class ApiSteps
{
    ScenarioContext _scenarioContext;
        EnvironmentData envData;
        CommonFunction commonFunction;
        ApiPage apiPage;
        

        public ApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
           
            commonFunction = new CommonFunction(_scenarioContext);
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
               "TestData", "EnvironmentConfig.json");
            EnvironmentManager.Load(path);
            envData = EnvironmentManager.Current;
            apiPage = new ApiPage(_scenarioContext);
        }

        [When("I requested the get (.*) API")]
        public async Task GetApiRequestStatus(string apiRequest)
        {
            // record username used in this step (masking is handled by collector if configured)
            StepDataCollector.Add(_scenarioContext, "ValidateApiCode", "API Code", apiRequest ?? string.Empty);
           await apiPage.SetGetApiResponse(apiRequest);
        }

        [Then("I should get status code (.*)")]
        public async Task GetApiRequestStatusCode(string code)
        {
            // record username used in this step (masking is handled by collector if configured)
            StepDataCollector.Add(_scenarioContext, "ValidateApiCode", "API Code", code ?? string.Empty);
           await apiPage.ValidateStatusCode(code);
        }

        [When("I requested the post (.*) API")]
        public async Task PostApiRequestStatus(string apiRequest)
        {
            // record username used in this step (masking is handled by collector if configured)
            StepDataCollector.Add(_scenarioContext, "Validate Post Api", "API Code", apiRequest ?? string.Empty);
           await apiPage.SetPostApiResponse(apiRequest);
        }
//I requested the Delete /delete api
        [When("I requested the Delete (.*) api")]
        public async Task DeleteApiRequestStatus(string apiRequest)
        {
            // record username used in this step (masking is handled by collector if configured)
            StepDataCollector.Add(_scenarioContext, "Validate Delete Api", "API Code", apiRequest ?? string.Empty);
           await apiPage.SetDeleteApiResponse(apiRequest);
        }
        [When("I saved the token from the api (.*)")]
        public async Task SaveToken(string apiRequest)
        {
            await apiPage.SetGetTokenApiResponse(apiRequest);
        }
        [When("I saved the response of the Get api (.*)")]
        public async Task SaveGetResponse(string apiRequest)
        {
            await apiPage.SetGetApiResponseToken(apiRequest);
        }

}
