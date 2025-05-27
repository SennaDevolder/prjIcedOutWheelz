using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Tls.Crypto.Impl;
using prjIcedOutWheelz.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Mail;
using System.Net;
using System.Drawing.Printing;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Drawing.Imaging;
using System.Globalization;
using prjIcedOutWheelz.Model;

namespace prjIcedOutWheelz.DA
{
    // Data Access Layer voor auto-gerelateerde database-operaties
    internal class AutoDA
    {
        // Haal alle autotypes op uit de database
        public static DataSet TypesOphalen()
        {
            // Maak een nieuw DataSet aan voor de resultaten
            DataSet dsTypes = new DataSet();

            // SQL-query om typeID, merk, type en jaar op te halen
            string sql = "SELECT typeID, Merk, type, jaar FROM tbltype ORDER BY Merk ASC";

            MySqlConnection conn = Database.MakeConnection();
            // DataAdapter voor het uitvoeren van de query
            MySqlDataAdapter daTypes = new MySqlDataAdapter(sql, conn);

            // Vul het DataSet met de resultaten
            daTypes.Fill(dsTypes);

            return dsTypes;
        }

        // Haal alle motoren op die gekoppeld zijn aan een bepaald autotype
        public static DataSet MotorPerAutoOphalen(int intAutoType)
        {
            DataSet dsMotorPerAuto = new DataSet();

            // SQL-query met INNER JOIN om motoren per type op te halen
            string sql = "SELECT MT.MotorPerTypeID, M.MotorType FROM tblmotorpertype MT INNER JOIN tblmotoren M ON MT.MotorID = M.MotorID WHERE MT.TypeID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AutoType", intAutoType);

            MySqlDataAdapter daMotorPerAuto = new MySqlDataAdapter(cmd);
            daMotorPerAuto.Fill(dsMotorPerAuto);

            return dsMotorPerAuto;
        }

        // Haal alle kleuren op die gekoppeld zijn aan een bepaald autotype
        public static DataSet KleurPerAutoOphalen(int intAutoType)
        {
            DataSet dsKleurPerAuto = new DataSet();

            // SQL-query met INNER JOIN om kleuren per type op te halen
            string sql = "SELECT KT.KleurPerTypeID, K.Kleur FROM tblkleurpertype KT INNER JOIN tblkleuren K ON KT.KleurID = K.KleurID WHERE KT.TypeID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AutoType", intAutoType);

            MySqlDataAdapter daKleurPerAuto = new MySqlDataAdapter(cmd);
            daKleurPerAuto.Fill(dsKleurPerAuto);

