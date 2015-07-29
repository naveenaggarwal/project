using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCOM.Test.MTM;
using Utilities.FileFormats.CSV;
using Utilities.FileFormats.Delimited;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace MSCOM.Test.Tools
{
    public static class CSVManager
    {

        private const string QUOTE = "\"";
        private const string COMMA = ",";
        private const string NEW_LINE = "\n";
        private const string ESCAPED_QUOTE = "\"\"";
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };
        private static string tcMigrationDescriptionMigratedFrom = "<p><strong>Migrated From: </strong><font color=\"#0080ff\"><a href=\"{0}\">{0}</a></font></p>";
        private static string tcMigrationDescriptionProjectCollection = "<p><strong>Original Project Collection: </strong><font color=\"#0080ff\">{1}</font></p>";
        private static string tcMigrationDescriptionProjectName = "<p><strong>Original Project Name: </strong><font color=\"#0080ff\">{2}</font></p>";
        private static string tcMigrationDescriptionAreaPath = "<p><strong>Original Area Path: </strong><font color=\"#0080ff\">{3}</font></p>";
        private static string tcMigrationDescriptionIterationPath = "<p><strong>Original Iteration Path: </strong><font color=\"#0080ff\">{4}</font></p>";
        private static string tcMigrationDescriptionOriginalWorkItemId = "<p><strong>Original Work Item Id: </strong><font color=\"#0080ff\"><a href=\"{0}/web/wi.aspx?id={5}\">{5}</a></font></p>";
        
        private static string tcMigrationDescriptionLinksTableHeader = "<tr><td style=\"width: 145px; font-family: Calibri; font-weight: bold; color: #FFFFFF; background-color: #99CCFF;\">Linked Work Items</td><td style=\"width: 80px; font-family: Calibri; font-weight: bold; color: #FFFFFF; background-color: #99CCFF;\">Link Type</td></tr>";
        private static string tcMigrationDescriptionLinksTableRow = "<tr><td style=\"font-family: Calibri;\"><a href=\"{0}/web/wi.aspx?id={1}\">{1}</a></td><td style=\"font-family: Calibri;\">{2}</td></tr>";
        
        private static string tcMigrationDescription = tcMigrationDescriptionMigratedFrom + tcMigrationDescriptionProjectCollection + tcMigrationDescriptionProjectName + tcMigrationDescriptionAreaPath + tcMigrationDescriptionIterationPath + tcMigrationDescriptionOriginalWorkItemId + "</br>";
        private static string tcMigrationDescriptionSuffix = "</br><strong>Original Work Item Description:</strong><p></br></p>";

        private static string outPutHTMLPath = string.Format(@"{0}\IO\{1}", System.IO.Directory.GetCurrentDirectory(), WorkItemsMigrate.Default.OutPutHTML);
        private static string outPutHTMLROWCodeEven = "<tr style=\"font-family: Calibri; vertical-align:text-top; background-color: #cfe9bb;\"><td>$FIELD$</td><td>$VALUE$</td></tr>";
        private static string outPutHTMLROWCodeOdd = "<tr style=\"font-family: Calibri; vertical-align:text-top; background-color: #FFFFFF;\"><td style=\"border-color:#70AD47; border-style:solid; border-width:.5pt;\">$FIELD$</td><td style=\"border-color:#70AD47; border-style:solid; border-width:.5pt;\">$VALUE$</td></tr>";
        private static string outPutHTMLTITLE = "$TITLE$";
        private static string outPutHTMLROW = "$ROW$";        

        public static string Encode(string s)
        {
            if (s.Contains(QUOTE))
                s = s.Replace(QUOTE, ESCAPED_QUOTE);

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
                s = QUOTE + s + QUOTE;

            return s;
        }

        public static string FromArrayToCSVHeaderLine(string[] array)
        {
            string result = "";
            bool first = true;
            foreach (string s in array)
            {
                if (!first)
                {
                    result += ",";
                }
                else
                {
                    first = false;
                }
                result += s;
            }
            return result;
        }

        public static string AppendFieldAndValueToHTML(string html, string field, string value, bool isRowOdd)
        {
            string result = html;
            if (field == "Steps" || field == "Parameters" || field == "LocalDataSource" || field == "Local Data Source")
            {
                value = System.Web.HttpUtility.HtmlEncode(value);
            }
            if (isRowOdd)
            {
                result = html.Replace(outPutHTMLROW, outPutHTMLROWCodeOdd.Replace("$FIELD$", field).Replace("$VALUE$", value) + outPutHTMLROW);
            }
            else
            {
                result = html.Replace(outPutHTMLROW, outPutHTMLROWCodeEven.Replace("$FIELD$", field).Replace("$VALUE$", value) + outPutHTMLROW);
            }
            return result;
        }

        public static string FromMTMTestCaseToCSVLine(MTMTestCase tc, string[] fieldsArray)
        {
            string result = tc.Id + "," + CSVManager.Encode(tc.Title);
            string rawValue = "";
            string issues = "";
            string allLinks = "";
            bool firstLink = true;

            foreach (string s in fieldsArray)
            {
                try
                {
                    if (s == "AllLinks")
                    {
                        allLinks = "";
                        foreach (Microsoft.TeamFoundation.WorkItemTracking.Client.RelatedLink link in tc.WorkItem.Links)
                        {
                            if (!firstLink)
                            {
                                allLinks += "|" + link.RelatedWorkItemId + "." + link.LinkTypeEnd.Name;                                
                            }
                            else
                            {
                                allLinks += link.RelatedWorkItemId + "." + link.LinkTypeEnd.Name;
                                firstLink = false;
                            }                            
                        }
                        result += "," + CSVManager.Encode(allLinks);
                        continue;
                    }
                    
                    rawValue = (string)tc.WorkItem[s.Split(':')[0]].ToString();
                }
                catch (Exception e)
                {
                    issues += string.Format("At WorkItem '{0}' there was a problem reading field '{1}'. Additional Info - {2}$", tc.Id, s, e.Message);
                    continue;
                }

                if (s == "Steps" && tc.steps == null)
                {
                    result += "," + CSVManager.Encode(ParseOldVersionOfRAWSteps(rawValue));
                }
                else 
                {
                    result += "," + CSVManager.Encode(rawValue);
                }
            }

            if (issues.Trim() != "")
            {
                throw new ArgumentException(issues);
            }

            return result;
        }

        public static WorkItem FromCSVLineToWorkItem(string[] fields, string[] data, WorkItem wi, out List<string> outPut)
        {
            if (fields.Length != data.Length - 2)
            {
                throw new DataMisalignedException(string.Format("Error: 'fields' length ({0}) differs from that of 'data' ({1})."));
            }

            string htmlOutPut = "";
            string allLinks = "";

            WorkItem result = wi;
            outPut = new List<string>();

            string issues = "";
            wi.Title = data[1];
            string currentField = "";
            string linksTable = "";
            string linksTableRows = "";

            var htmlOutPutContentReader = new System.IO.StreamReader(outPutHTMLPath);
            htmlOutPut = htmlOutPutContentReader.ReadToEnd();
            htmlOutPutContentReader.Close();
            htmlOutPut = htmlOutPut.Replace(outPutHTMLTITLE, wi.Title);

            foreach (string s in fields)
            {
                currentField = s.Split(':').Length > 1 ? s.Split(':')[1] : s;
                if (s.Split(':')[0] == "Area Path")
                {
                    wi.AreaPath = CSVManager.Decode(data[fields.ToList().IndexOf(s) + 2]);
                }
                else if (s.Split(':')[0] == "Iteration Path")
                {
                    wi.IterationPath = CSVManager.Decode(data[fields.ToList().IndexOf(s) + 2]);
                }
                else if (s == "AllLinks")
                {
                    if (CSVManager.Decode(data[fields.ToList().IndexOf(s) + 2]).Trim() != "")
                    {
                        allLinks = CSVManager.Decode(data[fields.ToList().IndexOf(s) + 2]).Trim();
                        foreach (string link in CSVManager.Decode(data[fields.ToList().IndexOf(s) + 2]).Split('|'))
                        {
                            linksTableRows += string.Format(tcMigrationDescriptionLinksTableRow, WorkItemsMigrate.Default.TFSServerOriginal, link.Split('.')[0], link.Split('.')[1]);
                        }

                        if (linksTableRows.Trim() != "")
                        {
                            linksTable = "<table>" + tcMigrationDescriptionLinksTableHeader + linksTableRows + "</table>";
                        }
                    }
                }
            }

            string[] valuesReplacementPair;
            string originalReplacementValue;
            string stringValue = "";
            string stringField = "";
            bool isRowOdd = true;

            foreach (string s in fields)
            {
                currentField = s.Split(':').Length > 1 ? s.Split(':')[1] : s;
                if (currentField == "AllLinks")
                {
                    continue;
                }

                try
                {
                    if (currentField == "Description")
                    {
                       var originaDescription = data[fields.ToList().IndexOf(s) + 2];
                       stringField = currentField;
                       stringValue = string.Format(tcMigrationDescription, WorkItemsMigrate.Default.TFSServerOriginal, WorkItemsMigrate.Default.TFSProjectCollectionOriginal, WorkItemsMigrate.Default.TFSProjectOriginal, wi.AreaPath, wi.IterationPath, data[0])
                        + linksTable + (originaDescription.Trim() == "" ? tcMigrationDescriptionSuffix.Replace("<p></br></p>", "") : tcMigrationDescriptionSuffix)
                        + CSVManager.Decode(originaDescription.Trim() == ""? "[None]" : originaDescription);

                       wi[stringField] = stringValue;
                       htmlOutPut = AppendFieldAndValueToHTML(htmlOutPut, stringField, stringValue, isRowOdd);
                       isRowOdd = !isRowOdd;
                    }
                    else if (currentField.Contains('[') && currentField.Contains(']'))
                    {
                        if (currentField.Contains('|'))
                        {
                            if (currentField.Contains('$'))
                            {
                                stringField = s.Split(':')[0];

                                valuesReplacementPair = currentField.Replace('[', ' ').Replace(']', ' ').Trim().Split('|');
                                originalReplacementValue = valuesReplacementPair[0].Split('$')[1];
                                stringValue = CSVManager.Decode(valuesReplacementPair[0].Replace("$" + originalReplacementValue + "$", valuesReplacementPair[1]));                                

                                wi[stringField] = stringValue;
                                htmlOutPut = AppendFieldAndValueToHTML(htmlOutPut, stringField, stringValue, isRowOdd);
                                isRowOdd = !isRowOdd;
                            }
                            else 
                            {
                                stringField = s.Split(':')[0];

                                valuesReplacementPair = currentField.Replace('[', ' ').Replace(']', ' ').Trim().Split('|');
                                stringValue = CSVManager.Decode(CSVManager.Decode(data[fields.ToList().IndexOf(s) + 2]) == valuesReplacementPair[0] ? valuesReplacementPair[1] : data[fields.ToList().IndexOf(s) + 2]);
                                wi[stringField] = stringValue;

                                wi[stringField] = stringValue;
                                htmlOutPut = AppendFieldAndValueToHTML(htmlOutPut, stringField, stringValue, isRowOdd);
                                isRowOdd = !isRowOdd;
                            }
                        } 
                        else
                        {
                            stringField = s.Split(':')[0];
                            stringValue = CSVManager.Decode(currentField.Replace('[', ' ').Replace(']', ' ').Trim());
                            wi[stringField] = stringValue;
                            htmlOutPut = AppendFieldAndValueToHTML(htmlOutPut, stringField, stringValue, isRowOdd);
                            isRowOdd = !isRowOdd;
                        }
                    }
                    else
                    {
                        stringField = currentField;
                        stringValue = CSVManager.Decode(data[fields.ToList().IndexOf(s) + 2]);
                        wi[stringField] = stringValue;
                        htmlOutPut = AppendFieldAndValueToHTML(htmlOutPut, stringField, stringValue, isRowOdd);
                        isRowOdd = !isRowOdd;
                    }
                }
                catch (Exception e)
                {
                    issues += string.Format("At WorkItem '{0}' there was a problem creating a copy with field '{1}'. Additional Info - {2}$", data[0], s, e.Message);
                }
            }

            if (issues.Trim() != "")
            {
                throw new ArgumentException(issues);
            }

            htmlOutPut = htmlOutPut.Replace(outPutHTMLROW, "");
            outPut.Add(htmlOutPut);
            outPut.Add(allLinks);
            return result;
        }

        public static string Decode(string s)
        {
            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
            }

            return s;
        }


        /// <summary>
        /// For a given CSV file, a List of string arrays is generated, 
        /// where each line is a List items and every item (commas-delimited) is a string array item
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string[]> ParseCSV(string path)
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

        private static string ParseOldVersionOfRAWSteps(string rawSteps)
        {
            if (rawSteps.Trim() == "")
            {
                return "";
            }

            string result = null;
            string[] resultArray = rawSteps.Replace("<BR/>", "\n").Split('\n');
            string sIter = "";
            int sIndex = 1;
            foreach (string s in resultArray)
            {
                sIter = s.Substring(s.IndexOf(". ") + 2);
                if (sIter[sIter.Length - 1] == '"')
                {
                    sIter = sIter.Substring(0, sIter.Length - 1);
                }
                result += string.Format("<step id=\"{0}\" type=\"{1}\"><parameterizedString><text>{2}</text></parameterizedString><parameterizedString /><description></description></step>", sIndex++, "ActionStep", sIter.Trim());
            }

            return string.Format("<steps id=\"0\" last=\"{0}\">{1}</steps>", sIndex, result);
        }
    }
}
