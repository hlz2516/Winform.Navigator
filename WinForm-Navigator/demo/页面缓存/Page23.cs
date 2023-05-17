using Navigator;
using Navigator.Interfaces;
using System.Windows.Forms;

namespace demo.页面缓存
{
    public partial class Page23 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; } = Authority.USER;

        private int number = 0;

        public Page23()
        {
            InitializeComponent();
        }

        public void Pause()
        {
            timer1.Stop();
            number = 0;
            lbl_number.Text = "0";
        }

        public void Restore()
        {

        }

        public void Reset()
        {
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