            return dsKleurPerAuto;
        }

        // Haal alle statussen op die gekoppeld zijn aan een bepaald autotype
        public static DataSet StatusPerAutoOphalen(int intAutoType)
        {
            DataSet dsStatsuPerAuto = new DataSet();

            // SQL-query met INNER JOIN om statussen per type op te halen
            string sql = "SELECT ST.StatusPerTypeID, S.Status FROM tblstatuspertype ST INNER JOIN tblstatus S ON ST.StatusID = S.StatusID WHERE ST.TypeID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AutoType", intAutoType);

            MySqlDataAdapter daStatusPerAuto = new MySqlDataAdapter(cmd);
            daStatusPerAuto.Fill(dsStatsuPerAuto);

            return dsStatsuPerAuto;
        }

        // Haal de foto op van een auto op basis van het typeID
        public static DataSet FotoOphalen(int AutoType)
        {
            DataSet dsFoto = new DataSet();
            string sql = "SELECT Foto FROM tbltype WHERE typeID=@typeID";

            MySqlConnection conn = Database.MakeConnection();
            MySqlDataAdapter daFoto = new MySqlDataAdapter(sql, conn);

            // Voeg het typeID toe als parameter
            daFoto.SelectCommand.Parameters.AddWithValue("@typeID", AutoType);

            // Vul het DataSet met de foto
            daFoto.Fill(dsFoto);

            return dsFoto;
        }

        // Haal alle info op van een auto en de gekoppelde motor op basis van type en motor
        public static DataSet AutoInfoOphalen(int AutoType, int MotorType)
        {
            DataSet dsAutoInfo = new DataSet();

            // SQL-query om alle relevante info van auto en motor op te halen
            string sql = "SELECT t.Merk, t.Type, t.Jaar, m.MotorType, m.BrandstofType, m.Vermogen, m.Koppel, m.Batterijcapaciteit FROM tblmotoren m INNER JOIN tbltype t ON m.AutoID = t.typeID WHERE m.AutoID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();
            MySqlDataAdapter daAutoInfo = new MySqlDataAdapter(sql, conn);

            // Voeg parameters toe voor de query
            daAutoInfo.SelectCommand.Parameters.AddWithValue("@AutoType", AutoType);
            daAutoInfo.SelectCommand.Parameters.AddWithValue("@MotorType", MotorType);

            // Vul het DataSet met de resultaten
            daAutoInfo.Fill(dsAutoInfo);

            return dsAutoInfo;
        }

        // Slaat een nieuwe offerte op in de database
        public static void AutoOfferteAanmaken(
            double prijs,
            int kleurID,
            int statusID,
            int typeID,
            int motorID,
            bool stuurVerwarming,
            bool cruiseControl,
            bool zetelverwarming,
            bool parkeersensoren,
            bool trekHaak,
            bool xenonlampen,
            bool geblindeerdeRamen)
        {
            // SQL-query om een offerte toe te voegen
            string sql = "INSERT INTO `tblautoofferte`(`Prijs`, `KleurID`, `StatusID`, `TypeID`, `MotorID`, `Stuurverwarming`, `CruiseControl`, `Zetelverwarming`, `Parkeersensoren`, `Trekhaak`, `Xenonlampen`, `GeblindeerdeRamen`) VALUES (@Prijs, @KleurID, @Status, @TypeID, @MotorID, @Stuurverwarming, @CruiseControl, @Zetelverwarming, @Parkeersensoren, @Trekhaak, @Xenonlampen, @GeblindeerdeRamen);";

            MySqlConnection conn = Database.MakeConnection();
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                // Voeg parameters toe aan de query
                cmd.Parameters.AddWithValue("@Prijs", prijs);
                cmd.Parameters.AddWithValue("@KleurID", kleurID);
                cmd.Parameters.AddWithValue("@Status", statusID);
                cmd.Parameters.AddWithValue("@TypeID", typeID);
                cmd.Parameters.AddWithValue("@MotorID", motorID);
                // Zet booleans om naar 1/0 voor database
                cmd.Parameters.AddWithValue("@Stuurverwarming", stuurVerwarming ? 1 : 0);
                cmd.Parameters.AddWithValue("@CruiseControl", cruiseControl ? 1 : 0);
                cmd.Parameters.AddWithValue("@Zetelverwarming", zetelverwarming ? 1 : 0);
                cmd.Parameters.AddWithValue("@Parkeersensoren", parkeersensoren ? 1 : 0);
                cmd.Parameters.AddWithValue("@Trekhaak", trekHaak ? 1 : 0);
                cmd.Parameters.AddWithValue("@Xenonlampen", xenonlampen ? 1 : 0);
                cmd.Parameters.AddWithValue("@GeblindeerdeRamen", geblindeerdeRamen ? 1 : 0);

                cmd.ExecuteNonQuery(); // Voer de insert uit
            }
            catch (MySqlException ex)
            {
                // Toon foutmelding bij databasefout
                MessageBox.Show($"Error: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Sluit de verbinding indien nodig
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Haal het KleurID op op basis van de naam van de kleur
        public static int GetKleurID(string kleur)
        {
            int kleurID = 0;
            string query = "SELECT KleurID FROM `tblkleuren` WHERE `Kleur` = @Kleur LIMIT 1";

            try
            {
                using (MySqlConnection conn = Database.MakeConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Kleur", kleur);

                        if (conn.State == System.Data.ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            kleurID = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show($"Geen KleurID gevonden voor: {kleur}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het ophalen van KleurID: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return kleurID;
        }

        // Haal het StatusID op op basis van de naam van de status
        public static int GetStatusID(string status)
        {
            int statusID = 0;
            string query = "SELECT StatusID FROM `tblstatus` WHERE `Status` = @Status LIMIT 1";

            try
            {
                using (MySqlConnection conn = Database.MakeConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);

                        if (conn.State == System.Data.ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            statusID = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show($"Geen StatusID gevonden voor: {status}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het ophalen van StatusID: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return statusID;
        }

        // Haal het typeID op op basis van de naam van het type
        public static int GetTypeID(string type)
        {
            int typeID = 0;
            string query = "SELECT typeID FROM `tbltypes` WHERE `Type` = @Type LIMIT 1";

            MySqlConnection conn = Database.MakeConnection();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Type", type);

            try
            {
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    typeID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het ophalen van TypeID: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            return typeID;
        }

        // Haal het MotorID op op basis van de naam van het motortype
        public static int GetMotorID(string motor)
        {
            int motorID = 0;
            string query = "SELECT MotorID FROM `tblmotoren` WHERE `MotorType` = @Motor LIMIT 1";

            try
            {
                using (MySqlConnection conn = Database.MakeConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Motor", motor);

                        if (conn.State == System.Data.ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            motorID = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show($"Geen MotorID gevonden voor: {motor}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het ophalen van MotorID: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return motorID;
        }
    }
}
