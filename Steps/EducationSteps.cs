using System.Collections.Immutable;
using Reqnroll;
using qa_dotnet_cucumber.Pages;
using qa_dotnet_cucumber.Entity;
using qa_dotnet_cucumber.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    [Scope(Feature = "Education")]
    public class EducationSteps
    {
        private readonly LoginPage _loginPage;
        private readonly NavigationHelper _navigationHelper;
        private readonly ScenarioContext _scenarioContext;
        private readonly EducationPage _educationPage;

        public EducationSteps(LoginPage loginPage, NavigationHelper navigationHelper, ScenarioContext scenarioContext, EducationPage educationPage)
        {
            _loginPage = loginPage;
            _navigationHelper = navigationHelper;
            _scenarioContext = scenarioContext;
            _educationPage = educationPage;
        }

        [Given("I navigate to the profile page as a registered user")]
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            _loginPage.Login("ambikaarumugams@gmail.com", "AmbikaSenthil123");
            _educationPage.NavigateToTheProfilePage();
        }


        [When("I enter education details from json file with the TestName {string}")]
        public void WhenIEnterEducationDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<TestFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);

            if (scenario != null)
            {
                var actualEducationList = new List<string>();
                var cleanupList = new List<string>();

                foreach (var testItem in scenario.TestItems)
                {
                    var details = testItem.EducationDetails;
                    _educationPage.AddEducationDetails(details.CollegeUniversityName, details.Country, details.Title, details.Degree, details.YearOfGraduation);
                    var successMessage = _educationPage.GetSuccessMessage();
                    actualEducationList.Add(successMessage);

                    cleanupList.Add(details.CollegeUniversityName);
                }
                _scenarioContext.Set(cleanupList, "EducationToCleanup");
                _scenarioContext.Set(actualEducationList, "ActualSuccessMessageList");
            }
        }

        [Then("I should see the success message")]
        public void ThenIShouldSeeTheSuccessMessage()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualSuccessMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(MessageConstants.SuccessMessageForAdd), $"Actual and Expected are mismatched!");
            }
        }

        [When("I enter invalid education details from json file with the TestName {string}")]
        public void WhenIEnterInvalidEducationDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<TestFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);

            if (scenario != null)
            {
                var messageResults = new List<(string Message, string Type)>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var details = testItem.EducationDetails;
                    _educationPage.AddEducationDetails(details.CollegeUniversityName, details.Country, details.Title, details.Degree, details.YearOfGraduation);
                    var (messageText, messageType) = _educationPage.GetToastMessage();
                    messageResults.Add((messageText, messageType));
                    Thread.Sleep(5000);
                    if (string.Equals(messageType, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(details.CollegeUniversityName);
                    }
                }
                _scenarioContext.Set(messageResults, "ActualMessageList");
                _scenarioContext.Set(cleanUpList, "EducationToCleanup");
            }
        }

        [Then("I should see the error message")]
        public void ThenIShouldSeeTheErrorMessage()
        {
            var messageResults = _scenarioContext.Get<List<(string Message, string Type)>>("ActualMessageList");
            Assert.Multiple(() =>
            {
                foreach (var (message, type) in messageResults)
                {
                    Assert.That(type, Is.EqualTo("error"), "Error message should be shown, But was success!!!");
                    Assert.That(message, Is.EqualTo(MessageConstants.ErrorMessage), $"Message {MessageConstants.ErrorMessage} is expected.");
                }
            });

        }


        [Then("I should see the error message for adding huge string")]
        public void ThenIShouldSeeTheErrorMessageForAddingHugeString()
        {
            var actualMessages = _scenarioContext.Get<List<string>>("ActualSuccessMessageList");
            foreach (var actualMessage in actualMessages)
            {
                Assert.That(actualMessage, Is.EqualTo(MessageConstants.ErrorMessage), $"Expected message is {MessageConstants.ErrorMessage}, but found {actualMessage}");
            }
        }

        [When("I leave either one or all the fields empty and give the data from json file with the TestName {string}")]
        public void WhenILeaveEitherOneOrAllTheFieldsEmptyAndGiveTheDataFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<TestFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);

            if (scenario != null)
            {
                var actualMessageList = new List<string>();

                foreach (var testItem in scenario.TestItems)
                {
                    var details = testItem.EducationDetails;
                    _educationPage.LeaveEitherOneOrAllTheFieldsEmpty(details.CollegeUniversityName, details.Country, details.Title, details.Degree, details.YearOfGraduation);
                    var message = _educationPage.GetMessage();
                    actualMessageList.Add(message);
                }
                _scenarioContext.Set(actualMessageList, "ActualErrorMessage");
            }
        }

        [Then("I should see the error message for empty fields")]
        public void ThenIShouldSeeTheErrorMessageForEmptyFields()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualErrorMessage");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(MessageConstants.ErrorMessageForEmptyField), $"Expected message is {MessageConstants.ErrorMessageForEmptyField}, but not found");
            }
        }

        [When("I enter same education details twice from json file with the TestName {string}")]
        public void WhenIEnterSameEducationDetailsTwiceFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<TestFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);
            var messageResults = new List<(string Message, string Type)>();
            var cleanUpList = new List<string>();
            if (scenario != null)
            {
                foreach (var testItem in scenario.TestItems)
                {
                    var details = testItem.EducationDetails;
                    _educationPage.AddEducationDetails(details.CollegeUniversityName, details.Country, details.Title, details.Degree, details.YearOfGraduation);
                    var (messageText, messageType) = _educationPage.GetToastMessage();
                    messageResults.Add((messageText, messageType));
                    Thread.Sleep(5000);
                    if (string.Equals(messageType, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(details.CollegeUniversityName);
                    }

                    messageResults.Remove(("Education has been added","success"));
                }
            }
            _scenarioContext.Set(messageResults, "ActualMessageList");
            _scenarioContext.Set(cleanUpList, "EducationToCleanup");
        }

        [Then("I should see the error message for duplicate data")]
        public void ThenIShouldSeeTheErrorMessageForDuplicateData()
        {
            var messageResults = _scenarioContext.Get<List<(string Message, string Type)>>("ActualMessageList");
            Assert.Multiple(() =>
            {
                foreach (var (message, type) in messageResults)
                {
                    Assert.That(type, Is.EqualTo("error"), $"Expected message type should be {type},but found error");
                    Assert.That(message, Is.EqualTo(MessageConstants.ErrorMessageForAddingDuplicateDetails), $"Message {MessageConstants.ErrorMessageForAddingDuplicateDetails} is expected.");
                }
            });
        }

        [When("I enter education details from json file after the session has expired with the TestName {string}")]
        public void WhenIEnterEducationDetailsFromJsonFileAfterTheSessionHasExpiredWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<TestFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);

            if (scenario != null)
            {
                var actualMessageList = new List<string>();
               
                foreach (var testItem in scenario.TestItems)
                {
                    var details = testItem.EducationDetails;
                    _educationPage.ExpireSession();
                    _educationPage.AddEducationDetails(details.CollegeUniversityName, details.Country, details.Title, details.Degree, details.YearOfGraduation);
                    var errorMessage = _educationPage.GetErrorMessage();
                    actualMessageList.Add(errorMessage);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
            }
        }

        [Then("I should see the error message for session expired")]
        public void ThenIShouldSeeTheErrorMessageForSessionExpired()
        {
            var actualList =_scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual,Is.EqualTo(MessageConstants.ErrorMessageForSessionExpired),$"Expected message is {MessageConstants.ErrorMessageForSessionExpired}, but it wasn't found");
            }
        }
    }
}
