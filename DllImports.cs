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



    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    static readonly IntPtr HWND_TOP = new IntPtr(0);
    static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    /// <summary>
    /// Window handles (HWND) used for hWndInsertAfter
    /// </summary>
    public static class HWND
    {
        public static IntPtr
        NoTopMost = new IntPtr(-2),
        TopMost = new IntPtr(-1),
        Top = new IntPtr(0),
        Bottom = new IntPtr(1);
    }

    /// <summary>
    /// SetWindowPos Flags
    /// </summary>
    public static class SWP
    {
        public static readonly int
        NOSIZE = 0x0001,
        NOMOVE = 0x0002,
        NOZORDER = 0x0004,
        NOREDRAW = 0x0008,
        NOACTIVATE = 0x0010,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        SHOWWINDOW = 0x0040,
        HIDEWINDOW = 0x0080,
        NOCOPYBITS = 0x0100,
        NOOWNERZORDER = 0x0200,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        DEFERERASE = 0x2000,
        ASYNCWINDOWPOS = 0x4000;
    }
}