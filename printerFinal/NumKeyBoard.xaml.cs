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

namespace printerFinal
{
    /// <summary>
    /// NumKeyBoard.xaml 的交互逻辑
    /// </summary>
    public partial class NumKeyBoard : Page
    {
        public TextBox tb;
        public NumKeyBoard()
        {
            InitializeComponent();
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(NumKeyBoard), new UIPropertyMetadata(false));


        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            //if (Item != null && Item.Quantity1 > 0)
            //{
            //    Item.Quantity2 = Item.Quantity1;
            //}
            tb.Text = null;
        }

        private void AddNumber(int num)
        {
            tb.Text += num.ToString();
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(2);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(3);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(4);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(5);
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(6);
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(7);
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(8);
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(9);
        }

        private void button0_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(0);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(1);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if(tb.Text.Length>0)
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
            //IsChecked = false;
        }
    }
}
