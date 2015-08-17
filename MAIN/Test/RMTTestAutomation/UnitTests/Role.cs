using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.BusinessHelper;
using MSCOM.Test.CSV;

namespace RMT.UnitTests
{
    [TestClass]
    public class Role
    {
        #region SQLQueries

        string getRoleDetailsQuery = "SELECT [dbo].[Role].roleDescription, [dbo].[Status].statusName FROM [dbo].[Role] JOIN [dbo].[Status] ON [dbo].[Role].statusID = [dbo].[Status].statusID WHERE [dbo].[Role].roleName = 'OCD_Pricing'";

        string getNewRoleDetailsQuery = "SELECT [dbo].[Role].roleName, [dbo].[Role].roleDescription, [dbo].[Status].statusName FROM [dbo].[Role] JOIN [dbo].[Status] ON [dbo].[Role].statusID = [dbo].[Status].statusID WHERE [dbo].[Role].roleName LIKE 'test%'";

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
        [WorkItem(200581)]
        [TestProperty("TestCaseId", "200581")]
        public void VerifyUserIsNavigatedToRolePage()
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
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
        [WorkItem(200582)]
        [TestProperty("TestCaseId", "200582")]
        public void VerifyAllControlsStateOnPageLoad()
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["roleLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["roleDescriptionLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["roleStatusLabel"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["roleDescriptionTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["roleStatusDropDown"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleDescriptionTextbox"], i["roleDescriptionDefaultText"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleStatusDropDown"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["roleDescriptionTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["roleStatusDropDown"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
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
        [WorkItem(200583)]
        [TestProperty("TestCaseId", "200583")]
        public void VerifyAutoPopulateFunctionality()
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
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleNameTextbox"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ImplicitlyWait(results[7]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["roleNamesAutoPopulateTextbox"]));
                    results.Add(SeleniumWebHelper.CheckCountOfAutoPopulatedValues(results[0], i["roleNamesAutoPopulateTextbox"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[0], i["roleNamesAutoPopulateTextbox"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["roleNameAlertBox"], i["foundMessage"]));
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
        [WorkItem(200635)]
        [TestProperty("TestCaseId", "200635")]
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
                    results.Add(SeleniumWebHelper.OpenWebBrowser(i["webBrowser"], i["url1"]));
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleTextbox"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleDescriptionTextbox"], i["roleDescription"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["roleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleDescriptionTextbox"], i["roleDescriptionDefaultText"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleStatusDropDown"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleTextbox"], i["newRoleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["roleErrorTextbox"], i["errorMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.CheckElementIsNotRendered(results[0], i["roleErrorTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleTextbox"]));
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
        [WorkItem(201076)]
        [TestProperty("TestCaseId", "201076")]
        public void VerifyLookUpButtonFunctionalityWithoutEnteringText()
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["roleErrorTextbox"], i["errorMessage"]));
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
        [WorkItem(201084)]
        [TestProperty("TestCaseId", "201084")]
        public void VerifyLookUpButtonFunctionalityForAnExistingRole()
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleTextbox"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["roleFoundTextbox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["roleDescriptionTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["roleStatusDropDown"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["agreementsTile"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["usersTile"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["agreementsTile"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["usersTile"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
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
        [WorkItem(201114)]
        [TestProperty("TestCaseId", "201114")]
        public void VerifyLookUpButtonFunctionalityOnEnteringRandomText()
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleTextbox"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["roleErrorTextbox"], i["errorMessage"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["roleDescriptionTextbox"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["roleStatusDropDown"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["saveButton"]));
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
        [WorkItem(201363)]
        [TestProperty("TestCaseId", "201363")]
        public void VerifyEditButtonFunctionality()
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleTextbox"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["editButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleDescriptionTextbox"], i["roleDescription"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["roleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompareIfDataIsUpdated(getRoleDetailsQuery, i["roleDescription"], i["roleStatus"]));
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
        [WorkItem(201380)]
        [TestProperty("TestCaseId", "201380")]
        public void VerifyNewRoleWithEnabledStatusIsCreated()
        {
            string roleName = DataHelper.GenerateRandomString();
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleTextbox"], roleName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleDescriptionTextbox"], i["roleDescription"]));
                    results.Add(SeleniumWebHelper.CheckElementIsNotRendered(results[0], i["agreementTile"]));
                    results.Add(SeleniumWebHelper.CheckElementIsNotRendered(results[0], i["userTile"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["roleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompareIfDataIsUpdated(getNewRoleDetailsQuery, roleName, i["roleDescription"], i["roleStatus"]));
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
        [WorkItem(201382)]
        [TestProperty("TestCaseId", "201382")]
        public void VerifyNewRoleWithDisabledStatusIsCreated()
        {
            string roleName = DataHelper.GenerateRandomString();
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
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["roleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleTextbox"], roleName));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["roleDescriptionTextbox"], i["roleDescription"]));
                    results.Add(SeleniumWebHelper.CheckElementIsNotRendered(results[0], i["agreementTile"]));
                    results.Add(SeleniumWebHelper.CheckElementIsNotRendered(results[0], i["userTile"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleStatusDropDown"], i["roleStatus"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["roleTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompareIfDataIsUpdated(getNewRoleDetailsQuery, roleName, i["roleDescription"], i["roleStatus"]));
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
