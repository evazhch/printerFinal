using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printerFinal.BLL
{
    class messgeBoxBll
    {
        public static List<NotificationWindow> _dialogs = new List<NotificationWindow>();
        static int i = 0;

        public static void Show(string tile,string msg)
        {
            i++;
            
            NotificationWindow dialog = new NotificationWindow();//new 一个通知
            dialog.TopFrom = GetTopFrom();
            dialog.Tile.Text = tile;
            dialog.msg.Text= msg;
            _dialogs.Add(dialog);
            dialog.Show();
        }

        public static double  GetTopFrom()
        {
            //屏幕的高度-底部TaskBar的高度。
            double topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;
            bool isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);

            while (isContinueFind)
            {
                topFrom = topFrom - 210;//此处100是NotifyWindow的高 110-100剩下的10  是通知之间的间距
                isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);
            }

            if (topFrom <= 0)
                topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;

            return topFrom;
        }
    }
}
