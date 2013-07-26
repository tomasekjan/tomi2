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
        
        public override GraphDefinition Pozitioning(GraphDefinition sourceDefinition)
        {
            Embedding embeding = new Embedding(sourceDefinition);
            switch (sourceDefinition.suraceType)
            {
                case SurfaceTypeEnum.Sphere:
                    embeding = GetSphereEmbeding(embeding);
                    break;
                case SurfaceTypeEnum.Torus:
                    embeding = GetTorusEmbeding(embeding);
                    break;
                default:
                    throw new ArgumentException();
            }
            if (embeding == null)
            {
                MessageBox.Show("unable to draw given graph on selected surface!");
                return sourceDefinition;
            }
            EmbedingMultiGraph multiGraphEmbeding = new EmbedingMultiGraph(embeding);
            Dictionary<int, PointF> positions = multiGraphEmbeding.GetPositions();            
            foreach (var item in positions)
            {
                sourceDefinition.vertices[item.Key].Pozition = item.Value;
            }
            foreach (Edge edge in sourceDefinition.edges)
            {
                edge.RemoveAllSubPoints();
            }
            return sourceDefinition;            
        }

        private Embedding GetTorusEmbeding(Embedding embeding)
        {
            Embedding sphereEmbeding = GetSphereEmbeding(embeding);
            if (sphereEmbeding != null)
            {
                // planar embedding is also torus embedding
                return sphereEmbeding;
            }
            Embedding h = SubgrafHomoomorphicToK5K3_3(embeding);
            if (h.IsK5())
            {
                List<int> k5Vertices = h.GetK5Vertices();
                Permutations permutations = new Permutations(k5Vertices.ToArray());
                foreach (int[] permutation in permutations.CalcPermutation())
                {
                    foreach (Embedding e in Embedding.GetK5Embedings(permutation))
                    {
                        var tmp = ExtedEmgeddingTorus(embeding, e);
                        if (tmp != null)
                        {
                            return tmp;
                        }
                    }
                 
                }
                // no embedding found
                return null;
            }
            if (h.IsK3_3())
            {
                List<int> k33Vertexes = h.GetK33Vertices();
                foreach (Embedding e in Embedding.GetK3_3Embedings())
                {
                    var tmp = ExtedEmgeddingTorus(embeding, e);
                    if (tmp != null)
                    {
                        return tmp;
                    }
                }
                // no embedding found
                return null;
            }
            throw new EmbeddingException("there should be k5 or k3_3");
        }
                
        private Embedding SubgrafHomoomorphicToK5K3_3(Embedding embeding)
        {
            Embedding h = new Embedding(embeding);            
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

        //returns null if not planar
        private Embedding GetSphereEmbeding(Embedding g)
        {
            List<int> cycle = GetCycle(g);
            if (cycle == null)
            {
                // no cycle in g so each embedding is valid
                return g;
            }
            Embedding h = new Embedding(cycle);
            Embedding i = new Embedding(g);
            i.Minus(h);
            return ExtedEmgeddingPlanar(g, h, i);
        }

        Dictionary<int, bool> visited;
        int start;
        Stack<int> cycleTmp;
        Embedding cycleG;
        bool end = false;
        private List<int> GetCycle(Embedding g)
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

        private Embedding ExtedEmgeddingPlanar(Embedding g, Embedding h, Embedding i)
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
                    foreach (int v in bridge.AttachmentVertexes)
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
                //this graph cannot be embedded.
                return null;
            }
            if (brigdesWithRespectBest.BridgeType == BridgType.Type1)
            {
                List<int> path = brigdesWithRespectBest.GetPath();
                if (path == null)
                {
                    // TODO can it occur ?
                    return null;
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
            //TODO this can be done better
            i = new Embedding(g);
            i.Minus(h);            
            return ExtedEmgeddingPlanar(g, h, i);
        }

        private static void FindAddmisibleFaces(List<CircularListInt> faces, List<BridgeWithRespectTo> brigdesWithRespect)
        {
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
        }

        private static List<int> GetPath(BridgeWithRespectTo brigdesWithRespectBest)
        {
            List<int> path = null;
            if (brigdesWithRespectBest.BridgeType == BridgType.Type1)
            {
                path = brigdesWithRespectBest.GetPath();
            }
            if (brigdesWithRespectBest.BridgeType == BridgType.Type2)
            {
                path = new List<int>();
                path.Add(brigdesWithRespectBest.Embeding.First().Item1);
                path.Add(brigdesWithRespectBest.Embeding.First().Item2);
            }
            return path;
        }

        private Embedding ExtedEmgeddingTorus(Embedding g, Embedding h)
        {
            if (g.Count() == h.Count())
            {
                return h;
            }
            Embedding i = new Embedding(g);
            i.Minus(h);
            List<CircularListInt> faces = h.GetFaces();
            List<BridgeWithRespectTo> brigdesWithRespect = GerBridgesWithRespect(g, h, i);
            FindAddmisibleFaces(faces, brigdesWithRespect);
            brigdesWithRespect.Sort();
            BridgeWithRespectTo brigdesWithRespectBest = brigdesWithRespect[0];
            if (brigdesWithRespectBest.NumberOfAdmisibleFaces == 0)
            {                
                return null;
            }
            List<int> path = GetPath(brigdesWithRespectBest);

            h.AddPath(path, brigdesWithRespectBest.Faces[0],VertexState.FIRST,VertexState.FIRST);
            Embedding emgeddingTorus = ExtedEmgeddingTorus(g, h);
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

        private static List<BridgeWithRespectTo> GerBridgesWithRespect(Embedding g, Embedding h, Embedding i)
        {
            ConnectedComponentGetter compenetGetter = new ConnectedComponentGetter(i);
            List<Embedding> components = compenetGetter.GetComponents();
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
                    
                    BridgeWithRespectTo tmp = new BridgeWithRespectTo(new Embedding(edge), edge);
                    bridgesWithRespectTo.Add(tmp);
                }
            }
            return bridgesWithRespectTo;
        }
    }
}
