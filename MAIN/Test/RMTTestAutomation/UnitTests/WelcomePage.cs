using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.BusinessHelper;
using MSCOM.Test.TFS;
using MSCOM.Test.CSV;
using MSCOM.DDA;

namespace RMT.UnitTests
{
    [TestClass]
    public class WelcomePage
    {
        #region Colors

        string blueColor = "rgba(9, 174, 237, 1)";

        #endregion

        #region UserName

        String UserName1 = "Hello, OPXRM@opxaad.onmicrosoft.com";

        #endregion

        #region SQLQueries

        string getEnabledUsersCountQuery = "SELECT COUNT(userID) FROM [dbo].[User] WHERE statusID = 1";
        string getTotalUsersCountQuery = "SELECT COUNT(userID) FROM [dbo].[User]";

        string getEnabledRolesCountQuery = "SELECT COUNT(roleID) FROM [dbo].[Role] WHERE statusID = 1";
        string getTotalRolesCountQuery = "SELECT COUNT(roleID) FROM [dbo].[Role]";

        string getEnabledAgreementsCountQuery = "SELECT COUNT(agreementID) FROM [dbo].[Agreement] WHERE statusID = 1";
        string getTotalAgreementsCountQuery = "SELECT COUNT(agreementID) FROM [dbo].[Agreement]";

        #endregion

        #region Attributes
        private CSVTestCase currentTC;
        #endregion

        public TestContext TestContext { get; set; }

        #region Initialization/Finalization

        [TestInitialize()]
        public void TestInitialize()
        {
            //Get TestCaseContext from the CSV file
            currentTC = new CSVTestCase(string.Format(@"{0}\IterationsData\{1}.csv", MSCOM.Test.Tools.Environment.GetTestContentLocation(), this.TestContext.Properties["TestCaseId"].ToString()));
            currentTC.ReplaceValuesInAllIterations(MSCOM.Test.Tools.Environment.GetEnvironmentValuesFilePath(MSCOM.Test.Tools.Environment.GetCurrentEnvironment()));
        }

        #endregion

