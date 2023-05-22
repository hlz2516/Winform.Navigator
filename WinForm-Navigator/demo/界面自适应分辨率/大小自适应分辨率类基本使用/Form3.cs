using AutoSizeTools;
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
            Label labe1 = new Label();
            labe1.AutoSize = true;
            labe1.Location = new System.Drawing.Point(77, 75);
            labe1.Name = "label1";
            labe1.Size = new System.Drawing.Size(41, 12);
            labe1.TabIndex = 6;
            labe1.Text = "label1";
            this.panel1.Controls.Add(labe1);
            helper.AddNewControl(labe1);
            helper.UpdateControls();
        }
    }
}
