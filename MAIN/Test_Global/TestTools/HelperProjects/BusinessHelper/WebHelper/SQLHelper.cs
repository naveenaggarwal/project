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
        /// Helper method to run a query. User current user's credentials.
        /// </summary>
        /// <param name="query">String containing the query.</param>
        /// <returns>Returns the results of this query.</returns>
        public static List<string[]> RunQuery(string query)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);

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

                return result;
            }
            catch (Exception e)
            {
                throw new DDA.DDAIterationException(string.Format("SQLHelper was unable to complete the execution of Query '{0}' at DB '{1}' on Server '{2}'. Error: {3}", query, DatabaseName, ServerName, e.Message));
            }

        }

        public static List<string[]> RunQueryAndCompare(string query, List<string> dataValues)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);

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

                //return result;
            }
            catch (Exception e)
            {
                throw new DDA.DDAIterationException(string.Format("SQLHelper was unable to complete the execution of Query '{0}' at DB '{1}' on Server '{2}'. Error: {3}", query, DatabaseName, ServerName, e.Message));
            }

            if (IsDataEnabledInDatabase(result, dataValues))
            {
                return result;
            }
            else
            {
                throw new DDA.DDAIterationException("The values are not enabled in the database.");
            }
        }

        /// <summary>
        /// Helper method to run a query and compare the results
        /// </summary>
        /// <param name="query">String containing the query.</param>
        /// <param name="value1">data which is added through UI</param>
        /// <param name="value2">data which is added through UI</param>
        /// <returns>Returns the results of this query.</returns>
        public static List<string[]> RunQueryAndCompare(string query, string value1, string value2)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);

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

            if (IsDataAddedToDatabase(result, value1, value2))
            {
                return result;
            }
            else
            {
                throw new DDA.DDAIterationException(string.Format("The values '{0}' and '{1}' were not mapped to the database.", value1, value2));
            }
        }

        /// <summary>
        /// Compares the results returned from the query with he values provided
        /// </summary>
        /// <param name="dbData">SQL query result</param>
        /// <param name="data1">data which is added through UI</param>
        /// <param name="data2">data which is added through UI</param>
        /// <returns>True if the data is added. Else returns false.</returns>
        public static bool IsDataAddedToDatabase(List<string[]> dbData, string data1, string data2)
        {
            int count = 0;
            for (int i = 0; i < dbData.Count; i++)
            {
                if (dbData[i][0].Equals(data1))
                {
                    count++;
                }
                else if (dbData[i][0].Equals(data2))
                {
                    count++;
                }
                else if (count == 2)
                {
                    return true;
                }
            }

            if (count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether the data is enabled in the database
        /// </summary>
        /// <param name="dbData">SQL query result</param>
        /// <param name="dataValues">data retrieved from the textbox</param>
        /// <returns>True if data is enabled. Else returns false.</returns>
        public static bool IsDataEnabledInDatabase(List<string[]> dbData, List<string> dataValues)
        {
            int n = 0;
            for (int i = 0; i < dbData.Count; i++ )
            {
                for (int j = 0; j < dataValues.Count; j++)
                {
                    if (dbData[i][0].Equals(dataValues[j]))
                    {
                        n++;
                    }
                }
            }

            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
