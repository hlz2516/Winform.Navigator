using Navigators;
using System;
using System.Windows.Forms;

namespace demo_router.some_page_events
{
    public partial class TestForm3 : Form
    {
        public TestForm3()
        {
            InitializeComponent();
            Router.SetContainer(panel1);
            Router.DefaultPageFirstShown += Router_DefaultPageFirstShown;
            Router.PageChanged += Router_PageChanged;
            Router.PageBeforeChanged += Router_PageBeforeChanged;
        }

        private bool Router_PageBeforeChanged(Navigators.Interfaces.IPage oldPage, Navigators.Interfaces.IPage newPage)
        {
            Console.WriteLine("Router_PageBeforeChanged");
            Console.WriteLine($"old page:{oldPage?.Path}");
            Console.WriteLine($"new page:{newPage?.Path}");
            Console.WriteLine("");
            return true;
        }

        private bool Router_PageChanged(Navigators.Interfaces.IPage oldPage, Navigators.Interfaces.IPage newPage)
        {
            Console.WriteLine("Router_PageChanged");
            Console.WriteLine($"old page:{oldPage?.Path}");
            Console.WriteLine($"new page:{newPage?.Path}");
            Console.WriteLine("");
            return true;
        }

        private void Router_DefaultPageFirstShown(object sender, EventArgs e)
        {
            Console.WriteLine("Router_DefaultPageFirstShown");
            Console.WriteLine("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Router.RouteTo("eventPage1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Router.RouteTo("eventPage2");
        }
    }
}
