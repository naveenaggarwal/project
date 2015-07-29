using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using MSCOM.Test.MTM;

namespace MSCOM.DDA.ENGINE
{
    /// <summary>
    /// This class will change a lot in the future. Don't worry too much about it for now
    /// </summary>
    public static class DDAInstrumentation
    {
        private static string filePath = (Environment.CurrentDirectory + DateTime.Now.ToString("MMM ddd d")).Replace("Debug", " ");
        private static string loggingOption = null;

        internal static void WriteStart() { }
        internal static void WriteEnd() { }

        internal static void WriteInProgress(MTMMethodInfo methodInfo, ActionType actionType, bool isNewTC = false)
        {
            StringBuilder actionName = new StringBuilder();

            if (!string.IsNullOrEmpty(Convert.ToString(APP.Default.loggingOption)))
            {
                loggingOption = Convert.ToString(APP.Default.loggingOption);
            }

            if (loggingOption == "Box")
            {
                if (isNewTC)
                {
                    MTMMethodInfo.LoggerText.Add(" ");
                    MTMMethodInfo.LoggerText.Add(string.Format("TestId - {0}", methodInfo.TestId.ToString()));
                    MTMMethodInfo.LoggerText.Add(" ");

                }
                if (actionType == ActionType.action)
                {
                    MTMMethodInfo.LoggerText.Add(string.Format("  Step {0}:", (methodInfo.StepNo + 1).ToString()));
                    actionName.Append(methodInfo.MethodName(actionType));
                    actionName = GetFormatedActionName(actionName, methodInfo);
                    MTMMethodInfo.LoggerText.Add(string.Format("      Action: {0}", actionName.ToString()));
                    MTMMethodInfo.LoggerText.Add(string.Format("        Outcome of the above Action: {0}", methodInfo.ActionOutcome.ToString()));
                }
                else if (actionType == ActionType.expectedResult)
                {
                    MTMMethodInfo.LoggerText.Add(string.Format("  Step {0}:", (methodInfo.StepNo + 1).ToString()));
                    actionName.Append(methodInfo.MethodName(actionType));
                    actionName = GetFormatedActionName(actionName, methodInfo);
                    MTMMethodInfo.LoggerText.Add(string.Format("      ExpectedResultMethod: {0}", actionName.ToString()));
                    MTMMethodInfo.LoggerText.Add(string.Format("      Outcome of the above Action: {0}", methodInfo.ExpectedResultOutcome.ToString()));
                }
                else if (actionType == ActionType.failure)
                {
                    MTMMethodInfo.LoggerText.Add(string.Format("  Step {0}:", (methodInfo.StepNo + 1).ToString()));
                    MTMMethodInfo.LoggerText.Add(string.Format("      Result of the above Action: {0}", methodInfo.ExpectedResultOutcome.ToString()));
                    MTMMethodInfo.LoggerText.Add(string.Format("      Step Failed.   Reason: {0}", methodInfo.ErrorMessage));
                }
                else
                {
                    MTMMethodInfo.LoggerText.Add(" ");
                    MTMMethodInfo.LoggerText.Add(string.Format("Final Test Result = {0}", Boolean.Parse(methodInfo.FinalTestResult.ToString()) ? "PASSED" : "FAILED"));
                }
                MTMMethodInfo.LoggerText.Add("************************************************");
            }
            else
            {
                if (!string.IsNullOrEmpty(Convert.ToString(APP.Default.logFilePath)))
                {
                    filePath = Convert.ToString(APP.Default.logFilePath);
                }
                using (TextWriter tw = new StreamWriter(filePath, true))
                {
                    if (isNewTC)
                    {
                        tw.WriteLine();
                        tw.WriteLine("TestId:{0}", methodInfo.TestId);
                    }

                    if (actionType == ActionType.action)
                    {
                        tw.WriteLine();
                        tw.WriteLine("Step:{0}:", methodInfo.StepNo + 1);
                        actionName.Append(methodInfo.MethodName(actionType));
                        actionName = GetFormatedActionName(actionName, methodInfo);
                        tw.WriteLine("Action:{0}", actionName);
                        tw.WriteLine("Result of the above Action: {0}", methodInfo.ActionOutcome.ToString());
                    }
                    else if (actionType == ActionType.expectedResult)
                    {
                        tw.WriteLine();
                        tw.WriteLine("Step:{0}:", methodInfo.StepNo + 1);
                        actionName.Append(methodInfo.MethodName(actionType));
                        actionName = GetFormatedActionName(actionName, methodInfo);
                        tw.WriteLine("ExpectedResultMethod:{0}", actionName);
                        tw.WriteLine("Result of the above Action: {0}", methodInfo.ExpectedResultOutcome.ToString());
                    }
                    else if (actionType == ActionType.failure)
                    {
                        tw.WriteLine(string.Format("Step:{0}:", (methodInfo.StepNo + 1).ToString()));
                        tw.WriteLine(string.Format("Step Failed because: {0}", methodInfo.ErrorMessage));
                    }
                    else
                    {
                        tw.WriteLine("Final Test Result= {0}", methodInfo.FinalTestResult);
                    }

                    tw.WriteLine("************************************************");
                }
            }
        }

