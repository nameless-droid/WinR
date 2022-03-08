using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Controls;

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

        }

        private void AutoSuggestBoxHost_ChildChanged(object sender, EventArgs e)
        {
            if (autoSuggestBoxHost.Child is Windows.UI.Xaml.Controls.AutoSuggestBox autoSuggestBox)
            {
                //autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
                //autoSuggestBox.TextChanged += (s, args) =>
                //{
                //    MessageBox.Show("Hi from UWP Button!");

                //};
                //autoSuggestBox.add_TextChanged(AutoSuggestBox_TextChanged);
                //autoSuggestBox.add_TextChanged((s, args) =>
                //{

                //});
            }
        }

        //private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{
            
        //}
    }
}
