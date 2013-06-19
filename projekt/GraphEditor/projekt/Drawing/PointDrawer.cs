﻿using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.GraphDeclaration;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using GraphEditor.Primitives3D;

namespace GraphEditor.Drawing
{
    class PointDrawer
    {
        internal SubPoint point;

        public PointDrawer(SubPoint point)
        {
            this.point = point;
        }

        //sub points are not shown on 3D projection
        public virtual void Draw3D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Matrix world, Microsoft.Xna.Framework.Matrix view, Microsoft.Xna.Framework.Matrix projection, SurfaceTypeEnum surfaceType, GeometricPrimitive sphere)
        {
            throw new NotImplementedException();
        }

        public virtual void Draw2D(Canvas canvas, ContextMenu vertexContextMenu, DependencyProperty dependencyProperty)
        {
            System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
            SolidColorBrush brush = new SolidColorBrush(Colors.Gray);
            rectangle.Fill = brush;
            rectangle.Height = point.Size;
            rectangle.Width = point.Size;
            rectangle.Margin = new Thickness((point.Pozition.X * canvas.RenderSize.Width) - rectangle.Height / 2, (point.Pozition.Y * canvas.RenderSize.Height) - rectangle.Height / 2, 0, 0);
            rectangle.Name = "a";
            rectangle.ContextMenu = vertexContextMenu;
            ((MenuItem)(rectangle.ContextMenu.Items[0])).SetValue(dependencyProperty, this);
            if (rectangle.ContextMenu.Items.Count > 1)
            {
                rectangle.ContextMenu.Items.RemoveAt(1);
            }
            canvas.Children.Add(rectangle);
        }  
    }
}
