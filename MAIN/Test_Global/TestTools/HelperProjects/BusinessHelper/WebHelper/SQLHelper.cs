﻿using System;
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
        /// <returns>Results of this query.</returns>
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
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

        }

        /// <summary>
        /// Helper method to run a DML query. User current user's credentials.
        /// </summary>
        /// <param name="query">String containing the query.</param>
        public static void RunDMLQuery(string query)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);

            List<string> resultSetRow = new List<string>();
            List<string[]> result = new List<string[]>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

        }

        /// <summary>
        /// Helper method to run a query and return the result
        /// </summary>
        /// <param name="query">String containing the query</param>
        /// <returns>Result of the query</returns>
        public static string RunQueryAndReturnResult(string query)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);
            string result = null;
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                result = reader.GetValue(0).ToString();
                            }
                        }
                    }
                    conn.Close();
                }

                if (result == null)
                {
                    throw new Exception(string.Format("Unable to fetch any data for the query '{0}'.", query));
                }
                else
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }
        }

        /// <summary>
        /// Helper method to run a query and return the result
        /// </summary>
        /// <param name="query">String containing the query</param>
        /// <returns>Result of the query</returns>
        public static List<string> RunQueryAndReturnResults(string query)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);
            List<string> result = new List<string>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(reader.GetValue(0).ToString());
                            }
                        }
                    }
                    conn.Close();
                }

                if (result == null)
                {
                    throw new Exception(string.Format("Unable to fetch any data for the query '{0}'.", query));
                }
                else
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }
        }

        /// <summary>
        /// Helper method to run a query and compares the result
        /// </summary>
        /// <param name="query">String containing the query</param>
        /// <param name="value">value to be compared</param>
        /// <returns>True if the values match. Else throws an Exception.</returns>
        public static bool RunQueryAndCompareResult(string query, string value)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);
            string result = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                result = reader.GetValue(0).ToString();
                            }
                        }
                    }
                    conn.Close();
                }

                if (result == value || result.Contains(value))
                {
                    return true;
                }
                else
                {
                    throw new Exception(string.Format("The data returned after executing the query did not match with '{0}'.", value));
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }
        }

        /// <summary>
        /// Helper method to run a query and compares the result
        /// </summary>
        /// <param name="query">String containing the query</param>
        /// <param name="paramValue">parameter value which needs to be concatenated with the query</param>
        /// <param name="value">value to be compared</param>
        /// <returns>True if the values match. Else throws an Exception.</returns>
        public static bool RunQueryAndCompareResult(string query, string paramValue, string value)
        {
            string modifiedQuery = query + "'" + paramValue + "'";
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);
            string result = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(modifiedQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                result = reader.GetValue(0).ToString();
                            }
                        }
                    }
                    conn.Close();
                }

                if (result == value || result.Contains(value))
                {
                    return true;
                }
                else
                {
                    throw new Exception(string.Format("The data returned after executing the query did not match with '{0}'.", modifiedQuery));
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }
        }

        /// <summary>
        /// Helper method to run a query and return the date result
        /// </summary>
        /// <param name="query">String containing the query</param>
        /// <returns>the value of the radio button which has been selected</returns>
        public static string RunQueryAndReturnDateResult(string query)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);
            string result = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                result = reader.GetValue(0).ToString();
                            }
                        }
                    }
                    conn.Close();
                }

                if (result != null)
                {
                    return "Expires";
                }
                else
                {
                    return "Never";
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }
        }

        /// <summary>
        /// Helper method to run two queries and return a string by concatenating the results of the queries.
        /// </summary>
        /// <param name="query1">String containing the first query</param>
        /// <param name="query2">String containing the second query</param>
        /// <returns>String by concatenating the results of the queries.</returns>
        public static string RunQuery(string query1, string query2)
        {
            var connString = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", ServerName, DatabaseName, UserName, Password);

            List<string> resultSetRow1 = new List<string>();
            List<string[]> result1 = new List<string[]>();
            List<string> resultSetRow2 = new List<string>();
            List<string[]> result2 = new List<string[]>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                    {
                        using (SqlDataReader reader1 = cmd1.ExecuteReader())
                        {
                            for (int i = 0; i < reader1.FieldCount; i++)
                            {
                                resultSetRow1.Add(reader1.GetName(i).ToString());
                            }

                            result1.Add(resultSetRow1.ToArray());
                            resultSetRow1.Clear();

                            while (reader1.Read())
                            {
                                Object[] values = new Object[reader1.FieldCount];
                                reader1.GetValues(values);
                                foreach (Object value in values)
                                {
                                    resultSetRow1.Add(value.ToString());
                                }
                                result1.Add(resultSetRow1.ToArray());
                                resultSetRow1.Clear();
                            }
                        }
                    }
                    using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                    {
                        using (SqlDataReader reader2 = cmd2.ExecuteReader())
                        {
                            for (int i = 0; i < reader2.FieldCount; i++)
                            {
                                resultSetRow2.Add(reader2.GetName(i).ToString());
                            }

                            result2.Add(resultSetRow2.ToArray());
                            resultSetRow2.Clear();

                            while (reader2.Read())
                            {
                                Object[] values = new Object[reader2.FieldCount];
                                reader2.GetValues(values);
                                foreach (Object value in values)
                                {
                                    resultSetRow2.Add(value.ToString());
                                }
                                result2.Add(resultSetRow2.ToArray());
                                resultSetRow2.Clear();
                            }
                        }
                    }
                    conn.Close();
                }

            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

            return CreateCountString(result1[1][0], result2[1][0]);
        }

        /// <summary>
        /// Helper method to run and compare the database and UI values
        /// </summary>
        /// <param name="query">String containing the query.</param>
        /// <param name="dataValues">Values fetched from the UI</param>
        /// <returns>Results of this query.</returns>
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

            }
            catch (Exception e)
            {
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

            if (IsDataEnabledInDatabase(result, dataValues))
            {
                return result;
            }
            else
            {
                throw new Exception("The values are not enabled in the database.");
            }
        }

        /// <summary>
        /// Helper method to run and compare the database and UI values
        /// </summary>
        /// <param name="query">String containing the query.</param>
        /// <param name="dataValues">Value fetched from the UI</param>
        /// <returns>Results of this query.</returns>
        public static List<string[]> RunQueryAndCompare(string query, string dataValue)
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
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

            if (IsDataEnabledInDatabase(result, dataValue))
            {
                return result;
            }
            else
            {
                throw new Exception("The value did not match in the database.");
            }
        }

        /// <summary>
        /// Helper method to run a query and compare the results
        /// </summary>
        /// <param name="query">String containing the query.</param>
        /// <param name="value1">data which is added through UI</param>
        /// <param name="value2">data which is added through UI</param>
        /// <returns>Results of this query.</returns>
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
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

            if (IsDataAddedToDatabase(result, value1, value2))
            {
                return result;
            }
            else
            {
                throw new Exception(string.Format("The values '{0}' and '{1}' were not mapped to the database.", value1, value2));
            }
        }

        /// <summary>
        /// Helper method to run a query and compare the results
        /// </summary>
        /// <param name="query">String containing the query.</param>
        /// <param name="value1">data which is added through UI</param>
        /// <param name="value2">data which is added through UI</param>
        /// <returns>Results of this query.</returns>
        public static List<string[]> RunQueryAndCompareIfDataIsUpdated(string query, string value1, string value2)
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
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

            if (IsDataUpdatedInDatabase(result, value1, value2))
            {
                return result;
            }
            else
            {
                throw new Exception(string.Format("The values '{0}' and '{1}' were not updated in the database.", value1, value2));
            }
        }

        /// <summary>
        /// Helper method to run a query and compare the results
        /// </summary>
        /// <param name="query">String containing the query.</param>
        /// <param name="value1">data which is added through UI</param>
        /// <param name="value2">data which is added through UI</param>
        /// <param name="value3">data which is added through UI</param>
        /// <returns>Results of this query.</returns>
        public static List<string[]> RunQueryAndCompareIfDataIsUpdated(string query, string value1, string value2, string value3)
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
                throw new Exception(string.Format("SQLHelper was unable to complete the execution of the Query. Error: '{0}'", e.Message));
            }

            if (IsDataUpdatedInDatabase(result, value1, value2, value3))
            {
                return result;
            }
            else
            {
                throw new Exception(string.Format("The values '{0}' and '{1}' were not updated in the database.", value1, value2));
            }
        }

        /// <summary>
        /// Concatenates the results of the SQL queries in a desired format
        /// </summary>
        /// <param name="value1">Result of the first SQL query</param>
        /// <param name="value2">Result of the second SQL query</param>
        /// <returns>Concatenated string</returns>
        private static string CreateCountString(string value1, string value2)
        {
            string countString = value1 + " of " + value2;
            return countString;
        }

        /// <summary>
        /// Compares the results returned from the query with the values provided
        /// </summary>
        /// <param name="dbData">SQL query result</param>
        /// <param name="data1">data which is added through UI</param>
        /// <param name="data2">data which is added through UI</param>
        /// <returns>True if the data is added. Else returns false.</returns>
        private static bool IsDataAddedToDatabase(List<string[]> dbData, string data1, string data2)
        {
            int count = 0;
            for (int i = 0; i < dbData.Count; i++)
            {
                if (dbData[i][0].Equals(data1) || dbData[i][0].Equals(data2))
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
        /// Compares the results returned from the query with the values provided
        /// </summary>
        /// <param name="dbData">SQL query result</param>
        /// <param name="data1">data which is added through UI</param>
        /// <param name="data2">data which is added through UI</param>
        /// <returns>True if the data is updated. Else returns false.</returns>
        private static bool IsDataUpdatedInDatabase(List<string[]> dbData, string data1, string data2)
        {
            int count = 0;
            for (int i = 0; i < dbData.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (dbData[i][j].Equals(data1) || dbData[i][j].Equals(data2))
                    {
                        count++;
                    }
                    else if (count == 2)
                    {
                        return true;
                    }
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
        /// Compares the results returned from the query with the values provided
        /// </summary>
        /// <param name="dbData">SQL query result</param>
        /// <param name="data1">data which is added through UI</param>
        /// <param name="data2">data which is added through UI</param>
        /// <param name="data3">data which is added through UI</param>
        /// <returns>True if the data is updated. Else returns false.</returns>
        private static bool IsDataUpdatedInDatabase(List<string[]> dbData, string data1, string data2, string data3)
        {
            int count = 0;
            for (int i = 0; i < dbData.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (dbData[i][j].Equals(data1) || dbData[i][j].Equals(data2) || dbData[i][j].Equals(data3))
                    {
                        count++;
                    }
                    else if (count == 3)
                    {
                        return true;
                    }
                }
            }

            if (count == 3)
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
        private static bool IsDataEnabledInDatabase(List<string[]> dbData, List<string> dataValues)
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

        /// <summary>
        /// Checks whether the data is enabled in the database
        /// </summary>
        /// <param name="dbData">SQL query result</param>
        /// <param name="dataValues">data retrieved from the element</param>
        /// <returns>True if data is enabled. Else returns false.</returns>
        private static bool IsDataEnabledInDatabase(List<string[]> dbData, string dataValue)
        {
            int n = 0;
            for (int i = 0; i < dbData.Count; i++)
            {
               if (dbData[i][0].Equals(dataValue))
               {
                   n++;
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
