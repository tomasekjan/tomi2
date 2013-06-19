using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using GraphEditor.GraphDeclaration;

namespace Plugin
{
    abstract class VertexDegree
    {
        public static bool Distinct(params int[] pole)
        {
            bool valid = true;
            for (int i = 0; i < pole.Length; i++)
            {
                for (int j = 0; j < pole.Length; j++)
                {
                    if (i != j)
                    {
                        if (pole[i] == pole[j])
                        {
                            valid = false;
                        }
                    }
                }
            }
            return valid;
        }
        public abstract void Remove(EmbedingMultiGraph embedding, int vertex);

        public PointF GetPosition(EdgeMultiGraph edgeSel, EmbedingMultiGraph embedding)
        {
            CircularListEdge selectedFace = null ;
            foreach (CircularListEdge face in embedding.GetFaces())
            {
                if (face.Contains(edgeSel))
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

        public abstract void Add(EmbedingMultiGraph embedding);
    }
}
