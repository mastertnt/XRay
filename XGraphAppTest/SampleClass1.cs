using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGraphTestApp
{
    class SampleClass1
    {
        public int OutputPortAsInt { get; private set; }

        public int BiPortAsInt { get; set; }

        public int InputPortAsInt { private get; set; }

        public event Action<int> TimeChanged;
    }
}
