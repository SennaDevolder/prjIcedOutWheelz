using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjIcedOutWheelz
{
    public partial class frmRegistreer : Form
    {
        public frmRegistreer()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void frmRegistreer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
