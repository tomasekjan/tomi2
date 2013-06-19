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
    public class GraphEmbeddingExponentioal : IPozitioning
    {
        GraphDefinition tmp;
        public override GraphDefinition Pozitioning(GraphDefinition sourceDefinition)
        {            
            tmp = sourceDefinition;
            Embeding embeding = new Embeding(sourceDefinition);
            Embeding embeding2 = GetSphereEmbeding(embeding);
            EmbedingMultiGraph multi = new EmbedingMultiGraph(embeding2);
            Dictionary<int, PointF> pos = multi.GetPositions();            
            foreach (var item in pos)
            {
                tmp.vertices[item.Key].Pozition = item.Value;
            }
            return tmp;
            switch (sourceDefinition.suraceType)
            {
                case SurfaceTypeEnum.Sphere:
                    return GetSpherePozitioning(embeding);
                case SurfaceTypeEnum.Torus:
                    return GetTorusPozitioning(embeding);
                default:
                    throw new ArgumentException();
            }
        }
        
        private GraphDefinition GetTorusPozitioning(Embeding embeding)
        {
            Embeding embeding2 = GetTorusEmbeding(embeding);
            if (embeding2 != null)
            {
                MessageBox.Show("ano");
            }
            else
            {
                MessageBox.Show("nenalezeno");
            }
            return tmp;
        }

        private Embeding GetTorusEmbeding(Embeding embeding)
        {
            Embeding sphereEmbeding = GetSphereEmbeding(embeding);
            if (sphereEmbeding != null)
            {
                // planar embedding is also torus embeding
                return sphereEmbeding;
            }
            Embeding h = SubgrafHomoomorphicToK5K3_3(embeding);
            if (h.IsK5())
            {
                List<int> k5Vertices = h.GetK5Vertices();
                foreach (Embeding e in Embeding.GetK5Embedings())
                {
                    //dfs najit vrchly pro vesechny potencionali sousedy a naprasit je tam pri vytvareni
                    var tmp = ExtedEmgeddingTorus(embeding, e);
                    if (tmp != null)
                    {
                        return tmp;
                    }
                }
                // no embeding found
                return null;
            }
            if (h.IsK3_3())
            {
                List<int> k33Vertexes = h.GetK33Vertices();
                foreach (Embeding e in Embeding.GetK3_3Embedings())
                {
                    var tmp = ExtedEmgeddingTorus(embeding, e);
                    if (tmp != null)
                    {
                        return tmp;
                    }
                }
                // no embeding found
                return null;
            }
            throw new EmbeddingException("there schould be k5 or k3_3");
        }
                
        private Embeding SubgrafHomoomorphicToK5K3_3(Embeding embeding)
        {
            Embeding h = new Embeding(embeding);            
            foreach (Tuple<int, int> edge in embeding)
            {
                if (edge.Item1 < edge.Item2)
                {
                    h.RemoveEdgeSym(edge);
                    if (GetSphereEmbeding(h) != null)
                    {
                        h.AddEdgeSym(edge);
                    }
                }
            }            
            return h;
        }

        private GraphDefinition GetSpherePozitioning(Embeding embeding)
        {
            Embeding embeding2 = GetSphereEmbeding(embeding);
            if (embeding2 == null)
            {
                MessageBox.Show("nenalezeno");
            }
            else
            {
                MessageBox.Show("ano");
            }
            return tmp;
        }

        //returns null if not planar
        private Embeding GetSphereEmbeding(Embeding g)
        {
            List<int> cycle = GetCycle(g);
            if (cycle == null)
            {
                // no cycle in g so each embedding is valid
                return g;
            }
            Embeding h = new Embeding(cycle);
            Embeding i = new Embeding(g);
            i.Minus(h);
            return ExtedEmgeddingPlanar(g, h, i);
        }

        Dictionary<int, bool> visited;
        int start;
        Stack<int> cycleTmp;
        Embeding cycleG;
        bool end = false;
        private List<int> GetCycle(Embeding g)
        {
            cycleG = g;
            foreach (int vertex in g.neighbors.Keys)
            {
                start = vertex;
                end = false;
                visited = new Dictionary<int, bool>();
                foreach (var v in g.neighbors.Keys)
                {
                    visited.Add(v, false);
                }
                cycleTmp = new Stack<int>();
                cycleTmp.Push(start);
                visit(start);
                if (end) break;
            }
            List<int> cycle = new List<int>(cycleTmp);
            if (cycle.Count == 1) return null;
            return cycle;
        }
                
        private void visit(int u)
        {
            if (end) return;
            visited[u] = true;
            foreach (var v in cycleG.neighbors[u])
            {
                if (v == start && cycleTmp.Count >2)
                {
                    end = true;
                    return;
                }
                if (!visited[v])
                {
                    cycleTmp.Push(v);
                    visit(v);
                    if (end) return;
                    cycleTmp.Pop();
                }
            }
        }

        private Embeding ExtedEmgeddingPlanar(Embeding g, Embeding h, Embeding i)
        {
             if (h.Count() == g.Count())
            {
                return h;
            }
            List<BridgeWithRespectTo> brigdesWithRespect = GerBridgesWithRespect(g, h, i);
            List<CircularListInt> faces = h.GetFaces();
            foreach (BridgeWithRespectTo bridge in brigdesWithRespect)
            {
                foreach (CircularListInt face in faces)
                {
                    bool isValid = true;
                    foreach (var v in bridge.AttachmentVertexes)
                    {
                        if (!face.Contains(v))
                        {
                            isValid = false;
                        }
                    }
                    if (isValid)
                    {
                        bridge.Faces.Add(face);
                    }
                }
            }
            brigdesWithRespect.Sort();
            BridgeWithRespectTo brigdesWithRespectBest = brigdesWithRespect[0];
            if (brigdesWithRespectBest.NumberOfAdmisibleFaces == 0)
            {
                //this graph canot be embeded.
                return null;
            }
            if (brigdesWithRespectBest.BridgeType == BridgType.Type1)
            {
                List<int> path = brigdesWithRespectBest.GetPath();
                if (path == null)
                {
                    throw new NotImplementedException();
                }
                h.AddPath(path, brigdesWithRespectBest.Faces[0]);
            }
            if (brigdesWithRespectBest.BridgeType == BridgType.Type2)
            {
                List<int> path = new List<int>();
                path.Add(brigdesWithRespectBest.Embeding.First().Item1);
                path.Add(brigdesWithRespectBest.Embeding.First().Item2);
                h.AddPath(path, brigdesWithRespectBest.Faces[0]);
            }
            //this can be done better
            i = new Embeding(g);
            i.Minus(h);
            //MessageBox.Show(h.ToString());
            return ExtedEmgeddingPlanar(g, h, i);
        }
        // add some better return type
        private Embeding ExtedEmgeddingTorus(Embeding g, Embeding h)
        {
            if (g.Count() == h.Count())
            {
                return h;
            }
            Embeding i = new Embeding(g);
            g.Minus(h);
            List<CircularListInt> faces = h.GetFaces();
            List<BridgeWithRespectTo> brigdesWithRespect = GerBridgesWithRespect(g, h, i);
            foreach (BridgeWithRespectTo bridge in brigdesWithRespect)
            {
                foreach (CircularListInt face in faces)
                {
                    bool isValid = true;
                    foreach (var v in bridge.AttachmentVertexes)
                    {
                        if (!face.Contains(v))
                        {
                            isValid = false;
                        }
                    }
                    if (isValid)
                    {
                        bridge.Faces.Add(face);
                    }                    
                }
            }
            brigdesWithRespect.Sort();
            BridgeWithRespectTo brigdesWithRespectBest = brigdesWithRespect[0];
            if (brigdesWithRespectBest.NumberOfAdmisibleFaces == 0)
            {                
                return null;
            }
            List<int> path = null ;
            if (brigdesWithRespectBest.BridgeType == BridgType.Type1)
            {
                path = brigdesWithRespectBest.GetPath();
                if (path == null)
                {
                    throw new NotImplementedException();
                }                
            }
            if (brigdesWithRespectBest.BridgeType == BridgType.Type2)
            {
                path = new List<int>();
                path.Add(brigdesWithRespectBest.Embeding.First().Item1);
                path.Add(brigdesWithRespectBest.Embeding.First().Item2);                
            }

            h.AddPath(path, brigdesWithRespectBest.Faces[0],VertexState.FIRST,VertexState.FIRST);
            Embeding emgeddingTorus = ExtedEmgeddingTorus(g, h);
            if (emgeddingTorus != null) return emgeddingTorus;
            h.RemovePath(path);

            h.AddPath(path, brigdesWithRespectBest.Faces[0], VertexState.FIRST, VertexState.SECOND);
            emgeddingTorus = ExtedEmgeddingTorus(g, h);
            if (emgeddingTorus != null) return emgeddingTorus;
            h.RemovePath(path);

            h.AddPath(path, brigdesWithRespectBest.Faces[0], VertexState.SECOND, VertexState.FIRST);
            emgeddingTorus = ExtedEmgeddingTorus(g, h);
            if (emgeddingTorus != null) return emgeddingTorus;
            h.RemovePath(path);

            h.AddPath(path, brigdesWithRespectBest.Faces[0], VertexState.SECOND, VertexState.SECOND);
            emgeddingTorus = ExtedEmgeddingTorus(g, h);
            if (emgeddingTorus != null) return emgeddingTorus;
            h.RemovePath(path);

            return emgeddingTorus;
        }

        private static List<BridgeWithRespectTo> GerBridgesWithRespect(Embeding g, Embeding h, Embeding i)
        {
            ConnectedComponentGetter compenetGetter = new ConnectedComponentGetter(i);
            List<Embeding> components = compenetGetter.GetComponents();
            List<BridgeWithRespectTo> bridgesWithRespectTo = new List<BridgeWithRespectTo>();
            components.ForEach(x => bridgesWithRespectTo.Add(new BridgeWithRespectTo(x)));


            foreach (var bridge in bridgesWithRespectTo)
            {
                List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
                foreach (Tuple<int, int> edge in g)
                {
                    if (bridge.Embeding.ConteintsVertex(edge.Item1) && h.ConteintsVertex(edge.Item2))
                    {
                        tmp.Add(edge);
                    }
                }
                foreach (Tuple<int, int> edge in tmp)
                {
                    bridge.AddEdgeSym(edge);
                    bridge.AttachmentVertexes.Add(edge.Item2);
                }
                bridge.BridgeType = BridgType.Type1;
            }
            foreach (Tuple<int, int> edge in g)
            {                
                if (h.ConteintsVertex(edge.Item1) && h.ConteintsVertex(edge.Item2) && !h.ContaintsEdgeAsym(edge) && edge.Item1 < edge.Item2)
                {
                    
                    BridgeWithRespectTo tmp = new BridgeWithRespectTo(new Embeding(edge), edge);
                    bridgesWithRespectTo.Add(tmp);
                }
            }
            return bridgesWithRespectTo;
        }
    }
}
