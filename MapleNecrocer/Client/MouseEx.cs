using System.Runtime.InteropServices;
using MouseState=Microsoft.Xna.Framework.Input.MouseState;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
namespace MouseExt;
public  class MouseEx
{
    [DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int X, int Y);

    private static Control _window;
    private static MouseInputWnd _mouseInputWnd = new MouseInputWnd();
    private static IntPtr PlatformGetWindowHandle()
    {
        return (_window == null) ? IntPtr.Zero : _window.Handle;
    }

    public static void PlatformSetWindowHandle(IntPtr windowHandle)
    {
        // Unregister old window
        if (_mouseInputWnd.Handle != IntPtr.Zero)
            _mouseInputWnd.ReleaseHandle();

        _window = Control.FromHandle(windowHandle);
        _mouseInputWnd.AssignHandle(windowHandle);
    }
  
    public static MouseState GetState()
    {
        if (_window != null)
        {
            var pos = Control.MousePosition;
            var clientPos = _window.PointToClient(pos);
            var buttons = Control.MouseButtons;

            return new MouseState(
                clientPos.X,
                clientPos.Y,
                _mouseInputWnd.ScrollWheelValue,
                (buttons & MouseButtons.Left) == MouseButtons.Left ? ButtonState.Pressed : ButtonState.Released,
                (buttons & MouseButtons.Middle) == MouseButtons.Middle ? ButtonState.Pressed : ButtonState.Released,
                (buttons & MouseButtons.Right) == MouseButtons.Right ? ButtonState.Pressed : ButtonState.Released,
                (buttons & MouseButtons.XButton1) == MouseButtons.XButton1 ? ButtonState.Pressed : ButtonState.Released,
                (buttons & MouseButtons.XButton2) == MouseButtons.XButton2 ? ButtonState.Pressed : ButtonState.Released,
                _mouseInputWnd.HorizontalScrollWheelValue
                );
        }
        return new MouseState();//_defaultState;
    }

    public static void PlatformSetPosition(int x, int y)
    {
        //  PrimaryWindow.MouseState.X = x;
        // PrimaryWindow.MouseState.Y = y;
        var pt = _window.PointToScreen(new System.Drawing.Point(x, y));
        SetCursorPos(pt.X, pt.Y);
    }

    class MouseInputWnd : System.Windows.Forms.NativeWindow
    {
        const int WM_MOUSEMOVE = 0x0200;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_RBUTTONUP = 0x0205;
        const int WM_MBUTTONDOWN = 0x0207;
        const int WM_MBUTTONUP = 0x0208;
        const int WM_MOUSEWHEEL = 0x020A;
        const int WM_MOUSEHWHEEL = 0x020E;
        public int ScrollWheelValue = 0;
        public int HorizontalScrollWheelValue = 0;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_MOUSEWHEEL:
                    var delta = (short)(((ulong)m.WParam >> 16) & 0xffff);
                    ScrollWheelValue += delta;
                    break;
                case WM_MOUSEHWHEEL:
                    var deltaH = (short)(((ulong)m.WParam >> 16) & 0xffff);
                    HorizontalScrollWheelValue += deltaH;
                    break;
            }

            base.WndProc(ref m);
        }
    }

}
