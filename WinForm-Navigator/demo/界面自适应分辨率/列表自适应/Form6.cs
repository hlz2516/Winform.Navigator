using AutoSizeTools;
using System;
using System.Windows.Forms;

namespace demo.界面自适应分辨率.列表自适应
{
    public partial class Form6 : Form
    {
        private AutoSizeHelper formHelper;
        private AutoSizeHelper listHelper;

        public Form6()
        {
            InitializeComponent();
            formHelper = new AutoSizeHelper();
            formHelper.SetContainer(this);
            listHelper = new AutoSizeHelper();
            listHelper.SetContainer(listView1);
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            for (int i = 0; i < 10; i++)
            {
                var row = new ListViewItem();
                row.Text = i.ToString();
                row.SubItems.Add("张三");
                row.SubItems.Add("我是张三");
                row.SubItems.Add("男");
                listView1.Items.Add(row);
            }
            listView1.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //listView1.Columns[0].Width = 200;
        }
    }
}
