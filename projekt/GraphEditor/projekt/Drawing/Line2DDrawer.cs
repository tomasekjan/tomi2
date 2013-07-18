using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using GraphEditor.Primitives3D;
using xna= Microsoft.Xna.Framework;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor.Drawing
{
    class Line2DDrawer :IDrawer
    {
        internal Line2D line2D;
        GeometricPrimitive line;
        Func<bool> invalidateFunction;
        SurfaceTypeEnum readyForSurface;

        /// <summary>
        /// crete new instance of line drawer
        /// </summary>
        /// <param name="line2D"></param>
        public Line2DDrawer(Line2D line2D)
        {
            this.line2D = line2D;
        }

        /// <summary>
        /// Basic 3D draw method
        /// </summary>
        /// <param name="graphicsDevice">device to draw on</param>
        /// <param name="world">world matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="surfaceType">surface to be projected on</param>
        public void Draw3D(xna.Graphics.GraphicsDevice graphicsDevice, xna.Matrix world, xna.Matrix view, xna.Matrix projection, SurfaceTypeEnum curentSurfaceType)
        {
            if ((!(line2D.PointBegin.isValid && line2D.PointEnd.isValid)) || line == null || curentSurfaceType != readyForSurface)
            {
                
                line = new LinePrimtive(graphicsDevice, line2D.PointBegin.Pozition, line2D.PointEnd.Pozition, line2D.WrappedHorizontal, line2D.WrapedVertical, curentSurfaceType);
                readyForSurface = curentSurfaceType;
            }

            line.Draw(world, view, projection, line2D.Color);
        }

        /// <summary>
        /// Creating context menu for line
        /// </summary>
        /// <param name="AddPointFunction">delegate to function for add new sub point</param>
        /// <param name="DelleteEdgeFunction">delegate to function for delete edge</param>
        /// <returns>new context menu</returns>
        private ContextMenu CreateLine2DContextMenu(Action<object, RoutedEventArgs> AddPointFunction, Action<object, RoutedEventArgs> DelleteEdgeFunction)
        {
            ContextMenu vertexContextMenu = new ContextMenu();

            MenuItem addPointMenuItem = new MenuItem()
            {
                Header = "Add Point",
                IsCheckable = false
            };
            addPointMenuItem.Click += new RoutedEventHandler(AddPointFunction.Invoke);

            MenuItem useXJoinMenuItem = new MenuItemWithWrapNumber()
            {
                JoinName = "X",
                WrapNumber = line2D.WrappedHorizontal,
                IsCheckable = false
            };
            useXJoinMenuItem.Click += new RoutedEventHandler(useXJoinMenuItem_Click);

            MenuItem useYJoinMenuItem = new MenuItemWithWrapNumber()
            {
                JoinName = "Y",
                WrapNumber = line2D.WrapedVertical,
                IsCheckable = false
            };
            useYJoinMenuItem.Click += new RoutedEventHandler(useYJoinMenuItem_Click);

            MenuItem delleteEdgeMenuItem = new MenuItem()
            {
                Header = "Delete",
                IsCheckable = false
            };
            delleteEdgeMenuItem.Click += new RoutedEventHandler(DelleteEdgeFunction.Invoke);

            MenuItem changeColorMenuItem = new MenuItem()
            {
                Header = "Change Color",
                IsCheckable = false
            };
            changeColorMenuItem.Click += new RoutedEventHandler(changeColorMenuItem_Click);

            vertexContextMenu.Items.Add(addPointMenuItem);
            vertexContextMenu.Items.Add(useXJoinMenuItem);
            vertexContextMenu.Items.Add(useYJoinMenuItem);
            vertexContextMenu.Items.Add(delleteEdgeMenuItem);
            vertexContextMenu.Items.Add(changeColorMenuItem);
            return vertexContextMenu;
        }

        void changeColorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.ColorDialog();
            dialog.ShowDialog();
            line2D.ChangeColor(dialog.Color);
            invalidateFunction();
        }

        void useYJoinMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItemWithWrapNumber useYJoinMenuItem = (MenuItemWithWrapNumber)sender;
            useYJoinMenuItem.SwitchWrapnumber();
            line2D.WrapedVertical = useYJoinMenuItem.WrapNumber;
            invalidateFunction.Invoke();
        }

        void useXJoinMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItemWithWrapNumber useXJoinMenuItem = (MenuItemWithWrapNumber)sender;
            useXJoinMenuItem.SwitchWrapnumber();
            line2D.WrappedHorizontal = useXJoinMenuItem.WrapNumber;
            invalidateFunction.Invoke();
        }

        /// <summary>
        /// drawing on canvas
        /// </summary>
        /// <param name="canvas">canvas to draw on</param>
        /// <param name="AddPointFunction">delegate to add point function</param>
        /// <param name="dependencyPropertyLine2D">dependency property to hold line pointer</param>
        /// <param name="invalidateFunction">delegate to invalidate function</param>
        /// <param name="DelleteEdgeFunction">delegate to deleting function</param>
        public void Draw2D(Canvas canvas, Action<object, RoutedEventArgs> AddPointFunction, DependencyProperty dependencyPropertyLine2D, Func<bool> invalidateFunction, Action<object, RoutedEventArgs> DelleteEdgeFunction)
        {            
            this.invalidateFunction = invalidateFunction;
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb((byte)line2D.Color.A, (byte)line2D.Color.R, (byte)line2D.Color.G, (byte)line2D.Color.B));
            ContextMenu line2DContextMenu = CreateLine2DContextMenu(AddPointFunction, DelleteEdgeFunction);

            Line line1 = new Line()
            {
                X1 = line2D.PointBegin.Pozition.X * canvas.RenderSize.Width,
                Y1 = line2D.PointBegin.Pozition.Y * canvas.RenderSize.Height,
                X2 = (line2D.PointEnd.Pozition.X + line2D.WrappedHorizontal) * canvas.RenderSize.Width,
                Y2 = (line2D.PointEnd.Pozition.Y + line2D.WrapedVertical) * canvas.RenderSize.Height,
                Stroke = brush,
                ContextMenu = line2DContextMenu
            };

            Line line2 = new Line()
            {
                X1 = (line2D.PointBegin.Pozition.X - line2D.WrappedHorizontal) * canvas.RenderSize.Width,
                Y1 = (line2D.PointBegin.Pozition.Y - line2D.WrapedVertical) * canvas.RenderSize.Height,
                X2 = (line2D.PointEnd.Pozition.X) * canvas.RenderSize.Width,
                Y2 = (line2D.PointEnd.Pozition.Y) * canvas.RenderSize.Height,
                Stroke = brush,
                ContextMenu = line2DContextMenu
            };

            Line line3 = new Line()
            {
                X1 = (line2D.PointBegin.Pozition.X - line2D.WrappedHorizontal) * canvas.RenderSize.Width,
                Y1 = (line2D.PointBegin.Pozition.Y) * canvas.RenderSize.Height,
                X2 = (line2D.PointEnd.Pozition.X) * canvas.RenderSize.Width,
                Y2 = (line2D.PointEnd.Pozition.Y + line2D.WrapedVertical) * canvas.RenderSize.Height,
                Stroke = brush,
                ContextMenu = line2DContextMenu
            };

            Line line4 = new Line()
            {
                X1 = (line2D.PointBegin.Pozition.X) * canvas.RenderSize.Width,
                Y1 = (line2D.PointBegin.Pozition.Y - line2D.WrapedVertical) * canvas.RenderSize.Height,
                X2 = (line2D.PointEnd.Pozition.X + line2D.WrappedHorizontal) * canvas.RenderSize.Width,
                Y2 = (line2D.PointEnd.Pozition.Y) * canvas.RenderSize.Height,
                Stroke = brush,
                ContextMenu = line2DContextMenu
            };

            foreach (MenuItem menu in line2DContextMenu.Items)
            {
                menu.SetValue(dependencyPropertyLine2D, this);
            }

            canvas.Children.Add(line1);
            canvas.Children.Add(line2);
            canvas.Children.Add(line3);
            canvas.Children.Add(line4);
        }


    }   
}
