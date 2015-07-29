using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.Test.MTM;

namespace MSCOM.DDA
{
    public class DDATestCaseException : Exception
    {
        public List<string[]> Logs;

        public string ErrorMessage
        {
            get
            {
                return base.Message.ToString();
            }
        }

        public DDATestCaseException(string errorMessage, List<string[]> logs = null)
            : base(errorMessage)
        {
            this.Logs = logs;
        }

        public DDATestCaseException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testContext"></param>
        /// <param name="iterationNo"></param>
        /// <param name="stepNo"></param>
        /// <returns></returns>
        public string AttachToTestResult(TestContext testContext, string iterationNo = "", string stepNo = "")
        {
            string tcId = testContext.Properties["TestCaseId"].ToString();
            string result = MSCOM.Test.Tools.StringManipulation.Truncate(string.Format("\nTest Case '{0}' failed with the following Exception: {1}", tcId, this.Message));

            if (MSCOM.Test.Tools.AutomationSettings.AutomationLogCSV)
            {
                string csvFileName = string.Format("{0}{1}{2}_TestLog.csv", tcId, iterationNo == "" ? "" : "_" + iterationNo, stepNo == "" ? "" : "_" + stepNo);
                string csvFilePath = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, csvFileName);
                if (csvFilePath != null)
                {
                    if (Logs != null)
                    {
                        MSCOM.Test.Tools.FileManagement.toCSVFile(Logs, csvFilePath);
                        testContext.AddResultFile(csvFilePath);
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("CSV Test Log '{0}' with additional information can be found at Test Results Attachments.\n", csvFileName, testContext.TestRunDirectory));
                    }
                    else
                    {
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("No CSV Test Log with additional information was created because Logs were null.\n"));
                    }
                }
                else
                {
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("CSV Test Log with additional information was attempted to be created but no Logs were found in MSCOM.DDA.DDAException object.\n"));
                }
            }
            else
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("CSV Test Log with additional information was requested to be created but it is disabled at 'AutomationSettings.csv' (AutomationLogCSV=false).\n", testContext.TestRunDirectory));
            }

            if (MSCOM.Test.Tools.AutomationSettings.AutomationErrorLog && this.Message != null && this.Message.Trim() != "")
            {
                string errorFileName = string.Format("{0}{1}{2}_ErrorMessage.txt", tcId, iterationNo == "" ? "" : "_" + iterationNo, stepNo == "" ? "" : "_" + stepNo);
                string testLogFileName = string.Format("{0}{1}{2}_TestLog.txt", tcId, iterationNo == "" ? "" : "_" + iterationNo, stepNo == "" ? "" : "_" + stepNo);
                string errorFilePath = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, errorFileName);
                string testLogPath = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, testLogFileName);

                MSCOM.Test.Tools.FileManagement.toTXTFile(result, errorFilePath);
                testContext.AddResultFile(errorFilePath);
                System.IO.File.Copy(string.Format(@"{0}\TestLogs\{1}", System.Environment.CurrentDirectory, "ErrorLog.txt"), testLogPath, true);
                testContext.AddResultFile(testLogPath);
                //System.IO.File.Delete(string.Format(@"{0}\TestLogs\{1}", System.Environment.CurrentDirectory, "ErrorLog.txt"));

                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test and Errors Logs ({0} & {1}) with additional information can be found at Test Results Attachments.\n", errorFileName, "ErrorLog.txt"));
            }
            else
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Error Log with additional information was requested to be created but it is disabled at 'AutomationSettings.csv' (AutomationErrorLog=false).\n", testContext.TestRunDirectory));
            }

            if (MSCOM.Test.Tools.AutomationSettings.AutomationLogHTML)
            {
                string htmlFileName = string.Format("{0}{1}{2}_TestResults.html", tcId, iterationNo == "" ? "" : "_" + iterationNo, stepNo == "" ? "" : "_" + stepNo);
                string htmlFilePath = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, htmlFileName);
                if (htmlFilePath != null)
                {
                    if (Logs != null)
                    {
                        MSCOM.Test.Tools.FileManagement.toHTMLFile(Logs, htmlFilePath, string.Format("{0} - {1}", tcId, testContext.TestName), this.Message);
                        testContext.AddResultFile(htmlFilePath);
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Results ({0}) with additional information can be found at Test Results Attachments.\n", htmlFileName, testContext.TestRunDirectory));
                    }
                    else
                    {
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("No Test Results with additional information was created because Logs were null.\n"));
                    }
                }
                else
                {
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Results with additional information was attempted to be created but no Logs were found in MSCOM.DDA.DDAException object.\n"));
                }
            }
            else
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Results with additional information was requested to be created but it is disabled at 'AutomationSettings.csv' (AutomationLogHTML=false).\n", testContext.TestRunDirectory));
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testContext"></param>
        /// <param name="iterationNo"></param>
        /// <param name="stepNo"></param>
        /// <returns></returns>
        public string AttachToTestResult(MTMTestCase testCase, string iterationNo = "", string stepNo = "")
        {
            string tcId = testCase.TestContext.Properties["TestCaseId"].ToString();
            string result = MSCOM.Test.Tools.StringManipulation.Truncate(string.Format("\nTest Case '{0}' failed with the following Exception: {1}", tcId, this.Message));

            if (MSCOM.Test.Tools.AutomationSettings.AutomationLogCSV)
            {
                string csvFileName = string.Format("{0}{1}{2}_TestLog.csv", tcId, iterationNo == "" ? "" : "_" + iterationNo, stepNo == "" ? "" : "_" + stepNo);
                string csvFilePath = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, csvFileName);
                if (csvFilePath != null)
                {
                    if (Logs != null)
                    {
                        MSCOM.Test.Tools.FileManagement.toCSVFile(Logs, csvFilePath);
                        testCase.TestContext.AddResultFile(csvFilePath);
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("CSV Test Log '{0}' with additional information can be found at Test Results Attachments.\n", csvFileName, testCase.TestContext.TestRunDirectory));
                    }
                    else
                    {
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("No CSV Test Log with additional information was created because Logs were null.\n"));
                    }
                }
                else
                {
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("CSV Test Log with additional information was attempted to be created but no Logs were found in MSCOM.DDA.DDAException object.\n"));
                }
            }
            else
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("CSV Test Log with additional information was requested to be created but it is disabled at 'AutomationSettings.csv' (AutomationLogCSV=false).\n", testCase.TestContext.TestRunDirectory));
            }

            if (MSCOM.Test.Tools.AutomationSettings.AutomationErrorLog && this.Message != null && this.Message.Trim() != "")
            {
                string errorFileName = string.Format("{0}{1}{2}_ErrorMessage.txt", tcId, iterationNo == "" ? "" : "_" + iterationNo, stepNo == "" ? "" : "_" + stepNo);
                string errorFilePath = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, errorFileName);

                MSCOM.Test.Tools.FileManagement.toTXTFile(result, errorFilePath);
                testCase.TestContext.AddResultFile(errorFilePath);
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Error Log ({0}) with additional information can be found at Test Results Attachments.\n", errorFileName, testCase.TestContext.TestRunDirectory));
            }
            else
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Error Log with additional information was requested to be created but it is disabled at 'AutomationSettings.csv' (AutomationErrorLog=false).\n", testCase.TestContext.TestRunDirectory));
            }

            if (MSCOM.Test.Tools.AutomationSettings.AutomationLogHTML)
            {
                string htmlFileName = string.Format("{0}{1}{2}_TestResults.html", tcId, iterationNo == "" ? "" : "_" + iterationNo, stepNo == "" ? "" : "_" + stepNo);
                string htmlFilePath = string.Format(@"{0}\{1}", System.Environment.CurrentDirectory, htmlFileName);
                if (htmlFilePath != null)
                {
                    if (Logs != null)
                    {
                        MSCOM.Test.Tools.FileManagement.toHTMLFile(Logs, htmlFilePath, string.Format("{0} - {1}", tcId, testCase.Title), this.Message);
                        testCase.TestContext.AddResultFile(htmlFilePath);
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Results ({0}) with additional information can be found at Test Results Attachments.\n", htmlFileName, testCase.TestContext.TestRunDirectory));
                    }
                    else
                    {
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("No Test Results with additional information was created because Logs were null.\n"));
                    }
                }
                else
                {
                    MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Results with additional information was attempted to be created but no Logs were found in MSCOM.DDA.DDAException object.\n"));
                }
            }
            else
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Test Results with additional information was requested to be created but it is disabled at 'AutomationSettings.csv' (AutomationLogHTML=false).\n", testCase.TestContext.TestRunDirectory));
            }

            return result;
        }

    }

    public class DDAIterationException : DDATestCaseException
    {
        public DDAIterationException(string errorMessage, List<string[]> logs = null)
            : base(errorMessage)
        {
            this.Logs = logs;
        }

        public DDAIterationException(string errorMessage, Exception innerEx, List<string[]> logs = null) : base(errorMessage, innerEx) { }

        public string AttachToTestResult(TestContext testContext, int iterationNo)
        {
            base.AttachToTestResult(testContext, iterationNo.ToString(), "");
            string tcId = testContext.Properties["TestCaseId"].ToString();
            return MSCOM.Test.Tools.StringManipulation.Truncate(string.Format("\nTest Case '{0}' failed at iteration '{1}' with the following Exception: {2}...", tcId, iterationNo, this.Message));
        }

        public string AttachToTestResult(MTMTestCase testCase, int iterationNo)
        {
            base.AttachToTestResult(testCase, iterationNo.ToString(), "");
            string tcId = testCase.TestContext.Properties["TestCaseId"].ToString();
            return MSCOM.Test.Tools.StringManipulation.Truncate(string.Format("\nTest Case '{0}' failed at iteration '{1}' with the following Exception: {2}...", tcId, iterationNo, this.Message));
        }
    }

    public class DDAStepException : DDAIterationException
    {
        public DDAStepException(string errorMessage, List<string[]> logs = null)
            : base(errorMessage)
        {
            this.Logs = logs;
        }

        public DDAStepException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }

        public string AttachToTestResult(TestContext testContext, int iterationNo, int stepNo)
        {
            base.AttachToTestResult(testContext, iterationNo.ToString(), stepNo.ToString());
            string tcId = testContext.Properties["TestCaseId"].ToString();
            return MSCOM.Test.Tools.StringManipulation.Truncate(string.Format("\nTest Case '{0}' failed at step '{1}' of iteration '{2}' with the following Exception: {3}...", tcId, stepNo, iterationNo, this.Message));
        }

        public string AttachToTestResult(MTMTestCase testCase, int iterationNo, int stepNo)
        {
            base.AttachToTestResult(testCase, iterationNo.ToString(), stepNo.ToString());
            string tcId = testCase.TestContext.Properties["TestCaseId"].ToString();
            return MSCOM.Test.Tools.StringManipulation.Truncate(string.Format("\nTest Case '{0}' failed at step '{1}' of iteration '{2}' with the following Exception: {3}...", tcId, stepNo, iterationNo, this.Message));
        }
    }
}