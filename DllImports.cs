using System;
using System.Runtime.InteropServices;
using System.Text;

public static partial class DllImports
{
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out POINT lpPoint);
    //static extern bool GetCursorPos(out Drawing.Point lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }

    //

    /// <summary>
    /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
    /// </summary>
    /// <remarks>The EnumChildWindows function is more reliable than calling GetWindow in a loop. An application that
    /// calls GetWindow to perform this task risks being caught in an infinite loop or referencing a handle to a window
    /// that has been destroyed.</remarks>
    /// <param name="hWnd">A handle to a window. The window handle retrieved is relative to this window, based on the
    /// value of the uCmd parameter.</param>
    /// <param name="uCmd">The relationship between the specified window and the window whose handle is to be
    /// retrieved.</param>
    /// <returns>
    /// If the function succeeds, the return value is a window handle. If no window exists with the specified relationship
    /// to the specified window, the return value is NULL. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

    //

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();
    //
    /// <summary>
    ///     Retrieves a handle to the foreground window (the window with which the user is currently working). The system
    ///     assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
    ///     <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms633505%28v=vs.85%29.aspx for more information.</para>
    /// </summary>
    /// <returns>
    ///     C++ ( Type: Type: HWND )<br /> The return value is a handle to the foreground window. The foreground window
    ///     can be NULL in certain circumstances, such as when a window is losing activation.
    /// </returns>
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    //

    public static string GetActiveWindowClass()
    {
        const int nChars = 256;
        IntPtr handle = IntPtr.Zero;
        StringBuilder Buff = new StringBuilder(nChars);
        handle = GetForegroundWindow();

        if (GetClassName(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return "";
    }

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
}