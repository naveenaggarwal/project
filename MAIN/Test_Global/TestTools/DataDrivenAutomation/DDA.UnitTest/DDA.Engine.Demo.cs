using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSCOM.Test.MTM;
using MSCOM.Test.TFS;
using MSCOM.DDA.ENGINE;

namespace MSCOM.DDA.UnitTest
{
    [TestClass]
    public class Demo
    {
        private static TFSContext TFSContext;
        private MTMTestCase currentTC;

        public TestContext TestContext { get; set; }

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            if (!DDAUnitTest.Default.LoadWorkItemsFromFile)
            {
                TFSContext = new TFSContext(DDAUnitTest.Default.TFSServer, DDAUnitTest.Default.TFSProject, DDAUnitTest.Default.TFSProjectCollection);
            }
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            //Get TestCaseContext from MTMContext            
            if (DDAUnitTest.Default.LoadWorkItemsFromFile)
            {
                currentTC = MTMTestCase.FromWorkItemInFile(Int32.Parse(this.TestContext.Properties["TestCaseId"].ToString()));
            }
            else 
            {
                currentTC = new MTMTestCase(Int32.Parse(this.TestContext.Properties["TestCaseId"].ToString()), TFSContext.tfsProject);
            }

           currentTC.ReplaceValuesInAllIterations(Tools.getEnvironmentValuesFilePath(DDAUnitTest.Default.EnvironmentValues));
        }

        [TestMethod]
        [WorkItem(54286)]
        [TestProperty("TestCaseId", "54286")]
        public void ClassicStaticTestCase()
        {
            DDAStaticExecute.ExecuteMTMTestCase(currentTC);
        }

        [TestMethod]
        [WorkItem(54285)]
        [TestProperty("TestCaseId", "54285")]
        public void ClassicWithParametersTestCase()
        {
            DDAStaticExecute.ExecuteMTMTestCase(currentTC);
        }

        [TestMethod]
        [WorkItem(54284)]
        [TestProperty("TestCaseId", "54284")]
        public void IterationsReplacementTestCase()
        {
            DDAStaticExecute.ExecuteMTMTestCase(currentTC);
        }

        [TestMethod]
        [WorkItem(54283)]
        [TestProperty("TestCaseId", "54283")]
        public void MultipleIterationsTestCase()
        {
            DDAStaticExecute.ExecuteMTMTestCase(currentTC);
        }

        [TestMethod]
        [WorkItem(54282)]
        [TestProperty("TestCaseId", "54282")]
        public void StepsDependencyTestCase()
        {
            DDAStaticExecute.ExecuteMTMTestCase(currentTC);            
        }

        [TestMethod]
        [WorkItem(54281)]
        [TestProperty("TestCaseId", "54281")]
        public void StartAtStepAheadTestCase()
        {
        }

        [TestMethod]
        [WorkItem(54280)]
        [TestProperty("TestCaseId", "54280")]
        public void ClassLoadedObjectTestCase()
        {
        }

        [TestMethod]
        [WorkItem(54279)]
        [TestProperty("TestCaseId", "54279")]
        public void VersionedStepsTestCase()
        {
        }
    }
}
