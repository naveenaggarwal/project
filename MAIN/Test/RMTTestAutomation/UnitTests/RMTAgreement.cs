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

        string getExistingRMTAgreementName = "SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>% AND expiryDate > GETDATE()";

        string getExistingRMTAgreementDescription = "SELECT agreementDescription FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>% AND expiryDate > GETDATE())";

        string getExistingRMTAgreementStatus = "SELECT [dbo].[Status].statusName FROM [dbo].[Agreement] JOIN [dbo].[Status] ON [dbo].[Agreement].statusID = [dbo].[Status].statusID WHERE [dbo].[Agreement].agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>% AND expiryDate > GETDATE())";

        string getExistingRMTAgreementExpiryDate = "SELECT expiryDate FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>% AND expiryDate > GETDATE())";

        string getExistingRMTAgreementOEM = "SELECT oemName FROM [dbo].[Oem] WHERE oemID = (SELECT oemID FROM [dbo].[OemAgreement] WHERE agreementID = (SELECT agreementID FROM [dbo].[Agreement] WHERE agreementName = (SELECT TOP 1 agreementName FROM [dbo].[Agreement] WHERE statusID = 1 AND oemDescription IS NOT NULL AND oemDescription NOT LIKE '%<br/>% AND expiryDate > GETDATE())))";

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

    }
}
