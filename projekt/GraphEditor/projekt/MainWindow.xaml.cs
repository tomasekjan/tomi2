using System;
using System.Windows;
using Microsoft.Xna.Framework;
using GraphEditor;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using GraphEditor.GraphDeclaration;
using AddInView;
using System.Globalization;
using Plugin;
using System.Windows.Annotations;


namespace GraphEditor
{

    public partial class MainWindow : Window
    {
        #region contants
        /// <summary>
        /// On load rotation.
        /// </summary>
        private const float DEFAULT_ROTATION = 1.5f;
        /// <summary>
        /// On load zoom level
        /// </summary>
        private const int ZOOM_LEVEL = 1;
        /// <summary>
        /// Added/Substracted to rotationX/rotationY/rotationZ by move keys
        /// </summary>
        private const int DEFAULT_ROTATiON_DELTA = 10;
        /// <summary>
        /// Added/Substracted to ZoomLevel by +/- keys
        /// </summary>
        private const float ZOOM_LEVEL_DELTA = 0.1f;
        /// <summary>
        /// On load surface color - can be changed in settings
        /// </summary>
        private readonly static Color DEFAULT_SURFACE_COLOR = Color.Red;
        /// <summary>
        /// On load background color
        /// </summary>
        private readonly static Color DEFAULT_BACKGROUND_COLOR = Color.Wheat;
        #endregion

        #region local varianles
        /// <summary>
        /// decides whether use Wire Frame rendering
        /// </summary>
        bool isWireFrame = false;
        private float rotationX = DEFAULT_ROTATION;
        private float rotationY = 0;
        private float rotationZ = DEFAULT_ROTATION;
        private float zoomLevel = ZOOM_LEVEL;
        private Color SurfaceColor = DEFAULT_SURFACE_COLOR;
        private Color BackGroungColor = DEFAULT_BACKGROUND_COLOR;
        ControlState controlState;
        string CurentFileName = string.Empty;
        Graph graph;
        
        #endregion

        #region load
        public MainWindow()
        {
            InitializeComponent();
            canvas.SizeChanged += new SizeChangedEventHandler(canvas_SizeChanged);
        }

        bool Invalidate()
        {
            canvas.Children.Clear();
            if (graph != null)
            {
                graph.Draw2D(canvas);
            }
            return true;
        }

        private void loadContent(object sender, GraphicsDeviceEventArgs e)
        {
            graph = new Graph(Invalidate);
            graph.Draw2D(canvas);
            graph.SetSurfaceColor(SurfaceColor);
            xnaControl1.RenderXna += xnaControl1_RenderXna;
        }
        #endregion

        #region XnaControl
        private void xnaControl1_RenderXna(object sender, GraphicsDeviceEventArgs e)
        {
            graph.SetGraphicDevice(e.GraphicsDevice);
            var wireFrameState = new RasterizerState()
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None};

            if (isWireFrame)
            {
                e.GraphicsDevice.RasterizerState = wireFrameState;
            }
            else
            {
                e.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            }

