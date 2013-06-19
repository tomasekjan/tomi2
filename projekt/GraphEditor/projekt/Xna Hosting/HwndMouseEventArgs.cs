using System;
using System.Windows.Input;
using System.Windows;

namespace GraphEditor
{
    public class HwndMouseEventArgs : EventArgs
    {
        
        public MouseButtonState LeftButton { get; private set; }

        public MouseButtonState RightButton { get; private set; }

        public MouseButtonState MiddleButton { get; private set; }

        public MouseButtonState X1Button { get; private set; }

        public MouseButtonState X2Button { get; private set; }

        public MouseButton? DoubleClickButton { get; private set; }

        public int WheelDelta { get; private set; }

        public int HorizontalWheelDelta { get; private set; }

        public Point Position { get; private set; }

        public Point PreviousPosition { get; private set; }

        public HwndMouseEventArgs(MouseState state)
        {
            LeftButton = state.LeftButton;
            RightButton = state.RightButton;
            MiddleButton = state.MiddleButton;
            X1Button = state.X1Button;
            X2Button = state.X2Button;
            Position = state.Position;
            PreviousPosition = state.PreviousPosition;
        }
        public HwndMouseEventArgs(MouseState state, int mouseWheelDelta, int mouseHWheelDelta)
            : this(state)
        {
            WheelDelta = mouseWheelDelta;
            HorizontalWheelDelta = mouseHWheelDelta;
        }
        
        public HwndMouseEventArgs(MouseState state, MouseButton doubleClickButton)
            : this(state)
        {
            DoubleClickButton = doubleClickButton;
        }
    }
}
