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


        public override void Draw3D(GraphicsDevice graphicsDevice, xna.Matrix world, xna.Matrix view, xna.Matrix projection, SurfaceTypeEnum surfaceType, GeometricPrimitive sphere)
        {
            Vertex vertex = point as Vertex;
            if (!vertex.isValid)
            {
                vertex.isValid = true;
            }
            //counting rotation angle
            float rotationXt = (xna.MathHelper.TwoPi) * vertex.Pozition.X;
            float rotationYt = (xna.MathHelper.TwoPi) * vertex.Pozition.Y;

            switch (surfaceType)
            {
                case SurfaceTypeEnum.Sphere:
                    #region EnumCode
                    xna.Vector3 position = new xna.Vector3(0, 0, 1);
                    float transformangleZ = -rotationXt;
                    float transformangleY = rotationYt;
                    position.X = (float)Math.Sin(transformangleZ) * (float)Math.Sin(transformangleY);
                    position.Y = (float)Math.Sin(transformangleZ) * (float)Math.Cos(transformangleY);
                    position.Z = (float)Math.Cos(transformangleZ);
                    position *= 0.5f;
                    xna.Matrix tranform = xna.Matrix.CreateTranslation(position);
                    sphere.Draw(tranform * world, view, projection, vertex.Color);
                    #endregion

                    #region ProjectFinishCode
                    //xna.Matrix translation = xna.Matrix.CreateTranslation(0.5f, 0, 0);
                    //xna.Matrix rotationY = xna.Matrix.CreateRotationY(rotationYt);
                    //xna.Matrix rotatuonZ = xna.Matrix.CreateRotationZ(-rotationXt);
                    //xna.Matrix tmp;
                    //xna.Quaternion q;
                    //q = xna.Quaternion.CreateFromAxisAngle(new xna.Vector3(0, 1, 0), rotationYt);
                    //q *= xna.Quaternion.CreateFromAxisAngle(new xna.Vector3(0, 0, 1), -rotationXt);
                    //xna.Matrix.CreateFromQuaternion(ref q, out tmp);
                    ////xna.Matrix tranform = translation * rotatuonZ * rotationY;                 
                    //xna.Matrix tranform = translation * tmp;
                    //sphere.Draw(tranform * world, view, projection, vertex.Color);
                    #endregion

                    break;
                case SurfaceTypeEnum.Torus:
                    xna.Matrix moveTomidleOfTorus = xna.Matrix.CreateTranslation(0.5f, 0, 0);
                    xna.Matrix rotationX = xna.Matrix.CreateRotationZ(rotationXt);
                    xna.Matrix rotationY2 = xna.Matrix.CreateRotationY(rotationYt);
                    xna.Matrix moveOnSideOfTorus = xna.Matrix.CreateTranslation(0.2f, 0, 0);
                    sphere.Draw(moveOnSideOfTorus * rotationX * moveTomidleOfTorus * rotationY2 * world, view, projection, vertex.Color);
                    break;                
                default:
                    break;
            }
        }

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
