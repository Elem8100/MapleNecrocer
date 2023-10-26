using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
namespace DPIUtils;

public static class DPIUtil
{
    public static float dpiX,dpiY;
    private const int MinOSVersionBuild = 14393;
    private const int MinOSVersionMajor = 10;
    private static bool _isSupportingDpiPerMonitor;
    private static bool _isOSVersionChecked;

    internal static bool IsSupportingDpiPerMonitor
    {
        get
        {
            if (_isOSVersionChecked)
            {
                return _isSupportingDpiPerMonitor;
            }

            _isOSVersionChecked = true;
            var osVersionInfo = new OSVERSIONINFOEXW
            {
                dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEXW))
            };

            if (RtlGetVersion(ref osVersionInfo) != 0)
            {
                _isSupportingDpiPerMonitor = Environment.OSVersion.Version.Major >= MinOSVersionMajor && Environment.OSVersion.Version.Build >= MinOSVersionBuild;

                return _isSupportingDpiPerMonitor;
            }

            _isSupportingDpiPerMonitor = osVersionInfo.dwMajorVersion >= MinOSVersionMajor && osVersionInfo.dwBuildNumber >= MinOSVersionBuild;

            return _isSupportingDpiPerMonitor;
        }
    }

    public static double ScaleFactor(Control control, Point monitorPoint)
    {
        var dpi = GetDpi(control, monitorPoint);

        return dpi / 96.0;
    }

    public static uint GetDpi(Control control, Point monitorPoint)
    {
        uint dpiX;

        if (IsSupportingDpiPerMonitor)
        {
            var monitorFromPoint = MonitorFromPoint(monitorPoint, 2);

            GetDpiForMonitor(monitorFromPoint, DpiType.Effective, out dpiX, out _);
        }
        else
        {
            // If using with System.Windows.Forms - can be used Control.DeviceDpi
            dpiX = control == null ? 96 : (uint)control.DeviceDpi;
        }

        return dpiX;
    }

    [DllImport("User32.dll")]
    internal static extern IntPtr MonitorFromPoint([In] Point pt, [In] uint dwFlags);

    [DllImport("Shcore.dll")]
    private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

    [SecurityCritical]
    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern int RtlGetVersion(ref OSVERSIONINFOEXW versionInfo);

    [StructLayout(LayoutKind.Sequential)]
    private struct OSVERSIONINFOEXW
    {
        internal int dwOSVersionInfoSize;
        internal int dwMajorVersion;
        internal int dwMinorVersion;
        internal int dwBuildNumber;
        internal int dwPlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string szCSDVersion;
        internal ushort wServicePackMajor;
        internal ushort wServicePackMinor;
        internal short wSuiteMask;
        internal byte wProductType;
        internal byte wReserved;
    }

    private enum DpiType
    {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }
}
