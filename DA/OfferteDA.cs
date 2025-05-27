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
    internal class OfferteDA
    {
        // Haalt de details van de meest recente auto-offerte op uit de database
        public static DataSet OfferteDetailsOphalen()
        {
            DataSet dsOfferteDetails = new DataSet();

            // SQL-query die alle relevante gegevens van de laatste offerte ophaalt, inclusief prijs, kleur, status, type, merk, motor en eventuele extra's
            string sql = @"
                SELECT 
                    ao.AutoOfferteID, 
                    t.Prijs AS TypePrijs, 
                    k.Kleur AS Kleur, 
                    s.Status AS Status, 
                    t.Type AS Type, 
                    t.Merk AS Merk, 
                    m.MotorType AS Motor,
                    GROUP_CONCAT(e.Omschrijving ORDER BY e.ExtraID) AS Extras,
                    GROUP_CONCAT(e.Prijs ORDER BY e.ExtraID) AS ExtraPrijzen
                FROM tblautoofferte ao 
                INNER JOIN tblkleuren k ON ao.KleurID = k.KleurID 
                INNER JOIN tblstatus s ON ao.StatusID = s.StatusID 
                INNER JOIN tbltype t ON ao.TypeID = t.TypeID
                INNER JOIN tblmotoren m ON ao.MotorID = m.MotorID 
                LEFT JOIN tblextras e 
                    ON (ao.Stuurverwarming = 1 AND e.ExtraID = 1)
                    OR (ao.CruiseControl = 1 AND e.ExtraID = 2)
                    OR (ao.Zetelverwarming = 1 AND e.ExtraID = 3)
                    OR (ao.Parkeersensoren = 1 AND e.ExtraID = 4)
                    OR (ao.Trekhaak = 1 AND e.ExtraID = 5)
                    OR (ao.Xenonlampen = 1 AND e.ExtraID = 6)
                    OR (ao.GeblindeerdeRamen = 1 AND e.ExtraID = 7)
                WHERE ao.AutoOfferteID = (SELECT MAX(AutoOfferteID) FROM tblautoofferte);";
            try
            {
                // Maak verbinding met de database en vul het DataSet met de resultaten van de query
                using (MySqlConnection conn = Database.MakeConnection())
                {
                    MySqlDataAdapter daOfferteDetails = new MySqlDataAdapter(sql, conn);

                    daOfferteDetails.Fill(dsOfferteDetails);
                }
            }
            catch (Exception ex)
            {
                // Toon een foutmelding als er iets misgaat bij het ophalen van de gegevens
                MessageBox.Show(ex.Message);
            }

            return dsOfferteDetails;
        }

        /// <summary>
        /// Genereert een offerte-PDF in het geheugen op basis van de opgegeven details.
        /// </summary>
        /// <param name="details">DataTable met offertetdetails (één rij)</param>
        /// <returns>MemoryStream met de PDF (klaar om te versturen of op te slaan)</returns>
        public static MemoryStream GeneratePdfInMemory(DataTable details)
        {
            try
            {
                // Maak een MemoryStream aan waarin de PDF in het geheugen wordt opgebouwd
                MemoryStream memoryStream = new MemoryStream();

                // Maak een nieuw PDF-document aan en koppel deze aan de MemoryStream
                Document document = new Document(PageSize.A4, 40f, 40f, 50f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                writer.CloseStream = false; // Laat de stream open zodat we deze later kunnen uitlezen

                // Open het document zodat we er inhoud aan kunnen toevoegen
                document.Open();

                // --- LOGO TOEVOEGEN ---
                try
                {
                    // Zet het logo uit de resources om naar een byte-array en voeg toe aan de PDF
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Properties.Resources.Logo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageBytes);
                        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        logo.ScaleToFit(150f, 150f);

                        document.Add(logo);
                        document.Add(new Paragraph(" ")); // Voeg witruimte toe
                    }
                }
                catch (Exception ex)
                {
                    // Als het logo niet kan worden toegevoegd, toon een waarschuwing maar ga verder
                    MessageBox.Show("Fout bij het toevoegen van logo: " + ex.Message, "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // --- TITEL EN SUBTITEL ---
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 22, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                Paragraph title = new Paragraph("Uw offerte", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph(" ")); // Extra witruimte

                iTextSharp.text.Font subheadingFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph subheading = new Paragraph("The Road is Calling - Answer in Style!", subheadingFont);
                subheading.Alignment = Element.ALIGN_CENTER;
                document.Add(subheading);
                document.Add(new Chunk(new LineSeparator())); // Lijn onder de subtitel

                // --- DETAILTABEL ---
                // Maak een tabel met 2 kolommen: Kenmerk | Waarde
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SpacingBefore = 10f;
                table.SpacingAfter = 20f;
                table.SetWidths(new float[] { 1.5f, 3f });

                // Tabelkoppen (donkerblauw met witte tekst)
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                BaseColor darkNavy = new BaseColor(0, 0, 102);

                PdfPCell header1 = new PdfPCell(new Phrase("Kenmerk", headerFont)) { BackgroundColor = darkNavy, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8f };
                PdfPCell header2 = new PdfPCell(new Phrase("Waarde", headerFont)) { BackgroundColor = darkNavy, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8f };
                table.AddCell(header1);
                table.AddCell(header2);

                decimal totalPrice = 0; // Houd de totale prijs bij
                CultureInfo belgianCulture = new CultureInfo("nl-BE"); // Belgische euroformattering

                // Pas het euro-formaat aan voor Belgische notatie
                NumberFormatInfo nbnFormat = (NumberFormatInfo)belgianCulture.NumberFormat.Clone();
                nbnFormat.CurrencyGroupSeparator = " ";
                nbnFormat.CurrencyDecimalSeparator = ",";
                nbnFormat.CurrencySymbol = "€";
                nbnFormat.CurrencyPositivePattern = 3; // € n

                bool alternateRow = false; // Voor afwisselende rij-kleuren

                // Loop door de DataTable (meestal 1 rij) en vul de tabel
                foreach (DataRow row in details.Rows)
                {
                    iTextSharp.text.Font cellFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    BaseColor rowColor = alternateRow ? new BaseColor(240, 240, 240) : BaseColor.WHITE;
                    alternateRow = !alternateRow;

                    // Ordernummer
                    table.AddCell(new PdfPCell(new Phrase("Order Nummer", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["AutoOfferteID"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Auto (merk + type)
                    table.AddCell(new PdfPCell(new Phrase("Auto", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase($"{row["Merk"]} {row["Type"]}", cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Prijsberekening
                    decimal basePrice = row["TypePrijs"] != DBNull.Value ? Convert.ToDecimal(row["TypePrijs"]) : 0;
                    decimal extrasTotal = 0;
                    if (row.Table.Columns.Contains("ExtraPrijzen") && row["ExtraPrijzen"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["ExtraPrijzen"].ToString()))
                    {
                        var extraPrices = row["ExtraPrijzen"].ToString().Split(',')
                            .Select(p => decimal.TryParse(p, out var val) ? val : 0);
                        extrasTotal = extraPrices.Sum();
                    }
                    totalPrice = basePrice + extrasTotal;

                    // Prijs
                    table.AddCell(new PdfPCell(new Phrase("Prijs (€)", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(basePrice.ToString("C2", nbnFormat), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Kleur
                    table.AddCell(new PdfPCell(new Phrase("Kleur", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Kleur"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Status
                    table.AddCell(new PdfPCell(new Phrase("Status", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Status"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Motor
                    table.AddCell(new PdfPCell(new Phrase("Motor", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Motor"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Extra's (met prijs per extra)
                    string extras = row["Extras"] != DBNull.Value ? row["Extras"].ToString() : "";
                    string extraPrijzen = row.Table.Columns.Contains("ExtraPrijzen") && row["ExtraPrijzen"] != DBNull.Value
                        ? row["ExtraPrijzen"].ToString()
                        : "";

                    string formattedExtras = "";
                    if (!string.IsNullOrWhiteSpace(extras) && !string.IsNullOrWhiteSpace(extraPrijzen))
                    {
                        var extrasList = extras.Split(',').Select(e => e.Trim()).ToList();
                        var prijzenList = extraPrijzen.Split(',').Select(p =>
                        {
                            decimal prijs;
                            return decimal.TryParse(p, out prijs) ? prijs : 0;
                        }).ToList();

                        var lines = new List<string>();
                        for (int i = 0; i < extrasList.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(extrasList[i]))
                            {
                                string prijsStr = (i < prijzenList.Count)
                                    ? prijzenList[i].ToString("C2", nbnFormat)
                                    : "";
                                lines.Add($"• {extrasList[i]}: {prijsStr}");
                            }
                        }
                        formattedExtras = string.Join("\n", lines);
                    }
                    else if (!string.IsNullOrWhiteSpace(extras))
                    {
                        // fallback: alleen namen tonen
                        formattedExtras = string.Join("\n• ", extras.Split(',').Select(extra => extra.Trim()));
                    }

                    // Voeg de extra's toe aan de tabel
                    table.AddCell(new PdfPCell(new Phrase("Extra's", cellFont)) { BackgroundColor = rowColor, Rowspan = 2, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(formattedExtras, cellFont)) { BackgroundColor = rowColor, Colspan = 2, Padding = 8f });
                }

                // Voeg de tabel toe aan het document
                document.Add(table);

                // --- TOTALE PRIJS ---
                iTextSharp.text.Font totalPriceFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.RED);
                Paragraph totalPriceParagraph = new Paragraph($"Totale Prijs: {totalPrice.ToString("C2", nbnFormat)}", totalPriceFont);
                totalPriceParagraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalPriceParagraph);

                document.Add(new Chunk(new LineSeparator())); // Scheidingsteken

                // --- CONTACTINFORMATIE ---
                iTextSharp.text.Font footerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.ITALIC, BaseColor.GRAY);
                Paragraph footer = new Paragraph("Bedankt voor uw interesse! Neem contact met ons op als u vragen heeft. Via het telefoonnummer +32 494 59 71 73 of het e-mailadres icedoutwheelz@gmail.com ", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                footer.SpacingBefore = 10f;
                document.Add(footer);
                document.Add(new Paragraph(" "));

                // --- HANDTEKENING ---
                iTextSharp.text.Font signatureFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph signature = new Paragraph("Met vriendelijke groet, \nIcedOutWheelz", signatureFont);
                signature.Alignment = Element.ALIGN_RIGHT;
                document.Add(signature);

                // Sluit het document af en zet de stream terug naar het begin
                document.Close();
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het genereren van de PDF: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Verstuurt een e-mail met een PDF-bijlage naar het opgegeven e-mailadres.
        /// </summary>
        /// <param name="toEmail">Bestemmeling</param>
        /// <param name="subject">Onderwerp</param>
        /// <param name="body">HTML-body van de e-mail</param>
        /// <param name="pdfStream">MemoryStream met de PDF-bijlage</param>
        public static void SendEmailWithPdf(string toEmail, string subject, string body, MemoryStream pdfStream)
        {
            try
            {
                // Zet de MemoryStream om naar een byte-array voor de bijlage
                byte[] pdfData = pdfStream.ToArray();

                // Maak een nieuw e-mailbericht aan
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("icedoutwheelz.2fa@gmail.com"); // Afzender
                mail.To.Add(toEmail); // Ontvanger
                mail.Subject = subject; // Onderwerp
                mail.Body = body; // HTML-body
                mail.IsBodyHtml = true; // Geef aan dat de body HTML is

                // Voeg de PDF als bijlage toe aan de e-mail
                MemoryStream attachmentStream = new MemoryStream(pdfData);
                mail.Attachments.Add(new Attachment(attachmentStream, "offerte.pdf", "application/pdf"));

                // Stel de SMTP-server in (Gmail in dit geval)
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;  // Standaard poort voor TLS
                smtp.Credentials = new NetworkCredential("icedoutwheelz.2fa@gmail.com", "kafe mqua douj epah");
                smtp.EnableSsl = true; // Gebruik SSL/TLS

                // Verstuur de e-mail met bijlage
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                // Toon een foutmelding als het versturen mislukt
                MessageBox.Show("Error sending email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
