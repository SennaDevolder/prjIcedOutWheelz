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
    internal class AutoDA
    {


        public static DataSet TypesOphalen()
        {
            //virtuele weergave van tabellen, maak tijdelijke eigen tabel.
            DataSet dsTypes = new DataSet();

            string sql = "SELECT typeID, Merk, type, jaar FROM tbltype ORDER BY Merk ASC";

            MySqlConnection conn = Database.MakeConnection();
            //vergelijking USB C naar USB A, converting. Tunnel tussen database en programma
            MySqlDataAdapter daTypes = new MySqlDataAdapter(sql, conn);

            //DataSet vullen met sql resultaat
            daTypes.Fill(dsTypes);

            return dsTypes;
        }

        public static DataSet MotorPerAutoOphalen(int intAutoType)
        {
            //virtuele weergave van tabellen, maak tijdelijke eigen tabel.
            DataSet dsMotorPerAuto = new DataSet();

            string sql = "SELECT MT.MotorPerTypeID, M.MotorType FROM tblmotorpertype MT INNER JOIN tblmotoren M ON MT.MotorID = M.MotorID WHERE MT.TypeID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AutoType", intAutoType);

            //vergelijking USB C naar USB A, converting. Tunnel tussen database en programma
            MySqlDataAdapter daMotorPerAuto = new MySqlDataAdapter(cmd);


            //DataSet vullen met sql resultaat
            daMotorPerAuto.Fill(dsMotorPerAuto);

            return dsMotorPerAuto;
        }

        public static DataSet KleurPerAutoOphalen(int intAutoType)
        {
            //virtuele weergave van tabellen, maak tijdelijke eigen tabel.
            DataSet dsKleurPerAuto = new DataSet();

            string sql = "SELECT KT.KleurPerTypeID, K.Kleur FROM tblkleurpertype KT INNER JOIN tblkleuren K ON KT.KleurID = K.KleurID WHERE KT.TypeID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AutoType", intAutoType);

            //vergelijking USB C naar USB A, converting. Tunnel tussen database en programma
            MySqlDataAdapter daKleurPerAuto = new MySqlDataAdapter(cmd);


            //DataSet vullen met sql resultaat
            daKleurPerAuto.Fill(dsKleurPerAuto);

            return dsKleurPerAuto;
        }

        public static DataSet StatusPerAutoOphalen(int intAutoType)
        {
            DataSet dsStatsuPerAuto = new DataSet();

            string sql = "SELECT ST.StatusPerTypeID, S.Status FROM tblstatuspertype ST INNER JOIN tblstatus S ON ST.StatusID = S.StatusID WHERE ST.TypeID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AutoType", intAutoType);

            MySqlDataAdapter daStatusPerAuto = new MySqlDataAdapter(cmd);

            daStatusPerAuto.Fill(dsStatsuPerAuto);

            return dsStatsuPerAuto;
        }

        public static DataSet FotoOphalen(int AutoType)
        {
            DataSet dsFoto = new DataSet();
            string sql = "SELECT Foto FROM tbltype WHERE typeID=@typeID";

            MySqlConnection conn = Database.MakeConnection();

            MySqlDataAdapter daFoto = new MySqlDataAdapter(sql, conn);

            daFoto.SelectCommand.Parameters.AddWithValue("@typeID", AutoType);

            daFoto.Fill(dsFoto);

            return dsFoto;
        }

        public static DataSet AutoInfoOphalen(int AutoType, int MotorType)
        {
            DataSet dsAutoInfo = new DataSet();

            string sql = "SELECT t.Merk, t.Type, t.Jaar, m.MotorType, m.BrandstofType, m.Vermogen, m.Koppel, m.Batterijcapaciteit FROM tblmotoren m INNER JOIN tbltype t ON m.AutoID = t.typeID WHERE m.AutoID=@AutoType";

            MySqlConnection conn = Database.MakeConnection();
            MySqlDataAdapter daAutoInfo = new MySqlDataAdapter(sql, conn);

            daAutoInfo.SelectCommand.Parameters.AddWithValue("@AutoType", AutoType);
            daAutoInfo.SelectCommand.Parameters.AddWithValue("@MotorType", MotorType);

            daAutoInfo.Fill(dsAutoInfo);

            return dsAutoInfo;
        }

        public static void AutoOfferteAanmaken(double prijs, int kleurID, int statusID, int typeID, int motorID, bool stuurVerwarming, bool cruiseControl, bool zetelverwarming, bool parkeersensoren, bool trekHaak, bool xenonlampen, bool geblindeerdeRamen)
        {
            string sql = "INSERT INTO `tblautoofferte`(`Prijs`, `KleurID`, `StatusID`, `TypeID`, `MotorID`, `Stuurverwarming`, `CruiseControl`, `Zetelverwarming`, `Parkeersensoren`, `Trekhaak`, `Xenonlampen`, `GeblindeerdeRamen`) VALUES (@Prijs, @KleurID, @Status, @TypeID, @MotorID, @Stuurverwarming, @CruiseControl, @Zetelverwarming, @Parkeersensoren, @Trekhaak, @Xenonlampen, @GeblindeerdeRamen);";

            MySqlConnection conn = Database.MakeConnection();
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@Prijs", prijs);
                cmd.Parameters.AddWithValue("@KleurID", kleurID);
                cmd.Parameters.AddWithValue("@Status", statusID);
                cmd.Parameters.AddWithValue("@TypeID", typeID);
                cmd.Parameters.AddWithValue("@MotorID", motorID);

                // Add checkbox states as parameters (convert booleans to 1 or 0)
                cmd.Parameters.AddWithValue("@Stuurverwarming", stuurVerwarming ? 1 : 0);
                cmd.Parameters.AddWithValue("@CruiseControl", cruiseControl ? 1 : 0);
                cmd.Parameters.AddWithValue("@Zetelverwarming", zetelverwarming ? 1 : 0);
                cmd.Parameters.AddWithValue("@Parkeersensoren", parkeersensoren ? 1 : 0);
                cmd.Parameters.AddWithValue("@Trekhaak", trekHaak ? 1 : 0);
                cmd.Parameters.AddWithValue("@Xenonlampen", xenonlampen ? 1 : 0);
                cmd.Parameters.AddWithValue("@GeblindeerdeRamen", geblindeerdeRamen ? 1 : 0);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // Output the full exception details for better debugging
                MessageBox.Show($"Error: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Close the connection
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static int GetKleurID(string kleur)
        {
            int kleurID = 0;
            string query = "SELECT KleurID FROM `tblkleuren` WHERE `Kleur` = @Kleur LIMIT 1";

            try
            {
                using (MySqlConnection conn = Database.MakeConnection()) // Verbind met database
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Kleur", kleur);

                        if (conn.State == System.Data.ConnectionState.Closed)
                        {
                            conn.Open(); // Open verbinding als deze nog niet open is
                        }

                        var result = cmd.ExecuteScalar(); // Voer query uit
                        if (result != null)
                        {
                            kleurID = Convert.ToInt32(result); // Zet om naar integer
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



        public static int GetStatusID(string status)
        {
            int statusID = 0;
            string query = "SELECT StatusID FROM `tblstatus` WHERE `Status` = @Status LIMIT 1";

            try
            {
                using (MySqlConnection conn = Database.MakeConnection()) // Verbind met database
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);

                        if (conn.State == System.Data.ConnectionState.Closed)
                        {
                            conn.Open(); // Open verbinding als deze nog niet open is
                        }

                        var result = cmd.ExecuteScalar(); // Voer query uit
                        if (result != null)
                        {
                            statusID = Convert.ToInt32(result); // Zet om naar integer
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


        public static int GetTypeID(string type)
        {
            int typeID = 0;

            string query = "SELECT typeID FROM `tbltypes` WHERE `Type` = @Type LIMIT 1";

            MySqlConnection conn = Database.MakeConnection();
            MySqlCommand cmd = new MySqlCommand(query, conn);

            // Voeg de parameter toe
            cmd.Parameters.AddWithValue("@Type", type);

            try
            {
                conn.Open();
                var result = cmd.ExecuteScalar();  // Haal de TypeID op
                if (result != null)
                {
                    typeID = Convert.ToInt32(result); // Zet om naar een integer
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
            conn.Close();
            return typeID;
        }

        public static int GetMotorID(string motor)
        {
            int motorID = 0;
            string query = "SELECT MotorID FROM `tblmotoren` WHERE `MotorType` = @Motor LIMIT 1";

            try
            {
                using (MySqlConnection conn = Database.MakeConnection()) // Verbind met database
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Motor", motor);

                        if (conn.State == System.Data.ConnectionState.Closed)
                        {
                            conn.Open(); // Open verbinding als deze nog niet open is
                        }

                        var result = cmd.ExecuteScalar(); // Voer query uit
                        if (result != null)
                        {
                            motorID = Convert.ToInt32(result); // Zet om naar integer
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
