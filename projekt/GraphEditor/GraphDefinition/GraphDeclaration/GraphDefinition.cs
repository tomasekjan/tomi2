using System;
using System.Collections.Generic;
using System.Linq;
using Xna= Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace GraphEditor.GraphDeclaration
{
    /// <summary>
    /// Main class of Graph content 
    /// contains vertices and edges description
    /// </summary>
    [Serializable]
    public sealed class GraphDefinition
    {
        /// <summary>
        /// List of vertices in graph
        /// </summary>
        public List<Vertex> vertices;
        /// <summary>
        /// List of edges in graph
        /// </summary>
        public List<Edge> edges;
        /// <summary>
        /// color of surface
        /// </summary>
        public Xna.Color surfaceColor;
        /// <summary>
        /// surface type (torus or sphere)
        /// </summary>
        public SurfaceTypeEnum suraceType;

        public GraphDefinition()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }

    }
}
