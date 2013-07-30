using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddInView;
using GraphEditor.GraphDeclaration;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace Plugin
{
    public struct Vertex2Record
    {
        public int V;
        public int Before;
        public int After;
        public int IndexBefore;
        public int IndexAfter;
    }
}
