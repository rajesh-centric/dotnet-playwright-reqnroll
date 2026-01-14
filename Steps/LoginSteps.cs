using Microsoft.Playwright;
using PlaywrightPoc.Pages;
using PlaywrightPoc.TestData;
using PlaywrightPoc.Utils;
using Reqnroll;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PlaywrightPoc.Steps
{
    [Binding]
    public class LoginSteps
    {
        public LoginPage lPage;
        public CommonFunction commonFunction;
        ScenarioContext _scenarioContext;
        EnvironmentData envData;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            lPage = new LoginPage(_scenarioContext);
            commonFunction = new CommonFunction(_scenarioContext);
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
               "TestData", "EnvironmentConfig.json");
            EnvironmentManager.Load(path);
            envData = EnvironmentManager.Current;
        }


        [Given("I navigate to the login page with url (.*)")]
        public async Task GivenINavigateToTheLoginPagewithurlAsync(string url)
        {
            if(url.Equals("AppUrl"))
            {
                url = envData?.AppUrl;
            }
            // record used value for reporting
            StepDataCollector.Add(_scenarioContext, "Navigate", "url", url ?? string.Empty);
            await lPage.NavigateAsync(url);
        }

        [When("I enter username (.*)")]
        public async Task WhenIEnterUsernameAsync(string username)
        {
            if (username.Equals("UserName"))
            {
                username = envData?.UserName;
            }
            // record username used in this step (masking is handled by collector if configured)
            StepDataCollector.Add(_scenarioContext, "EnterUsername", "username", username ?? string.Empty);
            await lPage.EnterUsername(username);
        }

        [When("I enter password (.*)")]
        public async Task WhenIEnterPasswordAsync(string password)
        {
            if (password.Equals("Password"))
            {
                password = envData?.Password;
            }
            // record password used in this step (collector will mask if configured)
            StepDataCollector.Add(_scenarioContext, "EnterPassword", "password", password ?? string.Empty);
            await lPage.EnterPassword(password);
        }

        [When("I click login")]
        public async Task WhenIClickLoginAsync()
        {
            await lPage.ClickLogin();
        }

        [Then("I should see the dashboard")]
        public async Task ThenIShouldSeeTheDashboardAsync()
        {
            
            await commonFunction.IsTitleVisible("OrangeHRM");
           // Assert.That(title, Does.Contain("OrangeHRM"));
        }

        [Then("I should see login error message Invalid Credential")]
        public async Task ThenIShouldSeeLoginErrorMessageInvalidCredential()
        {
            await lPage.IsErrorAvailable();
            //Assert.That(await lPage.IsErrorAvailable(), Is.True, "Unable to see the error message");
        }


        [Then("I should remain on the login page")]
        public async Task ThenIShouldRemainOnTheLoginPage()
        {
            await lPage.IsLoginPageVisible();
        }

        [Given("I click forgot password button")]
        public async Task GivenIClickForgotPasswordButton()
        {
            await lPage.ClickForgotPass();
        }

        [Then("I should see Reset Password screen")]
        public async Task ThenIShouldSeeResetPasswordScreen()
        {
            await lPage.IsTextVisible();
        }

    }


}

