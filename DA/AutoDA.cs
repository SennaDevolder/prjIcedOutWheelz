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

                MessageBox.Show("Uw offerte is aangemaakt!");
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

        public static DataSet OfferteDetailsOphalen()
        {
            DataSet dsOfferteDetails = new DataSet();

            string sql = @"
                SELECT ao.AutoOfferteID, ao.Prijs, k.Kleur AS Kleur, s.Status AS Status, t.Type AS Type, m.MotorType AS Motor,
                       
                GROUP_CONCAT(e.Omschrijving ORDER BY e.ExtraID) AS Extras 
                FROM tblautoofferte ao 
                INNER JOIN tblkleuren k ON ao.KleurID = k.KleurID 
                INNER JOIN tblstatus s ON ao.StatusID = s.StatusID 
                INNER JOIN tbltype t ON ao.TypeID = t.TypeID
                INNER JOIN tblmotoren m ON ao.MotorID = m.MotorID 
                LEFT JOIN tblextras e 
                ON (ao.Stuurverwarming = 1 AND e.ExtraID = 1)   -- Assuming 1 represents Stuurverwarming in tblextras
                OR (ao.CruiseControl = 1 AND e.ExtraID = 2)      -- Assuming 2 represents CruiseControl in tblextras
                OR (ao.Zetelverwarming = 1 AND e.ExtraID = 3)    -- Assuming 3 represents Zetelverwarming in tblextras
                OR (ao.Parkeersensoren = 1 AND e.ExtraID = 4)    -- Assuming 4 represents Parkeersensoren in tblextras
                OR (ao.Trekhaak = 1 AND e.ExtraID = 5)           -- Assuming 5 represents Trekhaak in tblextras
                OR (ao.Xenonlampen = 1 AND e.ExtraID = 6)        -- Assuming 6 represents Xenonlampen in tblextras
                OR (ao.GeblindeerdeRamen = 1 AND e.ExtraID = 7)  -- Assuming 7 represents GeblindeerdeRamen in tblextras
                WHERE ao.AutoOfferteID = (SELECT MAX(AutoOfferteID) FROM tblautoofferte);";
            try
            {
                using (MySqlConnection conn = Database.MakeConnection())
                {
                    MySqlDataAdapter daOfferteDetails = new MySqlDataAdapter(sql, conn);

                    daOfferteDetails.Fill(dsOfferteDetails);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return dsOfferteDetails;
        }

        public static MemoryStream GeneratePdfInMemory(DataTable details)
        {
            try
            {
                // Maak een MemoryStream om de PDF-gegevens in het geheugen vast te houden
                MemoryStream memoryStream = new MemoryStream();

                // Maak een document en koppel het aan de MemoryStream
                Document document = new Document(PageSize.A4, 40f, 40f, 50f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                writer.CloseStream = false;

                // Open het document om inhoud toe te voegen
                document.Open();

                // Voeg het bedrijfslogo toe vanuit de bronnen
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Properties.Resources.Logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageBytes);
                        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        logo.ScaleToFit(150f, 150f);

                        document.Add(logo);
                        document.Add(new Paragraph(" "));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("🔥 Fout bij het toevoegen van logo: " + ex.Message, "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 🎉 Titel met een vleugje elegantie
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 22, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                Paragraph title = new Paragraph("Uw offerte", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph(" ")); // Ruimte tussen de titel en inhoud

                // 🏅 Subtitel met flair
                iTextSharp.text.Font subheadingFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph subheading = new Paragraph("The Road is Calling - Answer in Style!", subheadingFont);
                subheading.Alignment = Element.ALIGN_CENTER;
                document.Add(subheading);
                document.Add(new Chunk(new LineSeparator()));

                // 🛠 Maak de detailtabel (Super strakke structuur)
                PdfPTable table = new PdfPTable(2); // Twee kolommen: Kenmerk, Waarde
                table.WidthPercentage = 100;
                table.SpacingBefore = 20f; // Ruimte voor de tabel
                table.SpacingAfter = 20f;  // Ruimte na de tabel
                table.SetWidths(new float[] { 1.5f, 3f });

                // 🌟 Tabelkoppen (Donker marineblauw voor een strakke uitstraling)
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                BaseColor darkNavy = new BaseColor(0, 0, 102); // Donker marineblauw (krachtig en gedurfd!)

                PdfPCell header1 = new PdfPCell(new Phrase("🔑 Kenmerk", headerFont)) { BackgroundColor = darkNavy, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8f };
                PdfPCell header2 = new PdfPCell(new Phrase("📊 Waarde", headerFont)) { BackgroundColor = darkNavy, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8f };
                table.AddCell(header1);
                table.AddCell(header2);

                decimal totalPrice = 0; // 💵 Houd de totale prijs bij
                CultureInfo belgianCulture = new CultureInfo("nl-BE"); // Belgische euroformattering magie

                bool alternateRow = false; // Wisselende rijkleuren - voor de verfijning

                // 🔄 Loop door de DataTable en vul de cellen in
                foreach (DataRow row in details.Rows)
                {
                    iTextSharp.text.Font cellFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    BaseColor rowColor = alternateRow ? new BaseColor(240, 240, 240) : BaseColor.WHITE;
                    alternateRow = !alternateRow; // Wissel de rijkleuren (chic)

                    // 📝 Dynamische gegevens toevoegen voor elke rij
                    table.AddCell(new PdfPCell(new Phrase("Order Nummer", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["AutoOfferteID"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Prijs (€)", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    decimal price = Convert.ToDecimal(row["Prijs"]);
                    table.AddCell(new PdfPCell(new Phrase(price.ToString("C2", belgianCulture), cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    totalPrice += price;

                    table.AddCell(new PdfPCell(new Phrase("Kleur", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Kleur"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Status", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Status"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Type", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Type"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Motor", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Motor"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // ✨ Formateren van "Extras" veld: Opsommingstekens voor extra's
                    string extras = row["Extras"].ToString();
                    string formattedExtras = string.Join("\n• ", extras.Split(',').Select(extra => extra.Trim())); // Bullet-points

                    // 📋 Voeg de extra's toe in een nette lijst
                    table.AddCell(new PdfPCell(new Phrase("Extra's", cellFont)) { BackgroundColor = rowColor, Rowspan = 2, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase("• " + formattedExtras, cellFont)) { BackgroundColor = rowColor, Colspan = 2, Padding = 8f });

                }

                // 🌈 Voeg de tabel toe aan het document (De grote onthulling van de tabel)
                document.Add(table);

                // 💰 Totale prijs sectie met flair
                iTextSharp.text.Font totalPriceFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.RED);
                Paragraph totalPriceParagraph = new Paragraph($"💵 Totale Prijs: {totalPrice.ToString("C2", belgianCulture)}", totalPriceFont);
                totalPriceParagraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalPriceParagraph);

                document.Add(new Chunk(new LineSeparator())); // Scheidingsteken om de secties te splitsen

                // 📞 Contactinformatie sectie (Een vriendelijk geheugensteuntje)
                iTextSharp.text.Font footerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.ITALIC, BaseColor.GRAY);
                Paragraph footer = new Paragraph("📞 Bedankt voor uw zaken! Neem contact met ons op als u vragen heeft.", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                footer.SpacingBefore = 10f;
                document.Add(footer);
                document.Add(new Paragraph(" "));

                // 🖊️ Handtekening (met extra charme)
                iTextSharp.text.Font signatureFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph signature = new Paragraph("Met vriendelijke groet, \nIcedOutWheelz", signatureFont);
                signature.Alignment = Element.ALIGN_RIGHT;
                document.Add(signature);

                // 🌟 Finaliseer het document (Epische afwerking!)
                document.Close();
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception ex)
            {
                MessageBox.Show("🔥 Fout bij het genereren van de PDF: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }


        public static void SendEmailWithPdf(string toEmail, string subject, string body, MemoryStream pdfStream)
        {
            try
            {
                // Convert MemoryStream to byte array
                byte[] pdfData = pdfStream.ToArray();

                // Create a new mail message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("icedoutwheelz.2fa@gmail.com"); // Your email address
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true; // Set to true to send HTML content

                // Attach the PDF file
                MemoryStream attachmentStream = new MemoryStream(pdfData);
                mail.Attachments.Add(new Attachment(attachmentStream, "offerte.pdf", "application/pdf"));

                // SMTP server setup
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;  // Or use the appropriate port
                smtp.Credentials = new NetworkCredential("icedoutwheelz.2fa@gmail.com", "kafe mqua douj epah");
                smtp.EnableSsl = true; // Use SSL if required

                // Send the email
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
