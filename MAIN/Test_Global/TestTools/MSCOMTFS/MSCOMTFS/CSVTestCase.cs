using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;
using System.Configuration;
using MSCOM.Test.Tools;

namespace MSCOM.Test.CSV
{
    public class CSVTestCase
    {
        public List<CSVDataIteration> DataIterations { get; set; }

        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; set; }

        /// <summary>
        /// Reads the CSV file and separates the data into ParameterNames and ParameterValues
        /// </summary>
        /// <param name="filePath">the path of the CSV file</param>
        public CSVTestCase(string filePath)
        {
            int n = 0;
            var reader = new System.IO.StreamReader(filePath);
            var reader1 = new System.IO.StreamReader(filePath);

            List<string> paramNames = new List<string>();
            List<string> paramValues = new List<string>();

            while (!reader.EndOfStream)                 //To determine the number of lines in the CSV file
            {
                var line = reader.ReadLine();
                n++;
            }

            while (!reader1.EndOfStream)
            {
                var data = reader1.ReadToEnd();
                var values = data.Split(',');
                int l = (values.Length) / n;                  //Number of parameters in the test case

                for (int k = 0; k < l; k++)
                {
                    paramNames.Add(values[k]);
                }
                for (int m = l; m < (n * l); m++)
                {
                    if (values[m].Contains("\r\n"))
                    {
                        string str = values[m];
                        string substr = str.Substring(2);                   //Eliminating the new line characters
                        values[m] = substr;
                    }
                    paramValues.Add(values[m]);                   //List which stores all the parameter values

                }
            }

            this.DataIterations = GetCSVParameters(paramNames, paramValues);
        }

        /// <summary>
        /// Loops through the parameter names and values lists to gather parameters and their values in iterations
        /// </summary>
        /// <param name="listKey">list containing the parameter names</param>
        /// <param name="listValue">list containing the parameter values</param>
        /// <returns>Returns a List of Dictionarys containing one item per CSV Iteration made of parameters name and their corresponding values</returns>
        public static List<CSVDataIteration> GetCSVParameters(List<string> listKey, List<string> listValue)
        {
            List<CSVDataIteration> result = new List<CSVDataIteration>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            int n = (listValue.Count) / (listKey.Count);                  //the number of data iterations for the test cases
            int x = 0;

            for (int m = 0; m < n; m++)
            {
                for (int i = 0; i < listKey.Count; i++)
                {

                    for (int j = x; j < listValue.Count; )
                    {
                        if (listKey[i] != null && listValue[j] != null)
                        {
                            parameters.Add(listKey[i], listValue[j]);                  //Adding the parameter name and value pair to a Dictionary object
                            x++;
                            break;
                        }
                    }
                }

                result.Add(new CSVDataIteration(parameters));
                parameters.Clear();
            }
            return result;
        }

        public void ReplaceValuesInAllIterations(ValuesDictionary valuesToReplace)
        {
            foreach (CSVDataIteration iter in DataIterations)
            {
                iter.ReplaceData(valuesToReplace);
            }
        }

        public void ReplaceValuesInAllIterations(string csvValuesFile)
        {
            ReplaceValuesInAllIterations(new ValuesDictionary(FileManagement.parseCSV(csvValuesFile)));
        }


    }
}

