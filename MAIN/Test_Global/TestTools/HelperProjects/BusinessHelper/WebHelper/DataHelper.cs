using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCOM.Test.Tools;

namespace MSCOM.BusinessHelper
{
    public class DataHelper
    {
        /// <summary>
        /// Generates a string concatenated with a random number
        /// </summary>
        /// <returns>String concatenated with a random number</returns>
        public static string GenerateRandomString()
        {

            Random r = new Random();
            int n = r.Next();
            string rNo = n.ToString();
            string name = "test" + rNo;

            return name;
        }

        /// <summary>
        /// Generates an user name concatenated with a random number
        /// </summary>
        /// <returns>String concatenated with a random number</returns>
        public static string GenerateRandomUserName()
        {

            Random r = new Random();
            int n = r.Next();
            string rNo = n.ToString();
            string name = "test_User_" + rNo + "@OPXPartner1.onmicrosoft.com";

            return name;
        }

    }
}
