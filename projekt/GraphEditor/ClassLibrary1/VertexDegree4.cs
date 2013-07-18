using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;

namespace Plugin
{
    class VertexDegree4 : VertexDegree
    {
        int u, v, w, x, y = -1;
        EdgeMultiGraph ev, ew, ex, ey = null;
        int iv, iw, ix, iy = -1;
        List<EdgeMultiGraph> addedEdges = new List<EdgeMultiGraph>();
        /// <summary>
        /// adding vertex back to embedding
        /// </summary>
        /// <param name="embedding">embedding to add vertex to</param>
        public override void Add(EmbedingMultiGraph embedding)
        {
            PointF position = new PointF();
            EdgeMultiGraph e = addedEdges[0];
            PointF posU = embedding.pozition[e.U];
            PointF posV = embedding.pozition[e.V];
            position.X = (posU.X + posV.X) / 2;
            position.Y = (posU.Y + posV.Y) / 2;
            embedding.pozition.Add(u, position);
            foreach (EdgeMultiGraph edge in addedEdges)
            {
                embedding.RemoveEdgeSym(edge);
            }
            embedding.incidenceEdges.Add(u, new CircularListEdge());
            embedding.incidenceEdges[u].Add(ev.SymetricEdge);
            embedding.incidenceEdges[u].Add(ew.SymetricEdge);
            embedding.incidenceEdges[u].Add(ex.SymetricEdge);
            embedding.incidenceEdges[u].Add(ey.SymetricEdge);

            embedding.incidenceEdges[y].Insert(iy, ey);
            embedding.incidenceEdges[x].Insert(ix, ex);
            embedding.incidenceEdges[w].Insert(iw, ew);
            embedding.incidenceEdges[v].Insert(iv, ev);
        }
        /// <summary>
        /// removing vertex with given index from embedding
        /// </summary>
        /// <param name="embedding">embedding to remove vertex from</param>
        /// <param name="vertex">index of vertex to remove</param>
        public override void Remove(EmbedingMultiGraph embedding, int vertex)
        {
            u = vertex;
            v = embedding.incidenceEdges[vertex][0].V;
            w = embedding.incidenceEdges[vertex][1].V;
            x = embedding.incidenceEdges[vertex][2].V;
            y = embedding.incidenceEdges[vertex][3].V;

            ev = embedding.incidenceEdges[vertex][0].SymetricEdge;
            ew = embedding.incidenceEdges[vertex][1].SymetricEdge;
            ex = embedding.incidenceEdges[vertex][2].SymetricEdge;
            ey = embedding.incidenceEdges[vertex][3].SymetricEdge;

            iv = embedding.incidenceEdges[v].IndexOf(ev);
            embedding.RemoveEdgeSym(ev);
            iw = embedding.incidenceEdges[w].IndexOf(ew);
            embedding.RemoveEdgeSym(ew);
            ix = embedding.incidenceEdges[x].IndexOf(ex);
            embedding.RemoveEdgeSym(ex);
            iy = embedding.incidenceEdges[y].IndexOf(ey);
            embedding.RemoveEdgeSym(ey);

            if (Distinct(v, x))
            {
                // add vx diagonal
                EdgeMultiGraph diagonalVX = embedding.GetNewEdgeSym(v, x);
                embedding.incidenceEdges[v].Insert(iv, diagonalVX);
                embedding.incidenceEdges[x].Insert(ix, diagonalVX.SymetricEdge);
                addedEdges.Add(diagonalVX);
                return;
            }
            if (Distinct(w, y))
            {
                EdgeMultiGraph diagonalWY = embedding.GetNewEdgeSym(w, y);
                embedding.incidenceEdges[w].Insert(iw, diagonalWY);
                embedding.incidenceEdges[y].Insert(iy, diagonalWY.SymetricEdge);
                addedEdges.Add(diagonalWY);
                return;
            }
                        
            throw new NotImplementedException();
        }
    }
}
