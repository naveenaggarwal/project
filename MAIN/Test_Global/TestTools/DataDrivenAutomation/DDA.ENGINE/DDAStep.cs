using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace MSCOM.DDA.ENGINE
{
    public class DDAStep
    {
        public int position;
        public DDAPair action;
        public DDAPair expectedResult;

        public DDAStep(DDAPair action, DDAPair expectedResult, int position)
        {
            this.position = position;
            this.action = action;
            this.expectedResult = expectedResult;
        }

        public bool Passed()
        {
            return expectedResult.result.IsPassed;
        }
    }
}


