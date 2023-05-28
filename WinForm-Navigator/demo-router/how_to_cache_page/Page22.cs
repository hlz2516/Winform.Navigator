using Navigators;
using Navigators.Attributes;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo_router.how_to_cache_page
{
    [Route("/cache-page/page2")]
    public partial class Page22 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; }

        private int number = 0;

        public Page22()
        {
            InitializeComponent();
        }

        public void Pause()
        {
            timer1.Stop();
        }

        public void Restore()
        {
            timer1.Start();
        }

        public void Reset()
        {

        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            number++;
            lbl_number.Text = number.ToString();
        }

        private void Page22_Shown(object sender, System.EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }
    }
}