        internal static void WriteInProgress(MTMTestCase t, DDAStep step, ActionType actionType, List<Object> parameters, int iteration, bool isNewTC = false, bool passed = true)
        {
            StringBuilder actionName = new StringBuilder();
            string invokeString = "(\"";

            if (!string.IsNullOrEmpty(Convert.ToString(APP.Default.loggingOption)))
            {
                loggingOption = Convert.ToString(APP.Default.loggingOption);
            }

            if (loggingOption == "Box")
            {
                if (isNewTC)
                {
                    if (iteration > 1)
                    {
                        MTMMethodInfo.LoggerText.Add(" ");
                    }

                    MTMMethodInfo.LoggerText.Add(string.Format("TestId - {0} | Iteration - {1}", t.Id.ToString(), iteration));
                    MTMMethodInfo.LoggerText.Add(" ");

                }
                if (actionType == ActionType.action)
                {
                    MTMMethodInfo.LoggerText.Add(string.Format("  Step {0} - Action:", step.position.ToString()));
                    MTMMethodInfo.LoggerText.Add(string.Format("    MTM Expression: {0}", step.action));
                    MTMMethodInfo.LoggerText.Add(string.Format("    Mapped Key: {0}", step.action.mappedKey));
                    MTMMethodInfo.LoggerText.Add(string.Format("    Method Signature: {0}", step.action.mappedMethodSignature));
                    MTMMethodInfo.LoggerText.Add(string.Format("    MTM Parameters:"));

                    foreach (string s in step.action.parametersMTMValues.Keys)
                    {
                        MTMMethodInfo.LoggerText.Add(string.Format("         @{0} = \"{1}\"", s, step.action.parametersMTMValues[s]));
                    }

                    foreach (Object o in parameters)
                    {
                        invokeString += o.ToString() + "\", \"";
                    }
                    invokeString = invokeString.LastIndexOf(',') > 0 ? invokeString.Substring(0, invokeString.LastIndexOf(',')) + ");" : ") [Error: No parameters were found.]";

                    MTMMethodInfo.LoggerText.Add(string.Format("    Invoked Method: {0}.{1}{2}", step.action.reference, step.action.methodSignature.Substring(0, step.action.methodSignature.IndexOf('(')), invokeString));
                    MTMMethodInfo.LoggerText.Add(string.Format("    Outcome of the above Action: \"{0}\"", step.action.result.Outcome));
                }
                else if (actionType == ActionType.expectedResult)
                {
                    MTMMethodInfo.LoggerText.Add(string.Format("  Step {0} - Expected Result:", step.position.ToString()));
                    MTMMethodInfo.LoggerText.Add(string.Format("    MTM Expression: {0}", step.expectedResult.MTMInnerText));
                    MTMMethodInfo.LoggerText.Add(string.Format("    Mapped Key: {0}", step.expectedResult.mappedKey));
                    MTMMethodInfo.LoggerText.Add(string.Format("    Method Signature: {0}", step.expectedResult.mappedMethodSignature));
                    MTMMethodInfo.LoggerText.Add(string.Format("    MTM Parameters:"));

                    foreach (string s in step.expectedResult.parametersMTMValues.Keys)
                    {
                        MTMMethodInfo.LoggerText.Add(string.Format("         @{0} = \"{1}\"", s, step.expectedResult.parametersMTMValues[s]));
                    }
                    foreach (Object o in parameters)
                    {
                        invokeString += o.ToString() + "\", \"";
                    }
                    invokeString = invokeString.LastIndexOf(',') > 0 ? invokeString.Substring(0, invokeString.LastIndexOf(',')) + ");" : "[Error: No parameters were found.]";

                    MTMMethodInfo.LoggerText.Add(string.Format("    Invoked Method: {0}.{1}{2}", step.expectedResult.reference, step.expectedResult.methodSignature.Substring(0, step.expectedResult.methodSignature.IndexOf('(')), invokeString));
                    MTMMethodInfo.LoggerText.Add(string.Format("    Outcome of the above Action: \"{0}\"", step.expectedResult.result.Outcome));
                }
                else if (actionType == ActionType.failure)
                {
                    MTMMethodInfo.LoggerText.Add(string.Format("  Step {0}:", step.position.ToString()));
                    //    MTMMethodInfo.LoggerText.Add(string.Format("      Result of the above Action: {0}", methodInfo.ExpectedResultOutcome.ToString()));
                    //    MTMMethodInfo.LoggerText.Add(string.Format("      Step Failed.   Reason: {0}", methodInfo.ErrorMessage));
                }
                else
                {
                    MTMMethodInfo.LoggerText.Add(" ");
                    MTMMethodInfo.LoggerText.Add(string.Format("Final Test Result = {0}", passed ? "PASSED" : "FAILED"));
                }
                MTMMethodInfo.LoggerText.Add("************************************************");
            }
            //else
            //{
            //    if (!string.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["logFilePath"])))
            //    {
            //        filePath = Convert.ToString(ConfigurationManager.AppSettings["logFilePath"]);
            //    }
            //    using (TextWriter tw = new StreamWriter(filePath, true))
            //    {
            //        if (isNewTC)
            //        {
            //            tw.WriteLine();
            //            tw.WriteLine("TestId:{0}", methodInfo.TestId);
            //        }

            //        if (actionType == ActionType.action)
            //        {
            //            tw.WriteLine();
            //            tw.WriteLine("Step:{0}:", methodInfo.StepNo + 1);
            //            actionName.Append(methodInfo.MethodName(actionType));
            //            actionName = GetFormatedActionName(actionName, methodInfo);
            //            tw.WriteLine("Action:{0}", actionName);
            //            tw.WriteLine("Result of the above Action: {0}", methodInfo.ActionOutcome.ToString());
            //        }
            //        else if (actionType == ActionType.expectedResult)
            //        {
            //            tw.WriteLine();
            //            tw.WriteLine("Step:{0}:", methodInfo.StepNo + 1);
            //            actionName.Append(methodInfo.MethodName(actionType));
            //            actionName = GetFormatedActionName(actionName, methodInfo);
            //            tw.WriteLine("ExpectedResultMethod:{0}", actionName);
            //            tw.WriteLine("Result of the above Action: {0}", methodInfo.ExpectedResultOutcome.ToString());
            //        }
            //        else if (actionType == ActionType.failure)
            //        {
            //            tw.WriteLine(string.Format("Step:{0}:", (methodInfo.StepNo + 1).ToString()));
            //            tw.WriteLine(string.Format("Step Failed because: {0}", methodInfo.ErrorMessage));
            //        }
            //        else
            //        {
            //            tw.WriteLine("Final Test Result= {0}", methodInfo.FinalTestResult);
            //        }

            //        tw.WriteLine("************************************************");
            //    }
            //}
        }

