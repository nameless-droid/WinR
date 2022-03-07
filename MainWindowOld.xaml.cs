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

using System.Threading;
using System.Windows.Threading;


using Path = System.IO.Path;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;

namespace WinR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowOld : Window
    {
        //public static DependencyProperty WndActiveProperty = DependencyProperty.Register("WndActive", typeof(bool), typeof(Button), new PropertyMetadata(""));
        //public bool WndActive
        //{
        //    get { return (bool)GetValue(WndActiveProperty); }
        //    set { SetValue(WndActiveProperty, value); }
        //}

        //bool wndactive = false;
        ////public static DependencyProperty WndActiveProperty = DependencyProperty.Register("WndActive", typeof(bool), typeof(Button), new PropertyMetadata(""));
        //public bool WndActive
        //{
        //    get { return wndactive; }
        //    set { wndactive = value; }
        //}

        public MainWindowOld()
        {
            InitializeComponent();
            //test.Visibility = Visibility.Hidden;
        }


        //public Button test = new Button();

        //public 

        public void WorkThreadFunction()
        {
            try
            {
                // do any background work

            }
            catch (Exception ex)
            {
                // log errors
            }
        }

        private int taskbarHeight => Forms.Screen.PrimaryScreen.Bounds.Height - Forms.Screen.PrimaryScreen.WorkingArea.Height;
        private Forms.Screen getScreenWithMouse => Forms.Screen.FromPoint(cursorPos);
        private Drawing.Point cursorPos
        {
            get
            {
                Drawing.Point point;
                GetCursorPos(out point);
                return point;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



            //App.Current.MainWindow.Left = getScreenWithMouse.Bounds.Left + 10;
            //App.Current.MainWindow.Top = getScreenWithMouse.Bounds.Height - taskbarHeight - App.Current.MainWindow.ActualHeight - 10;
            Left = 8;
            Top = 833;
            //


            //SetWndPosBasedOnWhichScreenCursorIsOn();
            //DataSet myDataSet;
            //myDataSet

            string[] args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                if (args.Length > 1 && args[1] == "-debug")
                {
                    //debugCard.Visibility = Visibility.Visible;
                    foreach (FrameworkElement item in mainGrid.Children)
                    {
                        if (item.Name.ToLower().Contains("debug"))
                        {
                            item.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    foreach (FrameworkElement item in mainGrid.Children)
                    {
                        if (item.Name.ToLower().Contains("debug"))
                        {
                            item.Visibility = Visibility.Collapsed;
                        }
                    }
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

            //Hook();
            //HookThread();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        //public async void Hook()




        private void FilteredComboBox1_KeyDown(object sender, KeyEventArgs e)
        {







            //((ComboBox)sender).IsDropDownOpen = true;   
            //return;
            string userDir = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
            string s = ((FilteredComboBox)sender).Text;
            //if (string.IsNullOrEmpty(s))
            if (e.Key == Key.Enter)
            {
                var fileName = "command-history.txt";
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }
                StreamWriter sw = File.AppendText(fileName);
                //sw.Close();

                bool found = false;

                //if (s.StartsWith("<"))
                //{
                    if (s.StartsWith("< "))
                    {
                        s = s.Remove(0, 2);
                    }
                    else if(s.StartsWith("<"))
                    {
                        s = s.Remove(0, 1);
                    }

                    string[] lines = System.IO.File.ReadAllLines(@"custom.txt");

                    foreach (var item in lines)
                    {

                        string[] str = item.Split(";");
                        //if (str[0].Equals(s))
                        //s = str[1];
                        if (s.Equals(str[0]))
                        {
                            //Process.Start(str[1]);

                            ProcessStartInfo psi = new ProcessStartInfo();
                            psi.CreateNoWindow = true;
                            psi.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
                            psi.FileName = "cmd";
                            //psi.Arguments = "/c start " + s;
                            psi.Arguments = "/c start " + str[1];
                            //sw.WriteLine(s);
                            sw.WriteLine(str[1]);

                            //psi.FileName = "powershell";
                            //psi.Arguments = "start " + s;
                            //psi.UseShellExecute = true;
                            //psi.ErrorDialog = true;
                            //psi.RedirectStandardError = true;
                            //psi.FileName = s;
                            //psi.Arguments = s;

                            Process.Start(psi);

                            //return;
                            found = true;
                            break;
                        }
                    }



                    //s.Replace("< ", "");
                //}


                if (found == true)
                {
                    return;
                }

                //FilteredComboBox1.Style = ;

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
                    psi.Arguments = "\""+s+"\"" ;

                    sw.WriteLine(psi.FileName + " " + psi.Arguments);

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
                            sw.WriteLine(psi1.FileName + " " + psi1.Arguments);
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
                            sw.WriteLine(psi1.FileName + " " + psi1.Arguments);
                            return;
                        }

                    }
                    


                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.CreateNoWindow = true;
                    psi.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
                    psi.FileName = "cmd";
                    psi.Arguments = "/c start " + s;
                    sw.WriteLine(s);

                    //psi.FileName = "powershell";
                    //psi.Arguments = "start " + s;
                    //psi.UseShellExecute = true;
                    //psi.ErrorDialog = true;
                    //psi.RedirectStandardError = true;
                    //psi.FileName = s;
                    //psi.Arguments = s;

                    Process.Start(psi);
                }



                sw.Close();
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


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool GetCursorPos(out POINT lpPoint);
        static extern bool GetCursorPos(out Drawing.Point lpPoint);


        //static extern Drawing.Point GetCursorPos();
        // DON'T use System.Drawing.Point, the order of the fields in System.Drawing.Point isn't guaranteed to stay the same.


        private void Window_Activated(object sender, EventArgs e)
        {
            foreach (FrameworkElement item in mainGrid.Children)
            {
                if (item is not FilteredComboBox)
                    item.Focusable = false;
            }
            //FilteredComboBox1.Focusable = true;

            FilteredComboBox1.Focusable = true;
            FilteredComboBox1.Focus();

            FilteredComboBox1.Focus();
            Keyboard.Focus(FilteredComboBox1 as ComboBox);

            Dispatcher.BeginInvoke(DispatcherPriority.Input,
            new Action(delegate ()
            {
                FilteredComboBox1.Focus();         // Set Logical Focus
                Keyboard.Focus(FilteredComboBox1); // Set Keyboard Focus
            }));

            //WndActive = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //WndActive = false;
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!FilteredComboBox1.IsKeyboardFocusWithin)
            {
                //e.Handled = true;
                //FilteredComboBox1.Focus();
                //Keyboard.Focus(FilteredComboBox1 as ComboBox);
                ////Keyboard.Focus(FilteredComboBox1);
                ///

                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    FilteredComboBox1.Focus();         // Set Logical Focus
                    Keyboard.Focus(FilteredComboBox1); // Set Keyboard Focus
                }));

                await Task.Delay(100);

                //char ch = (char)e.Key;
                var ch = (char)KeyInterop.VirtualKeyFromKey(e.Key);
                if (!char.IsLetter(ch)) {
                    //FilteredComboBox1.Text += e.Key;
                    return; 
                }

                bool upper = false;
                if (Keyboard.IsKeyToggled(Key.Capital) || Keyboard.IsKeyToggled(Key.CapsLock))
                {
                    upper = !upper;
                }
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    upper = !upper;
                }
                if (!upper)
                {
                    ch = char.ToLower(ch);
                }

                FilteredComboBox1.Text += ch;
                //FilteredComboBox1.SelectedIndex = -1;
                //FilteredComboBox1.CaretIndex = FilteredComboBox1.inde;

                //var myTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
                //if (myTextBox != null)
                //{
                //    this.FilteredComboBox1 = myTextBox;
                //}

                ///FilteredComboBox1.SetCaret(FilteredComboBox1.Text.Length);

                //var textbox = sender as TextBox;
                //if (textbox != null)
                //{
                //    //you can write your own logic.
                //    textbox.CaretIndex = textbox.Text.Length;
                //}
            }


        }



        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!FilteredComboBox1.IsKeyboardFocusWithin)
            {
                //e.Handled = true;
                //FilteredComboBox1.Focus();
                //Keyboard.Focus(FilteredComboBox1 as ComboBox);
                ////Keyboard.Focus(FilteredComboBox1);
                ///

                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    FilteredComboBox1.Focus();         // Set Logical Focus
                    Keyboard.Focus(FilteredComboBox1); // Set Keyboard Focus
                }));
            }
        }


    }
}