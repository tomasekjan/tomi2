using System;
using System.Collections.Generic;
using GraphEditor.GraphDeclaration;
using System.Drawing;
using AddInView;

namespace Plugin
{
    [Serializable]
    public sealed class RandomPozition : IPozitioning 
    {
        public override  GraphDefinition Pozitioning(GraphDefinition sourceDefinition)
        {
            Random r = new Random();            
            int cout = sourceDefinition.vertices.Count;
            for (int i = 0; i < cout; i++)
            {
                sourceDefinition.vertices[i].Pozition = new PointF((float)r.NextDouble(), (float)r.NextDouble());
            }
            return sourceDefinition;
        }
    }
}
