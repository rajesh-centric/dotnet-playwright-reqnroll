using System;
using Microsoft.Playwright;
using Reqnroll;

namespace PlaywrightPoc.Pages;

public class ApiPage
{
    ScenarioContext _scenarioContext;
        IAPIRequestContext request;

        public ApiPage(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            request = _scenarioContext.Get<IAPIRequestContext>("apiRequest");
        }
    
    public async Task IsGetUserApiStatusCode500()
    {
        var response = await request.GetAsync("/users/2");
        await Assertions.Expect(response).ToBeOKAsync();
        
    }

}
