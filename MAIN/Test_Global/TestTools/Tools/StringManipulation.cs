using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using MAT.Security.Credentials;
using MAT.HttpAnalysis;

namespace MSCOM.Test.Tools
{
    static public class StringManipulation
    {
        private static string jsContent1 = "<script type=\"text/javascript\">";
        private static string jsContent2 = "<script type=\'text/javascript\'>";
        private static string jsContent3 = "<script type=text/javascript>";

        private static string jsReference1 = "<script type=\"text/javascript\" src=";
        private static string jsReference2 = "<script type=\'text/javascript\' src=";
        private static string jsReference3 = "<script type=text/javascript src=";

        private static string jsClose = "</script>";

        public static string LOCAL_VALUES = "LOCAL.Values"; 
        public static string MAIN_VALUES = "MAIN.Values";
        public static string SHIP_VALUES = "SHIP.Values";
        public static string PPE_VALUES = "PPE.Values";
        public static string PROD_VALUES = "PROD.Values";

        
        /// <summary>
        /// Parse the keys: "found" and "not found" into boolean. If none of the valid keys provided are valid, an Expception is thrown.
        /// </summary>
        /// <param name="key">key to be parsed in the format: "found" or "not found"</param>
        /// <returns>returns ture when key is "found" and false if key is "not found"</returns>
        public static bool ParseFound(string key)
        {
            if (key.Trim() == "found")
            {
                return true;
            }
            else if (key.Trim() == "not found")
            {
                return false;
            }
            else
            {
                throw new System.ArgumentException(String.Format("Unable to parse the proided key ({0}).", key));
            }
        }

        /// <summary>
        /// For a given url, will remove the Query String Parameters
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Returns a string with the given url without query string parameters if any</returns>
        public static string RemoveQSP(string url)
        {
            return url.Split('?')[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static XDocument GetScripts(string content)
        {
            int tagIndex = -1;            
            int closeIndex = -1;
            string temp = "";

            content = content.ToLower();
            content = content.Replace(jsContent2, jsContent1).Replace(jsContent3, jsContent1).Replace(jsReference2, jsReference1).Replace(jsReference3,jsReference1);
            content = RemoveExtraSpaces(content).Trim();

            string resultText = "";

            while (content.Contains(jsContent1))
            {
                tagIndex = content.IndexOf(jsContent1);               

                if (tagIndex >= 0)
                {
                    closeIndex = content.IndexOf(jsClose);
                    if (closeIndex < 0)
                    {
                        //error. Parsing error Closing tag not found
                    }
                    else
                    {
                        temp = content.Substring(tagIndex + jsContent1.Length, closeIndex - tagIndex - jsContent1.Length).Trim();
                        resultText += jsContent1 + temp.Replace(temp, System.Net.WebUtility.HtmlEncode(temp)).Trim() + jsClose;
                        content = content.Replace(content.Substring(tagIndex, closeIndex + jsClose.Length), "").Trim();
                    }
                }
            }

            while (content.Contains(jsReference1))
            {
                tagIndex = content.IndexOf(jsReference1);

                if (tagIndex >= 0)
                {
                    closeIndex = content.IndexOf(jsClose);
                    if (closeIndex < 0)
                    {
                        //error. Parsing error Closing tag not found
                    }
                    else
                    {
                        resultText += content.Substring(tagIndex, closeIndex - tagIndex + jsClose.Length).Trim();
                        content = content.Replace(content.Substring(tagIndex, closeIndex - tagIndex + jsClose.Length), "").Trim();
                    }
                }
            }

            return XDocument.Parse("<?xml version=\"1.0\" encoding=\"utf-8\" ?><scripts>"+ resultText + "</scripts>");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveExtraSpaces(string s)
        {
            string result = "";
            string[] subStrings;
            //s = s.Replace('\n',' ').Replace('\r',' ');
            s = Regex.Replace(s, @"([\r\n]+)", " ");
            subStrings = s.Split(' ');
            foreach (string i in subStrings)
            {
                if (i.Trim() != "")
                {
                    result += i.Trim() + " ";
                }
            }

            return result;
        }

        public static List<string[]> GetTableFromHTML(string html)
        {
            List<string[]> result = null;
            mshtml.HTMLTable d = new mshtml.HTMLTable();
            d.outerHTML = html;
            return result;        
        }

        public static bool IsSameUrl(string url1, string url2)
        {
            if (url1[url1.Length - 1] != '/')
            {
                url1 = url1 + '/';
            }

            if (url2[url2.Length - 1] != '/')
            {
                url2 = url2 + '/';
            }

            return url1 == url2;        
        }

        public static bool HasDomain(string username)
        {
            if (username.Split('\\').Length == 2)
            {
                return true;
            }

            return false;
        }

        public static string ToString(string[] array, bool quote = false)
        {
            string result = "";
            bool isFirst = true;
            foreach (string s in array)
            {
                if (!isFirst)
                {
                    result += ",";
                }
                else
                {
                    isFirst = false;
                }
                result += quote? "\"" + s + "\"": s;
            }

            return result;
        }

        public static string Truncate(string s)
        {
            if(s.Length >=  MSCOM.Test.Tools.AutomationSettings.AutomationConsoleLogMaxCharacters)
            {
                return s.Substring(0, MSCOM.Test.Tools.AutomationSettings.AutomationConsoleLogMaxCharacters);
            }

            return s;
        }

        public static System.Security.SecureString ToSecureString(string unencryptedString)
        {
            var result = new System.Security.SecureString();
            if (unencryptedString.Length > 0)
            {
                foreach (var c in unencryptedString.ToCharArray()) result.AppendChar(c);
            }
            return result;
        }

        public static string TruncateHTMLSpaces(string original)
        {
            original = original.Replace("\n", " ");
            original = original.Replace("\r", " ");
            original = original.Replace("\t", " ");
            original = original.Replace(" <", "<");
            original = original.Replace("> ", ">");

            if (original.Contains("  "))
            {
                return TruncateHTMLSpaces(original.Replace("  ", " "));
            }
            else
            {
                return original;
            }
        }
    }
}