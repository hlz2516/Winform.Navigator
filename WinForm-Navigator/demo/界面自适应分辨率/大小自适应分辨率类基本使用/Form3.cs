using Navigator.Common;
using System;
using System.Windows.Forms;

namespace demo
{
    public partial class Form3 : Form
    {
        AutoSizeHelper helper;

        public Form3()
        {
            InitializeComponent();
            helper = new AutoSizeHelper();
            helper.SetContainer(this);
        }

        private void Form3_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Form3_SizeChanged");
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Form3_Load");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //动态添加控件并设置分辨率自适应
            Label label = new Label();
            label.Text = "测试文字";
            label.Location = new System.Drawing.Point(579, 260);
            label.Font = new System.Drawing.Font(label.Font.FontFamily, 16);
            this.Controls.Add(label);
            helper.AddNewControl(label);
            helper.UpdateControlSize();
        }
    }
}
