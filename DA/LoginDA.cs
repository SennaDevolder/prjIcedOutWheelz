﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

//dependencies om mails te versturen

using System.Net;
using System.Net.Mail;

//we gebruiken hetgene die in deze mappen zit (klassen Database en Login)
using prjIcedOutWheelz.Helper;
using prjIcedOutWheelz.Model;

//zodat we MySql kunnen gebruiken
using MySql.Data.MySqlClient;

namespace prjIcedOutWheelz.DA
{
    //alles programmeren van onze klasse login (we gebruiken daarvoor onze klasse Database, onze klasse Login
    public class LoginDA
    {

        //we bekijken of de logingegevens correct zijn 
        //boolean om te controleren of ze correct zijn --> True // anders Fale
        public static int LoginValidation(Login L)
        {
            //sqlconnection aanmaken --> verbinding met de databank te maken
            MySqlConnection conn = Database.MakeConnection();

            string query = "SELECT COUNT(1) FROM icedoutwheelz.tblKlant WHERE Email=@Email AND Wachtwoord=@Wachtwoord";
            //commando maken --> zorgt ervoor dat de sql statement wordt ingezet
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            //welk soort commando is dat?
            sqlcmd.Parameters.AddWithValue("@Email", L.Email);
            sqlcmd.Parameters.AddWithValue("@Wachtwoord", L.Wachtwoord);

            //tellen van het aantal gevonden gegevens
            int count = Convert.ToInt32(sqlcmd.ExecuteScalar());
            //return 1 of 0
            return count;

        }

        public static void GebruikerToevoegen(Login L)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "INSERT INTO `tblklant`(`Naam`, `Email`, `Wachtwoord`, `TelNummer`, `Straat_Num`, `Gemeente_Postc`) VALUES (@Naam,@Email,@Wachtwoord,@TelNummer,@Straat_Num,@Gemeente_Postc)";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@Naam", L.Naam);
            sqlcmd.Parameters.AddWithValue("@Email", L.Email);
            sqlcmd.Parameters.AddWithValue("@Wachtwoord", L.Wachtwoord);
            sqlcmd.Parameters.AddWithValue("@TelNummer", L.TelNummer);
            sqlcmd.Parameters.AddWithValue("@Straat_Num", L.Straat_Num);
            sqlcmd.Parameters.AddWithValue("@Gemeente_Postc", L.Gemeente_Postc);

            sqlcmd.ExecuteScalar();

            MessageBox.Show("Gebruiker succesvol toegevoegd!");
        }

        public static void WachtwoordVeranderen(Login L, string NewPass)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "UPDATE icedoutwheelz.tblKlant SET Wachtwoord=@NewPass WHERE Email=@Email AND Wachtwoord=@Wachtwoord";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@Email", L.Email);
            sqlcmd.Parameters.AddWithValue("@Wachtwoord", L.Wachtwoord);
            sqlcmd.Parameters.AddWithValue("@NewPass", NewPass);

            sqlcmd.ExecuteScalar();


            MessageBox.Show("Wachtwoord succesvol gewijzigd!", "Wachtwoord wijzigen");
        }

        public static void GebruikerVerwijderen(Login L)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "DELETE FROM icedoutwheelz.tblKlant WHERE Email=@Email AND Wachtwoord=@Wachtwoord";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@Email", L.Email);
            sqlcmd.Parameters.AddWithValue("@Wachtwoord", L.Wachtwoord);

            sqlcmd.ExecuteScalar();

            MessageBox.Show("Gebruiker werd verwijderd!");
        }

        public static string SoortGebruikerCheck(Login L)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "SELECT Soort_Gebruiker FROM icedoutwheelz.tblKlant WHERE Email=@Email AND Wachtwoord=@Wachtwoord";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@Email", L.Email);
            sqlcmd.Parameters.AddWithValue("@Wachtwoord", L.Wachtwoord);

            string SoortGebruiker = sqlcmd.ExecuteScalar().ToString();

            return SoortGebruiker;
        }

        public static void Email2FAversturen(string recipientEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("icedoutwheelz.2fa@gmail.com", "kafe mqua douj epah"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("icedoutwheelz.2fa@gmail.com", "IcedOutWheelz 2FA"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Enable HTML email
                };

                mailMessage.To.Add(recipientEmail);

                smtpClient.Send(mailMessage);
                MessageBox.Show("Verification code sent to email.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send email: {ex.Message}");
            }
        }

        public static string Genereer2FAcode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
