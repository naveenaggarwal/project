﻿using System;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Interactions;

namespace MSCOM.BusinessHelper
{
    static public class SeleniumWebHelper
    {
        #region CONSTANTS

        public static string IE_BROWSER = "IE";
        public static string FF_BROWSER = "Firefox";
        public static string GC_BROWSER = "Chrome";
        
        public static string CurrentBrowser = "";//crossbrowser change

        #endregion

        /// <summary>
        /// Creates an instance of an OpenQA.Selenium.IWebDriver object and navigates to the given url
        /// </summary>
        /// <param name="url">url to login once the OpenQA.Selenium.IWebDriver object gets initialized</param>
        /// <param name="overWriteTimeOut">OpenQA.Selenium.IWebDriver object TimeOutThreshhold to wait 
        /// while loading the page. By default if not specified, configured value will be used 
        /// as in AutomationSettings.csv file</param>
        /// <param name="deleteCookies">If true, cookies will be deleted before navigating to the given url</param>
        /// <returns>OpenQA.Selenium.IWebDriver object</returns>
        public static object OpenWebBrowser(string browser, string url, bool deleteCookies = true)
        {
            OpenQA.Selenium.IWebDriver driver = null;

            try
            {
                if (deleteCookies || MSCOM.Test.Tools.AutomationSettings.IEBrowserClearCookies)
                {
                    MSCOM.Test.Tools.TestAgent.KillInternetExplorerInstances();
                    MSCOM.Test.Tools.TestAgent.DeleteCookies();
                }
                else
                {
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": WARNING: no cookies were deleted neither existing ie instances were terminated");
                }

                if (browser == IE_BROWSER)
                {
                    CurrentBrowser = IE_BROWSER;//crossbrowser change
                    //driver = new OpenQA.Selenium.IE.InternetExplorerDriver(string.Format(@"{0}\Selenium\", System.Environment.CurrentDirectory), new OpenQA.Selenium.IE.InternetExplorerOptions(), TimeSpan.FromSeconds(300));
                    driver = new OpenQA.Selenium.IE.InternetExplorerDriver(string.Format(@"{0}Selenium\", MSCOM.Test.Tools.Environment.GetTestContentLocation()), new OpenQA.Selenium.IE.InternetExplorerOptions(), TimeSpan.FromSeconds(300));
                    driver.Navigate().GoToUrl(url);
                }
                else if (browser == FF_BROWSER)
                {
                    CurrentBrowser = FF_BROWSER;//crossbrowser change
                    driver = new FirefoxDriver();
                    driver.Navigate().GoToUrl(url);
                }
                else if (browser == GC_BROWSER)
                {
                    CurrentBrowser = GC_BROWSER;//crossbrowser change
                    driver = new OpenQA.Selenium.Chrome.ChromeDriver(string.Format(@"{0}\Selenium\", System.Environment.CurrentDirectory), new OpenQA.Selenium.Chrome.ChromeOptions());
                    driver.Navigate().GoToUrl(url);
                }
                else
                {
                    CurrentBrowser = IE_BROWSER;//crossbrowser change
                    driver = new OpenQA.Selenium.IE.InternetExplorerDriver(string.Format(@"{0}\Selenium\", System.Environment.CurrentDirectory), new OpenQA.Selenium.IE.InternetExplorerOptions(), TimeSpan.FromSeconds(300));
                    driver.Navigate().GoToUrl(url);
                }

                return driver;
            }
            catch (WebDriverTimeoutException)
            {
                Wait(driver);
            }

            return driver;
        }

        public static bool Refresh(object browser)
        {
            OpenQA.Selenium.IWebDriver driver = (OpenQA.Selenium.IWebDriver)browser;
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(60));
            driver.Navigate().Refresh();
            return true;
        }

        /// <summary>
        /// On the given page, reuqests the given new url within the same session
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="url"></param>
        /// <returns>Browser as an object</returns>
        public static object NavigateTo(object browser, string url)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            wBrowser.Navigate().GoToUrl(url);
            return wBrowser;
        }

        /// <summary>
        /// Closes the provided instance of IE 
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <returns>Browser as an object</returns>
        public static object CloseBrowser(object browser)
        {

            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            wBrowser.Quit();
            return wBrowser;
        }

        /// <summary>
        /// Captures the screenshot of the page and saves it in .PNG format
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="fileName">file name using which the screenshot will be saved</param>
        /// <returns>Browser as an object. Throws a DDAStepException otherwise.</returns>
        public static object GetPageScreenShot(object browser, string fileName)
        {
            try
            {
                OpenQA.Selenium.Remote.RemoteWebDriver wBrowser = (OpenQA.Selenium.Remote.RemoteWebDriver)browser;
                System.IO.Directory.CreateDirectory(string.Format(@"{0}\TestLogs\ErrorScreenshots", System.Environment.CurrentDirectory));
                string formattedFileName = string.Format(@"{0}\TestLogs\ErrorScreenshots\{1}-" + System.DateTime.Now.ToString("yyyy-MM-dd HHmmss"), System.Environment.CurrentDirectory, fileName);

                wBrowser.GetScreenshot().SaveAsFile(formattedFileName, System.Drawing.Imaging.ImageFormat.Png);
                return wBrowser;
            }
            catch (Exception)
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to get the screenshot of the error.");
                throw new DDA.DDAStepException(string.Format("Unable to get the screenshot of the error."));
            }
        }

        /// <summary>
        /// Captures and saves the page source of the page in a text file
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="fileName">file name using which the page source will be saved</param>
        /// <returns>Browser as an object. Throws a DDAStepException otherwise.</returns>
        public static object GetPageSource(object browser, string fileName)
        {
            try
            {
                OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
                System.IO.Directory.CreateDirectory(string.Format(@"{0}\TestLogs\ErrorPageSource", System.Environment.CurrentDirectory));
                string formattedFileName = string.Format(@"{0}\TestLogs\ErrorPageSource\{1}-" + System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".txt", System.Environment.CurrentDirectory, fileName);

                System.IO.File.WriteAllText(formattedFileName, wBrowser.PageSource);
                return wBrowser;
            }
            catch (Exception)
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to get the page source of the page.");
                throw new DDA.DDAStepException(string.Format("Unable to get the page source of the page."));
            }
        }

