using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Math;
using prjIcedOutWheelz.DA;
using prjIcedOutWheelz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjIcedOutWheelz
{
    public partial class frmhoofdpagina : Form
    {
        // DataSet met alle autotypes uit de database
        DataSet dsTypes = AutoDA.TypesOphalen();

        // Dictionary voor snelle toegang tot DataRows op basis van een string key
        private Dictionary<string, DataRow> dataMap = new Dictionary<string, DataRow>();

        public frmhoofdpagina()
        {
            InitializeComponent(); // Initialiseer UI-componenten
            this.CenterToScreen(); // Zet het venster in het midden van het scherm

            // Haal opnieuw alle types op (voor de zekerheid)
            DataSet dsTypes = AutoDA.TypesOphalen(); 
            DataTable dtTypes = dsTypes.Tables[0];

            FillListBoxTypes(dsTypes.Tables[0]); // Vul de ListBox met types
            
            // Unieke types verzamelen voor de combobox
            HashSet<string> uniqueTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in dtTypes.Rows)
            {
                string typeName = row[1].ToString(); // Haal type-naam op
                if (uniqueTypes.Add(typeName)) // Voeg alleen toe als nog niet aanwezig
                {
                    cmbType.Items.Add(typeName); // Voeg type toe aan combobox
                }
            }

            // Controleer of de gebruiker admin is en toon de admin-knop indien nodig
            if (LoginDA.SoortGebruikerCheck(frmLogin.log) == "A")
            {
                btnAdmin.Visible = true;
            }
            else
            {
                btnAdmin.Visible = false;
            }
        }

        // Filtert rijen op basis van de selectie in de combobox
        public DataTable FilterRowsByComboBoxSelection(DataTable originalTable, string selectedValue)
        {
            DataTable filteredTable = originalTable.Clone(); // Maak lege kopie van de structuur

            // Selecteer rijen waar het merk overeenkomt met de selectie
            var filteredRows = from row in originalTable.AsEnumerable()
                               where row.Field<string>("Merk") == selectedValue
                               select row;

            // Voeg gefilterde rijen toe aan de nieuwe tabel
            foreach (var row in filteredRows)
            {
                filteredTable.ImportRow(row);
            }

            return filteredTable;
        }

        // Vult de lijst met types in de ListBox
        public void FillListBoxTypes(DataTable dtTypess)
        {
            DataTable dtTypes = AutoDA.TypesOphalen().Tables[0];

            // Voeg samengestelde kolom toe voor weergave als die nog niet bestaat
            if (!dtTypes.Columns.Contains("DisplayText"))
            {
                dtTypes.Columns.Add("DisplayText", typeof(string), "Merk + ' | ' + Type + ' | ' + Jaar");
            }

            lsbTypes.DataSource = dtTypes; // Koppel data aan ListBox
            lsbTypes.DisplayMember = "DisplayText"; // Toon samengestelde tekst
            lsbTypes.ValueMember = "typeID"; // Gebruik typeID als waarde
        }

        // Open het adminscherm als op de knop wordt geklikt
        private void btnAdmin_Click(object sender, EventArgs e)
        {
            frmAdminscherm frm = new frmAdminscherm(); // Maak nieuw adminscherm
            this.Hide(); // Verberg huidig scherm
            frm.Show(); // Toon adminscherm
        }

        // Vul de motor-combobox op basis van het geselecteerde type
        private void FillComboBoxMotor(int autoTypeID)
        {
            DataSet dsMotorPerAuto = AutoDA.MotorPerAutoOphalen(autoTypeID); // Haal motoren op
            cmbMotor.DataSource = dsMotorPerAuto.Tables[0]; // Koppel aan combobox
            cmbMotor.ValueMember = "MotorPerTypeID"; // Waarde
            cmbMotor.DisplayMember = "MotorType"; // Weergave
        }

        // Vul de kleur-combobox op basis van het geselecteerde type
        private void FillComboBoxKleur(int autoTypeID)
        {
            DataSet dsKleurPerAuto = AutoDA.KleurPerAutoOphalen(autoTypeID); // Haal kleuren op
            cmbKleur.DataSource = dsKleurPerAuto.Tables[0];
            cmbKleur.ValueMember = "KleurPerTypeID";
            cmbKleur.DisplayMember = "Kleur";
        }

        // Vul de status-combobox op basis van het geselecteerde type
        private void FillComboBoxStatus(int autoTypeID)
        {
            DataSet dsStatusPerAuto = AutoDA.StatusPerAutoOphalen(autoTypeID); // Haal statussen op
            DataTable dtTypes = AutoDA.TypesOphalen().Tables[0];
            DataRow typeRow = dtTypes.AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["typeID"]) == autoTypeID);
            string merk = typeRow != null ? typeRow["Merk"].ToString() : string.Empty;

            cmbstatus.Items.Clear(); // Maak combobox leeg

            // Voeg statussen toe aan combobox
            if (dsStatusPerAuto.Tables.Count > 0 && dsStatusPerAuto.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsStatusPerAuto.Tables[0].Rows)
                {
                    cmbstatus.Items.Add(new { Text = row["Status"].ToString(), Value = row["StatusPerTypeID"] });
                }
                cmbstatus.DisplayMember = "Text";
                cmbstatus.ValueMember = "Value";
                cmbstatus.SelectedIndex = 0; // Selecteer eerste status
            }
            // Speciaal geval voor Volvo
            else if (merk.Equals("Volvo", StringComparison.OrdinalIgnoreCase))
            {
                cmbstatus.Items.Add(new { Text = "Nieuw", Value = 0 });
                cmbstatus.DisplayMember = "Text";
                cmbstatus.ValueMember = "Value";
                cmbstatus.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Geen status gevonden voor dit type auto.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Wordt uitgevoerd als een type in de lijst wordt geselecteerd
        private void lsbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsbTypes.SelectedItem != null)
            {
                // Haal geselecteerd type op
                DataRowView drv = (DataRowView)lsbTypes.SelectedItem;
                int selectedAutoTypeID = Convert.ToInt32(drv["typeID"]);

                // Vul comboboxen op basis van selectie
                FillComboBoxMotor(selectedAutoTypeID);
                FillComboBoxKleur(selectedAutoTypeID);
                FillComboBoxStatus(selectedAutoTypeID);
            }

            // Haal index van selectie op
            int selectedIndex = lsbTypes.SelectedIndex;
            DataTable dtListBox = (DataTable)lsbTypes.DataSource;
            int autoType = Convert.ToInt32(dtListBox.Rows[selectedIndex]["typeID"]);

            // Haal foto op van geselecteerd type
            DataSet dsFoto = AutoDA.FotoOphalen(autoType); 

            if (dsFoto.Tables[0].Rows.Count > 0)
            {
                byte[] photoData = dsFoto.Tables[0].Rows[0]["Foto"] as byte[];

                if (photoData != null && photoData.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(photoData))
                        {
                            picAuto.Image = Image.FromStream(ms); // Toon foto
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fout: {ex.Message}", "Fout");
                    }
                }
                else
                {
                    MessageBox.Show("Geen foto beschikbaar", "Geen foto");
                }
            }
            else
            {
                MessageBox.Show("Geen foto gevonden", "Geen foto");
            }
        }

        // Filtert de lijst op basis van het geselecteerde merk in de combobox
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem != null)
            {
                string selectedMerk = cmbType.SelectedItem.ToString();

                // Haal alle types op en filter op merk
                DataTable dtTypes = AutoDA.TypesOphalen().Tables[0];
                DataRow[] filteredRows = dtTypes.Select($"Merk = '{selectedMerk}'");

                DataTable dtFiltered = dtTypes.Clone();
                foreach (DataRow row in filteredRows)
                {
                    dtFiltered.ImportRow(row);
                }

                // Voeg samengestelde kolom toe als die nog niet bestaat
                if (!dtFiltered.Columns.Contains("DisplayText"))
                {
                    dtFiltered.Columns.Add("DisplayText", typeof(string), "Merk + ' | ' + Type + ' | ' + Jaar");
                }

                lsbTypes.DataSource = dtFiltered;
                lsbTypes.DisplayMember = "DisplayText";
                lsbTypes.ValueMember = "typeID";
            }
        }

        // Toont motorgegevens van de geselecteerde auto en motor
        private void cmbMotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drvMotorType = (DataRowView)cmbMotor.SelectedItem;
            int selectedMotorTypeID = Convert.ToInt32(drvMotorType[0]);

            DataRowView drvAutoType = (DataRowView)lsbTypes.SelectedItem;
            int selectedAutoTypeID = Convert.ToInt32(drvAutoType[0]);

            // Haal motorgegevens op
            DataTable dt = AutoDA.AutoInfoOphalen(selectedAutoTypeID, selectedMotorTypeID).Tables[0];

            List<string> listItems = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                // Maak een leesbare string van de motorgegevens
                string displayText = $"{row["Merk"]}  |  {row["Type"]}  |  {row["Jaar"]}  |  {row["MotorType"]}  |  {row["BrandstofType"]}  |  {row["Vermogen"]}  |  {row["Koppel"]}  |  {row["Batterijcapaciteit"]}";
                listItems.Add(displayText);
            }
            lsbInformatie.DataSource = listItems; // Toon info in ListBox
        }

        // Verwerkt het aanvragen van een offerte en stuurt deze per mail
        private void btnVraagOfferte_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            L = frmLogin.log; // Haal ingelogde gebruiker op

            if (lsbTypes.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)lsbTypes.SelectedItem;

                // Haal geselecteerde kleur op
                string kleurNaam = cmbKleur.SelectedItem != null ? ((DataRowView)cmbKleur.SelectedItem)["Kleur"].ToString() : string.Empty;
                int kleurID = AutoDA.GetKleurID(kleurNaam);

                // Haal geselecteerde motor op
                string motorNaam = cmbMotor.SelectedItem != null ? ((DataRowView)cmbMotor.SelectedItem)["MotorType"].ToString() : string.Empty;
                int motorID = AutoDA.GetMotorID(motorNaam);

                // Haal geselecteerde status op
                string statusNaam = string.Empty;
                if (cmbstatus.SelectedItem != null)
                {
                    if (cmbstatus.SelectedItem is DataRowView drvStatus)
                    {
                        statusNaam = drvStatus["Status"].ToString();
                    }
                    else
                    {
                        var prop = cmbstatus.SelectedItem.GetType().GetProperty("Text");
                        if (prop != null)
                        {
                            statusNaam = prop.GetValue(cmbstatus.SelectedItem, null)?.ToString() ?? string.Empty;
                        }
                    }
                }
                int statusID = AutoDA.GetStatusID(statusNaam);

                // Lees extra opties uit
                bool stuurVerwarming = chkStuurVerwarming.Checked;
                bool cruiseControl = chkCruiseControl.Checked;
                bool zetelverwarming = chkZetelverwarming.Checked;
                bool parkeersensoren = chkParkeersensoren.Checked;
                bool trekHaak = chkTrekhaak.Checked;
                bool xenonlampen = chkXenonLamp.Checked;
                bool geblindeerdeRamen = chkGeblindeerdeRamen.Checked;

                int typeID = Convert.ToInt32(drv["TypeID"]);

                // Haal prijs van het type op uit de database
                decimal typePrijs = 0;
                using (var conn = prjIcedOutWheelz.Helper.Database.MakeConnection())
                {
                    string sql = "SELECT Prijs FROM tbltype WHERE typeID = @typeID LIMIT 1";
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@typeID", typeID);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            typePrijs = Convert.ToDecimal(result);
                    }
                }

                // Haal prijs van geselecteerde extra's op
                decimal extrasTotal = 0;
                using (var conn = prjIcedOutWheelz.Helper.Database.MakeConnection())
                {
                    string sql = "SELECT ExtraID, Prijs FROM tblextras";
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                    using (var da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                    {
                        DataTable dtExtras = new DataTable();
                        da.Fill(dtExtras);

                        if (stuurVerwarming)
                            extrasTotal += GetExtraPrice(dtExtras, 1);
                        if (cruiseControl)
                            extrasTotal += GetExtraPrice(dtExtras, 2);
                        if (zetelverwarming)
                            extrasTotal += GetExtraPrice(dtExtras, 3);
                        if (parkeersensoren)
                            extrasTotal += GetExtraPrice(dtExtras, 4);
                        if (trekHaak)
                            extrasTotal += GetExtraPrice(dtExtras, 5);
                        if (xenonlampen)
                            extrasTotal += GetExtraPrice(dtExtras, 6);
                        if (geblindeerdeRamen)
                            extrasTotal += GetExtraPrice(dtExtras, 7);
                    }
                }

                // Bereken totale prijs
                decimal totalPrice = typePrijs + extrasTotal;

                // Sla offerte op in de database
                AutoDA.AutoOfferteAanmaken(
                    Convert.ToDouble(totalPrice),
                    kleurID,
                    statusID,
                    typeID,
                    motorID,
                    stuurVerwarming,
                    cruiseControl,
                    zetelverwarming,
                    parkeersensoren,
                    trekHaak,
                    xenonlampen,
                    geblindeerdeRamen
                );

                MessageBox.Show("Offerte succesvol aangemaakt!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Selecteer eerst een auto type uit de lijst.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Haal details van de offerte op en stuur per e-mail
            DataTable offerteDetails = OfferteDA.OfferteDetailsOphalen().Tables[0];

            if (offerteDetails.Rows.Count > 0)
            {
                MemoryStream pdfStream = OfferteDA.GeneratePdfInMemory(offerteDetails);
                if (pdfStream != null)
                {
                    string toEmail = L.Email;
                    string subject = "Uw offerte";
                    string body = "<h3>Beste klant,</h3><p>In de bijlage kunt u uw offerte vinden zie u zonet had opgevraagt.</p><br><p>Bij verdere vragen kunt u contact met ons opnemen via het\nemail: icedoutwheelz@gmail.com\nOf via ons telefoonnummer +32494597173</p><br><p>Bedankt voor uw interesse!</p><p>Met vriendelijke groet\nIcedOutWheelZz</p>";

                    OfferteDA.SendEmailWithPdf(toEmail, subject, body, pdfStream); // Stuur naar klant
                    OfferteDA.SendEmailWithPdf("icedoutwheelz@gmail.com", subject, body, pdfStream); // Stuur naar bedrijf
                }
            }
            else
            {
                MessageBox.Show("Geen offertetetails gevonden voor deze auto!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Haalt de prijs van een extra op basis van het ID uit de DataTable
        private decimal GetExtraPrice(DataTable dtExtras, int extraID)
        {
            var row = dtExtras.AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["ExtraID"]) == extraID);
            return row != null && row["Prijs"] != DBNull.Value ? Convert.ToDecimal(row["Prijs"]) : 0;
        }

        // Logt uit en keert terug naar het startscherm
        private void btnLoguit_Click(object sender, EventArgs e)
        {
            frmStartscherm frmStartscherm = new frmStartscherm();
            frmStartscherm.Show();
            this.Close();
        }

        private void frmhoofdpagina_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmStartscherm frm = new frmStartscherm();
            this.Hide();
            frm.Show();
        }
    }
}
