using Navigators;
using Navigators.Attributes;
using Navigators.Interfaces;
using System.Windows.Forms;

namespace demo_router.how_to_jump_page
{
    /**
     *  step 4: Inherit the IPage interface and  add the Route feature to the class which used as page.
     */
    [Route("/page-jump/page1")]
    public partial class Page11 : Form,IPage
    {
        public string Path { get; set; }
        public bool Cached { get; set; }
        public Authority Authority { get; set; }

        public Page11()
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
