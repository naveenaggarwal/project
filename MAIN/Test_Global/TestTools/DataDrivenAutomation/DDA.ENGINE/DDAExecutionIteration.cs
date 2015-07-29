using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSCOM.Test.MTM;

namespace MSCOM.DDA.ENGINE
{
    /// <summary>
    /// This class represents a mix of Test Case (Primarily a list of steps), 
    /// a Data Set (Data Iteration) to be used in said steps and relevant 
    /// execution items such as results and outcomes for each step execution.
    /// </summary>
    public class DDAExecutionIteration
    {
        public List<DDAStep> steps = new List<DDAStep>();
        public string resultNote;
        public Boolean passed;
        public Dictionary<string, Object> stepsOutcome;

        public static List<DDAExecutionIteration> ExtractMTMIterations(MTMTestCase tc)
        {
            List<DDAExecutionIteration> result = null;
            DDAStep currentStep = null;
            DDAExecutionIteration currentExecutionIteration = null;
            foreach (MTMDataIteration dataIter in tc.DataIterations)
            {
                foreach (MTMStep s in tc.steps)
                {
                    currentStep = new DDAStep(new DDAPair(s.Action, dataIter.data), new DDAPair(s.ExpectedResult, dataIter.data), s.Position);
                    currentExecutionIteration.steps.Add(currentStep);
                }
                
                result.Add(currentExecutionIteration);
            }

            return result;
        }

        public DDAExecutionIteration(MTMDataIteration dataIter, MTMTestCase tc)
        {
            DDAPair action = null;
            DDAPair expectedResult = null;
            foreach (MTMStep s in tc.steps)
            {
                action = new DDAPair(s.Action, dataIter.data);
                expectedResult = new DDAPair(s.ExpectedResult, dataIter.data);

                this.steps.Add(new DDAStep(action, expectedResult, s.Position));
            }                
        }     

        public Boolean Passed()
        {
            foreach (DDAStep s in steps)
            {
                if (!s.Passed())
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean AllPassed(List<DDAExecutionIteration> iterations)
        {
            foreach (DDAExecutionIteration i in iterations)
            {
                if (!i.Passed())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
