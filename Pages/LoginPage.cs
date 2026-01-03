using Microsoft.Playwright;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightPoc.Pages
{
    public class LoginPage
    {
        ScenarioContext _scenarioContext;
        IPage page;

        public LoginPage(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            page = _scenarioContext.Get<IPage>("page");
        }

        string usernameXpath = "//input[@name='username']";
        public string passwordXpath = "//input[@name='password']";
        string loginBtnXpath = "//button[@class='oxd-button oxd-button--medium oxd-button--main orangehrm-login-button']";
        string errorMessageXpath = "//p[text()='Invalid credentials']"; 

        public async Task NavigateAsync(string url) => await page.GotoAsync(url);
        public async Task EnterUsername(string username) => await page.FillAsync(usernameXpath, username);
        public async Task EnterPassword(string password) => await page.FillAsync(passwordXpath, password);
        public async Task ClickLogin() => await page.ClickAsync(loginBtnXpath);

        public async Task<string> TitleAsync() => await page.TitleAsync();

        public async Task<bool> IsErrorAvailable()
        {
            Thread.Sleep(5000);
            return await page.IsVisibleAsync(errorMessageXpath);
        }
        
            
 
    }
}
