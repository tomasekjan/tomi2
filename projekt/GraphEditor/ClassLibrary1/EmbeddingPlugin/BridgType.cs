using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddInView;
using GraphEditor.GraphDeclaration;
using System.Diagnostics;
using System.Drawing;
using System.Collections;

namespace Plugin
{
    /// <summary>
    /// type of bridge according to "A Faster Algorithm for Torus Embedding" [12]
    /// </summary>
    public enum BridgType
    {
        /// <summary>
        /// a connected component C of G \ H with edges (u,v) where u \in C and v \in H
        /// </summary>
        Type1,
        /// <summary>
        /// edge (u,v) of G \ H where v \in H and u \in H but (u,v) \notIn H
        /// </summary>
        Type2
    }
}
