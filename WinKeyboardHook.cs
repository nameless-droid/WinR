using Gma.UserActivityMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Path = System.IO.Path;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;
using Rectangle = System.Drawing.Rectangle;

using static DllImports;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WinR
{
    internal class WinKeyboardHook
    {
        public static void Sub()
        {
            HookManager.KeyPress += HookManager_KeyPress;
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;
        }

        public static List<Keys> keysDown = new List<Keys>();
        private static void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            //Used for overriding the Windows default hotkeys
            if (keysDown.Contains(e.KeyCode) == false)
            {
                keysDown.Add(e.KeyCode);
            }

            /*
            if (e.KeyCode == Keys.Right && WIN())
            {
                e.Handled = true;
                //Do what you want when this key combination is pressed
            }
            else if (e.KeyCode == Keys.Left && WIN())
            {
                e.Handled = true;
                //Do what you want when this key combination is pressed
            }
            */

            if (e.KeyCode == Keys.R && WIN())
            {
                System.Windows.Window mainWindow = App.Current.MainWindow;

                e.Handled = true;
                //App.Current.Dispatcher.BeginInvoke(new Action(() =>

                IntPtr intPtr = new WindowInteropHelper(App.Current.MainWindow).Handle;

                //SetForegroundWindow(intPtr);
                uint flags = (uint) (SWP.SHOWWINDOW | SWP.NOMOVE | SWP.NOSIZE);
                DllImports.SetWindowPos(intPtr, DllImports.HWND.TopMost, 0, 0, 0, 0, flags);
                DllImports.SetWindowPos(intPtr, DllImports.HWND.NoTopMost, 0, 0, 0, 0, flags);

                App.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
                App.Current.MainWindow.Show();
                App.Current.MainWindow.Activate();
                

                //App.Current.MainWindow.SetPositionToCurrentWindowOrDefaultPosOnWnd();
                //MainWindow.SetPositionToCurrentWindowOrDefaultPosOnWnd()
                //SetDefaultPosOnCurrentScreen();
                //MoveToScreenButKeepRelativePos();
                SetWndPosRelativeBasedOnWhichScreenCursorIsOn();
            }

        }


        private static Drawing.Point cursorPos
        {
            get
            {
                POINT point;
                GetCursorPos(out point);
                return point;
            }
        }

        //private Forms.Screen getScreenWithMouse => Forms.Screen.FromPoint(GetCursorPos());
        private static Forms.Screen getScreenWithMouse => Forms.Screen.FromPoint(cursorPos);
        private static Forms.Screen getCurrentScreen => Forms.Screen.FromPoint(new Drawing.Point((int)App.Current.MainWindow.Left, (int)App.Current.MainWindow.Top));
        private int getCurrentScreenLeft => getCurrentScreen.Bounds.Left;
        private int getCurrentScreenY => getCurrentScreen.Bounds.Y;
        //private int currentScreenTaskbarHeight => Forms.Screen.FromPoint(getCurrentPos).Bounds.Height - Forms.Screen.PrimaryScreen.WorkingArea.Height;

        public static void SetDefaultPosOnCurrentScreen()
        {
            //App.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            //int taskbarHeight = Forms.Screen.PrimaryScreen.Bounds.Height - Forms.Screen.PrimaryScreen.WorkingArea.Height;
            int taskbarHeight = getScreenWithMouse.Bounds.Height - getScreenWithMouse.WorkingArea.Height;

            App.Current.MainWindow.Left = 10;
            App.Current.MainWindow.Top = getScreenWithMouse.Bounds.Height - App.Current.MainWindow.ActualHeight - taskbarHeight - 10;
        }

        //static void SetPositionToCurrentWindowOrDefaultPosOnWnd()
        static void MoveToScreenButKeepRelativePos()
        {
            /*
            System.Windows.Window wnd = App.Current.MainWindow;
            //wnd.Left = 10;
            if (wnd.Left < getScreenWithMouse.Bounds.Left)
            {
                wnd.Left = getScreenWithMouse.Bounds.Left + wnd.Left;
            }
            else if (wnd.Left > getScreenWithMouse.Bounds.Right)
            //else if (wnd.Left)
            {
                wnd.Left = getScreenWithMouse.Bounds.Left - wnd.Left;
            }
            */
        }

        private static void SetWndPosRelativeBasedOnWhichScreenCursorIsOn()
        {
            #region position
            double left = App.Current.MainWindow.Left;
            double top = App.Current.MainWindow.Top;

            if (left < getScreenWithMouse.Bounds.Left)
            {
                App.Current.MainWindow.Left = App.Current.MainWindow.Left + getScreenWithMouse.Bounds.Width;
            }

            else if (left > getScreenWithMouse.Bounds.Left + getScreenWithMouse.Bounds.Width)
            {
                App.Current.MainWindow.Left = App.Current.MainWindow.Left - getScreenWithMouse.Bounds.Width;
            }

            #endregion
        }

        private static void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            //Used for overriding the Windows default hotkeys
            while (keysDown.Contains(e.KeyCode))
            {
                keysDown.Remove(e.KeyCode);
            }
        }

        private static void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Used for overriding the Windows default hotkeys

        }

        public static bool CTRL()
        {
            //return keysDown.Contains(Keys.LShiftKey)
            if (keysDown.Contains(Keys.LControlKey) ||
                keysDown.Contains(Keys.RControlKey) ||
                keysDown.Contains(Keys.Control) ||
                keysDown.Contains(Keys.ControlKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SHIFT()
        {
            //return keysDown.Contains(Keys.LShiftKey)
            if (keysDown.Contains(Keys.LShiftKey) ||
                keysDown.Contains(Keys.RShiftKey) ||
                keysDown.Contains(Keys.Shift) ||
                keysDown.Contains(Keys.ShiftKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool WIN()
        {
            //return keysDown.Contains(Keys.LShiftKey)
            if (keysDown.Contains(Keys.LWin) ||
                keysDown.Contains(Keys.RWin))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ALT()
        {
            //return keysDown.Contains(Keys.LShiftKey)
            if (keysDown.Contains(Keys.Alt) ||
                keysDown.Contains(Keys.LMenu) ||
                keysDown.Contains(Keys.RMenu))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
