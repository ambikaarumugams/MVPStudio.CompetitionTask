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
    public class CertificationsPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        public CertificationsPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        //Locators
        //Add
        private readonly By _profileTab = By.XPath("//a[normalize-space()='Profile']");
        private readonly By _certificationsTab = By.XPath("//a[normalize-space()='Certifications']");

        private readonly By _addNewButton =
            By.XPath(
                "//div[@class='ui bottom attached tab segment tooltip-target active']//div[contains(@class,'ui teal button')][normalize-space()='Add New']");
        private readonly By _certificationsTable = By.XPath("//div[@data-tab='fourth']//table[@class='ui fixed table']");
        private readonly By _certificateOrAwardField = By.XPath("//input[@placeholder='Certificate or Award']");
        private readonly By _certificateFromField = By.XPath("//input[@placeholder='Certified From (e.g. Adobe)']");
        private readonly By _certificationYearDropDown = By.XPath("//select[@name='certificationYear']");
        private readonly By _addButton = By.XPath("//input[@value='Add']");
        private readonly By _cancelButton = By.XPath("//input[@value='Cancel']");

        //Edit
        private readonly By _certificateOrAwardFieldForUpdate = By.XPath(".//input[@placeholder='Certificate or Award']");
        private readonly By _certificateFromFieldForUpdate = By.XPath(".//input[@placeholder='Certified From (e.g. Adobe)']");
        private readonly By _certificationYearDropDownForUpdate = By.XPath(".//select[@name='certificationYear']");
        private readonly By _updateButton = By.XPath("//input[@value='Update']");
        private readonly By _cancelUpdateButton = By.XPath("//input[@value='Cancel']");

        //Action Methods

        public void NavigateToTheProfilePage()
        {
            var profileElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_profileTab));
            profileElement.Click();

            var certificationElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_certificationsTab));
            certificationElement.Click();
        }

        public void ClickAddNewButton()
        {
            var addNewButtonElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_addNewButton));
            addNewButtonElement.Click();
        }

        public void AddCertifications(string certificationOrAward,string certificationFrom,string certificationYear)
        {
            ClickAddNewButton();
            var certificationOrAwardElement = _wait.Until(ExpectedConditions.ElementIsVisible(_certificateOrAwardField));
            certificationOrAwardElement.SendKeys(certificationOrAward);

            var certificateFromElement = _wait.Until(ExpectedConditions.ElementIsVisible(_certificateFromField));
            certificateFromElement.SendKeys(certificationFrom);

            var certificationYearElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_certificationYearDropDown));
            SelectElement selectCertificationYear = new SelectElement(certificationYearElement);
            selectCertificationYear.SelectByText(certificationYear);

            ClickAddButton();
        }

        public void ClickAddButton()
        {
            var addButtonElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_addButton));
            addButtonElement.Click();
        }

        public string GetSuccessMessage()
        {
            try
            {
                Thread.Sleep(3000);
                var successMessageElement =
                    _wait.Until(ExpectedConditions.ElementIsVisible(
                        By.XPath("//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']")));
                return successMessageElement.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void DeleteSpecificCertification(string certificationOrAwardToBeDeleted)
        {
            var certificationTable= _wait.Until(ExpectedConditions.ElementIsVisible(_certificationsTable));
            var row = certificationTable.FindElement(By.XPath($".//tr[td[1]='{certificationOrAwardToBeDeleted}']"));
            var deleteIcon = row.FindElement(By.XPath(".//i[contains(@class,'remove icon')]"));

            deleteIcon.Click();
        }








    }

}
