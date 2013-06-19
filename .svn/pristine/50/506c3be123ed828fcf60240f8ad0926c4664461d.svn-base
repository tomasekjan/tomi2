using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphEditor.Exceptions
{
    public class UndoRedoException : Exception
    {

        public UndoRedoException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the UndoRedoException class.
        /// </summary>
        public UndoRedoException(String message, Exception inerException)
            :base(message,inerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the UndoRedoException class.
        /// </summary>
        public UndoRedoException()
            : base("Unknown error while undo/redo Operation")
        {

        }
    }
}
