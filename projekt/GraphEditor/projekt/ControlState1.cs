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
    public enum ControlState
    {
        NONE,
        ADD_VERTEX,
        ADD_EDGE,
    }
}
