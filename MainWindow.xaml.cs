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

using Path = System.IO.Path;

namespace WinR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> list = new List<string>();

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
        }

        private void AutoSuggestBox_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void AutoSuggestBox_SuggestionChosen(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ;
        }

        private void AutoSuggestBox_KeyDown(object sender, KeyEventArgs e)
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


            if (e.Key == Key.Enter)
            {
                //if (((AutoSuggestBox)sender).ItemsSource.)
                if (list.Contains(autoSuggestBox.Text))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(autoSuggestBox.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error");
                    }

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