        [TestMethod]
        [WorkItem(199795)]
        [TestProperty("TestCaseId", "199795")]
        public void VerifyUserIsAbleToLoginAndLogout()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0],i["url1"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199824)]
        [TestProperty("TestCaseId", "199824")]
        public void VerifyMicrosoftLogoIsRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckImageLogos(results[0], i["MSLogo"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["AddUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckImageLogos(results[0], i["MSLogo"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.CheckImageLogos(results[0], i["MSLogo"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["logoXPath"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199876)]
        [TestProperty("TestCaseId", "199876")]
        public void VerifyLogOffLinkIsRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["logoffXpath"], i["logOffText"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["AddUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["logoffXpath"], i["logOffText"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["logoffXpath"], i["logOffText"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199882)]
        [TestProperty("TestCaseId", "199882")]
        public void VerifyMicrosoftCopyrightInFooter()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["microsoftCopyRightID"], i["verifyText"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199887)]
        [TestProperty("TestCaseId", "199887")]
        public void VerifyURLWithHTTP()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199890)]
        [TestProperty("TestCaseId", "199890")]
        public void VerifyURLWithoutHTTPAndHTTPS()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199903)]
        [TestProperty("TestCaseId", "199903")]
        public void VerifyWelcomeMessageAndTiles()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["userTile"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["rolesTile"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["agreementsTile"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["OEMTile"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200019)]
        [TestProperty("TestCaseId", "200019")]
        public void VerifyUserIsRedirectedToUserPage()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["UserTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["UserTab1"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200133)]
        [TestProperty("TestCaseId", "200133")]
        public void VerifyUserIsRedirectedToRolePage()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RoleTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["RoleTab1"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200175)]
        [TestProperty("TestCaseId", "200175")]
        public void VerifyUserIsRedirectedToRMTAgreementPage()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["RMTTab1"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199830)]
        [TestProperty("TestCaseId", "199830")]
        public void VerifyUserNameIsRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["UserTextName"], UserName1));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], UserName1));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["AddUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["UserTextName"], UserName1));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["UserTextName"], UserName1));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200194)]
        [TestProperty("TestCaseId", "200194")]
        public void VerifyUserIsRedirectedToOEMPage()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["OEMTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["OEMTab1"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200004)]
        [TestProperty("TestCaseId", "200004")]
        public void VerifyUsersCountInTileWithDatabase()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQuery(getEnabledUsersCountQuery, getTotalUsersCountQuery));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["usersTileCount"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(199892)]
        [TestProperty("TestCaseId", "199892")]
        public void VerifyTopNavigation()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckTextBackgroundColorByAnchorTag(results[0], i["RMTTab"], blueColor));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["AddUpdateXpath"], i["AddUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["AssociateXpath"], i["associateTab"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["AddUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["AddUpdatehref"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["RMTTabXpath"], i["RMTTab"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["AssociateXpath"], i["associateTab"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["Associatehref"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["AddUpdateXpath"], i["AddUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["RMTTabXpath"], i["RMTTab"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200127)]
        [TestProperty("TestCaseId", "200127")]
        public void VerifyRolesCountInTileWithDatabase()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQuery(getEnabledRolesCountQuery, getTotalRolesCountQuery));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["rolesTileCount"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200173)]
        [TestProperty("TestCaseId", "200173")]
        public void VerifyAgreementsCountInTileWithDatabase()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQuery(getEnabledAgreementsCountQuery, getTotalAgreementsCountQuery));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["agreementsTileCount"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200140)]
        [TestProperty("TestCaseId", "200140")]
        public void VerifyRolesCountInTilesOnAddingANewRole()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.GetElementText(results[0], i["rolesTile"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(CollectionHelper.GenerateRandomString());
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["rolesNameTextbox"], (string)results[8]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleDescriptionTextBox"], i["roleDescription"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["roleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["SaveBtn"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SQLHelper.RunQuery(getEnabledRolesCountQuery, getTotalRolesCountQuery));
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["rolesTile"], (string)results[16]));
                    results.Add(SeleniumWebHelper.IsTextDifferent(results[0], i["rolesTile"], (string)results[5]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200151)]
        [TestProperty("TestCaseId", "200151")]
        public void VerifyRolesCountInTilesOnDisablingARole()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(CollectionHelper.GenerateRandomString());
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["rolesNameTextbox"], (string)results[7]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleDescriptionTextBox"], i["roleDescription"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["roleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.GetElementText(results[0], i["rolesTile"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["rolesNameTextbox"], (string)results[7]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["newRoleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SQLHelper.RunQuery(getEnabledRolesCountQuery, getTotalRolesCountQuery));
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["rolesTile"], (string)results[26]));
                    results.Add(SeleniumWebHelper.IsTextDifferent(results[0], i["rolesTile"], (string)results[16]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(200167)]
        [TestProperty("TestCaseId", "200167")]
        public void VerifyRolesCountInTilesOnEnablingARole()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(CollectionHelper.GenerateRandomString());
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["rolesNameTextbox"], (string)results[7]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleDescriptionTextBox"], i["roleDescription"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["roleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.GetElementText(results[0], i["rolesTile"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["rolesNameTextbox"], (string)results[7]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["newRoleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SQLHelper.RunQuery(getEnabledRolesCountQuery, getTotalRolesCountQuery));
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["rolesTile"], (string)results[26]));
                    results.Add(SeleniumWebHelper.IsTextDifferent(results[0], i["rolesTile"], (string)results[16]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(202087)]
        [TestProperty("TestCaseId", "202087")]
        public void VerifyRMTFavIconAndBrowserTitle()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.IsFavIconRendered(results[4]));
                    results.Add(SeleniumWebHelper.CheckBrowserTitle(results[4], i["homePageTitle"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.IsFavIconRendered(results[7]));
                    results.Add(SeleniumWebHelper.CheckBrowserTitle(results[7], i["addUpdatePageTitle"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.IsFavIconRendered(results[10]));
                    results.Add(SeleniumWebHelper.CheckBrowserTitle(results[10], i["associatePageTitle"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

    }
}
