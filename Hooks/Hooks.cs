using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using Reqnroll.BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MyReqnrollProject.Utilities.ExtentReport;
using MyReqnrollProject.Utilities.GetEnvironementData;
using MyReqnrollProject.Constants;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using OpenQA.Selenium.Edge;
using Reqnroll;
using MyReqnrollProject.Base;
//using ETAF.Pages;
using System.Security.AccessControl;

namespace MyReqnrollProject.Hooks
{
    [Binding]
    public sealed class Hooks : ExtentReport
    {
        private readonly ScenarioContext? _scenarioContext;
        private IWebDriver? _driver;
        private BasePage? _basePage;
        //private LoginPage? _loginPage;
        // private TechnologyPage? _technologyPage;
        private readonly IObjectContainer _objectContainer;

        // Constructor where ScenarioContext is injected
        public Hooks(ScenarioContext scenarioContext, IObjectContainer objectContainer)
        {
            _scenarioContext = scenarioContext;
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Initialize Extent Reports
            ExtentReportInit();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Teardown Extent Reports
            ExtentReportTearDown();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            

            if (_extentReports == null)
            {
                ExtentReportInit(); // Defensive fallback
            }

            _feature = _extentReports?.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void FirstBeforeScenario(ScenarioContext _scenarioContext)
        {
            string? browser = GetEnvironementData.GetEnvData("Browser");

            switch (browser)
            {
                case "Chrome":
                    SelectBrowser(BrowserType.Chrome);
                    break;
                case "Firefox":
                    SelectBrowser(BrowserType.Firefox);
                    break;
                case "IE":
                    SelectBrowser(BrowserType.IE);
                    break;
                case "Edge":
                    SelectBrowser(BrowserType.Edge);
                    break;
                default:
                    throw new NotImplementedException("Please provide the correct Browser type in Environment.json file.");
            }
            if (!_scenarioContext.ContainsKey("driver"))
            {
                throw new InvalidOperationException("Driver is not set in ScenarioContext.");
            }
            if (_driver != null)
            {
                _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
            }
            else
            {
                throw new InvalidOperationException("WebDriver is not initialized.");
            }
            _basePage = new BasePage(_driver);
            // _loginPage = new LoginPage(_driver);  // Pass the _driver here
            // _technologyPage = new TechnologyPage(_driver);  // Pass the _driver here


            if (_scenarioContext.ScenarioInfo.Tags.Contains("ignore"))
            {
                // This will mark the scenario as skipped or pending
                _scenarioContext.Pending();  // Skips the scenario
            }

            _scenario = _feature?.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext _scenarioContext)
        {

            Console.WriteLine("Running after scenario...");
            //_driver?.Quit();
            // Check if the browser was closed manually (driver is null)
            // If the browser was closed manually, we need to flag the scenario as failed
            if (_driver == null)
            {
                // Set the custom flag to indicate the browser was closed manually
                _scenarioContext["BrowserClosedManually"] = true;

                // Mark the scenario as failed manually since the browser was closed
                _scenario?.Fail("Test failed: Browser was closed manually during the test.");
            }
            else
            {
                // Quit the driver normally if it wasn't manually closed
                _driver?.Quit();
            }




        }
        //[BeforeStep]
        //public void BeforeStep(ScenarioContext scenarioContext)
        //{

        //}

        [AfterStep]
        public void AfterStep(ScenarioContext _scenarioContext)
        {
            Console.WriteLine("Running after step....");
            string stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = _scenarioContext.StepContext.StepInfo.Text;

            // Check if the driver has been closed manually (driver is null)
            // Check if the driver has been closed manually (using the flag in ScenarioContext)
            if (_driver == null || _scenarioContext.ContainsKey("BrowserClosedManually") && (bool)_scenarioContext["BrowserClosedManually"])
            {
                // Mark the current step as failed because the browser was closed manually
                CreateStepNodeWithFailure(stepType, stepName, "Browser was closed manually.");
                return; // No further steps should be processed for this scenario
            }

            if (_scenarioContext.TestError == null)
            {
                CreateStepNode(stepType, stepName);
            }

            if (_scenarioContext.TestError != null)
            {
                CreateStepNodeWithFailure(stepType, stepName, _scenarioContext.TestError.Message);
            }

            if (_scenarioContext.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                CreateStepNodeWithSkip(stepType, stepName);
            }
        }

        private static void CreateStepNode(string stepType, string stepName)
        {
            switch (stepType)
            {
                case "Given":
                    _scenario?.CreateNode<Given>(stepName);
                    break;
                case "When":
                    _scenario?.CreateNode<When>(stepName);
                    break;
                case "Then":
                    _scenario?.CreateNode<Then>(stepName);
                    break;
                case "And":
                    _scenario?.CreateNode<And>(stepName);
                    break;
            }
        }

        private void CreateStepNodeWithFailure(string stepType, string stepName, string errorMessage)
        {
            string screenshotPath = addScreenshot(_driver, _scenarioContext);
            switch (stepType)
            {
                case "Given":
                    _scenario?.CreateNode<Given>(stepName).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                    break;
                case "When":
                    _scenario?.CreateNode<When>(stepName).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                    break;
                case "Then":
                    _scenario?.CreateNode<Then>(stepName).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                    break;
                case "And":
                    _scenario?.CreateNode<And>(stepName).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                    break;
            }
        }

        private static void CreateStepNodeWithSkip(string stepType, string stepName)
        {
            switch (stepType)
            {
                case "Given":
                    _scenario?.CreateNode<Given>(stepName).Skip("Test Skipped");
                    break;
                case "When":
                    _scenario?.CreateNode<When>(stepName).Skip("Test Skipped");
                    break;
                case "Then":
                    _scenario?.CreateNode<Then>(stepName).Skip("Test Skipped");
                    break;
                case "And":
                    _scenario?.CreateNode<And>(stepName).Skip("Test Skipped");
                    break;
            }
        }

        internal void SelectBrowser(BrowserType browserType)
        {
            string runEnvironment = GetEnvironementData.GetEnvData("RunEnvironment");
            IWebDriver? driver = null;

            switch (browserType)
            {
                case BrowserType.Chrome:
                    ChromeOptions chromeOptions = new ChromeOptions();
                    if (GetEnvironementData.GetEnvData("Headless") == "YES")
                    {
                        chromeOptions.AddArgument("--headless");
                    }
                    chromeOptions.AddArgument("--no-sandbox");
                    _driver = InitializeDriver(new ChromeDriver(chromeOptions), runEnvironment);
                    break;

                case BrowserType.Firefox:
                    FirefoxOptions fireFoxOptions = new FirefoxOptions();
                    if (GetEnvironementData.GetEnvData("Headless") == "YES")
                    {
                        fireFoxOptions.AddArgument("--headless");
                    }
                    fireFoxOptions.AddArgument("--no-sandbox");
                    _driver = InitializeDriver(new FirefoxDriver(fireFoxOptions), runEnvironment);
                    break;

                case BrowserType.Edge:
                    EdgeOptions edgeOptions = new EdgeOptions();
                    if (GetEnvironementData.GetEnvData("Headless") == "YES")
                    {
                        edgeOptions.AddArgument("--headless");
                    }
                    edgeOptions.AddArgument("--no-sandbox");
                    _driver = InitializeDriver(new EdgeDriver(edgeOptions), runEnvironment);
                    break;

                case BrowserType.IE:
                    // Handle IE initialization (if needed)
                    break;

                default:
                    throw new NotImplementedException("Unsupported browser.");
            }

            if (_driver != null)
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(BaseConstants.PAGELOADTIME);
                if (GetEnvironementData.GetEnvData("Headless") != "YES")
                {
                    _driver.Manage().Window.Maximize();
                }
                // Save driver to ScenarioContext
                _scenarioContext["driver"] = _driver;
            }
        }

        private static IWebDriver InitializeDriver(IWebDriver _driver, string runEnvironment)
        {
            if (runEnvironment == "Local")
            {
                return _driver; // Local driver
            }
            else if (runEnvironment == "Remote")
            {
                // Assume Selenium Grid setup here
                // If using RemoteWebDriver, we initialize it properly
                if (_driver is ChromeDriver || _driver is FirefoxDriver || _driver is EdgeDriver)
                {
                    // If you want to use RemoteWebDriver, we should use the capabilities specific to each browser
                    var options = _driver as ICapabilities;
                    return new RemoteWebDriver(new Uri("http://192.168.1.147:4444/wd/hub"), options);
                }
            }
            else
            {
                throw new NotImplementedException("Invalid run environment.");
            }

            return _driver;
        }

    }

    enum BrowserType
    {
        Chrome,
        Edge,
        Firefox,
        IE
    }
}
