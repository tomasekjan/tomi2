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
        /// <summary>
        /// left mouse button released, move vertex if selected
        /// </summary>
        /// <param name="pointF">position of mouse event</param> 
        public void MouseLeftUp(PointF pointF)
        {
            if (actualVertexMove != null)
            {
                Action();
                actualVertexMove.Pozition = pointF;                
            }
        }

        /// <summary>
        /// left mouse button pressed, if is nearby some point store it to actualVertexMove
        /// </summary>
        /// <param name="pointF">position of mouse event</param>
        /// <returns></returns>
        public bool MouseLeftDown(PointF pointF)
        {
            actualVertexMove = null;
            foreach (Vertex vertex in graphDeclaration.vertices)
            {
                if (vertex.Pozition.Compare(pointF))
                {
                    actualVertexMove = vertex;
                    return true;
                }
            }

            foreach (Edge edge in graphDeclaration.edges)
            {
                if (edge.HaveVertex(pointF))
                {
                    actualVertexMove = edge.getVertex(pointF);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// middle button pressed if is nearby some point store it to actualVertexAddEdge
        /// </summary>
        /// <param name="pointF">position of mouse event</param>
        /// <returns></returns>
        public bool MouseMiddletDown(PointF pointF)
        {
            actualVertexAddEdge = null;
            foreach (Vertex vertex in graphDeclaration.vertices)
            {
                if (vertex.Pozition.Compare(pointF))
                {
                    actualVertexAddEdge = vertex;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// middle button released if there is actualvertexAddEdge and button is released near by some other vertex
        /// </summary>
        /// <param name="pointF">position of mouse event</param>
        public void MouseMiddletUp(PointF pointF)
        {
            if (actualVertexAddEdge != null)
            {
                foreach (Vertex vertex in graphDeclaration.vertices)
                {
                    if (vertex.Pozition.Compare(pointF))
                    {
                        AddEdge(vertex, actualVertexAddEdge);
                    }
                }
            }
        }
    }
}
