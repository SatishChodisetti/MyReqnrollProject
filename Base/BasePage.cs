using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using MyReqnrollProject.Constants;
using System.Collections.ObjectModel;
using Reqnroll;
using MyReqnrollProject.Hooks;
using OpenQA.Selenium.Chrome;

namespace MyReqnrollProject.Base
{
    [Binding]
    /// <summary>
    /// Class to initialize driver and perform common operations
    /// </summary>
    class BasePage 
    {
        

        public IWebDriver _driver; //Webdriver object
        /// <summary>
        /// Default Basepage constructor
        /// </summary>
        public BasePage(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Type method enter the input text
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="inputText"></param>
        public void Type(By locator, String inputText)
        {
            find(locator).SendKeys(inputText);
        }
        public IWebElement find(By locator)
        {
            WebDriverWait wait= new WebDriverWait(_driver,TimeSpan.FromSeconds(Constants.BaseConstants.IMPLICITWAIT));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            return _driver.FindElement(locator);
        }

        /// <summary>
        /// To find elements using locator
        /// </summary>
        /// <param name="locator">locator</param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> findElements(By locator)
        {
            return _driver.FindElements(locator);
        }

        /// <summary>
        /// To navigate to the URL
        /// </summary>
        /// <param name="url">URL</param>
        public void Navigate(String url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// To click on WebElement
        /// </summary>
        /// <param name="locator"></param>
        public void PerformClick(By locator)
        {
            find(locator).Click();
        }

        /// <summary>
        /// To retrieve the text
        /// </summary>
        /// <param name="locator"></param>
        /// <returns>string</returns>
        public String GetText(By locator)
        {
            return find(locator).Text;
        }

        /// <summary>
        /// To check WebElement is displayed or not
        /// </summary>
        /// <param name="locator"></param>
        /// <returns>true/flase</returns>
        public Boolean IsDisplayed(By locator)
        {
            try
            {
                return find(locator).Displayed && find(locator).Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Counts number of elements
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public int GetCount(By locator)
        {
            try
            {
                return _driver.FindElements(locator).Count;
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// To perform submit operation
        /// </summary>
        /// <param name="locator"></param>
        public void Submit(By locator)
        {
            find(locator).Submit();
        }

        /// <summary>
        /// To wait for page to load
        /// </summary>
        public void WaitForPageLoad()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            WebDriverWait wait = new WebDriverWait(_driver, new TimeSpan(0, 0, BaseConstants.PAGELOADTIME));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
        /// <summary>
        /// To wait for the element dispaly
        /// </summary>
        /// <param name="Element"></param>
        /// <returns></returns>
        public bool WaitForElementToDisplay(IWebElement Element)
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(BaseConstants.EXPLICITWAIT)).Until(ElementIsVisible(Element));
            bool ElementStatus = Element.Displayed;
            return (ElementStatus);
        }

        /// <summary>
        /// To check element is visible
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static Func<IWebDriver, bool> ElementIsVisible(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    return element.Displayed;
                }
                catch (Exception)
                {
                    // If element is null, stale or if it cannot be located
                    return false;
                }
            };
        }

        /// <summary>
        /// To scroll to element visibility
        /// </summary>
        /// <param name="element"></param>
        public void ScrollToView(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].scrollIntoViewIfNeeded();", element);

        }

        /// <summary>
        /// method to perform click operation on the element
        /// </summary>
        /// <param name="Element"></param>
        public void Click(IWebElement Element)
        {
            try
            {
                if (Element != null)
                {
                    ScrollToView(Element);
                    Element.Click();
                }
            }
            catch (StaleElementReferenceException e)
            {
                try
                {
                    Console.WriteLine("Stale element expection occured, re-trying to perform Click action{0}", e);
                    ScrollToView(Element);
                    Element.Click();
                }
                catch (Exception e1)
                {
                    Console.WriteLine("Expection during click operation ../n{0}", e1.Message);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Expection during click operation ../n{0}", e.Message);

            }
        }

        /// <summary>
        /// method to select drop down by value
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="value"></param>
        public static void SelectDropDownByValue(IWebElement Element, string value)
        {
            try
            {
                SelectElement select = new SelectElement(Element);
                select.SelectByValue(value);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to select value from dropdown{0}", e);
            }
        }

        /// <summary>
        /// method to select drop down by value
        /// </summary>
        /// <param name="chevronDownIcon"></param>
        /// <param name="dropdownOptions"></param>
        /// <param name="value"></param>
        public void SelectDropDownByValue(By chevronDownIcon, By dropdownOptions, string value)
        {
            try
            {

                WaitForElementToDisplay(find(chevronDownIcon));
                Click(find(chevronDownIcon));

                ReadOnlyCollection<IWebElement> allDropdownOptions = findElements(dropdownOptions);

                foreach (IWebElement dropdownOption in allDropdownOptions)
                {

                    if (string.Equals(dropdownOption.Text.Trim(), value, StringComparison.OrdinalIgnoreCase))
                    {
                        Click(dropdownOption);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to select value from dropdown{0}", e.Message);
            }
        }

        /// <summary>
        /// To select drop down by index
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="index"></param>
        public static void SelectDropDownByIndex(IWebElement Element, int index)
        {
            try
            {
                SelectElement select = new SelectElement(Element);
                select.SelectByIndex(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// To Mouse over
        /// </summary>
        /// <param name="Element"></param>
        public void MouseOver(IWebElement Element)
        {
            Actions action = new Actions(_driver);
            action.MoveToElement(Element).Perform();
        }

        /// <summary>
        /// Methos to check element is visible
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public bool IsElementVisible(By locator, int timeoutInSeconds = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
                return element != null;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// method to enter text with Java Script
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="text"></param>
        public void EnterTextUsingJavaScript(By locator, string text)
        {
            try
            {
                var element = _driver.FindElement(locator);
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;
                jsExecutor.ExecuteScript("arguments[0].value = arguments[1];", element, text);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to enter text using JavaScript: {e.Message}");
            }
        }
        /// <summary>
        /// Clicks a web element using JavaScript to ensure it is interacted with directly.
        /// </summary>
        /// <param name="locator"></param>
        public void ClickByJavaScript(By locator)
        {
            var element = _driver.FindElement(locator);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;
            jsExecutor.ExecuteScript("arguments[0].click();", element);
        }
        /// <summary>
        /// Performs a double-click action on the specified element located by the given locator.
        /// </summary>
        /// <param name="locator"></param>
        public void DoubleClickElement(By locator)
        {
            IWebElement element = _driver.FindElement(locator);
            Actions action = new Actions(_driver);
            action.MoveToElement(element).DoubleClick().Perform();
        }
    }
}
