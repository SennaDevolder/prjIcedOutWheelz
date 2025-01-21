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

            txtnaam.Text = "Naam";
            txtnaam.ForeColor = Color.Gray;
            txtvoornaam.Text = "Voornaam";
            txtvoornaam.ForeColor = Color.Gray;
            txtemail.Text = "Email";
            txtemail.ForeColor = Color.Gray;
            txttelefoon.Text = "Email";
            txttelefoon.ForeColor = Color.Gray;
            txtstraatnr.Text = "Wachtwoord";
            txtstraatnr.ForeColor = Color.Gray;
            txtadres.Text = "Email";
            txtadres.ForeColor = Color.Gray;
            txtwachtwoord.Text = "Wachtwoord";
            txtwachtwoord.ForeColor = Color.Gray; 
            txtherhaal.Text = "Wachtwoord";
            txtherhaal.ForeColor = Color.Gray;
        }

        private void frmRegistreer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
