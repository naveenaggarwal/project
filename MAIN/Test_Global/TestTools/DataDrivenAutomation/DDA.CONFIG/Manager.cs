using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MSCOM.DDA.CONFIG
{
    public class Manager
    {
        public static Settings settings;
        public static Dictionary<string, string> variables;
        public static Dictionary<string, Method> methods;

        static Manager()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\DDA.config");

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
                    catch (ArgumentException e)
                    {
                        throw new ArgumentException(String.Format("Duplicate key [{0}] found. Remove any duplicate 'pair' key entries under methods node at DDA.config and try again. Additional info:{1}", pair.Attributes["key"].InnerText, e.Message));
                    }
                }
            }
        }
    }
}
