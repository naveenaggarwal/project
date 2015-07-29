using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSCOM.Test.MTM
{
    /// <summary>
    /// Holds the exception messages for all iterations of a test case
    /// </summary>
    public class TCIterationResult
    {
        //Local Variables
        public int index;
        private string assertMessages;
        private bool failed;

        //Constructor with initializing variables
        public TCIterationResult()
        {
            index = 1;
            failed = false;
            assertMessages = "";
        }

        /// <summary>
        /// Appends the exception message to string variable
        /// </summary>
        /// <param name="message">Message to be appended. Ideally from Assert result. Any Assert related substring will be removed</param>
        public void AddLog(string message)
        {
            AddLog(message, this.index);
        }

        /// <summary>
        /// Appends the exception message to string variable using the specified Iteration index
        /// </summary>
        /// <param name="message">Message to be appended. Ideally from Assert result. Any Assert related substring will be removed</param>
        /// <param name="index">Iteration index (number) that will be used as part of the error message.</param>
        public void AddLog(string message, int index)
        {
            //If message exists
            assertMessages = assertMessages.Trim().Length == 0 ? assertMessages : assertMessages + " | ";

            //If it is assert failure
            if (message.Contains("Assert.") && message.Contains("failed"))
            {
                assertMessages += "Iteration " + index + " - " + message.Substring(message.IndexOf("failed.") + 8);
            }
            else
            {
                assertMessages += "Iteration " + index + " - " + message;
            }
            failed = true;
        }


        //Resturns the all exception messages string of all test case iterations
        public string Results()
        {
            return assertMessages;
        }

        //Returns the final test case result
        public bool Failed()
        {
            return failed;
        }
    }
}
