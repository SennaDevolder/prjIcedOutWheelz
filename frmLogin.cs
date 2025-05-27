using Microsoft.VisualBasic;
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
using System.Xml.Linq;

namespace prjIcedOutWheelz
{
    public partial class frmLogin : Form
    {
        // Statisch login-object voor de huidige gebruiker
        public static Login log = new Login();

        public frmLogin()
        {
            InitializeComponent(); // Initialiseer UI-componenten

            this.CenterToScreen(); // Zet het venster in het midden

            // Zet standaardtekst en kleur voor e-mailveld
            txtEmail.Text = "Email";
            txtEmail.ForeColor = Color.Gray;
            // Zet standaardtekst en kleur voor wachtwoordveld
            txtWachtwoord.Text = "Wachtwoord";
            txtWachtwoord.ForeColor = Color.Gray;
        }

        // Login-knop: start loginproces met 2FA
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Controleer of de velden niet op de standaardwaarde staan
            if(txtEmail.Text != "Email" && txtWachtwoord.Text != "Wachtwoord")
            {
                string str2FAcodeCHECKER, str2FAcode;
                DateTime codeGenerationTime = DateTime.Now; // Tijdstip code-aanmaak
                TimeSpan codeValidityDuration = TimeSpan.FromMinutes(5); // Geldigheid 2FA
                DialogResult dl = new DialogResult();

                // Zet gebruikersgegevens
                log.Email = txtEmail.Text;
                log.Wachtwoord = txtWachtwoord.Text;

                // Genereer eerste 2FA-code
                str2FAcode = LoginDA.Genereer2FAcode();

                // Stel e-mailbody samen voor 2FA
                string strEmailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            background-color: #ffffff;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }}
        .email-header {{
            background-color: #007bff;
            color: #ffffff;
            padding: 10px;
            text-align: center;
            border-radius: 8px 8px 0 0;
        }}
        .email-content {{
            padding: 20px;
            text-align: center;
        }}
        .email-footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #6c757d;
            text-align: center;
        }}
        .token {{
            font-size: 24px;
            font-weight: bold;
            color: #007bff;
        }}
        a.button {{
            display: inline-block;
            padding: 10px 20px;
            margin: 10px 0;
            background-color: #007bff;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
        }}
        a.button:hover {{
            background-color: #0056b3;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='email-header'>
            <h1>Twee-Factor Authenticatie Code</h1>
        </div>
        <div class='email-content'>
            <p>Beste gebruiker,</p>
            <p>We hebben een verzoek ontvangen om toegang te krijgen tot uw account. Gebruik de onderstaande code om uw login te voltooien:</p>
            <p class='token'>{str2FAcode}</p>
            <p>Deze code is 5 minuten geldig. Als u dit verzoek niet heeft gedaan, neem dan onmiddellijk contact op met onze supportafdeling.</p>
        </div>
        <div class='email-footer'>
            <p>Bedankt om voor IcedOutWheelz te kiezen!</p>
            <p>&copy; 2025 IcedOutWheelz. Alle rechten voorbehouden.</p>
        </div>
    </div>
</body>
</html>
";

                // Controleer login-gegevens
                if (LoginDA.LoginValidation(log) == 1)
                {
                    do
                    { 
                        // Genereer nieuwe 2FA-code en e-mailbody
                        str2FAcode = LoginDA.Genereer2FAcode();
                        strEmailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            background-color: #ffffff;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }}
        .email-header {{
            background-color: #007bff;
            color: #ffffff;
            padding: 10px;
            text-align: center;
            border-radius: 8px 8px 0 0;
        }}
        .email-content {{
            padding: 20px;
            text-align: center;
        }}
        .email-footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #6c757d;
            text-align: center;
        }}
        .token {{
            font-size: 24px;
            font-weight: bold;
            color: #007bff;
        }}
        a.button {{
            display: inline-block;
            padding: 10px 20px;
            margin: 10px 0;
            background-color: #007bff;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
        }}
        a.button:hover {{
            background-color: #0056b3;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='email-header'>
            <h1>Twee-Factor Authenticatie Code</h1>
        </div>
        <div class='email-content'>
            <p>Beste gebruiker,</p>
            <p>We hebben een verzoek ontvangen om toegang te krijgen tot uw account. Gebruik de onderstaande code om uw login te voltooien:</p>
            <p class='token'>{str2FAcode}</p>
            <p>Deze code is 5 minuten geldig. Als u dit verzoek niet heeft gedaan, neem dan onmiddellijk contact op met onze supportafdeling.</p>
        </div>
        <div class='email-footer'>
            <p>Bedankt om voor IcedOutWheelz te kiezen!</p>
            <p>&copy; 2025 IcedOutWheelz. Alle rechten voorbehouden.</p>
        </div>
    </div>
</body>
</html>
";

                        // Verstuur 2FA-code per e-mail
                        LoginDA.Email2FAversturen(log.Email, "Uw 2FA code", strEmailBody);

                        // Vraag gebruiker om 2FA-code in te voeren
                        str2FAcodeCHECKER = Interaction.InputBox("Geef je 2FA code in", "2FA");

                        // Controleer of de code nog geldig is
                        if (DateTime.Now - codeGenerationTime > codeValidityDuration)
                        {
                            MessageBox.Show("Je 2FA is verlopen!\nGelieve opnieuw in te loggen om opnieuw een code te krijgen.");
                        }
                        else
                        {
                            // Controleer of de code correct is
                            if (str2FAcodeCHECKER == str2FAcode)
                            {
                                MessageBox.Show("Succesvol ingelogd!", "Login");
                                frmhoofdpagina frm = new frmhoofdpagina();
                                frm.Show();
                                this.Hide();
                            }
                            else
                            {
                                // Vraag of gebruiker een nieuwe code wil
                                dl = MessageBox.Show("2FA code is incorrect\rWil je een nieuwe code?", "2FA", MessageBoxButtons.YesNo);

                                if(dl != DialogResult.Yes)
                                {
                                    break;
                                }
                            }
                        }
                    } while (str2FAcodeCHECKER != str2FAcode);
                }
                else
                {
                    MessageBox.Show("Email en/of wachtwoord komen niet overeen!", "Login");
                }
            }
            else
            {
                MessageBox.Show("Gelieve alle velden in te vullen!", "Lege velden");
            }
        }

