using Navigators.Interfaces;
using System;
using System.Windows.Forms;

namespace demo.页面切换事件
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            navigator1.RegisterPage<Page51>(true);
            navigator1.RegisterPage<Page52>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            navigator1.NavigateTo<Page51>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            navigator1.NavigateTo<Page52>();
        }

        private bool navigator1_PageBeforeChanged(IPage oldPage, IPage newPage)
        {
            string msg = $"oldpage:{oldPage?.Path} newpage:{newPage.Path}";
            var res = MessageBox.Show(msg + "==点击确定跳转，点击取消不做任何操作","info",MessageBoxButtons.OKCancel);
            if (res == DialogResult.OK)
            {
                return true;
            }
            return false;
        }

        private bool navigator1_PageChanged(IPage oldPage, IPage newPage)
        {
            string msg = $"oldpage:{oldPage?.Path} newpage:{newPage.Path}";
            MessageBox.Show(msg);
            return true;
        }
    }
}
