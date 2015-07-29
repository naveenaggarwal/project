using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Client;
using System.Configuration;

namespace MSCOM.Test.TFS
{
    public class TFSContext
    {
        #region PROPERTIES

        private Dictionary<string, string> settingsData = new Dictionary<string, string>();
        public Project tfsProject { get; set; }        
        private ApplicationSettingsBase settingsFileObject = null;

        #endregion 

        #region PUBLIC

        /// <summary>
        /// Gets the Work Item Context. It gets the TFS Project object to read the paramter values from the MTM
        /// </summary>
        /// <param name="tfsServer">TFS Server name</param>
        /// <param name="tfsProjectName">TFS Project name</param>        
        public TFSContext(string tfsServer, string tfsProjectName, string tfsProjectCollection = "")
        {
            // If the TFS Server name or Project name is empty or null
            if (string.IsNullOrEmpty(tfsServer))
            {
                throw new ArgumentNullException("tfsServer");
            }
            else if (string.IsNullOrEmpty(tfsProjectName))
            {
                throw new ArgumentNullException("tfsProjectName");
            }
            else
            {
                tfsProject = ConnectTFSAndGetProject(tfsServer + (tfsProjectCollection.Trim() == "" ? "" : "/" + tfsProjectCollection), tfsProjectName);
            }
        }

        #endregion

        #region PRIVATE

        /// <summary>        
        /// Connects to TFS server and  returns the project object based on the given tfsProjectName
        /// </summary>
        /// <param name="tfsServer">TFS Server name</param>
        /// <param name="tfsProjectName">TFS Project name</param>  
        /// <returns>TFS Project object</returns>
        private Project ConnectTFSAndGetProject(string tfsServer, string tfsProjectName)
        {
            //TFS object
            Project tfsTestProject;

            //Connects to TFS server
            TfsTeamProjectCollection tfsTPC = new TfsTeamProjectCollection(new Uri(tfsServer));
            MSCOM.Test.Tools.TestAgent.LogToTestResult(string.Format(@"Current User - {0}\{1}", System.Environment.UserDomainName, System.Environment.UserName));
            tfsTPC.EnsureAuthenticated();
            //Gets the workitems from the TFS 

            WorkItemStore store = new WorkItemStore(tfsTPC);
            //WorkItemStore store = (WorkItemStore)tfsTPC.GetService(typeof(WorkItemStore));

            //Gets the project related data from the TFS workitem store           
            tfsTestProject = store.Projects[tfsProjectName];

            return tfsTestProject;             
        }
        
        #endregion

        #region QUARANTINE CODE

