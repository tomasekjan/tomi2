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
    class EmbedingMultiGraph : IEnumerable<EdgeMultiGraph>
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
                
        public EmbedingMultiGraph(Embedding embeding)
        {
            incidenceEdges = new Dictionary<int, CircularListEdge>();
            foreach (int u in embeding.neighbors.Keys)
            {
                incidenceEdges.Add(u, new CircularListEdge());
                foreach (int v in embeding.neighbors[u])
                {
                    incidenceEdges[u].Add(new EdgeMultiGraph(u, v, newEdgeid));
                    newEdgeid++;
                }
            }
            foreach (int u in embeding.neighbors.Keys)
            {                
                foreach (int v in embeding.neighbors[u])
                {
                    int indexuv = embeding.neighbors[u].IndexOf(v);
                    int indexvu = embeding.neighbors[v].IndexOf(u);
                    incidenceEdges[u][indexuv].SymetricEdge = incidenceEdges[v][indexvu];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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

        public List<CircularListEdge> GetFaces()
        {
            Dictionary<EdgeMultiGraph, bool> record = new Dictionary<EdgeMultiGraph, bool>();
            int faceCount = 0;
            foreach (EdgeMultiGraph edge in this)
            {
                record.Add(edge, false);
            }
            //h.makeAsymmetric();
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

        public Dictionary<int, PointF> GetPositions()
        {
            List<EdgeMultiGraph> before = new List<EdgeMultiGraph>();
            List<EdgeMultiGraph> after = new List<EdgeMultiGraph>();
            foreach (var edge in this)
            {
                before.Add(new EdgeMultiGraph(edge.U, edge.V, edge.Index));
            }
            Triangulate();
            Stack<VertexDegree> stack = new Stack<VertexDegree>();
            RemoveLoops(stack);            
            Reduce(stack);
            SetPosition();
            Reinsert(stack);            
            RemoveTriangulation();
            
            return pozition;
        }

        private void Reinsert(Stack<VertexDegree> stack)
        {
            while (stack.Count != 0)
            {
                VertexDegree vertex = stack.Pop();
                vertex.Add(this);
            }
        }

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
                        var vertexes = from t in face where incidenceEdges[t.U].Count == 2 select face.IndexOf(t);
                        
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
                        throw new NotImplementedException();
                    }
                }
            }

        }

        public EdgeMultiGraph GetNewEdgeAsym(int u, int v)
        {
            var tmp = new EdgeMultiGraph(u, v, newEdgeid);
            newEdgeid++;
            return tmp;
        }

        public EdgeMultiGraph GetNewEdgeSym(int u, int v)
        {
            var e1 = new EdgeMultiGraph(u, v, newEdgeid);
            newEdgeid++;
            var e2 = new EdgeMultiGraph(v, u, newEdgeid);
            newEdgeid++;
            e1.SymetricEdge = e2;
            e2.SymetricEdge = e1;
            return e1;
        }

        private void AddEdge(Tuple<int, int> tuple, CircularListInt face)
        {
            throw new NotImplementedException();
        }

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
                        run = true;
                        break;
                    }
                    if (face[1].U == face[2].U)
                    {
                        Loop l = new Loop();
                        l.Remove(this, face[1].U);
                        run = true;
                        break;
                    }
                    if (face[2].U == face[0].U)
                    {
                        Loop l = new Loop();
                        l.Remove(this, face[0].U);
                        run = true;
                        break;
                    }
                }
                Triangulate();
            }
        }

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

        private void SetPosition()
        {
            IEnumerable<int> degrees = from a in incidenceEdges.Keys select incidenceEdges[a].Count;
            var degree2 = from a in degrees where a == 2 select a;
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
            var degree6 = from a in degrees where a == 6 select a;
            if (degree6.Count() != incidenceEdges.Count())
            {
                throw new NotImplementedException();
            }
            if (degree6.Count() == 4)
            {
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
            //6-relar
        }

        

        private void RestoreLoops()
        {
            throw new NotImplementedException();
        }

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
