using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace WinR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string systemPath = "C:";
            string systemPath = Path.GetPathRoot(Environment.SystemDirectory);
            //string systemPath = Path.GetPathRoot(Environment.dir);
            //FileInfo fileInfo = new FileInfo(systemPath);
            string[] s1 = Environment.GetLogicalDrives();
            List<String> names = new List<string>();

            foreach (var item in s1)
            {
                foreach (string s in Directory.GetFileSystemEntries(item))
                {
                    names.Add("/" + s);
                }
            }


            names.Add("WPF rocks");
            names.Add("WCF rocks");
            names.Add("XAML is fun");
            names.Add("WPF rules");
            names.Add("WCF rules");
            names.Add("WinForms not");


            FilteredComboBox1.Items.Clear();


            FilteredComboBox1.IsEditable = true;
            FilteredComboBox1.IsTextSearchEnabled = false;
            FilteredComboBox1.ItemsSource = names;

            Hook();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        public async void Hook()
        {
            WinEventDelegate dele = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
            //string window = GetActiveWindowTitle();
            IntPtr intPtr = GetForegroundWindow();
            string windowClass = GetActiveWindowClass();
            //Console.WriteLine(window);
            while (true)
            {
                //if (window != GetActiveWindowTitle())
                //{
                //    window = GetActiveWindowTitle();
                //    Console.WriteLine(window);
                //    Debug.WriteLine(window);
                //}


                if (windowClass == "#32770")
                {
                    windowClass = GetActiveWindowClass();
                    //this.Show();
                    //ShowWindow(new WindowInteropHelper(this).Handle, 5);
                    //SendMessage(intPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    //SendMessage(intPtr, WM_CLOSE, 0, IntPtr.Zero);
                    //await Task.Delay(10);
                    ShowWindow(new WindowInteropHelper(this).Handle, 9);
                    SetWindowPos(new WindowInteropHelper(this).Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP.SHOWWINDOW | SWP.NOMOVE | SWP.NOSIZE);
                    SetWindowPos(new WindowInteropHelper(this).Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP.SHOWWINDOW | SWP.NOMOVE | SWP.NOSIZE);
                    this.Show();
                    this.Activate();
                }

                if (windowClass != GetActiveWindowClass())
                {
                    windowClass = GetActiveWindowClass();
                    intPtr = GetForegroundWindow();
                    Console.WriteLine(windowClass);
                    Debug.WriteLine(windowClass);
                }
                await Task.Delay(1);
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

        private void FilteredComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                string s = ((FilteredComboBox)sender).Text;
                s = s.Replace("/", "");

                //Process.Start(((FilteredComboBox)sender).Text);
                ProcessStartInfo psi = new ProcessStartInfo();
                //psi.UseShellExecute = false;    
                //psi.RedirectStandardOutput = true;
                //psi.RedirectStandardError = true;
                //psi.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
                psi.FileName = "explorer";
                psi.Arguments = s;
                Process.Start(psi); 

            }
        }
    }
}
