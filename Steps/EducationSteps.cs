using AngleSharp.Text;
using Reqnroll;
using qa_dotnet_cucumber.Pages;
using qa_dotnet_cucumber.Entity;
using qa_dotnet_cucumber.Helper;
using qa_dotnet_cucumber.MessageConstants;

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

        public EducationSteps(LoginPage loginPage, NavigationHelper navigationHelper, ScenarioContext scenarioContext,EducationPage educationPage) //Constructor
        {
            _loginPage = loginPage;
            _navigationHelper = navigationHelper;
            _scenarioContext = scenarioContext;
            _educationPage = educationPage;
        }

        [Given("I navigate to the profile page as a registered user")]  //Login using Json test data
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            var loginDetails = JsonHelper.LoadJson<LoginDetails>("LoginData");  
            var username = loginDetails.UserName;
            var password = loginDetails.Password;
            _loginPage.Login(username, password);
            _educationPage.NavigateToTheProfilePage();
        }

        [When("I enter education details from json file with the TestName {string}")]  //Add education details, >250 characters length, negative testing with valid input
        public void WhenIEnterEducationDetailsFromJsonFileWithTheTestName(string scenarioName) 
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName,scenarioName,StringComparison.OrdinalIgnoreCase));  //Filter the scenario using the scenario name 

            if (scenario != null)
            {
                var actualEducationList = new List<string>();
                var cleanupList = new List<string>();

                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.EducationDetails;
                    _educationPage.AddEducationDetails(detailsToAdd.CollegeUniversityName, detailsToAdd.Country, detailsToAdd.Title, detailsToAdd.Degree, detailsToAdd.YearOfGraduation);
                    var successMessage = _educationPage.GetSuccessMessage();
                    actualEducationList.Add(successMessage);
                    cleanupList.Add(detailsToAdd.CollegeUniversityName);
                }
                _scenarioContext.Set(cleanupList, "EducationToCleanup");
                _scenarioContext.Set(actualEducationList, "ActualMessageList");
            }
        }

        [Then("I should see the success message")] //Validation for valid input
        public void ThenIShouldSeeTheSuccessMessage()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.SuccessMessageForAdd), $"Actual and Expected are mismatched!"); 
            }
        }

        [When("I enter invalid education details from json file with the TestName {string}")] //Add invalid input
        public void WhenIEnterInvalidEducationDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));

            if (scenario != null)
            {
                var messageResults = new List<(string Message, string Type)>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.EducationDetails;
                    _educationPage.AddEducationDetails(detailsToAdd.CollegeUniversityName, detailsToAdd.Country, detailsToAdd.Title, detailsToAdd.Degree, detailsToAdd.YearOfGraduation);
                    var (messageText, messageType) = _educationPage.GetToastMessage();
                    messageResults.Add((messageText, messageType));
                    Thread.Sleep(5000);
                    if (string.Equals(messageType, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(detailsToAdd.CollegeUniversityName);
                    }
                }
                _scenarioContext.Set(messageResults, "ActualMessageList");
                _scenarioContext.Set(cleanUpList, "EducationToCleanup");
            }
        }

        [Then("I should see the error message")]  //Validation for invalid input
        public void ThenIShouldSeeTheErrorMessage()
        {
            var messageResults = _scenarioContext.Get<List<(string Message, string Type)>>("ActualMessageList");
            Assert.Multiple(() =>
            {
                foreach (var (message, type) in messageResults)
                {
                    Assert.That(type, Is.EqualTo("error"), "Error message should be shown, But was success!!!");
                    Assert.That(message, Is.EqualTo(EducationConstants.ErrorMessage), $"Message {EducationConstants.ErrorMessage} is expected.");
                }
            });
        }

        [Then("I should see the error message for adding huge string")]  //Lengthy text
        public void ThenIShouldSeeTheErrorMessageForAddingHugeString()
        {
            var actualMessages = _scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actualMessage in actualMessages)
            {
                Assert.That(actualMessage, Is.EqualTo(EducationConstants.ErrorMessage), $"Expected message is {EducationConstants.ErrorMessage}, but found {actualMessage}");
            }
        }

        [When("I leave either one or all the fields empty and give the data from json file with the TestName {string}")] //Leave either one or all the fields empty for add
        public void WhenILeaveEitherOneOrAllTheFieldsEmptyAndGiveTheDataFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));

            if (scenario != null)
            {
                var actualMessageList = new List<string>();

                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.EducationDetails;
                    _educationPage.LeaveEitherOneOrAllTheFieldsEmptyToAdd(detailsToAdd.CollegeUniversityName, detailsToAdd.Country, detailsToAdd.Title, detailsToAdd.Degree, detailsToAdd.YearOfGraduation);
                    var errorMessage = _educationPage.GetErrorMessage();
                    actualMessageList.Add(errorMessage);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
            }
        }

        [Then("I should see the error message for empty fields")]//Validation for empty fields add and update (share)
        public void ThenIShouldSeeTheErrorMessageForEmptyFields()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.ErrorMessageForEmptyField), $"Expected message is {EducationConstants.ErrorMessageForEmptyField}, but not found");
            }
        }

        [When("I enter same education details twice from json file with the TestName {string}")]  //Duplicate data
        public void WhenIEnterSameEducationDetailsTwiceFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            var messageResults = new List<(string Message, string Type)>();
            var cleanUpList = new List<string>();
            if (scenario != null)
            {
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.EducationDetails;
                    _educationPage.AddEducationDetails(detailsToAdd.CollegeUniversityName, detailsToAdd.Country, detailsToAdd.Title, detailsToAdd.Degree, detailsToAdd.YearOfGraduation);
                    var (messageText, messageType) = _educationPage.GetToastMessage();
                    messageResults.Add((messageText, messageType));
                    Thread.Sleep(5000);
                    if (string.Equals(messageType, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(detailsToAdd.CollegeUniversityName);
                    }
                    messageResults.Remove(("Education has been added", "success"));
                }
            }
            _scenarioContext.Set(messageResults, "ActualMessageList");
            _scenarioContext.Set(cleanUpList, "EducationToCleanup");
        }

        [Then("I should see the error message for duplicate data")]  //Validation for duplicate data
        public void ThenIShouldSeeTheErrorMessageForDuplicateData()
        {
            var messageResults = _scenarioContext.Get<List<(string Message, string Type)>>("ActualMessageList");
            Assert.Multiple(() =>
            {
                foreach (var (message, type) in messageResults)
                {
                    Assert.That(type, Is.EqualTo("error"), $"Expected message type should be {type},but found error");
                    Assert.That(message, Is.EqualTo(EducationConstants.ErrorMessageForAddingDuplicateDetails),
                        $"Message {EducationConstants.ErrorMessageForAddingDuplicateDetails} is expected.");
                }
            });
        }

        [When("I enter education details from json file after the session has expired with the TestName {string}")]  //Add during session expired
        public void WhenIEnterEducationDetailsFromJsonFileAfterTheSessionHasExpiredWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));

            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.EducationDetails;
                    _educationPage.ExpireSession();
                    _educationPage.AddEducationDetails(detailsToAdd.CollegeUniversityName, detailsToAdd.Country, detailsToAdd.Title, detailsToAdd.Degree, detailsToAdd.YearOfGraduation);
                    var errorMessage = _educationPage.GetErrorMessage();
                    actualMessageList.Add(errorMessage);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
            }
        }

        [Then("I should see the error message for session expired")]  //Validation for session expired
        public void ThenIShouldSeeTheErrorMessageForSessionExpired()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.ErrorMessageForSessionExpired), $"Expected message is {EducationConstants.ErrorMessageForSessionExpired}, but it wasn't found");
            }
        }

        [When("I enter education details from the Json file with the test name {string}")]   //Add education details for cancel
        public void WhenIEnterEducationDetailsFromTheJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName,scenarioName,StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var details = testItem.EducationDetails;
                    _educationPage.CancelAddEducationDetails(details.CollegeUniversityName, details.Country, details.Title, details.Degree, details.YearOfGraduation);
                }
                _scenarioContext.Set(actualList, "ActualListAfterCancel");
            }
        }

        [Then("I should see the education details shouldn't be added")]  //Validation message for cancel add
        public void ThenIShouldSeeTheEducationDetailsShouldntBeAdded()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualListAfterCancel");
            Assert.That(actualList, Is.Empty, $"Expected list should be empty, but found list with added details");
        }

        [Then("I should see the error message for adding education details")]
        public void ThenIShouldSeeTheErrorMessageForAddingEducationDetails()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.ErrorMessage), $"Expected message was '{EducationConstants.ErrorMessage}', but found '{actual}'");
            }
        }
        
        [When("I update education details with the existing details from json file with the TestName {string}")] //Update with valid input
        public void WhenIUpdateEducationDetailsWithTheExistingDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualUpdateMessages = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.EducationDetails;
                    var detailsToUpdate = testItem.EducationDetailsToUpdate;
                    _educationPage.AddEducationDetails(existingDetails.CollegeUniversityName, existingDetails.Country, existingDetails.Title, existingDetails.Degree, existingDetails.YearOfGraduation);
                    _educationPage.UpdateEducationDetails(existingDetails.CollegeUniversityName,detailsToUpdate.CollegeUniversityName, detailsToUpdate.Country, detailsToUpdate.Title, detailsToUpdate.Degree, detailsToUpdate.YearOfGraduation);
                    var successMessage = _educationPage.GetSuccessMessageForUpdate(EducationConstants.SuccessMessageForUpdate);
                    actualUpdateMessages.Add(successMessage);
                    cleanUpList.Add(detailsToUpdate.CollegeUniversityName);
                }
                _scenarioContext.Set(actualUpdateMessages, "ActualUpdateMessages");
                _scenarioContext.Set(cleanUpList, "EducationToCleanup");
            }
        }

        [Then("I should see the success message for update")]  //Validation for update
        public void ThenIShouldSeeTheSuccessMessageForUpdate()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualUpdateMessages");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.SuccessMessageForUpdate), $"Expected message is {EducationConstants.SuccessMessageForUpdate}, but found actual");
            }
        }

        [When("I enter invalid education details to update from json file with the TestName {string}")] //Update with invalid input
        public void WhenIEnterInvalidEducationDetailsToUpdateFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var cleanUpList = new List<string>();
                var messageResults = new List<(string Message, string Type)>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.EducationDetails;
                    var detailsToUpdate = testItem.EducationDetailsToUpdate;
                    _educationPage.AddEducationDetails(existingDetails.CollegeUniversityName, existingDetails.Country, existingDetails.Title, existingDetails.Degree, existingDetails.YearOfGraduation);
                    Thread.Sleep(5000);
                    _educationPage.UpdateEducationDetails(existingDetails.CollegeUniversityName,detailsToUpdate.CollegeUniversityName, detailsToUpdate.Country, detailsToUpdate.Title, detailsToUpdate.Degree, detailsToUpdate.YearOfGraduation);
                    var (messageText, messageType) = _educationPage.GetToastMessage();
                    messageResults.Add((messageText, messageType));
                    if (string.Equals(messageType, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(detailsToUpdate.CollegeUniversityName);
                    }
                    else if (string.Equals(messageType, "Error", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(messageText))
                    {
                        cleanUpList.Add(existingDetails.CollegeUniversityName);
                    }
                }
                _educationPage.ClickCancelUpdateButton();
                _scenarioContext.Set(messageResults, "ActualMessageList");
                _scenarioContext.Set(cleanUpList, "EducationToCleanup");
            }
        }

        [Then("I should see the error message for update invalid data")]  //Validation for invalid input
        public void ThenIShouldSeeTheErrorMessageForUpdateInvalidData()
        {
            var messageResults = _scenarioContext.Get<List<(string Message, string Type)>>("ActualMessageList");
            Assert.Multiple(() =>
            {
                foreach (var (message, type) in messageResults)
                {
                    Assert.That(type, Is.EqualTo("error"), "Error message should be shown, But was success!!!");
                    Assert.That(message, Is.EqualTo(EducationConstants.ErrorMessage), $"Message: {EducationConstants.ErrorMessage} is expected.");
                }
            });
        }

        [Then("I should see the error message for updating huge string")]  //Validation for lengthy text
        public void ThenIShouldSeeTheErrorMessageForUpdatingHugeString()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualUpdateMessages");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.ErrorMessage),
                    $"Expected message was {EducationConstants.ErrorMessage},but found {actual}");
            }
        }

       [When("I leave either one or all the fields empty and give the data to update from json file with the TestName {string}")]  //Leave either one or all the fields empty
        public void WhenILeaveEitherOneOrAllTheFieldsEmptyAndGiveTheDataToUpdateFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));

            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.EducationDetails;
                    var detailsToUpdate = testItem.EducationDetailsToUpdate;
                    _educationPage.AddEducationDetails(existingDetails.CollegeUniversityName, existingDetails.Country, existingDetails.Title, existingDetails.Degree, existingDetails.YearOfGraduation);
                    _educationPage.LeaveEitherOneOrAllTheFieldsEmptyToUpdate(existingDetails.CollegeUniversityName, detailsToUpdate.CollegeUniversityName, detailsToUpdate.Country, detailsToUpdate.Title, detailsToUpdate.Degree, detailsToUpdate.YearOfGraduation);
                    var message = _educationPage.GetErrorMessage();
                    actualMessageList.Add(message);
                    _educationPage.ClickCancelUpdateButton();
                    cleanUpList.Add(existingDetails.CollegeUniversityName);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
                _scenarioContext.Set(cleanUpList, "EducationToCleanup");
            }
        }

        [When("I update education details from json file after the session has expired with the TestName {string}")]  //update during session expired
        public void WhenIUpdateEducationDetailsFromJsonFileAfterTheSessionHasExpiredWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));

            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.EducationDetails;
                    var detailsToUpdate = testItem.EducationDetailsToUpdate;
                    _educationPage.AddEducationDetails(existingDetails.CollegeUniversityName, existingDetails.Country, existingDetails.Title, existingDetails.Degree, existingDetails.YearOfGraduation);
                    _educationPage.ExpireSession();
                    _educationPage.UpdateEducationDetails(existingDetails.CollegeUniversityName, detailsToUpdate.CollegeUniversityName, detailsToUpdate.Country, detailsToUpdate.Title, detailsToUpdate.Degree, detailsToUpdate.YearOfGraduation);
                    var errorMessage = _educationPage.GetErrorMessage();
                    actualMessageList.Add(errorMessage);
                    _educationPage.ClickCancelUpdateButton();
                    cleanUpList.Add(existingDetails.CollegeUniversityName);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
                _scenarioContext.Set(cleanUpList, "EducationToCleanup");
            }
        }
        
        [Then("I should see the error message to update for session expired")]    //Validation for session expired
        public void ThenIShouldSeeTheErrorMessageToUpdateForSessionExpired()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.ErrorMessageForSessionExpiredToUpdate),
                    $"Expected message is {EducationConstants.ErrorMessageForSessionExpiredToUpdate}, but it wasn't found");
            }
        }

        [Then("I should login again to perform cleanup")]    //Login to clean up
        public void ThenIShouldLoginAgainToPerformCleanup()
        {
            _navigationHelper.NavigateTo("Home");
            var loginDetails = JsonHelper.LoadJson<LoginDetails>("LoginData");
            var username = loginDetails.UserName;
            var password = loginDetails.Password;
            _loginPage.Login(username, password);
            _educationPage.NavigateToTheProfilePage();
        }

        [When("I delete education details from json file after the session has expired with the TestName {string}")]  //delete during session expired
        public void WhenIDeleteEducationDetailsFromJsonFileAfterTheSessionHasExpiredWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s =>string.Equals(s.ScenarioName,scenarioName,StringComparison.OrdinalIgnoreCase));

            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                var cleanUpList = new List<string>();

                foreach (var testItem in scenario.TestItems)
                {
                    var details = testItem.EducationDetails;
                    var detailsToDelete = testItem.EducationDetailsToDelete;
                   _educationPage.AddEducationDetails(details.CollegeUniversityName, details.Country, details.Title, details.Degree, details.YearOfGraduation);
                    _educationPage.ExpireSession();
                    _educationPage.DeleteSpecificEducation(detailsToDelete.CollegeUniversityName);
                    var errorMessage = _educationPage.GetErrorMessage();
                    actualMessageList.Add(errorMessage);
                    cleanUpList.Add(details.CollegeUniversityName);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
                _scenarioContext.Set(cleanUpList,"EducationToCleanup");
            }
        }

        [Then("I should see the error message to delete for session expired")] //Validation for session expired (delete)
        public void ThenIShouldSeeTheErrorMessageToDeleteForSessionExpired()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(actual, Is.EqualTo(EducationConstants.ErrorMessageForSessionExpiredToDelete),
                    $"Expected message is {EducationConstants.ErrorMessageForSessionExpiredToDelete}, but it wasn't found");
            }
        }

        [When("I update education details with the same existing details from json file with the TestName {string}")]  //Duplicate data
        public void WhenIUpdateEducationDetailsWithTheSameExistingDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName,scenarioName,StringComparison.OrdinalIgnoreCase));
            var messageResults = new List<(string Message, string Type)>();
            var cleanUpList = new List<string>();
            if (scenario != null)
            {
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.EducationDetails;
                    var detailsToUpdate = testItem.EducationDetailsToUpdate;
                    _educationPage.AddEducationDetails(existingDetails.CollegeUniversityName,existingDetails.Country, existingDetails.Title, existingDetails.Degree,
                        existingDetails.YearOfGraduation);
                    var successMessage = _educationPage.GetSuccessMessage();
                    Console.WriteLine(successMessage);
                    _educationPage.UpdateEducationDetails(existingDetails.CollegeUniversityName,
                        detailsToUpdate.CollegeUniversityName, detailsToUpdate.Country, detailsToUpdate.Title,
                        detailsToUpdate.Degree, detailsToUpdate.YearOfGraduation);
                    var (messageText, messageType) = _educationPage.GetToastMessage();
                    messageResults.Add((messageText, messageType));
                    Thread.Sleep(5000);
                    _educationPage.ClickCancelUpdateButton();
                    cleanUpList.Add(existingDetails.CollegeUniversityName);
                }
            }
            _scenarioContext.Set(messageResults, "ActualMessageList");
            _scenarioContext.Set(cleanUpList, "EducationToCleanup");
        }

        [Then("I should see the error message for updating education details")]   //Validation for valid input for update (negative testing)
        public void ThenIShouldSeeTheErrorMessageForUpdatingEducationDetails()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualUpdateMessages");
            Assert.Multiple(() =>
            {
                foreach (var actual in actualList)
                {
                    Assert.That(actual, Is.EqualTo(EducationConstants.ErrorMessage), $"Expected message was '{EducationConstants.ErrorMessage}', but found '{actual}'");
                }
            });
        }

        [When("I enter education details for destructive testing from json file with the TestName {string}")] //destructive testing for add
        public void WhenIEnterEducationDetailsForDestructiveTestingFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<EducationFeature>("EducationTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName,scenarioName,StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualEducationList = new List<string>();
                var cleanupList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var educationDetails = testItem.EducationDetails;
                    if (educationDetails.CollegeUniversityName.Equals("CollegeName_Text_5000"))
                    {
                        educationDetails.CollegeUniversityName = new string('A', 5000);
                    }

                    _educationPage.AddEducationDetails(educationDetails.CollegeUniversityName, educationDetails.Country,
                        educationDetails.Title, educationDetails.Degree, educationDetails.YearOfGraduation);
                    var actualMessage = _educationPage.GetSuccessMessage();
                    actualEducationList.Add(actualMessage);
                    cleanupList.Add(educationDetails.CollegeUniversityName);
                }
                _scenarioContext.Set(actualEducationList,"ActualEducationList");
                _scenarioContext.Set(cleanupList, "EducationToCleanup");
            }
        }

        [When("I update education details with the existing details for destructive testing from json file with the TestName {string}")]
        public void WhenIUpdateEducationDetailsWithTheExistingDetailsForDestructiveTestingFromJsonFileWithTheTestName(string p0)
        {
            throw new PendingStepException();
        }

        [Then("I should see the error message for huge data")] //Validation for destructive testing
        public void ThenIShouldSeeTheErrorMessageForHugeData()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualEducationList");
            foreach (var actual in actualList)
            {
                Assert.That(actual,Is.EqualTo(EducationConstants.ErrorMessage), $"Expected message is {EducationConstants.ErrorMessage}, but found {actual}");
            }
        }


    }
}
