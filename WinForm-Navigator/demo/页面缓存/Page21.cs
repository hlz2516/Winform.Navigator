using Navigator;
using Navigator.Interfaces;
using System;
using System.Windows.Forms;

namespace demo.页面缓存
{
    public partial class Page21 : Form,IPage
    {
        public bool Cached { get; set; }
        public Authority Authority { get; set; } = Authority.USER;

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            number++;
            lbl_number.Text = number.ToString();
        }

        private void Page21_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }
    }
}
