using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphEditor.Exceptions
{
    class SerializationException : Exception 
    {
        /// <summary>
        /// Initializes a new instance of the SerializationException class.
        /// </summary>
        public SerializationException(String message)
            :base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the SerializationException class.
        /// </summary>
        public SerializationException(string message, Exception inerException)
            :base(message, inerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the SerializationException class.
        /// </summary>
        public SerializationException()
            :base("Unknown error while serializing")
        {
            
        }
    }
}