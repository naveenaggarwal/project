using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.BusinessHelper;
using MSCOM.Test.CSV;

namespace RMT.UnitTests
{
    [TestClass]
    public class OnboardToAAD
    {
        #region Colors

        string onboardToAADTabColor = "rgba(246, 152, 67, 1)";

        string oemTabColor = "rgba(44, 185, 239, 1)";

        #endregion

        #region SQLQueries

        string getExistingAADTenantName = "SELECT TOP 1 tenantName FROM [dbo].[AADTenant] WHERE status = 1";

        string updateExistingAADTenantDetails = "UPDATE [dbo].[AADTenant] SET status = 0 WHERE tenantName LIKE 'opxPartner2%'";

        string getExistingOemName = "SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'";

        string getTenantNameForOem = "SELECT tenantName FROM [dbo].[AadTenant] WHERE aadTenantID = (SELECT aadTenantID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'))";

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
        [WorkItem(209315)]
        [TestProperty("TestCaseId", "209315")]
        public void VerifyDefaultBehaviorForOnboardToAADScreen()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["aadTenantTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["aadAdminTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["signUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["aadTenantTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["aadAdminTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["aadTenantTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["aadAdminTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["signUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(209322)]
        [TestProperty("TestCaseId", "209322")]
        public void VerifyExistingAADTenantDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingAADTenantName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["aadTenantNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["aadTenantFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[1], i["aadAdminTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[1], i["signUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(209329)]
        [TestProperty("TestCaseId", "209329")]
        public void VerifyANewAADTenantCanBeOnboardedToRMTOnAccept()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    SQLHelper.RunDMLQuery(updateExistingAADTenantDetails);
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], i["newAADTenantName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["aadTenantFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["acceptButton"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url3"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["homeLink"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url1"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url4"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["oemLookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], i["newAADTenantName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getTenantNameForOem, i["newAADTenantName"]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(209333)]
        [TestProperty("TestCaseId", "209333")]
        public void VerifyANewAADTenantIsNotOnboardedToRMTOnDecline()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    SQLHelper.RunDMLQuery(updateExistingAADTenantDetails);
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["aadTenantTextbox"], i["newAADTenantName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["aadTenantFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["declineButton"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["homeLink"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url1"]));
                    SeleniumWebHelper.GoBackToPreviousPage(results[0]);
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["onboardToAADLink"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(209334)]
        [TestProperty("TestCaseId", "209334")]
        public void VerifyNonAADAdminCannotOnboardNewAADTenantToRMT()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    SQLHelper.RunDMLQuery(updateExistingAADTenantDetails);
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["aadTenantTextbox"], i["newAADTenantName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["aadTenantFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["aadAdminTextbox"], i["aadAdminName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["exceptionElement"], i["exceptionMessage"]));
                    SeleniumWebHelper.GoBackToPreviousPage(results[0]);
                    SeleniumWebHelper.GoBackToPreviousPage(results[0]);
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(209341)]
        [TestProperty("TestCaseId", "209341")]
        public void VerifyClearButtonFunctionality()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingAADTenantName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryTextbox"], i["mandatoryMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], i["newAADTenantName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["aadTenantFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadTenantFoundTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["aadTenantNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["aadTenantFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadTenantFoundTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadTenantTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadAdminTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(209343)]
        [TestProperty("TestCaseId", "209343")]
        public void VerifyAADTenantFieldValidations()
        {
            string error = null;
            int iteration = 0;
            string newAADTenantName = "testPartner.onmicrosoft.com" + "~`!#$%^&*()_";
            string invalidFormatMessage = "Please enter only allowed characters A-Z,a-z,. and 0-9";
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingAADTenantName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryTextbox"], i["mandatoryMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], newAADTenantName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryTextbox"], invalidFormatMessage));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["aadTenantNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["aadTenantFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadTenantFoundTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadTenantTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadAdminTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(209344)]
        [TestProperty("TestCaseId", "209344")]
        public void VerifyAADAdminFieldValidations()
        {
            string error = null;
            int iteration = 0;
            string aadAdminName = "RMTAdmin@testPartner.onmicrosoft.com" + "`$%&*()_";
            string invalidFormatMessage = "Please enter only allowed Characters A-Z, a-z, 0-9, ., -, !, #, ^, ~ and @";
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingAADTenantName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["onboardToAADTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["onboardToAADTab"], onboardToAADTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], i["newAADTenantName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["aadTenantFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadAdminTextbox"], aadAdminName));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryTextbox"], invalidFormatMessage));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadTenantTextbox"], i["newAADTenantName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["aadTenantFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["aadAdminTextbox"], ""));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryTextbox"], i["mandatoryMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadTenantFoundTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadTenantTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["aadAdminTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Clear();
                }
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

    }
}
