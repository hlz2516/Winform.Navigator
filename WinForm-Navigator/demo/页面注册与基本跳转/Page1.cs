using Navigators;
using Navigators.Attributes;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo
{
    //第一步，给每一个需要有导航功能的Form类继承IPage接口
    [Route("/flowchart/chart1")]
    public partial class Page1 : Form,IPage
    {
        public string Path { get; set; } = "/flowchart/chart1";
        public bool Cached { get; set; }
        public Authority Authority { get; set; } = Authority.VISITOR; //给权限赋值一个初始值

        public string BankName
        {
            set
            {
                label2.Text = value;
            }

            get { return label2.Text; }
        }

        public Page1()
        {
            InitializeComponent();
        }

        public void Pause()
        {

        }

        public void Restore()
        {

        }

        public void Reset()
        {

        }
    }
}
