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
        /// <summary>
        /// sets new positions of vertices in given graph definition
        /// </summary>
        /// <param name="sourceDefinition"></param>
        /// <returns></returns>
        public override GraphDefinition Pozitioning(GraphDefinition sourceDefinition)
        {
            if (sourceDefinition.edges.Count < 2) return sourceDefinition;
            Embedding embdeding = new Embedding(sourceDefinition);
            ConnectedComponentGetter components = new ConnectedComponentGetter(embdeding);
            if (components.GetComponents().Count > 1)
            {
                MessageBox.Show("only connected graphs are supported");
                return sourceDefinition;
            }
            switch (sourceDefinition.suraceType)
            {
                case SurfaceTypeEnum.Sphere:
                    embdeding = GetSphereEmbedding(embdeding);
                    break;
                case SurfaceTypeEnum.Torus:
                    embdeding = GetTorusEmbedding(embdeding);
                    break;
                default:
                    throw new ArgumentException();
            }

            if (embdeding == null)
            {
                MessageBox.Show("unable to draw given graph on selected surface!");
                return sourceDefinition;
            }
            EmbeddingMultiGraph multiGraphEmbedding = new EmbeddingMultiGraph(embdeding);
            Dictionary<int, PointF> positions = multiGraphEmbedding.GetPositions();
            foreach (KeyValuePair<int, PointF> item in positions)
            {
                sourceDefinition.vertices[item.Key].Pozition = item.Value;
            }
            foreach (Edge edge in sourceDefinition.edges)
            {
                edge.RemoveAllSubPoints();
            }
            return sourceDefinition;
        }

        /// <summary>
        /// gets combinatoric embedding of graph on torus
        /// </summary>
        /// <param name="embedding"></param>
        /// <returns></returns>
        private Embedding GetTorusEmbedding(Embedding embedding)
        {
            Embedding sphereEmbedding = GetSphereEmbedding(embedding);
            if (sphereEmbedding != null)
            {
                // planar embedding is also torus embedding
                return sphereEmbedding;
            }
            Embedding h = SubgrafHomeomorphicToK5K3_3(embedding);
            if (h.IsK5())
            {
                Stack<Vertex2Record> subpoints = h.getSubPoints();
                int[] k5Vertices = h.GetK5Vertices().ToArray();                
                Permutations<int> permutations = new Permutations<int>();
                Embedding tmp = null;
                permutations.Eval(k5Vertices, (permutation) =>
                {
                    foreach (Embedding e in Embedding.GetK5Embeddings(permutation))
                    {
                        e.InsertSubPoints(subpoints);
                        tmp = ExtedEmgeddingTorus(embedding, e);
                        if (tmp != null)
                        {
                            return true;
                        }
                    }
                    return false;
                });
                return tmp;
            }
            if (h.IsK3_3())
            {
                
                Permutations<int> permutations = new Permutations<int>();
                Stack<Vertex2Record> subpoints = h.getSubPoints();
                int[][] k33Vertexes = h.GetK33Vertices();
                int[] k33Vertexes1 = k33Vertexes[0];
                int[] k33Vertexes2 = k33Vertexes[1];                
                Embedding tmp = null;
                permutations.Eval(k33Vertexes1, (permutation1) =>
                {
                    bool reult = false;
                    permutations.Eval(k33Vertexes2, (permutation2) =>
                        {
                            int[] permutationFinal = new int[6];
                            Array.Copy(permutation1, permutationFinal, 3);
                            Array.Copy(permutation2, 0, permutationFinal, 3, 3);
                            foreach (Embedding e in Embedding.GetK3_3Embeddings(permutationFinal))
                            {
                                e.InsertSubPoints(subpoints);
                                tmp = ExtedEmgeddingTorus(embedding, e);
                                if (tmp != null)
                                {
                                    reult = true;
                                    return true;
                                }

                            }
                            return false;
                        });
                    return reult;
                });
                return tmp;
            }
            throw new EmbeddingException("there should be k5 or k3_3");
        }
                
        /// <summary>
        /// finds graph homeomorphic to k5 or k3_3
        /// </summary>
        /// <param name="embedding"></param>
        /// <returns></returns>
        private Embedding SubgrafHomeomorphicToK5K3_3(Embedding embedding)
        {
            Embedding h = new Embedding(embedding);            
            foreach (Tuple<int, int> edge in embedding)
            {
                if (edge.Item1 < edge.Item2)
                {
                    h.RemoveEdgeSym(edge);
                    if (GetSphereEmbedding(h) != null)
                    {
                        h.AddEdgeSym(edge);
                    }
                }
            }            
            return h;
        }

        /// <summary>
        /// gets combinatoric embedding of graph on sphere
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private Embedding GetSphereEmbedding(Embedding g)
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

        /// <summary>
        /// finds cycle in given embedding
        /// </summary>
        /// <param name="embedding"></param>
        /// <returns>list of vertexes</returns>
        private List<int> GetCycle(Embedding embedding)
        {
            cycleG = embedding;
            foreach (int vertex in embedding.neighbors.Keys)
            {
                start = vertex;
                end = false;
                visited = new Dictionary<int, bool>();
                foreach (int v in embedding.neighbors.Keys)
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
            foreach (int v in cycleG.neighbors[u])
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

        /// <summary>
        /// iteratively extends embedding of graph h in sphere until it is equal to graph g
        /// </summary>
        /// <param name="g">final graph</param>
        /// <param name="h">already embedded graph</param>
        /// <param name="i">rest of graph ( g \ h )</param>
        /// <returns></returns>
        private Embedding ExtedEmgeddingPlanar(Embedding g, Embedding h, Embedding i)
        {
            //TODO caching count...
            if (h.Count() == g.Count())
            {
                return h;
            }
            if (h.Count() > g.Count())
            {
                //problem
                throw new NotImplementedException();
            }
            List<BridgeWithRespectTo> brigdesWithRespect = GetBridgesWithRespect(g, h, i);
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
                h.AddPath(path, brigdesWithRespectBest.Faces[0]);
            }
            if (brigdesWithRespectBest.BridgeType == BridgType.Type2)
            {
                List<int> path = new List<int>();
                path.Add(brigdesWithRespectBest.Embedding.First().Item1);
                path.Add(brigdesWithRespectBest.Embedding.First().Item2);
                h.AddPath(path, brigdesWithRespectBest.Faces[0]);
            }
            //TODO this can be done better
            i = new Embedding(g);
            i.Minus(h);            
            return ExtedEmgeddingPlanar(g, h, i);
        }

        /// <summary>
        /// finds faces where can the bridge be embedded
        /// </summary>
        /// <param name="faces">list of faces</param>
        /// <param name="brigdesWithRespect">bridge definition</param>
        private static void FindAddmisibleFaces(List<CircularListInt> faces, List<BridgeWithRespectTo> brigdesWithRespect)
        {
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
        }

        /// <summary>
        /// finds path of bridge which will be embedded
        /// </summary>
        /// <param name="brigdesWithRespectBest"></param>
        /// <returns></returns>
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
                path.Add(brigdesWithRespectBest.Embedding.First().Item1);
                path.Add(brigdesWithRespectBest.Embedding.First().Item2);
            }
            return path;
        }

        /// <summary>
        /// iteratively extends embedding of graph h until it is equal to graph g
        /// </summary>
        /// <param name="g">final graph</param>
        /// <param name="h">already embedded graph</param>
        private Embedding ExtedEmgeddingTorus(Embedding g, Embedding h)
        {            
            if (g.Count() == h.Count())
            {
                return h;
            }
            Embedding i = new Embedding(g);
            i.Minus(h);
            List<CircularListInt> faces = h.GetFaces();
            List<BridgeWithRespectTo> brigdesWithRespect = GetBridgesWithRespect(g, h, i);
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
            if (emgeddingTorus == null)
            {
                h.RemovePath(path);
                if (h.AddPath(path, brigdesWithRespectBest.Faces[0], VertexState.FIRST, VertexState.SECOND))
                {
                    emgeddingTorus = ExtedEmgeddingTorus(g, h);
                    if (emgeddingTorus == null)
                    {
                        h.RemovePath(path);
                    }
                }
                if (emgeddingTorus == null)               {
                    
                    if (h.AddPath(path, brigdesWithRespectBest.Faces[0], VertexState.SECOND, VertexState.FIRST))
                    {
                        emgeddingTorus = ExtedEmgeddingTorus(g, h);
                        if (emgeddingTorus == null)
                        {
                            h.RemovePath(path);
                        }
                    }
                    if (emgeddingTorus == null)
                    {
                        
                        if(h.AddPath(path, brigdesWithRespectBest.Faces[0], VertexState.SECOND, VertexState.SECOND))
                        {
                            emgeddingTorus = ExtedEmgeddingTorus(g, h);
                            if (emgeddingTorus == null)
                            {
                                h.RemovePath(path);
                            }
                        }
                    }
                }
                
            }
            return emgeddingTorus;
        }

        /// <summary>
        /// gets bridges of i with respect to h
        /// </summary>
        /// <param name="g">entire graph</param>
        /// <param name="h">already embedded graph</param>
        /// <param name="i">est of graph ( g \ h )</param>
        /// <returns></returns>
        private static List<BridgeWithRespectTo> GetBridgesWithRespect(Embedding g, Embedding h, Embedding i)
        {
            ConnectedComponentGetter compenetGetter = new ConnectedComponentGetter(i);
            List<Embedding> components = compenetGetter.GetComponents();
            List<BridgeWithRespectTo> bridgesWithRespectTo = new List<BridgeWithRespectTo>();
            components.ForEach(x => bridgesWithRespectTo.Add(new BridgeWithRespectTo(x)));


            foreach (BridgeWithRespectTo bridge in bridgesWithRespectTo)
            {
                List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
                foreach (Tuple<int, int> edge in g)
                {
                    if (bridge.Embedding.ConteintsVertex(edge.Item1) && h.ConteintsVertex(edge.Item2))
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
