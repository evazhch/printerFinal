using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using System.Configuration;

namespace printerFinal
{
    /// <summary>
    /// land.xaml 的交互逻辑
    /// </summary>
    public partial class land : Page
    {
        public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T) && parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }

        public land()
        {
            InitializeComponent();
        }



        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == ConfigurationManager.AppSettings["adminUser"] && textBox1.Text==ConfigurationManager.AppSettings["adminPass"])
            {
                SuperAdmin sa = new SuperAdmin();
                
                
                sa.Show();
            }
            else
            {
                MessageBox.Show("账户密码错误");
            }

            //PrintingPage ptpg = new PrintingPage();
            //ptpg.ShowDialog();
        }


        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {           
            NumKeyBoard nkb = new NumKeyBoard();
            nkb.tb = textBox;
            keyFram.Content = nkb;
         }

        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {

            NumKeyBoard nkb = new NumKeyBoard();
            nkb.tb = textBox1;
            keyFram.Content = nkb;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            textBox_GotFocus(sender, e);
        }
    }
}
