using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCOM.Test
{   
    public class TestDataValues
    {
        public string ParamName { get; set; }
        public string OriginalValue { get; set; } //Dictionary Key
        public string NewValue { get; set; }
        public string Note { get; set; }
    }

    public class ValuesDictionary
    {
        public Dictionary<string, TestDataValues> TestDataDictionary;

        private int paramNameIndex;
        private int originalValueIndex;
        private int newValueIndex;
        private int noteIndex;

        private string COL_PARAM_NAME = "paramName";
        private string COL_ORIGINAL_VALUE = "originalValue";
        private string COL_NEW_VALUE = "newValue";
        private string COL_NOTE = "note";

        private List<string[]> csvLogContent;
        
        public ValuesDictionary(List<string[]> csvLogContent)
        {
            this.csvLogContent = csvLogContent;
            TestDataValues current = null;
            TestDataDictionary = new Dictionary<string, TestDataValues>();

            bool first = true;

            foreach (string[] csvLine in csvLogContent)
            {
                if (!first)
                {
                    current = new TestDataValues { ParamName = csvLine[paramNameIndex], OriginalValue = csvLine[originalValueIndex], NewValue = csvLine[newValueIndex], Note = csvLine[noteIndex] };

                    if (current.OriginalValue.Trim() != "" && current.NewValue.Trim() != "")
                    {
                        TestDataDictionary.Add(current.OriginalValue, current);
                    }
                }
                else
                {
                    identifyIndexes(csvLine);
                    first = false;
                }
            }
        }

        /// <summary>
        /// This method is intended to correctly initialize indexes for each columnn in the given csv header. 
        /// Said indexes are used to identify columns based on names regardless of the order. Column names are based on COL_... private variables declared in these class
        /// </summary>
        /// <param name="csvHeader">A string array with a column name in each item</param>
        private void identifyIndexes(string[] csvHeader)
        {
            string error = null;

            paramNameIndex = Array.IndexOf(csvHeader, COL_PARAM_NAME);
            originalValueIndex = Array.IndexOf(csvHeader, COL_ORIGINAL_VALUE);
            newValueIndex = Array.IndexOf(csvHeader, COL_NEW_VALUE);
            noteIndex = Array.IndexOf(csvHeader, COL_NOTE);

            error = ((paramNameIndex < 0) ? string.Format("The field '{0}' was not found in the provided csvHeader.\n", COL_PARAM_NAME, csvHeader) : "") +
                ((originalValueIndex < 0) ? string.Format("The field '{0}' was not found in the provided csvHeader.\n", COL_ORIGINAL_VALUE, csvHeader) : "") +
                ((newValueIndex < 0) ? string.Format("The field '{0}' was not found in the provided csvHeader.\n", COL_NEW_VALUE, csvHeader) : "") +
                ((noteIndex < 0) ? string.Format("The field '{0}' was not found in the provided csvHeader.\n", COL_NOTE, csvHeader) : "");

            if (error.Trim() != "")
            {
                throw new Exception(string.Format("The provided csv file has invalid header line '{1}'. Unexpected error(s) found.\n {2}", String.Join(", ", csvHeader), error));
            }

        }
    }
}
