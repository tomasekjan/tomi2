using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphEditor.Exceptions
{
    class Draw3DException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the Draw3DException class.
        /// </summary>
        public Draw3DException()
            : base("Error by 3D rendering - Check your 3D card driver")
        {

        }

        /// <summary>
        /// Initializes a new instance of the Draw3DException class.
        /// </summary>
        public Draw3DException(Exception inerException)
            :base("Error by 3D rendering - Check your 3D card driver", inerException)
        {
            
        }
    }
}
