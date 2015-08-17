using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCOM.Test.Tools
{
    static public class AutomationSettings
    {
        public static string CurrentEnvironment;
        public static string AutomationLog;
        public static bool AutomationLogCSV;
        public static bool AutomationLogHTML;
        public static bool AutomationErrorLog;
        public static bool IEBrowserClearCookies;
        public static int TestTimeOut;
        
        static AutomationSettings()
        {
            CurrentEnvironment = GetSetting("CurrentEnvironment");
            AutomationLog = GetSetting("AutomationLog");
            bool.TryParse(GetSetting("AutomationLogCSV"), out AutomationLogCSV);
            bool.TryParse(GetSetting("AutomationLogHTML"), out AutomationLogHTML);
            bool.TryParse(GetSetting("AutomationErrorLog"), out AutomationErrorLog);
            bool.TryParse(GetSetting("IEBrowserClearCookies"), out IEBrowserClearCookies);
            int.TryParse(GetSetting("TestTimeOut"), out TestTimeOut);
         }

        public static string GetSetting(string setting)
        {
            List<string[]> automationSettings = Tools.FileManagement.parseCSV(Environment.GetTestContentLocation() + "AutomationSettings.csv");

            foreach (string[] item in automationSettings)
            {
                if (item[0].Trim() == setting.Trim())
                {
                    return item[1];
                }
            }

            throw new KeyNotFoundException(string.Format("MSCOM.Test.Tools.Environment.AutomationSettings was unable to find key '{0}' at '{1}'.", setting, Environment.GetTestContentLocation() + "AutomationSettings.csv"));
        }
    }
}
