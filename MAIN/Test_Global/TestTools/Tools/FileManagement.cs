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
