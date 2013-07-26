using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;
using Microsoft.Xna.Framework;

namespace Plugin
{
    class VertexDegree5 : VertexDegree
    {
        EdgeMultiGraph edgeUV, edgeUW, edgeUX, edgeUY, edgeUZ = null;
        int indexV, indexW, indexX, indexY, indexZ = -1;
        int v, w, x, y, z = -1;
        int u =-1;
        List<EdgeMultiGraph> addedEdges = new List<EdgeMultiGraph>();
        /// <summary>
        /// adding vertex back to embedding
        /// </summary>
        /// <param name="embedding">embedding to add vertex to</param>
        public override void Add(EmbedingMultiGraph e)
        {
            
            PointF pos = PentagonMidle(e.pozition[v], e.pozition[w], e.pozition[x], e.pozition[y], e.pozition[z]);
            e.pozition[u] = pos;
            foreach (EdgeMultiGraph edge in addedEdges)
            {
                e.RemoveEdgeSym(edge);
            }
            e.incidenceEdges.Add(u, new CircularListEdge());
            e.incidenceEdges[u].Add(edgeUV.SymetricEdge);
            e.incidenceEdges[u].Add(edgeUW.SymetricEdge);
            e.incidenceEdges[u].Add(edgeUX.SymetricEdge);
            e.incidenceEdges[u].Add(edgeUY.SymetricEdge);
            e.incidenceEdges[u].Add(edgeUZ.SymetricEdge);
            e.incidenceEdges[z].Insert(indexZ, edgeUZ);
            e.incidenceEdges[y].Insert(indexY, edgeUY);
            e.incidenceEdges[x].Insert(indexX, edgeUX);
            e.incidenceEdges[w].Insert(indexW, edgeUW);
            e.incidenceEdges[v].Insert(indexV, edgeUV);            
        }

        private PointF PentagonMidle(params PointF[] points)
        {
            PointF result = new PointF();
            int resulution = 100;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X *= resulution;
                points[i].Y *= resulution;
            }
            LineSegment[] pentagon = new LineSegment[5];
            LineSegment[] lines = new LineSegment[5];
            for (int i = 0; i < points.Length; i++)
            {
                pentagon[i] = new LineSegment(new Vector2(points[i].X, points[i].Y), new Vector2(points[(i + 1) % 5].X, points[(i + 1) % 5].Y));
                lines[i] = new LineSegment(new Vector2(points[i].X, points[i].Y), new Vector2(0, 0));
            }
            int sumX = 0;
            int sumY = 0;
            int count = 0;
            for (int x = 0; x < resulution; x++)
            {                
                for (int y = 0; y < resulution; y++)
                {
                    bool conflict = false;
                    Vector2 point = new Vector2(x, y);
                    for (int i = 0; i < 5; i++)
                    {
                        lines[i].B = point;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (pentagon[i].IsCrossing(lines[j]))
                            {
                                conflict = true; 
                            }
                        }
                    }
                    if (!conflict)
                    {
                        sumX = checked(sumX + x);
                        sumY = checked(sumY + y);
                        count++;
                    }
                }
            }
            result.X = sumX / count;
            result.Y = sumY / count;
            result.X /= resulution;
            result.Y /= resulution;
            return result;
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
            z = embedding.incidenceEdges[vertex][4].V;

            edgeUV = embedding.incidenceEdges[vertex][0].SymetricEdge;
            edgeUW = embedding.incidenceEdges[vertex][1].SymetricEdge;
            edgeUX = embedding.incidenceEdges[vertex][2].SymetricEdge;
            edgeUY = embedding.incidenceEdges[vertex][3].SymetricEdge;
            edgeUZ = embedding.incidenceEdges[vertex][4].SymetricEdge;

            indexV = embedding.incidenceEdges[v].IndexOf(edgeUV);
            embedding.RemoveEdgeSym(edgeUV);
            indexW = embedding.incidenceEdges[w].IndexOf(edgeUW);
            embedding.RemoveEdgeSym(edgeUW);
            indexX = embedding.incidenceEdges[x].IndexOf(edgeUX);
            embedding.RemoveEdgeSym(edgeUX);
            indexY = embedding.incidenceEdges[y].IndexOf(edgeUY);
            embedding.RemoveEdgeSym(edgeUY);
            indexZ = embedding.incidenceEdges[z].IndexOf(edgeUZ);
            embedding.RemoveEdgeSym(edgeUZ);

