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
    public class EmbeddingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the EmbeddingException class.
        /// </summary>
        public EmbeddingException(string error)
            : base(error)
        {

        }
    }
}
