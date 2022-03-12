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

        }

        private void AutoSuggestBox_SuggestionChosen(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ;
        }

        //DateTime lastTime = DateTime.Now;
        MessageBoxResult result;
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
                result = MessageBoxResult.None;
                //lastTime = DateTime.Now;

                //if (((AutoSuggestBox)sender).ItemsSource.)
                if (list.Contains(autoSuggestBox.Text))
                {
                    try
                    {
                        Process.Start(autoSuggestBox.Text);
                    }
                    catch (Exception)
                    {
                         MessageBox.Show("Error");
                    }

                }
                else
                {
                    //bool state = false;
                    Process p = null;
                    try
                    {
                        p = Process.Start(autoSuggestBox.Text);
                    }
                    catch (Exception)
                    {
                        result = MessageBox.Show("Couldn't start");
                    }

                    //if (p == null)
                    //    MessageBox.Show("Couldn't start");
                }


            }

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
