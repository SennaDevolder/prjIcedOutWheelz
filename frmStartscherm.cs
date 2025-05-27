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
            InitializeComponent(); // Initialiseer alle UI-componenten
            // Laat het startscherm in het midden van het scherm verschijnen
            this.CenterToScreen();
        }

        // Open het loginvenster wanneer op de login-knop wordt geklikt
        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin(); // Maak een nieuw loginvenster
            frmLogin.Show(); // Toon het loginvenster
            this.Hide(); // Verberg het startscherm
        }

        // Open het registratievenster wanneer op de registreer-knop wordt geklikt
        private void btnRegistreer_Click(object sender, EventArgs e)
        {
            frmRegistreer frmRegistreer = new frmRegistreer(); // Maak een nieuw registratievenster
            frmRegistreer.Show(); // Toon het registratievenster
            this.Hide(); // Verberg het startscherm
        }

        // Open het 'Over Ons'-venster wanneer op de betreffende knop wordt geklikt
        private void button1_Click(object sender, EventArgs e)
        {
            Over_Ons frmOverOns = new Over_Ons(); // Maak een nieuw Over_Ons-venster
            frmOverOns.Show(); // Toon het Over_Ons-venster
        }

        private void frmStartscherm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
