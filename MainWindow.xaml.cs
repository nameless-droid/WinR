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
using System.Windows.Threading;
using System.Security.Principal;

namespace WinR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool highlightInExplorer = false;
        //List<string> list = new List<string>();
        List<ItemsWithTooltip> list = new List<ItemsWithTooltip>();
        string shellCmdFile = "shell.txt";

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
            //List<string> names = new List<string>();
            List<ItemsWithTooltip> names = new List<ItemsWithTooltip>();

            //System.Windows.Controls.ListView listView = new System.Windows.Controls.ListView();

            foreach (var item in s1)
            {
                foreach (string s in Directory.GetFileSystemEntries(item))
                {
                    //names.Add("/" + s);
                    //names.Add(new ItemsWithTooltip { Name= "/" + s, ToolTip = "Folder (Opens in File Explorer)" });
                    names.Add(new ItemsWithTooltip { Name = "/" + s });
                    //listView.Items.Add(s);
                    //list.Add(s);
                }
            }

            foreach (string s in Directory.GetDirectories(systemPath))
            {
                //names.Add("\\" + s);
                names.Add(new ItemsWithTooltip { Name = "/" + s });
                //listView.Items.Add(s);
                //list.Add(s);
            }

            list.AddRange(names);

            ////list.AddRange(AddShellCommandsToList())
            //list.AddRange(GetShellCommands());


            list.AddRange(GetShellCommandsWithTooltip());

            //AddShellCmdTo(listView);

            //names.Add("WPF rocks");
            //names.Add("WCF rocks");
            //names.Add("XAML is fun");
            //names.Add("WPF rules");
            //names.Add("WCF rules");
            //names.Add("WinForms not");


            autoSuggestBox.Items.Clear();


            autoSuggestBox.IsTextSearchEnabled = false;
            //addtoSuggestBox.ItemsSource = names;
            //autoSuggestBox.ItemsSource = names;
            //autoSuggestBox.Items.Add(listView, false); //= (System.Collections.IEnumerable)listView;

            //autoSuggestBox.ItemsSource = this;

            //autoSuggestBox.ToolTip = "dd";

            WinKeyboardHook.Sub();
            WinKeyboardHook.SetDefaultPosOnCurrentScreen();
        }

        private void AddShellCmdTo(System.Windows.Controls.ListView listView)
        {
            //throw new NotImplementedException();

            string[] lines = System.IO.File.ReadAllLines(shellCmdFile);

            for (int i = 0; i < lines.Length / 2; i += 2)
            {
                System.Windows.Controls.ListViewItem item = new System.Windows.Controls.ListViewItem();
                item.Content = lines[i];
                item.ToolTip = lines[i + 1];
            }
        }

        private IEnumerable<ItemsWithTooltip> GetShellCommandsWithTooltip()
        {
            List<ItemsWithTooltip> list = new List<ItemsWithTooltip>();

            string[] lines = System.IO.File.ReadAllLines(shellCmdFile);

            for (int i = 0; i < lines.Length / 2; i += 2)
            {
                string d = lines[i];
                //if (string.IsNullOrEmpty(d))
                d = d.StartsWith("“") ? d.Remove(0, 1) : d;
                d = d.EndsWith("”") ? d.Remove(d.Length - 1, 1) : d;
                d = d.StartsWith("\"") ? d.Remove(0, 1) : d;
                d = d.EndsWith("\"") ? d.Remove(d.Length - 1, 1) : d;
                list.Add(new ItemsWithTooltip { Name = d, ToolTip = lines[i + 1] });
            }

            return list;
        }

        private IEnumerable<string> GetShellCommands()
        {
            //throw new NotImplementedException();
            List<string> list = new List<string>();

            //using (var sw = File.OpenRead(shellCmdFile))
            //{
            //    foreach (var item in sw)
            //    {

            //    }
            //}

            string[] lines = System.IO.File.ReadAllLines(shellCmdFile);

            // Display the file contents by using a foreach loop.
            //System.Console.WriteLine("Contents of WriteLines2.txt = ");
            //foreach (string line in lines)
            //{
            //    // Use a tab to indent each line of the file.
            //    //Console.WriteLine("\t" + line);
            //}

            for (int i = 0; i < lines.Length / 2; i += 2)
            {
                list.Add(lines[i] + ";" + lines[i + 1]);
            }

            return list;
        }

        internal static void SetPositionToCurrentWindowOrDefaultPosOnWnd()
        {

        }

        //public void Tooltip(AutoSuggestBox box)
        //{
        //    for (int d = 0; d < box.Items.Count; d++)
        //    {
        //        box.Items[d].Attributes.Add("title", box.Items[d].Text);
        //    }
        //    ModernWpf.Controls.AutoSuggestBox
        //    foreach (AutoSuggestBox item in drpID.Items)
        //    {
        //        item.Attributes.Add("Title", item.Text);
        //    }
        //}

        //public void Tooltip(ItemsControl control)
        //{
        //    for (int d = 0; d < control.Items.Count; d++)
        //    {
        //        control.Items[d].Attributes.Add("title", control.Items[d].Text);
        //    }
        //    ModernWpf.Controls.AutoSuggestBox
        //    foreach (AutoSuggestBox item in drpID.Items)
        //    {
        //        item.Attributes.Add("Title", item.Text);
        //    }
        //}

        //public void Tooltip(ModernWpf.Controls.Primitives.AutoSuggestBoxListViewItem lc)
        //{
        //    for (int d = 0; d < lc.Items.Count; d++)
        //    {
        //        lc.Items[d].Attributes.Add("title", lc.Items[d].Text);
        //    }
        //}

        private void AutoSuggestBox_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            //AutoSuggestBox autoSuggestBox = sender as AutoSuggestBox;
            //okButton.IsEnabled = autoSuggestBox.Text.Length > 0;
            okButton.IsEnabled = autoSuggestBox.Text.Trim().Length > 0;

            //object d = autoSuggestBox.Items[0] as ModernWpf.Controls.ListViewItem;

            //ModernWpf.Controls.Primitives.AutoSuggestBoxListViewItem autoSuggestBoxListViewItem = (ModernWpf.Controls.Primitives.AutoSuggestBoxListViewItem)sender.Items[0];
            //autoSuggestBoxListViewItem.ToolTip = "test";

            #region
            //var suitableItems = new List<string>();
            //var splitText = sender.Text.ToLower().Split(" ");
            //foreach (var item in list)
            //{
            //    var found = splitText.All((key) =>
            //    {
            //        return item.ToLower().Contains(key);
            //    });
            //    if (found)
            //    {
            //        suitableItems.Add(item);
            //    }
            //}
            //if (suitableItems.Count == 0)
            //{
            //    suitableItems.Add("No results found");
            //}
            //sender.ItemsSource = suitableItems;
            #endregion



            var suitableItems = new List<ItemsWithTooltip>();

            if (sender.Text.Length == 0)
            {
                sender.ItemsSource = suitableItems;
                return;
            }

            //if (sender.Text.Equals(list.All))

            if (sender.Text.Contains("shell:"))
            {
                //sender.IsTextSearchEnabled = false;
                //Debug.Write(sender.IsTextSearchCaseSensitive);
                //return;
                //var found1 = false;

                //foreach (var item in list)
                //{
                //    //found1 = sender.Text.All((key) =>
                //    //{
                //    //    return item.ToLower().Contains(key);
                //    //});
                //    //if (item.Equals(sender.Text))
                //    //if (item.ToString().Equals(sender.Text))
                //    //if (item.ToString().Equals("“" + sender.Text + "”"))
                //    if (item.ToString().Equals(sender.Text))
                //    {
                //        found1 = true;
                //        break;
                //    }
                //}

                ////if (sender.Text.Equals(list.All))
                //if (found1)
                //{
                //    return;
                //}

            }

            /*
            
            if (sender.Text.Contains("shell:"))
            {

                var text = "shell:";
                foreach (var item in list)
                {
                    //var found = text.All((key) =>
                    //{
                    //    return item.ToLower().Contains(key);
                    //});
                    //if (found)
                    //{
                    //    suitableItems.Add(item);
                    //    //suitableItems.Add(item.ToLower());
                    //    //suitableItems.Add(new CountryInfo { Name = item});
                    //    //sender.Items[0].GetType();
                    //}
                    if (item.ToString().Contains(text))
                    {
                        suitableItems.Add(item);
                    }
                }
                sender.ItemsSource = suitableItems;
                return;
            }
            */





            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var item in list)
            {
                var found = false;
                if (splitText[0].Equals("shell:"))
                {
                    if (item.Name.Contains("shell"))
                    {
                        ;
                    }

                    //if (splitText[0].Contains(item.Name))
                    if (item.Name.Contains(splitText[0]))
                    {
                        found = true;
                    }
                }
                else if (!splitText[0].Contains("shell:"))
                {
                    found = splitText.All((key) =>
                    {
                        return item.ToLower().Contains(key);
                    });
                    //var found = splitText.Any((key) =>
                    //{
                    //    return item.ToLower().Contains(key);
                    //});
                }
                else
                {
                    return;
                }

                if (found)
                {
                    suitableItems.Add(item);
                    //suitableItems.Add(item.ToLower());
                    //suitableItems.Add(new CountryInfo { Name = item});
                    //sender.Items[0].GetType();
                }
            }
            if (suitableItems.Count == 0)
            {
                //suitableItems.Add("No results found");
                suitableItems.Add(new ItemsWithTooltip { Name = "No results found" });

            }
            sender.ItemsSource = suitableItems;

        }

        private void AutoSuggestBox_SuggestionChosen(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //sender.ItemsSource = new List<ItemsWithTooltip>();
            //sender.IsSuggestionListOpen = true;

            if (sender.ItemsSource != null)
            {
                //sender.IsSuggestionListOpen = true;
                //sender.ToolTip = sender.Items[sender.Items.CurrentPosition];
                var d = sender.Items.CurrentItem;
                sender.Items.CurrentChanged += Items_CurrentChanged;
                //sender.Items.
                //sender.ToolTip = sender.Items[0];
                //ModernWpf.Controls.Primitives.AutoSuggestBoxListViewItem item = (ModernWpf.Controls.Primitives.AutoSuggestBoxListViewItem)args.SelectedItem;
                ItemsWithTooltip item = (ItemsWithTooltip)args.SelectedItem;

                //if (item != null)
                //{

                //}

                sender.ToolTip = item.ToolTip;
                Debug.WriteLine("dd" + sender.ToolTip + sender.Items.CurrentPosition + d);
            }
        }

        private void Items_CurrentChanged(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        //DateTime lastTime = DateTime.Now;
        MessageBoxResult result;

        private bool ExecuteCustomCommands(string cmd, bool asAdmin)
        {

            //((ComboBox)sender).IsDropDownOpen = true;   
            //return;
            string userDir = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
            //string s = ((FilteredComboBox)sender).Text;
            //if (string.IsNullOrEmpty(s))


            //var fileName = "command-history.txt";
            //if (!File.Exists(fileName))
            //{
            //    File.Create(fileName);
            //}

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

            string[] lines;

            try
            {
                if (!File.Exists("custom.txt"))
                {
                    File.Create("custom.txt");
                }

                lines = System.IO.File.ReadAllLines(@"custom.txt");
            }
            catch (Exception)
            {
                throw;
            }


            foreach (var item in lines)
            {
                if (item.StartsWith("//"))
                {
                    continue;
                }

                string[] str = item.Split(";");
                //if (str[0].Equals(s))
                //s = str[1];
                if (cmd.Equals(str[0]))
                {
                    ExecuteCommand(str[1], asAdmin, customCmd: true);

                    #region
                    /*
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
                    */
                    #endregion

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
                //StartAppOrCommandOrFolder();

                //ConsoleModifiers consoleModifiers = Keyboard.Modifiers;
                //System.Windows.Input.Keyboard modifiers = Keyboard.Modifiers;
                var modifiers = Keyboard.Modifiers;
                bool ctrl = modifiers.ToString().Contains("Control");
                StartAppOrCommandOrFolder(modifiers.ToString().Contains("Control"));
            }

        }

        public static bool RunningAsAdmin =>
   new WindowsPrincipal(WindowsIdentity.GetCurrent())
       .IsInRole(WindowsBuiltInRole.Administrator);

        //void ExecuteCmdOrStartAppOrFolder()
        /// <summary>
        /// Starts an application, Opens File Explorer Folder, Execute a command like cmd or taskkill /f /im winr.exe
        /// </summary>
        async void ExecuteCommand(string cmd, bool asAdmin, bool customCmd = false)
        {
            var fileName = "command-history.txt";
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }

            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(cmd);
            }

            cmd = RemoveCharactersFromCmd(cmd);

            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users", Environment.UserName);
            //info.UseShellExecute = true;
            //info.Arguments = "/c /a" + cmd;
            info.Arguments = "/c" + cmd;
            //info.Arguments = "/k " + cmd;

            //info.CreateNoWindow = true;
            //info.WindowStyle = ProcessWindowStyle.Hidden;



            //ExecuteAsAdmin(cmd);

            //return;

            info.FileName = "cmd";

            if (Directory.Exists(cmd) || File.Exists(cmd))
            {
                Process.Start("explorer", highlightInExplorer ? "/select," + cmd : cmd);
                return;
            }
            else if (cmd.StartsWith("shell:"))
            {
                Process.Start("explorer", highlightInExplorer ? "/select," + cmd : cmd);
                return;
                //info.Arguments = "/c start " + cmd;
            }
            else if (!cmd.Contains(" ") && customCmd == false)
            {
                info.FileName = cmd;
                info.Arguments = "";
                info.CreateNoWindow = false;
                //info.WorkingDirectory = @"C:\Windows\System32";
                info.WindowStyle = ProcessWindowStyle.Normal;
            }

            info.RedirectStandardError = true;

            p.StartInfo = info;


            if (!RunningAsAdmin && asAdmin)
                info.RedirectStandardError = false;


            if (asAdmin)
            {
                info.Verb = "runas";
                info.UseShellExecute = true;
            }



            //info.UseShellExecute = false;

            try
            {
                p.Start();
            }
            catch (Exception ex)
            {
                result = MessageBox.Show(ex.Message, "Couldn't start");
            }

            //await Task.Delay(500);
            await Task.Delay(1000);
            //await Task.Delay(10000);

            try
            {
                if (p.HasExited)
                {
                    string s = p.StandardError.ReadToEnd();
                    MessageBox.Show(s, "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error1");
                //throw;
            }

        }

        public void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        private static string RemoveCharactersFromCmd(string cmd)
        {
            cmd = cmd.StartsWith("/") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.StartsWith("\\") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.StartsWith("\"") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.EndsWith("\"") ? cmd.Remove(cmd.Length - 1, 1) : cmd;

            cmd = cmd.StartsWith("“") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.EndsWith("”") ? cmd.Remove(cmd.Length - 1, 1) : cmd;
            return cmd;
        }

        void StartAppOrCommandOrFolder(bool asAdmin)
        {


            //result = MessageBoxResult.None;
            //lastTime = DateTime.Now;

            string cmd = autoSuggestBox.Text;

            if (ExecuteCustomCommands(cmd, asAdmin))
                return;

            cmd = cmd.StartsWith("/") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.StartsWith("\\") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.StartsWith("\"") ? cmd.Remove(0, 1) : cmd;
            cmd = cmd.EndsWith("\"") ? cmd.Remove(cmd.Length - 1, 1) : cmd;

            ExecuteCommand(cmd, asAdmin);

            return;

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
            if (Directory.Exists(cmd) || File.Exists(cmd))
            {
                //Process.Start("cmd", cmd);
                //info.FileName = cmd;
                //p.Start();
                Process.Start("explorer", highlightInExplorer ? "/select," + cmd : cmd);
                return;
            }
            //else if (File.Exists(cmd))
            //{
            //    Process.Start(cmd);
            //    return;
            //}
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
            //string s = p.StandardError.ReadToEnd();
            //if (result != 0)
            if (p.HasExited)
            {
                string s = p.StandardError.ReadToEnd();
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
            StartAppOrCommandOrFolder(Keyboard.Modifiers.ToString().Contains("Control"));
        }

        private void HighlightCheckbox_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox? checkBox = (sender as CheckBox);
            highlightInExplorer = (bool)checkBox.IsChecked;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //this.Hide(); #todo enable
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true; //todo disable
            autoSuggestBox.Focus();
            Keyboard.Focus(autoSuggestBox as AutoSuggestBox);

            Dispatcher.BeginInvoke(DispatcherPriority.Input,
            new Action(delegate ()
            {
                autoSuggestBox.Focus();         // Set Logical Focus
                Keyboard.Focus(autoSuggestBox); // Set Keyboard Focus
            }));
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