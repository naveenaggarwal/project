using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.BusinessHelper;
using MSCOM.Test.CSV;

namespace RMT.UnitTests
{
    [TestClass]
    public class OEM
    {
        #region Colors

        string oemTabColor = "rgba(44, 185, 239, 1)";

        #endregion

        #region SQLQueries

        #region ExistingOEMSQLQueries

        string getExistingOemName = "SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'";

        string getExistingOemStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[Oem] JOIN [dbo].[Status] ON [dbo].[Oem].statusID = [dbo].[Status].statusID WHERE [dbo].[Oem].oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')";

        string getExistingOemDescription = "SELECT oemDescription FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')";

        string getExistingOemAADTenant = "SELECT tenantName FROM [dbo].[AadTenant] WHERE aadTenantID = (SELECT tenantID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'))";

        string getExistingOemActiveRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[OemAgreement] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')) AND agreementID NOT IN (SELECT agreementID FROM [dbo].[Agreement] WHERE statusId = 2 OR expiryDate < GETDATE())";

        string getExistingOemTotalRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[OemAgreement] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'))";

        string getExistingOemActiveUsersCount = "SELECT COUNT(userID) FROM [dbo].[user] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')) AND userID NOT IN (SELECT userID FROM [dbo].[User] WHERE statusID = 2 OR expiryDate < GETDATE())";

        string getExistingOemTotalUsersCount = "SELECT COUNT(userID) FROM [dbo].[user] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'))";

        #endregion

        #region ExistingDisabledOEMSQLQueries

        string getExistingDisabledOemName = "SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'";

        string getExistingDisabledOemStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[Oem] JOIN [dbo].[Status] ON [dbo].[Oem].statusID = [dbo].[Status].statusID WHERE [dbo].[Oem].oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')";

        string getExistingDisabledOemDescription = "SELECT oemDescription FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')";

        string getExistingDisabledOemAADTenant = "SELECT tenantName FROM [dbo].[AadTenant] WHERE aadTenantID = (SELECT tenantID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'))";

        string getExistingDisabledOemActiveRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[OemAgreement] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')) AND agreementID NOT IN (SELECT agreementID FROM [dbo].[Agreement] WHERE statusId = 2 OR expiryDate < GETDATE())";

        string getExistingDisabledOemTotalRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[OemAgreement] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'))";

        string getExistingDisabledOemActiveUsersCount = "SELECT COUNT(userID) FROM [dbo].[user] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%')) AND userID NOT IN (SELECT userID FROM [dbo].[User] WHERE statusID = 2 OR expiryDate < GETDATE())";

        string getExistingDisabledOemTotalUsersCount = "SELECT COUNT(userID) FROM [dbo].[user] WHERE oemID = (SELECT oemID FROM [dbo].[Oem] WHERE oemName = (SELECT TOP 1 oemName FROM [dbo].[Oem] WHERE statusID = 2 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>%'))";

        #endregion

        #region UpdationQueries

        string getOemStatus = "SELECT TOP 1 statusName FROM [dbo].[Status]";

        string getAADTenantName = "SELECT TOP 1 tenantName FROM [dbo].[AadTenant]";

        string getAADNames = "SELECT tenantName FROM [dbo].[AadTenant] WHERE status <> 0";

        #endregion

        #region NewOEMSQLQueries

        string getNewOEMDetails = "SELECT COUNT(oemID) FROM [dbo].[Oem] WHERE oemName = ";

