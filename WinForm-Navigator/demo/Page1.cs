using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Navigator;
using Navigator.Interfaces;

namespace demo
{
    public partial class Page1 : Form,IPage
    {
        public bool Cached { get; set; }
        public Authority Authority { get; set; } = Authority.VISITOR;

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
