﻿using prjIcedOutWheelz.DA;
using prjIcedOutWheelz.Model;
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
            SetTxts();
        }

        private void SetTxts()
        {
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

        private void btnaccount_Click(object sender, EventArgs e)
        {
            if (txtwachtwoord.Text == txtherhaal.Text)
            {
                Login L = new Login();

                L.Naam = txtnaam.Text;
                L.Email = txtemail.Text;
                L.Wachtwoord = txtwachtwoord.Text;
                L.TelNummer = txttelefoon.Text;
                L.Straat_Num = txtstraatnr.Text;
                L.Gemeente_Postc = txtadres.Text;

                LoginDA.GebruikerToevoegen(L);

                DialogResult dlr = MessageBox.Show("Wil je naar de inlog?", "Inloggen", MessageBoxButtons.YesNo);

                if(dlr == DialogResult.Yes)
                {
                    frmLogin frm = new frmLogin();
                    this.Hide();
                    frm.Show();
                }
                else if(dlr == DialogResult.No)
                {
                    SetTxts();
                }
            }
            else
            {
                MessageBox.Show("Wachtwoorden komen niet overeen!", "Incorrecte wachtwoorden");
            }
            

        }

        private void frmRegistreer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void txtnaam_Enter(object sender, EventArgs e)
        {
            if(txtnaam.Text == "Naam")
            {
                txtnaam.Text = "";
                txtnaam.ForeColor = Color.Black;
            } 
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
            if (txtvoornaam.Text == "Voornaam")
            {
                txtvoornaam.Text = "";
                txtvoornaam.ForeColor = Color.Black;
            }
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
            if(txtemail.Text == "Email")
            {
                txtemail.Text = "";
                txtemail.ForeColor = Color.Black;
            }
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
            if(txttelefoon.Text == "Telefoonnummer")
            {
                txttelefoon.Text = "";
                txttelefoon.ForeColor = Color.Black;
            }    
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
            if(txtstraatnr.Text == "Straatnaam + nummer")
            {
                txtstraatnr.Text = "";
                txtstraatnr.ForeColor = Color.Black;
            }
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
            if(txtadres.Text == "Gemeente + postcode")
            {
                txtadres.Text = "";
                txtadres.ForeColor = Color.Black;
            }
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
            if(txtwachtwoord.Text == "Wachtwoord")
            {
                txtwachtwoord.Text = "";
                txtwachtwoord.ForeColor = Color.Black;
            }
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
            if (txtherhaal.Text == "Herhaal wachtwoord")
            {
                txtherhaal.Text = "";
                txtherhaal.ForeColor = Color.Black;
            }
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
