using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using prjIcedOutWheelz.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjIcedOutWheelz.DA
{
    public class AdminDA
    {
        // Haalt het typeID op van een auto op basis van merk en type
        public static int GetAutoIdByMerkAndType(string merk, string type)
        {
            int typeID = 0;

            MySqlConnection conn = Database.MakeConnection();

            // Selecteert het laatste typeID uit de tabel
            string query = "SELECT typeID FROM tbltype ORDER BY typeID DESC;";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Merk", merk);
            cmd.Parameters.AddWithValue("@Type", type);

            typeID = Convert.ToInt32(cmd.ExecuteScalar());

            return typeID;
        }

        // Haalt het typeID op voor kleur (deze methode lijkt identiek aan de vorige)
        public static int GetKleurIdByMerkAndType(string merk, string type)
        {
            int typeID = 0;

            MySqlConnection conn = Database.MakeConnection();

            // Selecteert het laatste typeID uit de tabel
            string query = "SELECT typeID FROM tbltype ORDER BY typeID DESC;";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Merk", merk);
            cmd.Parameters.AddWithValue("@Type", type);

            typeID = Convert.ToInt32(cmd.ExecuteScalar());

            return typeID;
        }

        // Voegt een nieuw type auto toe aan de database
        public static void TypeToevoegen(Model.Type type)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "INSERT INTO `tbltype`(`Merk`, `Type`, `Jaar`, `Prijs`, `Foto`) VALUES (@Merk,@Type,@Jaar,@Prijs, @Foto)";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);

            sqlcmd.Parameters.AddWithValue("@Merk", type.Merk);
            sqlcmd.Parameters.AddWithValue("@Type", type._Type);
            sqlcmd.Parameters.AddWithValue("@Jaar", type.Jaar);
            sqlcmd.Parameters.AddWithValue("@Prijs", type.Prijs);
            sqlcmd.Parameters.AddWithValue("@Foto", type.Foto ?? (object)DBNull.Value);

            sqlcmd.ExecuteScalar();
            MessageBox.Show("Auto succesvol toegevoegd aan de database!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Voegt een motor toe aan de database, gekoppeld aan een auto
        public static void MotorToevoegen(Model.Motoren motor)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "INSERT INTO `tblmotoren`(`MotorType`, `BrandstofType`, `AutoID`, `Vermogen`, `Koppel`, `Batterijcapaciteit`) VALUES (@MotorType,@BrandstofType,@AutoID,@Vermogen,@Koppel,@Batterijcapaciteit)";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);

            sqlcmd.Parameters.AddWithValue("@MotorType", motor.Motortype);
            sqlcmd.Parameters.AddWithValue("@BrandstofType", motor.Brandstoftype);
            sqlcmd.Parameters.AddWithValue("@AutoID", motor.Autoid);
            sqlcmd.Parameters.AddWithValue("@Vermogen", motor.Vermogen);
            sqlcmd.Parameters.AddWithValue("@Koppel", motor.Koppel);
            sqlcmd.Parameters.AddWithValue("@Batterijcapaciteit", motor.Batterijcapaciteit);

            sqlcmd.ExecuteScalar();
            MessageBox.Show("Motor succesvol toegevoegd aan de database!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Koppelt een kleur aan een type auto
        public static void KleurPerTypeToevoegen(int TypeID, int KleurID)
        {
            MySqlConnection conn = Database.MakeConnection();
            string query = "INSERT INTO `tblkleurpertype`(`TypeID`, `KleurID`) VALUES (@TypeID, @KleurID)";

            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@TypeID", TypeID);
            sqlcmd.Parameters.AddWithValue("@KleurID", KleurID);

            sqlcmd.ExecuteNonQuery();
        }

        // Haalt het KleurID op op basis van de naam van de kleur
        public static int KleurIDOphalen(string Kleur)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "SELECT KleurID FROM `tblkleuren` WHERE `Kleur` = @Kleur";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@Kleur", Kleur);
            int kleurID = Convert.ToInt32(sqlcmd.ExecuteScalar());

            return kleurID;
        }

        // Koppelt een status aan een type auto
        public static void StatusPerTypeToevoegen(int TypeID, int StatusID)
        {
            MySqlConnection conn = Database.MakeConnection();
            string query = "INSERT INTO `tblstatuspertype`(`TypeID`, `StatusID`) VALUES (@TypeID, @StatusID)";

            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@TypeID", TypeID);
            sqlcmd.Parameters.AddWithValue("@StatusID", StatusID);

            sqlcmd.ExecuteNonQuery();
        }

        // Haalt het StatusID op op basis van de naam van de status
        public static int StatusIDOphalen(string Status)
        {
            MySqlConnection conn = Database.MakeConnection();

            string query = "SELECT StatusID FROM `tblstatus` WHERE `Status` = @Status";
            MySqlCommand sqlcmd = new MySqlCommand(query, conn);
            sqlcmd.Parameters.AddWithValue("@Status", Status);
            int kleurID = Convert.ToInt32(sqlcmd.ExecuteScalar());

            return kleurID;
        }
    }
}
