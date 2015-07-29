using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MSCOM.Test.MTM;
using MSCOM.Test.TFS;

namespace MSCOM.DDA
{

    public static class DDAInvoker
    {
        //This method is used for invoking the methods dynamically.
        //This mthod get the method and its parameter info and typecasts the input paramters and then invokes the appropriate method and the returns the result.
        public static object InvokeMethod(string methodName, string classReference, string assemblyName, params object[] parameterValues)
        {            
            Type type = null;

            try
            {
                type = System.Reflection.Assembly.LoadFile(System.IO.Directory.GetCurrentDirectory() + "\\" + assemblyName).GetType(classReference);
                if (type == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                throw new DDATestCaseException(string.Format("DDA.Engine encountered a problem loading Assembly: [{0}] (with class reference: [{1}]) from DDA.Engine.DDAInvoker.\n Additional details: {2}", assemblyName, classReference, e.Message));
            }
            
            Object result = null;
            int i = 0;
            string parametersSignatureLog = "";

            if (type != null)
            {
                MethodInfo methodInfo = null;
                try
                {
                    methodInfo = type.GetMethod(methodName);
                }
                catch (AmbiguousMatchException)
                {
                    throw new DDATestCaseException(string.Format("DDA.Engine could not retrieve method '{0}' from Assembly: [{1}] (with class reference: [{2}]). Avoid using same name for multiple methods within th same assembly.\n", methodName, assemblyName, classReference));
                }

                if (methodInfo != null)
                {
                    ParameterInfo[] parametersInfo = methodInfo.GetParameters();
                    object classInstance = Activator.CreateInstance(type, null);

                    foreach (ParameterInfo paramInfo in parametersInfo)
                    {
                        if (parameterValues[i] == null)
                        {
                            parameterValues[i] = null;
                            i++;
                            continue;
                        }

                        if (paramInfo.ParameterType != parameterValues[i].GetType())
                        {
                            try
                            {
                                parameterValues[i] = Convert.ChangeType(parameterValues[i], Type.GetType(paramInfo.ParameterType.FullName));
                            }
                            catch (FormatException e)
                            {
                                throw new DDAIterationException(string.Format("DDA.Engine was unable to change type of '{0}' as '{1}' while attempting to invoke '{2}.{3}' from {4}.\n Additional details: {5}",
                                    parameterValues[i], paramInfo.ParameterType.FullName, classReference, methodName, assemblyName, e.Message));
                            }                            
                        }
                        i++;
                    }

                    if (parametersInfo.Length == 0)
                    {
                        try
                        {
                            result = methodInfo.Invoke(classInstance, null);
                        }
                        finally
                        {
                            System.Console.Out.Write(string.Format("{0}.{1}(); Result: '{2}'\n", classReference, methodInfo.Name, result.ToString()));
                        }
                    }
                    else
                    {
                        parametersSignatureLog = "";
                        foreach (object o in parameterValues)
                        {
                            parametersSignatureLog += "," + (o == null? "null" : o.ToString());
                        }
                        parametersSignatureLog = parametersSignatureLog.Substring(1, parametersSignatureLog.Length - 1);

                        try
                        {
                            result = methodInfo.Invoke(classInstance, parameterValues);
                            System.Console.Out.Write(string.Format("{0}.{1}({3}); Result: '{2}'\n", classReference, methodInfo.Name, (result == null ? "NULL" : result.ToString()), parametersSignatureLog));
                        }
                        catch (TargetInvocationException e)
                        {
                            System.Console.Out.Write(string.Format("{0}.{1}({3}); Result: '{2}'\n", classReference, methodInfo.Name, (result == null ? "Exception|Failed" : result.ToString()), parametersSignatureLog));
                            if (e.InnerException.GetBaseException().GetType().Name == "DDAStepException")
                            {
                                throw new DDAStepException(e.Message);
                            }
                            else if (e.InnerException.GetBaseException().GetType().Name == "DDAIterationException")
                            {
                                throw new DDAIterationException(e.Message);
                            }
                            else
                            {
                                throw new DDATestCaseException(e.Message);
                            }
                        }                       
                    }
                }
                else
                {
                    throw new DDAStepException(string.Format("There was a problem getting info for Method [{0}] at Assembly: [{1}] " + 
                        "(with class reference: [{2}]) from DDA.Engine.DDAInvoker. Make sure such method is 'public' and that DDA.config " + 
                        "entry is correct.", methodName, assemblyName, classReference));
                }
            }

            return result;
        }
    }
}