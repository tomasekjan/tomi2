using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;

namespace Plugin
{
    class EdgeMultiGraph
    {
        //opozite edge
        EdgeMultiGraph symetricEdge;        
        int index;
        int u;
        int v;
        int wrapHorizontal = 0;
        int wrapVertical = 0;
        bool triangulateEdge;

        public EdgeMultiGraph SymetricEdge
        {
            get
            {
                return symetricEdge;
            }
            set
            {
                symetricEdge = value;
            }
        }
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }
        public int U
        {
            get
            {
                return u;
            }
            set
            {
                u = value;
            }
        }
        public int V
        {
            get
            {
                return v;
            }
            set
            {
                v = value;
            }
        }
        public int WrapHorizontal
        {
            get
            {
                return wrapHorizontal;
            }
            set
            {
                wrapHorizontal = value;
            }
        }
        public int WrapVertical
        {
            get
            {
                return wrapVertical;
            }
            set
            {
                wrapVertical = value;
            }
        }
        public bool TriangulateEdge
        {
            get
            {
                return triangulateEdge;
            }
            set
            {
                triangulateEdge = value;
            }
        }

        public EdgeMultiGraph(int u, int v, int index)            
        {
            this.u = u;
            this.v = v;
            this.index = index;
        }
        
        public override bool Equals(object obj)
        {
            EdgeMultiGraph other = obj as EdgeMultiGraph;
            return (u == other.u) && (v == other.v) && (index == other.index);
        }
        
        public override int GetHashCode()
        {
            return index;
        }

        public override string ToString()
        {
            return String.Format("({0},{1},{2})", u, v, index);
        }
    }
}
