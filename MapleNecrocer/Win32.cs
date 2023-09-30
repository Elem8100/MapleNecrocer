using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MapleNecrocer;

public class Win32
{
    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
    public static void SendMessage(IntPtr hWnd, bool wParam)
    {
        SendMessage(hWnd, 11, wParam, 0);
    }
}
