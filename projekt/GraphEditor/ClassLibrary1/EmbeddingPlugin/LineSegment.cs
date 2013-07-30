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
        public Vector2 U
        {
            get 
            {
                return u;            
            }
            set 
            { 
                u = value; 
            }
        }

        Vector2 u;
        /// <summary>
        /// second vertex
        /// </summary>
        public Vector2 V
        {
            get 
            { 
                return v; 
            }
            set 
            { 
                v = value; 
            }
        }
        Vector2 v;
        /// <summary>
        /// creates new line
        /// </summary>
        /// <param name="a">begin vertex</param>
        /// <param name="b">end vertex</param>
        public LineSegment(Vector2 a, Vector2 b)
        {
            this.u = a;
            this.v = b;
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
            if (i == U || i == V)
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

            float ua = (lineSegment.V.X - lineSegment.U.X) * (U.Y - lineSegment.U.Y) - (lineSegment.V.Y - lineSegment.U.Y) * (U.X - lineSegment.U.X);
            float ub = (V.X - U.X) * (U.Y - lineSegment.U.Y) - (V.Y - U.Y) * (U.X - lineSegment.U.X);
            float denominator = (lineSegment.V.Y - lineSegment.U.Y) * (V.X - U.X) - (lineSegment.V.X - lineSegment.U.X) * (V.Y - U.Y);

            crossing = false;

            if (Math.Abs(denominator) <= epsilon)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    crossing = true;
                    intersection = (U + V) / 2;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    crossing = true;
                    intersection.X = U.X + ua * (V.X - U.X);
                    intersection.Y = U.Y + ua * (V.Y - U.Y);
                }
            }

            return crossing;
        }
    }
}
