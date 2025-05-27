using prjIcedOutWheelz.DA;
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
            InitializeComponent(); // Initialiseer UI-componenten
            this.CenterToScreen(); // Zet het venster in het midden van het scherm
            SetTxts(); // Zet de standaardtekst in de tekstvakken
        }

        // Zet de standaardtekst en kleur in alle tekstvakken
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

        // Verwerkt het aanmaken van een nieuw account
        private void btnaccount_Click(object sender, EventArgs e)
        {
            // Controleer of het e-mailadres al bestaat
            if (!LoginDA.BestaandeGebruikenChecken(txtemail.Text))
            {
                // Controleer of alle velden zijn ingevuld en wachtwoorden overeenkomen
                if (txtwachtwoord.Text == txtherhaal.Text &&
                    txtnaam.Text != "Naam" &&
                    txtvoornaam.Text != "Voornaam" &&
                    txtemail.Text != "Email" &&
                    txttelefoon.Text != "Telefoonnummer" &&
                    txtstraatnr.Text != "Straatnaam + nummer" &
                    txtadres.Text != "Gemeente + postcode")
                {
                    Login L = new Login();

                    // Zet de gegevens uit de tekstvakken in het Login-object
                    L.Naam = txtvoornaam.Text + " " + txtnaam.Text;
                    L.Email = txtemail.Text;
                    L.Wachtwoord = txtwachtwoord.Text;
                    L.TelNummer = txttelefoon.Text;
                    L.Straat_Num = txtstraatnr.Text;
                    L.Gemeente_Postc = txtadres.Text;

                    // Voeg de gebruiker toe aan de database
                    LoginDA.GebruikerToevoegen(L);

                    // Vraag of de gebruiker direct wil inloggen
                    DialogResult dlr = MessageBox.Show("Wil je naar de inlog?", "Inloggen", MessageBoxButtons.YesNo);

                    if (dlr == DialogResult.Yes)
                    {
                        frmLogin frm = new frmLogin(); // Open loginvenster
                        this.Hide();
                        frm.Show();
                    }
                    else if (dlr == DialogResult.No)
                    {
                        SetTxts(); // Reset de tekstvakken
                    }
                }
                else
                {
                    // Toon foutmelding als niet alles is ingevuld
                    MessageBox.Show("Gelieve alle velden in te vullen!", "Incorrecte ingave");
                }
            }
            else
            {
                // Toon foutmelding als e-mail al bestaat
                MessageBox.Show("Deze email is al in gebruik!", "Email bestaat al");
            }
        }

        // Sluit het registratievenster en open het startscherm
        private void frmRegistreer_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmStartscherm frm = new frmStartscherm();
            this.Hide();
            frm.Show();
        }

        // Verwijdert de standaardtekst bij focus op het naamveld
        private void txtnaam_Enter(object sender, EventArgs e)
        {
            if(txtnaam.Text == "Naam")
            {
                txtnaam.Text = "";
                txtnaam.ForeColor = Color.Black;
            } 
        }

        // Zet de standaardtekst terug als het naamveld leeg is bij verlaten
        private void txtnaam_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtnaam.Text))
            {
                txtnaam.Text = "Naam";
                txtnaam.ForeColor = Color.Gray;
            }
        }

        // Verwijdert de standaardtekst bij focus op het voornaamveld
        private void txtvoornaam_Enter(object sender, EventArgs e)
        {
            if (txtvoornaam.Text == "Voornaam")
            {
                txtvoornaam.Text = "";
                txtvoornaam.ForeColor = Color.Black;
            }
        }

        // Zet de standaardtekst terug als het voornaamveld leeg is bij verlaten
        private void txtvoornaam_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtvoornaam.Text))
            {
                txtvoornaam.Text = "Voornaam";
                txtvoornaam.ForeColor = Color.Gray;
            }
        }

        // Verwijdert de standaardtekst bij focus op het e-mailveld
        private void txtemail_Enter(object sender, EventArgs e)
        {
            if(txtemail.Text == "Email")
            {
                txtemail.Text = "";
                txtemail.ForeColor = Color.Black;
            }
        }

        // Zet de standaardtekst terug als het e-mailveld leeg is bij verlaten
        private void txtemail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtemail.Text))
            {
                txtemail.Text = "Email";
                txtemail.ForeColor = Color.Gray;
            }
        }

        // Verwijdert de standaardtekst bij focus op het telefoonveld
        private void txttelefoon_Enter(object sender, EventArgs e)
        {
            if(txttelefoon.Text == "Telefoonnummer")
            {
                txttelefoon.Text = "";
                txttelefoon.ForeColor = Color.Black;
            }    
        }

        // Zet de standaardtekst terug als het telefoonveld leeg is bij verlaten
        private void txttelefoon_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txttelefoon.Text))
            {
                txttelefoon.Text = "Telefoonnummer";
                txttelefoon.ForeColor = Color.Gray;
            }
        }

        // Verwijdert de standaardtekst bij focus op het straat/nummer-veld
        private void txtstraatnr_Enter(object sender, EventArgs e)
        {
            if(txtstraatnr.Text == "Straatnaam + nummer")
            {
                txtstraatnr.Text = "";
                txtstraatnr.ForeColor = Color.Black;
            }
        }

        // Zet de standaardtekst terug als het straat/nummer-veld leeg is bij verlaten
        private void txtstraatnr_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtstraatnr.Text))
            {
                txtstraatnr.Text = "Straatnaam + Nummer";
                txtstraatnr.ForeColor = Color.Gray;
            }
        }

        // Verwijdert de standaardtekst bij focus op het adresveld
        private void txtadres_Enter(object sender, EventArgs e)
        {
            if(txtadres.Text == "Gemeente + postcode")
            {
                txtadres.Text = "";
                txtadres.ForeColor = Color.Black;
            }
        }

        // Zet de standaardtekst terug als het adresveld leeg is bij verlaten
        private void txtadres_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtadres.Text))
            {
                txtadres.Text = "Gemeente + Postcode";
                txtadres.ForeColor = Color.Gray;
            }
        }

        // Verwijdert de standaardtekst bij focus op het wachtwoordveld
        private void txtwachtwoord_Enter(object sender, EventArgs e)
        {
            if(txtwachtwoord.Text == "Wachtwoord")
            {
                txtwachtwoord.Text = "";
                txtwachtwoord.ForeColor = Color.Black;
            }
        }

        // Zet de standaardtekst terug als het wachtwoordveld leeg is bij verlaten
        private void txtwachtwoord_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtwachtwoord.Text))
            {
                txtwachtwoord.Text = "Wachtwoord";
                txtwachtwoord.ForeColor = Color.Gray;
            }
        }

        // Verwijdert de standaardtekst bij focus op het herhaal-wachtwoordveld
        private void txtherhaal_Enter(object sender, EventArgs e)
        {
            if (txtherhaal.Text == "Herhaal wachtwoord")
            {
                txtherhaal.Text = "";
                txtherhaal.ForeColor = Color.Black;
            }
        }

        // Zet de standaardtekst terug als het herhaal-wachtwoordveld leeg is bij verlaten
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
