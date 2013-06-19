using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GraphEditor.Primitives3D
{
    /// <summary>
    /// Geometric primitive class for drawing spheres.
    /// </summary>
    [Serializable]
    public class SpherePrimitive : GeometricPrimitive
    {
        
        public SpherePrimitive(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1, 16)
        {
        }

        public SpherePrimitive(GraphicsDevice graphicsDevice, float diameter, int tessellation)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            int verticalSegments = tessellation;
            int horizontalSegments = tessellation * 2;

            float radius = diameter / 2;

            // Bottom of the sphere.
            AddVertex(Vector3.Down * radius, Vector3.Down);
            
            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i < verticalSegments - 1; i++)
            {
                float latitude = ((i + 1) * MathHelper.Pi / verticalSegments) - MathHelper.PiOver2;

                float dy = (float)Math.Sin(latitude);
                float dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j < horizontalSegments; j++)
                {
                    float longitude = j * MathHelper.TwoPi / horizontalSegments;

                    float dx = (float)Math.Cos(longitude) * dxz;
                    float dz = (float)Math.Sin(longitude) * dxz;

                    Vector3 normal = new Vector3(dx, dy, dz);

                    AddVertex(normal * radius, normal);
                }
            }

            // Top of the sphere.
            AddVertex(Vector3.Up * radius, Vector3.Up);

            // top connections
            for (int i = 0; i < horizontalSegments; i++)
            {
                AddIndex(0);
                AddIndex(1 + (i + 1) % horizontalSegments);
                AddIndex(1 + i);
            }

            // inside connections
            for (int i = 0; i < verticalSegments - 2; i++)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % horizontalSegments;

                    AddIndex(1 + i * horizontalSegments + j);
                    AddIndex(1 + i * horizontalSegments + nextJ);
                    AddIndex(1 + nextI * horizontalSegments + j);

                    AddIndex(1 + i * horizontalSegments + nextJ);
                    AddIndex(1 + nextI * horizontalSegments + nextJ);
                    AddIndex(1 + nextI * horizontalSegments + j);
                }
            }

            //bottom connections
            for (int k = 0; k < horizontalSegments; k++)
            {
                AddIndex(CurrentVertex - 1);
                AddIndex(CurrentVertex - 2 - (k + 1) % horizontalSegments);
                AddIndex(CurrentVertex - 2 - k);
            }

            InitializePrimitive(graphicsDevice);
        }
    }
}
