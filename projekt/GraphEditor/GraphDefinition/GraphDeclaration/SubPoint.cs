﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using xna= Microsoft.Xna.Framework;
using System.Collections;

namespace GraphEditor.GraphDeclaration
{
    [Serializable]
    public class SubPoint
    {
        /// <summary>
        /// relative position 0 to 1 
        /// </summary>
        PointF pozition;
        /// <summary>
        /// dot size
        /// </summary>
        internal float size;
        /// <summary>
        /// checking cache integrity
        /// </summary>
        public bool isValid;
        /// <summary>
        /// point collor
        /// </summary>
        internal xna.Color color;

        public PointF Pozition
        {
            get
            {
                return pozition;
            }
            set
            {
                
                isValid = false;
                if (value.X > 1)
                {
                    value.X = 1;
                }
                if (value.Y > 1)
                {
                    value.Y = 1;
                }
                if (value.X < 0)
                {
                    value.X = 0;
                }
                if (value.Y < 0)
                {
                    value.Y = 0;
                }
                pozition = value;
            }
        }

        public float Size
        {
            get
            {
                return size;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pozition">PointF position of Vertex</param>
        public SubPoint(PointF pozition)
        {
            this.pozition = pozition;
            size = 4;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>returns string [x - y] where x is x position an y is y position</returns>
        public override string ToString()
        {
            return string.Format("[{0}-{1}]", pozition.X, pozition.Y);
        }

        public override int GetHashCode()
        {
            return (int)(pozition.X * int.MaxValue) + (int)(pozition.Y * int.MaxValue);
        }
        
        //public override bool Equals(object obj)
        //{
        //    SubPoint point2 = obj as SubPoint;
        //    return pozition.X.Equals(point2.pozition.X) && pozition.Y.Equals(point2.pozition.Y);
        //}

        public void ChangeColor(Color color)
        {
            this.color = new xna.Color(color.R, color.G, color.B, color.A);
        }
    }
}
