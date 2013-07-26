using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using GraphEditor.GraphDeclaration;
using System.Collections;
using System.Drawing;
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
        /// <param name="embeding"></param>
        public Embedding(List<int> comp, Embedding embeding)
        {
            neighbors = new Dictionary<int, CircularListInt>();
            foreach (var v in comp)
            {
                if (embeding.neighbors[v].Count == 0)
                {
                    neighbors.Add(v, new CircularListInt());
                }
            }
            foreach (Tuple<int, int> edge in embeding)
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
        public static List<Embedding> GetK5Embedings(int[] vertexNames)
        {
            Debug.Assert(vertexNames.Length == 5);
            List<Embedding> list = new List<Embedding>();
            Embedding K5Embeding = new Embedding();
            K5Embeding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[3], vertexNames[1], vertexNames[4] }));
            K5Embeding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[0], vertexNames[4], vertexNames[2], vertexNames[3] }));
            K5Embeding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[3], vertexNames[0], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embeding);
            K5Embeding = new Embedding();
            K5Embeding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[3] }));
            K5Embeding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[0], vertexNames[3], vertexNames[2], vertexNames[4] }));
            K5Embeding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[0], vertexNames[4], vertexNames[1] }));
            K5Embeding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[3], vertexNames[0], vertexNames[1], vertexNames[2] }));
            list.Add(K5Embeding);
            K5Embeding = new Embedding();
            K5Embeding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[3], vertexNames[1], vertexNames[4] }));
            K5Embeding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[2], vertexNames[0], vertexNames[4], vertexNames[3] }));
            K5Embeding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[3], vertexNames[4], vertexNames[1], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[0], vertexNames[1], vertexNames[2], vertexNames[4] }));
            K5Embeding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[1], vertexNames[3], vertexNames[0], vertexNames[2] }));
            list.Add(K5Embeding);
            K5Embeding = new Embedding();
            K5Embeding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[4], vertexNames[1], vertexNames[2], vertexNames[3] }));
            K5Embeding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[3], vertexNames[4], vertexNames[2], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[0], vertexNames[2], vertexNames[4], vertexNames[1] }));
            K5Embeding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[0], vertexNames[3], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embeding);
            K5Embeding = new Embedding();
            K5Embeding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[4], vertexNames[1], vertexNames[2], vertexNames[3] }));
            K5Embeding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[3], vertexNames[4], vertexNames[2], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[3], vertexNames[0], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embeding);
            K5Embeding = new Embedding();
            K5Embeding.neighbors.Add(vertexNames[0], new CircularListInt(new int[] { vertexNames[2], vertexNames[3], vertexNames[4], vertexNames[1] }));
            K5Embeding.neighbors.Add(vertexNames[1], new CircularListInt(new int[] { vertexNames[0], vertexNames[4], vertexNames[2], vertexNames[3] }));
            K5Embeding.neighbors.Add(vertexNames[2], new CircularListInt(new int[] { vertexNames[1], vertexNames[4], vertexNames[3], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[3], new CircularListInt(new int[] { vertexNames[2], vertexNames[1], vertexNames[4], vertexNames[0] }));
            K5Embeding.neighbors.Add(vertexNames[4], new CircularListInt(new int[] { vertexNames[0], vertexNames[3], vertexNames[2], vertexNames[1] }));
            list.Add(K5Embeding);
            return list;
        }

        public static List<Embedding> GetK5EmbedingsConstant()
        {
            List<Embedding> list = new List<Embedding>();
            Embedding embeding1 = new Embedding();
            embeding1.neighbors.Add(0, new CircularListInt(new int[] { 2, 3, 1, 4 }));
            embeding1.neighbors.Add(1, new CircularListInt(new int[] { 0, 4, 2, 3 }));
            embeding1.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embeding1.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 4, 0 }));
            embeding1.neighbors.Add(4, new CircularListInt(new int[] { 3, 0, 2, 1 }));
            list.Add(embeding1);
            Embedding embeding2 = new Embedding();
            embeding2.neighbors.Add(0, new CircularListInt(new int[] { 2, 1, 4, 3 }));
            embeding2.neighbors.Add(1, new CircularListInt(new int[] { 0, 3, 2, 4 }));
            embeding2.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embeding2.neighbors.Add(3, new CircularListInt(new int[] { 2, 0, 4, 1 }));
            embeding2.neighbors.Add(4, new CircularListInt(new int[] { 3, 0, 1, 2 }));
            list.Add(embeding2);
            Embedding embeding3 = new Embedding();
            embeding3.neighbors.Add(0, new CircularListInt(new int[] { 2, 3, 1, 4 }));
            embeding3.neighbors.Add(1, new CircularListInt(new int[] { 2, 0, 4, 3 }));
            embeding3.neighbors.Add(2, new CircularListInt(new int[] { 3, 4, 1, 0 }));
            embeding3.neighbors.Add(3, new CircularListInt(new int[] { 0, 1, 2, 4 }));
            embeding3.neighbors.Add(4, new CircularListInt(new int[] { 1, 3, 0, 2 }));
            list.Add(embeding3);
            Embedding embeding4 = new Embedding();
            embeding4.neighbors.Add(0, new CircularListInt(new int[] { 4, 1, 2, 3 }));
            embeding4.neighbors.Add(1, new CircularListInt(new int[] { 3, 4, 2, 0 }));
            embeding4.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embeding4.neighbors.Add(3, new CircularListInt(new int[] { 0, 2, 4, 1 }));
            embeding4.neighbors.Add(4, new CircularListInt(new int[] { 0, 3, 2, 1 }));
            list.Add(embeding4);
            Embedding embeding5 = new Embedding();
            embeding5.neighbors.Add(0, new CircularListInt(new int[] { 4, 1, 2, 3 }));
            embeding5.neighbors.Add(1, new CircularListInt(new int[] { 3, 4, 2, 0 }));
            embeding5.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embeding5.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 4, 0 }));
            embeding5.neighbors.Add(4, new CircularListInt(new int[] { 3, 0, 2, 1 }));
            list.Add(embeding5);
            Embedding embeding6 = new Embedding();
            embeding6.neighbors.Add(0, new CircularListInt(new int[] { 2, 3, 4, 1 }));
            embeding6.neighbors.Add(1, new CircularListInt(new int[] { 0, 4, 2, 3 }));
            embeding6.neighbors.Add(2, new CircularListInt(new int[] { 1, 4, 3, 0 }));
            embeding6.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 4, 0 }));
            embeding6.neighbors.Add(4, new CircularListInt(new int[] { 0, 3, 2, 1 }));
            list.Add(embeding6);
            return list;
        }

        /// <summary>
        /// enumerates all possible non-isomorphic embedding of K{3,3} on torus
        /// </summary>
        /// <returns></returns>
        public static List<Embedding> GetK3_3Embedings()
        {
            List<Embedding> list = new List<Embedding>();
            Embedding embeding1 = new Embedding();
            embeding1.neighbors.Add(0, new CircularListInt(new int[] { 5, 3, 4 }));
            embeding1.neighbors.Add(1, new CircularListInt(new int[] { 4, 5, 3 }));
            embeding1.neighbors.Add(2, new CircularListInt(new int[] { 5, 4, 3 }));
            embeding1.neighbors.Add(3, new CircularListInt(new int[] { 1, 2, 0 }));
            embeding1.neighbors.Add(4, new CircularListInt(new int[] { 2, 1, 0 }));
            embeding1.neighbors.Add(5, new CircularListInt(new int[] { 0, 2, 1 }));
            list.Add(embeding1);
            Embedding embeding2 = new Embedding();
            embeding2.neighbors.Add(0, new CircularListInt(new int[] { 3, 5, 4 }));
            embeding2.neighbors.Add(1, new CircularListInt(new int[] { 3, 5, 4 }));
            embeding2.neighbors.Add(2, new CircularListInt(new int[] { 4, 3, 5 }));
            embeding2.neighbors.Add(3, new CircularListInt(new int[] { 2, 1, 0 }));
            embeding2.neighbors.Add(4, new CircularListInt(new int[] { 1, 0, 2 }));
            embeding2.neighbors.Add(5, new CircularListInt(new int[] { 0, 2, 1 }));
            list.Add(embeding2);
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
            foreach (var v in neighbors.Keys)
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
            foreach (var v in neighbors.Keys)
            {
                if (neighbors[v].Count == 3)
                {
                    count++;
                }
            }
            return count == 6;
        }
        

        internal void RemovePath(List<int> bisectingPath, VertexState vertexStateFirst, VertexState vertexStateSeccond)
        {
            throw new NotImplementedException();
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
                neighbors.Remove(edge.Item1);
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
            foreach (var v in h.neighbors.Keys)
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
            foreach (var v in neighbors.Keys)
            {
                if (neighbors[v].Count == 0)
                {
                    toRemove.Add(v);
                }
            }
            foreach (var v in toRemove)
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
        /// add path to curren embedding
        /// </summary>
        /// <param name="path"></param>
        /// <param name="face"></param>
        /// <param name="vertexStateFirst"></param>
        /// <param name="vertexStateSeccond"></param>
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

        internal void AddPath(List<int> path, CircularListInt face)
        {
            AddPath(path, face, VertexState.FIRST, VertexState.FIRST);
            return;
        }

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
            foreach (var edge in this)
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
        internal List<int> GetK33Vertices()
        {
            List<int> list = new List<int>();
            foreach (var v in neighbors.Keys)
            {
                if (neighbors[v].Count == 4)
                {
                    list.Add(v);
                }
            }
            return list; 
        }

        /// <summary>
        /// returns indexes of vertices witch are isomorphic to K5
        /// </summary>
        /// <returns></returns>
        internal List<int> GetK5Vertices()
        {
            List<int> list = new List<int>();
            foreach (var v in neighbors.Keys)
            {
                if (neighbors[v].Count == 4)
                {
                    list.Add(v);
                }
            }
            return list; 
        }        
    }
}