        /// <summary>
        /// Waits for the browser implicitly
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <returns>Returns browser as an object after waiting implicitly for the time specified.</returns>
        public static object ImplicitlyWait(object browser)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            wBrowser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            return wBrowser;
        }

        /// <summary>
        /// Will wait for the browser to load a page as long as it does not Time Out as configured in Automation Settings
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        public static void Wait(object browser)
        {
            OpenQA.Selenium.Support.UI.IWait<OpenQA.Selenium.IWebDriver> wait = new OpenQA.Selenium.Support.UI.WebDriverWait((OpenQA.Selenium.IWebDriver)browser, TimeSpan.FromSeconds(MSCOM.Test.Tools.AutomationSettings.TestTimeOut));
            try
            {
                wait.Until(driver1 => ((OpenQA.Selenium.IJavaScriptExecutor)browser).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (System.InvalidOperationException e)
            {
                return;
            }
        }

        public static void WaitForAjax(object browser)
        {
            int seconds = 0;
            while (MSCOM.Test.Tools.AutomationSettings.TestTimeOut > seconds)
            {
                try
                {
                    var ajaxIsComplete = (bool)(browser as OpenQA.Selenium.IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                    if (ajaxIsComplete)
                        return;
                    System.Threading.Thread.Sleep(100);
                    seconds++;
                }
                catch (System.InvalidOperationException e)
                {
                    return;
                }

            }
        }

        /// <summary>
        /// Searches for a given key within the provided browser's current page rendered text
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="key">Key to look for within the rendered text</param>
        /// <param name="parentElement">Optional element name to scope the search within said element</param>
        /// <returns>True if the key is found. Throws DDAStepException otherwise</returns>
        public static bool TextIsRendered(object browser, string key, string parentElement = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)browser;
            WaitForAjax(browser);
            string fileName = string.Format("{0}TextNotFound", key);
            string innerText = (string)js.ExecuteScript("return document.documentElement.innerText");

            if (innerText.Contains(key))
            {
                return true;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The text '{0}' was not found in the provided browser.", key));
            throw new DDA.DDAStepException(string.Format("The key '{0}' was not found in the provided browser.", key));
        }

        /// <summary>
        /// Searches for a given key within the provided browser's current page rendered text
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="key">Optional element name to scope the search within said element</param>
        /// <returns>True if the key is NOT found. Throws DDAStepException otherwise</returns>
        public static bool TextIsNotRendered(object browser, string key, string element = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}TextFound", key);
            try
            {
                if (TextIsRendered(browser, key, element))
                {
                    goto FAIL;
                }
            }
            catch (DDA.DDAStepException)
            {
                return true;
            }

        FAIL:
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The text '{0}' was found in the provided browser.", key));
            throw new DDA.DDAStepException(string.Format("The key '{0}' was found in the provided browser.", key));
        }

        /// <summary>
        /// Gets an Input Element based on Value
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetElement(object browser, string id)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementNotFound", id);
            try
            {
                return wBrowser.FindElement(OpenQA.Selenium.By.Id(id));
            }
            catch
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was not found in the provided browser.", id));
                throw new DDA.DDAStepException(string.Format("Unable to find element with id '{0}' in provided browser.", id));
            }
        }

        /// <summary>
        /// Checks if an element is not rendered in the page
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">id associated with the control</param>
        /// <returns>Returns browser as an object.</returns>
        public static object CheckElementIsNotRendered(object browser, string id)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            
            try
            {
                OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(id));

                if (element == null)
                {
                    return wBrowser;
                }
            }
            catch (NoSuchElementException)
            {
                return wBrowser;
            }
            catch (ElementNotVisibleException)
            {
                return wBrowser;
            }
            catch (InvalidElementStateException)
            {
                return wBrowser;
            }