        /// <summary>
        /// Gets the WorkItem Context. It gets the TFS Project object to read the paramter values from the MTM and gives the
        /// modified paramter values using the settings files with the agent name
        /// </summary>
        /// <param name="tfsServer">TFS Server name</param>
        /// <param name="tfsProjectName">TFS Project name</param>
        /// <param name="settingsObject">settings file object</param>
        /// <param name="agentName">name of the agent to get the strings collection to modify the MTM paramter value</param>      
        /// Private until needed...
        private TFSContext(string tfsServer, string tfsProjectName, ApplicationSettingsBase settingsObject, string agentName)
        {
            // If Parameters is empty or null
            if (string.IsNullOrEmpty(tfsServer))
            {
                throw new ArgumentNullException("tfsServer");
            }
            else if (string.IsNullOrEmpty(tfsProjectName))
            {
                throw new ArgumentNullException("tfsProjectName");
            }
            else if (settingsObject == null)
            {
                throw new ArgumentNullException("settingsObject");
            }
            else if (string.IsNullOrEmpty(agentName))
            {
                throw new ArgumentNullException("agentName");
            }
            else
            {
                //Gets the TFSProject object                            
                tfsProject = ConnectTFSAndGetProject(tfsServer, tfsProjectName);

                string agent = agentName;
                if (settingsObject.Properties[agent] == null)
                {
                    agent = "DEFAULT";
                }
                //If settings object has data
                if (settingsObject.Properties[agent] != null && settingsObject.Properties[agent].DefaultValue != null)
                {
                    //Sets the settings object 
                    settingsFileObject = settingsObject;

                    //Array to hold the key and value
                    string[] settingKeyValue = new string[2];

                    //xml data with the setting key and values
                    string xmlData = settingsFileObject.Properties[agent].DefaultValue.ToString();

                    //Creating the dictionary object with key and values for the given agent name                  

                    //Reading the xml data
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlData)))
                    {
                        // Parse the file and display each of the nodes.
                        while (reader.Read())
                        {
                            //If node type is the text type
                            if (reader.NodeType == XmlNodeType.Text)
                            {
                                //Splitting to the array strinng
                                settingKeyValue = reader.Value.Split('|');

                                //Adding keys and values to the dictionary object
                                settingsData.Add(settingKeyValue[0], settingKeyValue[1]);
                            }
                        }
                    }
                }
            }
        }

        

        /// <summary>
        /// Map and	Replace parameters based on MTM value for a given TC or Execution
        /// </summary>
        /// Private until needed...
        private TFSContext(string tfsServer, string tfsProjectName, string mtmParametersConfigPath, bool replacementEnabled)
        {

            // If Parameters is empty or null
            if (string.IsNullOrEmpty(tfsServer))
            {
                throw new ArgumentNullException("tfsServer");
            }
            else if (string.IsNullOrEmpty(tfsProjectName))
            {
                throw new ArgumentNullException("tfsProjectName");
            }
            else if (string.IsNullOrEmpty(mtmParametersConfigPath))
            {
                throw new ArgumentNullException("mtmParametersConfigPath");
            }
            else
            {
                //Gets the TFSProject object                            
                tfsProject = ConnectTFSAndGetProject(tfsServer, tfsProjectName);
                if (replacementEnabled)
                {
                    settingsData = GetReplaceSettingData(mtmParametersConfigPath);
                }
            }
        }

        #endregion

        #region INCOMPLETE

        /// <summary>
        /// Get Replace Setting Data
        /// </summary>
        /// <param name="mtmParametersConfigPath">MTM Parameter Config Path</param>
        /// <returns>Replacement Setting Data</returns>
        /// Private until needed...
        private static Dictionary<string, string> GetReplaceSettingData(string mtmParametersConfigPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(mtmParametersConfigPath);
            XmlNodeList nodes = xmlDoc.SelectNodes("/Items/ValuesToBeUpdated/Value");
            Dictionary<string, string> settingsData = new Dictionary<string, string>();
            foreach (XmlNode node in nodes)
            {
                try
                {
                    settingsData.Add(node.Attributes["OldValue"].InnerText, node.Attributes["UpdatedValue"].InnerText);
                }
                catch (Exception e)
                {
                    throw new Exception("Duplicate Old Values found please check your MTMParameters.config file\n" + e.Message);
                }
            }
            return settingsData;
        }
        
        /// <summary>
        /// Transforms the paramter values using the settings file
        /// </summary>
        /// <param name="settingsOriginalString">Paramter string which is to be transformed</param>
        /// <returns>Updated paramter value string</returns>
        /// Private until needed...
        private string TransformSettings(string settingsOriginalString)
        {
            //If dictionary object is available
            if (settingsData != null)
            {
                //Looping through all keys from the dictionary object
                foreach (string keyName in settingsData.Keys)
                {
                    //If parameter value is not empty string
                    if (settingsData[keyName].Length > 0)
                    {
                        //Modifying the parameter value
                        settingsOriginalString = settingsOriginalString.ToLower().Replace(keyName.ToLower(), settingsData[keyName]);
                    }
                }
            }
            return settingsOriginalString;
        }

        #endregion
    }
}
