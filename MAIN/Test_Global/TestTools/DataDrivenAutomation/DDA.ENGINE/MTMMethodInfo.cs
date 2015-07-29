using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSCOM.Test.MTM;

namespace MSCOM.DDA.ENGINE
{
    public enum ActionType
    {
        action,
        expectedResult,
        failure,
        finalResult
    };

    //This Class is usefull for logging all the information of method which is invoked during test case execution.
   
    public class MTMMethodInfo
    {

        //private MTMExecutionItem item;
        private MTMTestCase item;
        private int index;
        private static List<string> loggerList = new List<string>();

        public MTMMethodInfo(MTMTestCase item, int index = 0)
        {
            this.item = item;
            this.index = index;        
        }

        public int TestId
        {
            get { return this.item.Id; }            
        }

        public string MethodName(ActionType type)
        {
            return DDAParser.GetMethodName(InvokedMethod(type));           
        }
        public List<object> Parameters
        {
            //get { return MAIAParser.GetParametersValues(item.testCase.steps[index].actionMethodSignature, item.testCase.iterations[0]); } 
            get;
            set;
        }
       
        public object ActionOutcome
        {
            get {return "IMPLEMENT";}//this.item.steps[index]. result.outcome;}
        }

        public object ExpectedResultOutcome
        {
            get { return "IMPLEMENT"; }//this.item.steps[index].result.isPassed; }
        }

        public int StepNo
        {
            get { return this.index; }
        }

        public string InvokedMethod(ActionType type)
        {
            ///if (type == ActionType.action)
            ///    return this.item.matchedSteps[index].actionMethodSignature;
            ///else
            ///    return this.item.matchedSteps[index].expectedResultMethodSignature;
            return "";
            
        }

        public object FinalTestResult
        {
            get
            {
                return null; ///this.item.testCase.MTMIterations[0].result.IsPassed;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return "IMPLEMENT";//this.item.steps[index].result.note;
            }
        }
        public static List<string> LoggerText
        {
            get
            {
                return loggerList;
            }            
        }
        

        //public static bool testIdFlag
        //{
        //    get;
        //    set;
        //}
       
    }
}
