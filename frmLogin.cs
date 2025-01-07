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
            Login L = new Login();

            string str2FAcodeCHECKER, str2FAcode;

            L.Email = txtEmail.Text;
            L.Wachtwoord = txtWachtwoord.Text;

            if (LoginDA.LoginValidation(L) == 1)
            {
                do
                {
                    str2FAcode = LoginDA.Genereer2FAcode();
                    LoginDA.Email2FAversturen(L.Email, "Your 2FA code", $"Your 2FA code is: {str2FAcode}");
                    str2FAcodeCHECKER = Interaction.InputBox("Geef je 2FA code in", "2FA");

                    if (str2FAcodeCHECKER == str2FAcode)
                    {
                        MessageBox.Show("Succesvol ingelogd!", "Login");
                        frmRegistreer frm = new frmRegistreer();
                        frm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("2FA code is incorrect\rEr is een nieuwe 2FA code verzonden.", "2FA");  
                    }

                } while (str2FAcodeCHECKER != str2FAcode);
            }
            else
            {
                MessageBox.Show("Email en/of wachtwoord komen niet overeen!", "Login");
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
    }  
}
