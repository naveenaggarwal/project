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
    public class MapRMTAgreementToRole
    {
        #region SQLQueries

        string mapRMTAgreementToRoleQuery = "SELECT Role.roleName FROM AgreementRole JOIN Role ON AgreementRole.roleID = Role.roleID JOIN Agreement ON Agreement.agreementID = AgreementRole.agreementID WHERE agreementName = 'ocehp'";
        string getEnabledRolesForAgreement = "SELECT Role.roleName FROM AgreementRole JOIN Role ON AgreementRole.roleID = Role.roleID JOIN Agreement ON Agreement.agreementID = AgreementRole.agreementID WHERE agreementName = 'ocehp' AND Role.statusID = 1";

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
        [WorkItem(187759)]
        [TestProperty("TestCaseId", "187759")]
        public void VerifyUserIsNavigatedToMapRMTAgreementToRolePage()
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
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
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
        [WorkItem(195143)]
        [TestProperty("TestCaseId", "195143")]
        public void VerifyMapRMTAgreementToRolePageAccess()
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
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["userNameTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["passwordTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckLinkIsNotRendered(results[0], i["logOff"]));
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
        [WorkItem(189368)]
        [TestProperty("TestCaseId", "189368")]
        public void VerifyAllControlsAreRendered()
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
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["RMTAgreementTextbox"]));
                    results.Add(SeleniumWebHelper.GetElementByXPath(results[0], i["rolesTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["submitButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["clearButton"]));
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
        [WorkItem(193081)]
        [TestProperty("TestCaseId", "193081")]
        public void VerifySubmitButtonFunctionality()
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
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["RMTAgreementTextbox"], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["rolesTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnOrderedListElement(results[0], i["rolesDropDown"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["submitButton"]));
                    //results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["alertPopUp"], i["text1"]));
                    //results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.CheckControlIsEmpty(results[0], i["RMTAgreementTextbox"]));
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
        [WorkItem(187785)]
        [TestProperty("TestCaseId", "187785")]
        public void VerifyOnlyEnabledRolesAreRendered()
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
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["RMTAgreementTextbox"], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetTextboxValues(results[0]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompare(getEnabledRolesForAgreement, (object)results[9]));
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
        [WorkItem(187792)]
        [TestProperty("TestCaseId", "187792")]
        public void VerifyRolesAreMappedToRMTAgreementInDatabase()
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
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["RMTAgreementTextbox"], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["rolesTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnOrderedListElement(results[0], i["rolesDropDown"], i["roleName1"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["rolesTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnOrderedListElement(results[0], i["rolesDropDown"], i["roleName2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["submitButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    //results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["alertPopUp"], i["text1"]));
                    results.Add(SeleniumWebHelper.CheckControlIsEmpty(results[0], i["RMTAgreementTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompare(mapRMTAgreementToRoleQuery, i["roleName1"], i["roleName2"]));
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
