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
using System.Windows.Shapes;

namespace printerFinal
{
    /// <summary>
    /// lan2.xaml 的交互逻辑
    /// </summary>
    public partial class land2 : MetroWindow
    {
        public int state { get; set; }

        public land2()
        {
            this.UseNoneWindowStyle = true;
            this.IgnoreTaskbarOnMaximize = true;
            InitializeComponent();
        }

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

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NumKeyBoard1 nkb = new NumKeyBoard1();
            nkb.tb = textBox;
            keyFram.Content = nkb;   
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            textBox_GotFocus(sender, e);
        }

        private async void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "0000")
            {
                if (state == 0)
                {
                    App.user.Name = "admin";
                    App.user.PassWord = "0000";
                    App.user.role = 0;
                    Setting st = new Setting();
                    st.DataContext = App.set;
                    st.Show();
                    this.Close();
                }
                else if (state == 1)
                {
                    PrintSource st = new PrintSource();
                    st.DataContext = App.set;
                    st.Show();
                    this.Close();
                }
            }
            else
            {

                MessageDialogResult clickresult = await(this as MetroWindow).ShowMessageAsync(this.Title, "密码错误，是否退出", MessageDialogStyle.AffirmativeAndNegative);

                if (clickresult == MessageDialogResult.Negative)//取消
                {
                    this.textBox.Text = null;
                    return;
                }
                else//确认
                {
                    Start st = new Start();
                    st.Show();
                    this.Close(); //确认后的处理
                }
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
