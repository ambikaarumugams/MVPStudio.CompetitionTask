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
        public EducationPage(IWebDriver driver)  //Inject the driver directly (BoDi)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        //Locators
        //Add
        private readonly By _profileTab = By.XPath("//a[normalize-space()='Profile']");
        private readonly By _educationTab = By.XPath("//form[@class='ui form']//a[normalize-space()='Education']");

        private readonly By _educationTable = By.XPath("//div[@data-tab='third']//table[@class='ui fixed table']");
        private readonly By _collegeUniversityNameField = By.XPath("//input[@placeholder='College/University Name']");
        private readonly By _countryDropDown = By.XPath("//select[@name='country']");
        private readonly By _titleDropDown = By.XPath("//select[@name='title']");
        private readonly By _degreeField = By.XPath("//input[@placeholder='Degree']");
        private readonly By _yearOfGraduationDropDown = By.XPath("//select[@name='yearOfGraduation']");
        private readonly By _addButton = By.XPath("//input[@value='Add']");
        private readonly By _cancelButton = By.XPath("//input[@value='Cancel']");

        //Edit
        private readonly By _collegeUniversityNameForUpdate = By.XPath(".//input[@placeholder='College/University Name']");
        private readonly By _countryDropDownForUpdate = By.XPath(".//select[@name='country']");
        private readonly By _titleDropDownForUpdate = By.XPath(".//select[@name='title']");
        private readonly By _degreeFieldForUpdate = By.XPath(".//input[@placeholder='Degree']");
        private readonly By _yearOfGraduationDropDownForUpdate = By.XPath(".//select[@name='yearOfGraduation']");
        private readonly By _updateButton = By.XPath(".//input[@value='Update']");
        private readonly By _cancelUpdateButton = By.XPath("//input[@value='Cancel']");


        //Action Methods

        public void NavigateToTheProfilePage()  //Navigate to the education page
        {
            var profileElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_profileTab));
            profileElement.Click();

            var educationElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_educationTab));
            educationElement.Click();
        }

        public void ClickAddNewButton()
        {
            //Click "Add New" button
            var addNewElement = _driver.FindElement(By.XPath("//div[@class='ui bottom attached tab segment tooltip-target active']//div[contains(@class,'ui teal button')][normalize-space()='Add New']"));
            addNewElement.Click();
        }

        public void AddEducationDetails(string universityName, string countryName, string title, string degree, string year) //Add education details
        {
            ClickAddNewButton();
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
            var selectYearOfGraduationDropDown = _wait.Until(ExpectedConditions.ElementIsVisible(_yearOfGraduationDropDown));

            SelectElement selectYear = new SelectElement(selectYearOfGraduationDropDown);
            selectYear.SelectByText(year);
            ClickAddButton();
        }

        public void ClickAddButton()
        {
            //Click "Add" button
            var addButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_addButton));
            addButton.Click();
        }

        public string GetSuccessMessage() //Get success Message
        {
            try
            {
                Thread.Sleep(3000);
                var successMessageElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']")));
                return successMessageElement.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void DeleteSpecificEducation(string educationToBeDeleted)  //Delete specific education
        {
            var educationTable = _wait.Until(ExpectedConditions.ElementIsVisible(_educationTable));
            var row = educationTable.FindElement(By.XPath($".//tr[td[2]='{educationToBeDeleted}']")); //University name is in the second column
            var deleteIcon = row.FindElement(By.XPath(".//i[contains(@class,'remove icon')]"));
            deleteIcon.Click();
        }

        public string GetErrorMessage()  //Get error message
        {
            try
            {
                var errorMessageElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']")));
                return errorMessageElement.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public (string MessageText, string MessageType) GetToastMessage()  //Tuples to get both success and error 
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));

                var toastMessageElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'ns-type-') and contains(@class,'ns-show')]")));
                Thread.Sleep(3000);
                var messageText = toastMessageElement.Text.Trim();
                var classAttribute = string.Empty;
                var messageType = string.Empty;

                classAttribute = toastMessageElement.GetAttribute("class");
                if (classAttribute != null)
                {
                    messageType = classAttribute.Contains("ns-type-success") ? "success" :
                                  classAttribute.Contains("ns-type-error") ? "error" : "none";
                }
                return (messageText, messageType);
            }
            catch
            {
                return ("", "error");
            }
        }

        public void LeaveEitherOneOrAllTheFieldsEmptyToAdd(string universityName, string countryName, string title, string degree, string year)  //Leave either one or all the fields empty
        {
            ClickAddNewButton();

            //Enter College/University Name
            var enterCollegeUniversityName = _wait.Until(ExpectedConditions.ElementIsVisible(_collegeUniversityNameField));
            if (!string.IsNullOrWhiteSpace(universityName))
            {
                enterCollegeUniversityName.SendKeys(universityName);
            }
            //Select the country name using drop down
            var selectCountryDropDown = _wait.Until(ExpectedConditions.ElementToBeClickable(_countryDropDown));

            SelectElement selectCountry = new SelectElement(selectCountryDropDown);
            if (!string.IsNullOrWhiteSpace(countryName))
            {
                selectCountry.SelectByText(countryName);
            }
            else
            {
                selectCountry.SelectByIndex(0);
            }
            //Select the title using drop down
            var titleDropDown = _wait.Until(ExpectedConditions.ElementToBeClickable(_titleDropDown));

            SelectElement selectTitle = new SelectElement(titleDropDown);
            if (!string.IsNullOrWhiteSpace(title))
            {
                selectTitle.SelectByText(title);
            }
            else
            {
                selectTitle.SelectByIndex(0);
            }
            //Enter the degree
            var degreeElement = _wait.Until(ExpectedConditions.ElementExists(_degreeField));
            if (!string.IsNullOrWhiteSpace(degree))
            {
                degreeElement.SendKeys(degree);
            }
            //Select the year of graduation drop down
            var selectYearOfGraduationDropDown = _wait.Until(ExpectedConditions.ElementIsVisible(_yearOfGraduationDropDown));

            SelectElement selectYear = new SelectElement(selectYearOfGraduationDropDown);
            if (!string.IsNullOrWhiteSpace(year))
            {
                selectYear.SelectByText(year);
            }
            else
            {
                selectYear.SelectByIndex(0);
            }
            ClickAddButton();
            ClickCancelButton();
        }

        public void ClickCancelButton()  //Click cancel button
        {
            var cancelElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_cancelButton));
            cancelElement.Click();

        }

        public void ExpireSession() //To delete the token to get the session timeout message
        {
            try
            {
                _driver.Manage().Cookies.DeleteCookieNamed("marsAuthToken");
            }
            catch
            {
            }
        }

        public void UpdateEducationDetails(string existingUniversityName, string universityName, string countryName, string title, string degree, string year)  //Update the education details
        {
            var educationTable = _wait.Until(ExpectedConditions.ElementIsVisible(_educationTable));
            var row = educationTable.FindElement(By.XPath($".//tr[td[normalize-space(text())='{existingUniversityName}']]"));
            var editIcon = row.FindElement(By.XPath(".//td[@class='right aligned']//i[@class='outline write icon']"));
            editIcon.Click();

            var editableRow = educationTable.FindElement(By.XPath($".//tr[.//input[@type='text' and @value='{existingUniversityName}']]"));

            //Enter College/University Name
            var enterCollegeUniversityNameForUpdate = editableRow.FindElement(_collegeUniversityNameForUpdate);
            enterCollegeUniversityNameForUpdate.SendKeys(Keys.Control + "a" + Keys.Delete);
            enterCollegeUniversityNameForUpdate.SendKeys(universityName);

            //Select the country name using drop down
            var selectCountryDropDownForUpdate = _wait.Until(ExpectedConditions.ElementToBeClickable(_countryDropDownForUpdate));

            SelectElement selectCountry = new SelectElement(selectCountryDropDownForUpdate);
            selectCountry.SelectByText(countryName);

            //Select the title using drop down
            var titleDropDownForUpdate = _wait.Until(ExpectedConditions.ElementToBeClickable(_titleDropDownForUpdate));

            SelectElement selectTitle = new SelectElement(titleDropDownForUpdate);
            selectTitle.SelectByText(title);

            //Enter the degree
            var degreeForUpdate = _wait.Until(ExpectedConditions.ElementExists(_degreeFieldForUpdate));
            degreeForUpdate.SendKeys(Keys.Control + "a" + Keys.Delete);
            degreeForUpdate.SendKeys(degree);

            //Select the year of graduation drop down
            var selectYearOfGraduationDropDownForUpdate = _wait.Until(ExpectedConditions.ElementIsVisible(_yearOfGraduationDropDownForUpdate));

            SelectElement selectYear = new SelectElement(selectYearOfGraduationDropDownForUpdate);
            selectYear.SelectByText(year);

            var updateButtonElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_updateButton));
            updateButtonElement.Click();
        }

        public string GetSuccessMessageForUpdate(string successMessage)
        {
            try
            {
                var successMessageForUpdateElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[@class='ns-box-inner' and  contains(text(), '{successMessage}')]")));
                return successMessageForUpdateElement.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void CancelAddEducationDetails(string universityName, string countryName, string title, string degree, string year) //Cancel add education details
        {
            ClickAddNewButton();
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
            var selectYearOfGraduationDropDown = _wait.Until(ExpectedConditions.ElementIsVisible(_yearOfGraduationDropDown));

            SelectElement selectYear = new SelectElement(selectYearOfGraduationDropDown);
            selectYear.SelectByText(year);
            ClickCancelButton();
        }

        public void LeaveEitherOneOrAllTheFieldsEmptyToUpdate(string existingUniversityName, string universityName, string countryName, string title, string degree, string year)  //Leave either one or all the fields empty
        {
            var educationTable = _wait.Until(ExpectedConditions.ElementIsVisible(_educationTable));
            var row = educationTable.FindElement(By.XPath($".//tr[td[normalize-space(text())='{existingUniversityName}']]"));
            var editIcon = row.FindElement(By.XPath(".//td[@class='right aligned']//i[@class='outline write icon']"));
            editIcon.Click();

            var editableRow = educationTable.FindElement(By.XPath($".//tr[.//input[@type='text' and @value='{existingUniversityName}']]"));

            //Enter College/University Name
            var enterCollegeUniversityNameForUpdate = editableRow.FindElement(_collegeUniversityNameForUpdate);
            enterCollegeUniversityNameForUpdate.SendKeys(Keys.Control + "a" + Keys.Delete);

            if (!string.IsNullOrWhiteSpace(universityName))
            {
                enterCollegeUniversityNameForUpdate.SendKeys(universityName);
            }
            //Select the country name using drop down
            var selectCountryDropDownForUpdate = _wait.Until(ExpectedConditions.ElementToBeClickable(_countryDropDownForUpdate));

            SelectElement selectCountry = new SelectElement(selectCountryDropDownForUpdate);

            if (!string.IsNullOrWhiteSpace(countryName))
            {
                selectCountry.SelectByText(countryName);
            }
            else
            {
                selectCountry.SelectByIndex(0);
            }
            //Select the title using drop down
            var titleDropDownForUpdate = _wait.Until(ExpectedConditions.ElementToBeClickable(_titleDropDownForUpdate));

            SelectElement selectTitle = new SelectElement(titleDropDownForUpdate);
            if (!string.IsNullOrWhiteSpace(title))
            {
                selectTitle.SelectByText(title);
            }
            else
            {
                selectTitle.SelectByIndex(0);
            }
            //Enter the degree
            var degreeForUpdate = _wait.Until(ExpectedConditions.ElementExists(_degreeFieldForUpdate));
            degreeForUpdate.SendKeys(Keys.Control + "a" + Keys.Delete);
            if (!string.IsNullOrWhiteSpace(degree))
            {
                degreeForUpdate.SendKeys(degree);
            }
            //Select the year of graduation drop down
            var selectYearOfGraduationDropDownForUpdate = _wait.Until(ExpectedConditions.ElementIsVisible(_yearOfGraduationDropDownForUpdate));

            SelectElement selectYear = new SelectElement(selectYearOfGraduationDropDownForUpdate);
            if (!string.IsNullOrWhiteSpace(year))
            {
                selectYear.SelectByText(year);
            }
            else
            {
                selectYear.SelectByIndex(0);
            }
            var updateButtonElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_updateButton));
            updateButtonElement.Click();
        }

        public void ClickCancelUpdateButton()  //Click cancel update
        {
            var cancelUpdateElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_cancelUpdateButton));
            cancelUpdateElement.Click();
        }
    }
}






