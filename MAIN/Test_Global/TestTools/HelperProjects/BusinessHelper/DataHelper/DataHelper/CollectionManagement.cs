using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCOM.Test.Tools;

namespace MSCOM.BusinessHelper
{
    public class CollectionHelper
    {
        /// <summary>
        /// Generates a string concatenated with a random number
        /// </summary>
        /// <returns>String concatenated with a random number</returns>
        public static string GenerateRandomString()
        {

            Random r = new Random();
            int n = r.Next();
            string rNo = n.ToString();
            string name = "test" + rNo;

            return name;
        }

        /// <summary>
        /// Given a list of string[], (2 Dimensional Data Structure like from CSV) and a location of a CSV this method will validate that the data in minimumExpectations also exists in 'actual'
        /// </summary>
        /// <param name="actual">Data set to be validated against minimum requirements</param>
        /// <param name="expectationsCSVPath">Location of the CSV file with the Minimum Expectations</param>
        /// <param name="useRegularExpressions"></param>
        /// <returns>True if all data in minimum is found in full. DDAStepException if fields are missing or if data does not match 100% for a given row in 'minimumExpectations'</returns>
        public static bool MeetsMinimumExpectationsAt(object actual, string expectationsCSVPath, bool useRegularExpressions = false)
        {
            return MeetsMinimumExpectations(actual, MSCOM.Test.Tools.FileManagement.parseCSV(MSCOM.Test.Tools.Environment.GetTestContentPath(expectationsCSVPath)), useRegularExpressions);
        }
        
