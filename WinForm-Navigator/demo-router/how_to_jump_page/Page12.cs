using Navigators;
using Navigators.Attributes;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo_router.how_to_jump_page
{
    [Route("/page-jump/page2","jumpPage2")]
    public partial class Page12 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; }

        public string TestParam
        {
            set
            {
                label2.Text = value;
            }
            get
            {
                return label2.Text;
            }
        }

        public Page12()
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
