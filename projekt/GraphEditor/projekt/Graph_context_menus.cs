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
using GraphEditor.Drawing;

namespace GraphEditor.GraphDeclaration
{
    public sealed partial class Graph
    {
        /// <summary>
        /// Creates new context menu which is shown on right click on vertex
        /// </summary>
        /// <returns></returns>
        private ContextMenu CreateVertexContextMenu()
        {
            ContextMenu vertexContextMenu = new ContextMenu();
            MenuItem DeletemenuItem = new MenuItem()
            {
                Header = "Delete",
                IsCheckable = false};
            MenuItem ChangeColormenuItem = new MenuItem()
            {
                Header = "Change Color",
                IsCheckable = false};
            ChangeColormenuItem.Click += new RoutedEventHandler(ChangeColormenuItem_Click);
            DeletemenuItem.Click += new RoutedEventHandler(deleteMenuItem_Click);
            vertexContextMenu.Items.Add(DeletemenuItem);
            vertexContextMenu.Items.Add(ChangeColormenuItem);
            return vertexContextMenu;
        }

        /// <summary>
        /// Handling change color event on vertex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        void ChangeColormenuItem_Click(object sender, RoutedEventArgs e)
        {
            Action();
            MenuItem deleteMenuItem = (MenuItem)sender;
            PointDrawer pointDrawer = deleteMenuItem.GetValue(dependencyPropertyVertex) as PointDrawer;
            SubPoint vertex = pointDrawer.point;
            var dialog =new System.Windows.Forms.ColorDialog();
            dialog.ShowDialog();
            vertex.ChangeColor(dialog.Color);
            invalidateFunction();
        }

        /// <summary>
        /// add point function for edge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddPoint(object sender, RoutedEventArgs e)
        {
            Action();
            MenuItem addPointItem = sender as MenuItem;
            Line2DDrawer lineDrawer = addPointItem.GetValue(dependencyPropertyLine2D) as Line2DDrawer;
            Line2D line = lineDrawer.line2D;
            KeyValuePair<int, Edge> pair = findEdge(line);
            // TODO repair this
            if (pair.Key == -1)
            {
                return;
            }
            pair.Value.addPoint(pair.Key);
            invalidateFunction.Invoke();
        }

        /// <summary>
        /// delete selected edge from graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        void DeleteEdge(object sender, RoutedEventArgs e)
        {
            Action();
            MenuItem delleteEdgeItem = sender as MenuItem;
            Line2DDrawer lineDrawer = delleteEdgeItem.GetValue(dependencyPropertyLine2D) as Line2DDrawer;
            Line2D line = lineDrawer.line2D;
            KeyValuePair<int, Edge> pair = findEdge(line);
            graphDeclaration.edges.Remove(pair.Value);
            if (edgeDrawerCache.Count > 1000)
            {
                edgeDrawerCache.Clear();
            }
            invalidateFunction();
        }

        /// <summary>
        /// find which edge has given line as part
        /// </summary>
        /// <param name="line">line to search for</param>
        /// <returns>return edge as value and line index as key</returns>
        private KeyValuePair<int, Edge> findEdge(Line2D line)
        {
            foreach (Edge edge in graphDeclaration.edges)
            {
                int index = edge.Containts(line);
                if (index != -1)
                {
                    return new KeyValuePair<int, Edge>(index, edge);
                }
            }
            return new KeyValuePair<int, Edge>(-1, null);
        }

        /// <summary>
        /// handling delete event on ponint or vertex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Action();
            MenuItem deleteMenuItem = (MenuItem)sender;
            PointDrawer pointDrawer = deleteMenuItem.GetValue(dependencyPropertyVertex) as PointDrawer;
            SubPoint vertex2Delete = pointDrawer.point;
            if (vertex2Delete is Vertex)
            {
                graphDeclaration.vertices.Remove((Vertex)vertex2Delete);
                List<Edge> list2Delete = new List<Edge>();
                foreach (Edge edge in graphDeclaration.edges)
                {
                    if (edge.HaveVertexBeginOrEnd((Vertex)vertex2Delete))
                    {
                        list2Delete.Add(edge);
                    }
                }
                foreach (Edge edge in list2Delete)
                {
                    graphDeclaration.edges.Remove(edge);
                }
            }
            else
            {
                foreach (Edge edge in graphDeclaration.edges)
                {
                    if (edge.HaveVertex(vertex2Delete))
                    {
                        edge.delete(vertex2Delete);
                    }
                }
            }
            invalidateFunction.Invoke();
        }
    }
}
