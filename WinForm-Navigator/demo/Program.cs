using demo.界面自适应分辨率.页面容器设置大小自适应;
using demo.页面缓存;
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
            Navigator.Navigator.SetRole(Navigator.Authority.VISITOR | Navigator.Authority.USER);
            Application.Run(new Form4());
        }
    }
}
