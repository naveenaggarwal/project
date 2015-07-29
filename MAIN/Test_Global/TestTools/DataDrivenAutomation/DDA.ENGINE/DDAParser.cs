using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using MSCOM.Test.MTM;
using MSCOM.DDA.CONFIG;

namespace MSCOM.DDA.ENGINE
{
    public class DDAParser
    {
        #region OLDMETHOD
        //public static List<DDAStep> Matcher(MTMTestCase testCase)
        //{
        //    //var steps = StepsCommentsFilter(testCase.steps);

        //    List<DDAStep> matchedSteps = new List<DDAStep>();
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(Environment.CurrentDirectory + "\\..\\..\\MethodsDictionary.config");
        //    XmlNodeList nodes = xmlDoc.SelectNodes("/KeysAndMethods/Pair");

        //    Dictionary<string, string> dictionary = new Dictionary<string, string>();
        //    foreach (XmlNode node in nodes)
        //    {
        //        try
        //        {

        //            dictionary.Add(node.Attributes[0].InnerText, node.Attributes[1].InnerText);
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception("Duplicate key found please check your Dictionary.XML file\n" + e.Message);
        //        }
        //    }

        //    string[] methodsKeys = Manager.methods.Keys.ToArray();
        //    string[] dictionaryKeys = dictionary.Keys.ToArray<string>();
        //    Method outMethod;
        //    foreach (DDAStep step in steps)
        //    {
        //        if (step.action.Trim() != "")
        //        {
        //            if (DDAConfigMan.methods.TryGetValue(step.action, out outMethod))
        //            {
        //                step.actionMethodSignature = outMethod.methodSignature;
        //            }
        //            else if (DDAConfigMan.methods.TryGetValue(RemoveDigits(step.action), out outMethod))
        //            {
        //                step.actionMethodSignature = outMethod.methodSignature;
        //            }
        //            else
        //            {
        //                step.result.note = String.Format("Couldn't find matching C# method to the MTM Action: {0}. TC: {1} - Step: {2}", step.action, testCase.Id, step.position);
        //            }
        //        }
        //        else
        //        { 
        //            step.result.note = "Empty Action on MTM. Skipped."; 
        //        }

        //        //Replace C# method parameters with MTM Action parameter

        //        bool matched = false;
        //        int count = 0;

        //        while (!matched && count < methodsKeys.Count())
        //        {
        //            if (step.action.Equals(Convert.ToString(methodsKeys[count]))) //Looping through methods' keys to match the Step's Action
        //            {
        //                matched = true;
        //                step.actionMethodSignature = MAIAConfigurationManager.methods[step.action].methodSignature;
        //                //dictionaryKeys[count] = null;
        //            }
        //            else
        //            {
        //                if (RemoveDigits(step.action).Equals(Convert.ToString(methodsKeys[count])))
        //                {
        //                    matched = true;
        //                    step.actionMethodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);                            
        //                    List<string> originalParams = GetParamsFromString(step.action);
        //                    List<string> matchedParams = GetParamsFromString(step.actionMethodSignature);
        //                    if (originalParams.Count == matchedParams.Count)
        //                    {
        //                        for (int i = 0; i < step.action.Count(o => o.Equals('@')); i++)
        //                        {
        //                            step.actionMethodSignature = step.actionMethodSignature.Replace(matchedParams[i], originalParams[i]);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //logging Errors
        //                    }
        //                }
        //            }
        //            count++;
        //        }
        //        matched = false;
        //        count = 0;



