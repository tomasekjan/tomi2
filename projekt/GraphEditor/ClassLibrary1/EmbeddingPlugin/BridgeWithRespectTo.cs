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
    /// <summary>
    /// class representing bridge with respect to according to "A Faster Algorithm for Torus Embedding" [12]
    /// </summary>
    class BridgeWithRespectTo : IComparable<BridgeWithRespectTo>
    {
        /// <summary>
        /// type of bridge according to "A Faster Algorithm for Torus Embedding" [12]
        /// </summary>
        BridgType bridgeType;
        /// <summary>
        /// graph declaration of bridge
        /// </summary>
        Embedding bridge;
        /// <summary>
        /// vertexes that is bridge attached to in embedding
        /// </summary>
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
        public Embedding Embeding
        {
            get
            {
                return bridge;
            }
            set
            {
                bridge = value;
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
        
        /// <summary>
        /// creating empty bridge
        /// </summary>
        public BridgeWithRespectTo()
        {
            attachmentVertexes = new HashSet<int>();
            faces = new List<CircularListInt>();
        }

        /// <summary>
        /// bridge of type 1
        /// </summary>
        /// <param name="embeding"></param>
        public BridgeWithRespectTo(Embedding embeding):this()
        {
            this.bridge = embeding;
            BridgeType = BridgType.Type1;
        }

        /// <summary>
        /// bridge of type 2
        /// </summary>
        /// <param name="embeding"></param>
        /// <param name="edge"></param>
        public BridgeWithRespectTo(Embedding embeding, Tuple<int, int> edge):this()
        {
            this.bridge = embeding;
            attachmentVertexes.Add(edge.Item1);
            attachmentVertexes.Add(edge.Item2);
            BridgeType = BridgType.Type2;
        }

        /// <summary>
        /// add symmetric edge to bridge
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdgeSym(Tuple<int, int> edge)
        {
            bridge.AddEdgeSym(edge);
        }

        /// <summary>
        /// compering two bridges by Number of Admissible faces
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(BridgeWithRespectTo other)
        {
            return NumberOfAdmisibleFaces.CompareTo(other.NumberOfAdmisibleFaces);
        }

        /// <summary>
        /// get path from bridge witch can be embedded
        /// </summary>
        /// <returns></returns>
        internal List<int> GetPath()
        {
            switch (BridgeType)
            {
                case BridgType.Type1:
                    path = new List<int>();
                    queue = new Queue<int>();
                    parrent = new Dictionary<int, int>();
                    enqueued = new HashSet<int>();

                    int start = attachmentVertexes.First();                    
                    enqueued.Add(start);
                    
                    enqueued.Add(start);
                    foreach (int v in bridge.neighbors[start])
                    {
                        queue.Enqueue(v);
                        enqueued.Add(v);
                        parrent.Add(v, start);
                    }
                    int end = search();
                    if (end == -1)
                    {
                        path.Add(start);
                        path.Add(bridge.neighbors[start][0]);
                    }
                    else
                    {
                        extractPath(start, end);
                    }
                    break;                    
                case BridgType.Type2:
                    path = new List<int>();
                    path.Add(Embeding.First().Item1);
                    path.Add(Embeding.First().Item2);                    
                    break;
            }
            return path;

        }

        private void extractPath(int start, int end)
        {
            int v = end;
            while (true)
            {
                path.Add(v);
                if (v == start)
                {
                    break;
                }
                v = parrent[v];
            }
        }

        private int search()
        {
            while (true)
            {
                if (queue.Count == 0)
                {
                    return -1;
                }
                int v = queue.Dequeue();                
                if (attachmentVertexes.Contains(v))
                {
                    return v;
                }
                foreach (int u in bridge.neighbors[v])
                {
                    if (!enqueued.Contains(u))
                    {
                        queue.Enqueue(u);
                        enqueued.Add(u);
                        parrent.Add(u, v);
                    }
                }
            }            
        }
        List<int> path;
        HashSet<int> enqueued;
        Dictionary<int, int> parrent;
        Queue<int> queue;
    }
}
