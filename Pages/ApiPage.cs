using System;
using Microsoft.Playwright;
using Reqnroll;

namespace PlaywrightPoc.Pages;

public class ApiPage
{
    ScenarioContext _scenarioContext;
        IAPIRequestContext request;
        IPage _page;

        public ApiPage(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            request = _scenarioContext.Get<IAPIRequestContext>("apiRequest");
            _page = _scenarioContext.Get<IPage>("page");
        }
    
    public async Task SetGetApiResponse(string apiEndpoint)
    {
        var response = await request.GetAsync(apiEndpoint);
        //var body=await response.JsonAsync();
        _scenarioContext.Set(response, "apiResponse");
        //_scenarioContext.Set(body, "apiResponseBody");        
    }

    public async Task SetDeleteApiResponse(string apiEndpoint)
    {
        var response = await request.DeleteAsync(apiEndpoint);
        var body=await response.JsonAsync();
        _scenarioContext.Set(response, "apiResponse");
        _scenarioContext.Set(body, "apiResponseBody");       
    }

    public async Task ValidateStatusCode(string expectedStatusCode)
    {
        var response = _scenarioContext.Get<IAPIResponse>("apiResponse");
        await Assertions.Expect(response).ToBeOKAsync();//ToBe(int.Parse(expectedStatusCode));
    }

    public async Task SetPostApiResponse(string apiEndpoint)
    {
        var headers = new Dictionary<string, string>
        {
            { "Accept", "application/json" },
            { "Content-Type", "application/json" }            
        };
        await _page.SetExtraHTTPHeadersAsync(headers);
        var data = new Dictionary<string, object>
        {
            { "name", "TestName" },
            { "Number", 1 }
        };
        var response = await request.PostAsync(apiEndpoint, new APIRequestContextOptions 
        { 
            DataObject = data
        });
        var body=await response.JsonAsync();
        _scenarioContext.Set(response, "apiResponse");
        _scenarioContext.Set(body, "apiResponseBody");
        //await Assertions.Expect(response).ToBeOKAsync();        
    }
    public async Task SetGetTokenApiResponse(string apiEndpoint)
    {
         var headers = new Dictionary<string, string>
        {
            { "Accept", "application/json" },
            { "Content-Type", "application/json" }            
        };
        await _page.SetExtraHTTPHeadersAsync(headers);
         var data = new Dictionary<string, object>
        {
            { "username", "emilys" },
            { "password", "emilyspass" },
            {"expiresInMins", 30 }
        };
        var response = await request.PostAsync(apiEndpoint, new APIRequestContextOptions 
        { 
            DataObject = data
        });
        var accessKey= response.JsonAsync().Result.Value.GetProperty("accessToken").GetRawText();
        
        _scenarioContext.Set(accessKey, "accessToken");        
    }
public async Task SetGetApiResponseToken(string apiEndpoint)
    {
         var headers = new Dictionary<string, string>
        {
            { "Accept", "application/json" },
            { "Content-Type", "application/json" }  ,
            {"Authorization",  _scenarioContext.Get<string>("accessToken") }          
        };
        await _page.SetExtraHTTPHeadersAsync(headers);
        var response = await request.GetAsync(apiEndpoint);
        var body=await response.JsonAsync();
        _scenarioContext.Set(response, "apiResponse");
        _scenarioContext.Set(body, "apiResponseBody");        
    }   
}
