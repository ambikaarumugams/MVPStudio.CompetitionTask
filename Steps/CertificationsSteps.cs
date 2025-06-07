using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using qa_dotnet_cucumber.Entity;
using qa_dotnet_cucumber.Helper;
using qa_dotnet_cucumber.Pages;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    [Scope(Feature ="Certifications")]
    public class CertificationsSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly NavigationHelper _navigationHelper;
        private readonly LoginPage _loginPage;
        private readonly CertificationsPage _certificationsPage;
        
        public CertificationsSteps(ScenarioContext scenarioContext,NavigationHelper navigationHelper,LoginPage loginPage, CertificationsPage certificationsPage)
        {
            _scenarioContext = scenarioContext;
            _navigationHelper = navigationHelper;
            _loginPage = loginPage;
            _certificationsPage = certificationsPage;
        }

        [Given("I navigate to the profile page as a registered user")]
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            var loginDetails = JsonHelper.LoadJson<LoginDetails>("LoginData");
            var username = loginDetails.UserName;
            var password = loginDetails.Password;
            _loginPage.Login(username, password);
            _certificationsPage.NavigateToTheProfilePage();
        }

        [When("I enter certification details from json file with the TestName {string}")]
        public void WhenIEnterCertificationDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s=>s.ScenarioName == scenarioName);
            if (scenario != null)
            {
                var actualCertificationList = new List<string>();
                var expectedCertificationList = new List<string>();
                var cleanUpList = new List<string>();
                foreach(var testItem in scenario.TestItems)
                {
                    var certificationDetails = testItem.CertificationDetailsToAdd;
                    expectedCertificationList.Add(certificationDetails.CertificateOrAward);
                    _certificationsPage.AddCertifications(certificationDetails.CertificateOrAward, certificationDetails.CertifiedFrom,certificationDetails.Year);
                    var success =_certificationsPage.GetSuccessMessage();
                   actualCertificationList.Add(success);
                   cleanUpList.Add(certificationDetails.CertificateOrAward);
                }
                _scenarioContext.Set(actualCertificationList,"ActualSuccessMessageList");
                _scenarioContext.Set(expectedCertificationList,"ExpectedCertificationsList");
                _scenarioContext.Set(cleanUpList,"CertificationsToCleanup");
            }
        }

        [Then("I should see the success message")]
        public void ThenIShouldSeeTheSuccessMessage()
        {
            var actualList =_scenarioContext.Get<List<string>>("ActualSuccessMessageList");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedCertificationsList");
            foreach (var expected in expectedList)
            {
                Assert.That(actualList.Any(actual => actual.Contains(expected)),
                    Is.True, $"Expected message contains'{expected}',but not found");
            }
        }

        [When("I enter invalid certification details from json file with the TestName {string}")]
        public void WhenIEnterInvalidCertificationDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);
            if (scenario != null)
            {
                var actualCertificationList = new List<string>();
                var cleanUpList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var certificationDetails = testItem.CertificationDetailsToAdd;
                    _certificationsPage.AddCertifications(certificationDetails.CertificateOrAward,
                        certificationDetails.CertifiedFrom, certificationDetails.Year);
                    var actualMessage = _certificationsPage.GetSuccessMessage();
                    actualCertificationList.Add(actualMessage);
                   cleanUpList.Add(certificationDetails.CertificateOrAward);
                }
                _scenarioContext.Set(cleanUpList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationList,"ActualCertificationsList");
            }
        }



        [When("I enter lengthy Certificate or Award details from json file with the TestName {string}")]
        public void WhenIEnterLengthyCertificateOrAwardDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);
            if (scenario != null)
            {
                var actualCertificationList = new List<string>();
                var cleanupList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var certificationDetails = testItem.CertificationDetailsToAdd;
                    if (certificationDetails.CertificateOrAward.Equals("CertificateText_Length_255"))
                    {
                        certificationDetails.CertificateOrAward = new string('A', 255);
                    }
                    _certificationsPage.AddCertifications(certificationDetails.CertificateOrAward,certificationDetails.CertifiedFrom,certificationDetails.Year);
                    var actualMessage = _certificationsPage.GetSuccessMessage();
                    actualCertificationList.Add(actualMessage);
                    cleanupList.Add(certificationDetails.CertificateOrAward);
                }
                _scenarioContext.Set(cleanupList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationList,"ActualCertificationsList");
            }
        }

        [When("I enter lengthy Certificate from details from json file with the TestName {string}")]
        public void WhenIEnterLengthyCertificateFromDetailsFromJsonFileWithTheTestName(string scenarioName)
        {
            var feature = JsonHelper.LoadJson<CertificationFeature>("CertificationsTestData");
            var scenario = feature.Scenarios.FirstOrDefault(s => s.ScenarioName == scenarioName);
            if (scenario != null)
            {
                var actualCertificationList = new List<string>();
                var cleanupList = new List<string>();
                foreach (var testItem in scenario.TestItems)
                {
                    var certificationDetails = testItem.CertificationDetailsToAdd;
                    if (certificationDetails.CertifiedFrom.Equals("CertificateFromText_Length_255"))
                    {
                        certificationDetails.CertifiedFrom= new string('P', 255);
                    }

                    _certificationsPage.AddCertifications(certificationDetails.CertificateOrAward,
                        certificationDetails.CertifiedFrom, certificationDetails.Year);
                    var actualMessage = _certificationsPage.GetSuccessMessage();
                    actualCertificationList.Add(actualMessage);
                    cleanupList.Add(certificationDetails.CertificateOrAward);
                }

                _scenarioContext.Set(cleanupList, "CertificationsToCleanup");
                _scenarioContext.Set(actualCertificationList, "ActualCertificationsList");
            }
        }

        [Then("I should see the error message")]
        public void ThenIShouldSeeTheErrorMessage()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualCertificationsList");
            Assert.Multiple(() => {
                foreach (var actual in actualList)
                {
                    Assert.That(actual, Is.EqualTo("Invalid Certification or Award"));
                }
            });
        }
        //[Then("I should see the error message for adding huge string")]
        //public void ThenIShouldSeeTheErrorMessageForAddingHugeString()
        //{
        //    var actualList = _scenarioContext.Get<List<string>>("ActualCertificationList");

        //}


    }
}
