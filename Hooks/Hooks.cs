using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using Reqnroll.BoDi;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Text.Json;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using qa_dotnet_cucumber.Config;
using qa_dotnet_cucumber.Pages;

namespace qa_dotnet_cucumber.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private static TestSettings _settings;
        private static readonly object _reportLock = new object();
        private static ExtentSparkReporter? _htmlReporter;
        private static ExtentReports? _extent;
        private static ExtentTest _test;

        private IWebDriver _driver;
        public static TestSettings Settings => _settings;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Console.WriteLine($"[BeforeTestRun] Starting test run at {DateTime.Now}");
            string currentDir = Directory.GetCurrentDirectory();  //Get the current working directory
            //Load settings.json from the current directory
            string settingsPath = Path.Combine(currentDir, "settings.json");
            string json = File.ReadAllText(settingsPath);
            _settings = JsonSerializer.Deserialize<TestSettings>(json);

            //Get project root by navigating up from bin/ Debug / net8.0
            string projectRoot = Path.GetFullPath(Path.Combine(currentDir, "..", ".."));
            string reportFileName = _settings.Report.Path.TrimStart('/'); // e.g., "TestReport.html"
            string reportPath = Path.Combine(projectRoot, reportFileName);

            Console.WriteLine($"[BeforeTestRun] Report path resolved: {reportPath}");

            //Initialize Extent spark reporter
            _htmlReporter = new ExtentSparkReporter(reportPath);
            _htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;

            //Attach the reporter to the extent reports
            _extent = new ExtentReports();
            _extent.AttachReporter(_htmlReporter);

            _extent.AddSystemInfo("Title", _settings.Report.Title);
            _extent.AddSystemInfo("BaseUrl", _settings.Environment.BaseUrl);
            _extent.AddSystemInfo("Testing Environment", _settings.Environment.TestingEnvironment);
            _extent.AddSystemInfo("BrowserType", _settings.Browser.Type);
            _extent.AddSystemInfo("OS", _settings.Environment.OS);
            _extent.AddSystemInfo("Author", _settings.Environment.Author);

            Console.WriteLine($"[BeforeTestRun] Extent report configured and attached.");
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Debugger launched.");
            Console.WriteLine($"Starting {scenarioContext.ScenarioInfo.Title} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");

            switch (_settings.Browser.Type.ToLower())     //Check the browser type which is in the settings.json
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());  //download chrome driver
                    var chromeOptions = new ChromeOptions();    //chrome options for headless mode execution
                    if (_settings.Browser.Headless)
                    {
                        chromeOptions.AddArgument("--headless");
                    }
                    _driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    var firefoxOptions = new FirefoxOptions();
                    if (_settings.Browser.Headless)
                    {
                        firefoxOptions.AddArgument("--headless");
                    }
                    _driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    var edgeOptions = new EdgeOptions();
                    if (_settings.Browser.Headless)
                    {
                        edgeOptions.AddArgument("--headless");
                    }
                    _driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new ArgumentException($"Unsupported Browser:{_settings.Browser.Type}");
            }
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_settings.Browser.TimeoutSeconds);
            _driver.Manage().Window.Maximize();

            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
            _objectContainer.RegisterInstanceAs(new NavigationHelper(_driver));
            _objectContainer.RegisterInstanceAs(new LoginPage(_driver));
            _objectContainer.RegisterInstanceAs(new LanguagePage(_driver));
            _objectContainer.RegisterInstanceAs(new SkillPage(_driver));
            _objectContainer.RegisterInstanceAs(new EducationPage(_driver));
            _objectContainer.RegisterInstanceAs(new CertificationPage(_driver));

            lock (_reportLock)
            {
                _test = _extent.CreateTest(scenarioContext.ScenarioInfo.Title);
                scenarioContext.Set(_test, "ExtentTest");
                Console.WriteLine($"Created test: {scenarioContext.ScenarioInfo.Title} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            var test = scenarioContext.Get<ExtentTest>("ExtentTest");
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepText = scenarioContext.StepContext.StepInfo.Text;

            Console.WriteLine($"{stepType}:{stepText} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
            if (scenarioContext.TestError == null)
            {
                test.Log(Status.Pass, $"<b>{stepType}</b>   {stepText}");
                Console.WriteLine($"Step Passed: {stepText}");
            }
            else
            {
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                string base64Screenshot = screenshot.AsBase64EncodedString;

                test.Log(Status.Fail, $"<b>{stepType}</b>   {stepText} ").AddScreenCaptureFromBase64String(base64Screenshot, "Failure Screenshot");
                Console.WriteLine($"Step failed: {stepText}");
            }
        }

        [AfterScenario]
        public void CleanUpAfterScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            try
            {
                try
                {
                    if (featureContext.FeatureInfo.Tags.Contains("language"))     //Check if the feature tag name is language
                    {
                        if (scenarioContext.TryGetValue("LanguagesToCleanup", out List<string>? languages))   //Get the value of "LanguagesToCleanup" 
                        {
                            if (languages != null && languages.Any())        //Check if the languages is not null and it has any value
                            {
                                var languagePage = _objectContainer.Resolve<LanguagePage>();   //retrieve the language page
                                foreach (var language in languages)  //Delete the languages after each scenario which we've given as input
                                {
                                    languagePage.DeleteSpecificLanguage(language);
                                }
                                Console.WriteLine($"Cleanup:Deleted {languages.Count} languages for this scenario");  //Check the count of languages deleted
                            }
                            else
                            {
                                Console.WriteLine("Clean up skipped: Language list is empty."); //Clean up skipped
                            }
                        }
                    }
                    else if (featureContext.FeatureInfo.Tags.Contains("skill"))  //Check if the feature tag name is skill
                    {
                        if (scenarioContext.TryGetValue("SkillsToCleanup", out List<string>? skills))  //Get the value of "SkillsToCleanup"
                        {
                            if (skills != null && skills.Any())
                            {
                                var skillsPage = _objectContainer.Resolve<SkillPage>();
                                foreach (var skill in skills)   //Delete the skill after each scenario 
                                {
                                    skillsPage.DeleteSpecificSkill(skill);
                                }
                                Console.WriteLine($"Cleanup:Deleted {skills.Count} skills for this scenario");
                            }
                            else
                            {
                                Console.WriteLine("Clean up skipped: Skill list is empty");
                            }
                        }
                    }
                    else if (featureContext.FeatureInfo.Tags.Contains("education"))
                    {
                        if (scenarioContext.TryGetValue("EducationToCleanup", out List<string>? educationList))
                        {
                            if (educationList != null && educationList.Any())
                            {
                                var educationPage = _objectContainer.Resolve<EducationPage>();
                                foreach (var education in educationList)
                                {
                                    educationPage.DeleteSpecificEducation(education);
                                }

                                Console.WriteLine($"Cleanup:Deleted {educationList.Count} education list for this scenario");
                            }
                            else
                            {
                                Console.WriteLine("Clean up skipped: Education list is empty");
                            }
                        }
                    }
                    else if (featureContext.FeatureInfo.Tags.Contains("certification"))
                    {
                        if (scenarioContext.TryGetValue("CertificationsToCleanup", out List<string>? certificationList))
                        {
                            if (certificationList != null && certificationList.Any())
                            {
                                var certificationPage = _objectContainer.Resolve<CertificationPage>();
                                foreach (var certification in certificationList)
                                {
                                    certificationPage.DeleteSpecificCertification(certification);
                                }
                                Console.WriteLine($"Cleanup: Deleted {certificationList.Count} certifications list for this scenario");
                            }
                            else
                            {
                                Console.WriteLine("Clean up skipped: Certification list is empty");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Clean up failed:{ex.Message}");
                }

                var driver = _objectContainer.Resolve<IWebDriver>();

                if (driver != null)
                {
                    driver?.Quit();
                    Console.WriteLine($"Finished scenario on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clean up failed:{ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            lock (_reportLock)
            {
                Console.WriteLine($"[AfterTestRun] Flushing report at {DateTime.Now}");
                _extent?.Flush();  //Write logs, steps, test results from memory into html report
                Console.WriteLine("[AfterTestRun] Report flushed successfully.");
            }
        }
    }
}