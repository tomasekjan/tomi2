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
    abstract class VertexDegree
    {
        /// <summary>
        /// checks if all variables are distinct
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        protected static bool Distinct(params int[] array)
        {
            bool valid = true;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    if (i != j)
                    {
                        if (array[i] == array[j])
                        {
                            valid = false;
                        }
                    }
                }
            }
            return valid;
        }

        protected Vector2 PointF2Vector(PointF point)
        {
            return new Vector2(point.X, point.Y);
        }

        /// <summary>
        /// removing vertex with given index from embedding
        /// </summary>
        /// <param name="embedding">embedding to remove vertex from</param>
        /// <param name="vertex">index of vertex to remove</param>
        public abstract void Remove(EmbedingMultiGraph embedding, int vertex);

        /// <summary>
        /// get position of middle of triangle drawn on torus
        /// </summary>
        /// <param name="selectingEdge"></param>
        /// <param name="embedding"></param>
        /// <returns></returns>
        protected PointF GetPosition(EdgeMultiGraph selectingEdge, EmbedingMultiGraph embedding)
        {
            CircularListEdge selectedFace = null ;
            foreach (CircularListEdge face in embedding.GetFaces())
            {
                if (face.Contains(selectingEdge))
                {
                    selectedFace = face;
                    break;
                }
            }
            PointF position = new PointF(0, 0);
            foreach (EdgeMultiGraph edge in selectedFace)
            {
                int u = edge.U;
                position.X += embedding.pozition[u].X + edge.WrapHorizontal;
                position.Y += embedding.pozition[u].Y + edge.WrapVertical;
            }
            position.X /= selectedFace.Count;
            position.Y /= selectedFace.Count;
            return position;
        }

        /// <summary>
        /// adding vertex back to embedding
        /// </summary>
        /// <param name="embedding">embedding to add vertex to</param>
        public abstract void Add(EmbedingMultiGraph embedding);
    }
}
