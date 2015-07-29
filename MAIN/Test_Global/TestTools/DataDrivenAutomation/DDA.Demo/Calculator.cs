using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCOM.DDA.DemoClassLibraries
{
    public class Calculator
    {
        public static double Add(double a, double b)
        {
            return a + b; 
        }

        public static double Multiply(double a, double b)
        {
            return a * b;
        }

        static double Substract(double a, double b)
        {
            return a - b;
        }

        static double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new DDAStepException("Denominator can not be 'zero'");
            }

            return a / b;
        }

        public static bool AreSameBool(bool a, bool b)
        {
            if (a != b)
            {
                throw new DDAStepException(a + " != " + b);
            }
            else
            {
                return true;
            }
        }

        public static bool AreSameDouble(double a, double b)
        {
            if (a != b)
            {
                throw new DDAStepException(a + " != " + b);
            }
            else
            {
                return true;
            }
        }

        static bool AreDifferent(double a, double b)
        {
            if (AreSameDouble(a, b))
            { 
                throw new DDAStepException(a + " == " + b);
            }
            else
            {
                return true;
            }
        }

        public static double AddOnePlusOne()
        {
            return Add(1,1);
        }

        public static double MultTwoTimesTwo()
        {
            return Multiply(2, 2);
        }

        public static bool IsFour(double i)
        {
            return AreSameDouble(i, 4);
        }

    }
}
