using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddInView;
using GraphEditor.GraphDeclaration;
using System.Diagnostics;
using System.Drawing;
using System.Collections;

namespace Plugin
{
    class BridgeWithRespectTo : IComparable<BridgeWithRespectTo>
    {
        BridgType bridgeType;
        Embeding embeding;
        HashSet<int> attachmentVertexes;
        List<CircularListInt> faces;
        
        public BridgType BridgeType
        {
            get
            {
                return bridgeType;
            }
            set
            {
                bridgeType = value;
            }
        }
        public List<CircularListInt> Faces
        {
            get
            {
                return faces;
            }
            set
            {
                faces = value;
            }
        }
        
        public int NumberOfAdmisibleFaces
        {
            get
            {
                return faces.Count;
            }
        }
        public Embeding Embeding
        {
            get
            {
                return embeding;
            }
            set
            {
                embeding = value;
            }
        }
        public HashSet<int> AttachmentVertexes
        {
            get
            {
                return attachmentVertexes;
            }
            set
            {
                attachmentVertexes = value;
            }
        }
        
        public BridgeWithRespectTo()
        {
            attachmentVertexes = new HashSet<int>();
            faces = new List<CircularListInt>();
        }

        public BridgeWithRespectTo(Embeding embeding):this()
        {
            this.embeding = embeding;
            BridgeType = BridgType.Type1;
        }

        public BridgeWithRespectTo(Embeding embeding, Tuple<int, int> edge):this()
        {
            this.embeding = embeding;
            attachmentVertexes.Add(edge.Item1);
            attachmentVertexes.Add(edge.Item2);
            BridgeType = BridgType.Type2;
        }
        public void AddEdgeSym(Tuple<int, int> edge)
        {
            embeding.AddEdgeSym(edge);
        }

        public int CompareTo(BridgeWithRespectTo other)
        {
            return NumberOfAdmisibleFaces.CompareTo(other.NumberOfAdmisibleFaces);
        }

        internal List<int> GetPath()
        {
            switch (BridgeType)
            {
                case BridgType.Type1:
                    path = new List<int>();
                    //this can be done better with some heuristic chose of start an end.
                    int start = attachmentVertexes.First();
                    path.Add(start);
                    visited = new Dictionary<int, bool>();
                    foreach (var v in embeding.neighbors.Keys)
                    {
                        visited.Add(v, false);
                    }
                    visit(start);
                    if (path.Count == 1) return null;
                    break;                    
                case BridgType.Type2:
                    path = new List<int>();
                    path.Add(Embeding.First().Item1);
                    path.Add(Embeding.First().Item2);                    
                    break;
            }
            return path;

        }
        List<int> path;
        Dictionary<int, bool> visited;
        bool end = false;
        private void visit(int u)
        {
            if (end)
            {
                return;
            }
            visited[u] = true;
            foreach (var v in embeding.neighbors[u])
            {
                if (end) return;
                if (!visited[v])
                {
                    path.Add(v);
                    if (attachmentVertexes.Contains(v))
                    {
                        end = true;
                        return;                        
                    }
                    visit(v);
                }
            }
        }
    }
    public enum BridgType
    {
        Type1,
        Type2
    }
}
