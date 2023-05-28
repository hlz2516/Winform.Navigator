using demo_router.how_to_cache_page;
using demo_router.how_to_jump_page;
using demo_router.some_page_events;
using Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo_router
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
            Router.LoadConfig();  //step 2:load the config from step1
            Application.Run(new TestForm3());
        }
    }
}
