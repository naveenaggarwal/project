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
    public class MapUserToRoles
    {
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
        [WorkItem(181978)]
        [TestProperty("TestCaseId", "181978")]
        public void VerifyUserIsNavigatedToMapUserToRolePage()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["UserToRoleTab"]));
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
        [WorkItem(189367)]
        [TestProperty("TestCaseId", "189367")]
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["nameTextbox"]));
                    results.Add(SeleniumWebHelper.GetElementByXPath(results[0], i["RMTAgreementsTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["availableRolesTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["assignedRolesTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["assignAllRolesButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["assignSelectedRoleButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["removeSelectedRoleButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["removeAllRolesButton"]));
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
        [WorkItem(198572)]
        [TestProperty("TestCaseId", "198572")]
        public void VerifyDefaultBehaviorOfMapUserToRolePage()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    //results.Add(SeleniumWebHelper.CheckElementBackgroundColor(results[0], i["userToRoleTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["nameLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["RMTAgreementLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["availableRolesLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["assignedRolesLabel"]));
                    results.Add(SeleniumWebHelper.CheckControlIsEmpty(results[0], i["NameTextbox"]));
                    results.Add(SeleniumWebHelper.CheckControlIsEmptyByXPath(results[0], i["RMTAgreementTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["submitButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.ClickOnSpanLinkByText(results[0], i["RMTHomeLink"]));
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

    }
}
