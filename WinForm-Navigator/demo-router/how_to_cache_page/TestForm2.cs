using Navigators;
using System.Windows.Forms;

namespace demo_router.how_to_cache_page
{
    public partial class TestForm2 : Form
    {
        public TestForm2()
        {
            InitializeComponent();
            Router.SetContainer(panel1);
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Router.RouteTo("cachePage1");
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            Router.RouteTo("cachePage2");
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            Router.RouteTo("cachePage3");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Router.Back();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Router.GoBackBack();
        }
    }
}
