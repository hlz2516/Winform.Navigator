using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //第二步，把继承了IPage的窗口类向导航器注册
            navigator1.RegisterPage<Page1>();
            navigator1.RegisterPage<Page2>();  //设置为默认页面
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var map = new Dictionary<string, object>();
            map["BankName"] = "带参跳转测试";
            navigator1.NavigateTo<Page1>(map);  //带参跳转至Page1
        }

        private void button2_Click(object sender, EventArgs e)
        {
            navigator1.NavigateTo<Page2>();  //不带参跳转至Page2
        }
    }
}
