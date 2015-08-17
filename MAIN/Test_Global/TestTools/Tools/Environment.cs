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
        //public static string contentLocation = System.Configuration.ConfigurationManager.AppSettings["ContentFolderLocation"];

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

    }
}
