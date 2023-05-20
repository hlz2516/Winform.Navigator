using AutoSizeTools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace demo.界面自适应分辨率.字体自适应
{
    public partial class Form7 : Form
    {
        private AutoSizeHelper helper;

        public Form7()
        {
            InitializeComponent();
            helper = new AutoSizeHelper();
            helper.SetContainer(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float fontSize = label1.Font.Size;
            fontSize += 1f;
            label1.Text = fontSize.ToString("F2");
            label1.Font = new Font(label1.Font.FontFamily, fontSize);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            float fontSize = label1.Font.Size;
            fontSize -= 1f;
            label1.Text = fontSize.ToString("F2");
            label1.Font = new Font(label1.Font.FontFamily, fontSize);
        }
    }
}
