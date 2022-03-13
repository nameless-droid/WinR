using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Windows.UI.Xaml.Controls;

using System.Diagnostics;
using Path = System.IO.Path;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;
using Rectangle = System.Drawing.Rectangle;
using static DllImports;
using System.Windows.Interop;

namespace WinR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> list = new List<string>();

        //MainWindow Singleton;

        //public MainWindow Instance
        //{
        //    get {
        //        if (App.Current.Windows[] MainWindow)
        //        {

        //        }    
        //        return Singleton; 
        //    }
        //}

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //autoSuggestBox.DataContext = this;

            string systemPath = Path.GetPathRoot(Environment.SystemDirectory);
            //string systemPath = Path.GetPathRoot(Environment.dir);
            //FileInfo fileInfo = new FileInfo(systemPath);
            string[] s1 = Environment.GetLogicalDrives();
            List<string> names = new List<string>();

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

            list.AddRange(names);

            //names.Add("WPF rocks");
            //names.Add("WCF rocks");
            //names.Add("XAML is fun");
            //names.Add("WPF rules");
            //names.Add("WCF rules");
            //names.Add("WinForms not");


            autoSuggestBox.Items.Clear();


            autoSuggestBox.IsTextSearchEnabled = false;
            autoSuggestBox.ItemsSource = names;

            //autoSuggestBox.ItemsSource = this;

            WinKeyboardHook.Sub();
            WinKeyboardHook.SetDefaultPosOnCurrentScreen();
        }

        

        internal static void SetPositionToCurrentWindowOrDefaultPosOnWnd()
        {
            
        }

        private void AutoSuggestBox_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            //AutoSuggestBox autoSuggestBox = sender as AutoSuggestBox;
            //okButton.IsEnabled = autoSuggestBox.Text.Length > 0;
            okButton.IsEnabled = autoSuggestBox.Text.Trim().Length > 0;



            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var item in list)
            {
                var found = splitText.All((key) =>
                {
                    return item.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add(item);
                }
            }
            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }

        private void AutoSuggestBox_SuggestionChosen(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            
        }

        //DateTime lastTime = DateTime.Now;
        MessageBoxResult result;

        private bool ExecuteCustomCommands(string cmd)
        {

            //((ComboBox)sender).IsDropDownOpen = true;   
            //return;
            string userDir = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
            //string s = ((FilteredComboBox)sender).Text;
            //if (string.IsNullOrEmpty(s))


            var fileName = "command-history.txt";
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }

            //StreamWriter sw = File.AppendText(fileName);


            //sw.Close();

            bool found = false;

            //if (cmd.StartsWith("<"))
            //{
            if (cmd.StartsWith("< "))
            {
                cmd = cmd.Remove(0, 2);
            }
            else if (cmd.StartsWith("<"))
            {
                cmd = cmd.Remove(0, 1);
            }

            string[] lines = System.IO.File.ReadAllLines(@"custom.txt");

            foreach (var item in lines)
            {

                string[] str = item.Split(";");
                //if (str[0].Equals(s))
                //s = str[1];
                if (cmd.Equals(str[0]))
                {
                    //Process.Start(str[1]);

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.CreateNoWindow = true;
                    psi.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
                    psi.FileName = "cmd";
                    //psi.Arguments = "/c start " + s;
                    psi.Arguments = "/c start " + str[1];
                    //sw.WriteLine(s);
                    using (var stream = File.AppendText(fileName))
                    {
                        stream.WriteLine(str[1]);
                    }


                    //psi.FileName = "powershell";
                    //psi.Arguments = "start " + s;
                    //psi.UseShellExecute = true;
                    //psi.ErrorDialog = true;
                    //psi.RedirectStandardError = true;
                    //psi.FileName = s;
                    //psi.Arguments = s;
                    //psi.RedirectStandardError = true;
                    
                    Process p = new Process();
                    p.StartInfo = psi;
                    //p.ErrorDataReceived += P_ErrorDataReceived;
                    //p.BeginErrorReadLine();
                    //App.Current.MainWindow.Dispatcher.Thread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    //App.Current.MainWindow.Dispatcher.Thread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
                    //Process.Start(psi.FileName, psi.Arguments);
                    try
                    {
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        //throw;
                    }
                    //string s = p.StandardError.ReadToEnd();    

                    //return;
                    found = true;
                    return true;
                    //break;
                }
            }



            //s.Replace("< ", "");
            //}


            if (found == true)
            {
                return true;
            }
            return false;
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AutoSuggestBox_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            AutoSuggestBox autoSuggestBox = sender as AutoSuggestBox;
            //if (autoSuggestBox != null)
            System.ComponentModel.ICollectionView collectionView = autoSuggestBox.ItemsSource as System.ComponentModel.ICollectionView;
            //string d = collectionView.Cast<ModernWpf.Controls.AutoSuggestBox>().ToList();
            IEnumerable<string> valueList = collectionView.Cast<string>();
            //List<string> valueList = (autoSuggestBox.ItemsSource as System.ComponentModel.ICollectionView).Cast<string>().ToList();
            */

            //AutoSuggestBox autoSuggestBox = sender as AutoSuggestBox;
            //List<string> valueList = autoSuggestBox.ItemsSource as List<string>;

            //DateTime d = lastTime.AddSeconds(5);
            //DateTime.Now > d
            //if (e.Key == Key.Enter && result == MessageBoxResult.None)
            //IntPtr intPtr = DllImports.GetWindow(GetActiveWindow(), GetWindowType.GW_ENABLEDPOPUP);
            //bool b = intPtr == IntPtr.Zero;
            //bool b = GetActiveWindow() == new WindowInteropHelper(this).Handle;
            //bool b = GetActiveWindowClass().Equals("#32770");

            if (e.Key == Key.Enter)
            {



            }

        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion == null && autoSuggestBox.Text.Trim().Length > 0)
            {
                StartAppOrCommandOrFolder();
            }

        }

        void StartAppOrCommandOrFolder()
        {


            //result = MessageBoxResult.None;
            //lastTime = DateTime.Now;

            string cmd = autoSuggestBox.Text;

            if (ExecuteCustomCommands(cmd))
                return;

            cmd = cmd.StartsWith("/") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.StartsWith("\\") ? cmd.Remove(0, 1) : cmd;

            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();


            info.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
            info.UseShellExecute = true;
            p.StartInfo = info;

            #region
            //if (((AutoSuggestBox)sender).ItemsSource.)
            /*
            if (list.Contains(autoSuggestBox.Text))
            {
                info.FileName = cmd;


                try
                {
                    //Process.Start(cmd);
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error");
                }

            }
            else
            {
            */
            #endregion

            info.Arguments = "/c " + cmd;

            //if (cmd.ToLower().Equals(cmd) || cmd.tolow)
            //{

            //}
            if (Directory.Exists(cmd))
            {
                //Process.Start("cmd", cmd);
                info.FileName = cmd;
                p.Start();
                return;
            }
            else if (File.Exists(cmd))
            {
                Process.Start(cmd);
                return;
            }
            else if (cmd.StartsWith("shell:"))
            {
                info.Arguments = "/c start " + cmd;
            }
            //else if (!cmd.Contains(" "))
            //{
            //    Process.Start(cmd);
            //}


            //bool state = false;
            /*
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = cmd;
            info.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
            */
            //info.UseShellExecute = true;

            //info.FileName = "cmd";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            //info.Arguments = "start /c" + cmd;

            //info.UseShellExecute = true;
            info.FileName = "cmd";

            p.StartInfo = info;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            try
            {
                //p = Process.Start(autoSuggestBox.Text);
                p.Start();
            }
            catch (Exception ex)
            {
                result = MessageBox.Show(ex.Message, "Couldn't start");
            }
            string s = p.StandardError.ReadToEnd();
            //if (result != 0)
            if (p.HasExited)
            {
                //string s1 = p.StandardError.ReadToEnd();
                MessageBox.Show(s, "");
            }


            //if (p == null)
            //    MessageBox.Show("Couldn't start");

            /*
             }
            */
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void AutoSuggestBoxHost_ChildChanged(object sender, EventArgs e)
        //{
        //    if (autoSuggestBoxHost.Child is Windows.UI.Xaml.Controls.AutoSuggestBox autoSuggestBox)
        //    {
        //        //autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
        //        //autoSuggestBox.TextChanged += (s, args) =>
        //        //{
        //        //    MessageBox.Show("Hi from UWP Button!");

        //        //};
        //        //autoSuggestBox.add_TextChanged(AutoSuggestBox_TextChanged);
        //        //autoSuggestBox.add_TextChanged((s, args) =>
        //        //{

        //        //});
        //    }
        //}

        //private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{

        //}
    }
}
