using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSCOM.Test.MTM
{
    /// <summary>
    /// Parameter Iteration object to store the dictionary object.
    /// Using indexer, it returns the value by passing the key name
    /// </summary>
    public class MTMDataIteration
    {
        public Dictionary<string, KeyValuePair<string,string>> data;

        public string this[string name]
        {
            get
            {
                try
                {
                    return data[name].Value;
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException(string.Format("The parameter '@{0}' was not found in the MTMDataIteration. Make sure Test Case parameter and what's been requested are the same.", name));
                }
            }
        }

        /// <summary>
        /// Constructor to set the dictionary object to store paramter keys and values
        /// </summary>
        /// <param name="values"></param>
        internal MTMDataIteration(Dictionary<string, string> values)
        {
            data = new Dictionary<string, KeyValuePair<string, string>>();
            foreach(string key in values.Keys)
            {
                data.Add(key, new KeyValuePair<string,string>(values[key], values[key]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesToReplace"></param>
        public void ReplaceData(ValuesDictionary valuesToReplace)
        {
            if (data.Count < 1)
            {
                return;
            }

            var descTestDataDictionary = from s in valuesToReplace.TestDataDictionary.Keys                                           
                                           select s;

            Dictionary<string, KeyValuePair<string, string>> newData = null;
            string current = "";
            bool wasReplaced = false;

            foreach (string oldValue in descTestDataDictionary) //for each entry in the ReplacementDictionary
            {
                newData = new Dictionary<string, KeyValuePair<string, string>>();
                
                foreach (string key in data.Keys) //for each parameter in the provided iteration (valuesToReplace)
                {
                    current = data[key].Value;
                    wasReplaced = data[key].Key != data[key].Value;

                    if (current.Contains(oldValue))
                    {
                        current = current.Replace(oldValue, valuesToReplace.TestDataDictionary[oldValue].NewValue);
                        newData.Add(key, new KeyValuePair<string, string>(data[key].Key, current));
                        
                        if (!wasReplaced)
                        {
                             MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Note - Value from MTM Data Iteration '{0}' matched originalValue key '{1}' (found in MTMReplacementValues.csv)" +
                                " and then replaced with '{2}'.", key, data[key].Key, current));
                        }
                        else
                        {
                            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format("Warning - While replacing Itration Parameter's data, a key was found that could lead to unexpected " +
                                "or unwanted replacements of data. \nSpecifically, value from MTM Data Iteration '{0}' matched originalValue key '{1}' (found in MTMReplacementValues.csv)" + 
                                " and then replaced with '{2}'. \nUse unique names for replacemnet keys.", key, data[key].Key, data[key].Value));
                        }
                    }
                    else 
                    {
                        newData.Add(key, new KeyValuePair<string, string>(data[key].Key, data[key].Value));
                    }                    
                }
                data = newData;
            }            
        }
    }
}
