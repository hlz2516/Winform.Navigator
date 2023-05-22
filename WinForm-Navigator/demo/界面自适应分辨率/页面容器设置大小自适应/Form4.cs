using AutoSizeTools;
using System;
using System.Windows.Forms;

namespace demo.界面自适应分辨率.页面容器设置大小自适应
{
    public partial class Form4 : Form
    {
        private AutoSizeHelper autoSizeHelper;

        public Form4()
        {
            InitializeComponent();
            autoSizeHelper = new AutoSizeHelper();
            autoSizeHelper.SetContainer(this);
            navigator1.RegisterPage<Page41>(true);
        }

        private void navigator1_DefaultPageFirstShow(object sender, EventArgs e)
        {
            //var page41 = navigator1.GetPage<Page41>();
            //autoSizeHelper.AddNewControl(page41 as Control);
            //autoSizeHelper.UpdateControls();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Page41 page = new Page41();
            page.ControlBox = false;
            page.FormBorderStyle = FormBorderStyle.None;
            page.TopLevel = false;
            page.Dock = DockStyle.Fill;
            panel1.Controls.Add(page);
            page.Show();
        }
    }
}
