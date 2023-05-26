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
            //第一步，如果想使用该包的鉴权机制，开启鉴权机制，设置程序使用者角色
            //Navigator.EnableAuthority = true;  //该属性默认false
            //Navigator.SetRole(Authority.VISITOR | Authority.USER);

            Router.LoadConfig();
            Application.Run(new Form1());
        }
    }
}
