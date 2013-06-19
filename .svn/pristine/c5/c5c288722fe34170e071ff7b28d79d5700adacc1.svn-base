using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Controls;
using System.Drawing;
using System.Windows;
using GraphEditor.Primitives3D;

namespace GraphEditor.GraphDeclaration
{
    public sealed partial class Graph
    {
        // left mouse button released 
        // move vertex if selected
        public void MouseLeftUp(PointF pointF)
        {
            if (ActualVertexMove != null)
            {
                Action();
                ActualVertexMove.Pozition = pointF;                
            }
        }

        // left mouse button pressed
        // if is nearby some point store it to ActualVertexMove
        public bool MouseLeftDown(PointF pointF)
        {
            ActualVertexMove = null;
            foreach (Vertex vertex in graphDeclaration.vertices)
            {
                if (vertex.Pozition.Compare(pointF))
                {
                    ActualVertexMove = vertex;
                    return true;
                }
            }

            foreach (Edge edge in graphDeclaration.edges)
            {
                if (edge.HaveVertex(pointF))
                {
                    ActualVertexMove = edge.getVertex(pointF);
                    return true;
                }
            }
            return false;
        }


        public bool MouseMiddletDown(PointF pointF)
        {
            ActualVertexAddEdge = null;
            foreach (Vertex vertex in graphDeclaration.vertices)
            {
                if (vertex.Pozition.Compare(pointF))
                {
                    ActualVertexAddEdge = vertex;
                    return true;
                }
            }
            return false;
        }

        public void MouseMiddletUp(PointF pointF, bool useXJoin, bool useYJoin)
        {
            if (ActualVertexAddEdge != null)
            {
                foreach (Vertex vertex in graphDeclaration.vertices)
                {
                    if (vertex.Pozition.Compare(pointF))
                    {
                        AddEdge(vertex, ActualVertexAddEdge, useXJoin, useYJoin);
                    }
                }
            }
        }
    }
}
