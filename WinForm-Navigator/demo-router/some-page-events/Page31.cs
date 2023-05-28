using Navigators;
using Navigators.Attributes;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo_router.some_page_events
{
    [Route("/events/page1")]
    public partial class Page31 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; }

        public Page31()
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