            if (Distinct(v, x, y))
            {
                //adding vx and vy diagonals
                EdgeMultiGraph diagonalVX = embedding.GetNewEdgeSym(v, x);
                EdgeMultiGraph diagonalVY = embedding.GetNewEdgeSym(v, y);
                embedding.incidenceEdges[v].Insert(indexV, diagonalVY);
                embedding.incidenceEdges[v].Insert(indexV, diagonalVX);
                embedding.incidenceEdges[y].Insert(indexY, diagonalVY.SymetricEdge);
                embedding.incidenceEdges[x].Insert(indexX, diagonalVX.SymetricEdge);
                addedEdges.Add(diagonalVX);
                addedEdges.Add(diagonalVY);
                return;
            }

            if (Distinct(w, y, z))
            {
                //adding wy wz diagonals
                EdgeMultiGraph diagonalWY = embedding.GetNewEdgeSym(w, y);
                EdgeMultiGraph diagonalWZ = embedding.GetNewEdgeSym(w, z);
                embedding.incidenceEdges[w].Insert(indexW, diagonalWZ);
                embedding.incidenceEdges[w].Insert(indexW, diagonalWY);
                embedding.incidenceEdges[z].Insert(indexZ, diagonalWZ.SymetricEdge);
                embedding.incidenceEdges[y].Insert(indexY, diagonalWY.SymetricEdge);
                addedEdges.Add(diagonalWY);
                addedEdges.Add(diagonalWZ);
                return;
            }

            

            if (Distinct(y, v, w))
            {
                //adding yv and yw
                EdgeMultiGraph diagonalYV = embedding.GetNewEdgeSym(y, v);
                EdgeMultiGraph diagonalYW = embedding.GetNewEdgeSym(y, w);
                embedding.incidenceEdges[y].Insert(indexY, diagonalYW);
                embedding.incidenceEdges[y].Insert(indexY, diagonalYV);
                embedding.incidenceEdges[w].Insert(indexW, diagonalYW.SymetricEdge);
                embedding.incidenceEdges[v].Insert(indexV, diagonalYV.SymetricEdge);
                addedEdges.Add(diagonalYV);
                addedEdges.Add(diagonalYW);
                return;
            }
            
            if (Distinct(z, w, x))
            {
                //adding zw and zx diagonal
                EdgeMultiGraph diagonalZW = embedding.GetNewEdgeSym(z, w);
                EdgeMultiGraph diagonalZX = embedding.GetNewEdgeSym(z, x);
                embedding.incidenceEdges[z].Insert(indexZ, diagonalZX);
                embedding.incidenceEdges[z].Insert(indexZ, diagonalZW);
                embedding.incidenceEdges[x].Insert(indexX, diagonalZX.SymetricEdge);
                embedding.incidenceEdges[w].Insert(indexW, diagonalZW.SymetricEdge);
                addedEdges.Add(diagonalZW);
                addedEdges.Add(diagonalZX);
                return;
            }
            if (Distinct(x, v, z))
            {
                //adding xv,xz diagonals
                EdgeMultiGraph diagonalXV = embedding.GetNewEdgeSym(x, v);
                EdgeMultiGraph diagonalXZ = embedding.GetNewEdgeSym(x, z);
                embedding.incidenceEdges[x].Insert(indexX, diagonalXV);
                embedding.incidenceEdges[x].Insert(indexX, diagonalXZ);
                if(indexV != 0)
                {
                    embedding.incidenceEdges[v].Insert((indexV - 1), diagonalXV.SymetricEdge);
                }
                else
                {
                    embedding.incidenceEdges[v].Insert(embedding.incidenceEdges[v].Count, diagonalXV.SymetricEdge);
                }
                embedding.incidenceEdges[z].Insert(indexZ, diagonalXZ.SymetricEdge);
                addedEdges.Add(diagonalXZ);
                addedEdges.Add(diagonalXV);
                return;
            }
            
            throw new NotImplementedException();
        }
    }
}
