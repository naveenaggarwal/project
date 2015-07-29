using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Client;
using System.Collections.Generic;
using MSCOM.Test.TFS;
using MSCOM.Test.MTM;
using System.Text;


namespace MSCOM.Test.Tools
{
    [TestClass]
    public class Operations
    {
        #region FILE PATHS
        
        private string originalIdListPath = string.Format(@"{0}\IO\{1}_{2}.csv", System.IO.Directory.GetCurrentDirectory(), WorkItemsMigrate.Default.OriginalIdList, WorkItemsMigrate.Default.ProductName);
        private string updatePairedIdListPath = string.Format(@"{0}\IO\{1}_{2}.csv", System.IO.Directory.GetCurrentDirectory(), WorkItemsMigrate.Default.UpdatePairedIdList, WorkItemsMigrate.Default.ProductName);
        private string originalIdListWithDetailsPath = string.Format(@"{0}\IO\{1}_{2}.csv", System.IO.Directory.GetCurrentDirectory(), WorkItemsMigrate.Default.OriginalDetails, WorkItemsMigrate.Default.ProductName);
        private string originalIdListWithMappingNewIdsPath = string.Format(@"{0}\IO\{1}_{2}_MAPP.csv", System.IO.Directory.GetCurrentDirectory(), WorkItemsMigrate.Default.OriginalDetails, WorkItemsMigrate.Default.ProductName);
        private string tcOriginalUrl = string.Format("{0}/web/wi.aspx?id={1}", WorkItemsMigrate.Default.TFSServerOriginal, ID_REPLACE);
        private static string fieldsToMigratePath = string.Format(@"{0}\IO\{1}_{2}.csv", System.IO.Directory.GetCurrentDirectory(), WorkItemsMigrate.Default.FieldsToMigrateFileName, WorkItemsMigrate.Default.ProductName);
        
        #endregion

        #region UI MESSAGES

        private static string exportConfirmMessage = "You are attempting to 'EXPORT' around {0} work item(s) from {1}. This action involves the creation of {0} file(s) and may replace any existing file(s) in 'IO' folder. Please confirm your action.";
        private static string validationWithHTMLConfirmMessage = "You are attempting to 'VALIDATE' around {0} work item(s). This action involves the creation of {0} file(s) and may replace any existing file(s) in 'IO' folder. Please confirm your action.";
        private static string validationWithNoHTMLConfirmMessage = "You are attempting to 'VALIDATE' around {0} work item(s). Please confirm your action.";
        private static string migrationConfirmMessage = "You are attempting to 'MIGRATE' around {0} work item(s) into {1}. Please confirm your action.";
        private static string updateConfirmMessage = "You are attempting to 'UPDATE' around {0} work item(s) at {1}. Please confirm your action.";
        private static string reLinkConfirmMessage = "You are attempting to 'LINK' around {0} work item(s) at {1}. Please confirm your action.";

        #endregion

        #region STATIC
       
        private static string TFS_TO_CSV = "TFS_TO_CSV";
        private static string CSV_TO_TFS = "CSV_TO_TFS";
        private static string TFS_VALIDATE = "TFS_VALIDATE";
        private static string TFS_UPDATE = "TFS_UPDATE";
        private static string TFS_LINKS = "TFS_LINKS";
       
        private static string ID_REPLACE = "ID_REPLACE";
        
        #endregion

        private static TFSContext TFSContext;
        private MTMTestCase currentTC;
        private static string[] fieldsToMigrate;
                
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            fieldsToMigrate = CSVManager.ParseCSV(fieldsToMigratePath).ToArray()[0];
        }
        
