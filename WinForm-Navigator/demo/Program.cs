using demo.界面自适应分辨率.列表自适应;
using demo.界面自适应分辨率.页面容器设置大小自适应;
using demo.页面切换事件;
using demo.页面缓存;
using Navigators;
using System;
using System.Windows.Forms;

namespace demo
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //第一步，设置程序使用者角色
            Navigator.SetRole(Authority.VISITOR | Authority.USER);
            //如果不想使用该包的鉴权机制，可以设置取消鉴权，设置后页面跳转时不再鉴权
            //Navigator.EnableAuthority = false;
            Application.Run(new Form6());
        }
    }
}
