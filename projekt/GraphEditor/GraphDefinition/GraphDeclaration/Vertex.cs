using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using xna=Microsoft.Xna.Framework;

using System.Runtime.Serialization;

namespace GraphEditor.GraphDeclaration
{
    [Serializable]
    public sealed class Vertex : SubPoint
    {      
        public xna.Color Color
        {
            get
            {
                return color;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pozition">Pointf position of Vertex</param>
        /// <param name="color">Xna Color of Vertex</param>
        /// <param name="radius">Vertex Radius</param>
        public Vertex(PointF pozition, xna.Color color, float radius)
            :base(pozition)
        {            
            this.color = color;
            this.size = radius;
        }
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="pozition">Pointf position of Vertex</param>
        /// <param name="color">Xna Color of Vertex</param>
        public Vertex(PointF pozition, xna.Color color)
            :this(pozition,color,0.03f)
        {
            
        }

        // on Deserialized mark invalid point for Cache reasons
        [OnDeserialized]
        private void SetValuesOnDeserializing(StreamingContext context)
        {
            isValid = false;
        }

    }
}
