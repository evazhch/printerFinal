using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using printerFinal.Models;

namespace printerFinal
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static SettingModel set=new SettingModel();
        //
        public static PageSta psta=new PageSta();
        //
        public static User user=new User();
    }
}
