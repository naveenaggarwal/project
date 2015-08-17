using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.BusinessHelper;
using MSCOM.Test.CSV;

namespace RMT.UnitTests
{
    [TestClass]
    public class MapUserToRoles
    {
        #region Colors

        string blueColor = "rgba(49, 112, 143, 1)";
        string redColor = "rgba(169, 68, 66, 1)";

        #endregion

        #region SQLQueries

        string getAssignedRolesCountQuery = "SELECT COUNT(*) FROM [dbo].[UserRole] WHERE userID = 1";
        string getAvailableRolesCountQuery = "SELECT COUNT(*) FROM [dbo].[AgreementRole] WHERE agreementID = 3 AND [dbo].[AgreementRole].roleID NOT IN (SELECT roleID FROM [dbo].[UserRole] WHERE userID = 1)";
        string mapRoleToUserQuery = "SELECT [dbo].[Role].roleName FROM [dbo].[Role] JOIN [dbo].[UserRole] ON [dbo].[Role].roleID = [dbo].[UserRole].roleID JOIN [dbo].[User] ON [dbo].[User].userID = [dbo].[UserRole].userID WHERE [dbo].[User].userName = 'DellTestUser_35@DellAAD.onmicrosoft.com'";

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
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
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
                catch (Exception e)
                {
                    error += string.Format("\nAt Iteration {0}, The following Exception was thrown: {1}", iteration, e.Message);

                    continue;

                }
            }

