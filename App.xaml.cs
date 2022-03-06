using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
//
using Path = System.IO.Path;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;
using Rectangle = System.Drawing.Rectangle;

namespace WinR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private int taskbarHeight => Forms.Screen.PrimaryScreen.Bounds.Height - Forms.Screen.PrimaryScreen.WorkingArea.Height;
        //private Drawing.Point getCurrentPos => new Drawing.Point((int)Left, (int)Top);
        private Forms.Screen getCurrentScreen => Forms.Screen.FromPoint(new Drawing.Point((int)App.Current.MainWindow.Left, (int)App.Current.MainWindow.Top));
        private Drawing.Point cursorPos
        {
            get
            {
                Drawing.Point point;
                GetCursorPos(out point);
                return point;
            }
        }
        //private Forms.Screen getScreenWithMouse => Forms.Screen.FromPoint(GetCursorPos());
        private Forms.Screen getScreenWithMouse => Forms.Screen.FromPoint(cursorPos);
        private int getCurrentScreenLeft => getCurrentScreen.Bounds.Left;
        private int getCurrentScreenY => getCurrentScreen.Bounds.Y;
        //private int currentScreenTaskbarHeight => Forms.Screen.FromPoint(getCurrentPos).Bounds.Height - Forms.Screen.PrimaryScreen.WorkingArea.Height;


        public App()
        {
            Thread thread = new Thread(new ThreadStart(HookThread));
            thread.Start();
        }

        private void SetWndPosBasedOnWhichScreenCursorIsOn()
        {
            #region position

            //var toolbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.FullPrimaryScreenHeight - SystemParameters.WindowCaptionHeight;

            //private int WindowsTaskBarHeight => Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height;

            //this.Left = getCurrentScreenLeft + 10;
            //this.Top = getCurrentScreen.Bounds.Height - taskbarHeight - this.ActualHeight - 10;

            App.Current.MainWindow.Left = getScreenWithMouse.Bounds.Left + 10;
            App.Current.MainWindow.Top = getScreenWithMouse.Bounds.Height - taskbarHeight - App.Current.MainWindow.ActualHeight - 10;

            #endregion
        }

        private void SetWndPosRelativeBasedOnWhichScreenCursorIsOn()
        {
            #region position
            double left = App.Current.MainWindow.Left;
            double top = App.Current.MainWindow.Top;
            
            //if (left > getScreenWithMouse.Bounds.Left && left < getScreenWithMouse.Bounds.Width)
            //{
            //    //App.Current.MainWindow.Left -= getScreenWithMouse.Bounds.Left;
            //    //App.Current.MainWindow.Left = getScreenWithMouse.Bounds.Left - App.Current.MainWindow.Left;
            //    //App.Current.MainWindow.Left = App.Current.MainWindow.Left + getScreenWithMouse.Bounds.Width;
            //}
            if (left < getScreenWithMouse.Bounds.Left)
            {
                //App.Current.MainWindow.Left -= getScreenWithMouse.Bounds.Left;
                //App.Current.MainWindow.Left = getScreenWithMouse.Bounds.Left - App.Current.MainWindow.Left;
                App.Current.MainWindow.Left = App.Current.MainWindow.Left + getScreenWithMouse.Bounds.Width;
            }
            //else if (left < getScreenWithMouse.Bounds.Left && left > getScreenWithMouse.Bounds.Width)
            //{
            //    //App.Current.MainWindow.Left -= getScreenWithMouse.Bounds.Left;
            //    //App.Current.MainWindow.Left = getScreenWithMouse.Bounds.Left - App.Current.MainWindow.Left;
            //    //App.Current.MainWindow.Left = App.Current.MainWindow.Left + getScreenWithMouse.Bounds.Width;
            //    MessageBox.Show("dd");
            //}
            else if (left > getScreenWithMouse.Bounds.Left + getScreenWithMouse.Bounds.Width)
            {
                App.Current.MainWindow.Left = App.Current.MainWindow.Left - getScreenWithMouse.Bounds.Width;
            }

            //else if (left < getScreenWithMouse.Bounds.Left)
            //{
            //    //App.Current.MainWindow.Left = getScreenWithMouse.Bounds.Left + App.Current.MainWindow.Left;
            //}

            
            //App.Current.MainWindow.Top = getScreenWithMouse.Bounds.Height - taskbarHeight - App.Current.MainWindow.ActualHeight - 10;

            #endregion
        }

        public async void HookThread()
        {
            WinEventDelegate dele = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
            //string window = GetActiveWindowTitle();
            IntPtr intPtr = GetForegroundWindow();
            string windowClass = GetActiveWindowClass();
            string windowTitle = GetActiveWindowTitle();
            //Console.WriteLine(window);
            while (true)
            {
                //if (window != GetActiveWindowTitle())
                //{
                //    window = GetActiveWindowTitle();
                //    Console.WriteLine(window);
                //    Debug.WriteLine(window);
                //}


                if (windowClass == "#32770" && (windowTitle.Equals("Ausführen") || windowTitle.Equals("Run")))
                {
                    windowClass = GetActiveWindowClass();
                    //this.Show();
                    //ShowWindow(new WindowInteropHelper(this).Handle, 5);
                    //SendMessage(intPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                    //App.Current.Dispatcher.BeginInvoke({
                    //});

                    //App.Current.MainWindow.Dispatcher.BeginInvoke((Action)(() =>
                    //{
                    //    SetWndPosBasedOnWhichScreenCursorIsOn();
                    //}));

                    this.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(
                        delegate ()
                        {
                            //SetWndPosBasedOnWhichScreenCursorIsOn();
                            SetWndPosRelativeBasedOnWhichScreenCursorIsOn();

                            //ShowWindow(intPtr, 0);
                            //SetWindowPos(intPtr, HWND_BOTTOM, (int)App.Current.MainWindow.Left, (int)App.Current.MainWindow.Top, 0, 0, SWP.HIDEWINDOW | SWP.NOACTIVATE);

                            //SendMessage(intPtr, WM_CLOSE, 0, IntPtr.Zero);
                            //await Task.Delay(10);
                            ShowWindow(new WindowInteropHelper(App.Current.MainWindow).Handle, 9);
                            SetWindowPos(new WindowInteropHelper(App.Current.MainWindow).Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP.SHOWWINDOW | SWP.NOMOVE | SWP.NOSIZE);
                            SetWindowPos(new WindowInteropHelper(App.Current.MainWindow).Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP.SHOWWINDOW | SWP.NOMOVE | SWP.NOSIZE);
                            App.Current.MainWindow.Show();
                            App.Current.MainWindow.Activate();
                            

                            SendMessage(intPtr, WM_CLOSE, 0, IntPtr.Zero);
                        }
                    ));


                }

                if (windowClass != GetActiveWindowClass())
                {
                    windowClass = GetActiveWindowClass();
                    intPtr = GetForegroundWindow();
                    windowTitle = GetActiveWindowTitle();
                    Console.WriteLine(windowClass);
                    Debug.WriteLine(windowClass);
                }
                await Task.Delay(1);
                //await Task.Delay(TimeSpan.FromMilliseconds(0.9));
                //await Task.Delay(500);
            }
        }

        private static string GetActiveWindowClass()
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

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return "";
        }

        public static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Console.WriteLine(GetActiveWindowTitle());
        }

        #region imports
        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        #endregion

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

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

        const UInt32 WM_CLOSE = 0x0010;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, ref nint lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, nint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool GetCursorPos(out POINT lpPoint);
        static extern bool GetCursorPos(out Drawing.Point lpPoint);
    }
}