        //        //while (!matched && count < dictionaryKeys.Count())
        //        //{
        //        //    if (step.action.Equals(Convert.ToString(dictionaryKeys[count])))//this one for the step
        //        //    {
        //        //        matched = true;
        //        //        step.actionMethodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
        //        //        //dictionaryKeys[count] = null;
        //        //    }
        //        //    else
        //        //    {
        //        //        if (RemoveDigits(step.action).Equals(Convert.ToString(dictionaryKeys[count])))
        //        //        {
        //        //            matched = true;
        //        //            step.actionMethodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
        //        //            List<string> originalParams = GetParamsFromString(step.action);
        //        //            List<string> matchedParams = GetParamsFromString(step.actionMethodSignature);
        //        //            if (originalParams.Count == matchedParams.Count)
        //        //            {
        //        //                for (int i = 0; i < step.action.Count(o => o.Equals('@')); i++)
        //        //                {
        //        //                    step.actionMethodSignature = step.actionMethodSignature.Replace(matchedParams[i], originalParams[i]);
        //        //                }
        //        //            }
        //        //            else
        //        //            {
        //        //               //logging Errors
        //        //            }
        //        //        }
        //        //    }
        //        //    count++;
        //        //}
        //        //matched = false;
        //        //count = 0;
        //        while (!matched && count < dictionaryKeys.Count()) //this one for the expected
        //        {
        //            if (step.expectedResult.Equals(Convert.ToString(dictionaryKeys[count])))
        //            {
        //                matched = true;
        //                step.expectedResultMethodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
        //                //dictionaryKeys[count] = null;
        //            }
        //            else
        //            {
        //                if (RemoveDigits(step.expectedResult).Equals(Convert.ToString(dictionaryKeys[count])))
        //                {
        //                    matched = true;
        //                    step.expectedResultMethodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
        //                    List<string> originalParams = GetParamsFromString(step.expectedResult);
        //                    List<string> matchedParams = GetParamsFromString(step.expectedResultMethodSignature);

        //                    if (originalParams.Count == matchedParams.Count)
        //                    {
        //                        for (int i = 0; i < step.expectedResult.Count(o => o.Equals('@')); i++)
        //                        {
        //                            step.expectedResultMethodSignature = step.expectedResultMethodSignature.Replace(matchedParams[i], originalParams[i]);
        //                        }
        //                    }
        //                    else
        //                    {
        //                       //logging Errors
        //                    }
        //                }
        //            }
        //            count++;
        //        }

        //        if (step.expectedResultMethodSignature == "" && step.expectedResult.Length > 0)
        //        {
        //            step.result.note = "Couldn't find matching C# method to the expected result:-> " + step.expectedResult;
        //        }
        //        if (step.actionMethodSignature == "" && step.action.Length > 0)
        //        {
        //            step.result.note = "Couldn't find matching C# method to the step:-> " + step.action;
        //        }
        //        matchedSteps.Add(step);
        //    }
        //    return matchedSteps;

        //}
        #endregion

        public static List<DDAStep> Matcher(DDAExecutionIteration executionIteration)
        {
            //var steps = StepsCommentsFilter(executionIteration.steps);

            List<DDAStep> matchedSteps = new List<DDAStep>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Environment.CurrentDirectory + "\\..\\..\\MethodsDictionary.config");
            XmlNodeList nodes = xmlDoc.SelectNodes("/KeysAndMethods/Pair");

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (XmlNode node in nodes)
            {
                try
                {
                    dictionary.Add(node.Attributes[0].InnerText, node.Attributes[1].InnerText);
                }
                catch (Exception e)
                {
                    throw new Exception("Duplicate key found please check your Dictionary.XML file\n" + e.Message);
                }
            }

