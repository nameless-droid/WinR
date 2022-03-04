using System;
using System.Collections.Generic;
using System.Data;
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
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;

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
            //test.Visibility = Visibility.Hidden;
        }

        private int taskbarHeight => Forms.Screen.PrimaryScreen.Bounds.Height - Forms.Screen.PrimaryScreen.WorkingArea.Height;
        //private Drawing.Point getCurrentPos => new Drawing.Point((int)Left, (int)Top);
        private Forms.Screen getCurrentScreen => Forms.Screen.FromPoint(new Drawing.Point((int)Left, (int)Top));
        private int getCurrentScreenLeft => getCurrentScreen.Bounds.Left;
        private int getCurrentScreenY => getCurrentScreen.Bounds.Y;
        //private int currentScreenTaskbarHeight => Forms.Screen.FromPoint(getCurrentPos).Bounds.Height - Forms.Screen.PrimaryScreen.WorkingArea.Height;

        //public Button test = new Button();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region position

            //var toolbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.FullPrimaryScreenHeight - SystemParameters.WindowCaptionHeight;

            //private int WindowsTaskBarHeight => Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height;

            this.Left = getCurrentScreenLeft + 10;
            this.Top = getCurrentScreen.Bounds.Height - taskbarHeight - this.ActualHeight - 10;

                #endregion

            //DataSet myDataSet;
            //myDataSet

            string[] args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                if (args.Length > 1 && args[1] == "-debug")
                {
                    debugCard.Visibility = Visibility.Visible;
                }
            }

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

            foreach (string s in Directory.GetDirectories(systemPath))
            {
                names.Add("\\" + s);
            }


            //names.Add("WPF rocks");
            //names.Add("WCF rocks");
            //names.Add("XAML is fun");
            //names.Add("WPF rules");
            //names.Add("WCF rules");
            //names.Add("WinForms not");


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
                    windowTitle = GetActiveWindowTitle();
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
            string userDir = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
            string s = ((FilteredComboBox)sender).Text;
            //if (string.IsNullOrEmpty(s))
            if (e.Key == Key.Enter)
            {
                /*
                float d = dd("Dokumente");
                float d2 = dd("Musik");
                float d3 = dd2("Musik");
                float d4 = dd33("Musik");
                float d5 = dd33("3d-Objekte");
                float d6 = dd33("3D Objekte");

                if (d > 0.7)
                {
                    ;
                }

                if (dd2("Musik") > 0.7)
                {
                    ;
                }
                
                if (dd33("Musik") > 0.7)
                {
                    ;
                }

                s = s.Replace("\\\\", "\\");
                */



                //s = s.Replace("/", "");
                if (s.StartsWith("/") || s.StartsWith("\\"))
                {
                    s = s.Remove(0, 1);
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
                //else if (Directory.Exists(Path.Combine(userDir, s)))
                //{
                //    string path = Path.Combine(userDir, s);
                //    Process.Start(path);
                //}
                else
                {
                    string pathToOpen = "";

                    //if (dd33(s) > 0.6)
                    string[] list = { "Pictures", "Desktop", "3D Objects", "Documents", "Music", "Downloads", "Videos" };
                    foreach (var item in list)
                    {
                        if (dd2ee(s, item) > 0.6)
                        {
                            switch (item)
                            {
                                case "Pictures":
                                    pathToOpen = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                                    break;
                                case "Desktop":
                                    pathToOpen = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                                    break;
                                case "3D Objects":
                                    //pathToOpen = Environment.GetFolderPath(Environment.SpecialFolder.);
                                    break;
                                case "Documents":
                                    pathToOpen = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                    break;
                                case "Music":
                                    pathToOpen = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                                    break;
                                case "Downloads":
                                    //pathToOpen = Environment.GetFolderPath(Environment.SpecialFolder.);
                                    pathToOpen = "{374DE290-123F-4565-9164-39C4925E467B}";
                                    break;
                                case "Videos":
                                    pathToOpen = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                    }

                    if (pathToOpen != "")
                    {
                        if (pathToOpen.StartsWith("{") && pathToOpen.EndsWith("}"))
                        {
                            //C:\Windows\explorer.exe shell:::{374DE290-123F-4565-9164-39C4925E467B}
                            ProcessStartInfo psi1 = new ProcessStartInfo();
                            psi1.CreateNoWindow = true;
                            psi1.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
                            psi1.FileName = "explorer.exe";
                            psi1.Arguments = "shell:::" + pathToOpen;
                            Process.Start(psi1);
                            return;
                        }
                        else
                        {
                            ProcessStartInfo psi1 = new ProcessStartInfo();
                            psi1.CreateNoWindow = true;
                            psi1.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
                            psi1.FileName = "explorer.exe";
                            psi1.Arguments = pathToOpen;
                            Process.Start(psi1);
                            return;
                        }

                    }



                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.CreateNoWindow = true;
                    psi.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
                    psi.FileName = "cmd";
                    psi.Arguments = "/c start " + s;


                    //psi.FileName = "powershell";
                    //psi.Arguments = "start " + s;
                    //psi.UseShellExecute = true;
                    //psi.ErrorDialog = true;
                    //psi.RedirectStandardError = true;
                    //psi.FileName = s;
                    //psi.Arguments = s;

                    Process.Start(psi);
                }




            }
        }

        private void CloseClick_HideWindow(object sender, RoutedEventArgs e)
        {
            //SetWindowPos(new WindowInteropHelper(this).Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP.HIDEWINDOW | SWP.NOMOVE | SWP.NOSIZE);
            App.Current.Shutdown();
        }

        public float dd(string s)
        {
            int result = 0;
            string str = "Documents";

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == str[i])
                {
                    result++;
                }
            }
            return (float)result / str.Length;
            //return 2.1f;
        }

        public float dd2(string s)
        {
            int result = 0;
            string str = "Music";

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == str[i])
                {
                    result++;
                }
            }
            return (float)result / str.Length;
        }

        public float dd33(string s)
        {
            //int result = 0;
            List<float> results = new List<float>();
            //string str = "Music";
            string[] list = { "Pictures", "Desktop", "3D Objects", "Documents", "Music", "Downloads", "Videos" };

            int tmp = 0;
            for (int k = 0; k < list.Length; k++)
            {
                string current = list[k];
                for (int i = 0; i < s.Length; i++)
                {
                    //if (list[k][i] == str[i])
                    //char c = list[k][i];
                    //if (s[i] == c)
                    if (s.Length > i && current.Length > i && s[i] == current[i])
                    {
                        tmp++;
                    }
                }
                //results.Add(tmp / list[k].Length);
                results.Add((float)tmp / current.Length);
                tmp = 0;
            }

           float max = results.Max();

           return max;
           //return (float)max / str.Length;
        }

        public float dd2ee(string s, string s2)
        {
            int result = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s.Length > i && s2.Length > i && s[i] == s2[i])
                {
                    result++;
                }
            }
            return (float)result / s2.Length;
        }

    }
}
