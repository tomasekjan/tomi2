using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xna= Microsoft.Xna.Framework;
using GraphEditor;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace GraphEditor.GraphDeclaration
{
    [Serializable]
    public sealed class Edge
    {
        public List<SubPoint> points;
        public List<Line2D> lines;
        xna.Color color;

        /// <summary>
        /// gets begin vertex of edge
        /// </summary>
        public SubPoint Begin
        {
            get
            {
                return points.First();
            }
        }

        /// <summary>
        /// gets end point of edge
        /// </summary>
        public SubPoint End
        {
            get
            {
                return points.Last();
            }
        }


        /// <summary>
        /// constructs new edge
        /// </summary>
        /// <param name="pointBegin">begin vertex</param>
        /// <param name="pointEnd">end vertex</param>
        /// <param name="color">initial color of new edge</param>        
        public Edge(Vertex pointBegin, Vertex pointEnd, xna.Color color)
        {
            points = new List<SubPoint>();
            points.Add(pointBegin);
            points.Add(pointEnd);
            this.color = color;
            LinesRebuild();
        }

        /// <summary>
        /// rebuild sub lines list
        /// </summary>
        internal void LinesRebuild()
        {
            if (points.Count < 2)
            {
                throw new Exception("point missing");
            }
            lines = new List<Line2D>();
            for (int i = 0; i < (points.Count - 1); i++)
            {
                lines.Add(new Line2D(points[i], points[i + 1], color, 0, 0));
            }
        }

        /// <summary>
        /// add new sub point and rebuild lines
        /// </summary>
        /// <param name="index">index to insert at</param>
        /// <param name="point">new point</param>
        internal void addPoint(int index, SubPoint point)
        {
            if (index > (points.Count - 2))
            {
                throw new ArgumentException("Index is to large");
            }
            points.Insert(index + 1, point);
            LinesRebuild();
        }

        /// <summary>
        /// add line sub point
        /// </summary>
        /// <param name="index">index of line where is point added</param>
        public void addPoint(int index)
        {
            if (index > (points.Count - 2))
            {
                throw new ArgumentException("Index is to large");
            }

            SubPoint point = new SubPoint(lines[index].getMiddle());
            
            points.Insert(index+1, point);
            LinesRebuild();
        }

        /// <summary>
        /// search for sub vertex on begin or end of line
        /// </summary>
        /// <param name="vertex">vertex to search for</param>
        /// <returns>returns true if line begins or ends with given vertex</returns>
        public bool HaveVertexBeginOrEnd(SubPoint vertex)
        {            
            if (vertex == points[0])
            {
                return true;
            }
            if (vertex == points.Last())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// returns true if given vertex is start vertex of this edge
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public bool HaveVertexBegin(SubPoint vertex)
        {
            if (vertex == points[0])
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// returns true if given vertex is end vertex of this edge
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public bool HaveVertexEnd(SubPoint vertex)
        {            
            if (vertex == points.Last())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// search for sub vertex on line
        /// </summary>
        /// <param name="vertex">vertex to search for</param>
        /// <returns>returns true if line contains given vertex</returns>
        public bool HaveVertex(SubPoint vertex)
        {
            foreach (SubPoint point in points)
            {
                if(vertex == point)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// search if line contains vertex nearby given pointF ( with toleration of 1% )
        /// </summary>
        /// <param name="pointF">point to search by</param>
        /// <returns>returns true if line contains some vertex nearby given pointF</returns>
        public bool HaveVertex(System.Drawing.PointF pointF)
        {
            foreach (SubPoint point in points)
            {
                if (point.Pozition.Compare(pointF))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// delete given vertex
        /// </summary>
        /// <param name="vertex2Delete"></param>
        public void delete(SubPoint vertex2Delete)
        {   
            points.Remove(vertex2Delete);
            LinesRebuild();
        }

        /// <summary>
        /// get first vertex which is nearby given pointF
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public SubPoint getVertex(System.Drawing.PointF pointF)
        {
            foreach (SubPoint point in points)
            {
                if (point.Pozition.Compare(pointF))
                {
                    return point;
                }
            }
            return null;
        }
        
        /// <summary>
        /// returns index of given line at edge
        /// </summary>
        /// <param name="line">line we are looking for</param>
        /// <returns></returns>
        public int Containts(Line2D line)
        {
            for(int i = 0;i <lines.Count;i++)
            {
                if (lines[i] == line)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
