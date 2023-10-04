global using MonoGame.SpriteEngine;
global using WzComparerR2.Common;
using System.Runtime.InteropServices;

namespace MapleNecrocer;
internal static class Program
{
  

    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        Application.SetHighDpiMode(HighDpiMode.PerMonitor);
               
        ApplicationConfiguration.Initialize();
    
        Application.Run(new MainForm());
    }
}