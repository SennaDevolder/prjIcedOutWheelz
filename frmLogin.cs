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
        public static Login log = new Login();
        public frmLogin()
        {
            InitializeComponent();

            this.CenterToScreen();

            txtEmail.Text = "Email";
            txtEmail.ForeColor = Color.Gray;
            txtWachtwoord.Text = "Wachtwoord";
            txtWachtwoord.ForeColor = Color.Gray;


        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtEmail.Text != "Email" && txtWachtwoord.Text != "Wachtwoord")
            {
                string str2FAcodeCHECKER, str2FAcode;
                DateTime codeGenerationTime = DateTime.Now;
                TimeSpan codeValidityDuration = TimeSpan.FromMinutes(5);
                DialogResult dl = new DialogResult();


                log.Email = txtEmail.Text;
                log.Wachtwoord = txtWachtwoord.Text;

                str2FAcode = LoginDA.Genereer2FAcode();

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

                if (LoginDA.LoginValidation(log) == 1)
                {
                    do
                    { 
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
                        LoginDA.Email2FAversturen(log.Email, "Your 2FA code", strEmailBody);

                        str2FAcodeCHECKER = Interaction.InputBox("Geef je 2FA code in", "2FA");

                        if (DateTime.Now - codeGenerationTime > codeValidityDuration)
                        {
                            MessageBox.Show("Je 2FA is verlopen!\nGelieve opnieuw in te loggen om opnieuw een code te krijgen.");
                        }
                        else
                        {
                            if (str2FAcodeCHECKER == str2FAcode)
                            {
                                MessageBox.Show("Succesvol ingelogd!", "Login");
                                frmhoofdpagina frm = new frmhoofdpagina();
                                frm.Show();
                                this.Hide();
                            }
                            else
                            {
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

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            txtEmail.ForeColor = Color.Black;
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.Text = "Email";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        private void txtWachtwoord_Enter(object sender, EventArgs e)
        {
            txtWachtwoord.Text = "";
            txtWachtwoord.ForeColor = Color.Black;
        }

        private void txtWachtwoord_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWachtwoord.Text))
            {
                txtWachtwoord.Text = "Wachtwoord";
                txtWachtwoord.ForeColor = Color.Gray;
            }
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            Login L = new Login();

            L.Email = txtEmail.Text;
            L.Wachtwoord = txtWachtwoord.Text;

            if(LoginDA.LoginValidation(L) == 1)
            {
                string strNewPass, strPassConfirm;

                c:
                strNewPass = Interaction.InputBox("Voer nieuw wachtwoord in:", "Wachtwoord wijzigen");
                strPassConfirm = Interaction.InputBox("Voer wachtwoord opnieuw in.", "Wachtwoord wijzigen");

                if (strNewPass == strPassConfirm)
                {
                    LoginDA.WachtwoordVeranderen(L, strNewPass);
                }
                else
                {
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
        }

        private void btnGebruikerVerwijderen_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text != "Email" && txtWachtwoord.Text != "Wachtwoord")
            {
                Login L = new Login();
                string str2FAcodeCHECKER, str2FAcode;
                DateTime codeGenerationTime = DateTime.Now;
                TimeSpan codeValidityDuration = TimeSpan.FromMinutes(5);


                L.Email = txtEmail.Text;
                L.Wachtwoord = txtWachtwoord.Text;

                str2FAcode = LoginDA.Genereer2FAcode();

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

                if (LoginDA.LoginValidation(L) == 1)
                {
                    do
                    {
                        LoginDA.Email2FAversturen(L.Email, "Your 2FA code", strEmailBody);

                        str2FAcodeCHECKER = Interaction.InputBox("Geef je 2FA code in", "2FA");

                        if (DateTime.Now - codeGenerationTime > codeValidityDuration)
                        {
                            MessageBox.Show("Je 2FA is verlopen!\nGelieve opnieuw in te loggen om opnieuw een code te krijgen.");
                        }
                        else
                        {
                            if (str2FAcodeCHECKER == str2FAcode)
                            {
                                LoginDA.GebruikerVerwijderen(L);
                            }
                            else
                            {
                                MessageBox.Show("2FA code is incorrect\rEr is een nieuwe 2FA code verzonden.", "2FA");
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
