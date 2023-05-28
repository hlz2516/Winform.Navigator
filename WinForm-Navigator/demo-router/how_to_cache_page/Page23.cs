using Navigators;
using Navigators.Attributes;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo_router.how_to_cache_page
{
    [Route("/cache-page/page3")]
    public partial class Page23 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; }

        private int number = 0;

        public Page23()
        {
            InitializeComponent();
        }

        public void Pause()
        {
            timer1.Stop();
        }

        public void Restore()
        {

        }

        public void Reset()
        {
            number = 0;
            lbl_number.Text = "0";
            timer1.Start();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            number++;
            lbl_number.Text = number.ToString();
        }

        private void Page23_Shown(object sender, System.EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }
    }
}
