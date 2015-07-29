using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MSCOM.DDA.CONFIG
{
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
                mappings.Add(node.Attributes["MTMParam"].InnerText, node.Attributes["CSParam"].InnerText);                
            }
        }
    }
}
