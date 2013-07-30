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
using Xna=Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using GraphEditor.Drawing;
using xna= Microsoft.Xna.Framework;
using GraphEditor.Exceptions;
using System.Collections;

namespace GraphEditor.GraphDeclaration
{  
    public sealed partial class Graph
    {        
        GeometricPrimitive sufaceCache;                
        Boolean isSurfaceValid;               
        DependencyProperty dependencyPropertyVertex;        
        DependencyProperty dependencyPropertyLine2D;
        Func<bool> invalidateFunction;
        
        SubPoint actualVertexMove = null;
        Vertex actualVertexAddEdge = null;        
        GraphicsDevice graphicsDevice;
        public GraphDefinition graphDeclaration;
        UndoRedeSerializer serializer;
        Dictionary<Edge, EdgeDrawer> edgeDrawerCache;
        
        /// <summary>
        /// gets instance of valid surface
        /// </summary>
        private GeometricPrimitive Surface
        {
            get
            {
                if (!isSurfaceValid)                
                {
                    GeometricPrimitive primitive = null;
                    switch (SurfaceType)
                    {
                        case SurfaceTypeEnum.Sphere:
                            primitive = new SpherePrimitive(graphicsDevice);
                            break;
                        case SurfaceTypeEnum.Torus:
                            primitive = new TorusPrimitive(graphicsDevice);
                            break;                        
                        default:
                            throw new NotImplementedException();
                    }
                    sufaceCache = primitive;
                    isSurfaceValid = true;                    
                }
                return sufaceCache;
            }
        }
                
        public SurfaceTypeEnum SurfaceType
        {
            get
            {
                return graphDeclaration.suraceType;
            }
            set
            {
                Action();
                isSurfaceValid = false;
                graphDeclaration.suraceType = value;
            }
        }

        /// <summary>
        /// set surface color
        /// </summary>
        /// <param name="color"></param>
        public void SetSurfaceColor(Xna.Color color)
        {
            Action();
            graphDeclaration.surfaceColor = color;
        }

        /// <summary>
        /// create an empty graph object
        /// </summary>
        /// <param name="invalidateFunction">invalidate function called on each redraw</param>
        public Graph(Func<bool> invalidateFunction)
        {
            edgeDrawerCache = new Dictionary<Edge, EdgeDrawer>();
            this.invalidateFunction = invalidateFunction;
            graphDeclaration = new GraphDefinition();    
            dependencyPropertyVertex = DependencyProperty.Register("Point", typeof(PointDrawer), typeof(Graph));
            dependencyPropertyLine2D = DependencyProperty.Register("Edge", typeof(Line2DDrawer), typeof(Graph));
            
            this.serializer = new UndoRedeSerializer();
            SurfaceType = SurfaceTypeEnum.Torus;            
        }

        /// <summary>
        /// setting graphic device for xna drawing
        /// </summary>
        /// <param name="device"></param>
        public void SetGraphicDevice(GraphicsDevice device)
        {
            this.graphicsDevice = device;
            pointSphereCache = new SpherePrimitive(graphicsDevice, 0.04f, 32);
        }

        /// <summary>
        /// add vertex to graph
        /// </summary>
        /// <param name="vertex">vertex to be added</param>
        public void AddVertex(Vertex vertex)
        {
            Action();
            graphDeclaration.vertices.Add(vertex);
        }

        /// <summary>
        /// Create new vertex on given position
        /// </summary>
        /// <param name="pointF">vertex position for new vertex get default color - black</param>
        public void AddVertex(PointF pointF)
        {   
            AddVertex(new Vertex(pointF, xna.Color.Black));
        }

        /// <summary>
        /// add edge to graph
        /// </summary>
        /// <param name="edge">new edge</param>
        public void AddEdge (Edge edge)
        {
            Action();
            if (!graphDeclaration.edges.Contains(edge))
            {
                graphDeclaration.edges.Add(edge);
            }
        }

        private void AddEdge(Vertex vertex, Vertex ActualVertexAddEdge)
        {            
            AddEdge(new Edge(vertex, ActualVertexAddEdge, xna.Color.Blue));
        }

        /// <summary>
        /// get vertex with given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vertex GetByIndex(int index)
        {            
            return graphDeclaration.vertices[index];
        }

        // take from cache or create new Edge Drawer
        private EdgeDrawer GetEdgeDraver(Edge edge)
        {
            EdgeDrawer edgeDrawer;
            if (!edgeDrawerCache.TryGetValue(edge,out edgeDrawer))
            {
                edgeDrawer = new EdgeDrawer(edge);
                edgeDrawerCache.Add(edge, edgeDrawer);
            }
            return edgeDrawer;
        }

        // cache for vertex 3d visualization
        GeometricPrimitive pointSphereCache;

        /// <summary>
        /// draw 3D using xna
        /// </summary>
        /// <param name="world">word matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <exception cref="Draw3DException">throws when there is problem with 3D drawing</exception>
        public void Draw3D(Matrix world, Matrix view, Matrix projection)
        {
            try
            {
                Surface.Draw(world, view, projection, graphDeclaration.surfaceColor);
                foreach (Edge edge in graphDeclaration.edges)
                {
                    GetEdgeDraver(edge).Draw3D(graphicsDevice, world, view, projection, SurfaceType);                    
                }
                foreach (Vertex vertex in graphDeclaration.vertices)
                {
                    VertexDrawer vertexDrawer = new VertexDrawer(vertex);
                    vertexDrawer.Draw3D(graphicsDevice, world, view, projection, SurfaceType, pointSphereCache);
                }
            }
            catch (Exception exception)
            {
                throw new Draw3DException(exception);
            }
        }

        /// <summary>
        /// draw entire graph on canvas
        /// </summary>
        /// <param name="canvas"></param>
        public void Draw2D(Canvas canvas)
        {
            foreach (Edge edge in graphDeclaration.edges)
            {                
                GetEdgeDraver(edge).Draw2D(canvas, CreateVertexContextMenu(), dependencyPropertyVertex, dependencyPropertyLine2D, AddPoint, DeleteEdge, invalidateFunction);                
            }
            foreach (Vertex vertex in graphDeclaration.vertices)
            {
                VertexDrawer vertexDrawer = new VertexDrawer(vertex);
                vertexDrawer.Draw2D(canvas, CreateVertexContextMenu(), dependencyPropertyVertex);
            }
        }

        public void SetValuesOnDeserializing()
        {
            isSurfaceValid = false;            
        }

        /// <summary>
        /// delete all vertices an edges
        /// </summary>
        public void Clear()
        {
            Action();
            graphDeclaration.vertices.Clear();
            graphDeclaration.edges.Clear();            
            SurfaceType = SurfaceTypeEnum.Torus;
            invalidateFunction();
        }

        /// <summary>
        /// undo last action
        /// </summary>
        public void Undo()
        {
            try
            {
                Action();
                GraphDefinition tmp = serializer.Undo();
                if (tmp != null)
                {
                    graphDeclaration = tmp;
                }
                invalidateFunction();
            }
            catch (Exception exception)
            {
                throw new UndoRedoException("Undo Error",exception);
            }
        }

        /// <summary>
        /// redo last action
        /// </summary>
        public void Redo()
        {
            try
            {
                GraphDefinition tmp = serializer.Redo();
                if (tmp != null)
                {
                    graphDeclaration = tmp;
                }
                invalidateFunction();
            }
            catch (Exception exception)
            {
                throw new UndoRedoException("Redo Error", exception);
            }
        }

        /// <summary>
        /// action which is stored for undo/redo
        /// </summary>
        public void Action()
        {
            serializer.Action(graphDeclaration);
        }
    }
}
