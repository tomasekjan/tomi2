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
        /// <summary>
        /// symmetric edge
        /// </summary>
        EdgeMultiGraph symetricEdge;        
        int index;
        int u;
        int v;
        int wrapHorizontal = 0;
        int wrapVertical = 0;
        bool triangulateEdge;

        /// <summary>
        /// gets or sets symmetric edge property
        /// </summary>
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
        /// <summary>
        /// gets or sets index of begin vertex
        /// </summary>
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
        /// <summary>
        /// gets or sets index of end vertex
        /// </summary>
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
        /// <summary>
        /// mark if this edge is from original graph or added in triangulation
        /// </summary>
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

        /// <summary>
        /// creates new instance of edge in multi graph
        /// </summary>
        /// <param name="u">first vertex of new edge</param>
        /// <param name="v">second vertex of new edge</param>
        /// <param name="index">index of new edge (two edges with same vertexes need to be unique)</param>
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

        /// <summary>
        /// To sting function just for debbuging reasons
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("({0},{1},{2})", u, v, index);
        }
    }
}
