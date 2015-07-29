using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace MSCOM.Test.Tools
{
    static public class TestAgent
    {
        public static string LogTextFile = "TextFile";
        public static string LogConsole = "Console";
        public static string LogAll = "All";
        public static string LogNone = "None";

        /// <summary>
        /// Deletes all cookie files in users machine
        /// </summary>
        public static void DeleteCookies()
        {
            List<string> cookies = new List<string>();
            cookies.AddRange(Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Cookies)));
            cookies.AddRange(Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.InternetCache)));

            try
            {
                foreach (string cookieFile in cookies)
                {
                    if (cookieFile.Contains(".txt") || cookieFile.Contains("cookie"))
                    {
                        System.IO.File.Delete(cookieFile);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException(string.Format("Unexpected error was found. Try running this method as Administrator. Additional Info:\n {0}"));
            }

            System.Diagnostics.Process windowsProcess = new System.Diagnostics.Process();
            windowsProcess.StartInfo.FileName = @"C:\Windows\System32\rundll32.exe";
            windowsProcess.StartInfo.Arguments = "InetCpl.cpl,ClearMyTracksByProcess 4351"; //4351 is for Deleting the cookies and Add on settings

            windowsProcess.Start();
            while (!windowsProcess.HasExited)
            {
                System.Threading.Thread.Sleep(1000);
            }
            windowsProcess.Close();
        }

        public static void KillInternetExplorerInstances()
        {
            int instances = 0;
            SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();
            foreach (SHDocVw.InternetExplorer ie in shellWindows)
            {
                ie.Quit();
                instances++;
            }

            if (instances > 0)
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("'{0}' instance{1} of Internet Explorer {2} terminated", instances, instances == 1 ? "" : "s", instances == 1 ? "was" : "were"));
            }
            else
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("No Internet Explorer instances were terminated"));
            }
        }

        /// <summary>
        /// This method will return the Test Agent Full Quallified Domain Name 
        /// </summary>
        /// <returns>Full Quallified Domain Name</returns>
        public static string GetLocalHostFQDN()
        {
            var ipProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
            return string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
        }

        /// <summary>
        /// Logs to console when AutomationSettings.AutomationLog = true
        /// </summary>
        /// <param name="message">Message to Log</param>
        /// <param name="hide">This string will be replaced with "********" when provided and available in the message</param>
        /// <returns>The messaged as logged</returns>
        public static string LogToTestResult(string message, string hide = "")
        {
            string result = "";
            if (AutomationSettings.AutomationLog != LogNone && AutomationSettings.AutomationLog != "false")
            {
                try
                {
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                    result = string.Format("{0}.{1}@{2}: {3}\n", stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName, stackTrace.GetFrame(1).GetMethod().Name, GetLocalHostFQDN().ToUpper(), hide.Trim() == "" ? message : message.Replace(hide, "*********"));
                    if (AutomationSettings.AutomationLog == LogTextFile || AutomationSettings.AutomationLog == LogAll)
                    {
                        System.IO.Directory.CreateDirectory(string.Format(@"{0}\TestLogs", System.Environment.CurrentDirectory));
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(string.Format(@"{0}\TestLogs\{1}", System.Environment.CurrentDirectory, "ErrorLog.txt"), true))
                        {
                            file.WriteLine(result);
                        }
                    }

                    if (AutomationSettings.AutomationLog == LogConsole || AutomationSettings.AutomationLog == LogAll)
                    {
                        System.Console.Out.Write(result);
                    }
                    return result;
                }
                catch (Exception e)
                {
                    result = string.Format("WARNING: Unable to LogToTestResult. Error '{0}'. \nLogToTestResult: {1}\n", e.Message, message);
                    System.Console.Out.Write(result);
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Logs the test case result to console when AutomationSettings.AutomationLog = true
        /// </summary>
        /// <param name="message">Message to Log</param>
        /// <param name="hide">This string will be replaced with "********" when provided and available in the message</param>
        /// <returns>The messaged as logged</returns>
        public static string LogTestCaseResult(string message, string hide = "")
        {
            string result = "";
            if (AutomationSettings.AutomationLog != LogNone && AutomationSettings.AutomationLog != "false")
            {
                try
                {
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                    result = string.Format("{0}.{1}@{2}: {3}\n", stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName, stackTrace.GetFrame(1).GetMethod().Name, GetLocalHostFQDN().ToUpper(), hide.Trim() == "" ? message : message.Replace(hide, "*********"));
                    if (AutomationSettings.AutomationLog == LogTextFile || AutomationSettings.AutomationLog == LogAll)
                    {
                        System.IO.Directory.CreateDirectory(string.Format(@"{0}\TestLogs", System.Environment.CurrentDirectory));
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(string.Format(@"{0}\TestLogs\{1}", System.Environment.CurrentDirectory, "TCResultLog.txt"), true))
                        {
                            file.WriteLine(result);
                        }
                    }

                    if (AutomationSettings.AutomationLog == LogConsole || AutomationSettings.AutomationLog == LogAll)
                    {
                        System.Console.Out.Write(result);
                    }
                    return result;
                }
                catch (Exception e)
                {
                    result = string.Format("WARNING: Unable to LogToTestResult. Error '{0}'. \nLogToTestResult: {1}\n", e.Message, message);
                    System.Console.Out.Write(result);
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool IsTimeOut(int seconds)
        {
            if (seconds > AutomationSettings.TestTimeOut)
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                throw new TimeoutException(string.Format("{0}.{1}@{2} Execution Timed Out.", stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName, stackTrace.GetFrame(1).GetMethod().Name, GetLocalHostFQDN().ToUpper()));
            }
            System.Threading.Thread.Sleep(1000);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool Wait(string seconds)
        {
            try
            {
                System.Threading.Thread.Sleep(int.Parse(seconds) * 1000);
                return true;
            }
            catch
            {
                LogToTestResult(string.Format("Unable to wait as requested '{0}'. Waited 1 second instead.", seconds));
                System.Threading.Thread.Sleep(1000);
                return false;
            }
        }
    }
}
