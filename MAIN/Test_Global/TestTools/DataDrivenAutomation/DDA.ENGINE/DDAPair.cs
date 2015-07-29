using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MSCOM.DDA.ENGINE
{
    public class DDAPair
    {
        public string MTMInnerText;

        public string mappedKey;
        public string mappedMethodSignature;
        public Dictionary<string, string> parametersMapping; //DDA.config Mapping
        public Dictionary<string, string> parametersMTMValues; //MTM parameters and values

        public string methodSignature;
        public string reference;
        public string assembly;

        public string invokeMethodName;

        public DDAResult result;

        public DDAPair(string innerText, Dictionary<string, KeyValuePair<string, string>> parametersMTMValues)
        {
            if (innerText.Trim().Equals(String.Empty))
            {
                return;
            }

            MSCOM.DDA.CONFIG.Method outMethod;
            
            if (MSCOM.DDA.CONFIG.Manager.methods.TryGetValue(DDAParser.RemoveDigits(innerText.Trim()), out outMethod)) //Looks for match of the innerText (what the MTM step says) while removing any digits from parameters used in MTM (eg. @x7 = @x) and any entry in DDA.config
            {
                mappedKey = outMethod.pairKey;
            }
            
            else
            {
                throw new DDATestCaseException(string.Format("DDA.Engine couldn't find a matching method at DDA.config for MTM Expression: {0}", innerText));
            }

            this.MTMInnerText = innerText;
            //Mapped DDA.config Data
            this.mappedMethodSignature = MSCOM.DDA.CONFIG.Manager.methods[mappedKey].methodSignature;
            this.methodSignature = this.mappedMethodSignature;
            this.parametersMapping = MSCOM.DDA.CONFIG.Manager.methods[mappedKey].mappings;
            this.reference = MSCOM.DDA.CONFIG.Manager.methods[mappedKey].reference;
            this.assembly = MSCOM.DDA.CONFIG.Manager.methods[mappedKey].assembly;            
            
            KeyValuePair<string,string> outParam;
            string temp;
            bool innerExpressionHadMTMParams = false;

            this.parametersMTMValues = new Dictionary<string, string>();

            foreach (string mtmParam in DDAParser.GetParamsFromString(innerText)) //For each parameter in MTM Step innerText 
            {
                //Adding items to this.parametersMTMValues through the mapping of parametersMTMValues
                if (parametersMTMValues.TryGetValue(mtmParam.Substring(mtmParam.IndexOf('@') + 1), out outParam))
                {
                    try
                    {
                        //Attempting to get the value for current parameter from MTM Data Iteration (parametersMTMValues).
                        this.parametersMTMValues.Add(mtmParam.Substring(mtmParam.IndexOf('@') + 1), outParam.Value); 
                    }
                    catch (ArgumentException e)
                    {
                        //If the item was already ther, continue
                        System.Console.Out.Write(string.Format("Warning: DDA.Engine was unable to add '{0},{1}' " +
                        "while iterating through step: '{2}' parameters.\n Additional info: {3}\n", 
                        mtmParam.Substring(mtmParam.IndexOf('@') + 1), outParam.Value, innerText, e.Message));
                        continue;                        
                    }
                }
                else
                {
                    throw new Exception(string.Format("DDA.Engine was unable to find MTM Parameter '{0}' and/or its corresponding value from WorkItem 'Parameter Data' as referenced" +
                    "in Steps. Verify that neither Steps nor Iterations' Parameters are corrupted", mtmParam));
                }

                foreach (string key in parametersMapping.Keys)
                {
                    temp = DDAParser.RemoveDigits(mtmParam);
                    if (key.Substring(key.IndexOf('@') + 1) == temp.Substring(temp.IndexOf('@') + 1))
                    {
                        this.methodSignature = DDAParser.ReplaceParameter(this.methodSignature, parametersMapping[key], mtmParam);
                    }
                    else if (key == "$ACTION_RESULT")//If special expression is used in DDA.config, use it as value. It's final value will be replace before Invoke
                    {
                        this.methodSignature = DDAParser.ReplaceParameter(this.methodSignature, parametersMapping[key], key); 
                    }
                }

                innerExpressionHadMTMParams = true;
            }

            if (!innerExpressionHadMTMParams)//If no MTM Parameters were used in Step's innerText, loop through all mapping keys again to replace any special expression that could be used
            {
                foreach (string key in parametersMapping.Keys)
                {
                    if (key == "$ACTION_RESULT")//If special expression is used in DDA.config, use it as value. It's final value will be replace before Invoke
                    {
                        this.methodSignature = DDAParser.ReplaceParameter(this.methodSignature, parametersMapping[key], key); 
                    }
                }
            }

            this.invokeMethodName = DDAParser.GetMethodName(mappedMethodSignature);
            this.result = new DDAResult();
        }
    }
}
