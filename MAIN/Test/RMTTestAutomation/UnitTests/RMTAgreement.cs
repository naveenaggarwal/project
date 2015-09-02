using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.BusinessHelper;
using MSCOM.Test.CSV;

namespace RMT.UnitTests
{
    [TestClass]
    public class RMTAgreement
    {
        #region Colors

        string RMTAgreementTabColor = "rgba(44, 185, 239, 1)";

        #endregion

        #region SQLQueries

        #region ExistingRMTAgreementSQLQueries

        string getExistingRMTAgreementName = "SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE()";

        string getExistingRMTAgreementDescription = "SELECT agreementDescription FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())";

        string getExistingRMTAgreementStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[Agreement] JOIN [dbo].[Status] ON [dbo].[Agreement].statusID = [dbo].[Status].statusID WHERE [dbo].[Agreement].agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())";

        string getExistingRMTAgreementExpiryDate = "SELECT expiryDate FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())";

        string getExistingRMTAgreementOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[OemAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())))";

        string getExistingRMTAgreementActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingRMTAgreementTotalRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE()))";
  
        string getExistingRMTAgreementActiveUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())) AND userID NOT IN (SELECT userID FROM [dbo].[User] WHERE statusId = 2 OR expiryDate < GETDATE())";

        string getExistingRMTAgreementTotalUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE()))";

        #endregion

        #region ExistingDisabledRMTAgreementSQLQueries

        string getExistingDisabledRMTAgreementName = "SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE()";

        string getExistingDisabledRMTAgreementDescription = "SELECT agreementDescription FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())";

        string getExistingDisabledRMTAgreementStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[Agreement] JOIN [dbo].[Status] ON [dbo].[Agreement].statusID = [dbo].[Status].statusID WHERE [dbo].[Agreement].agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())";

        string getExistingDisabledRMTAgreementExpiryDate = "SELECT expiryDate FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())";

        string getExistingDisabledRMTAgreementOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[OemAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())))";

        string getExistingDisabledRMTAgreementActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingDisabledRMTAgreementTotalRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE()))";

        string getExistingDisabledRMTAgreementActiveUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE())) AND userID NOT IN (SELECT userID FROM [dbo].[User] WHERE statusId = 2 OR expiryDate < GETDATE())";

        string getExistingDisabledRMTAgreementTotalUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate > GETDATE()))";

        #endregion

        #region ExistingExpiredRMTAgreementSQLQueries

        string getExistingExpiredRMTAgreementName = "SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName";

        string getExistingExpiredRMTAgreementDescription = "SELECT agreementDescription FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName)";

        string getExistingExpiredRMTAgreementStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[Agreement] JOIN [dbo].[Status] ON [dbo].[Agreement].statusID = [dbo].[Status].statusID WHERE [dbo].[Agreement].agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName)";

        string getExistingExpiredRMTAgreementExpiryDate = "SELECT expiryDate FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName)";

        string getExistingExpiredRMTAgreementOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[OemAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName)))";

        string getExistingExpiredRMTAgreementActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName)) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingExpiredRMTAgreementTotalRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName))";

        string getExistingExpiredRMTAgreementActiveUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName)) AND userID NOT IN (SELECT userID FROM [dbo].[User] WHERE statusId = 2 OR expiryDate < GETDATE())";

        string getExistingExpiredRMTAgreementTotalUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE() ORDER BY agreementName))";

        #endregion

        #region ExistingDisabledAndExpiredRMTAgreementSQLQueries

        string getExistingDisabledAndExpiredRMTAgreementName = "SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE()";

        string getExistingDisabledAndExpiredRMTAgreementDescription = "SELECT agreementDescription FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE())";

        string getExistingDisabledAndExpiredRMTAgreementStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[Agreement] JOIN [dbo].[Status] ON [dbo].[Agreement].statusID = [dbo].[Status].statusID WHERE [dbo].[Agreement].agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE())";

        string getExistingDisabledAndExpiredRMTAgreementExpiryDate = "SELECT expiryDate FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE())";

        string getExistingDisabledAndExpiredRMTAgreementOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[OemAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE())))";

        string getExistingDisabledAndExpiredRMTAgreementActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE())) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingDisabledAndExpiredRMTAgreementTotalRolesCount = "SELECT COUNT(roleID) FROM [dbo].[AgreementRole] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE()))";

        string getExistingDisabledAndExpiredRMTAgreementActiveUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE())) AND userID NOT IN (SELECT userID FROM [dbo].[User] WHERE statusId = 2 OR expiryDate < GETDATE())";

        string getExistingDisabledAndExpiredRMTAgreementTotalUsersCount = "SELECT COUNT(userID) FROM [dbo].[UserAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 2 AND agreementDescription IS NOT NULL AND agreementDescription NOT LIKE '%<br/>%' AND expiryDate < GETDATE()))";

        #endregion

        #region UpdationSQLQueries

        string getOEM = "SELECT TOP 1 oemName FROM [dbo].[Oem]";

        string getStatus = "SELECT TOP 1 statusName FROM [dbo].[Status]";

        string getExistingRMTAgreementExtendedExpiryDate = "SELECT expiryDate FROM [dbo].[Agreement] WHERE agreementName = ";

        string getOEMNames = "SELECT oemName FROM [dbo].[Oem]";

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
        [WorkItem(205925)]
        [TestProperty("TestCaseId", "205925")]
        public void VerifyDefaultBehaviorForRMTAgreementScreen()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["RMTAgreementNameTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["descriptionTextbox"]));
                    results.Add(SeleniumWebHelper.CheckDropDownIsRendered(results[0], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.CheckDropDownIsRendered(results[0], i["RMTAgreementStatusDropDown"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["RMTAgreementExpiryDateTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["rolesTile"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["usersTile"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["RMTAgreementNameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["descriptionTextbox"], i["descriptionWatermark"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["RMTAgreementExpiryDateTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["rolesTileCount"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["usersTileCount"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[0], i["oemDropDown"], i["oemDropDownText"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[0], i["RMTAgreementStatusDropDown"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["RMTAgreementNameTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["descriptionTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["RMTAgreementExpiryDateTextbox"]));
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
        [WorkItem(205930)]
        [TestProperty("TestCaseId", "205930")]
        public void VerifyExistingRMTAgreementDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementOEM));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementDescription));
                    results.Add(SQLHelper.RunQuery(getExistingRMTAgreementActiveRolesCount, getExistingRMTAgreementTotalRolesCount));
                    results.Add(SQLHelper.RunQuery(getExistingRMTAgreementActiveUsersCount, getExistingRMTAgreementTotalUsersCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["RMTAgreementStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["descriptionTextbox"], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
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
        [WorkItem(205944)]
        [TestProperty("TestCaseId", "205944")]
        public void VerifyExistingDisabledRMTAgreementDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledRMTAgreementName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledRMTAgreementStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledRMTAgreementOEM));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledRMTAgreementDescription));
                    results.Add(SQLHelper.RunQuery(getExistingDisabledRMTAgreementActiveRolesCount, getExistingDisabledRMTAgreementTotalRolesCount));
                    results.Add(SQLHelper.RunQuery(getExistingDisabledRMTAgreementActiveUsersCount, getExistingDisabledRMTAgreementTotalUsersCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["RMTAgreementStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["descriptionTextbox"], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
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
        [WorkItem(205963)]
        [TestProperty("TestCaseId", "205963")]
        public void VerifyExistingExpiredRMTAgreementDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredRMTAgreementName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredRMTAgreementStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredRMTAgreementOEM));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredRMTAgreementDescription));
                    results.Add(SQLHelper.RunQuery(getExistingExpiredRMTAgreementActiveRolesCount, getExistingExpiredRMTAgreementTotalRolesCount));
                    results.Add(SQLHelper.RunQuery(getExistingExpiredRMTAgreementActiveUsersCount, getExistingExpiredRMTAgreementTotalUsersCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["RMTAgreementStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["descriptionTextbox"], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
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
        [WorkItem(205969)]
        [TestProperty("TestCaseId", "205969")]
        public void VerifyExistingDisabledAndExpiredRMTAgreementDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredRMTAgreementName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredRMTAgreementStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredRMTAgreementOEM));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredRMTAgreementDescription));
                    results.Add(SQLHelper.RunQuery(getExistingDisabledAndExpiredRMTAgreementActiveRolesCount, getExistingDisabledAndExpiredRMTAgreementTotalRolesCount));
                    results.Add(SQLHelper.RunQuery(getExistingDisabledAndExpiredRMTAgreementActiveUsersCount, getExistingDisabledAndExpiredRMTAgreementTotalUsersCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["RMTAgreementStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["descriptionTextbox"], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
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
        [WorkItem(206012)]
        [TestProperty("TestCaseId", "206012")]
        public void VerifyExistingRMTAgreementDetailsCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            string newDate = System.DateTime.Now.AddYears(2).ToString("M/d/yyyy");
            string modifiedNewDate = System.DateTime.Now.AddYears(2).ToString("MMM/d/yyyy");
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["descriptionTextbox"], i["descriptionText"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getOEM));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], (string)results[16]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getStatus));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["RMTAgreementStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], (string)results[19]));
                    results.Add(SeleniumWebHelper.SetDateFromCalendar(results[1], i["expiryDateTextbox"], modifiedNewDate));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementDescription, i["descriptionText"]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementOEM, (string)results[16]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementStatus, (string)results[19]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementExpiryDate, newDate));
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
        [WorkItem(206019)]
        [TestProperty("TestCaseId", "206019")]
        public void VerifyExistingRMTAgreementDescriptionCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["descriptionTextbox"], i["descriptionText"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementDescription, i["descriptionText"]));
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
        [WorkItem(206022)]
        [TestProperty("TestCaseId", "206022")]
        public void VerifyExistingRMTAgreementOEMDetailCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getOEM));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], (string)results[15]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementOEM, (string)results[15]));
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
        [WorkItem(206024)]
        [TestProperty("TestCaseId", "206024")]
        public void VerifyExistingRMTAgreementStatusCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getStatus));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["RMTAgreementStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], (string)results[15]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementStatus, (string)results[15]));
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
        [WorkItem(206036)]
        [TestProperty("TestCaseId", "206036")]
        public void VerifyExistingRMTAgreementExpiryDateCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            string newDate = System.DateTime.Now.AddYears(2).ToString("M/d/yyyy");
            string modifiedNewDate = System.DateTime.Now.AddYears(2).ToString("MMM/d/yyyy");
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.SetDateFromCalendar(results[1], i["expiryDateTextbox"], modifiedNewDate));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementExpiryDate, newDate));
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
        [WorkItem(206038)]
        [TestProperty("TestCaseId", "206038")]
        public void VerifyExistingExpiredRMTAgreementExpiryDateCanBeExtended()
        {
            string error = null;
            int iteration = 0;
            string newDate = System.DateTime.Now.AddMonths(2).ToString("M/d/yyyy");
            string modifiedNewDate = System.DateTime.Now.AddMonths(2).ToString("MMM/d/yyyy");
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.SetDateFromCalendar(results[1], i["expiryDateTextbox"], modifiedNewDate));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingRMTAgreementExtendedExpiryDate, (string)results[0], newDate));
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
        [WorkItem(206010)]
        [TestProperty("TestCaseId", "206010")]
        public void VerifyClearButtonFunctionality()
        {
            string error = null;
            int iteration = 0;
            string newRMTAgreementName = DataHelper.GenerateRandomString();
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryFieldTextbox"], i["mandatoryMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryFieldTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], newRMTAgreementName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["RMTAgreementFoundTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["RMTAgreementFoundTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["RMTAgreementNameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["descriptionTextbox"], i["descriptionWatermark"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["expiryDateTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["rolesTileCount"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["usersTileCount"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[1], i["oemDropDown"], i["oemDropDownText"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[1], i["RMTAgreementStatusDropDown"]));
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
        [WorkItem(205999)]
        [TestProperty("TestCaseId", "205999")]
        public void VerifyRMTAgreementNameFieldValidations()
        {
            string error = null;
            int iteration = 0;
            string invalidFormatMessage = "Please enter only allowed characters A-Z,a-z,0-9,@,-,.,_,&,;, ( and )";
            string newRMTAgreementName = DataHelper.GenerateRandomString() + "#$%^@";
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryFieldTextbox"], i["mandatoryMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryFieldTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], newRMTAgreementName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["mandatoryFieldTextbox"], invalidFormatMessage));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["mandatoryFieldTextbox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[1], i["RMTAgreementFoundTextbox"]));
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
        [WorkItem(205975)]
        [TestProperty("TestCaseId", "205975")]
        public void VerifyDescriptionFieldValidations()
        {
            string error = null;
            int iteration = 0;
            string newRMTAgreementName = DataHelper.GenerateRandomString();
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["RMTAgreementNameTextbox"], newRMTAgreementName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["RMTAgreementFoundTextbox"], i["notFoundMessage"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["descriptionTextbox"], specialText));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[0], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["descriptionErrorTextbox"], i["errorMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[0], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["descriptionTextbox"], specialCharText));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[0], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["descriptionErrorTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[0], i["oemDropDown"]));
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
        [WorkItem(205933)]
        [TestProperty("TestCaseId", "205933")]
        public void VerifyOEMsAreConsistentWithDatabase()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingRMTAgreementName));
                    results.Add(SQLHelper.RunQueryAndReturnResults(getOEMNames));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[2]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[2], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["RMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[2], i["RMTAgreementTab"], RMTAgreementTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[2], i["RMTAgreementNameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[2], i["RMTAgreementNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[2], i["RMTAgreementFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[2], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[2], i["oemDropDown"]));
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
