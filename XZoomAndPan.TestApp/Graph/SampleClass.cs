﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZoomAndPan.TestApp.Graph
{
    class SampleClass
    {
        public int OutputPortAsInt { get { return 0; } }

        public int BiPortAsInt { get; set; }

        public int InputPortAsInt { set { } }

        public event Action<int> TimeChanged;
    }
}
