using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.BusinessHelper;
using MSCOM.Test.CSV;

namespace RMT.UnitTests
{
    [TestClass]
    public class User
    {
        #region Colors

        string userTabColor = "rgba(44, 185, 239, 1)";

        #endregion

        #region SQLQueries

        #region ExistingUserAccountSQLQueries

        string getExistingUserName = "SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1";

        string getExistingUserAccountStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[User] JOIN [dbo].[Status] ON [dbo].[User].statusID = [dbo].[Status].statusID WHERE [dbo].[User].userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1)";

        string getExistingUserAccountExpiryDate = "SELECT expiryDate FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1)";

        string getExistingUserAccountOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1))";

        string getExistingUserAccountActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[UserRole] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1)) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingUserAccountActiveRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[UserAgreement] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1)) AND agreementID NOT IN (SELECT agreementID FROM [dbo].[Agreement] WHERE statusId = 2 OR expiryDate < GETDATE())";

        #endregion

        #region DisabledUserAccountSQLQueries

        string getExistingDisabledUserName = "SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2";

        string getExistingDisabledUserAccountStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[User] JOIN [dbo].[Status] ON [dbo].[User].statusID = [dbo].[Status].statusID WHERE [dbo].[User].userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2)";

        string getExistingDisabledUserAccountExpiryDate = "SELECT expiryDate FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2)";

        string getExistingDisabledUserAccountOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2))";

        string getExistingDisabledUserAccountActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[UserRole] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2)) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingDisabledUserAccountActiveRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[UserAgreement] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2)) AND agreementID NOT IN (SELECT agreementID FROM [dbo].[Agreement] WHERE statusId = 2 OR expiryDate < GETDATE())";

        #endregion

        #region ExpiredUserAccountSQLQueries

        string getExistingExpiredUserName = "SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1 AND expiryDate < GETDATE()";

        string getExistingExpiredUserAccountStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[User] JOIN [dbo].[Status] ON [dbo].[User].statusID = [dbo].[Status].statusID WHERE [dbo].[User].userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1 AND expiryDate < GETDATE())";

        string getExistingExpiredUserAccountExpiryDate = "SELECT expiryDate FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1 AND expiryDate < GETDATE())";

        string getExistingExpiredUserAccountOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1 AND expiryDate < GETDATE()))";

        string getExistingExpiredUserAccountActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[UserRole] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1 AND expiryDate < GETDATE())) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingExpiredUserAccountActiveRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[UserAgreement] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 1 AND expiryDate < GETDATE())) AND agreementID NOT IN (SELECT agreementID FROM [dbo].[Agreement] WHERE statusId = 2 OR expiryDate < GETDATE())";

        #endregion

        #region DisbaledAndExpiredUserAccountSQLQueries

        string getExistingDisabledAndExpiredUserName = "SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2 AND expiryDate < GETDATE()";

        string getExistingDisabledAndExpiredUserAccountStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[User] JOIN [dbo].[Status] ON [dbo].[User].statusID = [dbo].[Status].statusID WHERE [dbo].[User].userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2 AND expiryDate < GETDATE())";

        string getExistingDisabledAndExpiredUserAccountExpiryDate = "SELECT expiryDate FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2 AND expiryDate < GETDATE())";

        string getExistingDisabledAndExpiredUserAccountOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2 AND expiryDate < GETDATE()))";

        string getExistingDisabledAndExpiredUserAccountActiveRolesCount = "SELECT COUNT(roleID) FROM [dbo].[UserRole] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2 AND expiryDate < GETDATE())) AND roleID NOT IN (SELECT roleID FROM [dbo].[Role] WHERE statusId = 2)";

        string getExistingDisabledAndExpiredUserAccountActiveRMTAgreementsCount = "SELECT COUNT(agreementID) FROM [dbo].[UserAgreement] WHERE userID = (SELECT userID FROM [dbo].[User] WHERE userName = (SELECT TOP 1 userName FROM [dbo].[User] WHERE userName like '%OPXPartner%' AND statusID = 2 AND expiryDate < GETDATE())) AND agreementID NOT IN (SELECT agreementID FROM [dbo].[Agreement] WHERE statusId = 2 OR expiryDate < GETDATE())";

        #endregion

        #region UpdationSQLQueries

        string getOEM = "SELECT TOP 1 oemName FROM [dbo].[Oem]";

        string getStatus = "SELECT TOP 1 statusName FROM [dbo].[Status]";

        string getExistingUserAccountExtendedExpiryDate = "SELECT expiryDate FROM [dbo].[User] WHERE userName = ";

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
        [WorkItem(205898)]
        [TestProperty("TestCaseId", "205898")]
        public void VerifyDefaultBehaviorForUserScreen()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[0], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["nameTextbox"]));
                    results.Add(SeleniumWebHelper.CheckDropDownIsRendered(results[0], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.CheckDropDownIsRendered(results[0], i["userStatusDropDown"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["expiresOnTextbox"]));
                    results.Add(SeleniumWebHelper.CheckRadioButtonIsRendered(results[0], i["never"]));
                    results.Add(SeleniumWebHelper.CheckRadioButtonIsRendered(results[0], i["expiresOn"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["showMoreDetailsLink"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["rolesTile"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["RMTAgreementsTile"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["nameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["expiresOnTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["rolesTileCount"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["RMTAgreementsTileCount"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[0], i["oemDropDown"], i["oemDropDownText"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[0], i["userStatusDropDown"]));
                    results.Add(SeleniumWebHelper.IsRadioButtonSelected(results[0], i["never"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["nameTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsRadioButtonDisabled(results[0], i["never"]));
                    results.Add(SeleniumWebHelper.IsRadioButtonDisabled(results[0], i["expiresOn"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["expiresOnTextbox"]));
                    //results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["showMoreDetailsLink"]));
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
        [WorkItem(205909)]
        [TestProperty("TestCaseId", "205909")]
        public void VerifyExistingUserAccountDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserAccountStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserAccountOEM));
                    results.Add(SQLHelper.RunQueryAndReturnDateResult(getExistingUserAccountExpiryDate));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserAccountActiveRolesCount));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserAccountActiveRMTAgreementsCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["userStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.IsRadioButtonSelected(results[6], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementsTile"], (string)results[5]));
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
        [WorkItem(205910)]
        [TestProperty("TestCaseId", "205910")]
        public void VerifyExistingDisabledUserAccountDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledUserName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledUserAccountStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledUserAccountOEM));
                    results.Add(SQLHelper.RunQueryAndReturnDateResult(getExistingDisabledUserAccountExpiryDate));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledUserAccountActiveRolesCount));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledUserAccountActiveRMTAgreementsCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["userStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.IsRadioButtonSelected(results[6], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementsTile"], (string)results[5]));
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
        [WorkItem(205909)]
        [TestProperty("TestCaseId", "205919")]
        public void VerifyExistingExpiredUserAccountDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredUserName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredUserAccountStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredUserAccountOEM));
                    results.Add(SQLHelper.RunQueryAndReturnDateResult(getExistingExpiredUserAccountExpiryDate));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredUserAccountActiveRolesCount));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredUserAccountActiveRMTAgreementsCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["userStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.IsRadioButtonSelected(results[6], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementsTile"], (string)results[5]));
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
        [WorkItem(205909)]
        [TestProperty("TestCaseId", "205921")]
        public void VerifyExistingDisabledAndExpiredUserAccountDetailsAreRendered()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredUserName));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredUserAccountStatus));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredUserAccountOEM));
                    results.Add(SQLHelper.RunQueryAndReturnDateResult(getExistingDisabledAndExpiredUserAccountExpiryDate));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredUserAccountActiveRolesCount));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingDisabledAndExpiredUserAccountActiveRMTAgreementsCount));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[6]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[6], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[6], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[6], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[6], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[6], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["userStatusDropDown"], (string)results[1]));
                    results.Add(SeleniumWebHelper.CheckDropDownText(results[6], i["oemDropDown"], (string)results[2]));
                    results.Add(SeleniumWebHelper.IsRadioButtonSelected(results[6], (string)results[3]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["rolesTile"], (string)results[4]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[6], i["RMTAgreementsTile"], (string)results[5]));
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
        [WorkItem(205945)]
        [TestProperty("TestCaseId", "205945")]
        public void VerifyExistingUserAccountDetailsCanBeUpdated()
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
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getOEM));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], i["oemDropDown"], (string)results[15]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getStatus));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["userStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], i["userStatusDropDown"], (string)results[18]));
                    results.Add(SeleniumWebHelper.ClickOnRadioButton(results[1], i["expiresOn"]));
                    results.Add(SeleniumWebHelper.SetDateFromCalendar(results[1], i["expiresOnTextbox"], modifiedNewDate));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountOEM, (string)results[15]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountStatus, (string)results[18]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountExpiryDate, newDate));
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
        [WorkItem(205953)]
        [TestProperty("TestCaseId", "205953")]
        public void VerifyExistingUserAccountOEMDetailCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getOEM));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["oemDropDown"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountOEM, (string)results[15]));
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
        [WorkItem(205955)]
        [TestProperty("TestCaseId", "205955")]
        public void VerifyExistingUserAccountStatusCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SQLHelper.RunQueryAndReturnResult(getStatus));
                    results.Add(SeleniumWebHelper.ClickOnDropDown(results[1], i["userStatusDropDown"]));
                    results.Add(SeleniumWebHelper.SelectDropDownText(results[1], i["userStatusDropDown"], (string)results[15]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountStatus, (string)results[15]));
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
        [WorkItem(205959)]
        [TestProperty("TestCaseId", "205959")]
        public void VerifyExistingUserAccountExpiryDateCanBeUpdated()
        {
            string error = null;
            int iteration = 0;
            string newDate = System.DateTime.Now.AddYears(1).ToString("M/d/yyyy");
            string modifiedNewDate = System.DateTime.Now.AddYears(1).ToString("MMM/d/yyyy");
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnRadioButton(results[1], i["expiresOn"]));
                    results.Add(SeleniumWebHelper.SetDateFromCalendar(results[1], i["expiresOnTextbox"], modifiedNewDate));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountExpiryDate, newDate));
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
        [WorkItem(205966)]
        [TestProperty("TestCaseId", "205966")]
        public void VerifyExistingUserAccountExpiryDateCanBeUpdatedToNever()
        {
            string error = null;
            int iteration = 0;
            string newDate = "";
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingUserName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnRadioButton(results[1], i["never"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountExpiryDate, newDate));
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
        [WorkItem(205980)]
        [TestProperty("TestCaseId", "205980")]
        public void VerifyExistingExpiredUserAccountExpiryDateCanBeExtended()
        {
            string error = null;
            int iteration = 0;
            string newDate = System.DateTime.Now.AddMonths(1).ToString("M/d/yyyy");
            string modifiedNewDate = System.DateTime.Now.AddMonths(1).ToString("MMM/d/yyyy");
            List<object> results = new List<object>();
            foreach (CSVDataIteration i in currentTC.DataIterations)
            {
                iteration++;
                try
                {
                    results.Add(SQLHelper.RunQueryAndReturnResult(getExistingExpiredUserName));
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[1]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[1], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["userTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIs(results[1], i["userTab"], userTabColor));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[1], i["nameTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[1], i["userNamesAutoPopulateTextbox"], (string)results[0]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[1], i["userFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["editButton"]));
                    results.Add(SeleniumWebHelper.ClickOnRadioButton(results[1], i["expiresOn"]));
                    results.Add(SeleniumWebHelper.SetDateFromCalendar(results[1], i["expiresOnTextbox"], modifiedNewDate));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[1], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["OKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[1], i["finalOKButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[1], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[1]));
                    results.Add(SQLHelper.RunQueryAndCompareResult(getExistingUserAccountExtendedExpiryDate, (string)results[0], newDate));
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
