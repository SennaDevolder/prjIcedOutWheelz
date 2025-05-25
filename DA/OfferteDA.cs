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
                    MessageBox.Show("Fout bij het toevoegen van logo: " + ex.Message, "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Titel
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 22, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                Paragraph title = new Paragraph("Uw offerte", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph(" ")); // Ruimte tussen de titel en inhoud

                // Subtitel
                iTextSharp.text.Font subheadingFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph subheading = new Paragraph("The Road is Calling - Answer in Style!", subheadingFont);
                subheading.Alignment = Element.ALIGN_CENTER;
                document.Add(subheading);
                document.Add(new Chunk(new LineSeparator()));

                // Maak de detailtabel
                PdfPTable table = new PdfPTable(2); // Twee kolommen: Kenmerk, Waarde
                table.WidthPercentage = 100;
                table.SpacingBefore = 20f; // Ruimte voor de tabel
                table.SpacingAfter = 20f;  // Ruimte na de tabel
                table.SetWidths(new float[] { 1.5f, 3f });

                // Tabelkoppen
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                BaseColor darkNavy = new BaseColor(0, 0, 102); // Donker marineblauw

                PdfPCell header1 = new PdfPCell(new Phrase("Kenmerk", headerFont)) { BackgroundColor = darkNavy, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8f };
                PdfPCell header2 = new PdfPCell(new Phrase("Waarde", headerFont)) { BackgroundColor = darkNavy, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8f };
                table.AddCell(header1);
                table.AddCell(header2);

                decimal totalPrice = 0; // Houd de totale prijs bij
                CultureInfo belgianCulture = new CultureInfo("nl-BE"); // Belgische euroformattering

                // Clone and customize NumberFormat for NBN
                NumberFormatInfo nbnFormat = (NumberFormatInfo)belgianCulture.NumberFormat.Clone();
                nbnFormat.CurrencyGroupSeparator = " "; // space for thousands
                nbnFormat.CurrencyDecimalSeparator = ","; // comma for decimals
                nbnFormat.CurrencySymbol = "€";
                nbnFormat.CurrencyPositivePattern = 3; // € n

                bool alternateRow = false; // Wisselende rijkleuren

                // Loop door de DataTable en vul de cellen in
                foreach (DataRow row in details.Rows)
                {
                    iTextSharp.text.Font cellFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    BaseColor rowColor = alternateRow ? new BaseColor(240, 240, 240) : BaseColor.WHITE;
                    alternateRow = !alternateRow;

                    table.AddCell(new PdfPCell(new Phrase("Order Nummer", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["AutoOfferteID"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Auto", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase($"{row["Merk"]} {row["Type"]}", cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Calculate total price
                    decimal basePrice = row["TypePrijs"] != DBNull.Value ? Convert.ToDecimal(row["TypePrijs"]) : 0;
                    decimal extrasTotal = 0;
                    if (row.Table.Columns.Contains("ExtraPrijzen") && row["ExtraPrijzen"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["ExtraPrijzen"].ToString()))
                    {
                        var extraPrices = row["ExtraPrijzen"].ToString().Split(',')
                            .Select(p => decimal.TryParse(p, out var val) ? val : 0);
                        extrasTotal = extraPrices.Sum();
                    }
                    totalPrice = basePrice + extrasTotal;

                    table.AddCell(new PdfPCell(new Phrase("Prijs (€)", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(totalPrice.ToString("C2", nbnFormat), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Kleur", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Kleur"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Status", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Status"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    table.AddCell(new PdfPCell(new Phrase("Motor", cellFont)) { BackgroundColor = rowColor, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(row["Motor"].ToString(), cellFont)) { BackgroundColor = rowColor, Padding = 8f });

                    // Extras
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

                        var culture = belgianCulture;
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
                        // fallback: show only names
                        formattedExtras = string.join("\n• ", extras.Split(',').Select(extra => extra.Trim()));
                    }

                    table.AddCell(new PdfPCell(new Phrase("Extra's", cellFont)) { BackgroundColor = rowColor, Rowspan = 2, Padding = 8f });
                    table.AddCell(new PdfPCell(new Phrase(formattedExtras, cellFont)) { BackgroundColor = rowColor, Colspan = 2, Padding = 8f });
                }

                // Voeg de tabel toe aan het document
                document.Add(table);

                // Totale prijs sectie
                iTextSharp.text.Font totalPriceFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.RED);
                Paragraph totalPriceParagraph = new Paragraph($"Totale Prijs: {totalPrice.ToString("C2", nbnFormat)}", totalPriceFont);
                totalPriceParagraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalPriceParagraph);

                document.Add(new Chunk(new LineSeparator())); // Scheidingsteken om de secties te splitsen

                // Contactinformatie sectie
                iTextSharp.text.Font footerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.ITALIC, BaseColor.GRAY);
                Paragraph footer = new Paragraph("Bedankt voor uw zaken! Neem contact met ons op als u vragen heeft. Via het telefoonnummer +32 494 59 71 73 of het e-mailadres icedoutwheelz@gmail.com ", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                footer.SpacingBefore = 10f;
                document.Add(footer);
                document.Add(new Paragraph(" "));

                // Handtekening
                iTextSharp.text.Font signatureFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph signature = new Paragraph("Met vriendelijke groet, \nIcedOutWheelz", signatureFont);
                signature.Alignment = Element.ALIGN_RIGHT;
                document.Add(signature);

                // Finaliseer het document 
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
