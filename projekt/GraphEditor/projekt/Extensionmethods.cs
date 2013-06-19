using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GraphEditor
{
    /// <summary>
    /// comparing two points by position with 1% toleration
    /// </summary>
    /// <param name="point1">first point</param>
    /// <param name="point2">point to be compare with</param>
    /// <returns>returns true if points have position difference smaller then 1% in both axes </returns>
    static class Extensionmethods
    {
        public static bool Compare(this PointF point1, PointF point2)
        {
            if ((Math.Abs(point1.X - point2.X) < 0.01) & (Math.Abs(point1.Y - point2.Y) < 0.01))
            {
                return true;
            }
            return false;
        }
    }
}
