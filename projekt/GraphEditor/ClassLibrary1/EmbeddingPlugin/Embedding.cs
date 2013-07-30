using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using System.Diagnostics;

namespace Plugin
{   
    class Embedding : IEnumerable<Tuple<int, int>>
    {        
        public Dictionary<int, CircularListInt> neighbors;

        /// <summary>
        /// creates new embedding from graph definition
        /// </summary>
        /// <param name="sourceDefinition"></param>
        public Embedding(GraphDefinition sourceDefinition)
        {
            neighbors = new Dictionary<int, CircularListInt>();
            
            foreach (Edge edge in sourceDefinition.edges)
            {
                int first = sourceDefinition.vertices.IndexOf(edge.Begin as Vertex);
                int last = sourceDefinition.vertices.IndexOf(edge.End as Vertex);
                if (first != last)
                {
                    AddEdgeSym(new Tuple<int, int>(first, last));
                }
            }
        }

        /// <summary>
        /// creates new embedding from stack of edges
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        public Embedding(Stack<Tuple<int, int>> stack, int u, int v)
        {
            //agreeOrientation = new List<bool>();
            neighbors = new Dictionary<int, CircularListInt>();            
            foreach (Tuple<int, int> pair in stack)
            {
                AddEdgeSym(pair);
                if (pair.Item1 == u && pair.Item2 == v)
                {
                    break;
                }
            }            
        }

        /// <summary>
        /// copy constructor for embedding
        /// </summary>
        /// <param name="embedding"></param>
        public Embedding(Embedding embedding)
        {
            neighbors = new Dictionary<int, CircularListInt>();
            foreach (KeyValuePair<int, CircularListInt> v in embedding.neighbors)
            {
                CircularListInt l = new CircularListInt(v.Value);
                neighbors.Add(v.Key, l);
            }
        }

        /// <summary>
        /// creates empty embedding
        /// </summary>
        public Embedding()
        {
            neighbors = new Dictionary<int, CircularListInt>();
        }

        /// <summary>
        /// creates embedding from cycle definition
        /// </summary>
        /// <param name="cycle"></param>
        public Embedding(List<int> cycle)
        {
            neighbors = new Dictionary<int, CircularListInt>();
            for (int j = 0; j < cycle.Count; j++)
            {
                AddEdgeSym(new Tuple<int,int>(cycle[j],cycle[(j+1) % cycle.Count]));
            }
        }

        /// <summary>
        /// creates embedding with only one edge
        /// </summary>
        /// <param name="edge"></param>
        public Embedding(Tuple<int, int> edge)
        {
            neighbors = new Dictionary<int, CircularListInt>();
            AddEdgeSym(edge);
        }

        /// <summary>
        /// get sub embedding for selected component
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="embedding"></param>
        public Embedding(List<int> comp, Embedding embedding)
        {
            neighbors = new Dictionary<int, CircularListInt>();
            foreach (int v in comp)
            {
                if (embedding.neighbors[v].Count == 0)
                {
                    neighbors.Add(v, new CircularListInt());
                }
            }
            foreach (Tuple<int, int> edge in embedding)
            {
                if (comp.Contains(edge.Item1) && comp.Contains(edge.Item2))
                {
                    AddEdgeAsym(edge);
                }
            }
        }