            Assert.IsNull(error, error);
        }

        [TestMethod]
        [WorkItem(181980)]
        [TestProperty("TestCaseId", "181980")]
        public void VerifyControlsAreRenderedEmptyByDefault()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["usernameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["availableRoles"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["selectedRoles"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["filterAssignedRoles"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["filterAvailableRoles"]));
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
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["nameTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["RMTAgreementsTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["availableRolesTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["assignedRolesTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["assignAllRolesButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["assignSelectedRoleButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["removeSelectedRoleButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["removeAllRolesButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["clearButton"]));
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
        [WorkItem(193059)]
        [TestProperty("TestCaseId", "193059")]
        public void VerifyButtonControls()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
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
                    results.Add(SeleniumWebHelper.CheckIfCachedCredentialsAreRendered(results[0]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextbox"], i["userName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["passwordTextbox"], i["password"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["associateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.CheckLinkBackgroundColorIsNot(results[0], i["associateTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIsNot(results[0], i["userToRoleTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["nameLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["RMTAgreementsLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["availableRolesLabel"]));
                    results.Add(SeleniumWebHelper.GetElementLabel(results[0], i["assignedRolesLabel"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["nameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyByXPath(results[0], i["RMTAgreementsTextbox"], i["RMTAgreementsTextboxDefaultText"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ElementIsDisabled(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ElementIsEnabled(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RMTAgreementToRoleTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIsNot(results[0], i["RMTAgreementToRoleTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRMTAgreementTab"]));
                    results.Add(SeleniumWebHelper.CheckElementBackgroundColorIsNot(results[0], i["userToRMTAgreementTab"], i["bgColor"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["addUpdateTab"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url3"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTHomeLink"]));
                    results.Add(SeleniumWebHelper.CheckPageURLContains(results[0], i["url1"]));
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
        [WorkItem(193064)]
        [TestProperty("TestCaseId", "193064")]
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
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["UserNameErrorBox"], i["errorMessage"]));
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
        [WorkItem(193060)]
        [TestProperty("TestCaseId", "193060")]
        public void VerifyLookUpButtonFunctionalityForAnExistingUser()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextboxId"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["userNameAlertBox"], i["existMessage"]));
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
        [WorkItem(181991)]
        [TestProperty("TestCaseId", "181991")]
        public void VerifyRMTAgreementCheckboxesAreInitiallyDeselected()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.IsCheckboxDeselected(results[0], i["RMTAgreementCheckbox"]));
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
        [WorkItem(181992)]
        [TestProperty("TestCaseId", "181992")]
        public void VerifyUserCanSelectDeselectAgreementCheckbox()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextboxId"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTAgreementsDropdown"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["selectAll"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["deselectAll"]));
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
        [WorkItem(181994)]
        [TestProperty("TestCaseId", "181994")]
        public void VerifyAvailableRolesAreNotRenderedWhileFetchingRMTAgreements()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["userNameTextboxId"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["availableRoles"]));
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
        [WorkItem(181996)]
        [TestProperty("TestCaseId", "181996")]
        public void VerifyRolesAreRenderedSortedAlphabetically()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxSelected(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsDataSorted(results[0], i["availableRoles"]));
                    results.Add(SeleniumWebHelper.IsDataSorted(results[0], i["assignedRoles"]));
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
        [WorkItem(181997)]
        [TestProperty("TestCaseId", "181997")]
        public void VerifyCountOfAvailableAndAssignedRolesIsRendered()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxSelected(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.GetElementTextAfterFormat(results[0], i["availableRolesCount"]));
                    results.Add(SeleniumWebHelper.GetElementTextAfterFormat(results[0], i["assignedRolesCount"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompare(getAvailableRolesCountQuery, (string)results[11]));
                    results.Add(SQLHelper.RunQueryAndCompare(getAssignedRolesCountQuery, (string)results[12]));
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
        [WorkItem(181998)]
        [TestProperty("TestCaseId", "181998")]
        public void VerifyMapUserToRolePageAccess()
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
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["userNameTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["passwordTextbox"]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["signInButton"]));
                    results.Add(SeleniumWebHelper.CheckLinkIsNotRendered(results[0], i["logOff"]));
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
        [WorkItem(182001)]
        [TestProperty("TestCaseId", "182001")]
        public void VerifyAllAvailableRolesAssignedToUser()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxSelected(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addButton"]));
                    results.Add(SeleniumWebHelper.IsSelectControlEmptyById(results[0], i["roleDropDown"]));
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
        [WorkItem(182002)]
        [TestProperty("TestCaseId", "182002")]
        public void VerifyAllAssignedRolesRevokedFromUser()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["UserToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RemoveAllButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["AssignedRolesTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlNotEmptyById(results[0], i["AvailableRolesTextbox"]));
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
        [WorkItem(199430)]
        [TestProperty("TestCaseId", "199430")]
        public void VerifySaveButtonFunctionality()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxSelected(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleDropDown"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    //results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["alertPopUp"], i["text1"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["NameTextbox"]));
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
        [WorkItem(182005)]
        [TestProperty("TestCaseId", "182005")]
        public void VerifyRolesAreMappedToUserInDatabase()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxSelected(results[0], i["RMTAgreementName"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["roleDropDown"], i["roleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addButton"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["saveButton"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["yesButton"]));
                    //results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["OKButton"]));
                    //results.Add(SeleniumWebHelper.CheckElementTextByXPath(results[0], i["alertPopUp"], i["text1"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["NameTextbox"]));
                    results.Add(SeleniumWebHelper.ClickOnLinkByText(results[0], i["logOff"]));
                    results.Add(SeleniumWebHelper.CloseBrowser(results[0]));
                    results.Add(SQLHelper.RunQueryAndCompare(mapRoleToUserQuery, i["roleName"]));
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
        [WorkItem(189358)]
        [TestProperty("TestCaseId", "189358")]
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
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["UserToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTAgreementBox"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["SelectAllCheckBox"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["clearButton"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["NameTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["AvailableRolesTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyById(results[0], i["AssignedRolesTextbox"]));
                    results.Add(SeleniumWebHelper.IsControlEmptyByXPath(results[0], i["RMTAgreementBox"], i["NoneSelectedText"]));
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
        [WorkItem(193061)]
        [TestProperty("TestCaseId", "193061")]
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
                    results.Add(SeleniumWebHelper.NavigateTo(results[0], i["url2"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["UserToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["userNameAlertBox"], i["existMessage"]));
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
        [WorkItem(193062)]
        [TestProperty("TestCaseId", "193062")]
        public void VerifyAssignedRolesColor()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["UserToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTAgreementBox"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["SelectAllCheckBox"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["AvailableRolesTextbox"], i["AvailableRolesName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["AddButton"]));
                    results.Add(SeleniumWebHelper.CheckTextBackgroundColorIs(results[0], i["AvailableRolesName"], blueColor));
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
        [WorkItem(193063)]
        [TestProperty("TestCaseId", "193063")]
        public void VerifyRevokedRolesColor()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["UserToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["AssignedRolesTextbox"], i["AssignedRolesName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["RemoveButton"]));
                    results.Add(SeleniumWebHelper.CheckTextBackgroundColorIs(results[0], i["AssignedRolesName"], redColor));
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
        [WorkItem(181999)]
        [TestProperty("TestCaseId", "181999")]
        public void VerifyRolesSearchFilterFunctionality()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["UserToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["NameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["RMTAgreementBox"]));
                    results.Add(SeleniumWebHelper.ClickElementWithXPath(results[0], i["SelectAllCheckBox"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["AvailableSearchTextbox"], i["AvailableSearchName"]));
                    results.Add(SeleniumWebHelper.ImplicitlyWait(results[11]));
                    results.Add(SeleniumWebHelper.CheckDropDownValueText(results[0], i["AvailableSearchName"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["AssignedSearchTextbox"], i["AssignedSearchName"]));
                    results.Add(SeleniumWebHelper.ImplicitlyWait(results[14]));
                    results.Add(SeleniumWebHelper.CheckDropDownValueText(results[0], i["AssignedSearchName"]));
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
        [WorkItem(181986)]
        [TestProperty("TestCaseId", "181986")]
        public void VerifyExistingUserNamesAreAutoPopulated()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["nameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ImplicitlyWait(results[7]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["userNamesAutoPopulateTextbox"]));
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
        [WorkItem(181987)]
        [TestProperty("TestCaseId", "181987")]
        public void VerifySelectedAutoPopulatedUserNameIsRendered()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["nameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ImplicitlyWait(results[7]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["userNamesAutoPopulateTextbox"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[0], i["userNamesAutoPopulateTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["userNameAlertBox"], i["foundMessage"]));
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
        [WorkItem(193065)]
        [TestProperty("TestCaseId", "193065")]
        public void VerifyOnlyEnabledRMTAgreementsCanBeSelected()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["nameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ImplicitlyWait(results[7]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["userNamesAutoPopulateTextbox"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[0], i["userNamesAutoPopulateTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["userNameAlertBox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.IsCheckboxDisabled(results[0], i["disabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["disabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxDeselected(results[0], i["disabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["enabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxSelected(results[0], i["enabledRMTAgreementName"]));
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
        [WorkItem(193067)]
        [TestProperty("TestCaseId", "193067")]
        public void VerifyOnlyEnabledRolesCanBeAssigned()
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
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["userToRoleTab"]));
                    results.Add(SeleniumWebHelper.WriteOnTextBox(results[0], i["nameTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ImplicitlyWait(results[7]));
                    results.Add(SeleniumWebHelper.GetElement(results[0], i["userNamesAutoPopulateTextbox"]));
                    results.Add(SeleniumWebHelper.SelectAutoPopulateValue(results[0], i["userNamesAutoPopulateTextbox"], i["Name"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["lookUpButton"]));
                    results.Add(SeleniumWebHelper.CheckElementTextById(results[0], i["userNameAlertBox"], i["foundMessage"]));
                    results.Add(SeleniumWebHelper.ClickOnAButton(results[0], i["RMTAgreementDropDown"]));
                    results.Add(SeleniumWebHelper.IsCheckboxDisabled(results[0], i["disabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["disabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxDeselected(results[0], i["disabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.ClickOnCheckbox(results[0], i["enabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.IsCheckboxSelected(results[0], i["enabledRMTAgreementName"]));
                    results.Add(SeleniumWebHelper.CheckSelectOptionIsDisabled(results[0], i["availableRoles"], i["disabledAvailableRoleName"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["availableRoles"], i["disabledAvailableRoleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addButton"]));
                    results.Add(SeleniumWebHelper.CheckSelectOptionDoesNotContain(results[0], i["assignedRoles"], i["disabledAvailableRoleName"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["assignedRoles"], i["disabledAssignedRoleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["revokeButton"]));
                    results.Add(SeleniumWebHelper.CheckSelectOptionContains(results[0], i["availableRoles"], i["disabledAssignedRoleName"]));
                    results.Add(SeleniumWebHelper.CheckSelectOptionIsDisabled(results[0], i["availableRoles"], i["disabledAssignedRoleName"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["availableRoles"], i["disabledAssignedRoleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addButton"]));
                    results.Add(SeleniumWebHelper.CheckSelectOptionDoesNotContain(results[0], i["assignedRoles"], i["disabledAssignedRoleName"]));
                    results.Add(SeleniumWebHelper.SelectDropdownValue(results[0], i["availableRoles"], i["enabledRoleName"]));
                    results.Add(SeleniumWebHelper.ClickOnElement(results[0], i["addButton"]));
                    results.Add(SeleniumWebHelper.CheckSelectOptionContains(results[0], i["assignedRoles"], i["enabledRoleName"]));
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

    }
}
