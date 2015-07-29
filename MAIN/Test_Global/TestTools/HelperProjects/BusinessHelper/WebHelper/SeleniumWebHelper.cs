using System;
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
        /// Creates an instance of an OpenQA.Selenium.IE.InternetExplorerDriver object and navigates to the given url
        /// </summary>
        /// <param name="url">url to login once the OpenQA.Selenium.IE.InternetExplorerDriver object gets initialized</param>
        /// <param name="overWriteTimeOut">OpenQA.Selenium.IE.InternetExplorerDriver object TimeOutThreshhold to wait 
        /// while loading the page. By default if not specified, configured value will be used 
        /// as in AutomationSettings.csv file</param>
        /// <param name="deleteCookies">If true, cookies will be deleted before navigating to the given url</param>
        /// <returns>OpenQA.Selenium.IE.InternetExplorerDriver object</returns>
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="fileName">file name using which the screenshot will be saved</param>
        /// <returns>Browser as an object. Throws a DDAStepException otherwise.</returns>
        public static object GetPageScreenShot(object browser, string fileName)
        {
            try
            {
                OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="fileName">file name using which the page source will be saved</param>
        /// <returns>Browser as an object. Throws a DDAStepException otherwise.</returns>
        public static object GetPageSource(object browser, string fileName)
        {
            try
            {
                OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// Will wait for the browser to load a page as long as it does not Time Out as configured in Automation Settings
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="key">Key to look for within the rendered text</param>
        /// <param name="parentElement">Optional element name to scope the search within said element</param>
        /// <returns>True if the key is found. Throws DDAStepException otherwise</returns>
        public static bool TextIsRendered(object browser, string key, string parentElement = "")
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="key">Optional element name to scope the search within said element</param>
        /// <returns>True if the key is NOT found. Throws DDAStepException otherwise</returns>
        public static bool TextIsNotRendered(object browser, string key, string element = "")
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetElement(object browser, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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

        public static string GetElementInnerHTML(object browser, string id)
        {
            OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)browser;
            return (string)js.ExecuteScript(string.Format("return document.getElementById(\"{0}\").innerHTML", id));
        }

        /// <summary>
        /// Checks for the image element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">the Id or "src" attribute of the element</param>
        /// <returns>Returns the element if found. Returns NULL otherwise.</returns>
        private static object GetImage(object browser, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("{0}ImageNotFound", value);

            foreach (var element in wBrowser.FindElementsByTagName("img"))
            {
                if (element.GetAttribute("title") == value)
                {
                    return element;
                }
                else if (element.GetAttribute("id") == value)
                {
                    return element;
                }
                else if (element.GetAttribute("alt") == value)
                {
                    return element;
                }
            }
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The image '{0}' was not found in the provided browser.", value));
            return null;
        }

        /// <summary>
        /// Writes the given value on the given element (based on id)
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="elementId">If no element is found with given "elementId", RDBusiness.settings file is searched for an equivalent mapping (without spaces)</param>
        /// <param name="value">value to write on text box</param>
        /// <returns>Browser as an object</returns>
        public static object WriteOnElement(object browser, string elementId, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotWriteOn{0}Element", elementId);
            OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)GetElement(browser, elementId);

            if (element == null)
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to write on text element '{0}'.", elementId));
                throw new DDA.DDAStepException(string.Format("Unable to write on text element '{0}'. Unable to find element in provided browser.", element));
            }

            OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)browser;
            js.ExecuteScript(string.Format("document.getElementById(\"{0}\").value = \"{1}\";", elementId, value));

            return wBrowser;
        }

        /// <summary>
        /// Clicks the given element (input based on Value)
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value"></param>
        /// <returns>Browser as an object</returns>
        public static object ClickOnElement(object browser, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">title or text value associated with that input element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ClickOnInputElement(object browser, string value)
        {
            string fileName = "UnableToFindInputElement";
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;

        TryBlock: try
            {
                foreach (IWebElement elementSet in wBrowser.FindElementsByTagName("input"))
                {
                    if (elementSet.GetAttribute("title") == value || elementSet.GetAttribute("text") == value || elementSet.GetAttribute("class") == value)
                    {
                        elementSet.Click();
                        return wBrowser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">Used to identify the Anchor/Link Element based on innertText, Id or title</param>
        /// <returns></returns>
        public static object GetALink(object browser, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("{0}LinkNotFound", value);

            foreach (var link in wBrowser.FindElementsByTagName("a"))
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">Used to identify the Anchor/Link Element based on innertText or Id</param>
        /// <returns>Browser as an object</returns>
        public static object ClickOnALink(object browser, string value)
        {

        NullLoop: OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">Used to identify the Anchor/Link Element based on innertText or Id</param>
        /// <returns>True if the method is able to click the element. False otherwise</returns>
        public static bool ClickOnALinkIfAvailable(object browser, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="url"></param>
        /// <returns>True if the url is the expected on. Throws DDAStepException otherwise</returns>
        public static bool PageUrlContains(object browser, string url)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="url"></param>
        /// <returns>True if the url is the expected on. Throws DDAStepException otherwise</returns>
        public static bool PageUrlDoesNotContains(object browser, string url)
        {
            try
            {
                if (PageUrlContains(browser, url))
                {
                    OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// Gets an element based on its id
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="id">The value based on which the element is fetched</param>
        /// <returns>Element as an object if found. NULL otherwise.</returns>
        private static object GetRenderedElement(object browser, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotFindElement{0}", id);
            try
            {
                return wBrowser.FindElement(OpenQA.Selenium.By.Id(id));
            }
            catch
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was not found in the provided browser.", id));
                return null;
                throw new DDA.DDAStepException(string.Format("Unable to find element with id '{0}' in provided browser.", id));
            }
        }

        /// <summary>
        /// Gets an element based on its tagName and className, when the element doesn't has an id and the className includes space
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">the tagName associated with that element</param>
        /// <param name="id">the className by which the element is fetched</param>
        /// <returns>Element if found. NULL otherwise.</returns>
        private static object GetRenderedClassElement(object browser, string value, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotFindClassElement{0}", id);
            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName(value))
            {
                if (elementSet.GetAttribute("class") == id)
                {
                    return elementSet;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to find element '{0}' by 'className'.", id));
            return null;
        }

        /// <summary>
        /// Checks for the labels
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="element">The text which needs to be checked for</param>
        /// <returns>Text if found.</returns>
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
            throw new MSCOM.DDA.DDAIterationException(string.Format("The label '{0}' was not found in the provided browser.", labelText));
        }

        /// <summary>
        /// Writes the text in a TextBox element
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="elementId">the textbox name in which the text needs to be written</param>
        /// <param name="value">the text which needs to be written in the textbox</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object WriteOnTextBox(object browser, string elementId, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
        /// Checks the element is not rendered in the browser with particular style value
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="id">the id of the element which needs to be checked</param>
        /// <param name="value">the values of the "Style" attirbute of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ElementIsNotRendered(object browser, string id, string value = "")
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("{0}ElementIsRendered", id);
            OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)GetRenderedElement(browser, id);

            if (element == null)
            {
                return wBrowser;
            }
            else if (element.Text == value || element.GetAttribute("style") == value)
            {
                return wBrowser;
            }
            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was found in the provided browser.", id));
            throw new DDA.DDAStepException(string.Format("Able to find element '{0}' in provided browser.", id));
        }

        /// <summary>
        /// Checks the element is rendered in the browser with particular style value
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="id">the id of the element which needs to be checked</param>
        /// <param name="value">the value of the "style" attribute of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ElementIsRendered(object browser, string id, string value = "")
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("{0}ElementIsNotRendered", id);
            OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)GetRenderedElement(browser, id);

            if (element == null)
            {
                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was not found in the provided browser.", id));
                throw new DDA.DDAStepException(string.Format("Unable to find element '{0}' in provided browser.", id));
            }
            else if (element.Text == value || element.GetAttribute("style") == value)
            {
                return wBrowser;
            }
            else if (element != null)
            {
                return wBrowser;
            }

            return wBrowser;
        }

        /// <summary>
        /// Checks the element with no id but className is present in the browser
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">the tagName of the element</param>
        /// <param name="id">the className of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ClassElementIsRendered(object browser, string value, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("{0}ClassElementIsNotRendered", id);
            OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)GetRenderedClassElement(browser, value, id);

            if (element != null)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was not found in the provided browser.", id));
            throw new DDA.DDAStepException(string.Format("Unable to find element '{0}' in provided browser.", id));
        }

        /// <summary>
        /// Checks the element with no id but className is not present in the browser
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">the tagName of the element</param>
        /// <param name="id">the className of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ClassElementIsNotRendered(object browser, string value, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("{0}ClassElementIsRendered", id);
            OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)GetRenderedClassElement(browser, value, id);

            if (element == null)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was found in the provided browser.", id));
            throw new DDA.DDAStepException(string.Format("Able to find element '{0}' in provided browser.", id));
        }

        /// <summary>
        /// Checks if the link is rendered in the page
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="value">the value of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object LinkIsRendered(object browser, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotFindLink{0}", value);

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName("a"))
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

        public static object ClickOnLinkByText(object browser, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotFindElement{0}", value);

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName("a"))
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

        public static object ClickOnSpanLinkByText(object browser, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotFindElement{0}", value);

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName("span"))
            {
                if ((elementSet.GetAttribute("innerText") == value || elementSet.GetAttribute("title") == value) && elementSet.GetAttribute("href") != null)
                {
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Span link element '{0}' was found in the page.", value));
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
        /// To click on an element which doesn't have an Id or a name but only has a className
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="tagname">the tag with which the element is associated with</param>
        /// <param name="className">the className of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object ClickOnClassElement(object browser, string tagname, string className)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotClickClassElement{0}", className);

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName(tagname))
            {
                if (elementSet.GetAttribute("class") == className)
                {
                    OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)browser;
                    js.ExecuteScript(string.Format("document.getElementsByClassName(\"{0}\")[0].click();", className));
                    Wait(wBrowser);

                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' could not be clicked in the provided browser.", className));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' could not be clicked in the provided browser.", className));
        }

        /// <summary>
        /// To select a particular from a drop down menu
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="id">the ID or class attribute of the drop down menu</param>
        /// <param name="value">the value to be selected in the menu</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object SelectDropDownMenu(object browser, string id, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotFindSelectElement{0}", id);

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName("select"))
            {
                if (elementSet.GetAttribute("id") == id || elementSet.GetAttribute("class") == id)
                {
                    OpenQA.Selenium.Support.UI.SelectElement elementSelect = new OpenQA.Selenium.Support.UI.SelectElement(elementSet);
                    elementSelect.SelectByText(value);

                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' could not be selected in the provided browser.", id));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element '{0}' could not be selected in the provided browser.", id));
        }

        /// <summary>
        /// To write on an "input" textbox which doesn't have an Id and only has a className which includes space
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="className">the class attribute of the textbox</param>
        /// <param name="value">the value to be written</param>
        /// <returns>Browser as an object.Throws DDAStepException otherwise.</returns>
        public static object WriteOnInputElement(object browser, string className, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("CannotFindInputElelment{0}", className);
            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName("input"))
            {
                if (elementSet.GetAttribute("class") == className)
                {
                    elementSet.Clear();
                    elementSet.SendKeys(value);

                    return wBrowser;
                }
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element with id '{0}' was not found in the provided browser.", className));
            throw new MSCOM.DDA.DDAStepException(string.Format("The element id '{0}' was not found in the provided browser.", className));
        }

        /// <summary>
        /// To check the browser URL contains a particular string or not
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="URL">the string to be compared</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckPageURLContains(object browser, string URL)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("UnableToClickSelect{0}", id);
            OpenQA.Selenium.IWebElement DataSourceDropdown = null;
            try
            {

                DataSourceDropdown = (OpenQA.Selenium.IWebElement)GetElement(browser, id);
                OpenQA.Selenium.Support.UI.SelectElement DataSourceType = new OpenQA.Selenium.Support.UI.SelectElement(DataSourceDropdown);
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

        public static object GetDivElement(object browser, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("UnableToFindDivElement{0}", value);
        TRYBlock: try
            {
                foreach (var element in wBrowser.FindElementsByTagName("div"))
                {
                    if (element.GetAttribute("title") == value || element.GetAttribute("class") == value)
                    {
                        return element;
                    }
                }

                GetPageScreenShot(wBrowser, fileName);
                GetPageSource(wBrowser, fileName);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to find DIV element '{0}' in the provided browser.", value));
                return null;

            }
            catch (NoSuchElementException)
            {
                goto TRYBlock;
            }
            catch (StaleElementReferenceException)
            {
                goto TRYBlock;
            }
        }

        public static object ClickDivElement(object browser, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("UnableToClickOnDivElement{0}", id);
            OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)GetDivElement(wBrowser, id);

        TRYBlock: try
            {
                if (element == null)
                {
                    GetPageScreenShot(wBrowser, fileName);
                    GetPageSource(wBrowser, fileName);
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": Unable to click on DIV element '{0}' in the provided browser.", id));
                    throw new DDA.DDAStepException(string.Format("Unable to click on DIV element '{0}'. Unable to find element in provided browser.", id));
                }
                else
                {
                    element.Click();
                    Wait(wBrowser);
                    return wBrowser;
                }
            }

            catch (NoSuchElementException)
            {
                Wait(wBrowser);
                goto TRYBlock;
            }

            catch (StaleElementReferenceException)
            {
                Wait(wBrowser);
                goto TRYBlock;
            }
        }

        public static object ClickElementWithXPath(object browser, string ElementXpath)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("UnableToClickElementWithXPath");
            try
            {
                WaitForAjax(wBrowser);
            TRYBLOCK: try
                {
                    OpenQA.Selenium.IWebElement element = (OpenQA.Selenium.IWebElement)wBrowser.FindElementByXPath(ElementXpath);


                    if (element != null)
                    {
                        OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)browser;
                        js.ExecuteScript("arguments[0].click();", element);
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

            return wBrowser;
        }

        /// <summary>
        /// To check that the drop down menu has some values
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="id">the id of the select drop down menu</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object IsSelectOptionNull(object browser, string id)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("UnableToFindSelectElement");

            foreach (OpenQA.Selenium.IWebElement elementSet in wBrowser.FindElementsByTagName("select"))
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
        /// <param name="browser">OpenQA.Selenium.IE.InternetExplorerDriver object</param>
        /// <param name="id">id of the dropdown menu</param>
        /// <param name="value">'Option' value which needs to be verified</param>
        /// <returns>Browser as an object. Throws DDAStepExecption otherwise.</returns>
        public static object CheckDropDownValues(object browser, string id, string value)
        {
            OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("UnableToCheckDropDownValue");
            try
            {
                OpenQA.Selenium.IWebElement elementSet = wBrowser.FindElementById(id);
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

        public static object GetButtonElement(object browser, string value)
        {
        TRYBlock: try
            {
                OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;

                foreach (var element in wBrowser.FindElementsByTagName("input"))
                {
                    if (element.GetAttribute("type") == "submit")
                    {
                        if (element.GetAttribute("title") == value)
                        {
                            return element;
                        }
                        else if (element.GetAttribute("id") == value)
                        {
                            return element;
                        }
                        else if (element.GetAttribute("value") == value)
                        {
                            return element;
                        }
                    }
                }

                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Unable to find DIV element '{0}' by 'Title' or 'Id'.", value));
                return null;

            }

            catch (StaleElementReferenceException e)
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Unable to check for the element.", value));
                goto TRYBlock;
            }
        }

        public static object ClickOnAButton(object browser, string value)
        {

        NullLoop: OpenQA.Selenium.IE.InternetExplorerDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            Wait(wBrowser);
            Wait(wBrowser);
            Wait(wBrowser);
            Wait(wBrowser);
            var button = (OpenQA.Selenium.IWebElement)GetButtonElement(wBrowser, value);
            if (button != null)
            {
                if (button.GetAttribute("id") != null && button.GetAttribute("id").Trim() != "")
                {
                    var obj = ClickOnElement(wBrowser, button.GetAttribute("id"));
                    Wait(wBrowser);
                }
                else
                {
                    button.Click();
                    WaitForAjax(wBrowser);
                }

                return wBrowser;
            }
            else
            {
                goto NullLoop;
            }
            throw new DDA.DDAStepException(string.Format("Unable to click on button '{0}'. Unable to find element in provided browser.", value));
        }

        /// <summary>
        /// Checks if the control rendered on the page is empty
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="id">id of the control element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckControlIsEmpty(object browser, string id, string value = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("ControlIsNotEmpty");

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(id));

            if (element.Text == null || element.GetAttribute("innerText") == null || element.Text == value)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The control '{0}' was not rendered empty in the provided browser.", id));
            throw new MSCOM.DDA.DDAStepException(string.Format("The control '{0}' was not rendered empty in the provided browser.", id));
        }

        /// <summary>
        /// Checks if the control rendered on the page is empty using XPath
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="elementXPath">XPath of the control element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckControlIsEmptyByXPath(object browser, string elementXPath, string value = "")
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
            string fileName = string.Format("ControlIsNotEmptyByXPath");

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.XPath(elementXPath));

            if (element.Text == null || element.GetAttribute("innerText") == null || element.Text == value)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The control '{0}' was not rendered empty in the provided browser.", elementXPath));
            throw new MSCOM.DDA.DDAStepException(string.Format("The control '{0}' was not rendered empty in the provided browser.", elementXPath));
        }

        /// <summary>
        /// Checks if a particular link is rendered in the page
        /// </summary>
        /// <param name="browser">OpenQA.Selenium.IWebDriver object</param>
        /// <param name="value">text associated with the link</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckLinkIsNotRendered(object browser, string value)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IE.InternetExplorerDriver)browser;
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

            if (element.Text == text)
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
        /// <param name="elementID">ID of the element</param>
        /// <param name="bgColor">the background color of the element</param>
        /// <returns>Browser as an object. Throws DDAStepException otherwise.</returns>
        public static object CheckElementBackgroundColor(object browser, string elementID, string bgColor)
        {
            OpenQA.Selenium.IWebDriver wBrowser = (OpenQA.Selenium.IWebDriver)browser;
            string fileName = string.Format("{0}ElementBackgroundColorMismatch", elementID);

            OpenQA.Selenium.IWebElement element = wBrowser.FindElement(By.Id(elementID));
            var color = element.GetCssValue("background-color");
            if (element.GetCssValue("background-color") == bgColor)
            {
                return wBrowser;
            }

            GetPageScreenShot(wBrowser, fileName);
            GetPageSource(wBrowser, fileName);
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(System.DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ": The element '{0}' has a different background color in the provided browser.", elementID));
            throw new DDA.DDAStepException(string.Format("The element '{0}' has a different background color in the provided browser.", elementID));
        }
    }
}