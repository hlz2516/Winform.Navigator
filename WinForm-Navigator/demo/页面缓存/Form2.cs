using System;
using System.Windows.Forms;

namespace demo.页面缓存
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Navigator.Navigator.SetRole(Navigator.Authority.USER);
            navigator1.RegisterPage<Page21>(true);
            navigator1.RegisterPage<Page22>();
            navigator1.RegisterPage<Page23>();
        }

        private void btn_page1_Click(object sender, EventArgs e)
        {
            navigator1.NavigateTo<Page21>();
        }

        private void btn_page2_Click(object sender, EventArgs e)
        {
            var page22 = navigator1.GetPage<Page22>();
            navigator1.NavigateTo(page22);
        }

        private void btn_page3_Click(object sender, EventArgs e)
        {
            navigator1.NavigateTo<Page23>();
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            navigator1.Back();
        }

        private void btn_backback_Click(object sender, EventArgs e)
        {
            navigator1.GoBackBack();
        }
    }
}