        /// <summary>
        /// enumerates all possible non-isomorphic embedding of K5 on torus
        /// </summary>
        /// <returns></returns>
        public static List<Embedding> GetK5Embeddings(int[] vertexNames)
        {
            Debug.Assert(vertexNames.Length == 5);
            List<Embedding> list = new List<Embedding>();
            Embedding K5Embedding = new Embedding();
            K5Embedding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[3], vertexNames[1], vertexNames[4] }));
            K5Embedding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[0], vertexNames[4], vertexNames[2], vertexNames[3] }));
            K5Embedding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[3], vertexNames[0], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embedding);
            K5Embedding = new Embedding();
            K5Embedding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[3] }));
            K5Embedding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[0], vertexNames[3], vertexNames[2], vertexNames[4] }));
            K5Embedding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[0], vertexNames[4], vertexNames[1] }));
            K5Embedding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[3], vertexNames[0], vertexNames[1], vertexNames[2] }));
            list.Add(K5Embedding);
            K5Embedding = new Embedding();
            K5Embedding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[3], vertexNames[1], vertexNames[4] }));
            K5Embedding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[2], vertexNames[0], vertexNames[4], vertexNames[3] }));
            K5Embedding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[3], vertexNames[4], vertexNames[1], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[0], vertexNames[1], vertexNames[2], vertexNames[4] }));
            K5Embedding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[1], vertexNames[3], vertexNames[0], vertexNames[2] }));
            list.Add(K5Embedding);
            K5Embedding = new Embedding();
            K5Embedding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[4], vertexNames[1], vertexNames[2], vertexNames[3] }));
            K5Embedding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[3], vertexNames[4], vertexNames[2], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[0], vertexNames[2], vertexNames[4], vertexNames[1] }));
            K5Embedding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[0], vertexNames[3], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embedding);
            K5Embedding = new Embedding();
            K5Embedding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[4], vertexNames[1], vertexNames[2], vertexNames[3] }));
            K5Embedding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[3], vertexNames[4], vertexNames[2], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[3], vertexNames[0], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embedding);
            K5Embedding = new Embedding();
            K5Embedding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[3], vertexNames[4], vertexNames[1] }));
            K5Embedding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[0], vertexNames[4], vertexNames[2], vertexNames[3] }));
            K5Embedding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[0] }));
            K5Embedding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[0], vertexNames[3], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embedding);
            return list;
        }

        public static List<Embedding> GetK5EmbeddingsConstant()
        {
            List<Embedding> list = new List<Embedding>();
            Embedding embedding1 = new Embedding();
            embedding1.neighbors.Add(0, new CircularListInt(new int[] { 2, 3, 1, 4 }));
            embedding1.neighbors.Add(1, new CircularListInt(new int[] { 0, 4, 2, 3 }));
            embedding1.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embedding1.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 4, 0 }));
            embedding1.neighbors.Add(4, new CircularListInt(new int[] { 3, 0, 2, 1 }));
            list.Add(embedding1);
            Embedding embedding2 = new Embedding();
            embedding2.neighbors.Add(0, new CircularListInt(new int[] { 2, 1, 4, 3 }));
            embedding2.neighbors.Add(1, new CircularListInt(new int[] { 0, 3, 2, 4 }));
            embedding2.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embedding2.neighbors.Add(3, new CircularListInt(new int[] { 2, 0, 4, 1 }));
            embedding2.neighbors.Add(4, new CircularListInt(new int[] { 3, 0, 1, 2 }));
            list.Add(embedding2);
            Embedding embedding3 = new Embedding();
            embedding3.neighbors.Add(0, new CircularListInt(new int[] { 2, 3, 1, 4 }));
            embedding3.neighbors.Add(1, new CircularListInt(new int[] { 2, 0, 4, 3 }));
            embedding3.neighbors.Add(2, new CircularListInt(new int[] { 3, 4, 1, 0 }));
            embedding3.neighbors.Add(3, new CircularListInt(new int[] { 0, 1, 2, 4 }));
            embedding3.neighbors.Add(4, new CircularListInt(new int[] { 1, 3, 0, 2 }));
            list.Add(embedding3);
            Embedding embedding4 = new Embedding();
            embedding4.neighbors.Add(0, new CircularListInt(new int[] { 4, 1, 2, 3 }));
            embedding4.neighbors.Add(1, new CircularListInt(new int[] { 3, 4, 2, 0 }));
            embedding4.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embedding4.neighbors.Add(3, new CircularListInt(new int[] { 0, 2, 4, 1 }));
            embedding4.neighbors.Add(4, new CircularListInt(new int[] { 0, 3, 2, 1 }));
            list.Add(embedding4);
            Embedding embedding5 = new Embedding();
            embedding5.neighbors.Add(0, new CircularListInt(new int[] { 4, 1, 2, 3 }));
            embedding5.neighbors.Add(1, new CircularListInt(new int[] { 3, 4, 2, 0 }));
            embedding5.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embedding5.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 4, 0 }));
            embedding5.neighbors.Add(4, new CircularListInt(new int[] { 3, 0, 2, 1 }));
            list.Add(embedding5);
            Embedding embedding6 = new Embedding();
            embedding6.neighbors.Add(0, new CircularListInt(new int[] { 2, 3, 4, 1 }));
            embedding6.neighbors.Add(1, new CircularListInt(new int[] { 0, 4, 2, 3 }));
            embedding6.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embedding6.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 4, 0 }));
            embedding6.neighbors.Add(4, new CircularListInt(new int[] { 0, 3, 2, 1 }));
            list.Add(embedding6);
            return list;
        }

        /// <summary>
        /// enumerates all possible non-isomorphic embedding of K{3,3} on torus
        /// </summary>
        /// <returns></returns>
        public static List<Embedding> GetK3_3Embeddings(int[] vertexNames)
        {
            List<Embedding> list = new List<Embedding>();
            Embedding embeddingA = new Embedding();
            embeddingA.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[5], vertexNames[3], vertexNames[4] }));
            embeddingA.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[4], vertexNames[5], vertexNames[3] }));
            embeddingA.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[5], vertexNames[4], vertexNames[3] }));
            embeddingA.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[1], vertexNames[2], vertexNames[0] }));
            embeddingA.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[0] }));
            embeddingA.neighbors.Add(vertexNames[5], new CircularListInt(new int[] { vertexNames[0], vertexNames[2], vertexNames[1] }));
            list.Add(embeddingA);
            Embedding embeddingB = new Embedding();
            embeddingB.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[3], vertexNames[5], vertexNames[4] }));
            embeddingB.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[3], vertexNames[5], vertexNames[4] }));
            embeddingB.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[4], vertexNames[3], vertexNames[5] }));
            embeddingB.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[0] }));
            embeddingB.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[1], vertexNames[0], vertexNames[2] }));
            embeddingB.neighbors.Add(vertexNames[5], new CircularListInt(new int[] { vertexNames[0], vertexNames[2], vertexNames[1] }));
            list.Add(embeddingB);
            return list;
        }

        /// <summary>
        /// enumerates all possible non-isomorphic embedding of K{3,3} on torus
        /// </summary>
        /// <returns></returns>
        public static List<Embedding> GetK3_3EmbeddingsConstant()
        {
            List<Embedding> list = new List<Embedding>();
            Embedding embedding1 = new Embedding();
            embedding1.neighbors.Add(0, new CircularListInt(new int[] { 5, 3, 4 }));
            embedding1.neighbors.Add(1, new CircularListInt(new int[] { 4, 5, 3 }));
            embedding1.neighbors.Add(2, new CircularListInt(new int[] { 5, 4, 3 }));
            embedding1.neighbors.Add(3, new CircularListInt(new int[] { 1, 2, 0 }));
            embedding1.neighbors.Add(4, new CircularListInt(new int[] { 2, 1, 0 }));
            embedding1.neighbors.Add(5, new CircularListInt(new int[] { 0, 2, 1 }));
            list.Add(embedding1);
            Embedding embedding2 = new Embedding();
            embedding2.neighbors.Add(0, new CircularListInt(new int[] { 3, 5, 4 }));
            embedding2.neighbors.Add(1, new CircularListInt(new int[] { 3, 5, 4 }));
            embedding2.neighbors.Add(2, new CircularListInt(new int[] { 4, 3, 5 }));
            embedding2.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 0 }));
            embedding2.neighbors.Add(4, new CircularListInt(new int[] { 1, 0, 2 }));
            embedding2.neighbors.Add(5, new CircularListInt(new int[] { 0, 2, 1 }));
            list.Add(embedding2);
            return list;
        }
                
        /// <summary>
        /// get all faces in embedding
        /// </summary>
        /// <returns>List of faces in current embedding</returns>
        public List<CircularListInt> GetFaces()
        {
            // algoritm 2.2.
            // faceWalk
            // page 14
            Dictionary<Tuple<int, int>, bool> record = new Dictionary<Tuple<int, int>, bool>();
            int faceCount = 0;
            foreach (Tuple<int, int> edge in this)
            {
                record.Add(edge, false);
            }
            //h.makeAsymmetric();
            List<CircularListInt> faces = new List<CircularListInt>();
            foreach (Tuple<int, int> edge in this)
            {
                if (record[edge] == false)
                {
                    faceCount++;
                    int a = edge.Item1;
                    int b = edge.Item2;
                    CircularListInt face = new CircularListInt();
                    while (record[new Tuple<int, int>(a, b)] == false)
                    {
                        record[new Tuple<int, int>(a, b)] = true;
                        face.Add(a);
                        int index = neighbors[b].IndexOf(a);
                        int count = neighbors[b].Count;
                        int c = neighbors[b][(index + 1) % count];
                        a = b;
                        b = c;
                    }
                    faces.Add(face);
                }
            }
            return faces;
        }

        /// <summary>
        /// checks if current graph is isomorphic to K5
        /// </summary>
        /// <returns></returns>
        internal bool IsK5()
        {
            int count = 0;
            foreach (int v in neighbors.Keys)
            {
                if (neighbors[v].Count == 4)
                {
                    count++;
                }
            }
            return count == 5;
        }

        /// <summary>
        /// checks if current graph is isomorphic to K{3,3}
        /// </summary>
        /// <returns></returns>
        internal bool IsK3_3()
        {
            int count = 0;
            foreach (int v in neighbors.Keys)
            {
                if (neighbors[v].Count == 3)
                {
                    count++;
                }
            }
            return count == 6;
        }
        
        public IEnumerator<Tuple<int, int>> GetEnumerator()
        {
            foreach (int u in neighbors.Keys)
            {
                foreach (int v in neighbors[u])
                {
                    //if (u < v)
                    //{
                        yield return new Tuple<int, int>(u, v);
                    //}
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        /// <summary>
        /// remove edge from embedding (not deleting symmetric edge)
        /// </summary>
        /// <param name="edge"></param>
        internal void RemoveEdgeASym(Tuple<int, int> edge)
        {
            neighbors[edge.Item1].Remove(edge.Item2);   
        }

        /// <summary>
        /// remove edge from embedding with deleting symmetric edge
        /// </summary>
        /// <param name="edge"></param>
        internal void RemoveEdgeSym(Tuple<int, int> edge)
        {

            neighbors[edge.Item1].Remove(edge.Item2);
            neighbors[edge.Item2].Remove(edge.Item1);
            if (neighbors[edge.Item1].Count == 0)
            {
                neighbors.Remove(edge.Item1);
            }
            if (neighbors[edge.Item2].Count == 0)
            {
                neighbors.Remove(edge.Item2);
            }
        }

        /// <summary>
        /// add edge to embedding with creating 
        /// </summary>
        /// <param name="edge"></param>
        /// <exception cref="System.ArgumentException">throws then you try to add loop to graph</exception>
        internal void AddEdgeSym(Tuple<int, int> edge)
        {
            if (edge.Item1 == edge.Item2) throw new ArgumentException("does not support adding loops");
            if (!neighbors.ContainsKey(edge.Item1))
            {
                neighbors.Add(edge.Item1, new CircularListInt());
            }
            if (!neighbors.ContainsKey(edge.Item2))
            {
                neighbors.Add(edge.Item2, new CircularListInt());
            }
            if (!ContaintsEdgeAsym(edge))
            {
                neighbors[edge.Item1].Add(edge.Item2);
                neighbors[edge.Item2].Add(edge.Item1);
            }

        }

        /// <summary>
        /// add edge without adding symmetric edge
        /// </summary>
        /// <param name="edge"></param>
        /// <exception cref="System.ArgumentException">throws then you try to add loop to graph</exception>
        internal void AddEdgeAsym(Tuple<int, int> edge)
        {
            if (edge.Item1 == edge.Item2) throw new ArgumentException("does not support adding loops");
            if (!neighbors.ContainsKey(edge.Item1))
            {
                neighbors.Add(edge.Item1, new CircularListInt());
            }            
            neighbors[edge.Item1].Add(edge.Item2);            
        }               
        
        /// <summary>
        /// subtracts embedding h from current embedding
        /// </summary>
        /// <param name="h"></param>
        internal void Minus(Embedding h)
        {
            List<Tuple<int, int>> toRemove = new List<Tuple<int, int>>();
            foreach (Tuple<int, int> edge in this)
            {
                if (h.neighbors.ContainsKey(edge.Item1) || h.neighbors.ContainsKey(edge.Item2))
                {
                    toRemove.Add(new Tuple<int, int>(edge.Item1, edge.Item2));
                }
            }
            foreach (Tuple<int, int> edge in toRemove)
            {
                RemoveEdgeASym(edge);
            }
            foreach (int v in h.neighbors.Keys)
            {
                neighbors.Remove(v);
            }
        }

        /// <summary>
        /// checks if embedding contains given edge
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool ContaintsEdgeAsym(Tuple<int, int> edge)
        {
            return neighbors[edge.Item1].Contains(edge.Item2);
        }

        /// <summary>
        /// returns true if embedding is empty
        /// </summary>
        /// <returns></returns>
        internal bool isEmpty()
        {
            //RemoveEmpty();
            return neighbors.Keys.Count == 0;
        }

        /// <summary>
        /// removes empty vertexes from embedding
        /// </summary>
        private void RemoveEmpty()
        {
            List<int> toRemove = new List<int>();
            foreach (int v in neighbors.Keys)
            {
                if (neighbors[v].Count == 0)
                {
                    toRemove.Add(v);
                }
            }
            foreach (int v in toRemove)
            {
                neighbors.Remove(v);
            }
        }

        /// <summary>
        /// return true if embedding contains given vertex
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        internal bool ConteintsVertex(int vertex)
        {
            return neighbors.ContainsKey(vertex);
        }

        /// <summary>
        /// add path to current embedding
        /// </summary>
        /// <param name="path">list of integers representing path</param>
        /// <param name="face">face to insert path into</param>
        /// <param name="vertexStateFirst">determines whether use first or second copy of vertex on face</param>
        /// <param name="vertexStateSeccond">determines whether use first or second copy of vertex on face</param>
        /// <returns></returns>
        internal bool AddPath(List<int> path, CircularListInt face, VertexState vertexStateFirst, VertexState vertexStateSeccond)
        {
            if (path.Count < 2)
            {
                throw new NotImplementedException();
            }
            int first = path[0];
            int last = path[path.Count - 1];
            int beforeFirst = -1;
            int afterFirst = -1;
            int beforeLast = -1;
            int afterLast = -1;
            if (vertexStateFirst == VertexState.FIRST && vertexStateSeccond == VertexState.FIRST)
            {
                beforeFirst = face.GetBeforeValue(first);
                afterFirst = face.GetAfterValue(first);
                beforeLast = face.GetBeforeValue(last);
                afterLast = face.GetAfterValue(last);                
            }
            if (vertexStateFirst == VertexState.FIRST && vertexStateSeccond == VertexState.SECOND)
            {
                beforeFirst = face.GetBeforeValue(first);
                afterFirst = face.GetAfterValue(first);
                beforeLast = face.GetBeforeSecondValue(last);
                afterLast = face.GetAfterSecondValue(last);
                if (beforeLast == -1 || afterLast == -1)
                {
                    return false;
                }
            }
            if (vertexStateFirst == VertexState.SECOND && vertexStateSeccond == VertexState.FIRST)
            {
                beforeFirst = face.GetBeforeSecondValue(first);
                afterFirst = face.GetAfterSecondValue(first);
                beforeLast = face.GetBeforeValue(last);
                afterLast = face.GetAfterValue(last);
                if (beforeFirst == -1 || afterFirst == -1)
                {
                    return false;
                }
            }
            if (vertexStateFirst == VertexState.SECOND && vertexStateSeccond == VertexState.SECOND)
            {
                beforeFirst = face.GetBeforeSecondValue(first);
                afterFirst = face.GetAfterSecondValue(first);
                beforeLast = face.GetBeforeSecondValue(last);
                afterLast = face.GetAfterSecondValue(last);
                if (beforeFirst == -1 || afterFirst == -1)
                {
                    return false;
                }
                if (beforeLast == -1 || afterLast == -1)
                {
                    return false;
                }
            }
            neighbors[first].InserBeintwen(path[1], beforeFirst, afterFirst);
            if (neighbors.ContainsKey(last))
            {
                neighbors[last].InserBeintwen(path[path.Count - 2], beforeLast, afterLast);
            }
            else
            {
                neighbors.Add(last, new CircularListInt());
                neighbors[last].Add(path[path.Count - 2]);
            }
            if (path.Count != 2)
            {
                AddEdgeAsym(new Tuple<int, int>(path[path.Count - 2], last));
                AddEdgeAsym(new Tuple<int, int>(path[1], first));
            }
            for (int i = 1; i < path.Count - 2; i++)
            {
                AddEdgeSym(new Tuple<int, int>(path[i], path[i + 1]));
            }
            return true;
        }

        /// <summary>
        /// add path to current embedding
        /// </summary>
        /// <param name="path">list of integers representing path</param>
        /// <param name="face">face to insert path into</param>>
        internal void AddPath(List<int> path, CircularListInt face)
        {
            AddPath(path, face, VertexState.FIRST, VertexState.FIRST);
            return;
        }

        /// <summary>
        /// add edge to current embedding
        /// </summary>
        /// <param name="edge">edge to insert</param>
        /// <param name="face">face to insert edge into</param>
        internal void AddEdge(Tuple<int, int> edge, CircularListInt face)
        {
            List<int> path = new List<int>();
            path.Add(edge.Item1);
            path.Add(edge.Item2);
            AddPath(path, face);
        }
        
        /// <summary>
        /// remove path from embedding
        /// </summary>
        /// <param name="path"></param>
        internal void RemovePath(List<int> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                RemoveEdgeSym(new Tuple<int, int>(path[i], path[i + 1]));
            }
        }

        /// <summary>
        /// to string value -  for debugging reasons
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int vertex in neighbors.Keys)
            {
                sb.Append(vertex.ToString());
                sb.Append(", ");
            }
            sb.Append(string.Format("Number of edges : {0}, ",this.Count()));
            foreach (Tuple<int, int> edge in this)
            {
                if (edge.Item1 <= edge.Item2)
                {                    
                    if(!ContaintsEdgeAsym(new Tuple<int,int>(edge.Item2,edge.Item1)))
                    {
                        sb.Append("data consistensi error");
                    }
                    sb.Append(string.Format("{0} <-> {1}", edge.Item1, edge.Item2));
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// returns indexes of vertices witch are isomorphic to K{3,3}
        /// </summary>
        /// <returns></returns>
        internal int[][] GetK33Vertices()
        {
            int[][] returArray = new int[2][];
            returArray[0] = new int[3];
            returArray[1] = new int[3];
            int vertexFirsPartity = -1;
            int indexFrist = 0;
            int indexSecond = 0;
            foreach (int v in neighbors.Keys)
            {
                if (neighbors[v].Count == 3)
                {
                    if (vertexFirsPartity == -1)
                    {
                        vertexFirsPartity = v;
                        returArray[0][indexFrist] = v;
                        indexFrist++;
                    }
                    else
                    {
                        if (neighbors[vertexFirsPartity].Contains(v))
                        {
                            returArray[1][indexSecond] = v;
                            indexSecond++;
                        }
                        else 
                        {
                            returArray[0][indexFrist] = v;
                            indexFrist++;
                        }
                    }                    
                }
            }
            return returArray;
        }

        /// <summary>
        /// returns indexes of vertices witch are isomorphic to K5
        /// </summary>
        /// <returns></returns>
        internal List<int> GetK5Vertices()
        {
            List<int> list = new List<int>();
            foreach (int v in neighbors.Keys)
            {
                if (neighbors[v].Count == 4)
                {
                    list.Add(v);
                }
            }
            return list; 
        }

        /// <summary>
        /// removes points of degree 2 from graph isomorphic co k5 or k3_3
        /// </summary>
        /// <returns>stack of deleted vertexes</returns>
        internal Stack<Vertex2Record> getSubPoints()
        {
            Stack<Vertex2Record> stack = new Stack<Vertex2Record>();
            bool run = true;
            while (run)
            {
                run = false;
                foreach (int v in neighbors.Keys)
                {
                    if (neighbors[v].Count == 2)
                    {
                        run = true;
                        stack.Push(removeVertex2(v));
                        break;
                    }
                }
            }
            return stack;
        }

        private Vertex2Record removeVertex2(int v)
        {
            Vertex2Record record = new Vertex2Record();
            record.V = v;
            record.Before = neighbors[v][0];
            record.After = neighbors[v][1];
            record.IndexBefore = neighbors[record.Before].IndexOf(v);
            record.IndexAfter = neighbors[record.After].IndexOf(v);
            RemoveEdgeSym(new Tuple<int, int>(v, record.Before));
            RemoveEdgeSym(new Tuple<int, int>(v, record.After));
            neighbors[record.Before].Insert(record.IndexBefore, record.After);
            neighbors[record.After].Insert(record.IndexAfter, record.Before);
            return record;
        }

        private void insertVertex2(Vertex2Record record)
        {
            RemoveEdgeSym(new Tuple<int, int>(record.Before, record.After));
            neighbors.Add(record.V, new CircularListInt(new int[] {record.Before, record.After}));
            neighbors[record.Before].Insert(record.IndexBefore, record.V);
            neighbors[record.After].Insert(record.IndexAfter, record.V);
        }

        /// <summary>
        /// reinserting vertexes of degree 2 back into embedding of k5 or k3_3
        /// </summary>
        /// <param name="subpoints">stack of deleted vertexes</param>
        internal void InsertSubPoints(Stack<Vertex2Record> subpoints)
        {
            foreach (Vertex2Record record in subpoints)
            {
                insertVertex2(record);
            }
        }
    }
}