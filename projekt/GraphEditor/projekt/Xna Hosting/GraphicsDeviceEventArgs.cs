using System;
using Microsoft.Xna.Framework.Graphics;

namespace GraphEditor
{
    public class GraphicsDeviceEventArgs : EventArgs
    {
        public GraphicsDevice GraphicsDevice { get; private set; }

        public GraphicsDeviceEventArgs(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }
    }
}
