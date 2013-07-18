using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GraphEditor.Primitives3D
{
    [Serializable]
    public class TorusPrimitive : GeometricPrimitive
    {
        public TorusPrimitive(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1, 0.2f, 32)
        {
        }

        /// <summary>
        /// creating new torus
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="diameter">diameter of torus</param>
        /// <param name="radius">radius of torus</param>
        /// <param name="tessellation">tessellation (bigger means more detail, but possibly slow output) </param>
        public TorusPrimitive(GraphicsDevice graphicsDevice,  float diameter, float radius, int tessellation)
        {
            if (tessellation < 3)
            {
                throw new ArgumentOutOfRangeException("tessellation");
            }

            //Main loop
            for (int i = 0; i < tessellation; i++)
            {
                float outerAngle = i * MathHelper.TwoPi / tessellation;
                Matrix transform = Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(outerAngle);    

                //One disc
                for (int j = 0; j < tessellation; j++)
                {
                    float innerAngle = j * MathHelper.TwoPi / tessellation;

                    float dx = (float)Math.Cos(innerAngle);
                    float dy = (float)Math.Sin(innerAngle);
                    
                    Vector3 normal = new Vector3(dx, dy, 0);
                    Vector3 position = normal * radius;
                    
                    position = Vector3.Transform(position, transform);
                    normal = Vector3.TransformNormal(normal, transform);

                    AddVertex(position, normal);
                    int nextI = (i + 1) % tessellation;
                    int nextJ = (j + 1) % tessellation;

                    AddIndex(i * tessellation + j);
                    AddIndex(i * tessellation + nextJ);
                    AddIndex(nextI * tessellation + j);

                    AddIndex(i * tessellation + nextJ);
                    AddIndex(nextI * tessellation + nextJ);
                    AddIndex(nextI * tessellation + j);
                }
            }

            InitializePrimitive(graphicsDevice);
        }
    }
}
