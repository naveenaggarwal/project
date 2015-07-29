using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MSCOM.DDA.CONFIG
{
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
}
