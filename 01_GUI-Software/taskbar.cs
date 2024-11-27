using System;
using System.Runtime.InteropServices;

public class taskbar
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string className, string windowText);

    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hwnd, int command);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, string className, string windowText);

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 1;

    public static void Hide()
    {
        // Haupt-Taskleiste
        IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", "");
        ShowWindow(taskbarHandle, SW_HIDE);

        // Sekundäre Taskleisten
        IntPtr secondaryTaskbarHandle = IntPtr.Zero;
        while (true)
        {
            secondaryTaskbarHandle = FindWindowEx(IntPtr.Zero, secondaryTaskbarHandle, "Shell_SecondaryTrayWnd", "");
            if (secondaryTaskbarHandle == IntPtr.Zero) break;
            ShowWindow(secondaryTaskbarHandle, SW_HIDE);
        }
    }

    public static void Show()
    {
        // Haupt-Taskleiste
        IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", "");
        ShowWindow(taskbarHandle, SW_SHOW);

        // Sekundäre Taskleisten
        IntPtr secondaryTaskbarHandle = IntPtr.Zero;
        while (true)
        {
            secondaryTaskbarHandle = FindWindowEx(IntPtr.Zero, secondaryTaskbarHandle, "Shell_SecondaryTrayWnd", "");
            if (secondaryTaskbarHandle == IntPtr.Zero) break;
            ShowWindow(secondaryTaskbarHandle, SW_SHOW);
        }
    }
}