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
        ScenarioContext _scenarioContext;
        List<ApplicationData> appData;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            lPage = new LoginPage(_scenarioContext);
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
               "TestData", "ApplicationData.json");
            appData = DataReaderFile.GetAppData(path);
        }


        [Given("I navigate to the login page with url (.*)")]
        public async Task GivenINavigateToTheLoginPagewithurlAsync(string url)
        {
            if(url.Equals("AppUrl"))
            {
                url = appData[0].AppUrl;
            }
            await lPage.NavigateAsync(url);
        }

        [When("I enter username (.*)")]
        public async Task WhenIEnterUsernameAsync(string username)
        {
            if (username.Equals("UserName"))
            {
                username = appData[0].UserName;
            }
            await lPage.EnterUsername(username);
        }

        [When("I enter password (.*)")]
        public async Task WhenIEnterPasswordAsync(string password)
        {
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
            string title = await lPage.TitleAsync();
            Assert.That(title, Does.Contain("OrangeHRM"));
        }

        [Then("I should see login error message Invalid Credential")]
        public async Task ThenIShouldSeeLoginErrorMessageInvalidCredential()
        {
            Assert.That(await lPage.IsErrorAvailable(), Is.True, "Unable to see the error message");
        }


        [Then("I should remain on the login page")]
        public void ThenIShouldRemainOnTheLoginPage()
        {
            
        }


    }


}

