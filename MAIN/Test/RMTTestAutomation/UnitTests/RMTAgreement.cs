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
        #region NewRandomAgreement

        string RMTAgreementName = DataHelper.GenerateRandomString();

        #endregion

        #region SQLQueries

        string getRMTDetailsQuery = "SELECT [dbo].[Agreement].agreementDescription, [dbo].[Status].statusName FROM [dbo].[Role] JOIN [dbo].[Status] ON [dbo].[Agreement].statusID = [dbo].[Status].statusID WHERE [dbo].[Agreement].agreementName LIKE 'test%'";

        string getNewRMTAgreementDetailsQuery = "SELECT [dbo].[Agreement].agreementName, [dbo].[Agreement].agreementDescription, [dbo].[Status].statusName FROM [dbo].[Agreement] JOIN [dbo].[Status] ON [dbo].[Agreement].statusID = [dbo].[Status].statusID WHERE [dbo].[Agreement].agreementName LIKE 'test%'";

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


    }
}