        [TestMethod]
        public void FromTFSToCSV()
        {
            //Getting TFS Context of Original TFS Server as per configured values
            TFSContext = new TFSContext(WorkItemsMigrate.Default.TFSServerOriginal, WorkItemsMigrate.Default.TFSProjectOriginal, WorkItemsMigrate.Default.TFSProjectCollectionOriginal);

            string logContent = "";

            bool errorFound = false;
            int success = 0;
            int fail = 0;
            int skipped = 0;       
            
            int currentID;
            bool isFirst = true;

            //List of WorkItem Ids to be exported from Original TFS in CSV format. Each line should contain an Id and there must be a header, since the first element is ignored.
            List<string[]> wiIds = CSVManager.ParseCSV(originalIdListPath);

            //Request to user to Confirm the requested Action which warns users of the potential of multiple files creation
            if (!(System.Windows.Forms.MessageBox.Show(string.Format(exportConfirmMessage, wiIds.Count, WorkItemsMigrate.Default.TFSServerOriginal), "CONFIRM EXPORT", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK))
            {
                Assert.Fail("Operation cancelled by user.");
                return;
            }

            //Looping through all loaded WorkItem Ids
            foreach (string[] wiLine in wiIds)
            {
                if (!isFirst) //first item is considered a header so it gets ignored
                {
                    if (wiLine[0].Contains("--")) //'--' is considered a comment and get's skipped (and logged)
                    {
                        logContent += string.Format("Skipped - Work Item: {0} has been skipped as it is in comments.$", wiLine[0].Replace("--", ""));
                        skipped++;
                        continue;
                    }
                    else if (wiLine[0].Trim() == "")
                    {
                        continue; //Empty lines are ignored.
                    }

                    //Creating a file for the current WorkItem based on its Id that will contain exported fields data in csv format
                    Int32.TryParse(wiLine[0], out currentID);
                    System.IO.StreamWriter outPutCSV = new System.IO.StreamWriter(this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.csv", currentID)));
                    
                    try
                    {                        
                        currentTC = new MTMTestCase(currentID, TFSContext.tfsProject);

                        //It is obligatory/minimum requirement to export WorkItem Id and Title. The rest is determined by the 'fields to migrate' list
                        outPutCSV.WriteLine("OriginalId, Title, " + CSVManager.FromArrayToCSVHeaderLine(fieldsToMigrate)); //Creating header for CSV file
                        outPutCSV.WriteLine(CSVManager.FromMTMTestCaseToCSVLine(currentTC, fieldsToMigrate)); //Exporting to csv format based on the 'fields to migrate' list
                        
                        logContent += string.Format("Success - Work Item: {0} has been exported to CSV format at {1}$", wiLine[0], this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.csv", currentID)));
                        success++;
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} could not be exported to CSV format. ${1}$", wiLine[0], e.InnerException + e.Message);
                        fail++;
                    }
                    finally
                    {
                        outPutCSV.Close();
                    }
                }
                else
                {
                    isFirst = false;
                }
            }

            //Overall Log file name. This will be located under dropn 'IO' folder and be named based on the action and time of creation.
            string logFileName = this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}_{1}.log", TFS_TO_CSV, System.DateTime.Now.TimeOfDay.ToString().Replace(':', '.')));

            //Initializing logOutPut var and creating Overall Log file.         
            var logOutPut = new System.IO.StreamWriter(logFileName);

            logOutPut.WriteLine("TFS Work Items Export to CSV Results:");
            logOutPut.WriteLine("");
            foreach (string s in logContent.Split('$'))
            {
                logOutPut.WriteLine(s);
            }

            logOutPut.WriteLine(string.Format("Exported From:{0}/{1}/{2} ", WorkItemsMigrate.Default.TFSServerOriginal, WorkItemsMigrate.Default.TFSProjectCollectionOriginal, WorkItemsMigrate.Default.TFSProjectOriginal));
            logOutPut.WriteLine("Successful Exports: " + success);
            logOutPut.WriteLine("Failed Exports: " + fail);
            logOutPut.WriteLine("Skipped Items: " + skipped);
            logOutPut.WriteLine("Total Processed Items: " + (fail + success + skipped));
            logOutPut.Close();
            
            Assert.IsFalse(errorFound, string.Format("Errors Found during Export. For more details, see {0}.", logFileName));
        }

        [TestMethod]
        public void FromCSVToTFSValidate()
        {
            //Getting TFS Context of New TFS Server as per configured values
            TFSContext = new TFSContext(WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectNew, WorkItemsMigrate.Default.TFSProjectCollectionNew);

            string logContent = "";
            
            bool errorFound = false;
            int success = 0;
            int fail = 0;
            int skipped = 0;          

            List<string[]> currentWI = null;
            bool isFirst = true;

            //List of WorkItem Ids to be validated against NEW TFS as CSV format. Each line should contain an Id and there must be a header, since the first element is ignored.
            List<string[]> wiIds = CSVManager.ParseCSV(originalIdListPath);

            List<string> ParseResults = new List<string>();            
            
            WorkItemTypeCollection workItemTypes = null;
            WorkItem workItem = null;

            //Request to user to Confirm the requested Action. First Pop up is for 'CreateHTMLOnValidate = true' which warns users of the potential of multiple files creation, while the second one does not

            if (!WorkItemsMigrate.Default.CreateHTMLOnValidate)
            {
                if (!(System.Windows.Forms.MessageBox.Show(string.Format(validationWithNoHTMLConfirmMessage, wiIds.Count, WorkItemsMigrate.Default.TFSProjectNew), "CONFIRM VALIDATION", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK))
                {
                    Assert.Fail("Operation cancelled by user.");
                    return;
                }
            }
            else if (!(System.Windows.Forms.MessageBox.Show(string.Format(validationWithHTMLConfirmMessage, wiIds.Count), "CONFIRM VALIDATION", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK))
            {
                Assert.Fail("Operation cancelled by user.");
                return;
            }

            //Looping through all loaded WorkItem Ids
            foreach (string[] wiLine in wiIds)
            {
                if (!isFirst)//first item is considered a header so it gets ignored
                {
                    if (wiLine[0].Contains("--")) //'--' is considered a comment and get's skipped (and logged)
                    {
                        logContent += string.Format("Skipped - Work Item: {0} has been skipped as it is in comments.$", wiLine[0].Replace("--", ""));
                        skipped++;
                        continue;
                    }
                    else if (wiLine[0].Trim() == "")
                    {
                        continue; //Empty lines are ignored.
                    }

                    try
                    {
                        //Reading WorkItem content from CSV file [Created by FromTFSToCSV()] which should be named based on WorkItem Id
                        currentWI = CSVManager.ParseCSV(this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.csv", wiLine[0])));                       

                        //Creating a dummy WorkItem in new TFS. This will never be saved, but is created to see if Validation passes.
                        workItemTypes = TFSContext.tfsProject.Store.Projects[WorkItemsMigrate.Default.TFSProjectNew].WorkItemTypes;
                        workItem = new WorkItem(workItemTypes[WorkItemsMigrate.Default.WorkItemType]);

                        //Loading WorkItem object from csvContent based on fieldsToMigrate
                        workItem = CSVManager.FromCSVLineToWorkItem(fieldsToMigrate, currentWI[1], workItem, out ParseResults);
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} can't be prepared for migration to TFS. {1}$", currentWI[1][0], e.Message);
                        fail++;
                    }

                    if (WorkItemsMigrate.Default.CreateHTMLOnValidate) //If configured to output html on Validation
                    {
                        System.IO.StreamWriter outPutHTML = new System.IO.StreamWriter(this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.html", wiLine[0])));
                        try
                        {
                            outPutHTML.Write(ParseResults.ToArray()[0]);
                            outPutHTML.Close();
                        }
                        catch (Exception e)
                        {
                            logContent += string.Format("Failure: {0} html Validation preview could not be generated. More Details: {1}$", currentWI[1][0], e.Message);
                        }
                        finally
                        {
                            outPutHTML.Close();
                        }
                    }

                    System.Collections.ArrayList validationResults = workItem.Validate(); //Validation results will be loaded into validationResults array

                    if (validationResults.Count > 0)//Loop through validation results and list results in logContent
                    {
                        logContent += string.Format("Failure - Work Item: {0} can't be migrated to TFS. As in {1}.$", currentWI[1][0], this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.csv", wiLine[0])));
                        foreach (Microsoft.TeamFoundation.WorkItemTracking.Client.Field item in validationResults)
                        {
                            logContent += string.Format("Field '{0}' has invalid value '{1}'.$", item.Name, item.Value);
                        }
                        fail++;
                        errorFound = true;
                    }
                    else
                    {
                        logContent += string.Format("Success - Work Item: {0} can be migrated to TFS. As in {1}$", currentWI[1][0], this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.csv", wiLine[0])));
                        success++;
                    }
                }
                else
                {
                    isFirst = false;
                }
            }

            //Overall Log file name. This will be located under dropn 'IO' folder and be named based on the action and time of creation.
            string logFileName = this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}_{1}.log", TFS_VALIDATE, System.DateTime.Now.TimeOfDay.ToString().Replace(':', '.')));

            //Initializing logOutPut var and creating Overall Log file.
            var logOutPut = new System.IO.StreamWriter(logFileName);

            logOutPut.WriteLine("TFS Validation Results:");
            logOutPut.WriteLine("");
            foreach (string s in logContent.Split('$'))
            {
                logOutPut.WriteLine(s);
            }

            logOutPut.WriteLine(string.Format("Migration To:{0}/{1}/{2} ", WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectCollectionNew, WorkItemsMigrate.Default.TFSProjectNew));
            logOutPut.WriteLine("Successfully Processed Items: " + success);
            logOutPut.WriteLine("Failed Items: " + fail);
            logOutPut.WriteLine("Skipped Items: " + skipped);
            logOutPut.WriteLine("Total Processed Items: " + (fail + success + skipped));
            logOutPut.Close();

            Assert.IsFalse(errorFound, string.Format("Errors Found during Migration. For more details, see {0}.", logFileName));
        }

        [TestMethod]
        public void FromCSVToTFSMigrate()
        {
            //Getting TFS Context of New TFS Server as per configured values
            TFSContext = new TFSContext(WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectNew, WorkItemsMigrate.Default.TFSProjectCollectionNew);

            string logContent = "";

            bool errorFound = false;
            int success = 0;
            int fail = 0;
            int skipped = 0;

            List<string[]> currentWI = null;
            bool isFirst = true;

            //List of WorkItem Ids to be migrated to New TFS as CSV format. Each line should contain an Id and there must be a header, since the first element is ignored.
            List<string[]> wiIds = CSVManager.ParseCSV(originalIdListPath);

            List<string> ParseResults = null;            

            WorkItemTypeCollection workItemTypes = null;
            WorkItem workItem = null;

            //Creating Writer for Mapping outPut File based on time of creation
            var outPut = new System.IO.StreamWriter(this.originalIdListWithMappingNewIdsPath.Replace(".csv", string.Format("{0}.csv", System.DateTime.Now.TimeOfDay.ToString().Replace(':', '.'))));
            outPut.WriteLine(string.Format("{0},{1},{2},{3}", "originalId", "newId", "Title", "AllLinks")); //Adding header (csv format)
            
            //Request to user to Confirm the requested Action which warns users of the potential of multiple work items creation
            if (!(System.Windows.Forms.MessageBox.Show(string.Format(migrationConfirmMessage, wiIds.Count, WorkItemsMigrate.Default.TFSServerNew), "CONFIRM MIGRATION", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK))
            {
                Assert.Fail("Operation cancelled by user.");
                return;
            }

            //Looping through all loaded WorkItem Ids
            foreach (string[] wiLine in wiIds)
            {
                if (!isFirst)//first item is considered a header so it gets ignored
                {
                    if (wiLine[0].Contains("--")) //'--' is considered a comment and get's skipped (and logged)
                    {
                        logContent += string.Format("Skipped - Work Item: {0} has been skipped as it is in comments.$", wiLine[0].Replace("--", ""));
                        skipped++;
                        continue;
                    }
                    else if (wiLine[0].Trim() == "")
                    {
                        continue; //Empty lines are ignored.
                    }

                    try
                    {
                        //Reading WorkItem content from CSV file [Created by FromTFSToCSV()] which should be named based on WorkItem Id
                        currentWI = CSVManager.ParseCSV(this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.csv", wiLine[0])));

                        //Creating a WorkItem in new TFS to be saved if valid.
                        workItemTypes = TFSContext.tfsProject.Store.Projects[WorkItemsMigrate.Default.TFSProjectNew].WorkItemTypes;
                        workItem = new WorkItem(workItemTypes[WorkItemsMigrate.Default.WorkItemType]);

                        //Loading WorkItem object from csvContent based on fieldsToMigrate
                        workItem = CSVManager.FromCSVLineToWorkItem(fieldsToMigrate, currentWI[1], workItem, out ParseResults);
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} can't be prepared for migration to TFS. {1}$", currentWI[1][0], e.Message);
                        fail++;
                    }

                    try
                    {
                        workItem.Save();
                        logContent += string.Format("Success - Work Item: {0} has been migrated to TFS. New Work Item Id: {1}$", currentWI[1][0], workItem.Id);
                        outPut.WriteLine(string.Format("{0},{1},{2},{3}", currentWI[1][0], workItem.Id, workItem.Title, ParseResults.ToArray()[1]));
                        success++;
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} was not migrated to TFS. Error Detail: {1}$", currentWI[1][0], e.InnerException + e.Message);
                        fail++;
                    }                    
                }
                else
                {
                    isFirst = false;
                }
            }

            outPut.Close();
            //Overall Log file name. This will be located under dropn 'IO' folder and be named based on the action and time of creation.
            string logFileName = this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}_{1}.log", CSV_TO_TFS, System.DateTime.Now.TimeOfDay.ToString().Replace(':', '.')));

            //Initializing logOutPut var and creating Overall Log file.
            var logOutPut = new System.IO.StreamWriter(logFileName);
            logOutPut.WriteLine("TFS Migration Results");
            logOutPut.WriteLine("");
            foreach (string s in logContent.Split('$'))
            {
                logOutPut.WriteLine(s);
            }

            logOutPut.WriteLine(string.Format("Migration To:{0}/{1}/{2} ", WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectCollectionNew, WorkItemsMigrate.Default.TFSProjectNew));
            logOutPut.WriteLine("Successfully Processed Items: " + success);
            logOutPut.WriteLine("Failed Items: " + fail);
            logOutPut.WriteLine("Skipped Items: " + skipped);
            logOutPut.WriteLine("Total Processed Items: " + (fail + success + skipped));
            logOutPut.Close();

            Assert.IsFalse(errorFound, string.Format("Errors Found during Migration. For more details, see {0}.", logFileName));
        }

        [TestMethod]
        public void UpdateOnTFS()
        {
            //Getting TFS Context of New TFS Server as per configured values            
            TFSContext = new TFSContext(WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectNew, WorkItemsMigrate.Default.TFSProjectCollectionNew);

            string logContent = "";

            bool errorFound = false;
            int success = 0;
            int fail = 0;
            int skipped = 0;

            List<string[]> currentWI = null;
            bool isFirst = true;

            //List of WorkItem Ids to be updated at New TFS as CSV format. Each line should contain an original Id in the first column and it's pairing new Id in the second. There must be a header, since the first element is ignored.
            List<string[]> wiIdPairs = CSVManager.ParseCSV(updatePairedIdListPath);

            List<string> ParseResults = new List<string>();            
            
            WorkItem workItem = null;
            int tryId = -1;
            
            //Request to user to Confirm the requested Action which warns users of the potential of multiple work items changes
            if (!(System.Windows.Forms.MessageBox.Show(string.Format(updateConfirmMessage, wiIdPairs.Count, WorkItemsMigrate.Default.TFSServerNew), "CONFIRM UPDATE", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK))
            {
                Assert.Fail("Operation cancelled by user.");
                return;
            }

            //Looping through all loaded WorkItem Ids
            foreach (string[] wiLine in wiIdPairs)
            {
                if (!isFirst)//first item is considered a header so it gets ignored
                {
                    if (wiLine[0].Contains("--"))//'--' is considered a comment and get's skipped (and logged)
                    {
                        logContent += string.Format("Skipped - Work Item: {0} has been skipped as it is in comments.$", wiLine[0].Replace("--", ""));
                        skipped++;
                        continue;
                    }
                    else if (wiLine[0].Trim() == "")
                    {
                        continue; //Empty lines are ignored.
                    }

                    try
                    {
                        //Reading WorkItem content from CSV file [Created by FromTFSToCSV()] which should be named based on WorkItem Id
                        currentWI = CSVManager.ParseCSV(this.originalIdListWithDetailsPath.Replace(".csv", string.Format("_{0}.csv", wiLine[0])));

                        Int32.TryParse(wiLine[1], out tryId);

                        //Creating a WorkItem in new TFS to be saved if valid.                        
                        workItem = TFSContext.tfsProject.Store.GetWorkItem(tryId);
                        workItem = CSVManager.FromCSVLineToWorkItem(fieldsToMigrate, currentWI[1], workItem, out ParseResults);
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} can't be updated. {1}$", workItem.Id, e.Message);
                        fail++;
                    }

                    try
                    {
                        workItem.Save();
                        logContent += string.Format("Success - Work Item: {1} has been updated as per Original Work Item Id: {0}$", currentWI[1][0], workItem.Id);
                        success++;
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} can't be updated. Error Detail: {1}$", workItem.Id, e.Message);
                        fail++;
                    }                   
                }
                else
                {
                    isFirst = false;
                }
            }

            //Overall Log file name. This will be located under dropn 'IO' folder and be named based on the action and time of creation.
            string logFileName = this.updatePairedIdListPath.Replace(".csv", string.Format("__{0}_{1}.log", TFS_UPDATE, System.DateTime.Now.TimeOfDay.ToString().Replace(':', '.')));

            //Initializing logOutPut var and creating Overall Log file.
            var logOutPut = new System.IO.StreamWriter(logFileName);
            logOutPut.WriteLine("TFS Update Results");
            logOutPut.WriteLine("");
            foreach (string s in logContent.Split('$'))
            {
                logOutPut.WriteLine(s);
            }

            logOutPut.WriteLine(string.Format("Update at:{0}/{1}/{2} ", WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectCollectionNew, WorkItemsMigrate.Default.TFSProjectNew));
            logOutPut.WriteLine("Successfully Processed Items: " + success);
            logOutPut.WriteLine("Failed Items: " + fail);
            logOutPut.WriteLine("Skipped Items: " + skipped);
            logOutPut.WriteLine("Total Processed Items: " + (fail + success + skipped));
            logOutPut.Close();

            Assert.IsFalse(errorFound, string.Format("Errors Found during Update. For more details, see {0}.", logFileName));
        }

        [TestMethod]
        public void ReLink()
        {
            //Getting TFS Context of New TFS Server as per configured values            
            TFSContext = new TFSContext(WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectNew, WorkItemsMigrate.Default.TFSProjectCollectionNew);

            List<string[]> migrationMappingResults = CSVManager.ParseCSV(updatePairedIdListPath);//Contains: "OriginalId, newId, Title, AllLinks" as resulting from Migration 'FromCSVToTFSMigrate'
            
            List<string[]> oneDimensionLinks = new List<string[]>();           

            //Turning the Mapping data from a 2D array to a 1D array
            foreach (string[] wiLine in migrationMappingResults)
            {
                if (CSVManager.Decode(wiLine[3]).Contains("."))
                {
                    foreach (string link in CSVManager.Decode(wiLine[3]).Split('|'))
                    {
                        oneDimensionLinks.Add(new string[] { wiLine[0], wiLine[1], link.Split('.')[0], link.Split('.')[1] });
                    }
                }
            }

            //Creating links with new IDs only
            List<string[]> newLinks = new List<string[]>();
           
            foreach (string[] oneDimensionLink in oneDimensionLinks.ToArray())
            {
                foreach (string[] iter in oneDimensionLinks.ToArray())
                {
                    if (oneDimensionLink[2] == iter[0])
                    {
                        newLinks.Add(new string[] { oneDimensionLink[1].Trim(), iter[1].Trim(), oneDimensionLink[3].Trim()});
                    }
                }
            }

            //Removing redundant links Eg: 1->2 = 2<-1
            List<string[]> finalLinks = new List<string[]>();
            string csvFinalLinks = "";

            foreach (string[] link in newLinks)
            {
                if (!csvFinalLinks.Contains(string.Format("{0},{1}", (link[0]), link[1])) && !csvFinalLinks.Contains(string.Format("{0},{1}", (link[1]), link[0])))
                {
                    csvFinalLinks += string.Format("{0},{1},{2}|", link[0], link[1], link[2]);                    
                }
            }

            //Adding new links to List of csv lines
            foreach (string csvLine in csvFinalLinks.Split('|'))
            {
                if (csvLine.Trim() == "")
                {
                    continue;
                }
                else
                {
                    finalLinks.Add(new string[] { csvLine.Split(',')[0], csvLine.Split(',')[1], csvLine.Split(',')[2] });
                }
            }      

            string logContent = "";

            bool errorFound = false;
            int success = 0;
            int fail = 0;
            int skipped = 0;

            List<string> ParseResults = new List<string>();

            WorkItemLinkTypeEnd workItemLinkType = null;
            WorkItem workItem = null;
            int tryId = -1;

            //Request to user to Confirm the requested Action which warns users of the potential of multiple work items changes
            if (!(System.Windows.Forms.MessageBox.Show(string.Format(reLinkConfirmMessage, finalLinks.Count, WorkItemsMigrate.Default.TFSServerNew), "CONFIRM LINK", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK))
            {
                Assert.Fail("Operation cancelled by user.");
                return;
            }

            //Looping through all loaded WorkItem Ids
            foreach (string[] linkLine in finalLinks)
            {
                
                    if (linkLine[0].Trim() == "")
                    {
                        continue; //Empty lines are ignored.
                    }

                    try
                    {
                        workItemLinkType = TFSContext.tfsProject.Store.WorkItemLinkTypes.LinkTypeEnds[linkLine[2]]; //Creates a Work ITem Link Type based on link line from CSV

                        //Getting the WorkItem to be linked from new TFS to be saved if valid.                        
                        Int32.TryParse(linkLine[0], out tryId);
                        workItem = TFSContext.tfsProject.Store.GetWorkItem(tryId);

                        Int32.TryParse(linkLine[1], out tryId); //PArsing id of item to be linked
                        workItem.Links.Add(new RelatedLink(workItemLinkType, tryId)); //Linking items            
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} can't be linked to {1}. Error Detail: {2}$", linkLine[0], linkLine[1], e.Message);
                        fail++;
                    }

                    try
                    {
                        workItem.Save();
                        logContent += string.Format("Success - Work Item: {0} has been linked with: {1} as '{2}'$", linkLine[0], linkLine[1], linkLine[2]);
                        success++;
                    }
                    catch (Exception e)
                    {
                        errorFound = true;
                        logContent += string.Format("Failure - Work Item: {0} can't be linked to {1}. Error Detail: '{2}'$", linkLine[0], linkLine[1], e.Message);
                        fail++;
                    }
            }

            //Overall Log file name. This will be located under drop 'IO' folder and be named based on the action and time of creation.
            string logFileName = this.updatePairedIdListPath.Replace(".csv", string.Format("_{0}_{1}.log", TFS_LINKS, System.DateTime.Now.TimeOfDay.ToString().Replace(':', '.')));

            //Initializing logOutPut var and creating Overall Log file.
            var logOutPut = new System.IO.StreamWriter(logFileName);
            logOutPut.WriteLine("TFS Linking Results");
            logOutPut.WriteLine("");
            foreach (string s in logContent.Split('$'))
            {
                logOutPut.WriteLine(s);
            }

            logOutPut.WriteLine(string.Format("Update at:{0}/{1}/{2} ", WorkItemsMigrate.Default.TFSServerNew, WorkItemsMigrate.Default.TFSProjectCollectionNew, WorkItemsMigrate.Default.TFSProjectNew));
            logOutPut.WriteLine("Successfully Processed Items: " + success);
            logOutPut.WriteLine("Failed Items: " + fail);
            logOutPut.WriteLine("Skipped Items: " + skipped);
            logOutPut.WriteLine("Total Processed Links: " + (fail + success + skipped));
            logOutPut.Close();

            Assert.IsFalse(errorFound, string.Format("Errors Found during Linking. For more details, see {0}.", logFileName));
        }
    }
}
