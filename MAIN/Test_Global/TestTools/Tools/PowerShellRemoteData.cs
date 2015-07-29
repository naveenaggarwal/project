using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSCOM.Test.Tools
{
    public class PowerShellRemoteData
    {
        public PSCredentials credentials;

        //For a given TestAgent/TargetServer pair there will be a credentials struct
        
        private int testAgentIndex;
        private int usernameIndex;
        private int targetServerIndex;

        private string COL_TEST_AGENT = "testAgent";
        private string COL_USERNAME = "username";
        private string COL_TARGET_SERVER = "targetServer";

        private string CREDENTIALS_FILE = "CredentialsPowerShell.csv";
        
        private List<string[]> csvLogContent;

        private PowerShellRemoteData(string environment, string targetRemoteServer)
        {
            string credentialsLocation = Environment.GetTestContentLocation();

            try
            {
                this.csvLogContent = FileManagement.parseCSV(String.Format("{0}\\{1}", credentialsLocation, CREDENTIALS_FILE));
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(string.Format("PowerShellHelper was unable to load credentials using '{0}{1}.{2}' because the file was not found.", credentialsLocation, environment, CREDENTIALS_FILE));
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(string.Format("PowerShellHelper was unable to load credentials using '{0}{1}.{2}' because the file was not found.", credentialsLocation, environment, CREDENTIALS_FILE));
            } 
            
            PSCredentials current;
            
            bool first = true;

            foreach (string[] csvLine in csvLogContent)
            {
                if (!first)
                {
                    current = new PSCredentials { Environment = environment, TestAgent = csvLine[testAgentIndex], UserName = csvLine[usernameIndex], TargetServer = csvLine[targetServerIndex] };

                    if (current.TestAgent.ToUpper() == TestAgent.GetLocalHostFQDN().ToUpper() && current.TargetServer == targetRemoteServer)
                    {
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Environment - {0}", environment));
                        MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("TargetRemoteComputer - {0}", targetRemoteServer));
                        current.Password = Test.Tools.Environment.GetPassword(current.UserName);
                        credentials = current;
                        break;
                    }
                }
                else
                {
                    identifyIndexes(csvLine);
                    first = false;
                }
            }
        }

        public static PSCredentials GetPSCredential(string environment, string remoteComputer)
        {
            PowerShellRemoteData result = null;
            
            try
            {
                result = new PowerShellRemoteData(environment, remoteComputer);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(string.Format("Credentials for user '{0}' in '{1}' Environment were not found at 'CredentialsPowerShell.csv' for Target Server '{2}'.", System.Environment.UserDomainName + "\\" + System.Environment.UserName, environment, remoteComputer));
            }
            return result.credentials;
        }

        public struct PSCredentials
        {
            public string Environment { get; set; }

            public string TestAgent { get; set; }
            public string UserName { get; set; }
            public string TargetServer { get; set; }
            public string Password { get; set; }
        }

        /// <summary>
        /// This method is intended to correctly initialize indexes for each columnn in the given csv header. 
        /// Said indexes are used to identify columns based on names regardless of the order. Column names are based on COL_... private variables declared in these class
        /// </summary>
        /// <param name="csvHeader">A string array with a column name in each item</param>
        private void identifyIndexes(string[] csvHeader)
        {
            string error = null;

            testAgentIndex = Array.IndexOf(csvHeader, COL_TEST_AGENT);
            usernameIndex = Array.IndexOf(csvHeader, COL_USERNAME);
            targetServerIndex = Array.IndexOf(csvHeader, COL_TARGET_SERVER);

            error = ((testAgentIndex < 0) ? string.Format("The field '{0}' was not found in the provided csvHeader.\n", COL_TEST_AGENT, csvHeader) : "") +
                ((usernameIndex < 0) ? string.Format("The field '{0}' was not found in the provided csvHeader.\n", COL_USERNAME, csvHeader) : "") +
                ((targetServerIndex < 0) ? string.Format("The field '{0}' was not found in the provided csvHeader.\n", COL_TARGET_SERVER, csvHeader) : "");

            if (error.Trim() != "")
            {
                throw new Exception(string.Format("The provided csv file has invalid header line '{0}'. Unexpected error(s) found.\n {1}", String.Join(", ", csvHeader), error));
            }
        }        
    }
}
