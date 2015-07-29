using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Utilities.FileFormats.CSV;
using Utilities.FileFormats.Delimited;
using System.Drawing;

namespace MSCOM.Test.Tools
{
    public static class FileManagement
    {
        private const string QUOTE = "\"";
        private const string COMMA = ",";
        private const string NEW_LINE = "\n";
        private const string ESCAPED_QUOTE = "\"\"";
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileContent(string path)
        {
            string result = "";
            using (StreamReader readFile = new StreamReader(path))
            {
                string line;

                while ((line = readFile.ReadLine()) != null)
                {
                    if (line.Trim() != "")
                    {
                        result += line + "\n";
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// For a given CSV file, a List if string arrays is generated, 
        /// where each line is a List items and every item (commas-delimited) is a string array item
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static List<string[]> parseCSVOLD(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            using (System.IO.StreamReader readFile = new System.IO.StreamReader(path))
            {
                string line;
                string[] row;

                while ((line = readFile.ReadLine()) != null)
                {
                    if (line.Trim() != "")
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }

            return parsedData;
        }

        

        public static List<string[]> parseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();
            string[] row;
            int i = 0;

            string encodedContent = System.IO.File.ReadAllText(path);
            encodedContent = encodedContent.Replace("\"\"", "$QUOTES$");
            CSV csvObject = new CSV(encodedContent);
            var csvContent = csvObject.Rows;
            foreach (Row csvLines in csvContent)
            {
                row = new string[csvLines.Cells.Count];

                foreach (Cell cell in csvLines.Cells)
                {
                    row[i++] = Decode(cell.Value.Replace("$QUOTES$", "\""));
                }
                parsedData.Add(row);
                i = 0;
            }

            return parsedData;
        }

        private static string Decode(string s)
        {
            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
            }

            return s;
        }

        public static void toTXTFile(string message, string filePath)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);

            foreach (string line in message.Split('\n'))
            {
                file.WriteLine(line);
            }

            file.Close();
        }

        public static void toCSVFile(List<string[]> items, string filePath)
        {
            if (items == null)
            {
                MSCOM.Test.Tools.TestAgent.LogToTestResult("Could not log List as CSV because it was null.");
                return;
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);

            foreach (string[] line in items)
            {
                file.WriteLine(Tools.StringManipulation.ToString(line));
            }

            file.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="filePath"></param>
        /// <param name="tcID"></param>
        public static void toHTMLFile(List<string[]> items, string filePath, string title, string message = "")
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);

            string cssStyle = AutomationSettings.HtmlCSS;     
            string head = string.Format("<head>\n<title>{0}</title>\n<style>\n{1}\n</style>\n</head>\n", title, cssStyle);
            string header = string.Format("<h1>{0}</h1>\n", title);

            bool first = true;
            string tableRow = "";
            string table = "";
            bool odd = false;

            foreach (string[] tr in items)
            {
                foreach (string td in tr)
                {
                    if (first)
                    {
                        tableRow += string.Format("<th>{0}</th>", System.Web.HttpUtility.HtmlEncode(td));
                    }
                    else
                    {
                        tableRow += string.Format("<td>{0}</td>", System.Web.HttpUtility.HtmlEncode(td));
                    }
                }

                if (odd)
                {
                    table += string.Format("<tr class=\"alt\">{0}</tr>\n", tableRow);
                    odd = false;
                }
                else
                {
                    table += string.Format("<tr>{0}</tr>\n", tableRow);
                    odd = true;
                }

                tableRow = "";
                first = false;
            }

            table = string.Format("<h2>Log Results</h2>\n<table id=\"logTable\">\n{0}</table>\n", table);
            string error = message.Trim() != "" ? string.Format("<h2>{0}</h2>\n<p>{1}</p>\n", "Thrown Exception Message", message.Replace("\n", "</br>")): "";
            string pageHTML = string.Format("<html>\n{0}<body>{1}{2}{3}</body>\n</html>", head, header, table, error);

            foreach (string line in pageHTML.Split('\n'))
            {
                file.WriteLine(line);
            }

            file.Close();            
        }

        public static Dictionary<string, int> GetCSVHeaderIndexes(string[] header)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            int i = 0;
            foreach (string col in header)
            {
                result.Add(col, i++);
            }
            return result;
        }
    }
}