        // Sluit loginvenster en open startscherm
        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmStartscherm frm = new frmStartscherm();
            this.Hide();
            frm.Show();
        }

        // Verwerk focus op e-mailveld
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            txtEmail.Text = ""; // Maak veld leeg
            txtEmail.ForeColor = Color.Black; // Zet tekstkleur op zwart
        }

        // Zet standaardtekst terug als e-mailveld leeg is
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.Text = "Email";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        // Verwerk focus op wachtwoordveld
        private void txtWachtwoord_Enter(object sender, EventArgs e)
        {
            txtWachtwoord.Text = "";
            txtWachtwoord.ForeColor = Color.Black;
        }

        // Zet standaardtekst terug als wachtwoordveld leeg is
        private void txtWachtwoord_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWachtwoord.Text))
            {
                txtWachtwoord.Text = "Wachtwoord";
                txtWachtwoord.ForeColor = Color.Gray;
            }
        }

        // Wachtwoord wijzigen knop
        private void btnChangePass_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            DialogResult dlr2;

            L.Email = txtEmail.Text;
            L.Wachtwoord = txtWachtwoord.Text;

            // Controleer inloggegevens voor wachtwoord wijzigen
            if(LoginDA.LoginValidation(L) == 1)
            {
                string strNewPass, strPassConfirm;

                c:
                // Vraag nieuw wachtwoord en bevestiging
                strNewPass = Interaction.InputBox("Voer nieuw wachtwoord in:", "Wachtwoord wijzigen");
                strPassConfirm = Interaction.InputBox("Voer wachtwoord opnieuw in.", "Wachtwoord wijzigen");

                // Controleer of beide velden zijn ingevuld
                if(strNewPass != "" || strPassConfirm != "")
                {
                    // Controleer of wachtwoorden overeenkomen
                    if (strNewPass == strPassConfirm)
                    {
                        LoginDA.WachtwoordVeranderen(L, strNewPass);
                    }
                    else
                    {
                        // Vraag of gebruiker opnieuw wil proberen
                        DialogResult dlr = MessageBox.Show("Wachtwoorden komen niet overeen", "Wachtwoord wijzigen", MessageBoxButtons.RetryCancel);

                        switch (dlr)
                        {
                            case DialogResult.Cancel:
                                {
                                    return;
                                }
                            case DialogResult.Retry:
                                {
                                    goto c;
                                }
                        }
                    }
                    txtEmail.Clear();
                }
                else
                {
                    // Vraag of gebruiker wil stoppen met wijzigen
                    dlr2 = MessageBox.Show("U heeft geen nieuw wachtwoord ingevuld.\nWilt u stoppen met uw wachtwoord veranderen?", "Lege velden", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if(dlr2 == DialogResult.No)
                    {
                        goto c;
                    }
                }
            }
            else
            {
                MessageBox.Show("Email en/of wachtwoord komen niet overeen!", "Wachtwoord wijzigen");
            }
        }

        // Gebruiker verwijderen knop
        private void btnGebruikerVerwijderen_Click(object sender, EventArgs e)
        {
            // Controleer of de velden niet op de standaardwaarde staan
            if (txtEmail.Text != "Email" && txtWachtwoord.Text != "Wachtwoord")
            {
                Login L = new Login();
                string str2FAcodeCHECKER, str2FAcode;
                DateTime codeGenerationTime = DateTime.Now;
                TimeSpan codeValidityDuration = TimeSpan.FromMinutes(5);
                DialogResult dlr;

                L.Email = txtEmail.Text;
                L.Wachtwoord = txtWachtwoord.Text;

                // Genereer 2FA-code
                str2FAcode = LoginDA.Genereer2FAcode();

                // Stel e-mailbody samen voor 2FA
                string strEmailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            background-color: #ffffff;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }}
        .email-header {{
            background-color: #007bff;
            color: #ffffff;
            padding: 10px;
            text-align: center;
            border-radius: 8px 8px 0 0;
        }}
        .email-content {{
            padding: 20px;
            text-align: center;
        }}
        .email-footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #6c757d;
            text-align: center;
        }}
        .token {{
            font-size: 24px;
            font-weight: bold;
            color: #007bff;
        }}
        a.button {{
            display: inline-block;
            padding: 10px 20px;
            margin: 10px 0;
            background-color: #007bff;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
        }}
        a.button:hover {{
            background-color: #0056b3;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='email-header'>
            <h1>Twee-Factor Authenticatie Code</h1>
        </div>
        <div class='email-content'>
            <p>Beste gebruiker,</p>
            <p>We hebben een verzoek ontvangen om toegang te krijgen tot uw account. Gebruik de onderstaande code om uw login te voltooien:</p>
            <p class='token'>{str2FAcode}</p>
            <p>Deze code is 5 minuten geldig. Als u dit verzoek niet heeft gedaan, neem dan onmiddellijk contact op met onze supportafdeling.</p>
        </div>
        <div class='email-footer'>
            <p>Bedankt om voor IcedOutWheelz te kiezen!</p>
            <p>&copy; 2025 IcedOutWheelz. Alle rechten voorbehouden.</p>
        </div>
    </div>
</body>
</html>
";

                // Controleer login-gegevens
                if (LoginDA.LoginValidation(L) == 1)
                {
                    do
                    {
                        // Verstuur 2FA-code per e-mail
                        LoginDA.Email2FAversturen(L.Email, "Uw 2FA code", strEmailBody);

                        // Vraag gebruiker om 2FA-code in te voeren
                        str2FAcodeCHECKER = Interaction.InputBox("Geef je 2FA code in", "2FA");

                        // Controleer of de code nog geldig is
                        if (DateTime.Now - codeGenerationTime > codeValidityDuration)
                        {
                            MessageBox.Show("Je 2FA is verlopen!\nGelieve opnieuw in te loggen om opnieuw een code te krijgen.");
                        }
                        else
                        {
                            // Controleer of de code correct is
                            if (str2FAcodeCHECKER == str2FAcode)
                            {
                                LoginDA.GebruikerVerwijderen(L);
                            }
                            else
                            {
                                // Vraag of gebruiker een nieuwe code wil
                                dlr = MessageBox.Show("2FA code is incorrect\rWil je een nieuwe code?", "2FA", MessageBoxButtons.YesNo);

                                if (dlr != DialogResult.Yes)
                                {
                                    break;
                                }
                            }
                        }
                    } while (str2FAcodeCHECKER != str2FAcode);
                }
                else
                {
                    MessageBox.Show("Email en/of wachtwoord komen niet overeen!", "Login");
                }
            }
            else
            {
                MessageBox.Show("Gelieve alle velden in te vullen!", "Lege velden");
            }
        }
    }  
}
