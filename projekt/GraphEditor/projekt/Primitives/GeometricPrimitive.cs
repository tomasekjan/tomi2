using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GraphEditor.Primitives3D
{    
    [Serializable]
    public abstract class GeometricPrimitive :IDisposable
    {        
        List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
        List<ushort> indices = new List<ushort>();

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect basicEffect;

        /// <summary>
        /// add vertex with position same as normal
        /// </summary>
        /// <param name="pozition">position and normal vector</param>
        protected void AddVertex(Vector3 pozition)
        {
            vertices.Add(new VertexPositionNormal(pozition,pozition));
        }

        /// <summary>
        /// add vertices with position, each position will be tranformed with given matrix
        /// </summary>
        /// <param name="transofmMatrix">matrix to transform with</param>
        /// <param name="positions">positions</param>
        protected void AddVertexs(Matrix transofmMatrix , params Vector3[] positions)
        {
            foreach (Vector3 v in positions)
            {
                AddVertex(Vector3.Transform(v,transofmMatrix));
            }
        }

        /// <summary>
        /// add vertex with position and normal
        /// </summary>
        /// <param name="position">position of vertex</param>
        /// <param name="normal">normal of vertex</param>
        protected void AddVertex(Vector3 position, Vector3 normal)
        {
            vertices.Add(new VertexPositionNormal(position, normal));
        }

        /// <summary>
        /// transforms all vertices with given transformation
        /// </summary>
        /// <param name="transformMatrix">transform matrix to transform with</param>
        public void Transform(Matrix transformMatrix)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = new VertexPositionNormal(Vector3.Transform(vertices[i].Position, transformMatrix), Vector3.Transform(vertices[i].Normal, transformMatrix));
            }
        }
        
        /// <summary>
        /// add new index to indexes objects
        /// </summary>
        /// <param name="index"></param>
        /// <<exception cref="System.OverflowException">only values in rang of unsigned short are allowed</exception>
        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
            {
                throw new OverflowException();
            }
            indices.Add((ushort)index);
        }

        /// <summary>
        /// add multiple indexes to indexes objects
        /// </summary>
        /// <param name="indexes"></param>
        protected void AddIndexes(params int[] indexes)
        {
            foreach (int i in indexes)
            {
                AddIndex(i);
            }
        }

        protected int VerticesCount
        {
            get { return vertices.Count; }
        }

        /// <summary>
        /// initializing buffers
        /// </summary>
        /// <param name="graphicsDevice">device to show on</param>
        protected void InitializePrimitive(GraphicsDevice graphicsDevice)
        {
         
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormal), vertices.Count, BufferUsage.None);
            vertexBuffer.SetData(vertices.ToArray());         
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), indices.Count, BufferUsage.None);

            indexBuffer.SetData(indices.ToArray());
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.LightingEnabled = false;
            basicEffect.EnableDefaultLighting();            
        }

        /// <summary>
        /// draw primitive
        /// </summary>
        /// <param name="effect"></param>
        public void Draw(Effect effect)
        {
            GraphicsDevice graphicsDevice = effect.GraphicsDevice;

            // Set our vertex declaration, vertex buffer, and index buffer.
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            graphicsDevice.Indices = indexBuffer;            
            

            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();

                int primitiveCount = indices.Count / 3;               
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                     vertices.Count, 0, primitiveCount);

            }
        }

        /// <summary>
        /// draw primitive
        /// </summary>
        /// <param name="world">word matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="color">color</param>
        public void Draw(Matrix world, Matrix view, Matrix projection, Color color)
        {
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.DiffuseColor = color.ToVector3();
            basicEffect.Alpha = 1;

            GraphicsDevice device = basicEffect.GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;

            device.BlendState = BlendState.Opaque;        
            Draw(basicEffect);
        }

        /// <summary>
        /// disposing buffers
        /// </summary>
        public void Dispose()
        {
            basicEffect.Dispose();
            indexBuffer.Dispose();
            vertexBuffer.Dispose();
        }
    }
}
