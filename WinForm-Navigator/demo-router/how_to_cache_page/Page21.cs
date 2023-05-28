using Navigators;
using Navigators.Attributes;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo_router.how_to_cache_page
{
    [Route("/cache-page/page1")]
    public partial class Page21 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; }

        private int number = 0;

        public Page21()
        {
            InitializeComponent();
        }

        public void Pause()
        {

        }

        public void Restore()
        {

        }

        public void Reset()
        {

        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            number++;
            lbl_number.Text = number.ToString();
        }

        private void Page21_Shown(object sender, System.EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }
    }
}
