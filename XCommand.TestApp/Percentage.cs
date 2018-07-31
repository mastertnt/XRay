using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandTest
{
    public class Percentage
    {
        public Percentage(double pValue)
        {
            this.Value = pValue;
        }

        public double Value
        {
            get;
            private set;
        }
    }
}
