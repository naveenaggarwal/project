using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCOM.Test.MTM;
using MSCOM.DDA.CONFIG;

namespace MSCOM.DDA.ENGINE
{
    public class DDATestSuite
    {
        public List<DDAExecutionIteration> executionIterations;
        public List<MTMTestCase> mtmTCs;
        public MSCOM.Test.TFS.TFSContext tfsContext;

        public DDATestSuite(string tfsServer, string tfsProject)
        {
            executionIterations = new List<DDAExecutionIteration>();
            mtmTCs = new List<MTMTestCase>();
            tfsContext = new Test.TFS.TFSContext(tfsServer, tfsProject);
        }

        private void AppendIterations(List<DDAExecutionIteration> iters)
        {
            foreach (DDAExecutionIteration i in iters)
            {
                executionIterations.Add(i);
            }
        }


        /// <summary>
        /// To be called from Assert at Test Solution Test Method
        /// </summary>
        /// <param name="tCId">Test Case Id for the Test Case associated to Test Method to be executed</param>
        /// <param name="tfsServer"></param>
        /// <param name="tfsProject"></param>
        /// <param name="tfsProjectCollection"></param>
        /// <returns>Returns true if TC passed (all steps were completed succesfully and all Actual Results were as Expected). Throws DDATestCaseException otherwise</returns>
        public bool ExecuteMTMTestCase(string tCId, string tfsServer, string tfsProject, string tfsProjectCollection)
        {   
            MSCOM.Test.TFS.TFSContext tfsContext = new Test.TFS.TFSContext(tfsServer, tfsProject, tfsProjectCollection);
            MTMTestCase currentTC = new MTMTestCase(Int32.Parse(tCId), tfsContext.tfsProject);
            return DDAStaticExecute.ExecuteMTMTestCase(currentTC);
        }
    }
}
