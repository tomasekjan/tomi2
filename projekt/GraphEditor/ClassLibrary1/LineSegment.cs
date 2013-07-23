using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;
using Microsoft.Xna.Framework;

namespace Plugin
{
    struct LineSegment
    {
        /// <summary>
        /// fist vertex
        /// </summary>
        public Vector2 A
        {
            get 
            {
                return a;            
            }
            set 
            { 
                a = value; 
            }
        }

        Vector2 a;
        /// <summary>
        /// second vertex
        /// </summary>
        public Vector2 B
        {
            get 
            { 
                return b; 
            }
            set 
            { 
                b = value; 
            }
        }
        Vector2 b;
        /// <summary>
        /// creates new line
        /// </summary>
        /// <param name="a">begin vertex</param>
        /// <param name="b">end vertex</param>
        public LineSegment(Vector2 a, Vector2 b)
        {
            this.a = a;
            this.b = b;
        }

        /// <summary>
        /// checks if current line is crossing with lineSegment
        /// </summary>
        /// <param name="lineSegment"></param>
        /// <returns></returns>
        public bool IsCrossing(LineSegment lineSegment)
        {
            Vector2 i;
            bool result = IsCrossing(lineSegment, out i);
            if (i == A || i == B)
            {
                return false;
            }
            return result;
        }

        

        /// <summary>
        /// checks if current line is crossing with lineSegment and outs intersection if so
        /// </summary>
        /// <param name="lineSegment"></param>
        /// <param name="intersection"></param>
        /// <returns></returns>
        public bool IsCrossing(LineSegment lineSegment, out Vector2 intersection)
        {
            intersection = Vector2.Zero;
            
            bool crossing;
            const float epsilon = 0.00001f;

            float ua = (lineSegment.B.X - lineSegment.A.X) * (A.Y - lineSegment.A.Y) - (lineSegment.B.Y - lineSegment.A.Y) * (A.X - lineSegment.A.X);
            float ub = (B.X - A.X) * (A.Y - lineSegment.A.Y) - (B.Y - A.Y) * (A.X - lineSegment.A.X);
            float denominator = (lineSegment.B.Y - lineSegment.A.Y) * (B.X - A.X) - (lineSegment.B.X - lineSegment.A.X) * (B.Y - A.Y);

            crossing = false;

            if (Math.Abs(denominator) <= epsilon)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    crossing = true;
                    intersection = (A + B) / 2;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    crossing = true;
                    intersection.X = A.X + ua * (B.X - A.X);
                    intersection.Y = A.Y + ua * (B.Y - A.Y);
                }
            }

            return crossing;
        }
    }
}
