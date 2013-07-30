using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;
using System.Windows.Forms;

namespace Plugin
{
    class EmbeddingMultiGraph : IEnumerable<EdgeMultiGraph>
    {
        int newEdgeid = 0;
        public int newVertexid = 9999;
        public Dictionary<int, CircularListEdge> incidenceEdges;
        public Dictionary<int, PointF> pozition = new Dictionary<int, PointF>();

        public IEnumerator<EdgeMultiGraph> GetEnumerator()
        {
            foreach (KeyValuePair<int, CircularListEdge> pair in incidenceEdges)
            {
                foreach (EdgeMultiGraph edge in pair.Value)
                {
                    yield return edge;
                }
            }
        }
                
        /// <summary>
        /// creates multi graph embeding from basic embedding
        /// </summary>
        /// <param name="embedding"></param>
        public EmbeddingMultiGraph(Embedding embedding)
        {
            incidenceEdges = new Dictionary<int, CircularListEdge>();
            foreach (int u in embedding.neighbors.Keys)
            {
                incidenceEdges.Add(u, new CircularListEdge());
                foreach (int v in embedding.neighbors[u])
                {
                    incidenceEdges[u].Add(new EdgeMultiGraph(u, v, newEdgeid));
                    newEdgeid++;
                }
            }
            foreach (int u in embedding.neighbors.Keys)
            {                
                foreach (int v in embedding.neighbors[u])
                {
                    int indexuv = embedding.neighbors[u].IndexOf(v);
                    int indexvu = embedding.neighbors[v].IndexOf(u);
                    incidenceEdges[u][indexuv].SymetricEdge = incidenceEdges[v][indexvu];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// to string method - just for debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (EdgeMultiGraph edge in this)
            {
                sb.Append(edge.ToString());
                sb.AppendLine(", ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// gets all faces of curren embedding
        /// </summary>
        /// <returns>list of faces</returns>
        public List<CircularListEdge> GetFaces()
        {
            Dictionary<EdgeMultiGraph, bool> record = new Dictionary<EdgeMultiGraph, bool>();
            int faceCount = 0;
            foreach (EdgeMultiGraph edge in this)
            {
                record.Add(edge, false);
            }            
            List<CircularListEdge> faces = new List<CircularListEdge>();
            foreach (EdgeMultiGraph e in this)
            {               
                if (record[e] == false)
                {
                    faceCount++;
                    EdgeMultiGraph edge = e;
                    CircularListEdge face = new CircularListEdge();
                    while (record[edge] == false)
                    {
                        record[edge] = true;
                        int a = edge.U;
                        int b = edge.V;
                        face.Add(edge);
                        int index = incidenceEdges[b].IndexOf(edge.SymetricEdge);
                        int count = incidenceEdges[b].Count;
                        edge = incidenceEdges[b][(index + 1) % count];
                        
                    }
                    faces.Add(face);
                }
            }
            return faces;
        }

        /// <summary>
        /// Read's algorithm updated for torus
        /// </summary>
        /// <returns>positions of given vertexes</returns>
        public Dictionary<int, PointF> GetPositions()
        {            
            Triangulate();
            Stack<VertexDegree> stack = new Stack<VertexDegree>();
            RemoveLoops(stack);            
            Reduce(stack);
            SetPosition();
            Reinsert(stack);            
            RemoveTriangulation();            
            return pozition;
        }

        /// <summary>
        /// reinsert vertexes deleted in reduction
        /// </summary>
        /// <param name="stack">stack of reduced vertexes</param>
        private void Reinsert(Stack<VertexDegree> stack)
        {
            while (stack.Count != 0)
            {
                VertexDegree vertex = stack.Pop();
                vertex.Add(this);
            }
        }
                
        /// <summary>
        /// triangulate all faces in graph
        /// </summary>
        /// <exception cref="System.ArgumentException">throws when there is multi graph on input instead of graph</exception>
        private void Triangulate()  
        {
            bool run = true;
            while (run)
            {
                run = false;
                List<CircularListEdge> faces = GetFaces();
                foreach (CircularListEdge face in faces)
                {

                    if (face.Count > 3)
                    {
                        run = true;
                        IEnumerable<int> vertexes = from t in face where incidenceEdges[t.U].Count == 2 select face.IndexOf(t);
                        
                        int startIndex = 0;
                        if (vertexes.Count() != 0)
                        {
                            startIndex = vertexes.First();
                        }
                        else
                        {
                            if (face[startIndex].U == face[(startIndex + 2) % face.Count].U)
                            {
                                startIndex++;
                            }
                        }
                        int a = face[startIndex].U;
                        int b = face[(startIndex + 2) % face.Count].U;
                        EdgeMultiGraph first = GetNewEdgeAsym(a, b);
                        EdgeMultiGraph second = GetNewEdgeAsym(b, a);
                        first.TriangulateEdge = true;
                        second.TriangulateEdge = true;
                        first.SymetricEdge = second;
                        second.SymetricEdge = first;
                        incidenceEdges[a].InserBeintwen(first, face[(startIndex - 1 + face.Count) % face.Count].SymetricEdge, face[startIndex]);
                        incidenceEdges[b].InserBeintwen(second, face[(startIndex + 1) % face.Count].SymetricEdge, face[(startIndex + 2) % face.Count]);                        
                    }
                    if (face.Count < 3)
                    {
                        throw new ArgumentException("multi graph on input - not supported");
                    }
                }
            }

        }

        /// <summary>
        /// adding new edge in current embedding
        /// </summary>
        /// <param name="u">first vertex</param>
        /// <param name="v">second vertex</param>
        /// <returns></returns>
        private EdgeMultiGraph GetNewEdgeAsym(int u, int v)
        {
            EdgeMultiGraph tmp = new EdgeMultiGraph(u, v, newEdgeid);
            newEdgeid++;
            return tmp;
        }

        /// <summary>
        /// adding new edge in current embedding with symmetric edge
        /// </summary>
        /// <param name="u">first vertex</param>
        /// <param name="v">second vertex</param>
        /// <returns></returns>
        internal EdgeMultiGraph GetNewEdgeSym(int u, int v)
        {
            EdgeMultiGraph e1 = new EdgeMultiGraph(u, v, newEdgeid);
            newEdgeid++;
            EdgeMultiGraph e2 = new EdgeMultiGraph(v, u, newEdgeid);
            newEdgeid++;
            e1.SymetricEdge = e2;
            e2.SymetricEdge = e1;
            return e1;
        }
        
        /// <summary>
        /// remove edge from embedding
        /// </summary>
        /// <param name="edge"></param>
        public void RemoveEdgeSym(EdgeMultiGraph edge)
        {
            incidenceEdges[edge.U].Remove(edge);
            incidenceEdges[edge.V].Remove(edge.SymetricEdge);
            if (incidenceEdges[edge.U].Count == 0)
            {
                incidenceEdges.Remove(edge.U);
            }
            if (incidenceEdges[edge.V].Count == 0)
            {
                incidenceEdges.Remove(edge.V);
            }
        }
        
        /// <summary>
        /// removes loop of multi graph and calls triangulation again
        /// </summary>
        /// <param name="stack"></param>
        private void RemoveLoops(Stack<VertexDegree> stack)
        {
            bool run = true;
            while (run)
            {
                run = false;
                List<CircularListEdge> faces = GetFaces();
                foreach (CircularListEdge face in faces)
                {
                    if (face[0].U == face[1].U)
                    {
                        Loop l = new Loop();
                        l.Remove(this, face[0].U);
                        stack.Push(l);
                        run = true;
                        break;
                    }
                    if (face[1].U == face[2].U)
                    {
                        Loop l = new Loop();
                        l.Remove(this, face[1].U);
                        stack.Push(l);
                        run = true;
                        break;
                    }
                    if (face[2].U == face[0].U)
                    {
                        Loop l = new Loop();
                        l.Remove(this, face[0].U);
                        stack.Push(l);
                        run = true;
                        break;
                    }
                }
                Triangulate();
            }
        }

        /// <summary>
        /// reducin graph according to read's algorithm
        /// </summary>
        /// <param name="stack"></param>
        private void Reduce(Stack<VertexDegree> stack)
        {            
            bool run = true;
            List<int> vertices = new List<int>();
            foreach (int vertex in incidenceEdges.Keys)
            {
                vertices.Add(vertex);
            }
            while (run)
            {
                run = false;
                int vertexToRemove = -1;

                foreach (int vertex in vertices)
                {
                    if (incidenceEdges[vertex].Count == 3)
                    {
                        run = true;
                        VertexDegree3 vertex3 = new VertexDegree3();
                        vertex3.Remove(this, vertex);
                        vertexToRemove = vertex;
                        stack.Push(vertex3);
                        break;
                    }

                    if (incidenceEdges[vertex].Count == 4)
                    {
                        run = true;
                        VertexDegree4 vertex4 = new VertexDegree4();
                        vertex4.Remove(this, vertex);
                        vertexToRemove = vertex;
                        stack.Push(vertex4);
                        break;
                    }
                    
                    if (incidenceEdges[vertex].Count == 5)
                    {
                        run = true;
                        VertexDegree5 vertex5 = new VertexDegree5();
                        vertex5.Remove(this, vertex);
                        stack.Push(vertex5);
                        vertexToRemove = vertex;
                        break;
                    }                    
                }
                if (vertexToRemove != -1)
                {
                    vertices.Remove(vertexToRemove);
                }
            }
            
        }

        /// <summary>
        /// set positons of basic cases after reduction
        /// </summary>
        private void SetPosition()
        {
            IEnumerable<int> degrees = from a in incidenceEdges.Keys select incidenceEdges[a].Count;
            IEnumerable<int> degree2 = from a in degrees where a == 2 select a;
            if (degree2.Count() == incidenceEdges.Count())
            {
                PointF Midle = new PointF(0.5f, 0.5f);
                float radius = 0.4f;
                float angle = (float)((Math.PI * 2) / incidenceEdges.Count);
                List<int> vertexes = new List<int>();
                foreach (int ver in incidenceEdges.Keys)
                {
                    vertexes.Add(ver);
                }
                for (int i = 0; i < vertexes.Count; i++)
                {
                    float alfa = i *angle;
                    int vertex = vertexes[i];
                    float y = (float)(radius * Math.Sin(alfa));
                    float x = (float)(radius * Math.Cos(alfa));
                    pozition.Add(vertex, new PointF(Midle.X + x, Midle.Y + y));
                }
                return;
            }
            IEnumerable<int> degree6 = from a in degrees where a == 6 select a;
            if (degree6.Count() != incidenceEdges.Count())
            {
                throw new NotImplementedException();
            }
            if (degree6.Count() == 4)
            {
                //t4
                int i = 0;
                int j = 0;
                foreach (int key in incidenceEdges.Keys)
                {

                    if (incidenceEdges[key].Count == 4)
                    {
                        if (i == 0)
                        {
                            pozition.Add(key, new PointF(0.2f, 0.8f));
                            i++;
                        }
                        if (i == 1)
                        {
                            pozition.Add(key, new PointF(0.8f, 0.2f));
                        }
                    }

                    if (incidenceEdges[key].Count == 8)
                    {
                        if (j == 0)
                        {
                            pozition.Add(key, new PointF(0.2f, 0.2f));
                            j++;
                        }
                        if (j == 1)
                        {
                            pozition.Add(key, new PointF(0.8f, 0.8f));
                        }

                    }
                }
            }
            if (degree6.Count() == 3)
            {
                //t3
            }
            // 6 -regular
            List<int> v = new List<int>();
            int v0 = incidenceEdges.Keys.First();
            v.Add(v0);
            v.Add(incidenceEdges[v0][0].V);
            for (int i = 1; i < incidenceEdges.Count; i++)
            {
                int vi = v[i];
                int viLast = v[i - 1];
                int viNew = -1;
                for (int j = 0; j < incidenceEdges[vi].Count; j++)
                {
                    EdgeMultiGraph edge = incidenceEdges[vi][j];
                    if (edge.U == vi && edge.V == viLast)
                    {
                        int index = incidenceEdges[vi].IndexOf(edge);
                        viNew = incidenceEdges[vi][(index + 3) % 6].V;
                        break;
                    }
                }
                if (v.Contains(viNew))
                {
                    break;
                }
                v.Add(viNew);
            }
            List<int> w = new List<int>();
            for (int i = 0; i < v.Count-1; i++)
            {

            }
            int m = v.Count;
        }

        /// <summary>
        /// removing edges added during triangulation
        /// </summary>
        private void RemoveTriangulation()
        {
            List<EdgeMultiGraph> list = new List<EdgeMultiGraph>();
            foreach (EdgeMultiGraph edge in this)
            {
                if (edge.TriangulateEdge)
                {
                    list.Add(edge);
                }
            }
            foreach(EdgeMultiGraph edge in list)
            {
                RemoveEdgeASym(edge);
            }
        }

        private void RemoveEdgeASym(EdgeMultiGraph edge)
        {
            incidenceEdges[edge.U].Remove(edge);            
            if (incidenceEdges[edge.U].Count == 0)
            {
                incidenceEdges.Remove(edge.U);
            }
        }        
    }
}
