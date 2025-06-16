using qa_dotnet_cucumber.Entity;
using qa_dotnet_cucumber.Helper;
using qa_dotnet_cucumber.Pages;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    [Scope(Feature = "Certification")]
    public class CertificationSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly NavigationHelper _navigationHelper;
        private readonly LoginPage _loginPage;
        private readonly CertificationPage _certificationPage;

        //Constructor
        public CertificationSteps(ScenarioContext scenarioContext, NavigationHelper navigationHelper, LoginPage loginPage, CertificationPage certificationPage)
        {
            _scenarioContext = scenarioContext;
            _navigationHelper = navigationHelper;
            _loginPage = loginPage;
            _certificationPage = certificationPage;
        }

        [Given("I navigate to the profile page as a registered user")] //Login to the application
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            var loginDetails = JsonHelper.LoadJson<LoginDetails>("LoginData"); 
            var username = loginDetails.UserName;
            var password = loginDetails.Password;
            _loginPage.Login(username, password);
            _certificationPage.NavigateToTheProfilePage();
        }

        [When("I enter certification details from json file with the TestName {string}")] //Add valid certification details, Certificate or Award and Certificate From mismatch,
        public void WhenIEnterCertificationDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName,scenarioName,StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualCertificationList = new List<string>();
                var expectedCertificationList = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var success = _certificationPage.GetSuccessMessage();
                    actualCertificationList.Add(success);
                    cleanUpList.Add(detailsToAdd.CertificateOrAward);
                    expectedCertificationList.Add(detailsToAdd.ExpectedMessage);
                }
                _scenarioContext.Set(actualCertificationList, "ActualMessageList");
                _scenarioContext.Set(expectedCertificationList, "ExpectedMessageList");
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
            }
        }

        [Then("I should see the success message")] //Validation for valid input 
        public void ThenIShouldSeeTheSuccessMessage()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedMessageList");
            foreach (var actual in actualList)
            {
                Assert.That(expectedList, Does.Contain(actual), "No matching item found in expected list.");
            }
        }

        [When("I enter invalid certification details from json file with the TestName {string}")]  //Enter invalid certification details (both Certificate Or Award and Certificate From share the same steps)
        public void WhenIEnterInvalidCertificationDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualCertificationsList = new List<string>();
                var expectedCertificationsList = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var actualMessage = _certificationPage.GetSuccessMessage();
                    actualCertificationsList.Add(actualMessage);
                    expectedCertificationsList.Add(detailsToAdd.ExpectedMessage);
                    cleanUpList.Add(detailsToAdd.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationsList, "ActualMessageList");
                _scenarioContext.Set(expectedCertificationsList, "ExpectedMessageList");
            }
        }

        [When("I enter lengthy Certificate or Award details from json file with the TestName {string}")] // Enter the length of the Certificate or Award text >250 
        public void WhenIEnterLengthyCertificateOrAwardDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s =>string.Equals(s.ScenarioName, scenarioName,StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualCertificationsList = new List<string>();
                var cleanUpList = new List<string>();
                var expectedCertificationsList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    if (detailsToAdd.CertificateOrAward.Equals("CertificateText_Length_255"))  //Use placeholder to generate the data dynamically
                    {
                        detailsToAdd.CertificateOrAward = new string('A', 255);  //Assign the string value to the certificate or award
                    }
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var actualMessage = _certificationPage.GetSuccessMessage();
                    actualCertificationsList.Add(actualMessage);
                    expectedCertificationsList.Add(detailsToAdd.ExpectedMessage);
                    cleanUpList.Add(detailsToAdd.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationsList, "ActualMessageList");
                _scenarioContext.Set(expectedCertificationsList, "ExpectedMessageList");
            }
        }

        [When("I enter lengthy Certificate from details from json file with the TestName {string}")]  // Enter the length of the Certificate from text >250 
        public void WhenIEnterLengthyCertificateFromDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualCertificationsList = new List<string>();
                var expectedCertificationsList = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    if (detailsToAdd.CertifiedFrom.Equals("CertificateFromText_Length_255"))
                    {
                        detailsToAdd.CertifiedFrom = new string('P', 255);
                    }
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var actualMessage = _certificationPage.GetSuccessMessage();
                    actualCertificationsList.Add(actualMessage);
                    cleanUpList.Add(detailsToAdd.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationsList, "ActualMessageList");
                _scenarioContext.Set(expectedCertificationsList, "ExpectedMessageList");
            }
        }

        [Then("I should see the error message")]  //Validation for invalid input, lengthy certificate or award, lengthy certificate from, destructive testing(add,update)
        public void ThenIShouldSeeTheErrorMessage()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedMessageList");
            Assert.Multiple(() =>   
            {
                foreach (var actual in actualList)
                {
                    Assert.That(expectedList.Contains(actual), Is.True, $"Expected message not found, instead found {actual}");
                }
            });
        }

        [When("I leave either one or all the fields empty and give the data from json file with the TestName {string}")]  //Leave either one or all the fields are empty for add
        public void WhenILeaveEitherOneOrAllTheFieldsEmptyAndGiveTheDataFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualCertificationsList = new List<string>();
                var expectedCertificationsList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    _certificationPage.LeaveEitherOneOrAllTheFieldsEmptyForAdd(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var actualMessage = _certificationPage.GetErrorMessage();
                    actualCertificationsList.Add(actualMessage);
                    expectedCertificationsList.Add(detailsToAdd.ExpectedMessage);
                    _certificationPage.ClickCancelButton();
                }
                _scenarioContext.Set(actualCertificationsList, "ActualMessageList");
                _scenarioContext.Set(expectedCertificationsList,"ExpectedMessageList");
            }
        }

        [Then("I should see the error message for empty fields")]  //Validation for empty fields(share same step for add and update) 
        public void ThenIShouldSeeTheErrorMessageForEmptyFields()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedMessageList");
            Assert.Multiple(() =>
            {
                Assert.That(expectedList,Is.EqualTo(actualList),$"Expected and actual lists are not equal");
            });
        }
        
        [When("I enter same certification details twice from json file with the TestName {string}")]   //Add duplicate data 
        public void WhenIEnterSameCertificationDetailsTwiceFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            var actualMessages = new List<(string Message, string Type)>();
            var expectedMessages = new List<string>();
            var cleanUpList = new List<string>();
            if (scenario != null)
            {
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var (messageText, messageType) = _certificationPage.GetToastMessage();  // Get the toast message
                    if (string.Equals(messageType, "error", StringComparison.OrdinalIgnoreCase))
                    {
                        actualMessages.Add((messageText, messageType));
                    }
                    Thread.Sleep(5000);
                    if (string.Equals(messageType, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(detailsToAdd.CertificateOrAward);
                    }
                    expectedMessages.Add(detailsToAdd.ExpectedMessage);
                }
            }
            _scenarioContext.Set(actualMessages, "ActualMessageList");
            _scenarioContext.Set(expectedMessages,"ExpectedMessageList");
            _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
        }

        [Then("I should see the error message for duplicate data")] //Validation for duplicate data (share step for add and update)
        public void ThenIShouldSeeTheErrorMessageForDuplicateData()
        {
            var actualList = _scenarioContext.Get<List<(string, string)>>("ActualMessageList");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedMessageList");

            Assert.Multiple(() =>
            {
                foreach (var (message, type) in actualList)
                {
                    Assert.That(type, Is.EqualTo("error"), $"Expected message type should be {type},but found error");
                    Assert.That(expectedList.Contains(message),Is.True, $"Expected list don't contain the actual message");
                }
            });
        }
        
        [When("I enter certification details from json file after the session has expired with the TestName {string}")]   //Add certification details during session expired
        public void WhenIEnterCertificationDetailsFromJsonFileAfterTheSessionHasExpiredWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                var expectedMessageList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    _certificationPage.ExpireSession();
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var actualMessage = _certificationPage.GetErrorMessage();
                    actualMessageList.Add(actualMessage);
                    expectedMessageList.Add(detailsToAdd.ExpectedMessage);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
                _scenarioContext.Set(expectedMessageList,"ExpectedMessageList");
            }
        }

        [Then("I should see the error message for session expired")]  //Validation for session expired
        public void ThenIShouldSeeTheErrorMessageForSessionExpired()
        {
            var actualMessageList = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedMessageList = _scenarioContext.Get<List<string>>("ExpectedMessageList");
            foreach (var actual in actualMessageList)
            {
                Assert.That(expectedMessageList.Contains(actual),Is.True, $"Expected message list don't contain {actual}");
            }
        }

        [Then("I should see the error message for certificate and provider mismatch")]  //Mismatch of Certificate or Award and Certificate From 
        public void ThenIShouldSeeTheErrorMessageForCertificateAndProviderMismatch()
        {
            var actualMessageList = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedMessageList = _scenarioContext.Get<List<string>>("ExpectedMessageList");
            Assert.Multiple(() =>
            {
                foreach (var actual in actualMessageList)
                {
                    Assert.That(expectedMessageList.Contains(actual),Is.True,$"Expected message list don't contain the {actual}");
                }
            });
        }

        [When("I enter certification details from json file and cancel the add with the TestName {string}")]   //Cancel the Add certification details
        public void WhenIEnterCertificationDetailsFromJsonFileAndCancelTheAddWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    _certificationPage.CancelAddCertificationDetails(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                }
                _scenarioContext.Set(actualList, "ActualListAfterCancel");
            }
        }

        [Then("I should see the certification details shouldn't be added")]  //Validation for cancel add certification details
        public void ThenIShouldSeeTheCertificationDetailsShouldntBeAdded()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualListAfterCancel");
            Assert.That(actualList, Is.Empty, $"Expected list should be empty, but found list with added details");
        }

        [When("I update certification details with the existing details from json file with the TestName {string}")]   //Update valid input, invalid input, mismatch Certificate or Award and Certificate From
        public void WhenIUpdateCertificationDetailsWithTheExistingDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualUpdateMessages = new List<string>();
                var expectedUpdateMessages = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                    _certificationPage.AddCertifications(existingDetails.CertificateOrAward, existingDetails.CertifiedFrom, existingDetails.Year);
                    _certificationPage.UpdateCertificationDetails(existingDetails.CertificateOrAward, detailsToUpdate.CertificateOrAward, detailsToUpdate.CertifiedFrom, detailsToUpdate.Year);
                    var successMessage = _certificationPage.GetSuccessMessageForUpdate(detailsToUpdate.CertificateOrAward);
                    actualUpdateMessages.Add(successMessage);
                    expectedUpdateMessages.Add(detailsToUpdate.ExpectedMessage);
                    cleanUpList.Add(detailsToUpdate.CertificateOrAward);
                }
                _scenarioContext.Set(actualUpdateMessages, "ActualMessageList");
                _scenarioContext.Set(expectedUpdateMessages,"ExpectedMessageList");
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
            }
        }

        [Then("I should see the success message for update")]  //Validation for valid input (update)
        public void ThenIShouldSeeTheSuccessMessageForUpdate()
        {
            var actualMessages = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedMessages = _scenarioContext.Get<List<string>>("ExpectedMessageList");

            Assert.That(expectedMessages,Is.EqualTo(actualMessages),$"Expected and actual messages are not equal");
        }

        [Then("I should see the error message for update invalid data")]  //Validation for invalid input (update)
        public void ThenIShouldSeeTheErrorMessageForUpdateInvalidData()
        {
            var actualMessages = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedMessages = _scenarioContext.Get<List<string>>("ExpectedMessageList");
            Assert.Multiple(() =>
            {
                foreach (var actual in actualMessages)
                {
                    Assert.That(expectedMessages.Contains(actual),Is.True,$"Expected message list don't contain '{actual}'");
                }
            });
        }

        [When("I update lengthy Certificate or Award details with the existing details from json file with the TestName {string}")]  //Update Certificate or Award text length >250 characters
        public void WhenIUpdateLengthyCertificateOrAwardDetailsWithTheExistingDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualCertificationsListForUpdate = new List<string>();
                var expectedCertificationsListForUpdate = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                    _certificationPage.AddCertifications(existingDetails.CertificateOrAward, existingDetails.CertifiedFrom, existingDetails.Year);
                    if (detailsToUpdate.CertificateOrAward.Equals("CertificateText_Length_255"))
                    {
                        detailsToUpdate.CertificateOrAward = new string('s', 255);
                    }
                    _certificationPage.UpdateCertificationDetails(existingDetails.CertificateOrAward, detailsToUpdate.CertificateOrAward, detailsToUpdate.CertifiedFrom, detailsToUpdate.Year);
                    var actualMessage = _certificationPage.GetSuccessMessage();
                    actualCertificationsListForUpdate.Add(actualMessage);
                    expectedCertificationsListForUpdate.Add(detailsToUpdate.ExpectedMessage);
                    cleanUpList.Add(detailsToUpdate.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationsListForUpdate, "ActualMessageList");
                _scenarioContext.Set(expectedCertificationsListForUpdate, "ExpectedMessageList");
            }
        }

        [When("I update lengthy Certificate From details with the existing details from json file with the TestName {string}")] //Update Certificate From text length >250
        public void WhenIUpdateLengthyCertificateFromDetailsWithTheExistingDetailsFromJsonFileWithTheTestName(
            string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualCertificationsListForUpdate = new List<string>();
                var expectedCertificationsListForUpdate = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                    _certificationPage.AddCertifications(existingDetails.CertificateOrAward, existingDetails.CertifiedFrom, existingDetails.Year);
                    if (detailsToUpdate.CertifiedFrom.Equals("CertifiedFrom_Length_255"))
                    {
                        detailsToUpdate.CertifiedFrom = new string('g', 255);
                    }
                    _certificationPage.UpdateCertificationDetails(existingDetails.CertificateOrAward, detailsToUpdate.CertificateOrAward, detailsToUpdate.CertifiedFrom, detailsToUpdate.Year);
                    var actualMessage = _certificationPage.GetSuccessMessage();
                    actualCertificationsListForUpdate.Add(actualMessage);
                    expectedCertificationsListForUpdate.Add(detailsToUpdate.ExpectedMessage);
                    cleanUpList.Add(detailsToUpdate.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationsListForUpdate, "ActualMessageList");
                _scenarioContext.Set(expectedCertificationsListForUpdate, "ExpectedMessageList");
            }
        }

        [Then("I should see the error message for updating huge string")]  //Validation step for huge string
        public void ThenIShouldSeeTheErrorMessageForUpdatingHugeString()
        {
            var actualMessages = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedMessages = _scenarioContext.Get<List<string>>("ExpectedMessageList");
            foreach (var actual in actualMessages)
            {
                Assert.That(expectedMessages.Contains(actual),Is.True,$"Expected message not found, instead found {actual}");
            }
        }

        [When("I leave either one or all the fields empty and give the data to update from json file with the TestName {string}")] //Leave either one or all the fields empty for update
        public void WhenILeaveEitherOneOrAllTheFieldsEmptyAndGiveTheDataToUpdateFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualUpdateMessages = new List<string>();
                var expectedUpdateMessages = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                    _certificationPage.AddCertifications(existingDetails.CertificateOrAward, existingDetails.CertifiedFrom, existingDetails.Year);
                    _certificationPage.LeaveEitherOneOrAllTheFieldsEmptyForUpdate(existingDetails.CertificateOrAward,detailsToUpdate.CertificateOrAward, detailsToUpdate.CertifiedFrom, detailsToUpdate.Year);
                    var actualMessage = _certificationPage.GetErrorMessage();
                    actualUpdateMessages.Add(actualMessage);
                    _certificationPage.ClickCancelUpdate();
                    expectedUpdateMessages.Add(detailsToUpdate.ExpectedMessage);
                    cleanUpList.Add(existingDetails.CertificateOrAward);
                }
                _scenarioContext.Set(actualUpdateMessages, "ActualMessageList");
                _scenarioContext.Set(expectedUpdateMessages, "ExpectedMessageList");
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
            }
        }

        [When("I update same certification details twice from json file with the TestName {string}")]  //Update duplicate data
        public void WhenIUpdateSameCertificationDetailsTwiceFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualMessages = new List<(string Message, string Type)>();
                var expectedMessages = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                    _certificationPage.AddCertifications(existingDetails.CertificateOrAward,existingDetails.CertifiedFrom,existingDetails.Year);
                    _certificationPage.UpdateCertificationDetails(existingDetails.CertificateOrAward,detailsToUpdate.CertificateOrAward,detailsToUpdate.CertifiedFrom,detailsToUpdate.Year);
                    var (messageText, messageType) = _certificationPage.GetToastMessage();
                    if (string.Equals(messageType, "error", StringComparison.OrdinalIgnoreCase))
                    {
                        actualMessages.Add((messageText, messageType));
                    }
                    Thread.Sleep(5000);
                    if (string.Equals(messageType, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(detailsToUpdate.CertificateOrAward);
                    }
                    else if (string.Equals(messageType, "ERROR", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanUpList.Add(existingDetails.CertificateOrAward);
                    }
                    expectedMessages.Add(detailsToUpdate.ExpectedMessage);
                }
                _scenarioContext.Set(actualMessages, "ActualMessageList");
                _scenarioContext.Set(expectedMessages, "ExpectedMessageList");
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
            }
        }

        [When("I enter certification details from json file and cancel the update with the TestName {string}")]  //Cancel update process
        public void WhenIEnterCertificationDetailsFromJsonFileAndCancelTheUpdateWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualMessages = new List<string>();
                var expectedMessages = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                    expectedMessages.Add(existingDetails.ExpectedMessage);
                    _certificationPage.AddCertifications(existingDetails.CertificateOrAward, existingDetails.CertifiedFrom, existingDetails.Year);
                    var actualMessage = _certificationPage.GetSuccessMessage();
                    actualMessages.Add(actualMessage);
                    _certificationPage.CancelUpdateCertificationDetails(existingDetails.CertificateOrAward,detailsToUpdate.CertificateOrAward,detailsToUpdate.CertifiedFrom,detailsToUpdate.Year);
                    cleanUpList.Add(existingDetails.CertificateOrAward);
                }
                _scenarioContext.Set(actualMessages, "ActualMessageList");
                _scenarioContext.Set(expectedMessages, "ExpectedMessageList");
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
            }
        }

        [Then("I should see the added certification not the updated one")]  //Validation for cancel update
        public void ThenIShouldSeeTheAddedCertificationNotTheUpdatedOne()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualMessageList");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedMessageList");
           
            Assert.That(expectedList,Is.EqualTo(actualList), $"Expected list is not equal to actualList");
        }

        [When("I update certification details from json file after the session has expired with the TestName {string}")]  //Update during session expired
        public void WhenIUpdateCertificationDetailsFromJsonFileAfterTheSessionHasExpiredWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                var expectedMessageList = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                    _certificationPage.AddCertifications(existingDetails.CertificateOrAward,existingDetails.CertifiedFrom, existingDetails.Year);
                    _certificationPage.ExpireSession();
                    _certificationPage.UpdateCertificationDetails(existingDetails.CertificateOrAward,detailsToUpdate.CertificateOrAward, detailsToUpdate.CertifiedFrom, detailsToUpdate.Year);
                    var actualMessage = _certificationPage.GetErrorMessage();
                    actualMessageList.Add(actualMessage);
                    _certificationPage.ClickCancelUpdate();
                    expectedMessageList.Add(detailsToUpdate.ExpectedMessage);
                    cleanUpList.Add(existingDetails.CertificateOrAward);
                }
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
                _scenarioContext.Set(expectedMessageList, "ExpectedMessageList");
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
            }
        }

        [Then("I should log in again to perform clean up")] //Login again to clean up
        public void ThenIShouldLogInAgainToPerformCleanUp()
        {
            _navigationHelper.NavigateTo("Home");
            var loginDetails = JsonHelper.LoadJson<LoginDetails>("LoginData");
            var username = loginDetails.UserName;
            var password = loginDetails.Password;
            _loginPage.Login(username, password);
            _certificationPage.NavigateToTheProfilePage();
        }

        [When("I enter huge Certificate or Award details to perform add from json file with the TestName {string}")]  //Destructive testing for add
        public void WhenIEnterHugeCertificateOrAwardDetailsToPerformAddFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualMessagesList = new List<string>();
                var cleanUpList = new List<string>();
                var expectedMessageList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    if (detailsToAdd.CertificateOrAward.Equals("CertificateText_Length_5000"))
                    {
                        detailsToAdd.CertificateOrAward = new string('A', 5000);
                    }
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var actualMessage = _certificationPage.GetSuccessMessage();
                    actualMessagesList.Add(actualMessage);
                    expectedMessageList.Add(detailsToAdd.ExpectedMessage);
                    cleanUpList.Add(detailsToAdd.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualMessagesList, "ActualMessageList");
                _scenarioContext.Set(expectedMessageList, "ExpectedMessageList");
            }
        }

        [When("I enter huge Certificate or Award details to perform update from json file with the TestName {string}")] //Destructive testing for update
        public void WhenIEnterHugeCertificateOrAwardDetailsToPerformUpdateFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                var cleanUpList = new List<string>();
                var expectedMessageList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var existingDetails = testItem.CertificationDetailsToAdd;
                    var detailsToUpdate = testItem.CertificationDetailsToUpdate;
                   _certificationPage.AddCertifications(existingDetails.CertificateOrAward, existingDetails.CertifiedFrom, existingDetails.Year);
                   if (detailsToUpdate.CertificateOrAward.Equals("CertificateText_Length_5000"))
                   {
                       detailsToUpdate.CertificateOrAward = new string('K', 5000);
                   }
                   _certificationPage.UpdateCertificationDetails(existingDetails.CertificateOrAward,detailsToUpdate.CertificateOrAward,detailsToUpdate.CertifiedFrom,detailsToUpdate.Year);
                   var actualMessage = _certificationPage.GetSuccessMessageForUpdate(detailsToUpdate.CertificateOrAward);
                    actualMessageList.Add(actualMessage);
                    expectedMessageList.Add(detailsToUpdate.ExpectedMessage);
                    cleanUpList.Add(detailsToUpdate.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
                _scenarioContext.Set(expectedMessageList, "ExpectedMessageList");
            }
        }

        [When("I delete certification details from json file after the session has expired with the TestName {string}")] //Delete during session expired
        public void WhenIDeleteCertificationDetailsFromJsonFileAfterTheSessionHasExpiredWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => string.Equals(s.ScenarioName, scenarioName, StringComparison.OrdinalIgnoreCase));
            if (scenario != null)
            {
                var actualMessageList = new List<string>();
                var cleanUpList = new List<string>();
                var expectedMessageList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var detailsToAdd = testItem.CertificationDetailsToAdd;
                    var detailsToDelete = testItem.CertificationDetailsToDelete;
                    _certificationPage.AddCertifications(detailsToAdd.CertificateOrAward, detailsToAdd.CertifiedFrom, detailsToAdd.Year);
                    var successMessage = _certificationPage.GetSuccessMessage();
                    Console.WriteLine(successMessage);
                    _certificationPage.ExpireSession();
                    _certificationPage.DeleteSpecificCertification(detailsToDelete.CertificateOrAward);
                    expectedMessageList.Add(detailsToDelete.ExpectedMessage);
                    var actualMessage = _certificationPage.GetErrorMessage();
                    actualMessageList.Add(actualMessage);
                    cleanUpList.Add(detailsToDelete.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualMessageList, "ActualMessageList");
                _scenarioContext.Set(expectedMessageList, "ExpectedMessageList");
            }
        }
    }
}
