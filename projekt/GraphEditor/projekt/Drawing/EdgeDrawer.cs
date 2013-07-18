using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Controls;
using System.Windows;

namespace GraphEditor.Drawing
{
    /// <summary>
    /// Class for drawing Edges
    /// </summary>
    class EdgeDrawer :IDrawer
    {
        Edge edge;
        Dictionary<Line2D, Line2DDrawer> cache;

        /// <summary>
        /// creates new instance of edge drawer
        /// </summary>
        /// <param name="edge">Edge for drawing</param>
        public EdgeDrawer(Edge edge)
        {
            this.edge = edge;
            cache = new Dictionary<Line2D, Line2DDrawer>();
        }


        /// <summary>
        /// Basic 3D draw method
        /// </summary>
        /// <param name="graphicsDevice">device to draw on</param>
        /// <param name="world">world matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="surfaceType">surface to be projected on</param>
        public void Draw3D(GraphicsDevice graphicsDevice, Matrix world, Matrix view, Matrix projection, SurfaceTypeEnum surfaceType)
        {
            foreach (Line2D line in edge.lines)
            {                
                GetLineDrawer(line).Draw3D(graphicsDevice, world, view, projection, surfaceType);                
            }
        }

        /// <summary>
        /// getting drawer from cache or create new and store to cache
        /// </summary>
        /// <param name="line"></param>
        /// <returns>valid drawer for line</returns>
 
        private Line2DDrawer GetLineDrawer(Line2D line)
        {
            Line2DDrawer lineDrawer;
            if (!cache.TryGetValue(line, out lineDrawer))
            {
                lineDrawer = new Line2DDrawer(line);
                cache.Add(line, lineDrawer);
            }
            return lineDrawer;
        }

        /// <summary>
        /// draw  on 2D canvas
        /// </summary>
        /// <param name="canvas">canvas to draw on</param>
        /// <param name="vertexContextMenu">context menu which is attached to each vertex</param>
        /// <param name="dependencyPropertyVertex">dependency property to store vertex pointer</param>
        /// <param name="dependencyPropertyLine2D">dependency property to store line pointer </param>
        /// <param name="AddPointFunction"></param>
        /// <param name="DelleteEdgeFunction"></param>
        /// <param name="invalidateFunction"></param>
        public void Draw2D(Canvas canvas, ContextMenu vertexContextMenu, DependencyProperty dependencyPropertyVertex, DependencyProperty dependencyPropertyLine2D, Action<object, RoutedEventArgs> AddPointFunction, Action<object, RoutedEventArgs> DelleteEdgeFunction, Func<bool> invalidateFunction)
        {
            foreach (var line in edge.lines)
            {                
                GetLineDrawer(line).Draw2D(canvas, AddPointFunction, dependencyPropertyLine2D, invalidateFunction, DelleteEdgeFunction);
            }
            for (int i = 1; i < (edge.points.Count - 1); i++)
            {
                PointDrawer pointDrawer = new PointDrawer(edge.points[i]);
                pointDrawer.Draw2D(canvas, vertexContextMenu, dependencyPropertyVertex);                
            }
        }
    }
}