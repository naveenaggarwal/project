using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCOM.DDA.ENGINE
{
    public static class DDAStaticExecute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        public static bool ExecuteMTMTestCase(MSCOM.Test.MTM.MTMTestCase tc)
        {
            DDAExecutionIteration currentExecutionIteration = null;

            string error = null;
            int iteration = 0;

            foreach (MSCOM.Test.MTM.MTMDataIteration i in tc.DataIterations)
            {
                currentExecutionIteration = new DDAExecutionIteration(i, tc);

                iteration++;
                try
                {
                    System.Console.Out.Write(string.Format("Executing Iteration '{0}'\n",iteration));
                    ExecuteTestCaseIteration(currentExecutionIteration);
                    System.Console.Out.Write("\n");
                }
                catch (DDAIterationException e)
                {
                    error += string.Format("\nAt Iteration [{0}], The following Exception was thrown: {1}", iteration, e.Message);
                    continue;
                }
            }

            if (error == null)
            {
                return true;
            }
            else
            {
                throw new DDATestCaseException(error);
            }
        }

        //This method gets the name of the method from the steps in the test case object then,
        //It parses the parameter names from the method name(string) .
        //After parsing it gets the parameter values from test case object using those parsed parmameter names.
        //It later call the Invoke Method which inturn invokes the appropriate method and then gets the result of the invoked method.
        //It will do above steps for all the steps and expected results in the test case object.
        //It will check the results of all the steps and returns false(test case failed) if atleast one step is failed else it returns true(test case passed).
        private static bool ExecuteTestCaseIteration(DDAExecutionIteration iter)
        {
            MSCOM.DDA.CONFIG.Manager config = new MSCOM.DDA.CONFIG.Manager();
            string error = null;
            List<object> parametersList = new List<object>();
            int stepNo;
            int latestActionStepNo = -1;

            iter.stepsOutcome = new Dictionary<string, object>();

            foreach (DDAStep step in iter.steps)
            { 
                stepNo= step.position +1;
               
                parametersList.Clear(); //Iteration's Parameters are cleared for every step execution

                if (step.action.mappedMethodSignature != null && step.action.mappedMethodSignature != string.Empty)
                {
                    parametersList = DDAParser.GetParametersValues(step.action.methodSignature, step.action.parametersMTMValues, iter.stepsOutcome, stepNo, latestActionStepNo);

                    try
                    {
                        iter.stepsOutcome.Add("$ACTION_RESULT_" +  stepNo, DDAInvoker.InvokeMethod(step.action.invokeMethodName, step.action.reference, step.action.assembly, parametersList.ToArray()));
                    }
                    catch (DDAStepException sEx)
                    {
                        iter.stepsOutcome.Add("$ACTION_RESULT_" + stepNo, null);
                        error += string.Format((error == null ? "" : "\n") + "Step Level Exception: At Step '{0}', while executing Action '{1}':\n {2}", stepNo, step.action.mappedMethodSignature, sEx.Message);
                        continue;
                    }
                    catch (DDAIterationException iEx)
                    {
                        iter.stepsOutcome.Add("$ACTION_RESULT_" + stepNo, null);
                        error += string.Format((error == null ? "":"\n") + "Iteration Level Exception: At Step '{0}', while executing Action '{1}':\n {2}", stepNo, step.action.mappedMethodSignature, iEx.Message);
                        break;
                    }
                    catch (DDATestCaseException tEx)
                    {
                        iter.stepsOutcome.Add("$EXPECTATION_RESULT_" + stepNo, null);
                        throw tEx;
                    }
                    finally
                    {
                        step.action.result.Outcome = iter.stepsOutcome["$ACTION_RESULT_" + stepNo];
                        latestActionStepNo = stepNo;
                        //DDAInstrumentation.WriteInProgress(this, step, ActionType.action, parametersList, iteration, step.position == 1);
                    }
                }

                if (step.expectedResult.mappedMethodSignature != null && step.expectedResult.mappedMethodSignature != string.Empty)
                {
                    parametersList = DDAParser.GetParametersValues(step.expectedResult.methodSignature, step.expectedResult.parametersMTMValues, iter.stepsOutcome, stepNo, latestActionStepNo);
                    try
                    {
                        iter.stepsOutcome.Add("$EXPECTATION_RESULT_" + stepNo, DDAInvoker.InvokeMethod(step.expectedResult.invokeMethodName, step.expectedResult.reference, step.expectedResult.assembly, parametersList.ToArray()));
                    }
                    catch (DDAStepException sEx)
                    {
                        iter.stepsOutcome.Add("$EXPECTATION_RESULT_" + stepNo, null);
                        error += string.Format((error == null ? "" : "\n") + "Step Level Exception: At Step '{0}', while executing Expected Result '{1}':\n {2}", stepNo, step.expectedResult.mappedMethodSignature, sEx.Message);
                        continue;
                    }
                    catch (DDAIterationException iEx)
                    {
                        iter.stepsOutcome.Add("$EXPECTATION_RESULT_" + stepNo, null);
                        error += string.Format((error == null ? "" : "\n") + "Iteration Level Exception: At Step '{0}', while executing Expected Result '{1}':\n {2}", stepNo, step.expectedResult.mappedMethodSignature, iEx.Message);
                        break;
                    }
                    catch (DDATestCaseException tEx)
                    {
                        iter.stepsOutcome.Add("$EXPECTATION_RESULT_" + stepNo, null);
                        throw tEx;
                    }
                    finally
                    {
                        step.expectedResult.result.Outcome = iter.stepsOutcome["$EXPECTATION_RESULT_" + stepNo];
                        step.expectedResult.result.IsPassed = (step.expectedResult.result.Outcome != null) ? (bool)step.expectedResult.result.Outcome : false;
                        latestActionStepNo = stepNo;
                        //DDAInstrumentation.WriteInProgress(this, step, ActionType.expectedResult, parametersList, iteration);
                    }
                }
            }

            if (error != null)
            {
                throw new DDAIterationException(error);
            }

            return true;
        }
    }
}