            string[] methodsKeys = Manager.methods.Keys.ToArray();
            string[] dictionaryKeys = dictionary.Keys.ToArray<string>();
            Method outMethod;
            foreach (DDAStep step in executionIteration.steps)
            {
                if (step.action.MTMInnerText.Trim() != "")
                {
                    if (Manager.methods.TryGetValue(step.action.MTMInnerText.Trim(), out outMethod))
                    {
                        step.action.methodSignature = outMethod.methodSignature;
                    }
                    else if (Manager.methods.TryGetValue(RemoveDigits(step.action.MTMInnerText), out outMethod))
                    {
                        step.action.methodSignature = outMethod.methodSignature;
                    }
                    else
                    {
                        throw new DDAStepException(String.Format("Couldn't find matching C# method to the MTM Action: {0} - Step: {1}", step.action, step.position));
                    }
                }
                else
                {
                    executionIteration.resultNote = string.Format("Empty Action on MTM Step [{0}]. Skipped.", step.position);
                }

                //Replace C# method parameters with MTM Action parameter

                bool matched = false;
                int count = 0;

                while (!matched && count < methodsKeys.Count())
                {
                    if (step.action.Equals(Convert.ToString(methodsKeys[count]))) //Looping through methods' keys to match the Step's Action
                    {
                        matched = true;
                        step.action.methodSignature = Manager.methods[step.action.MTMInnerText].methodSignature;
                        //dictionaryKeys[count] = null;
                    }
                    else
                    {
                        if (RemoveDigits(step.action.MTMInnerText).Equals(Convert.ToString(methodsKeys[count])))
                        {
                            matched = true;
                            step.action.methodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
                            List<string> originalParams = GetParamsFromString(step.action.MTMInnerText);
                            List<string> matchedParams = GetParamsFromString(step.action.methodSignature);
                            if (originalParams.Count == matchedParams.Count)
                            {
                                for (int i = 0; i < step.action.MTMInnerText.Count(o => o.Equals('@')); i++)
                                {
                                    step.action.methodSignature = step.action.methodSignature.Replace(matchedParams[i], originalParams[i]);
                                }
                            }
                            else
                            {
                                //logging Errors
                            }
                        }
                    }
                    count++;
                }
                matched = false;
                count = 0;



                //while (!matched && count < dictionaryKeys.Count())
                //{
                //    if (step.action.Equals(Convert.ToString(dictionaryKeys[count])))//this one for the step
                //    {
                //        matched = true;
                //        step.actionMethodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
                //        //dictionaryKeys[count] = null;
                //    }
                //    else
                //    {
                //        if (RemoveDigits(step.action).Equals(Convert.ToString(dictionaryKeys[count])))
                //        {
                //            matched = true;
                //            step.actionMethodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
                //            List<string> originalParams = GetParamsFromString(step.action);
                //            List<string> matchedParams = GetParamsFromString(step.actionMethodSignature);
                //            if (originalParams.Count == matchedParams.Count)
                //            {
                //                for (int i = 0; i < step.action.Count(o => o.Equals('@')); i++)
                //                {
                //                    step.actionMethodSignature = step.actionMethodSignature.Replace(matchedParams[i], originalParams[i]);
                //                }
                //            }
                //            else
                //            {
                //               //logging Errors
                //            }
                //        }
                //    }
                //    count++;
                //}
                //matched = false;
                //count = 0;
                while (!matched && count < dictionaryKeys.Count()) //this one for the expected
                {
                    if (step.expectedResult.Equals(Convert.ToString(dictionaryKeys[count])))
                    {
                        matched = true;
                        step.expectedResult.methodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
                        //dictionaryKeys[count] = null;
                    }
                    else
                    {
                        if (RemoveDigits(step.expectedResult.MTMInnerText).Equals(Convert.ToString(dictionaryKeys[count])))
                        {
                            matched = true;
                            step.expectedResult.methodSignature = MethodParser(nodes[count].ChildNodes, dictionary[dictionaryKeys[count]]);
                            List<string> originalParams = GetParamsFromString(step.expectedResult.MTMInnerText);
                            List<string> matchedParams = GetParamsFromString(step.expectedResult.methodSignature);

                            if (originalParams.Count == matchedParams.Count)
                            {
                                for (int i = 0; i < step.expectedResult.MTMInnerText.Count(o => o.Equals('@')); i++)
                                {
                                    step.expectedResult.methodSignature = step.expectedResult.methodSignature.Replace(matchedParams[i], originalParams[i]);
                                }
                            }
                            else
                            {
                                //logging Errors
                            }
                        }
                    }
                    count++;
                }

                if (step.expectedResult.methodSignature == "" && step.expectedResult.MTMInnerText.Length > 0)
                {
                    executionIteration.resultNote += string.Format("\nCouldn't find matching C# method to the step:[{0}]", step.expectedResult);
                }
                if (step.action.methodSignature == "" && step.action.MTMInnerText.Length > 0)
                {
                    executionIteration.resultNote += string.Format("\nCouldn't find matching C# method to the step:[{0}]", step.action);
                }
                matchedSteps.Add(step);
            }

