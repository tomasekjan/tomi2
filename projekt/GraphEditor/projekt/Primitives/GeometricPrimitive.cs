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

        protected void AddVertex(Vector3 pozition)
        {
            vertices.Add(new VertexPositionNormal(pozition,pozition));
        }
        protected void AddVertexs(Matrix m , params Vector3[] pozitions)
        {
            foreach (Vector3 v in pozitions)
            {
                AddVertex(Vector3.Transform(v,m));
            }
        }

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

        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
            {
                throw new OverflowException();
            }
            indices.Add((ushort)index);
        }

        protected void AddIndexs(params int[] indexs)
        {
            foreach (int i in indexs)
            {
                AddIndex(i);
            }
        }
        protected int CurrentVertex
        {
            get { return vertices.Count; }
        }

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
        public void Draw(Matrix world, Matrix view, Matrix projection, Color color)
        {
            // Set BasicEffect parameters.
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.DiffuseColor = color.ToVector3();
            basicEffect.Alpha = color.A / 255.0f;            

            GraphicsDevice device = basicEffect.GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;

            if (color.A < 255)
            {
                // Set renderstates for alpha blended rendering.
                device.BlendState = BlendState.AlphaBlend;
            }
            else
            {
                // Set renderstates for opaque rendering.
                device.BlendState = BlendState.Opaque;
            }
                        
            Draw(basicEffect);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
