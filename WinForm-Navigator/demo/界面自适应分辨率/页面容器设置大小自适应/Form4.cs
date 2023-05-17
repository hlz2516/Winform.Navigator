using Navigator.Common;
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
            var page41 = navigator1.GetPage<Page41>();
            autoSizeHelper.AddNewControl(page41 as Control);
            autoSizeHelper.UpdateControlSize();
        }
    }
}
