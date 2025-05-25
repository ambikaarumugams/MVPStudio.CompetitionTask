using Reqnroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qa_dotnet_cucumber.Pages;

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

        public EducationSteps(LoginPage loginPage,NavigationHelper navigationHelper,ScenarioContext scenarioContext,EducationPage educationPage)
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

       [When("I click the {string} button")]
        public void WhenIClickTheButton(string buttonName)
        {
            if(buttonName=="Add New")
            {
                _educationPage.ClickAddNewButton();
            }
            else if (buttonName == "Add")
            {
                _educationPage.ClickAddButton();
            }
            
        }

        [When("I enter education details from JSON file")]
        public void WhenIEnterEducationDetailsFromJSONFile()
        {
             var details = JsonHelper.LoadJson<EducationDetails>("EducationTestData");
            _educationPage.AddEducationDetails(details.CollegeUniversityName,details.Country,details.Title,details.Degree,details.YearOfGraduation);
        }

        
        [Then("I should see the success message")]
        public void ThenIShouldSeeTheSuccessMessage()
        {
            Console.WriteLine("Passed");
        }


    }
}
