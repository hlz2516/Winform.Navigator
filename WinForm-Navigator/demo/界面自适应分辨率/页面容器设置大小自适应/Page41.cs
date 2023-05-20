using AutoSizeTools;
using Navigator;
using Navigator.Interfaces;
using System.Windows.Forms;

namespace demo.界面自适应分辨率.页面容器设置大小自适应
{
    //第一步，给每一个需要有导航功能的Form类继承IPage接口
    public partial class Page41 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; } = Authority.VISITOR; //给权限赋值一个初始值

        private AutoSizeHelper autoSizeHelper;

        public Page41()
        {
            InitializeComponent();
            autoSizeHelper = new AutoSizeHelper();
            autoSizeHelper.SetContainer(this);
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
