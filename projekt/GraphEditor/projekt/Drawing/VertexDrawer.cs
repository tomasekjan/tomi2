using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Controls;
using System.Windows;
using GraphEditor.Primitives3D;
using xna= Microsoft.Xna.Framework;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GraphEditor.Drawing
{
    class VertexDrawer : PointDrawer
    {
        public VertexDrawer(Vertex vertex)
            :base(vertex)
        {
            
            vertex.isValid = false;
        }


        /// <summary>
        /// Basic 3D draw method
        /// </summary>
        /// <param name="graphicsDevice">device to draw on</param>
        /// <param name="world">world matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="surfaceType">surface to be projected on</param>
        public override void Draw3D(GraphicsDevice graphicsDevice, xna.Matrix world, xna.Matrix view, xna.Matrix projection, SurfaceTypeEnum surfaceType, GeometricPrimitive sphere)
        {
            Vertex vertex = point as Vertex;
            if (!vertex.isValid)
            {
                vertex.isValid = true;
            }
            
            
            
            switch (surfaceType)
            {
                case SurfaceTypeEnum.Sphere:
                    float x = vertex.Pozition.X * 0.7f;
                    float y = vertex.Pozition.Y * 0.7f;
                    xna.Vector3 position = new xna.Vector3(0, 0, 1);
                    position.X = x;
                    position.Y = y;
                    position.Z = (float)Math.Sqrt((1 - x*x -y*y));                    
                    position *= 0.7f;
                    xna.Matrix tranform = xna.Matrix.CreateTranslation(position);
                    sphere.Draw(tranform * world, view, projection, vertex.Color);                    

                    break;
                case SurfaceTypeEnum.Torus:
                    float rotationXt = (xna.MathHelper.TwoPi) * vertex.Pozition.X;
                    float rotationYt = (xna.MathHelper.TwoPi) * vertex.Pozition.Y;
                    xna.Matrix moveToMidleOfTorus = xna.Matrix.CreateTranslation(0.5f, 0, 0);
                    xna.Matrix rotationX = xna.Matrix.CreateRotationZ(rotationXt);
                    xna.Matrix rotationY2 = xna.Matrix.CreateRotationY(rotationYt);
                    xna.Matrix moveOnSideOfTorus = xna.Matrix.CreateTranslation(0.2f, 0, 0);
                    sphere.Draw(moveOnSideOfTorus * rotationX * moveToMidleOfTorus * rotationY2 * world, view, projection, vertex.Color);
                    break;                
                default:
                    break;
            }
        }

        /// <summary>
        /// drawing on canvas
        /// </summary>
        /// <param name="canvas">canvas to draw on</param>
        /// <param name="vertexContextMenu">context menu for vertex</param>
        /// <param name="dependencyProperty"></param>
        public override void Draw2D(Canvas canvas, ContextMenu vertexContextMenu, DependencyProperty dependencyProperty)
        {
            Vertex vertex = point as Vertex;
            Ellipse ellipse = new Ellipse();
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb((byte)vertex.Color.A, (byte)vertex.Color.R, (byte)vertex.Color.G, (byte)vertex.Color.B));
            ellipse.Fill = brush;
            ellipse.Height = vertex.Size * 300;
            ellipse.Width = vertex.Size * 300;
            ellipse.Margin = new Thickness((vertex.Pozition.X * canvas.RenderSize.Width) - ellipse.Height / 2, (vertex.Pozition.Y * canvas.RenderSize.Height) - ellipse.Height / 2, 0, 0);
            ellipse.Name = "a";
            ellipse.ContextMenu = vertexContextMenu;
            foreach (MenuItem item in ellipse.ContextMenu.Items)
            {
                item.SetValue(dependencyProperty, this);
            }
            canvas.Children.Add(ellipse);
        }
    }
}
