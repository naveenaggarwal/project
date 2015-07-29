using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSCOM.DDA.ENGINE
{
    /// <summary>
    /// This object is to be used by Actions and ExpectedResults (DDAPairs) to track execution result individually. 
    /// </summary>
    public class DDAResult
    {
        public string Note { get; set; }
        public bool IsPassed { get; set; }
        public object Outcome{get; set;}        
    }
}
