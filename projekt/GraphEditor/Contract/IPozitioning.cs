using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.GraphDeclaration;

namespace AddInView
{
    public abstract class IPozitioning :MarshalByRefObject
    {
        /// <summary>
        /// Function witch gets some graph definition and return new graph definition with another vertex positions
        /// </summary>
        /// <param name="sourceDefinition">source graph definition</param>
        /// <returns>new graph definition</returns>
        public abstract GraphDefinition Pozitioning(GraphDefinition sourceDefinition);
    }
}
