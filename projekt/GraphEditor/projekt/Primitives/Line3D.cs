using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using GraphEditor.GraphDeclaration;
using System.Windows.Forms;

namespace GraphEditor.Primitives3D
{
    class LinePrimtive : GeometricPrimitive
    {
        /// <summary>
        /// Construct LilePrimitive used to show 3D line
        /// </summary>
        /// <param name="graphicsDevice">Graphics device to show on</param>
        /// <param name="startPoint">line begin point</param>
        /// <param name="targePoint">line end point</param>
        /// <param name="wrapedHorizontal"></param>
        /// <param name="wrapedVertical"></param>
        /// <param name="surfaceType">Chose torus or sphere</param>
        public LinePrimtive(GraphicsDevice graphicsDevice, PointF startPoint, PointF targePoint, int wrapedHorizontal, int wrapedVertical, SurfaceTypeEnum surfaceType)
            : this(graphicsDevice, startPoint, targePoint, 0.01f, 0.01f, 200, 0.2f, wrapedHorizontal, wrapedVertical, surfaceType)
        {

        }
        
        /// <summary>
        /// Construct LilePrimitive used to show 3D line
        /// </summary>
        /// <param name="graphicsDevice">Graphics device to show on</param>
        /// <param name="startPoint">line begin point</param>
        /// <param name="targePoint">line end point</param>
        /// <param name="width">line cuboid width</param>
        /// <param name="depth">line cuboid depth</param>
        /// <param name="tessellation">line cuboid tessellation</param>
        /// <param name="radius"></param>
        /// <param name="wrapedHorizontal"></param>
        /// <param name="wrapedVertical"></param>
        /// <param name="surfaceType"></param>
        private LinePrimtive(GraphicsDevice graphicsDevice, PointF startPoint, PointF targePoint, float width, float depth, int tessellation, float radius,  int wrapedHorizontal, int wrapedVertical, SurfaceTypeEnum surfaceType)
        {
            switch (surfaceType)
            {
                case SurfaceTypeEnum.Sphere:
                    LineConstructionSphere(graphicsDevice, startPoint, targePoint, width, depth, tessellation);
                    break;
                case SurfaceTypeEnum.Torus:
                    LineConstructionTorus(graphicsDevice, startPoint, targePoint, width, depth, tessellation, radius, wrapedHorizontal, wrapedVertical);                    
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Create line on Sphere
        /// </summary>
        /// <param name="graphicsDevice">device to create on</param>
        /// <param name="startPoint">position of begin point</param>
        /// <param name="targePoint">position of end point</param>
        /// <param name="width">line width</param>
        /// <param name="depth">line depth</param>
        /// <param name="tessellation">set tessellation of line detail</param>
        /// <param name="radius">sphere radius</param>
        private void LineConstructionSphere(GraphicsDevice graphicsDevice, PointF startPoint, PointF targePoint, float width, float depth, int tessellation)
        {
            
            
            float sizeX = (startPoint.X - targePoint.X);
            float sizeY = (startPoint.Y - targePoint.Y);
            float lenght = (float)Math.Sqrt(sizeX * sizeX + sizeY * sizeY) * 0.1f;            

            for (int i = 0; i < tessellation; i++)
            {
                PointF vertex = new PointF(startPoint.X - i * (sizeX / tessellation), startPoint.Y - i * (sizeY / tessellation));
                float x = vertex.X * 0.7f;
                float y = vertex.Y * 0.7f;
                Vector3 position = new Vector3(0, 0, 1);
                position.X = x;
                position.Y = y;
                position.Z = (float)Math.Sqrt((1 - x * x - y * y));
                position *= 0.7f;
                Matrix tranform = Matrix.CreateTranslation(position);
                
                tranform = Matrix.CreateTranslation(position);

                float pozitionY = (lenght / tessellation) * i;
                float pozitionXLeft = -width;
                float pozitionXRight = width;
                float pozitionZFront = depth;
                float pozitionZRear = -depth;

                Vector3 pozitionLeftFront = new Vector3(pozitionXLeft, pozitionY, pozitionZFront);
                Vector3 pozitionlLeftRear = new Vector3(pozitionXLeft, pozitionY, pozitionZRear);
                Vector3 pozitionlRightFront = new Vector3(pozitionXRight, pozitionY, pozitionZFront);
                Vector3 pozitionRightRear = new Vector3(pozitionXRight, pozitionY, pozitionZRear);

                AddVertexs(tranform, pozitionLeftFront, pozitionlLeftRear, pozitionlRightFront, pozitionRightRear);

                if (i != tessellation - 1)
                {
                    CreateSides(i);
                }
            }
            InitializePrimitive(graphicsDevice);
        }

        /// <summary>
        /// Create line on Torus
        /// </summary>
        /// <param name="graphicsDevice">device to create on</param>
        /// <param name="startPoint">position of begin point</param>
        /// <param name="targetPoint">position of end point</param>
        /// <param name="width">line width</param>
        /// <param name="depth">line depth</param>
        /// <param name="tessellation">set tessellation of line detail</param>
        /// <param name="radius">Torus radius</param>
        /// <param name="wrapedHorizontal">strait or round line in X axe</param>
        /// <param name="wrapedVertical">straint or round line in Y axe</param>
        private void LineConstructionTorus(GraphicsDevice graphicsDevice, PointF startPoint, PointF targetPoint, float width, float depth, int tessellation, float radius, int wrapedHorizontal, int wrapedVertical)
        {
            targetPoint.X += wrapedHorizontal;
            targetPoint.Y += wrapedVertical;

            float sizeX = (float)Math.Abs(startPoint.X - targetPoint.X);
            float sizeY = (float)Math.Abs(startPoint.Y - targetPoint.Y);
            float angleX = MathHelper.TwoPi * sizeX;            
            float angleY = MathHelper.TwoPi * sizeY;
            float anglePartX = angleX / tessellation;
            float anglePartY = angleY / tessellation;
            float beginAngleX = MathHelper.TwoPi * startPoint.X;
            float beginAngleY = MathHelper.TwoPi * startPoint.Y;
            float lenght = (float)Math.Sqrt(sizeX * sizeX + sizeY * sizeY) * 0.1f;
            //float lenght = sizeX * 0.1f;
            for (int i = 0; i < tessellation; i++)
            {
                float actualAngelX = beginAngleX - (i * anglePartX);
                float actualAngelY = beginAngleY - (i * anglePartY);

                if (startPoint.X < targetPoint.X)
                {
                    actualAngelX = beginAngleX + (i * anglePartX);
                }
                if (startPoint.Y < targetPoint.Y)
                {
                    actualAngelY = beginAngleY + (i * anglePartY);
                }

                const float diameter = 1;
                Matrix tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(actualAngelX) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(actualAngelY); 

                float pozitionY = (lenght / tessellation) * i;
                float pozitionXLeft = -width;
                float pozitionXRight = width;
                float pozitionZFront = depth;
                float pozitionZRear = -depth;

                Vector3 pozitionLeftFront = new Vector3(pozitionXLeft, pozitionY, pozitionZFront);
                Vector3 pozitionlLeftRear = new Vector3(pozitionXLeft, pozitionY, pozitionZRear);
                Vector3 pozitionlRightFront = new Vector3(pozitionXRight, pozitionY, pozitionZFront);
                Vector3 pozitionRightRear = new Vector3(pozitionXRight, pozitionY, pozitionZRear);

                AddVertexs(tranform, pozitionLeftFront, pozitionlLeftRear, pozitionlRightFront, pozitionRightRear);

                if (i != tessellation - 1)
                {
                    CreateSides(i);
                }
            }
            InitializePrimitive(graphicsDevice);
        }

        /// <summary>
        /// Create line on Torus
        /// </summary>
        /// <param name="graphicsDevice">device to create on</param>
        /// <param name="startPoint">position of begin point</param>
        /// <param name="targePoint">position of end point</param>
        /// <param name="width">line width</param>
        /// <param name="depth">line depth</param>
        /// <param name="tessellation">set tessellation of line detail</param>
        /// <param name="radius">Torus radius</param>
        /// <param name="wrapedHorizontal">strait or round line in X axe</param>
        /// <param name="wrapedVertical">straint or round line in Y axe</param>
        private void LineConstructionTorusOld(GraphicsDevice graphicsDevice, PointF startPoint, PointF targePoint, float width, float depth, int tessellation, float radius, int wrapedHorizontal, int wrapedVertical)
        {
            float circuit = (float)(MathHelper.TwoPi * radius);
            float sizeX = (float)Math.Abs(startPoint.X - targePoint.X);
            if (wrapedHorizontal != 0) sizeX = 1 - sizeX;
            float sizeY = (float)Math.Abs(startPoint.Y - targePoint.Y);
            if (wrapedVertical !=0) sizeY = 1 - sizeY;
            double angleX = MathHelper.TwoPi * sizeX;
            float lenght = sizeX * 0.1f;
            if (wrapedHorizontal != 0) lenght = (1 - sizeX) * 0.045f;
            double angleY = MathHelper.TwoPi * sizeY;
            double anglePartX = angleX / tessellation;
            double anglePartY = angleY / tessellation;
            float beginAngleX = MathHelper.TwoPi * startPoint.X;
            float beginAngleY = MathHelper.TwoPi * startPoint.Y;
            for (int i = 0; i < tessellation; i++)
            {
                
                float actualAngelX = i * (float)anglePartX;
                float actualAngelY = i * (float)anglePartY;
                float diameter = 1;
                if (wrapedHorizontal != 0) actualAngelX *= -1;
                if (wrapedVertical != 0) actualAngelY *= -1;
                Matrix tranform;
                if (startPoint.X < targePoint.X)
                {
                    //right
                    if (startPoint.Y < targePoint.Y)
                    {
                        //down
                        if (wrapedHorizontal != 0)
                        {
                            tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(beginAngleX + actualAngelX * 1f) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(beginAngleY + actualAngelY);
                        }
                        else
                        {
                            tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(beginAngleX + actualAngelX * 0.9f) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(beginAngleY + actualAngelY);
                        }
                    }
                    else
                    {
                        //up
                        if (wrapedHorizontal != 0)
                        {
                            tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(beginAngleX + actualAngelX * 1f) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(beginAngleY - actualAngelY);
                        }
                        else
                        {
                            tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(beginAngleX + actualAngelX * 0.9f) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(beginAngleY - actualAngelY);
                        }
                    }
                }
                else
                {
                    //left
                    if (startPoint.Y < targePoint.Y)
                    {
                        //down
                        if (wrapedHorizontal !=0)
                        {
                            tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(beginAngleX - actualAngelX * 1f) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(beginAngleY + actualAngelY);
                        }
                        else
                        {
                            tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(beginAngleX - actualAngelX * 1.1f) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(beginAngleY + actualAngelY);
                        }
                    }
                    else
                    {
                        //up
                        tranform = Matrix.CreateTranslation(radius - width / 2, 0, 0) * Matrix.CreateRotationZ(beginAngleX - actualAngelX * 1.1f) * Matrix.CreateTranslation(diameter / 2, 0, 0) * Matrix.CreateRotationY(beginAngleY - actualAngelY);
                    }
                }

                float pozitionY = (lenght / tessellation) * i;
                float pozitionXLeft = -width;
                float pozitionXRight = width;
                float pozitionZFront = depth;
                float pozitionZRear = -depth;

                Vector3 pozitionLeftFront = new Vector3(pozitionXLeft, pozitionY, pozitionZFront);
                Vector3 pozitionlLeftRear = new Vector3(pozitionXLeft, pozitionY, pozitionZRear);
                Vector3 pozitionlRightFront = new Vector3(pozitionXRight, pozitionY, pozitionZFront);
                Vector3 pozitionRightRear = new Vector3(pozitionXRight, pozitionY, pozitionZRear);

                AddVertexs(tranform, pozitionLeftFront, pozitionlLeftRear, pozitionlRightFront, pozitionRightRear);

                if (i != tessellation - 1)
                {
                    CreateSides(i);
                }
            }
            InitializePrimitive(graphicsDevice);
        }

        /// <summary>
        /// creates sides of box by triangles
        /// </summary>
        /// <param name="i">start index</param>
        private void CreateSides(int i)
        {
            int j = i * 4;

            //bottom side
            AddIndexes(j + 0, j + 1, j + 2);
            AddIndexes(j + 0, j + 2, j + 1);
            AddIndexes(j + 1, j + 2, j + 3);
            AddIndexes(j + 1, j + 3, j + 2);

            //upper side
            AddIndexes(j + 4, j + 5, j + 6);
            AddIndexes(j + 4, j + 6, j + 5);
            AddIndexes(j + 5, j + 6, j + 7);
            AddIndexes(j + 5, j + 7, j + 6);

            //left side
            AddIndexes(j + 0, j + 1, j + 4);
            AddIndexes(j + 0, j + 4, j + 1);
            AddIndexes(j + 1, j + 5, j + 4);
            AddIndexes(j + 1, j + 4, j + 5);

            //right side
            AddIndexes(j + 2, j + 3, j + 6);
            AddIndexes(j + 2, j + 6, j + 3);
            AddIndexes(j + 3, j + 6, j + 7);
            AddIndexes(j + 3, j + 7, j + 6);

            //front side
            AddIndexes(j + 0, j + 2, j + 4);
            AddIndexes(j + 0, j + 4, j + 2);
            AddIndexes(j + 2, j + 4, j + 6);
            AddIndexes(j + 2, j + 6, j + 4);

            //back side
            AddIndexes(j + 1, j + 3, j + 5);
            AddIndexes(j + 1, j + 5, j + 3);
            AddIndexes(j + 3, j + 5, j + 7);
            AddIndexes(j + 3, j + 7, j + 5);
        }
    }
}