            return matchedSteps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static List<string> GetParamsFromString(string method)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < method.Length && method.IndexOf('@', i) != -1; i++)
            {
                i=method.IndexOf('@', i);                
                int j = i + 1;
                StringBuilder param = new StringBuilder();
                param.Append(method[i]);
                bool spaceFoundOrParentheses = false;
                while (j < method.Length && !spaceFoundOrParentheses)
                {
                    if (!method[j].Equals(' ') && !method[j].Equals(')') && !method[j].Equals(','))
                    {
                        param.Append(method[j]);
                        j++;
                    }
                    else
                    {
                        spaceFoundOrParentheses = true;
                    } 
                }

                i = j;
                list.Add(param.ToString());
            }

            return list;
        }


        //public static List<string> GetStepsFromString(string MTMExpression) 
        //{
        //    List<string> result = new List<string>();
        //    string[] fullList = Regex.Split(MTMExpression, "([$]STEP_[\\d]*_[$]ACTION_RESULT)|([$]STEP_[\\d]*_[$]EXPECTED_RESULT)");
        //    var first = true;
        //    foreach (string s in fullList)
        //    { 
            
            
        //    }           
            
        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveDigits(string key)
        {
            bool checkedTheWholeString = false;
            int currentIndex = 0;
            while (!checkedTheWholeString)
            {
                if (key.IndexOf("@", currentIndex) != -1)
                {
                    currentIndex = key.IndexOf("@", currentIndex);
                    if (key.IndexOf(" ", currentIndex) != -1)
                    {
                        key = key.Replace(key.Substring(currentIndex, key.IndexOf(" ", currentIndex) - currentIndex), Regex.Replace(key.Substring(currentIndex, key.IndexOf(" ", currentIndex) - currentIndex), @"\d", ""));
                        if (key.IndexOf("@", currentIndex + 1) != -1)
                        {
                            currentIndex = key.IndexOf("@", currentIndex + 1);
                        }
                        else
                        {
                            checkedTheWholeString = true;
                        }
                    }
                    else
                    {
                        key = key.Replace(key.Substring(currentIndex, key.Length - currentIndex), Regex.Replace(key.Substring(currentIndex, key.Length - currentIndex), @"\d", ""));
                        checkedTheWholeString = true;
                    }
                }
                else
                {
                    checkedTheWholeString = true;
                }
            }

            return key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childList"></param>
        /// <param name="methodName"></param>
        /// <param name="actualParamter"></param>
        /// <returns></returns>
        public static string MethodParser(XmlNodeList childList, string methodName, string actualParamter=null)
        {
            foreach (XmlNode node in childList)
            {
                string mTMParam = node.Attributes["MTMParam"].InnerText;
                string cSParam = node.Attributes["CSParam"].InnerText;

                if (methodName.IndexOf(cSParam) == -1)
                {
                    throw new Exception(string.Format("Couldn't find \"{0}\" in \"{1}\" please check your dictionary for this method", cSParam, methodName));
                }
                methodName = methodName.Replace(cSParam, mTMParam);
            }
            return methodName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="methodName"></param>
        /// <param name="actualParamter"></param>
        /// <returns></returns>
        public static string MethodParser(Method method, string methodName, string actualParamter = null)
        {
            foreach (string MTMParam in method.mappings.Keys)
            {
                string cSParam = "";
                if (method.mappings.TryGetValue(method.mappings[MTMParam], out cSParam))
                {
                    if (methodName.IndexOf(cSParam) == -1)
                    {
                        throw new DDATestCaseException(string.Format("Couldn't find \"{0}\" in \"{1}\" please check your dictionary for this method", cSParam, methodName));
                    }
                    methodName = methodName.Replace(cSParam, method.mappings[MTMParam]);
                   // throw new Exception(string.Format("Couldn't find \"{0}\" in \"{1}\" please check your dictionary for this method", cSParam, methodName));                
                }                
                
            }
            return methodName;
        }

        /// <summary>
        /// TO IMPLEMENT MTM Comments
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        private static List<DDAStep> StepsCommentsFilter(List<DDAStep> steps)
        {
            List<DDAStep> modifiedSteps = new List<DDAStep>();
            //DDAStep pivotStep = null;
            foreach (DDAStep sd in steps)
            {
                string action = sd.action.MTMInnerText;
                string expected = sd.expectedResult.MTMInnerText;

                if (action.Contains("[") && action.Contains("]"))
                {
                    if (!action.Substring(action.IndexOf("["), action.IndexOf("]") - action.IndexOf("[") + 1).Contains(" ")) //do not remove if there are two words or more
                    {
                        action = action.Remove(action.IndexOf("["), action.IndexOf("]") - action.IndexOf("[") + 1);
                    }
                }
                while (action.EndsWith(" ") || action.EndsWith("."))
                {
                    action = action.Remove(action.Length - 1, 1);
                }

                if (expected.Contains("[") && expected.Contains("]"))
                {
                    if (!expected.Substring(expected.IndexOf("["), expected.IndexOf("]") - expected.IndexOf("[") + 1).Contains(" ")) //do not remove if there are two words or more
                    {
                        expected = expected.Remove(expected.IndexOf("["), expected.IndexOf("]") - expected.IndexOf("[") + 1);
                    }
                }
                while (expected.EndsWith(" ") || expected.EndsWith("."))
                {
                    expected = expected.Remove(expected.Length - 1, 1);
                }

                //modifiedSteps.Add(new DDAStep(action, expected, sd));
            }

            return modifiedSteps;
        }

        //This method extracts the list of parameter name when a methodname is passed
        public static Object[] GetParameterList(string methodName)
        {
            List<object> parameters = new List<object>();
            int startIndex = methodName.IndexOf('@') + 1;
            StringBuilder parameterName = new StringBuilder();

            for (int i = startIndex; i <= methodName.IndexOf(')'); i++)
            {
                if (methodName[i] != ',' && methodName[i] != ')')
                    parameterName.Append(methodName[i]);

                else if (methodName[i] == ',')
                {
                    parameters.Add(parameterName.ToString());
                    parameterName.Clear();

                    methodName = methodName.Substring(i + 1);
                    i = methodName.IndexOf('@');
                }

                else if (methodName[i] == ')')
                {
                    parameters.Add(parameterName.ToString());
                    parameterName.Clear();
                }
            }
            return parameters.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="iteration"></param>
        /// <returns></returns>
        public static List<Object> GetParametersValues(string methodInvokeSignature, Dictionary<string, string> parameters, Dictionary<string, object> outcomes, int currentStepPosition, int latestActionStepPosition)
        {
            List<Object> result = new List<object>();

            var parametersSignatures = methodInvokeSignature.Substring(methodInvokeSignature.IndexOf('(') + 1).Replace(')', ' ').Split(',');

            string key;
            Object outObject;
            string outString;
            string requestedStep;
            //string stepDependencyRx = "([$]STEP_[\\d]*_[$]ACTION_RESULT)|([$]STEP_[\\d]*_[$]EXPECTATION_RESULT)";
            //string stepDependencyDDARx = "([$]STEP_[A-Z]+_[$]ACTION_RESULT)|([$]STEP_[A-Z]+_[$]EXPECTED_RESULT)";
            //string[] innerXMLMatches;
            //string[] keyXMLMatches;

            //Regex stepResultRegEx = new Regex(stepDependencyRx);

            foreach (string param in parametersSignatures)
            {
                if (param.Trim() == "")
                {
                    continue;
                }
                requestedStep = "";
                key = param.Trim().Split(' ').Last().Trim();

                if (parameters.TryGetValue(key.Substring(key.IndexOf('@') + 1), out outString))
                {
                    if (outString == "$ACTION_RESULT" || outString == "$EXPECTATION_RESULT")
                    {
                        if (latestActionStepPosition < 1) //This can be done only if at least one has been completed already.
                        {
                            throw new Exception(String.Format("DDA.Engine was unable to read a previous step outcome at step '{0}' since no such step has been performed yet.", currentStepPosition));
                        }

                        if (outString == "$ACTION_RESULT" && outcomes.TryGetValue("$ACTION_RESULT_" + latestActionStepPosition, out outObject))
                        {
                            result.Add(outObject);
                            System.Console.Out.Write(string.Format("Note: At step '{0}', '{1}' (from latest executed 'Action' result) was identified as value for parameter '{2}'", 
                                currentStepPosition, outObject == null ? "null" : outObject.ToString(), outString));
                        }
                        else if (outcomes.TryGetValue("$EXPECTATION_RESULT_" + latestActionStepPosition, out outObject))
                        {
                            result.Add(outObject);
                            System.Console.Out.Write(string.Format("Note: At step '{0}', '{1}' (from latest executed 'Expected Result' result) was identified as value for parameter '{2}'", 
                                currentStepPosition, outObject == null ? "null" : outObject.ToString(), outString));
                        }
                    }
                    else if (outString.Contains("$STEP_") && (outString.Contains("_ACTION_RESULT") || outString.Contains("_EXPECTATION_RESULT")))
                    {
                        if (latestActionStepPosition < 1) //This can be done only if at least one has been completed already.
                        {
                            throw new DDATestCaseException(String.Format("DDA.Engine was unable to read a previous step outcome at step '{0}' since no such step has been performed yet.", currentStepPosition));
                        }

                        requestedStep = outString.Replace("$STEP_", "").Replace("_ACTION_RESULT", "").Replace("_EXPECTATION_RESULT", "");

                        if (outcomes.TryGetValue(string.Format("${0}_{1}", outString.Replace(string.Format("$STEP_{0}_", requestedStep), ""),requestedStep), out outObject))
                        {
                            result.Add(outObject);
                            System.Console.Out.Write(string.Format("Note: At step '{0}', '{1}' (from step {3} {4} result) was identified as value for parameter '{2}'", 
                                currentStepPosition, outObject == null ? "null" : outObject.ToString(), outString, requestedStep, outString.Contains("EXPECTATION") ? "Expected Result's" : "Action's"));
                        }
                        else
                        {
                            throw new DDATestCaseException(string.Format("DDA.Engine was unable to parse a given parameter. At step '{0}' no outcome from any previous step could be found using key '{1}'.", currentStepPosition, string.Format("${0}_{1}", outString.Replace(string.Format("$STEP_{0}_", requestedStep), ""), requestedStep)));
                        }
                    }
                    else
                    {
                        result.Add(outString);
                        System.Console.Out.Write(string.Format("Note: At step '{0}', '{1}' was identified as value for parameter '{2}'", currentStepPosition, outString, key));
                    }
                }                
                else if (key == "$ACTION_RESULT" || key == "$EXPECTATION_RESULT")
                {
                    if (latestActionStepPosition < 1) //This can be done only if at least one has been completed already.
                    {
                        throw new Exception(String.Format("DDA.Engine was unable to read a previous step outcome at step '{0}' since no such step has been performed yet.", currentStepPosition));
                    }

                    if (key == "$ACTION_RESULT" && outcomes.TryGetValue("$ACTION_RESULT_" + latestActionStepPosition, out outObject))
                    {
                        result.Add(outObject);
                        System.Console.Out.Write(string.Format("Note: At step '{0}', '{1}' (from latest executed 'Action' result) was identified as value for parameter '{2}'", 
                            currentStepPosition, outObject == null ? "null" : outObject.ToString(), key));
                    }
                    else if (outcomes.TryGetValue("$EXPECTATION_RESULT_" + latestActionStepPosition, out outObject))
                    {
                        result.Add(outObject);
                        System.Console.Out.Write(string.Format("Note: At step '{0}', '{1}' (from latest executed 'Expected Result' result) was identified as value for parameter '{2}'", 
                            currentStepPosition, outObject == null ? "null" : outObject.ToString(), key));
                    }
                }                
                else
                {
                    throw new DDATestCaseException(string.Format("DDA.Engine was unable to parse a given parameter. At step '{0}' no value could be found for parameter '{1}'. " + 
                        "Make sure mapping is correct and parameters exist in MTM.", currentStepPosition, param));
                }
                System.Console.Out.Write("\n");
            }
            return result;
        }

        public static string GetParameterName(string expectedResult)
        {
            expectedResult = expectedResult.Substring(expectedResult.IndexOf('@'));
            StringBuilder paramName = new StringBuilder();

            for (int i = 0; i < expectedResult.Length; i++)
            {
                if (expectedResult[i] != ' ')
                {
                    paramName.Append(expectedResult[i]);
                }
                else
                    break;
            }
            return paramName.ToString();
        }

        /// <summary>
        /// For a method signature, as a string, this method will return the method name
        /// </summary>
        /// <param name="methodSignature">eg. Method(type1 param1, type2 param2)</param>
        /// <returns>"Method"</returns>
        public static string GetMethodName(String methodSignature)
        {
            if (!string.IsNullOrEmpty(methodSignature))
                return methodSignature.Substring(0, methodSignature.IndexOf("("));
            else
                return methodSignature;
        }

        /// <summary>
        /// For a method signature, as a string, this method will return a List of KeyValuePair representing method's signature parameter in the form of [type,name]
        /// </summary>
        /// <param name="methodSignature">eg. Method(type1 param1, type2 param2)</param>
        /// <returns>eg. [type1,param1];[type2,param2]</returns>
        public static List<KeyValuePair<string, string>> GetParameters(string methodSignature)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            string[] parameters = methodSignature.Substring(methodSignature.IndexOf("(")).Replace('(', ' ').Replace(')', ' ').Split(',');
            foreach (string parameter in parameters)
            {
                result.Add(new KeyValuePair<string, string>(parameter.Trim().Split(' ')[0], parameter.Trim().Split(' ')[1]));
            }
            return result;
        }

        /// <summary>
        /// For a method signature, as a string, this method will replace the provided parameterName 
        /// with a provided newParameterName
        /// </summary>
        /// <param name="methodSignature">eg. Method(type1 oldParamName, type2 param2)</param>
        /// <param name="oldParameterName">eg. oldParamName</param>
        /// <param name="newParameterName">eg. newParamName</param>
        /// <returns>eg. "Method(type1 newParamName, type2 param2)"</returns>
        public static string ReplaceParameter(string methodSignature, string oldParameterName, string newParameterName)
        {
            string parameters = "";
            bool hasParameters = false;
            foreach(KeyValuePair<string, string> param in GetParameters(methodSignature))
            {
                hasParameters = true;
                if (param.Value.Trim() == oldParameterName.Trim())
                {
                    parameters += param.Key.Trim() + " " + newParameterName.Trim() + ", ";
                }
                else
                {
                    parameters += param.Key.Trim() + " " + param.Value + ", ";
                }
            }

            if (!hasParameters)
            {
                return methodSignature;
            }

            return string.Format("{0}({1})", GetMethodName(methodSignature), parameters.Substring(0, parameters.Length - 2));
        }
    }
}
