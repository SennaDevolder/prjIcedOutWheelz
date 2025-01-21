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
            txttelefoon.Text = "Telefoonnummer";
            txttelefoon.ForeColor = Color.Gray;
            txtstraatnr.Text = "Straatnaam + nummer";
            txtstraatnr.ForeColor = Color.Gray;
            txtadres.Text = "Gemeente + postcode";
            txtadres.ForeColor = Color.Gray;
            txtwachtwoord.Text = "Wachtwoord";
            txtwachtwoord.ForeColor = Color.Gray; 
            txtherhaal.Text = "Herhaal wachtwoord";
            txtherhaal.ForeColor = Color.Gray;
        }

        private void frmRegistreer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void txtnaam_Enter(object sender, EventArgs e)
        {
            txtnaam.Text = "";
            txtnaam.ForeColor = Color.Black;
        }

        private void txtnaam_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtnaam.Text))
            {
                txtnaam.Text = "Naam";
                txtnaam.ForeColor = Color.Gray;
            }
        }

        private void txtvoornaam_Enter(object sender, EventArgs e)
        {
            txtvoornaam.Text = "";
            txtvoornaam.ForeColor = Color.Black;
        }

        private void txtvoornaam_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtvoornaam.Text))
            {
                txtvoornaam.Text = "Voornaam";
                txtvoornaam.ForeColor = Color.Gray;
            }
        }

        private void txtemail_Enter(object sender, EventArgs e)
        {
            txtemail.Text = "";
            txtemail.ForeColor = Color.Black;
        }

        private void txtemail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtemail.Text))
            {
                txtemail.Text = "Email";
                txtemail.ForeColor = Color.Gray;
            }
        }

        private void txttelefoon_Enter(object sender, EventArgs e)
        {
            txttelefoon.Text = "";
            txttelefoon.ForeColor = Color.Black;
        }

        private void txttelefoon_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txttelefoon.Text))
            {
                txttelefoon.Text = "Telefoonnummer";
                txttelefoon.ForeColor = Color.Gray;
            }
        }

        private void txtstraatnr_Enter(object sender, EventArgs e)
        {
            txtstraatnr.Text = "";
            txtstraatnr.ForeColor = Color.Black;
        }

        private void txtstraatnr_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtstraatnr.Text))
            {
                txtstraatnr.Text = "Straatnaam + Nummer";
                txtstraatnr.ForeColor = Color.Gray;
            }
        }

        private void txtadres_Enter(object sender, EventArgs e)
        {
            txtadres.Text = "";
            txtadres.ForeColor = Color.Black;
        }

        private void txtadres_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtadres.Text))
            {
                txtadres.Text = "Gemeente + Postcode";
                txtadres.ForeColor = Color.Gray;
            }
        }

        private void txtwachtwoord_Enter(object sender, EventArgs e)
        {
            txtwachtwoord.Text = "";
            txtwachtwoord.ForeColor = Color.Black;
        }

        private void txtwachtwoord_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtwachtwoord.Text))
            {
                txtwachtwoord.Text = "Wachtwoord";
                txtwachtwoord.ForeColor = Color.Gray;
            }
        }

        private void txtherhaal_Enter(object sender, EventArgs e)
        {
            txtherhaal.Text = "";
            txtherhaal.ForeColor = Color.Black;
        }

        private void txtherhaal_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtherhaal.Text))
            {
                txtherhaal.Text = "Herhaal Wachtwoord";
                txtherhaal.ForeColor = Color.Gray;
            }
        }
    }
}