        /// <summary>
        /// Given two lists of string[], (2 Dimensional Data Structure like from CSV) this method will validate that the data in minimumExpectations also exists in full
        /// </summary>
        /// <param name="actual">Data set to be validated against minimum requirements</param>
        /// <param name="expected">Data set with the needed data to be validated</param>
        /// <param name="useRegularExpressions"></param>
        /// <returns>True if all data in minimum is found in full. DDAStepException if fields are missing or if data does not match 100% for a given row in 'minimumExpectations'</returns>
        public static bool MeetsMinimumExpectations(object actual, object expected, bool useRegularExpressions = false)
        {
            List<string[]> actualResult = (List<string[]>) actual;
            List<string[]> minimumExpectations = (List<string[]>)expected;

            bool expectationsFirst = true;
            bool fullFirst = true;
            Dictionary<string, int> fullHeader = new Dictionary<string, int>();
            Dictionary<string, int> minimumExpectationsHeader = new Dictionary<string, int>();

            List<string[]> csvLog = new List<string[]>();
            csvLog.Add(new string[] { "Key", "Expected", "Actual" });

            string error = null;
            string errorHeader = "";
            string errorValues = "";
            bool wrongFullRow = false;
            int currentFullIndex = 0;

            List<string> foundActual = new List<string>();
            
            string currentActualResultCell = "";
            foreach (string[] rowInMinExpectations in minimumExpectations)
            {
                if (expectationsFirst)
                {
                    minimumExpectationsHeader = FileManagement.GetCSVHeaderIndexes(rowInMinExpectations);
                    expectationsFirst = false;
                }
                else
                {
                    currentFullIndex = 1;
                    foreach (string[] itemInActualResult in actualResult)
                    {
                        
                        if (fullFirst)
                        {
                            fullHeader = FileManagement.GetCSVHeaderIndexes(itemInActualResult);
                            fullFirst = false;
                        }
                        else
                        {
                            foundActual.Clear();
                            foreach (string currentHeaderItem in minimumExpectationsHeader.Keys)
                            {
                                try
                                {
                                    currentActualResultCell = itemInActualResult[fullHeader[currentHeaderItem]];  
                                }
                                catch (KeyNotFoundException)
                                {
                                    throw new MissingFieldException(string.Format("Expected field '{0}' as in minimumExpectations was not found in actualResult List.\n", currentHeaderItem));
                                }

                                if (useRegularExpressions && !System.Text.RegularExpressions.Regex.IsMatch(currentActualResultCell.Trim(), rowInMinExpectations[minimumExpectationsHeader[currentHeaderItem]].Trim()))
                                {
                                    wrongFullRow = true;
                                    goto WRONG_ROW;
                                }
                                else if (!useRegularExpressions && !currentActualResultCell.Trim().Equals(rowInMinExpectations[minimumExpectationsHeader[currentHeaderItem]].Trim(), StringComparison.InvariantCultureIgnoreCase))
                                {
                                    wrongFullRow = true;
                                    goto WRONG_ROW;
                                }
                                else
                                {
                                    foundActual.Add(currentActualResultCell);
                                }
                                
                                wrongFullRow = false;
                            }
                        WRONG_ROW:
                            if (currentFullIndex == actualResult.Count - 1 && wrongFullRow) //if the full list was scanned & the last row was the wrong row, then expectation was not found
                            {                                
                                foreach (string s in minimumExpectationsHeader.Keys)
                                {
                                    errorHeader += s + ",";
                                }

                                foreach (string s in rowInMinExpectations)
                                {
                                    errorValues += "\"" + s + "\",";
                                }

                                errorHeader = errorHeader.Substring(0, errorHeader.Length - 1);
                                errorValues = errorValues.Substring(0, errorValues.Length - 1);                                
                               
                                error += string.Format("Expectation: {0} was not met in provided Full List.\n", string.Format("'{0}' with values: '{1}'", errorHeader, errorValues));
                                csvLog.Add(new string[] { errorHeader, errorValues, "NOT FOUND" });

                                errorHeader = "";
                                errorValues = "";
                            }
                            else if (!wrongFullRow) //if here and not wrong row, then correct row was found
                            {
                                wrongFullRow = false;
                                MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Match found for Expectation '{0}' at Actual Result '{1}'", StringManipulation.ToString(rowInMinExpectations), StringManipulation.ToString(foundActual.ToArray())));
                                break; //found so looping to next expectation
                            }
                            currentFullIndex++;
                        }                        
                    }

                    fullFirst = true;
                    wrongFullRow = false;
                }
            }

            if (error != null)
            { 
                throw new MSCOM.DDA.DDAStepException(string.Format("Minimum expectations where not met by provided Full List, based on provided Expectations.\n{0}", error), csvLog);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualResult"></param>
        /// <param name="unwantedExpectations"></param>
        /// <returns></returns>
        public static bool DoesNotContainAnyOfMinimumExpectations(List<string[]> actualResult, List<string[]> unwantedExpectations)
        {
            bool actualFirst = true;
            bool expectationsFirst = true;
            Dictionary<string, int> actualHeader = new Dictionary<string, int>();
            Dictionary<string, int> minimumExpectationsHeader = new Dictionary<string, int>();

            List<string[]> csvLog = new List<string[]>();
            csvLog.Add(new string[] { "Key", "Expected", "Actual" });

            string error = null;
            int currentActualIndex = 1;

            if (unwantedExpectations.Count <= 1)
            {
                throw new InvalidOperationException("Expectations had no sufficient items");
            }
           
           
            string currentActualResultCell = "";
            
            foreach (string[] rowInActualResult in actualResult)
            {
                if (actualFirst)
                {
                    actualHeader = FileManagement.GetCSVHeaderIndexes(rowInActualResult);
                    actualFirst = false;
                }
                else
                {

                    foreach (string[] itemInExpectations in unwantedExpectations)
                    {

                        if (expectationsFirst)
                        {
                            minimumExpectationsHeader = FileManagement.GetCSVHeaderIndexes(itemInExpectations);
                            expectationsFirst = false;
                        }
                        else
                        {
                            foreach (string currentHeaderItem in minimumExpectationsHeader.Keys)
                            {
                                try
                                {
                                    currentActualResultCell = rowInActualResult[actualHeader[currentHeaderItem]];
                                }
                                catch (KeyNotFoundException)
                                {
                                    throw new MissingFieldException(string.Format("Expected field '{0}' as in minimumExpectations was not found in actualResult List.\n", currentHeaderItem));
                                }

                                if (System.Text.RegularExpressions.Regex.IsMatch(currentActualResultCell, itemInExpectations[minimumExpectationsHeader[currentHeaderItem]]))
                                {
                                    error += string.Format("A matching entry for '{0}' - '{1}' was found in Actual Result '{0}' - '{2}' at row {3}\n", currentHeaderItem, itemInExpectations[minimumExpectationsHeader[currentHeaderItem]], currentActualResultCell, currentActualIndex);
                                    csvLog.Add(new string[] { currentHeaderItem, "NOT FOUND", currentActualResultCell });
                                }
                            }
                            currentActualIndex++;
                        }
                    }
                    expectationsFirst = false;
                }
            }

            if (error != null)
            {
                throw new MSCOM.DDA.DDAStepException(string.Format("Some unwanted items were matched by items in the provided actualResult List, based on provided Expectations.\n{0}", error), csvLog);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <param name="order"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static bool IsSortedAlphabetically(object oList, string order, string columnIndex)
        {
            List<string[]> list = (List<string[]>) oList;
            var listWithoutHeader = list.Skip(1).ToArray();

            int column = -1;
            if(!Int32.TryParse(columnIndex.ToString(), out column))
            {
                throw new DDA.DDAStepException(string.Format("The provided column index '{0}' is not parsable as an 'int'", columnIndex));
            }

            if (order.ToUpper() == "ASCENDING")
            {
                if (!listWithoutHeader.SequenceEqual(listWithoutHeader.OrderBy(x => x[column - 1]).ToArray()))
                {
                    throw new DDA.DDAStepException(string.Format("The provided list was not sorted alphabetically in '{0}' order at column '{1}'.", order, columnIndex), list);
                }
            }
            else if (order.ToUpper() == "DESCENDING")
            {
                if (!listWithoutHeader.SequenceEqual(listWithoutHeader.OrderByDescending(x => x[column - 1]).ToArray()))
                {
                    throw new DDA.DDAStepException(string.Format("The provided list was not sorted alphabetically in '{0}' order at column '{1}'.", order, columnIndex), list);
                }
            }
            else
            {
                throw new DDA.DDAStepException(string.Format("The provided Sorting 'order', '{0}' is not valid", order));
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<string[]> RemoveCommented(List<string[]> target)
        {
            List<string[]> result = new List<string[]>();

            if (target.Count <= 2)
            {
                throw new DDA.DDAStepException("The provided list does not contain enough items.");
            }

            result.Add(target[0]);
            Dictionary<string, int> columns =  MSCOM.Test.Tools.FileManagement.GetCSVHeaderIndexes(result[0]);

            foreach (string[] row in target)
            {
                if (row[columns["isComment"]] == false.ToString())
                {
                    result.Add(row);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualList"></param>
        /// <param name="columnsToRemove"></param>
        /// <returns></returns>
        public static List<string[]> RemoveColumns(List<string[]> actualList, string[] columnsToRemove)
        {
            if (actualList.Count <= 2)
            {
                throw new DDA.DDAStepException("The provided list does not contain enough items.");
            }
         
            List<string[]> result = new List<string[]>();
            List<string> current = new List<string>();
            List<string> removableColumns = new List<string>();
            List<int> skipIndexes = new List<int>();

            int cColIterator = 0;
            bool first = true;            

            foreach(string s in columnsToRemove)
            {
                removableColumns.Add(s);
            }

            foreach (string[] row in actualList)
            {
                if (first)
                {
                    foreach (string actualColumn in row)
                    {
                        if (removableColumns.Contains(actualColumn))
                        {
                           skipIndexes.Add(cColIterator);
                        }
                        else
                        {
                          current.Add(actualColumn);                            
                        }
                        cColIterator++;
                    }                    

                    first = false;
                }
                else
                {
                    foreach (string actualColumn in row)
                    {
                        if (!skipIndexes.Contains(cColIterator))
                        {
                            current.Add(actualColumn);
                        }
                        cColIterator++;
                    }
                }

                cColIterator = 0;
                result.Add(current.ToArray());
                current = new List<string>();
            }
            return result;
        }

    }
}
