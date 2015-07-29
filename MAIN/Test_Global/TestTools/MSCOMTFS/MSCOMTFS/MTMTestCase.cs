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

namespace MSCOM.Test.MTM
{
    public class MTMTestCase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public string State { get; set; }
        public string Priority { get; set; }
        public string AutomationStatus { get; set; }
        public string AreaPath { get; set; }
        public string IterationPath { get; set; }
        public string Description { get; set; }
        public List<MTMStep> steps;
        public List<MTMDataIteration> DataIterations { get; set; }

        private string rawSteps;
        private string rawParameters;
        private string rawParametersValues;
        private WorkItem wi;
        
        public string RawSteps { get { return rawSteps; } }
        public string RawParameters { get { return rawParameters; } }
        public string RawParametersValues { get { return rawParametersValues; } }
        public WorkItem WorkItem { get { return wi; } }

        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; set; }

        public static void WorkItemToFile(int id, Project tfsProject)
        {
            MTMTestCase tc = new MTMTestCase(id, tfsProject);
            
            System.IO.StreamWriter w = new System.IO.StreamWriter(string.Format("{0}_rawSteps.xml", id));
            w.Write(tc.rawSteps);
            w.Close();

            w = new System.IO.StreamWriter(string.Format("{0}_rawParameters.xml", id));
            w.Write(tc.rawParameters);
            w.Close();

            w = new System.IO.StreamWriter(string.Format("{0}_rawParametersValues.xml", id));
            w.Write(tc.rawParametersValues);
            w.Close();  
        }

        private MTMTestCase()
        { }

        public static MTMTestCase FromWorkItemInFile(int id)
        {
            MTMTestCase result = new MTMTestCase();
            result.Id = id;

            result.rawSteps = FileManagement.GetFileContent(string.Format("{0}_rawSteps.xml", id));

            if (result.rawSteps.Trim() != "")
            {
                result.steps = GetSteps(result.rawSteps);
            }

            result.rawParametersValues = FileManagement.GetFileContent(string.Format("{0}_rawParametersValues.xml", id)); ;

            if (result.rawParametersValues != null)
            {
                result.rawParameters = FileManagement.GetFileContent(string.Format("{0}_rawParameters.xml", id));
                result.DataIterations = GetParameters(result.rawParametersValues);
            }

            return result;
        }

        public MTMTestCase(TestContext testContext, Project tfsProject)
        {
            TestContext = testContext;
            int id = Int32.Parse(this.TestContext.Properties["TestCaseId"].ToString());
            DataSet ds = new DataSet();
            this.wi = tfsProject.Store.GetWorkItem(id);

            this.Id = wi.Id;
            this.Title = wi.Title;
            this.AssignedTo = GetWorkItemFieldValue(wi, "Assigned To");
            this.State = wi.State;
            this.Priority = GetWorkItemFieldValue(wi, "Priority");
            this.AutomationStatus = GetWorkItemFieldValue(wi, "Automation status");
            this.AreaPath = wi.AreaPath;
            this.IterationPath = wi.IterationPath;
            this.Description = wi.Description;

            if (wi.Store.GetWorkItem(this.Id)["Steps"] != null)
            {
                this.rawSteps = wi.Store.GetWorkItem(this.Id)["Steps"].ToString();
                if (this.rawSteps.Trim() != "")
                {
                    this.steps = GetSteps(rawSteps);
                }
            }

            this.rawParametersValues = GetLocalDataSource(wi);

            if (this.rawParametersValues != null)
            {
                this.rawParameters = wi.Store.GetWorkItem(this.Id)["Parameters"].ToString();
                this.DataIterations = GetParameters(this.rawParametersValues);
            }   
        }

        /// <summary>
        /// Load class from MTM using TFS and Project specified in MAIA.config
        /// </summary>
        /// <param name="id">VSTF WorkItem-TestCase id</param>
        public MTMTestCase(int id, Project tfsProject)
        {
            DataSet ds = new DataSet();
            this.wi = tfsProject.Store.GetWorkItem(id);

            this.Id = wi.Id;
            this.Title = wi.Title;
            this.AssignedTo = GetWorkItemFieldValue(wi, "Assigned To");
            this.State = wi.State;
            this.Priority = GetWorkItemFieldValue(wi, "Priority");
            this.AutomationStatus = GetWorkItemFieldValue(wi,"Automation status");
            this.AreaPath = wi.AreaPath;
            this.IterationPath = wi.IterationPath;  
            this.Description = wi.Description;
            
            if (wi.Store.GetWorkItem(this.Id)["Steps"] != null)
            {
                this.rawSteps = wi.Store.GetWorkItem(this.Id)["Steps"].ToString();
                if (this.rawSteps.Trim() != "")
                {
                    this.steps = GetSteps(rawSteps);
                }                
            }

            this.rawParametersValues = GetLocalDataSource(wi);

            if (this.rawParametersValues != null)
            {
                this.rawParameters = wi.Store.GetWorkItem(this.Id)["Parameters"].ToString();
                this.DataIterations = GetParameters(this.rawParametersValues);                
            }            
        }
        
        /// <summary>
        /// Loops through TFS interanl data source to gather steps Actions and Expected Results
        /// </summary>
        /// <param name="xmlData">TFS Workitem internal data source xml data for steps</param>
        /// <returns>Returns a list of MTMSteps objects, each loaded with Actions and ExpectedResults as in MTM Test Cases</returns>
        public static List<MTMStep> GetSteps(string xmlData)
        {
            List<MTMStep> result = new List<MTMStep>();
            MTMStep current = null;
            MTMStep previous = null;
            XmlDocument stepsDoc = new XmlDocument();

            try
            {
                stepsDoc.LoadXml(xmlData);
            }
            catch (XmlException)
            {
                return null;
            }

            int position = 0;

            foreach (XmlElement s in stepsDoc.SelectNodes("/steps/step"))
            {
                bool flag = true;
                XmlNodeList innerElememts = s.ChildNodes;

                previous = current;
                current = new MTMStep();
                foreach (XmlNode childNode in innerElememts)
                {
                    if (childNode.Name != "parameterizedString")
                    {
                        break;
                    }
                    XmlNodeList list = childNode.ChildNodes;

                    if (flag)
                    {
                        current.Action = GetFullStepFromXML(list).ToString();
                        flag = false;
                    }
                    else
                    {
                        current.ExpectedResult = GetFullStepFromXML(list).ToString();
                    }
                }

                current.previous = previous;
                result.Add(current);
                current.Position = position++;

            }
            return result;
        }

        /// <summary>
        /// Loops through TFS interanl data source to gather parameters and their values in iterations
        /// </summary>
        /// <param name="xmlData">TFS Workitem internal data source xml data for parameters</param>
        /// <returns>Returns a List of Dictionarys containing one item per MTM Iteration made of parameters name and their corresponding values</returns>
        public static List<MTMDataIteration> GetParameters(string xmlData)
        {
            List<MTMDataIteration> result = new List<MTMDataIteration>();
            Dictionary<string, string> parameterValues;
            //If there is no parameters data
            if (xmlData == null || xmlData == "")
            {
                return result;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            foreach (XmlNode iterationNode in xmlDoc.GetElementsByTagName("NewDataSet")[0].ChildNodes) //foreach iteration
            {
                if (iterationNode.Name != "xs:schema")
                {
                    parameterValues = new Dictionary<string, string>();

                    foreach (XmlNode valueNode in iterationNode.ChildNodes)
                    {
                        parameterValues.Add(valueNode.Name, valueNode.InnerText);
                    }

                    result.Add(new MTMDataIteration(parameterValues));
                }
            }

            return result;
        }

        public void ReplaceValuesInAllIterations(ValuesDictionary valuesToReplace)
        {
            foreach (MTMDataIteration iter in DataIterations)
            {
                iter.ReplaceData(valuesToReplace);
            }
        }

        public void ReplaceValuesInAllIterations(string csvValuesFile)
        {
            ReplaceValuesInAllIterations(new ValuesDictionary(FileManagement.parseCSV(csvValuesFile)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wi"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string GetWorkItemFieldValue(WorkItem wi, string fieldName)
        {
            string result = "";
            int val = -1;
                foreach(Field f in wi.Fields)
                {
                    if (f.Name == fieldName)
                    {
                        if (Int32.TryParse(f.Value.ToString(), out val))
                        {
                            result = val.ToString();
                        }
                        else
                        {
                            result = (string) f.Value;
                        }
                    }
                }
             
            return result;        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wi"></param>
        /// <returns></returns>
        private string GetLocalDataSource(WorkItem wi)
        {
            string result = null;            
            foreach (Field f in wi.Fields)
            {
                if (f.Name == "LocalDataSource" || f.Name == "Local Data Source")
                {
                    return f.Value.ToString();
                }                
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        private static StringBuilder GetFullStepFromXML(XmlNodeList nodeList)
        {

            StringBuilder stringValue = new StringBuilder();

            if (nodeList.Count != 0)
            {
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "text")
                    {
                        stringValue.Append(node.InnerText);
                    }
                    else if (node.Name == "parameter")
                    {
                        stringValue.Append("@");
                        stringValue.Append(node.InnerText);
                    }
                }
            }
            else
            {
                stringValue.Append("");
            }

            return stringValue;
        }

        #region QUARANTINE CODE

        /// <summary>
        /// Gets the paramter values from MTM for the given testcase ID
        /// If settings file is used, it parses the MTM parameter values and gives updated paramter values
        /// </summary>
        /// <param name="testCaseID">testcase ID</param>
        /// <returns>TCContext object which will have TCIteration list</returns>
        /// Private until needed...
        private MTMTestCase GetTestCaseContext(string testCaseID)
        {
            //// If testcaseid is null or empty
            //if (string.IsNullOrEmpty(testCaseID))
            //{
            //    throw new ArgumentNullException("testCaseID");
            //}

            //// Loading the testcase parameters data from MTM to xml document
            ////Creating the xml document 
            //XmlDocument xmlDoc = null;

            ////Creating the testcase context object
            MTMTestCase testcaseContext = null; // new MTMTestCase();

            ////Gets the testcase data from TFS for given testcase ID
            //WorkItem testCase = tfsProject.Store.GetWorkItem(Int32.Parse(testCaseID));

            ////If parameters data is available
            //if (testCase["LocalDataSource"] != null)
            //{
            //    //Creates the new xml document
            //    xmlDoc = new XmlDocument();

            //    //Loads the xml document
            //    xmlDoc.LoadXml(testCase["LocalDataSource"].ToString());
            //}

            //// tranforming the testcase parameters data

            ////List object to hold the TCIteration objects
            //List<TCIteration> tcIterations = new List<TCIteration>();
            ////If xml document exists
            //if (xmlDoc != null)
            //{
            //    //Dictionary object to hold the key and values
            //    Dictionary<string, string> data;

            //    //Node path to parse the paramter tables
            //    const string nodepath = "/NewDataSet/Table1";
            //    //Getting the nodes using the node path
            //    XmlNodeList nodes = xmlDoc.SelectNodes(nodepath);
            //    //Looping through all parent nodes
            //    foreach (XmlNode parentNode in nodes)
            //    {
            //        data = new Dictionary<string, string>();
            //        //Looping through all nodes and sets the datatable column values
            //        for (int nodeIndex = 0; nodeIndex < parentNode.ChildNodes.Count; nodeIndex++)
            //        {
            //            //If paramter value exists(i.e not empty string)
            //            if (parentNode.ChildNodes[nodeIndex].InnerText.Length > 0)
            //            {
            //                //Replacing the parameter value with the settings value
            //                data.Add(parentNode.ChildNodes[nodeIndex].Name, TransformSettings(parentNode.ChildNodes[nodeIndex].InnerText));
            //            }
            //        }

            //        //If dictionary has keys and values(Logic to avoid empty parameter iteration from MTM)
            //        if (data.Keys.Count > 0)
            //        {
            //            //Creating TCIteration object with the dictionary object
            //            TCIteration testcaseIteration = new TCIteration(data);

            //            //Adding TCIteration object to tcIterations list object
            //            tcIterations.Add(testcaseIteration);
            //        }
            //    }
            //}
            //testcaseContext.Iterations = tcIterations;

            return testcaseContext;
        }

        /// <summary>
        /// Maps Iterations parameters/values with MAIA.config variables to be replaced and performs the replacements
        /// accordingly into this.IterationsReplacedParams as an altered copy of this.MTMIterationsParams
        /// </summary>
        private void MapAndReplaceVariables()
        {
            //foreach (Dictionary<string, string> paramsIteration in this.IterationsReplacedParams)
            //{
            //    List<string> toReplaceValues = new List<string>();
            //    foreach (string oldValue in DDAConfigMan.variables.Keys)
            //    {
            //        if (oldValue.Length > 1 && (oldValue.First().Equals('@'))) //This is a parameter
            //        {
            //            if (paramsIteration.ContainsKey(oldValue.Remove(0, 1)))
            //            {
            //                paramsIteration[oldValue.Remove(0, 1)] = MAIAConfigurationManager.variables[oldValue];
            //            }
            //        }
            //        else //it is a value
            //        {
            //            if (paramsIteration.ContainsValue(oldValue))
            //            {
            //                foreach (string name in paramsIteration.Keys)
            //                {
            //                    if (paramsIteration[name] == oldValue)
            //                    {
            //                        toReplaceValues.Add(name);
            //                    }
            //                }

            //                foreach (string value in toReplaceValues)
            //                {
            //                    paramsIteration[value] = DDAConfigMan.variables[oldValue];
            //                }
            //                toReplaceValues = new List<string>();
            //            }
            //        }
            //    }
            //}
        }

        #endregion
    }
}


