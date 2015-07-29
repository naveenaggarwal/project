using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MAT.Security.Credentials;

namespace MSCOM.Test.Tools
{
    static public class Environment
    {
        private static List<string[]> environmentsCSV = new List<string[]>();

        //public static string contentLocation = System.Configuration.ConfigurationManager.AppSettings["ContentFolderLocation"];

        static Environment()
        {
            environmentsCSV = Tools.FileManagement.parseCSV(string.Format("{0}Environments.csv",GetTestContentLocation()));
        }

        /// <summary>
        /// This method will append the Environment.CurrentDirectory path (as a prefix) and the extension 'csv' (as a suffix) to the provided parameter
        /// </summary>
        /// <param name="environmentValuesFileName">The name (as in settings file) of the Environment Values csv file</param>
        /// <returns>Location of the Envirnoment Values csv file</returns>
        public static string GetEnvironmentValuesFilePath(string environment)
        {
            string envLoc = GetEnvironmentsDataLocation(environment);
            return string.Format("{0}{1}.MTMReplacementValues.csv", GetEnvironmentsDataLocation(environment), environment);
        }

        public static string GetEnvironmentsDataLocation(string environment)
        {
            string contLoc = GetTestContentLocation();
            string contLocPath = GetTestContentPath(contLoc);
            return string.Format(@"{0}{1}\", GetTestContentLocation(), environment.Trim());
        }
        
        /// <summary>
        /// Will look into 'CurrentEnvironment.csv' and return the top value there
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentEnvironment()
        {
            return AutomationSettings.CurrentEnvironment;
        }

        public static string GetTestContentLocation()
        {
            return string.Format(@"{0}\TestContent\", System.Environment.CurrentDirectory);
            //return contentLocation;
        }

        public static string GetTestContentPath(string tfsPath)
        {
            return tfsPath.Replace("$TestContent_Location$", GetTestContentLocation()).Replace("/", @"\");
        }

       /// <summary>
        /// For a given user or service account, this method will return the password leveraging PDS (Protected Data Storage)
        /// </summary>
        /// <param name="username">In the format of "Domain\username". When intended to be retrieved from RDBusiness setting file, the key must be "Domain_username" instead.</param>
        /// <param name="pdsLocation">File path representing the location of PDS Tool</param>
        /// <returns></returns>
        public static string GetPassword(String username, string pdsLocation = "")
        {
            if (pdsLocation.Trim() != "")
            {
                ProtectedDataStore.PdsLocation = pdsLocation;
            }
            else
            {
                ProtectedDataStore.PdsLocation = AutomationSettings.PDSLocation;
            }
            
            return ProtectedDataStore.GetAssetPasswordAsPlainText(username);
        }
        /// <summary>
        /// This Method will return the current pst DateTime.
        /// </summary>
        /// <returns>return the current pst DateTime.</returns>
        public static DateTime GetDateTime()
        {
            DateTime timeUtc = DateTime.UtcNow;
            try
            {
                TimeZoneInfo pstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime pstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, pstTimeZone);
                return pstTime;
            }
            catch (TimeZoneNotFoundException)
            {
                throw new TimeZoneNotFoundException(string.Format("The registry does not define the Pacific Standard Time zone."));
            }
            catch (InvalidTimeZoneException)
            {
                throw new TimeZoneNotFoundException(string.Format("Registry data on the Pacific Standard Time zone has been corrupted"));
            }
        }

        public static string GetURLServerName(string url)
        {            
            List<string[]> environmentServerURLCSV = new List<string[]>();
            string filename = "EnvironmentServerURL";
            string currentEnvCSVFilePath = string.Format(@"{0}\{1}.csv", GetTestContentLocation(), filename);

            try
            {
                environmentServerURLCSV = FileManagement.parseCSV(currentEnvCSVFilePath);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(string.Format("MSCOM.Test.Tools was unable to load '{1}' using '{0}' because the file was not found", currentEnvCSVFilePath, filename));
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(string.Format("MSCOM.Test.Tools was unable to load '{1}' using '{0}' because the file was not found", currentEnvCSVFilePath, filename));
            }

            Dictionary<string, int> header = new Dictionary<string,int>();

            bool first = true;
            foreach (string[] row in environmentServerURLCSV)
            {
                if (first)
                {
                    header = FileManagement.GetCSVHeaderIndexes(row);
                    first = false;
                }
                else
                {
                    if (url.Contains(row[header["URL"]]))
                    {
                        return row[header["PhysicalServer"]];
                    }
                }
            }

            throw new KeyNotFoundException(string.Format("Unable to get 'Physical Server' for the provided 'url' '{0}'.", url));
        }

        public static List<List<string>> GetEnvironmetsCSVData(string filters, string columns)
        {         
            List<List<string>> result = new List<List<string>>();
            List<string> newRow = new List<string>();

            string currentEnvCSVFilePath = string.Format(@"{0}\{1}.csv", GetTestContentLocation(), "Environments");
            var envsData = MSCOM.Test.Tools.FileManagement.parseCSV(currentEnvCSVFilePath);

            bool first = true;
            Dictionary<string, int> header = new Dictionary<string, int>();

            string[] requestedColumns = columns.Split(',');
            string[] requestedFilters = filters.Split(',');

            foreach (string[] row in envsData)
            {
                if (first)
                {
                    first = false;
                    header = FileManagement.GetCSVHeaderIndexes(row);
                }
                else
                {                    
                    //if (row[header["PhysicalServer"]])
                    //{
                    //newRow = new List<string>();
                    //foreach(string column in requestedColumns)
                    //{
                    //    newRow.Add(row[header[column]]);
                    //}
                    //result.Add(newRow);
                    //}
                }
            
            }
            return result;
        }

        public static string GetSQLConnectionString(string serverName)
        {            
            string result = "true";

            List<List<string>> results = GetEnvironmetsCSVData(string.Format("PhysicalServer:{0}", serverName), "IntegratedSecurity");
            bool first = true;
            Dictionary<string, int> header = new Dictionary<string, int>();

            foreach (List<string> row in results)
            {
                if (first)
                {
                    first = false;
                    header = FileManagement.GetCSVHeaderIndexes(row.ToArray());
                }
                else 
                {
                    result = row[header["IntegratedSecurity"]];
                    break;
                }
            }

            return result;
        }

       
    }
}