            e.GraphicsDevice.Clear(BackGroungColor);
            Matrix world = Matrix.CreateFromYawPitchRoll(rotationX, rotationY, rotationZ);
            world *= Matrix.CreateScale(zoomLevel);
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 2f), Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, e.GraphicsDevice.Viewport.AspectRatio, 1, 10);

            try
            {
                graph.Draw3D(world, view, projection);
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message);
            }
        }



        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Right)
            {
                rotationX += MathHelper.ToRadians(DEFAULT_ROTATiON_DELTA);
            }
            if (e.Key == System.Windows.Input.Key.Left)
            {
                rotationX -= MathHelper.ToRadians(DEFAULT_ROTATiON_DELTA);
            }
            if (e.Key == System.Windows.Input.Key.O)
            {
                rotationY += MathHelper.ToRadians(DEFAULT_ROTATiON_DELTA);
            }
            if (e.Key == System.Windows.Input.Key.P)
            {
                rotationY -= MathHelper.ToRadians(DEFAULT_ROTATiON_DELTA);
            }
            if (e.Key == System.Windows.Input.Key.Down)
            {
                rotationZ += MathHelper.ToRadians(DEFAULT_ROTATiON_DELTA);
            }
            if (e.Key == System.Windows.Input.Key.Up)
            {
                rotationZ -= MathHelper.ToRadians(DEFAULT_ROTATiON_DELTA);
            }
            if (e.Key == System.Windows.Input.Key.Add)
            {
                zoomLevel += ZOOM_LEVEL_DELTA;
            }
            if (e.Key == System.Windows.Input.Key.Subtract)
            {
                zoomLevel -= ZOOM_LEVEL_DELTA;
            }
            if (e.Key == System.Windows.Input.Key.LeftCtrl)
            {
                controlState.CtrlDown = false;
            }
            if (e.Key == System.Windows.Input.Key.System)
            {
                controlState.AltDown = false;
            }
        }

        private void xnaControl1_HwndMouseMove(object sender, HwndMouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed ||
                e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                rotationX += (float)(e.Position.X - e.PreviousPosition.X) * .01f;
                rotationZ += (float)(e.Position.Y - e.PreviousPosition.Y) * .01f;
            }
        } 
        #endregion

        #region EventHandling
                
        private void canvas_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(canvas);
            System.Drawing.PointF pointF = new System.Drawing.PointF(((float)point.X) / (float)canvas.RenderSize.Width, ((float)point.Y) / (float)canvas.RenderSize.Height);
            switch (controlState.menuSate)
            {
                case MenuState.NONE:
                    if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    {
                        controlState.isLeftDown = true;
                        
                        if (graph.MouseLeftDown(pointF))
                        {
                            canvas.Cursor = System.Windows.Input.Cursors.Hand;
                        }
                    }        
                    break;
                case MenuState.ADD_VERTEX:
                    graph.AddVertex(pointF);
                    controlState.menuSate = MenuState.NONE;
                    Invalidate();
                    break;
                case MenuState.ADD_EDGE:
                    //TODO solve this case
                          
                    if (graph.MouseMiddletDown(pointF))
                    {
                        canvas.Cursor = System.Windows.Input.Cursors.Pen;
                        controlState.isMiddleDown = true;
                    }
                    break;
            }
            
        }

        void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Invalidate();
        }

        private void canvas_MouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            canvas.Cursor = System.Windows.Input.Cursors.Arrow;
            if (controlState.isLeftDown)
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Released)
                {
                    var point = e.GetPosition(canvas);
                    System.Drawing.PointF pointF = new System.Drawing.PointF(((float)point.X) / (float)canvas.RenderSize.Width, ((float)point.Y) / (float)canvas.RenderSize.Height);
                    graph.MouseLeftUp(pointF);
                    Invalidate();
                    controlState.isLeftDown = false;
                }
            }
        }
        
        private void canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                controlState.isMiddleDown = true;
                var point = e.GetPosition(canvas);
                System.Drawing.PointF pointF = new System.Drawing.PointF(((float)point.X) / (float)canvas.RenderSize.Width, ((float)point.Y) / (float)canvas.RenderSize.Height);
                if (graph.MouseMiddletDown(pointF))
                {
                    canvas.Cursor = System.Windows.Input.Cursors.Pen;
                }
                else
                {
                    graph.AddVertex(pointF);
                    Invalidate();
                }
            }
        }
        
        private void canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {            
            if (controlState.isMiddleDown)
            {
                var point = e.GetPosition(canvas);
                System.Drawing.PointF pointF = new System.Drawing.PointF(((float)point.X) / (float)canvas.RenderSize.Width, ((float)point.Y) / (float)canvas.RenderSize.Height);
                graph.MouseMiddletUp(pointF);
                Invalidate();
                controlState.isMiddleDown = false;
                canvas.Cursor = System.Windows.Input.Cursors.Arrow;
            }
            controlState.menuSate = MenuState.NONE;
        }
        
        private void MainForm_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                canvas.Cursor = System.Windows.Input.Cursors.Arrow;
                controlState.menuSate = MenuState.NONE;
            }
            if (e.Key == System.Windows.Input.Key.LeftCtrl)
            {
                controlState.CtrlDown = true;
            }
            if (e.Key == System.Windows.Input.Key.System)
            {
                controlState.AltDown = true;
            }
            if (e.Key == System.Windows.Input.Key.F5)
            {
                RunPlugin(new GraphEmbeddingExponentioal());
            }
            if (e.Key == System.Windows.Input.Key.F1)
            {
                graph.SurfaceType = SurfaceTypeEnum.Sphere;
            }
            if (e.Key == System.Windows.Input.Key.F2)
            {
                graph.SurfaceType = SurfaceTypeEnum.Torus;
            }

            if (controlState.CtrlDown == true)
            {
                if (e.Key == System.Windows.Input.Key.O)
                {
                    OpenFile();
                }
                if (e.Key == System.Windows.Input.Key.Z)
                {
                    graph.Undo();
                }
                if (e.Key == System.Windows.Input.Key.Y)
                {
                    graph.Redo();
                }
                if (e.Key == System.Windows.Input.Key.N)
                {
                    graph.Clear();
                }
                if (e.Key == System.Windows.Input.Key.S)
                {
                    Save();
                }
            }
            
        }

        private void canvas_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            canvas.Cursor = System.Windows.Input.Cursors.Arrow;
            controlState.menuSate = MenuState.NONE;
        }


        #endregion

        #region Menuitems

        private void OpenFile()
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                graph.graphDeclaration = UndoRedeSerializer.Deserialize(fileDialog.FileName);
                graph.SetValuesOnDeserializing();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message);
            }
            Invalidate();
        }

        private void Save()
        {
            if (CurentFileName == string.Empty)
            {
                SaveAs();
            }
            else
            {
                UndoRedeSerializer.Serialize(CurentFileName, graph);
            }
        }

        private void SaveAs()
        {
            try
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.ShowDialog();
                CurentFileName = fileDialog.FileName;
                UndoRedeSerializer.Serialize(fileDialog.FileName, graph);
            }

            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message);
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            graph.Clear();
        }
                
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void ChangeSurfaceColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowDialog();
            SurfaceColor = new Color(dialog.Color.R, dialog.Color.G, dialog.Color.B, dialog.Color.A);
            graph.SetSurfaceColor(SurfaceColor);
        }

        private void ChangeBackGroundColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowDialog();
            BackGroungColor = new Color(dialog.Color.R, dialog.Color.G, dialog.Color.B, dialog.Color.A);

        }

        private void IsWireModel_Click(object sender, RoutedEventArgs e)
        {
            isWireFrame = !isWireFrame;
        }

        private void SphereSurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            graph.SurfaceType = SurfaceTypeEnum.Sphere;
        }

        private void TorusSurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            graph.SurfaceType = SurfaceTypeEnum.Torus;
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            graph.Undo();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            graph.Redo();
        }

        private void RandomPluginButton_Click(object sender, RoutedEventArgs e)
        {
            RunPlugin(new RandomPozition());
        }

        private void EmbedingPluginButton_Click(object sender, RoutedEventArgs e)
        {
            //EmbeddingWindow tmp = new EmbeddingWindow();
            //tmp.Show();
            RunPlugin(new GraphEmbeddingExponentioal());            
        }
        
        private void RunPlugin(IPozitioning plugin)
        {
            graph.graphDeclaration = plugin.Pozitioning(graph.graphDeclaration);
            Invalidate();
        }

        private void AddVertex_Click(object sender, RoutedEventArgs e)
        {
            controlState.menuSate = MenuState.ADD_VERTEX;
            canvas.Cursor = System.Windows.Input.Cursors.Cross;
        }

        private void AddEdge_Click(object sender, RoutedEventArgs e)
        {
            controlState.menuSate = MenuState.ADD_EDGE;
            canvas.Cursor = System.Windows.Input.Cursors.Pen;            
        }
        #endregion                        
    }
}
