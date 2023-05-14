using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            navigator1.SetRole(Navigator.Authority.VISITOR);
            navigator1.RegisterPage<Page1>();
            navigator1.RegisterPage<Page2>();
            navigator1.RegisterPage<Page3>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var map = new Dictionary<string, object>();
            map["BankName"] = "招商银行";
            navigator1.NavigateTo<Page1>(map);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            navigator1.NavigateTo<Page2>();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            navigator1.NavigateTo<Page3>();
        }
    }
}
