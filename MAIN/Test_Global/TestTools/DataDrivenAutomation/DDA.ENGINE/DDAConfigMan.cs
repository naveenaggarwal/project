using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MSCOM.DDA
{
    public class DDAConfigMan
    {
        public static Settings settings;
        public static Dictionary<string, string> variables;
        public static Dictionary<string, Method> methods;

        static DDAConfigMan()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\MAIA.config");

            var settingsNode = xmlDoc.SelectSingleNode("configuration/settings");

            settings = new Settings(settingsNode);
            var variablesNode = xmlDoc.SelectSingleNode("configuration/variables");
            variables = new Dictionary<string, string>();
            foreach (XmlNode replace in variablesNode)
            {
                if (replace.NodeType == XmlNodeType.Element)
                {
                    try
                    {
                        variables.Add(replace.Attributes["oldValue"].InnerText, replace.Attributes["updatedValue"].InnerText);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Duplicate key found please check your variables XML definition. [{0}|{1}] \n {2}", replace.Attributes["oldValue"].InnerText, replace.Attributes["updatedValue"].InnerText, e.Message));
                    }
                }
            }

            var methodsNode = xmlDoc.SelectSingleNode("configuration/methods");
            methods = new Dictionary<string, Method>();
            foreach (XmlNode pair in methodsNode)
            {
                if (pair.NodeType == XmlNodeType.Element)
                {
                    try
                    {
                        methods.Add(pair.Attributes["key"].InnerText, new Method(pair, pair.Attributes["key"].InnerText));
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Duplicate key found please check your variables XML definition. [{0}|{1}] \n {2}", pair.Attributes["oldValue"].InnerText, pair.Attributes["methodSignature"].InnerText, e.Message));
                    }
                }
            }
        }


    }

    public class Settings
    {
        public string logFilePath = "";
        public string loggingOption = "";
        public string tfsServer = "";
        public string tfsProjectName = "";

        public Settings(XmlNode settings)
        {
            logFilePath = settings.Attributes["logFilePath"].InnerText;
            loggingOption = settings.Attributes["loggingOption"].InnerText;
            tfsServer = settings.Attributes["tfsServer"].InnerText;
            tfsProjectName = settings.Attributes["tfsProjectName"].InnerText;
        }
    }

    public class Method
    {
        public string methodSignature = "";
        public string reference = "";
        public string assembly = "";
        public Dictionary<string, string> mappings = new Dictionary<string, string>();
        public string pairKey = "";

        public Method(XmlNode pair, string key)
        {
            this.pairKey = key;
            this.methodSignature = pair.Attributes["methodSignature"].InnerText;
            this.reference = pair.Attributes["reference"] == null ? "" : pair.Attributes["reference"].InnerText;
            this.assembly = pair.Attributes["assembly"] == null ? "" : pair.Attributes["assembly"].InnerText;

            XmlNodeList mapp_nodes = pair.ChildNodes;

            foreach (XmlNode node in mapp_nodes)
            {
                try
                {
                    mappings.Add(node.Attributes["MTMParam"].InnerText, node.Attributes["CSParam"].InnerText);
                }
                catch (Exception e)
                {
                    throw new Exception(String.Format("Duplicate key found please check your methods XML definition. [{0}|{1}] \n {2}", node.Attributes["MTMParam"].InnerText, node.Attributes["CSParam"].InnerText, e.Message));
                }
            }
        }
    }
}
