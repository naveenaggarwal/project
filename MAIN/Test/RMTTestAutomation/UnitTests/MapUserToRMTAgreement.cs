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
    public class MapUserToRMTAgreement
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
        [WorkItem(189406)]
        [TestProperty("TestCaseId", "189406")]
        public void VerifyUserIsNavigatedToMapUserToRMTAgreementPage()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRMTAgreementTab"]));
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
        [WorkItem(195169)]
        [TestProperty("TestCaseId", "195169")]
        public void VerifyMapUserToRMTAgreementPageAccess()
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
        [WorkItem(189407)]
        [TestProperty("TestCaseId", "189407")]
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["NameTextbox"]));
                    results.Add(SeleniumWebHelper.GetElementByXPath(results[0], i["RMTAgreementTextbox"]));
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
        [WorkItem(193090)]
        [TestProperty("TestCaseId", "193090")]
        public void VerifyDefaultBehaviorOfMapUserToRMTAgreementPage()
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
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["associateTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIsNot(results[0], i["userToRMTAgreementTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["nameLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["RMTAgreementsLabel"]));
                    results.Add(SeleniumWebHelper.CheckControlIsEmpty(results[0], i["NameTextbox"]));
                    results.Add(SeleniumWebHelper.CheckControlIsEmptyByXPath(results[0], i["RMTAgreementsTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["submitButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIsNot(results[0], i["RMTAgreementToRoleTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIsNot(results[0], i["userToRoleTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTHomeLink"]));
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
        [WorkItem(193091)]
        [TestProperty("TestCaseId", "193091")]
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTAgreementTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnOrderedListElement(results[0], i["agreementsDropDown"], i["agreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["submitButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["yesButton"]));
                    //results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["alertPopUp"], i["text1"]));
                    //results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.CheckControlIsEmpty(results[0], i["NameTextbox"]));
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
