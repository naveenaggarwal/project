using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace MSCOM.BusinessHelper
{
    public static class SQLHelper
    {
        #region CONSTANTS
        public static string ServerName = "hbir4trrc7.database.windows.net";
        public static string DatabaseName = "RoleManagement";
        public static string UserName = "RMTDBAdmin@hbir4trrc7";
        public static string Password = "OPX@2015";
        #endregion

        /// <summary>
        /// Helper method to kick a job. User current user's credentials.
        /// </summary>
        /// <param name="serverName">Name of the server hosting the Job.</param>
        /// <param name="jobName">Name of the job to run.</param>
        public static void KickJob(string serverName, string jobName)
        {
            Server server;
            try
            {
                server = new Server(serverName);
                Job job = server.JobServer.Jobs[jobName];
                job.Start();
            }
            catch (Exception e)
            {
                throw new DDA.DDAStepException(string.Format("There was an error while running SQLJob '{0}' at '{1}'. Exception Details: {2}", jobName, serverName, e.Message));
            }

            Thread.Sleep(3 * 1000);

            MSCOM.Test.Tools.TestAgent.LogToTestResult(DateTime.Now.Subtract(server.JobServer.Jobs[jobName].LastRunDate).ToString());
        }

        /// <summary>
        /// Helper method to run a query. User current user's credentials.
        /// </summary>
        /// <param name="serverName">Name of the server to run the query on.</param>
        /// <param name="databaseName">Name of the DB in the server.</param>
        /// <param name="query">String containing the query.</param>
        /// <returns>Returns the results of this query.</returns>
        public static List<string[]> RunQuery(string query, string fileName)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};Integrated Security=true;", ServerName, DatabaseName);

            List<string> resultSetRow = new List<string>();
            List<string[]> result = new List<string[]>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                resultSetRow.Add(reader.GetName(i).ToString());
                            }

                            result.Add(resultSetRow.ToArray());
                            resultSetRow.Clear();

                            while (reader.Read())
                            {
                                Object[] values = new Object[reader.FieldCount];
                                reader.GetValues(values);
                                foreach (Object value in values)
                                {
                                    resultSetRow.Add(value.ToString());
                                }
                                result.Add(resultSetRow.ToArray());
                                resultSetRow.Clear();
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw new DDA.DDAIterationException(string.Format("SQLHelper was unable to complete the execution of Query '{0}' at DB '{1}' on Server '{2}'. Error: {3}", query, DatabaseName, ServerName, e.Message));
            }

            return result;
        }

        ///// <summary>
        ///// Helper method to run a Stored Procedure with no arguments. Using current user's credentials.
        ///// </summary>
        ///// <param name="serverName">Name of the server to run the query on.</param>
        ///// <param name="databaseName">Name of the DB in the server.</param>
        ///// <param name="spName">Name of the Stored Procedure to run.</param>
        ///// <returns>Two dimensions data structure with the result of the Stored Procedure</returns>
        //public static List<string[]> RunStoredProcedure(string serverName, string databaseName, string spName, string parameters = "")
        //{
        //    return RunQuery(string.Format("exec [{0}] {1}", spName, parameters));
        //}
    }
}
