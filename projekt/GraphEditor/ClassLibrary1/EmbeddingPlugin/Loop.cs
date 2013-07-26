using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;

namespace Plugin
{
    class Loop : VertexDegree
    {
        EdgeMultiGraph edge1 = null;
        EdgeMultiGraph edge2 = null;
        EdgeMultiGraph originalEdge = null;
        int indexReflexiveEdge = -1;
        int indexReflexiveSymetricEdge = -1;
        int z = -1;
        int v = -1;
        /// <summary>
        /// removing vertex with given index from embedding
        /// </summary>
        /// <param name="embedding">embedding to remove vertex from</param>
        /// <param name="vertex">index of vertex to remove</param>
        public override void Remove(EmbedingMultiGraph embedding, int v)
        {
            this.v = v;
            EdgeMultiGraph edgeReflexive = null ;
            foreach (EdgeMultiGraph edge in embedding.incidenceEdges[v])
            {
                if (edge.U == edge.V)
                {
                    edgeReflexive = edge;
                    break;
                }
            }
            originalEdge = edgeReflexive;
            indexReflexiveEdge = embedding.incidenceEdges[v].IndexOf(edgeReflexive);
            indexReflexiveSymetricEdge = embedding.incidenceEdges[v].IndexOf(edgeReflexive.SymetricEdge);
            z = embedding.newVertexid;
            embedding.newVertexid++;
            embedding.incidenceEdges.Add(z, new CircularListEdge());
            embedding.RemoveEdgeSym(edgeReflexive);
            edge1 = embedding.GetNewEdgeSym(v, z);
            edge2 = embedding.GetNewEdgeSym(v, z);
            embedding.incidenceEdges[v].Remove(edgeReflexive);
            embedding.incidenceEdges[v].Remove(edgeReflexive.SymetricEdge);
            embedding.incidenceEdges[v].Insert(indexReflexiveEdge, edge1);
            embedding.incidenceEdges[v].Insert(indexReflexiveSymetricEdge, edge2);
            embedding.incidenceEdges[z].Add(edge1.SymetricEdge);
            embedding.incidenceEdges[z].Add(edge2.SymetricEdge);                        
        }

        public override void Add(EmbedingMultiGraph embedding)
        {
            //don't set asix
            embedding.RemoveEdgeSym(edge1);
            embedding.RemoveEdgeSym(edge2);
            embedding.incidenceEdges[v].Insert(indexReflexiveEdge, originalEdge);
            embedding.incidenceEdges[v].Insert(indexReflexiveSymetricEdge, originalEdge.SymetricEdge);
            embedding.pozition.Remove(z);
        }
    }
}
