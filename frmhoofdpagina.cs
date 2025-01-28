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
    public partial class frmhoofdpagina : Form
    {
        public frmhoofdpagina()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            frmAdminscherm frmAdminscherm = new frmAdminscherm();
            frmAdminscherm.Show();
        }
    }
}