        #endregion

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
        [WorkItem(205893)]
        [TestProperty("TestCaseId", "205893")]
        public void VerifyDefaultBehaviorForOEMScreen()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["oemNameTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["descriptionTextbox"]));
                    results.Add(SeleniumWebHelper.CheckDropDownIsRendered(results[0], i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.CheckDropDownIsRendered(results[0], i["oemStatusDropDown"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["RMTAgreementsTile"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["usersTile"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["oemNameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["descriptionTextbox"], i["descriptionWatermark"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["RMTAgreementsTileCount"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["usersTileCount"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[0], i["aadTenantDropDown"], i["aadTenantDropDownText"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[0], i["oemStatusDropDown"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["oemNameTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["descriptionTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["saveButton"]));
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
        [WorkItem(205916)]
        [TestProperty("TestCaseId", "205916")]
        public void VerifyExistingOEMDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemAADTenant));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemDescription));
                    results.Add(SQLHelper.RunQuery(getExistingOemActiveRMTAgreementsCount, getExistingOemTotalRMTAgreementsCount));
                    results.Add(SQLHelper.RunQuery(getExistingOemActiveUsersCount, getExistingOemTotalUsersCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["aadTenantDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["descriptionTextbox"], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementsTile"], (string)results[4]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["usersTile"], (string)results[5]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[6], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[6]));
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
        [WorkItem(205918)]
        [TestProperty("TestCaseId", "205918")]
        public void VerifyExistingDisabledOEMDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledOemName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledOemStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledOemAADTenant));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledOemDescription));
                    results.Add(SQLHelper.RunQuery(getExistingDisabledOemActiveRMTAgreementsCount, getExistingDisabledOemTotalRMTAgreementsCount));
                    results.Add(SQLHelper.RunQuery(getExistingDisabledOemActiveUsersCount, getExistingDisabledOemTotalUsersCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["aadTenantDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["descriptionTextbox"], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementsTile"], (string)results[4]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["usersTile"], (string)results[5]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[6], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[6]));
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
        [WorkItem(207875)]
        [TestProperty("TestCaseId", "207875")]
        public void VerifyANewOEMCanbeCreatedWithDefaultValues()
        {
            string error = null;
            int iteration = 0;
            string newOEMName = DataHelper.GenerateRandomString();
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["oemNameTextbox"], newOEMName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["oemFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getAADTenantName));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[0]), i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[0], (string)results[12]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getNewOEMDetails, newOEMName, "1"));
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
        [WorkItem(205924)]
        [TestProperty("TestCaseId", "205924")]
        public void VerifyANewEnabledOEMCanbeCreatedForAnExistingAAD()
        {
            string error = null;
            int iteration = 0;
            string newOEMName = DataHelper.GenerateRandomString();
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["oemNameTextbox"], newOEMName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["oemFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getAADTenantName));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[0]), i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[0], (string)results[12]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[0]), i["oemStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[0], i["oemStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getNewOEMDetails, newOEMName, "1"));
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
        [WorkItem(205928)]
        [TestProperty("TestCaseId", "205928")]
        public void VerifyANewDisabledOEMCanbeCreatedForAnExistingAAD()
        {
            string error = null;
            int iteration = 0;
            string newOEMName = DataHelper.GenerateRandomString();
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["oemNameTextbox"], newOEMName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["oemFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getAADTenantName));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[0]), i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[0], (string)results[12]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[0]), i["oemStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[0], i["oemStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getNewOEMDetails, newOEMName, "1"));
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
        [WorkItem(205934)]
        [TestProperty("TestCaseId", "205934")]
        public void VerifyExistingOEMDetailsCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getAADTenantName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getOemStatus));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[3]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[3], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[3], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[3], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[3], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[3], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[3], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[3], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[3], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[3], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[3], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[3], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[3], i["editButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[3], i["descriptionTextbox"], i["description"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[3]), i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[3], (string)results[1]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[3]), i["oemStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[3], (string)results[2]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[3], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[3], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[3], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[3], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[3]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingOemAADTenant, (string)results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingOemStatus, (string)results[2]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingOemDescription, i["description"]));
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
        [WorkItem(205936)]
        [TestProperty("TestCaseId", "205936")]
        public void VerifyExistingOEMDescriptionCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["descriptionTextbox"], i["description"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingOemDescription, i["description"]));
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
        [WorkItem(205938)]
        [TestProperty("TestCaseId", "205938")]
        public void VerifyExistingOEMStatusCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getOemStatus));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[2]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[2], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[2], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[2], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[2], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[2]), i["oemStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[2], (string)results[1]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[2], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[2], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[2], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[2]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingOemStatus, (string)results[1]));
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
        [WorkItem(205939)]
        [TestProperty("TestCaseId", "205939")]
        public void VerifyExistingOEMAADTenantCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getAADTenantName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[2]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[2], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[2], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[2], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[2], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown((results[2]), i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[2], (string)results[1]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[2], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[2], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[2], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[2]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingOemAADTenant, (string)results[1]));
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
        [WorkItem(205964)]
        [TestProperty("TestCaseId", "205964")]
        public void VerifyClearButtonFunctionality()
        {
            string error = null;
            int iteration = 0;
            string newOemName = DataHelper.GenerateRandomString();
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryFieldTextbox"], i["mandatoryMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryFieldTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["oemNameTextbox"], newOemName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["oemFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["oemFoundTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["oemFoundTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["oemNameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["descriptionTextbox"], i["descriptionWatermark"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["RMTAgreementsTileCount"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["usersTileCount"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[1], i["aadTenantDropDown"], i["aadTenantDropDownText"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[1], i["oemStatusDropDown"]));
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
        [WorkItem(205987)]
        [TestProperty("TestCaseId", "205987")]
        public void VerifyOemNameFieldValidations()
        {
            string error = null;
            int iteration = 0;
            string invalidFormatMessage = "Please enter only allowed characters A-Z,a-z,0-9,@,-,. ,_,( and )";
            string newOemName = DataHelper.GenerateRandomString() + "#$%^@";
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryFieldTextbox"], i["mandatoryMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryFieldTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["oemNameTextbox"], newOemName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryFieldTextbox"], invalidFormatMessage));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryFieldTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["oemFoundTextbox"]));
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
        [WorkItem(206000)]
        [TestProperty("TestCaseId", "206000")]
        public void VerifyDescriptionFieldValidations()
        {
            string error = null;
            int iteration = 0;
            string newOemName = DataHelper.GenerateRandomString();
            string specialText = "测试实体名称";
            string specialCharText = "~`!@#$%^&*()_+-={}[]|\\:\";'<>,.?/";
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["oemNameTextbox"], newOemName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["oemFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["descriptionTextbox"], specialText));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["descriptionErrorTextbox"], i["errorMessage"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["descriptionTextbox"], specialCharText));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["descriptionErrorTextbox"]));
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
        [WorkItem(206612)]
        [TestProperty("TestCaseId", "206612")]
        public void VerifyAADsAreConsistentWithDatabase()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingOemName));
                    results.Add(SQLHelper.RunQueryAndReturnResults(getAADNames));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[2]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[2], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["oemTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[2], i["oemTab"], oemTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["oemNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[2], i["oemNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[2], i["oemFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[2], i["aadTenantDropDown"]));
                    results.Add(SeleniumWebHelper.CheckDropDownValues(results[2], (List<string>)results[1]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[2], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[2]));
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
