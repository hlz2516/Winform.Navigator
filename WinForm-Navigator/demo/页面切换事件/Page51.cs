using Navigator;
using Navigator.Interfaces;
using System.Windows.Forms;

namespace demo.页面切换事件
{
    public partial class Page51 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; } = Authority.USER;
        public Page51()
        {
            InitializeComponent();
            Path = "/页面切换事件/page51";
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
