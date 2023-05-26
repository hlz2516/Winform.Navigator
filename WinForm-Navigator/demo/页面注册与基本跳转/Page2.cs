using Navigators;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo
{
    public partial class Page2 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; } = Authority.VISITOR;

        public Page2()
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
