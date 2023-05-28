using Navigators;
using System.Collections.Generic;
using System.Windows.Forms;

namespace demo_router.how_to_jump_page
{
    public partial class TestForm1 : Form
    {
        public TestForm1()
        {
            InitializeComponent();
            Router.SetContainer(panel1);  //step 3:set page container
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Router.RouteTo("/page-jump/page1");  //step 5: call RouteTo to jump
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            var paramMap = new Dictionary<string, object>();
            paramMap.Add("TestParam", "It's test word");
            Router.RouteTo("jumpPage2", paramMap); //you can jump with params
        }
    }
}
