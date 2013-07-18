using System;
using System.Collections.Generic;
using System.Linq;
using xna= Microsoft.Xna.Framework;
using System.Collections;

namespace GraphEditor.GraphDeclaration
{
    [Serializable]
    public sealed class Line2D
    {
        SubPoint pointBegin;
        SubPoint pointEnd;
        xna.Color color;

        /// <summary>
        /// 0 if no wrap
        /// +1 if wrap right
        /// -1 if wrap left
        /// </summary>
        int wrappedHorizontal;

        /// <summary>
        /// 0 if no wrap
        /// +1 if wrap down
        /// -1 if wrap up
        /// </summary>
        int wrapedVertical;
                
        #region Geters        
        public xna.Color Color
        {
            get
            {
                return color;
            }
        }
        public SubPoint PointBegin
        {
            get
            {
                return pointBegin;
            }
        }

        public SubPoint PointEnd
        {
            get
            {
                return pointEnd;
            }
        }        

        /// <summary>
        /// gets or sets wrapped horizontal value with check
        /// </summary>
        public int WrappedHorizontal
        {
            get
            {
                return wrappedHorizontal;
            }
            set
            {
                if (value == 0)
                {
                    wrappedHorizontal = 0;
                }
                if (value < 0)
                {
                    wrappedHorizontal = -1;
                }
                if (value > 0)
                {
                    wrappedHorizontal = 1;
                }
            }
        }

        /// <summary>
        /// gets or sets wrapped vertical value with check
        /// </summary>
        public int WrapedVertical
        {
            get
            {
                return wrapedVertical;
            }
            set
            {
                if (value == 0)
                {
                    wrapedVertical = 0;
                }
                if (value < 0)
                {
                    wrapedVertical = -1;
                }
                if (value > 0)
                {
                    wrapedVertical = 1;
                }
            }
        }
        #endregion

        /// <summary>
        /// constructs new line
        /// </summary>
        /// <param name="pointBegin">begin point</param>
        /// <param name="pointEnd">end point</param>
        /// <param name="color">line color</param>
        public Line2D(SubPoint pointBegin, SubPoint pointEnd, xna.Color color, int wrapedHorizontal, int wrapedVertical)
        {
            this.pointBegin = pointBegin;
            this.pointEnd = pointEnd;
            this.color = color;
            this.wrappedHorizontal = wrapedHorizontal;
            this.wrapedVertical = wrapedVertical;
        }
        
        /// <summary>
        /// gets position of middle of the line
        /// </summary>
        /// <returns>point in the middle of the line</returns>
        internal System.Drawing.PointF getMiddle()
        {
            return new System.Drawing.PointF((PointBegin.Pozition.X + PointEnd.Pozition.X) / 2, (PointBegin.Pozition.Y + PointEnd.Pozition.Y) / 2);
        }

        public override bool Equals(object obj)
        {
            Line2D line = obj as Line2D;
            return (line.PointBegin.Pozition.X == PointBegin.Pozition.X) && (line.PointBegin.Pozition.Y == PointBegin.Pozition.Y) && (line.PointEnd.Pozition.X == PointEnd.Pozition.X) && (line.PointEnd.Pozition.Y == PointEnd.Pozition.Y);
        }

        public override int GetHashCode()
        {
            return (int) (pointBegin.Pozition.X + pointBegin.Pozition.Y * 127 + pointEnd.Pozition.X * 983 + pointEnd.Pozition.Y * 4937);
        }

        /// <summary>
        /// Change line color
        /// </summary>
        /// <param name="color">new color</param>
        public void ChangeColor(System.Drawing.Color color)
        {
            this.color = new xna.Color(color.R, color.G, color.B, color.A);
        }
    }
}
    