using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Xna.Framework.Graphics;

namespace GraphEditor
{
    public class GraphicsDeviceControl : HwndHost
    {
        
        private const string windowClassName = "GraphicsDeviceControlHostWindowClass";
        private IntPtr handler;                
        private GraphicsDeviceService graphicsService;                
        private bool applicationHasFocus = false;                
        private bool mouseInWindow = false;
        private MouseState mouseState = new MouseState();
        private bool isMouseCaptured = false;
        private int capturedMouseX;
        private int capturedMouseY;
        private int capturedMouseClientX;
        private int capturedMouseClientY;

        /// <summary>
        /// Invoked when the control has initialized the GraphicsDevice.
        /// </summary>
        public event EventHandler<GraphicsDeviceEventArgs> LoadContent;

        /// <summary>
        /// Invoked when the control is ready to render XNA content
        /// </summary>
        public event EventHandler<GraphicsDeviceEventArgs> RenderXna;

        /// <summary>
        /// Invoked when the control receives a left mouse down message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndLButtonDown;

        /// <summary>
        /// Invoked when the control receives a left mouse up message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndLButtonUp;

        /// <summary>
        /// Invoked when the control receives a left mouse double click message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndLButtonDblClick;

        /// <summary>
        /// Invoked when the control receives a right mouse down message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndRButtonDown;

        /// <summary>
        /// Invoked when the control receives a right mouse up message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndRButtonUp;

        /// <summary>
        /// Invoked when the control receives a rigt mouse double click message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndRButtonDblClick;

        /// <summary>
        /// Invoked when the control receives a middle mouse down message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndMButtonDown;

        /// <summary>
        /// Invoked when the control receives a middle mouse up message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndMButtonUp;

        /// <summary>
        /// Invoked when the control receives a middle mouse double click message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndMButtonDblClick;

        /// <summary>
        /// Invoked when the control receives a mouse down message for the first extra button.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDown;

        /// <summary>
        /// Invoked when the control receives a mouse up message for the first extra button.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonUp;

        /// <summary>
        /// Invoked when the control receives a double click message for the first extra mouse button.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDblClick;

        /// <summary>
        /// Invoked when the control receives a mouse down message for the second extra button.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDown;

        /// <summary>
        /// Invoked when the control receives a mouse up message for the second extra button.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonUp;

        /// <summary>
        /// Invoked when the control receives a double click message for the first extra mouse button.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDblClick;

        /// <summary>
        /// Invoked when the control receives a mouse move message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndMouseMove;

        /// <summary>
        /// Invoked when the control first gets a mouse move message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndMouseEnter;

        /// <summary>
        /// Invoked when the control gets a mouse leave message.
        /// </summary>
        public event EventHandler<HwndMouseEventArgs> HwndMouseLeave;

        
        public GraphicsDeviceControl()
        {            
            Loaded += new RoutedEventHandler(XnaWindowHost_Loaded);         
            SizeChanged += new SizeChangedEventHandler(XnaWindowHost_SizeChanged);
            Application.Current.Activated += new EventHandler(Current_Activated);
            Application.Current.Deactivated += new EventHandler(Current_Deactivated);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        protected override void Dispose(bool disposing)
        {
            if (graphicsService != null)
            {
                graphicsService.Release(disposing);
                graphicsService = null;
            }
            CompositionTarget.Rendering -= CompositionTarget_Rendering;

            base.Dispose(disposing);
        }


        
        public new void CaptureMouse()
        {            
            if (isMouseCaptured)
                return;

            NativeMethods.ShowCursor(false);
            isMouseCaptured = true;
            NativeMethods.POINT p = new NativeMethods.POINT();
            NativeMethods.GetCursorPos(ref p);
            capturedMouseX = p.X;
            capturedMouseY = p.Y;

            NativeMethods.ScreenToClient(handler, ref p);
            capturedMouseClientX = p.X;
            capturedMouseClientY = p.Y;
        }
        public new void ReleaseMouseCapture()
        {
            
            if (!isMouseCaptured)
                return;

            NativeMethods.ShowCursor(true);
            isMouseCaptured = false;
        }

        #region Graphics Device Control Implementation

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            
            if (isMouseCaptured &&
                (int)mouseState.Position.X != capturedMouseX &&
                (int)mouseState.Position.Y != capturedMouseY)
            {
                NativeMethods.SetCursorPos(capturedMouseX, capturedMouseY);

                mouseState.Position = mouseState.PreviousPosition = new Point(capturedMouseClientX, capturedMouseClientY);
            }

            
            if (graphicsService == null)
                return;

            
            int width = (int)ActualWidth;
            int height = (int)ActualHeight;

            
            if (width < 1 || height < 1)
            {
                return;
            }

            
            Viewport viewport = new Viewport(0, 0, width, height);
            graphicsService.GraphicsDevice.Viewport = viewport;


            if (RenderXna != null)
            {
                RenderXna(this, new GraphicsDeviceEventArgs(graphicsService.GraphicsDevice));
            }

            
            graphicsService.GraphicsDevice.Present(viewport.Bounds, null, handler);
        }

