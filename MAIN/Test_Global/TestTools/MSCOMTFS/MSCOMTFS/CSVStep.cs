using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace MSCOM.Test.CSV
{
    public class CSVStep
    {
        public int Position { get; set; }
        public string Action { get; set; }
        public string ExpectedResult { get; set; }

        public CSVStep previous;



        /// <summary>
        /// Generating the dictionary object with the steps xml data
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        private static List<CSVStep> GenerateStepsDictionary(string xmlData)
        {
            //If steps xml data is empty
            if (xmlData == null || xmlData == "")
            {
                return new List<CSVStep>();
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            Hashtable stepsTable = new Hashtable();
            List<CSVStep> stepsAndResults1 = new List<CSVStep>();

            string nodepath = "/steps/step";
            XmlNodeList nodes = xmlDoc.SelectNodes(nodepath);

            try
            {
                foreach (XmlNode parentNode in nodes)
                {
                    string actionValue = string.Empty;
                    string expectedResultValue = string.Empty;

                    XmlNodeList topnodes = parentNode.SelectNodes("parameterizedString");

                    if (topnodes.Count != 2)
                    {
                        throw new Exception("XML is invalid");
                    }

                    const string parameterNodeName = "parameter";
                    foreach (XmlNode x in topnodes[0])
                    {
                        if (x.Name == parameterNodeName && x.InnerText.Length > 0)
                        {
                            actionValue += "@" + x.InnerText;
                        }
                        else if (x.Name != parameterNodeName && x.InnerText.Length > 0)
                        {
                            actionValue += x.InnerText;
                        }
                    }

                    foreach (XmlNode x in topnodes[1])
                    {
                        if (x.Name == parameterNodeName && x.InnerText.Length > 0)
                        {
                            expectedResultValue += "@" + x.InnerText;
                        }
                        else if (x.Name != parameterNodeName && x.InnerText.Length > 0)
                        {
                            expectedResultValue += x.InnerText;
                        }
                    }
                    CSVStep data1 = new CSVStep();
                    data1.Action = actionValue;
                    data1.ExpectedResult = expectedResultValue;
                    stepsAndResults1.Add(data1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return stepsAndResults1;
        }

    }
}


