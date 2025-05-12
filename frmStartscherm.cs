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
    public partial class frmStartscherm : Form
    {
        public frmStartscherm()
        {
            InitializeComponent();
            //form midden in beeld laten verschijnen
            this.CenterToScreen();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
            this.Hide();
        }

        private void btnRegistreer_Click(object sender, EventArgs e)
        {
            frmRegistreer frmRegistreer = new frmRegistreer();
            frmRegistreer.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