        void XnaWindowHost_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (graphicsService == null)
            {
                graphicsService = GraphicsDeviceService.AddRef(handler, (int)ActualWidth, (int)ActualHeight);


                if (LoadContent != null)
                {
                    LoadContent(this, new GraphicsDeviceEventArgs(graphicsService.GraphicsDevice));
                }
            }
        }

        void XnaWindowHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (graphicsService != null)
            {
                graphicsService.ResetDevice((int)ActualWidth, (int)ActualHeight);
            }
        }

        void Current_Activated(object sender, EventArgs e)
        {
            applicationHasFocus = true;
        }

        void Current_Deactivated(object sender, EventArgs e)
        {
            applicationHasFocus = false;
            ResetMouseState();

            if (mouseInWindow)
            {
                mouseInWindow = false;
                if (HwndMouseLeave != null)
                    HwndMouseLeave(this, new HwndMouseEventArgs(mouseState));
            }

            ReleaseMouseCapture();
        }

        private void ResetMouseState()
        {            
            bool fireL = mouseState.LeftButton == MouseButtonState.Pressed;
            bool fireM = mouseState.MiddleButton == MouseButtonState.Pressed;
            bool fireR = mouseState.RightButton == MouseButtonState.Pressed;
            bool fireX1 = mouseState.X1Button == MouseButtonState.Pressed;
            bool fireX2 = mouseState.X2Button == MouseButtonState.Pressed;
                       
            mouseState.LeftButton = MouseButtonState.Released;
            mouseState.MiddleButton = MouseButtonState.Released;
            mouseState.RightButton = MouseButtonState.Released;
            mouseState.X1Button = MouseButtonState.Released;
            mouseState.X2Button = MouseButtonState.Released;
                     
            HwndMouseEventArgs args = new HwndMouseEventArgs(mouseState);
            if (fireL && HwndLButtonUp != null)
                HwndLButtonUp(this, args);
            if (fireM && HwndMButtonUp != null)
                HwndMButtonUp(this, args);
            if (fireR && HwndRButtonUp != null)
                HwndRButtonUp(this, args);
            if (fireX1 && HwndX1ButtonUp != null)
                HwndX1ButtonUp(this, args);
            if (fireX2 && HwndX2ButtonUp != null)
                HwndX2ButtonUp(this, args);            
            mouseInWindow = false;
        }

        #endregion

        #region HWND Management

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {            
            handler = CreateHostWindow(hwndParent.Handle);
            return new HandleRef(this, handler);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {            
            NativeMethods.DestroyWindow(hwnd.Handle);
            handler = IntPtr.Zero;
        }

        private IntPtr CreateHostWindow(IntPtr hWndParent)
        {
            
            RegisterWindowClass();
                        
            return NativeMethods.CreateWindowEx(0, windowClassName, "",
               NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE,
               0, 0, (int)Width, (int)Height, hWndParent, IntPtr.Zero, IntPtr.Zero, 0);
        }
        private void RegisterWindowClass()
        {
            NativeMethods.WNDCLASSEX wndClass = new NativeMethods.WNDCLASSEX();
            wndClass.cbSize = (uint)Marshal.SizeOf(wndClass);
            wndClass.hInstance = NativeMethods.GetModuleHandle(null);
            wndClass.lpfnWndProc = NativeMethods.DefaultWindowProc;
            wndClass.lpszClassName = windowClassName;
            wndClass.hCursor = NativeMethods.LoadCursor(IntPtr.Zero, NativeMethods.IDC_ARROW);

            NativeMethods.RegisterClassEx(ref wndClass);
        }

        #endregion

        #region WndProc Implementation

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {                    
                case NativeMethods.WM_LBUTTONDOWN:
                    mouseState.LeftButton = MouseButtonState.Pressed;
                    if (HwndLButtonDown != null)
                        HwndLButtonDown(this, new HwndMouseEventArgs(mouseState));
                    break;
                case NativeMethods.WM_LBUTTONUP:
                    mouseState.LeftButton = MouseButtonState.Released;
                    if (HwndLButtonUp != null)
                        HwndLButtonUp(this, new HwndMouseEventArgs(mouseState));
                    break;
                case NativeMethods.WM_LBUTTONDBLCLK:
                    if (HwndLButtonDblClick != null)
                        HwndLButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.Left));
                    break;
                case NativeMethods.WM_RBUTTONDOWN:
                    mouseState.RightButton = MouseButtonState.Pressed;
                    if (HwndRButtonDown != null)
                        HwndRButtonDown(this, new HwndMouseEventArgs(mouseState));
                    break;
                case NativeMethods.WM_RBUTTONUP:
                    mouseState.RightButton = MouseButtonState.Released;
                    if (HwndRButtonUp != null)
                        HwndRButtonUp(this, new HwndMouseEventArgs(mouseState));
                    break;
                case NativeMethods.WM_RBUTTONDBLCLK:
                    if (HwndRButtonDblClick != null)
                        HwndRButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.Right));
                    break;
                case NativeMethods.WM_MBUTTONDOWN:
                    mouseState.MiddleButton = MouseButtonState.Pressed;
                    if (HwndMButtonDown != null)
                        HwndMButtonDown(this, new HwndMouseEventArgs(mouseState));
                    break;
                case NativeMethods.WM_MBUTTONUP:
                    mouseState.MiddleButton = MouseButtonState.Released;
                    if (HwndMButtonUp != null)
                        HwndMButtonUp(this, new HwndMouseEventArgs(mouseState));
                    break;
                case NativeMethods.WM_MBUTTONDBLCLK:
                    if (HwndMButtonDblClick != null)
                        HwndMButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.Middle));
                    break;
                case NativeMethods.WM_XBUTTONDOWN:
                    if (((int)wParam & NativeMethods.MK_XBUTTON1) != 0)
                    {
                        mouseState.X1Button = MouseButtonState.Pressed;
                        if (HwndX1ButtonDown != null)
                            HwndX1ButtonDown(this, new HwndMouseEventArgs(mouseState));
                    }
                    else if (((int)wParam & NativeMethods.MK_XBUTTON2) != 0)
                    {
                        mouseState.X2Button = MouseButtonState.Pressed;
                        if (HwndX2ButtonDown != null)
                            HwndX2ButtonDown(this, new HwndMouseEventArgs(mouseState));
                    }
                    break;
                case NativeMethods.WM_XBUTTONUP:
                    if (((int)wParam & NativeMethods.MK_XBUTTON1) != 0)
                    {
                        mouseState.X1Button = MouseButtonState.Released;
                        if (HwndX1ButtonUp != null)
                            HwndX1ButtonUp(this, new HwndMouseEventArgs(mouseState));
                    }
                    else if (((int)wParam & NativeMethods.MK_XBUTTON2) != 0)
                    {
                        mouseState.X2Button = MouseButtonState.Released;
                        if (HwndX2ButtonUp != null)
                            HwndX2ButtonUp(this, new HwndMouseEventArgs(mouseState));
                    }
                    break;
                case NativeMethods.WM_XBUTTONDBLCLK:
                    if (((int)wParam & NativeMethods.MK_XBUTTON1) != 0)
                    {
                        if (HwndX1ButtonDblClick != null)
                            HwndX1ButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.XButton1));
                    }
                    else if (((int)wParam & NativeMethods.MK_XBUTTON2) != 0)
                    {
                        if (HwndX2ButtonDblClick != null)
                            HwndX2ButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.XButton2));
                    }
                    break;
                case NativeMethods.WM_MOUSEMOVE:
                    
                    if (!applicationHasFocus)
                        break;
                                        
                    mouseState.PreviousPosition = mouseState.Position;
                    mouseState.Position = new Point(
                        NativeMethods.GetXLParam((int)lParam),
                        NativeMethods.GetYLParam((int)lParam));

                    if (!mouseInWindow)
                    {
                        mouseInWindow = true;
                        mouseState.PreviousPosition = mouseState.Position;

                        if (HwndMouseEnter != null)
                            HwndMouseEnter(this, new HwndMouseEventArgs(mouseState));

                        
                        NativeMethods.TRACKMOUSEEVENT tme = new NativeMethods.TRACKMOUSEEVENT();
                        tme.cbSize = Marshal.SizeOf(typeof(NativeMethods.TRACKMOUSEEVENT));
                        tme.dwFlags = NativeMethods.TME_LEAVE;
                        tme.hWnd = hwnd;
                        NativeMethods.TrackMouseEvent(ref tme);
                    }

                    
                    if (mouseState.Position != mouseState.PreviousPosition)
                    {
                        if (HwndMouseMove != null)
                            HwndMouseMove(this, new HwndMouseEventArgs(mouseState));
                    }

                    break;
                case NativeMethods.WM_MOUSELEAVE:

                    if (isMouseCaptured)
                        break;
                    ResetMouseState();

                    if (HwndMouseLeave != null)
                        HwndMouseLeave(this, new HwndMouseEventArgs(mouseState));
                    break;
                default:
                    break;
            }

            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}
