using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;

namespace Plugin
{
    class VertexDegree3 : VertexDegree
    {
        int indexV, indexW, indexX = -1;
        int u, v, w, x = -1;

        List<EdgeMultiGraph> copyFromU = new List<EdgeMultiGraph>();
        public override void Add(EmbedingMultiGraph embedding)
        {
            PointF position = GetPosition(embedding.incidenceEdges[v][indexV], embedding);
            embedding.incidenceEdges.Add(u, new CircularListEdge());
            for (int i = 0; i < copyFromU.Count; i++)
            {
                EdgeMultiGraph edge = copyFromU[i];
                if (position.X > 1)
                {
                    edge.WrapHorizontal = 1;
                }
                if(position.X <0)
                {
                    edge.WrapHorizontal = - 1;
                }
                if (position.Y > 1)
                {
                    edge.WrapVertical = 1;
                }
                if (position.Y < 0)
                {
                    edge.WrapVertical = -1;
                }
                embedding.incidenceEdges[u].Add(edge);
            }
            embedding.incidenceEdges[v].Insert(indexV, copyFromU[0].SymetricEdge);
            embedding.incidenceEdges[w].Insert(indexW, copyFromU[1].SymetricEdge);
            embedding.incidenceEdges[x].Insert(indexX, copyFromU[2].SymetricEdge);
            embedding.pozition.Add(u, position);
        }
        public override void Remove(EmbedingMultiGraph embedding, int u)
        {
            copyFromU = new List<EdgeMultiGraph>();
            this.u = u;
            v = embedding.incidenceEdges[u][0].V;
            w = embedding.incidenceEdges[u][1].V;
            x = embedding.incidenceEdges[u][2].V;

            indexV = embedding.incidenceEdges[v].IndexOf(embedding.incidenceEdges[u][0].SymetricEdge);
            indexW = embedding.incidenceEdges[w].IndexOf(embedding.incidenceEdges[u][1].SymetricEdge);
            indexX = embedding.incidenceEdges[x].IndexOf(embedding.incidenceEdges[u][2].SymetricEdge);

            foreach (var edge in embedding.incidenceEdges[u])
            {
                copyFromU.Add(edge);                
            }
            foreach (var edge in copyFromU)
            {
                embedding.RemoveEdgeSym(edge);
            }
        }
    }
}
