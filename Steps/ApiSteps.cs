using System;
using System.Reflection;
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
        

        public ApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
           
            commonFunction = new CommonFunction(_scenarioContext);
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
               "TestData", "EnvironmentConfig.json");
            EnvironmentManager.Load(path);
            envData = EnvironmentManager.Current;
        }

        [When("I requested the get user API status code is (.*)")]
        public async Task GetApiCodeStatus(string apiCode)
        {
            // record username used in this step (masking is handled by collector if configured)
            StepDataCollector.Add(_scenarioContext, "ValidateApiCode", "API Code", apiCode ?? string.Empty);
           
        }

}