        internal static string GetCurrentState(DDAPair pair, List<object> parametersList, int iteration, int stepNo, Exception e)
        {
            string invokeString = "(\"";

            List<string> error = new List<string>();

            error.Add(string.Format("At Iteration: {0}, Step: {1} the following exception occured: {2}", iteration, stepNo, e.InnerException));
            error.Add(string.Format("MTM Expression: {0}", pair.MTMInnerText));
            error.Add(string.Format("Mapped Key: {0}", pair.mappedKey));
            error.Add(string.Format("Method Signature: {0}", pair.mappedMethodSignature));
            error.Add(string.Format("MTM Parameters:"));

            foreach (string s in pair.parametersMTMValues.Keys)
            {
                error.Add(string.Format("[@{0} = \"{1}\"]", s, pair.parametersMTMValues[s]));
            }
            foreach (Object o in parametersList)
            {
                invokeString += o.ToString() + "\", \"";
            }
            invokeString = invokeString.LastIndexOf(',') > 0 ? invokeString.Substring(0, invokeString.LastIndexOf(',')) + ");" : "[Error: No parameters were found.]";

            error.Add(string.Format("Invoked Method: {0}.{1}{2}", pair.reference, pair.methodSignature.Substring(0, pair.methodSignature.IndexOf('(')), invokeString));

            return string.Join("----->", error.ToArray());

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        private static StringBuilder GetFormatedActionName(StringBuilder actionName, MTMMethodInfo methodInfo)
        {
            if (methodInfo.Parameters.Count > 0)
                actionName.Append('(');

            foreach (object param in methodInfo.Parameters)
            {
                actionName.Append(param.ToString() + ",");
            }
            actionName.Replace(",", "", actionName.Length - 1, 1);
            actionName.Append(')');
            return actionName;
        }

    }
}