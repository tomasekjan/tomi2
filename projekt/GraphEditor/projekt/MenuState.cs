using System;
using System.Windows;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using GraphEditor.GraphDeclaration;
using AddInView;
using System.Globalization;
using Plugin;

namespace GraphEditor
{
    
    public enum MenuState
    {
        /// <summary>
        /// mouse on canvas is in normal mode (left to move vertex and middle to add vertex or edge)
        /// </summary>
        NONE,
        /// <summary>
        /// adding vertex tool is selected
        /// </summary>
        ADD_VERTEX,
        /// <summary>
        /// adding edge tool is selected
        /// </summary>
        ADD_EDGE
    }
}