            return wBrowser;
        }

        /// <summary>
        /// Checks if cached credentials are rendered
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <returns>If the element is rendered, clicks on it and returns the browser as an object. Else return the browser as an object.</returns>
        public static object CheckIfCachedCredentialsAreRendered(object browser)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;

            try
            {
                OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id("use_another_account_link"));

                if (element != null)
                {
                    element.Click();
                    return wBrowser;
                }
                else
                {
                    return wBrowser;
                }
            }
            
            catch (InvalidElementStateException)
            {
                return wBrowser;
            }
            catch (NoSuchElementException)
            {
                return wBrowser;
            }
        }

        /// <summary>
        /// Clicks the given element (input based on Value)
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value"></param>
        /// <returns>Browser as an object</returns>
        public static object ClickOnElement(object browser, string id)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("CannotClick{0}Element", id);
            WaitForAjax(wBrowser);
            Wait(wBrowser);
            OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)GetElement(wBrowser, id);

            if (element == null)
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to click on element '{0}' in the provided browser.", element));
                throw new DDA.DDAStepException(string.Format("Unable to click on element '{0}'. Unable to find element in provided browser.", element));
            }

            OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)wBrowser;
            js.ExecuteScript(string.Format("document.getElementById(\"{0}\").click();", id));
            Wait(wBrowser);
            WaitForAjax(wBrowser);

            return wBrowser;
        }

        /// <summary>
        /// Clicks on an input element, which has no unique id
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">title or text value associated with that input element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ClickOnInputElement(object browser, string value)
        {
            string fileName = "UnableToFindInputElement";
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;

        TryBlock: try
            {
                foreach (IWebElement elementSet in wBrowser.FindElements(By.TagName("input")))
                {
                    if (elementSet.Text != null && elementSet.Text != "")
                    {
                        if (elementSet.GetAttribute("title") == value || elementSet.GetAttribute("text") == value || elementSet.GetAttribute("class") == value)
                        {
                            elementSet.Click();
                            return wBrowser;
                        }
                    }
                }
            }

            catch (ElementNotVisibleException)
            {
                goto TryBlock;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to find input element '{0}' in the provided browser.", value));
            throw new DDA.DDAStepException(string.Format("Unable to find input element '{0}'. Unable to find element in provided browser.", value));
        }

        /// <summary>
        /// Gets an Anchor/Link based on innerText, Id or title
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">Used to identify the Anchor/Link Element based on innertText, Id or title</param>
        /// <returns></returns>
        public static object GetALink(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}LinkNotFound", value);

            foreach (var link in wBrowser.FindElements(By.TagName("a")))
            {
                if (link.Text == value || link.GetAttribute("id") == value || link.GetAttribute("title") == value || link.GetAttribute("name") == value)
                {
                    return link;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to find Anchor/Link element '{0}' in the provided browser.", value));
            return null;
        }

        /// <summary>
        /// Clicks the given element (Anchor/Link based on id)
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">Used to identify the Anchor/Link Element based on innertText or Id</param>
        /// <returns>Browser as an object</returns>
        public static object ClickOnALink(object browser, string value)
        {

        NullLoop: OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("CannotClickLink{0}", value);
            WaitForAjax(wBrowser);
            var link = (OpenQA.Selenium.IWebElement)GetALink(wBrowser, value);
            if (link != null)
            {
                if (link.GetAttribute("id") != null && link.GetAttribute("id").Trim() != "")
                {
                    ClickOnElement(browser, link.GetAttribute("id"));
                    WaitForAjax(wBrowser);
                }
                else
                {
                    link.Click();
                }

                return wBrowser;
            }
            else
            {
                goto NullLoop;
            }
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to click on link '{0}' in the provided browser.", value));
            throw new DDA.DDAStepException(string.Format("Unable to click on link '{0}'. Unable to find element in provided browser.", value));
        }

        /// <summary>
        /// Clicks the given element (anchor/link based on value) without failing if not found
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">Used to identify the Anchor/Link Element based on innertText or Id</param>
        /// <returns>True if the method is able to click the element. False otherwise</returns>
        public static bool ClickOnALinkIfAvailable(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("CannotClickLink{0}", value);
            try
            {
                ClickOnALink(browser, value);
            }
            catch (DDA.DDAStepException)
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to click on link '{0}' in the provided browser.", value));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compares the given browser current url with the provided url
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="url"></param>
        /// <returns>True if the url is the expected on. Throws DDAStepException otherwise</returns>
        public static bool PageUrlContains(object browser, string url)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("PageURLDoesNotContain{0}", url);
            string cleanUrl = MSCOM.Test.Tools.StringManipulation.RemoveQSP(wBrowser.Url);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Browser 'url' was '{0}'", cleanUrl));

            if (!cleanUrl.Contains(MSCOM.Test.Tools.StringManipulation.RemoveQSP(url)))
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Browser URL is '{0}' which is not expected.", url));
                throw new MSCOM.DDA.DDAStepException(string.Format("Browser 'url' was expected to contain '{0}' but current URL is '{1}'.", url, cleanUrl));
            }

            return true;
        }

        /// <summary>
        /// Compares the given browser current url with the provided url
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="url"></param>
        /// <returns>True if the url is the expected on. Throws DDAStepException otherwise</returns>
        public static bool PageUrlDoesNotContains(object browser, string url)
        {
            try
            {
                if (PageUrlContains(browser, url))
                {
                    OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
                    string fileName = string.Format("PageURLContains{0}", url);
                    GetPageScreenShot(wBrowser, fileName);
                    GetPageSource(wBrowser, fileName);
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Browser URL contains '{0}' which is not expected.", url));
                    throw new MSCOM.DDA.DDAStepException(string.Format("Browser 'url' was expected not to contain '{0}' but current URL is '{1}'.", url, wBrowser.Url));
                }
            }
            catch (MSCOM.DDA.DDAStepException)
            {
                return true;
            }

            return true;
        }

        /// <summary>
        /// Returns the text associated with the control
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">ID of the control</param>
        /// <returns>text associated with the control. Returns NULL otherwise.</returns>
        public static string GetElementText(object browser, string id)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToFindElement{0}", id);

            try
            {
                OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(id));
                if (element != null)
                {
                    return element.Text;
                }
                else
                {
                    return null;
                }
            }
            catch (InvalidElementStateException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }

        }

        /// <summary>
        /// Formats the text associated with the control and returns it
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">ID of the control</param>
        /// <returns>Formatted text associated with the control. Returns NULL otherwise.</returns>
        public static string GetElementTextAfterFormat(object browser, string id)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToFindElement{0}", id);

            try
            { 
                OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(id));
                if (element != null)
                {
                    string elementText = element.Text;
                    int i = elementText.IndexOf('(');
                    elementText = elementText.Remove(i, 1);
                    i = elementText.IndexOf(')');
                    elementText = elementText.Remove(i, 1);
                    return elementText;
                }
                else
                {
                    return null;
                }
            }
            catch (InvalidElementStateException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }

        }

        /// <summary>
        /// Checks for the labels
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="element">The text which needs to be checked for</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object GetElementLabel(object browser, string labelText)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}LabelIsNotRendered", labelText);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("label")))
            {
                if (element.Text == labelText || element.GetAttribute("innerText") == labelText)
                {
                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The label '{0}' was not found in the provided browser.", labelText));
            throw new MSCOM.DDA.DDAStepException(string.Format("The label '{0}' was not found in the provided browser.", labelText));
        }

        /// <summary>
        /// Writes the text in a TextBox element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementId">the textbox name in which the text needs to be written</param>
        /// <param name="value">the text which needs to be written in the textbox</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object WriteOnTextBox(object browser, string elementId, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("CannotFindTextBox{0}", elementId);
            try
            {
                OpenQA.Selenium.IWebElement textElement = (OpenQA.Selenium.IWebElement)wBrowser.FindElement(OpenQA.Selenium.By.Id(elementId));
                textElement.Clear();
                textElement.SendKeys(value);
            }

            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was not found in the provided browser.", elementId));
                throw new MSCOM.DDA.DDAIterationException(string.Format("The element id '{0}' was not found in the provided browser.", elementId));
            }

            return wBrowser;
        }

        /// <summary>
        /// Checks if the link is rendered in the page
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">the value of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object LinkIsRendered(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("CannotFindLink{0}", value);

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElements(By.TagName("a")))
            {
                if (elementSet.GetAttribute("innerText") == value && elementSet.GetAttribute("href") != null)
                {
                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The link element '{0}' was not found in the provided browser.", value));
            throw new DDA.DDAStepException(string.Format("Unable to find link element '{0}' in the page.", value));
        }

        /// <summary>
        /// Clicks on a link with the help of the text associated with it
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">text associated with the link</param>
        /// <returns>Returns browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ClickOnLinkByText(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("CannotFindElement{0}", value);

            wBrowser.Navigate().Refresh();
            wBrowser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElements(By.TagName("a")))
            {
                if ((elementSet.GetAttribute("innerText") == value || elementSet.GetAttribute("title") == value) && elementSet.GetAttribute("href") != null)
                {
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Link element '{0}' was found in the page.", value));
                    elementSet.Click();
                    WaitForAjax(wBrowser);
                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The link element '{0}' was not found in the provided browser.", value));
            throw new DDA.DDAStepException(string.Format("Unable to find link element '{0}' in the page.", value));
        }

        /// <summary>
        /// To check the browser URL contains a particular string or not
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="URL">the string to be compared</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckPageURLContains(object browser, string URL)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("PageURLDoesNotContain");

            if (wBrowser.Url.Contains(URL))
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The URL of the provided browser did not match with '{0}'.", URL));
            throw new MSCOM.DDA.DDAStepException(string.Format("The URL of the provided browser did not match with {0}.", URL));
        }

        public static object SelectDropdownValue(object browser, string id, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToClickSelect{0}", id);
            OpenQA.Selenium.IWebElement SelectDropdown = null;
            try
            {

                SelectDropdown = (OpenQA.Selenium.IWebElement)GetElement(browser, id);
                OpenQA.Selenium.Support.UI.SelectElement DataSourceType = new OpenQA.Selenium.Support.UI.SelectElement(SelectDropdown);
                DataSourceType.SelectByText(value);

            }

            catch (DDA.DDAStepException)
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to select element '{0}' in the provided browser.", value));
                return false;
            }

            return wBrowser;

        }

        public static object ClickElementWithXPath(object browser, string ElementXpath)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToClickElementWithXPath");
            try
            {
                WaitForAjax(wBrowser);
            TRYBLOCK: try
                {
                    OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)wBrowser.FindElement(By.XPath(ElementXpath));


                    if (element != null)
                    {
                        OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)browser;
                        js.ExecuteScript("arguments[0].click();", element);
                        wBrowser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                        Wait(wBrowser);
                    }
                }
                catch (NoSuchElementException)
                {
                    goto TRYBLOCK;
                }
                catch (StaleElementReferenceException)
                {
                    goto TRYBLOCK;
                }
                catch (ElementNotVisibleException)
                {
                    goto TRYBLOCK;
                }
            }

            catch (DDA.DDAStepException)
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to find element with XPath '{0}' in the provided browser.", ElementXpath));
                return false;
            }
            
            wBrowser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
            return wBrowser;
        }

        /// <summary>
        /// To check that the drop down menu has some values
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">the id of the select drop down menu</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object IsSelectOptionNull(object browser, string id)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToFindSelectElement");

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElements(By.TagName("select")))
            {
                if (elementSet.GetAttribute("id") == id || elementSet.GetAttribute("class") == id)
                {
                    OpenQA.Selenium.Support.UI.SelectElement elementSelect = new OpenQA.Selenium.Support.UI.SelectElement(elementSet);
                    IList<IWebElement> selectOptions = elementSelect.Options;

                    if (selectOptions.Count <= 1)
                    {
                        GetPageScreenShot(wBrowser, fileName);
                        GetPageSource(wBrowser, fileName);
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to find element '{0}' in the provided browser.", id));
                        throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' was not found in the provided browser.", id));
                    }

                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to find input element '{0}' in the provided browser.", id));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' was not found in the provided browser.", id));
        }

        /// <summary>
        /// Checks for the values in the drop down menu
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">id of the dropdown menu</param>
        /// <param name="value">'Option' value which needs to be verified</param>
        /// <returns>Browser as an object. Throws DDAStepExecption otherwise.</returns>
        public static object CheckDropDownValues(object browser, string id, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToCheckDropDownValue");
            try
            {
                OpenQA.Selenium.IWebElement elementSet = wBrowser.FindElement(By.Id(id));
                OpenQA.Selenium.Support.UI.SelectElement elementSelect = new OpenQA.Selenium.Support.UI.SelectElement(elementSet);
                IList<IWebElement> selectOptions = elementSelect.Options;

                foreach (IWebElement element in selectOptions)
                {
                    if (element.Text == value)
                    {
                        return wBrowser;
                    }
                }
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' was not found in the provided browser.", value));
                throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' was not found in drop down menu in the provided browser.", value));

            }

            catch (DDA.DDAStepException e)
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Unable to click on WebpartEditDropdown. '{0}'", e.Message));
                return false;
            }

            return wBrowser;
        }

        /// <summary>
        /// Click on a button element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">text associated with the button element</param>
        /// <returns>Browser as an object. Throws DDAStepExecption otherwise.</returns>
        public static object ClickOnAButton(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("button")))
            {
                if (element.GetAttribute("innerText") == value || element.Text == value)
                {
                    element.Click();
                    return wBrowser;
                }
            }
            
            throw new DDA.DDAStepException(string.Format("Unable to click on button '{0}'. Unable to find element in provided browser.", value));
        }

        /// <summary>
        /// Checks if the control rendered on the page is empty
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">id of the control element</param>
        /// <returns>Returns true if the control is empty. Returns false otherwise.</returns>
        public static bool IsControlEmptyById(object browser, string id, string value = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("ControlIsNotEmpty");

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(id));

            if (element.Text == null || element.GetAttribute("innerText") == null || element.Text == value)
            {
                return true;
            }
            else
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The control '{0}' was not rendered empty in the provided browser.", id));
                throw new DDA.DDAStepException(string.Format("The element '{0}' was not rendered empty in the provided browser.", id));
            }
        }

        /// <summary>
        /// Checks if the control rendered on the page is not empty
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">id of the control element</param>
        /// <returns>Returns true if the control is not empty. Returns false otherwise.</returns>
        public static bool IsControlNotEmptyById(object browser, string id, string value = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("ControlIsEmpty");
            wBrowser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            try
            { 
                if (IsControlEmptyById(browser, id, value))
                {
                    goto FAIL;
                }
            }

            catch (Exception)
            {
                return true;
            }

        FAIL:
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The control '{0}' was rendered empty in the provided browser.", id));
            throw new DDA.DDAStepException(string.Format("The element '{0}' was rendered empty in the provided browser.", id));
        }

        /// <summary>
        /// Checks if the control rendered on the page is empty
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementXPath">XPath of the control element</param>
        /// <returns>Returns true if the control is empty. Returns false otherwise.</returns>
        public static bool IsControlEmptyByXPath(object browser, string elementXPath, string value = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("ControlIsNotEmpty");

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.XPath(elementXPath));

            if (element.Text == null || element.GetAttribute("innerText") == null || element.Text == value)
            {
                return true;
            }
            else
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The control '{0}' was not rendered empty in the provided browser.", elementXPath));
                throw new DDA.DDAStepException(string.Format("The element '{0}' was not rendered empty in the provided browser.", elementXPath));
            }
        }

        /// <summary>
        /// Checks if the control rendered on the page is not empty
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementXPath">XPath of the control element</param>
        /// <returns>Returns true if the control is not empty. Returns false otherwise.</returns>
        public static bool IsControlNotEmptyByXPath(object browser, string elementXPath, string value = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("ControlIsEmpty");
            wBrowser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            try
            {
                if (IsControlEmptyByXPath(browser, elementXPath, value))
                {
                    goto FAIL;
                }
            }

            catch (DDA.DDAStepException)
            {
                return true;
            }

        FAIL:
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The control '{0}' was rendered empty in the provided browser.", elementXPath));
            throw new DDA.DDAStepException(string.Format("The element '{0}' was rendered empty in the provided browser.", elementXPath));
        }

        /// <summary>
        /// Checks if a particular link is rendered in the page
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">text associated with the link</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckLinkIsNotRendered(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}LinkIsRendered", value);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("a")))

            if (element.Text != value || element.GetAttribute("innerText") != value)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Able to find link '{0}' in the provided browser.", value));
            throw new MSCOM.DDA.DDAStepException(string.Format("The link '{0}' was not found in the provided browser.", value));
        }

        /// <summary>
        /// Finds an element by its XPath
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="xpath">XPath of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object GetElementByXPath(object browser, string xpath)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToFindElementByXPath");
            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.XPath(xpath));

            if (element != null)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' was not rendered in the provided browser.", xpath));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' was not rendered in the provided browser.", xpath));
        }

        /// <summary>
        /// Checks the text associated with an element by its ID
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">ID of the element</param>
        /// <param name="text">text associated with the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckElementTextById(object browser, string id, string text)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToFindElementByXPath");
            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(id));

            if (element.Text == text || element.Text.Contains(text))
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' was not rendered in the provided browser.", id));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' was not rendered in the provided browser.", id));
        }

        /// <summary>
        /// Checks the text associated with an element by its XPath
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="xpath">XPath of the element</param>
        /// <param name="text">text associated with the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckElementTextByXPath(object browser, string xpath, string text)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToFindElementByXPath");
            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.XPath(xpath));

            if (element.Text == text || element.GetAttribute("innerText").Contains(text) || element.Text.Contains(text))
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' was not rendered in the provided browser.", xpath));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' was not rendered in the provided browser.", xpath));
        }

        /// <summary>
        /// Clicks on an OrderedList element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">ID of the element to be clicked</param>
        /// <param name="value">Text value of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ClickOnOrderedListElement(object browser, string id, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToClickOnOrderedListElement{0}", value);

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(id));
            foreach (OpenQA.Selenium.IWebElement reqElement in wBrowser.FindElements(By.TagName("li")))
            {
                if (reqElement.Text == value || reqElement.GetAttribute("innerText") == value)
                {
                    reqElement.Click();
                    wBrowser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                    Wait(browser);
                    WaitForAjax(browser);
                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' was not selected in the provided browser.", value));
            throw new MSCOM.DDA.DDAStepException(string.Format("The value '{0}' could not be selected in the provided browser.", value));
        }

        /// <summary>
        /// Switches the control to an alert popup and accepts the popup
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver browser</param>
        /// <param name="text1">text rendered in the popup</param>
        /// <param name="text2"></param>
        /// <param name="buttonName"></param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object SwitchToPopUpAndVerify(object browser, string text1, string text2 = "", string buttonName = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToFindPopUp");

            OpenQA.Selenium.IAlert alertElement = wBrowser.SwitchTo().Alert();
            if (alertElement.Text.Contains(text1))
            {
                alertElement.Accept();
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The alert popup was not rendered in the provided browser."));
            throw new MSCOM.DDA.DDAStepException(string.Format("The popup was not found in the provided browser."));
        }

        /// <summary>
        /// Checks if a particular control is enabled
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementID">ID of the element</param>
        /// <returns>Returns true if the element is enabled. Throws DDAStepException otherwise.</returns>
        public static bool ElementIsEnabled(object browser, string elementID)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementIsNotEnabled", elementID);

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(elementID));
            if (element.Enabled)
            {
                return true;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' was not enabled in the provided browser.", elementID));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' was not enabled in the provided browser.", elementID));
        }

        /// <summary>
        /// Checks if a particular element is disabled
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementID">ID of the element</param>
        /// <returns>Returns true if the element is disabled. Throws DDAStepException otherwise.</returns>
        public static bool ElementIsDisabled(object browser, string elementID)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementIsNotDisabled", elementID);
            try
            {
                if (ElementIsEnabled(browser, elementID))
                {
                    goto FAIL;
                }
            }
            catch (DDA.DDAStepException)
            {
                return true;
            }

        FAIL:
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' was not disabled in the provided browser.", elementID));
            throw new DDA.DDAStepException(string.Format("The element '{0}' was not disabled in the provided browser.", elementID));
        }

        /// <summary>
        /// Checks the background color for the element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementText">text associated with the element</param>
        /// <param name="bgColor">the background color of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckElementBackgroundColorByText(object browser, string elementText, string bgColor)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementBackgroundColorMismatch", elementText);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("li")))
            {
                if ((element.Text.Contains(elementText) || element.GetAttribute("innerText").Contains(elementText)) && (element.GetCssValue("background-color") == bgColor))
                {
                    return wBrowser;
                }
            }
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' has a different background color in the provided browser.", elementText));
            throw new DDA.DDAStepException(string.Format("The element '{0}' has a different background color in the provided browser.", elementText));
        }

        /// <summary>
        /// Checks the background color for the element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementID">ID of the element</param>
        /// <param name="bgColor">the background color of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckElementBackgroundColorIs(object browser, string elementID, string bgColor)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementBackgroundColorMismatch", elementID);

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(elementID));
            if (element.GetCssValue("background-color") == bgColor)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' has a different background color in the provided browser.", elementID));
            throw new DDA.DDAStepException(string.Format("The element '{0}' has a different background color in the provided browser.", elementID));
        }

        /// <summary>
        /// Checks the background color for the element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementID">ID of the element</param>
        /// <param name="bgColor">the background color of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckElementBackgroundColorIsNot(object browser, string elementID, string bgColor)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementBackgroundColorMismatch", elementID);

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(elementID));
            if (element.GetCssValue("background-color") != bgColor)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' has a different background color in the provided browser.", elementID));
            throw new DDA.DDAStepException(string.Format("The element '{0}' has a different background color in the provided browser.", elementID));
        }

        /// <summary>
        /// Checks the background color for the link element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="linkText">text associated with the link</param>
        /// <param name="bgColor">the background color of the link</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckLinkBackgroundColorIsNot(object browser, string linkText, string bgColor)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementBackgroundColorMismatch", linkText);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("a")))
            {
                if (element.GetAttribute("href").Contains(linkText) && element.GetCssValue("background-color") != bgColor)
                {
                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The link '{0}' has a different background color in the provided browser.", linkText));
            throw new DDA.DDAStepException(string.Format("The element '{0}' has a different background color in the provided browser.", linkText));
        }

        /// <summary>
        /// Gets the values rendered in the textbox
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <returns>the values as a list</returns>
        public static List<string> GetTextboxValues(object browser)
        {
            List<string> values = new List<string>();
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElements(By.ClassName("select2-selection__choice")))
            {
                if (!(elementSet.GetAttribute("title").Contains("(Disabled)")))
                {
                    values.Add(elementSet.GetAttribute("title"));
                }
            }

            if (values.Count > 0)
            {
                return values;
            }
            else
            {
                throw new DDA.DDAStepException("The values could not be fetched from the textbox.");
            }
        }

        /// <summary>
        /// Checks if the 'Select' drop down menu data is sorted
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="selectID">ID of the 'Select' control</param>
        /// <returns>Return true if the data is sorted. Throws DDAStepException otherwise.</returns>
        public static bool IsDataSorted(object browser, string selectID)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}DataIsNotSorted", selectID);
            int n = 0;
            List<string> dropDownData = new List<string>();
            
            OpenQA.Selenium.IWebElement elementSet = wBrowser.FindElement(By.Id(selectID));
            OpenQA.Selenium.Support.UI.SelectElement elementSelect = new OpenQA.Selenium.Support.UI.SelectElement(elementSet);
            IList<IWebElement> selectOptions = elementSelect.Options;

            foreach (IWebElement element in selectOptions)
            {
                if (element.Text != null)
                {
                    dropDownData.Add(element.Text);
                }
            }

            List<string> sortedDropDownData = new List<string>(dropDownData);
            dropDownData.Sort();
            for (int i = 0; i < dropDownData.Count; i++)
            {
                if (sortedDropDownData[i].Equals(dropDownData[i]))
                {
                    n++;
                }
            }

            if (n == sortedDropDownData.Count)
            {
                return true;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The '{0}' data was not sorted in the provided browser.", selectID));
            throw new MSCOM.DDA.DDAStepException(string.Format("The '{0}' data was not sorted in drop down menu in the provided browser.", selectID));
        }

        /// <summary>
        /// Checks if a particualr checkbox is deselected
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementText">text associated with the textbox</param>
        /// <returns>True if the checkbox is deselected. Throws DDAStepException otherwise.</returns>
        public static bool IsCheckboxDeselected(object browser, string elementText)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}CheckboxIsSelected", elementText);

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElements(By.TagName("li")))
            {
                if (elementSet.GetAttribute("innerText").Contains(elementText))
                {
                    if (elementSet.GetAttribute("className") == "multiselect-item multiselect-all")
                    {
                        return true;
                    }
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The '{0}' checkbox was selected in the provided browser.", elementText));
            throw new MSCOM.DDA.DDAStepException(string.Format("The '{0}' checkbox was selected in the provided browser.", elementText));
        }

        /// <summary>
        /// Checks the color of a text
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementText">text associated with the element</param>
        /// <param name="Color">the background color of the text</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckTextBackgroundColorIs(object browser, string elementText, string Color)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}TextColorMismatch", elementText);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("option")))
            {
                if (element.Text == elementText)
                {
                    if (element.GetCssValue("color") == Color)
                    {
                        return wBrowser;
                    }
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' has a different background color in the provided browser.", elementText));
            throw new DDA.DDAStepException(string.Format("The element '{0}' has a different background color in the provided browser.", elementText));
        }

        /// <summary>
        /// Checks the text of dropdown value
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementText">text associated with the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckDropDownValueText(object browser, string elementText)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}TextNotFiltered", elementText);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("option")))
            {
                if (element.Text == elementText)
                {
                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' is not filtered in the provided browser.", elementText));
            throw new DDA.DDAStepException(string.Format("The element '{0}' is not filtered in the provided browser.", elementText));
        }

        /// <summary>
        /// Checking the Logo
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">text associated with the image element</param>
        /// <returns>Browser as an object. Throws DDAStepExecption otherwise.</returns>
        public static object CheckImageLogos(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ImageMismatch", value);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("img")))
            {
                if (element.GetAttribute("innerText") == value || element.Text == value || element.GetAttribute("Title") == value)
                {
                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' has a different background color in the provided browser.", value));
            throw new DDA.DDAStepException(string.Format("The element '{0}' not rendered in the provided browser.", value));
        }

        /// <summary>
        /// Checks the color of a text by using anchor tag
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementText">text associated with the element</param>
        /// <param name="Color">the background color of the text</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckTextBackgroundColorByAnchorTag(object browser, string elementText, string Color)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}TextColorMismatch", elementText);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("a")))
            {
                if (element.Text == elementText)
                {
                    if (element.GetCssValue("color") == Color)
                    {
                        return wBrowser;
                    }
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' has a different background color in the provided browser.", elementText));
            throw new DDA.DDAStepException(string.Format("The element '{0}' has a different background color in the provided browser.", elementText));
        }

        /// <summary>
        /// Checks if the text assciated with a control is similar to the input parameter
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementId">ID associated with the control</param>
        /// <param name="text">text to be compared</param>
        /// <returns>True if the text is different. Returns false otherwise.</returns>
        public static bool IsTextDifferent(object browser, string elementId, string text)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}TextIsSame", text);

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(elementId));
            if (element.Text != text)
            {
                return true;
            }
            else
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The text '{0}' is same as the element text in the provided browser.", text));
                throw new DDA.DDAStepException("The text is same in the provided browser.");
            }
        }

        /// <summary>
        /// Checks the browser title
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="browserTitle">title of the browser window</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckBrowserTitle(object browser, string browserTitle)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("BrowserTitleIsNot{0}", browserTitle);

            if (wBrowser.Title == browserTitle || wBrowser.Title.Contains(browserTitle))
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The browser title is not '{0}'.", browserTitle));
            throw new DDA.DDAStepException(string.Format("The window title is not '{0}' in the provided browser.", browserTitle));
        }
        
        /// <summary>
        /// Checks if the Portal faviocn is rendered
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <returns>True if the favicon is rendered. Throws DDAStepException otherwise.</returns>
        public static bool IsFavIconRendered (object browser)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = "FavIconIsNotRendered";

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("link")))
            {
                if (element.GetAttribute("href").Contains("Favicon.ico"))
                {
                    return true;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The favicon is not rendered for the portal."));
            throw new DDA.DDAStepException("The favicon for the portal is not rendered in the provided browser.");
        }

        /// <summary>
        /// Checks if the text associated with a control exceeds the specified limit
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementID">ID of the control</param>
        /// <param name="count">maximum number of characters that are allowed in the control</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckControlCharactersCount(object browser, string elementID, string count)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ControlCharacterCountExceeds", elementID);
            
            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(elementID));
            string elementText = element.Text;

            if (elementText.Length.ToString() == count)
            {
                return wBrowser;
            }
            else
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The text in '{0}' textbox exceeds '{1}' characters.", elementID, count));
                throw new DDA.DDAStepException(string.Format("The text in '{0}' textbox exceeds '{1}' characters.", elementID, count));
            }
        }

        /// <summary>
        /// Checks the number of auto-populated values
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckCountOfAutoPopulatedValues(object browser, string autopopulateId)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            int count = 0;
            string fileName = "AutoPopulateContainsMoreThanTenValues";

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("ul")))
            {
                if (element.GetAttribute("id") == autopopulateId)
                {
                    foreach (OpenQA.Selenium.IWebElement elementSet in element.FindElements(By.TagName("li")))
                    {
                        if (elementSet.GetAttribute("id").Contains("ui-id-"))
                        {
                            count++;
                        }
                    }
                }
            }
            if (count <= 10)
            {
                return wBrowser;
            }
            else
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": More than ten values are auto-populated."));
                throw new DDA.DDAStepException("More than ten values are auto-populated in the provided browser.");
            }
        }

        /// <summary>
        /// Selects the specified value from the list of auto-populated values
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver wBrowser object</param>
        /// <param name="text">value which needs to be selected</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object SelectAutoPopulateValue(object browser, string autopopulateId, string text)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("UnableToSelect{0}FromAutoPopulate", text);

            foreach (OpenQA.Selenium.IWebElement element in wBrowser.FindElements(By.TagName("ul")))
            {
                if (element.GetAttribute("id") == autopopulateId)
                {
                    foreach (OpenQA.Selenium.IWebElement elementSet in element.FindElements(By.TagName("li")))
                    {
                        if (elementSet.Text == text || elementSet.GetAttribute("innerText").Contains(text))
                        {
                            elementSet.Click();
                            return wBrowser;
                        }
                    }
                }
            }
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The '{0}' auto-populated value could not be selected.", text));
            throw new DDA.DDAStepException(string.Format("The '{0}' auto-populated value could not be selected in the provided browser.", text));
        }

    }
}