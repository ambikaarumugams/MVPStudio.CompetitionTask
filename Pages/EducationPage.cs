using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace qa_dotnet_cucumber.Pages
{
    public class EducationPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        //Constructor
        public EducationPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        //Locators
        //Add
        private readonly By _profileTab = By.XPath("//a[normalize-space()='Profile']");
        private readonly By _educationTab = By.XPath("//form[@class='ui form']//a[normalize-space()='Education']");

        private readonly By _educationTable = By.XPath("//div[@class='ui bottom attached tab segment tooltip-target active']//h3[contains(.,'Education')]");
       // private readonly By _addNewButton = By.XPath("//div[@class='ui teal button' and normalize-space()='Add New']");

        private readonly By _collegeUniversityNameField = By.XPath("//input[@placeholder='College/University Name']");
        private readonly By _countryDropDown = By.XPath("//select[@name='country']");
        private readonly By _titleDropDown = By.XPath("//select[@name='title']");
        private readonly By _degreeField = By.XPath("//input[@placeholder='Degree']");
        private readonly By _yearOfGraduationDropDown = By.XPath("//select[@name='yearOfGraduation']");
        private readonly By _addButton = By.XPath("//input[@value='Add']");
        private readonly By _cancelButton = By.XPath("//input[@value='Cancel']");

        //Edit


        //Action Methods

        public void NavigateToTheProfilePage()
        {
            var profileElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_profileTab));
            profileElement.Click();

            var educationElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_educationTab));
            educationElement.Click();
        }

        public void ClickAddNewButton()
        {
            //Click "Add New" button
            var educationTable = _wait.Until(ExpectedConditions.ElementIsVisible(_educationTable));
            var addNewElement = educationTable.FindElement(By.XPath("//div[@class='ui bottom attached tab segment tooltip-target active']//div[contains(@class,'ui teal button')][normalize-space()='Add New']"));
            addNewElement.Click();
        }

        public void AddEducationDetails(string universityName, string countryName, string title, string degree, string year)
        {
           //Enter College/University Name
            var enterCollegeUniversityName = _wait.Until(ExpectedConditions.ElementIsVisible(_collegeUniversityNameField));
            enterCollegeUniversityName.SendKeys(universityName);

            //Select the country name using drop down
            var selectCountryDropDown = _wait.Until(ExpectedConditions.ElementToBeClickable(_countryDropDown));

            SelectElement selectCountry = new SelectElement(selectCountryDropDown);
            selectCountry.SelectByText(countryName);

            //Select the title using drop down
            var titleDropDown = _wait.Until(ExpectedConditions.ElementToBeClickable(_titleDropDown));

            SelectElement selectTitle = new SelectElement(titleDropDown);
            selectTitle.SelectByText(title);

            //Enter the degree
            var degreeElement = _wait.Until(ExpectedConditions.ElementExists(_degreeField));
            degreeElement.SendKeys(degree);

            //Select the year of graduation drop down
            var selectYearOfGraduationDropDown =
                _wait.Until(ExpectedConditions.ElementIsVisible(_yearOfGraduationDropDown));

            SelectElement selectYear = new SelectElement(selectYearOfGraduationDropDown);
            selectYear.SelectByText(year);
        }

        public void ClickAddButton()
        {
            //Click "Add" button
            var addButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_addButton));
            addButton.Click();
        }
    }
}
