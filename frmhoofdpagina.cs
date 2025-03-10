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
        /*  
         *  TODO:
         *  - Fix dubbel Fiat en Porsche in cmbType (filter)
         *  - Fix geen status weergegeven on selection Volvo EX90 --> StatusID = 0 in DataBase in tblautoofferte
         *  - Merk toevoegen in DataTable (details uit pdf creator AutoDA) om extra rij in PDF te hebben met Merk van voertuig
         *  - 
        */





        DataSet dsTypes = AutoDA.TypesOphalen();
        private Dictionary<string, DataRow> dataMap = new Dictionary<string, DataRow>();


        public frmhoofdpagina()
        {
            InitializeComponent();
            this.CenterToScreen();

            DataSet dsTypes = AutoDA.TypesOphalen(); 
            DataTable dtTypes = dsTypes.Tables[0];

            FillListBoxTypes(dsTypes.Tables[0]);

            foreach (DataRow row in dtTypes.Rows)
            {
                cmbType.Items.Add(row[1].ToString());
            }

            if (LoginDA.SoortGebruikerCheck(frmLogin.log) == "A")
            {
                btnAdmin.Visible = true;
            }
            else
            {
                btnAdmin.Visible = false;
            }
        }

        public DataTable FilterRowsByComboBoxSelection(DataTable originalTable, string selectedValue)
        {
            // Create a new DataTable that will contain the filtered rows
            DataTable filteredTable = originalTable.Clone(); // Clone the structure of the original table (without data)

            // Use LINQ or DataTable.Select() to filter the rows where the column value matches the selected value from ComboBox
            var filteredRows = from row in originalTable.AsEnumerable()
                               where row.Field<string>("Merk") == selectedValue // Replace "Name" with your actual column name
                               select row;

            // Import the filtered rows into the new DataTable
            foreach (var row in filteredRows)
            {
                filteredTable.ImportRow(row);
            }

            return filteredTable;
        }

        public void FillListBoxTypes(DataTable dtTypess)
        {
            // Get DataTable
            DataTable dtTypes = AutoDA.TypesOphalen().Tables[0];

            // Add a new computed column for the combined display text (Merk | Type | Jaar)
            if (!dtTypes.Columns.Contains("DisplayText"))
            {
                dtTypes.Columns.Add("DisplayText", typeof(string), "Merk + ' | ' + Type + ' | ' + Jaar");
            }

            // Bind directly to DataTable
            lsbTypes.DataSource = dtTypes;
            lsbTypes.DisplayMember = "DisplayText";  // Show the combined text (Merk | Type | Jaar)
            lsbTypes.ValueMember = "typeID";  // Store the unique ID (typeID or other unique identifier)

        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            frmAdminscherm frm = new frmAdminscherm();
            this.Hide();
            frm.Show();
        }

        private void FillComboBoxMotor(int autoTypeID)
        {
            DataSet dsMotorPerAuto = AutoDA.MotorPerAutoOphalen(autoTypeID);
            cmbMotor.DataSource = dsMotorPerAuto.Tables[0];
            cmbMotor.ValueMember = "MotorPerTypeID";
            cmbMotor.DisplayMember = "MotorType";
        }

        private void FillComboBoxKleur(int autoTypeID)
        {
            DataSet dsKleurPerAuto = AutoDA.KleurPerAutoOphalen(autoTypeID);
            cmbKleur.DataSource = dsKleurPerAuto.Tables[0];
            cmbKleur.ValueMember = "KleurPerTypeID";
            cmbKleur.DisplayMember = "Kleur";
        }

        private void FillComboBoxStatus(int autoTypeID)
        {
            DataSet dsStatusPerAuto = AutoDA.StatusPerAutoOphalen(autoTypeID);
            cmbstatus.DataSource = dsStatusPerAuto.Tables[0];
            cmbstatus.ValueMember = "StatusPerTypeID";
            cmbstatus.DisplayMember = "Status";
        }

        private void lsbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsbTypes.SelectedItem != null)
            {
                // Cast to DataRowView (since ListBox is now bound to DataTable)
                DataRowView drv = (DataRowView)lsbTypes.SelectedItem;
                int selectedAutoTypeID = Convert.ToInt32(drv["typeID"]);

                // Fill related ComboBoxes
                FillComboBoxMotor(selectedAutoTypeID);
                FillComboBoxKleur(selectedAutoTypeID);
                FillComboBoxStatus(selectedAutoTypeID);
            }


            // Index van auto in listbox ophalen
            int selectedIndex = lsbTypes.SelectedIndex;

            // DataTable halen uit listbox
            DataTable dtListBox = (DataTable)lsbTypes.DataSource;

            // AutoType waarde uit DataTable halen (eerste rij zijn indexen)

            int autoType = Convert.ToInt32(dtListBox.Rows[selectedIndex]["typeID"]);

            // Foto ophalen uit DA
            DataSet dsFoto = AutoDA.FotoOphalen(autoType); 

            // Chekcen of DataSet rijen heeft
            if (dsFoto.Tables[0].Rows.Count > 0)
            {
                // Foto data uit DataSet halen
                byte[] photoData = dsFoto.Tables[0].Rows[0]["Foto"] as byte[];

                if (photoData != null && photoData.Length > 0)
                {
                    try
                    {
                        // Foto laden
                        using (MemoryStream ms = new MemoryStream(photoData))
                        {
                            picAuto.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Converteer error
                        MessageBox.Show($"Error: {ex.Message}", "Error");
                    }
                }
                else
                {
                    // Error als er geen foto is
                    MessageBox.Show("Geen foto beschikbaar", "Geen foto");
                }
            }
            else
            {
                // Error als er geen rij gereturnedt wordt uit DataSet
                MessageBox.Show("Geen foto gevonden", "Geen foto");
            }

        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem != null)
            {
                string selectedMerk = cmbType.SelectedItem.ToString(); // Get the selected brand

                // Get the original DataTable
                DataTable dtTypes = AutoDA.TypesOphalen().Tables[0];

                // Filter the DataTable based on the selected "Merk"
                DataRow[] filteredRows = dtTypes.Select($"Merk = '{selectedMerk}'");

                // Create a new DataTable to hold the filtered rows
                DataTable dtFiltered = dtTypes.Clone();  // Clone the structure of the original DataTable
                foreach (DataRow row in filteredRows)
                {
                    dtFiltered.ImportRow(row);  // Import the filtered rows
                }

                // Add the "DisplayText" column to the filtered DataTable
                if (!dtFiltered.Columns.Contains("DisplayText"))
                {
                    dtFiltered.Columns.Add("DisplayText", typeof(string), "Merk + ' | ' + Type + ' | ' + Jaar");
                }

                // Bind the filtered DataTable to the ListBox
                lsbTypes.DataSource = dtFiltered;
                lsbTypes.DisplayMember = "DisplayText";  // Show the combined text (Merk | Type | Jaar)
                lsbTypes.ValueMember = "typeID";  // Store the unique ID (typeID or other unique identifier)
            }
        }

        private void cmbMotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drvMotorType = (DataRowView)cmbMotor.SelectedItem;
            int selectedMotorTypeID = Convert.ToInt32(drvMotorType[0]);

            DataRowView drvAutoType = (DataRowView)lsbTypes.SelectedItem;
            int selectedAutoTypeID = Convert.ToInt32(drvAutoType[0]);

            // Assume dsAutoInfo is your DataSet
            DataTable dt = AutoDA.AutoInfoOphalen(selectedAutoTypeID, selectedMotorTypeID).Tables[0];

            // Create a new list to store formatted rows
            List<string> listItems = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                // Format each row into a readable string
                string displayText = $"{row["Merk"]}  |  {row["Type"]}  |  {row["Jaar"]}  |  {row["MotorType"]}  |  {row["BrandstofType"]}  |  {row["Vermogen"]}  |  {row["Koppel"]}  |  {row["Batterijcapaciteit"]}";

                // Add the formatted string to the list
                listItems.Add(displayText);
            }

            // Bind the list to the ListBox
            lsbInformatie.DataSource = listItems;

        }

        private void btnVraagOfferte_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            L = frmLogin.log;

            // Retrieve the selected item from ListBox (lsbTypes)
            if (lsbTypes.SelectedItem != null)
            {
                // Cast to DataRowView (since ListBox is now bound to DataTable)
                DataRowView drv = (DataRowView)lsbTypes.SelectedItem;

                // Extract values from the selected DataRow (from ListBox)
                string merk = drv["Merk"].ToString();
                string type = drv["Type"].ToString();
                string jaar = drv["Jaar"].ToString();

                // Retrieve values from ComboBoxes
                string kleurNaam = cmbKleur.SelectedItem != null ? ((DataRowView)cmbKleur.SelectedItem)["Kleur"].ToString() : string.Empty;
                int kleurID = AutoDA.GetKleurID(kleurNaam); // Roep de GetKleurID methode aan met de naam

                string motorNaam = cmbMotor.SelectedItem != null ? ((DataRowView)cmbMotor.SelectedItem)["MotorType"].ToString() : string.Empty;
                int motorID = AutoDA.GetMotorID(motorNaam); // Roep de GetMotorID methode aan met de naam

                string statusNaam = cmbstatus.SelectedItem != null ? ((DataRowView)cmbstatus.SelectedItem)["Status"].ToString() : string.Empty;
                int statusID = AutoDA.GetStatusID(statusNaam); // Roep de GetStatusID methode aan met de naam

                // Retrieve checkbox values (checked or unchecked)
                bool stuurVerwarming = chkStuurVerwarming.Checked;
                bool cruiseControl = chkCruiseControl.Checked;
                bool zetelverwarming = chkZetelverwarming.Checked;
                bool parkeersensoren = chkParkeersensoren.Checked;
                bool trekHaak = chkTrekhaak.Checked;
                bool xenonlampen = chkXenonLamp.Checked;
                bool geblindeerdeRamen = chkGeblindeerdeRamen.Checked;

                // Assuming you have the TypeID retrieved from other parts of the app
                int typeID = Convert.ToInt32(drv["TypeID"]);

                // Insert this data into a new table in your database.
                AutoDA.AutoOfferteAanmaken(100000, kleurID, statusID, typeID, motorID, stuurVerwarming, cruiseControl, zetelverwarming, parkeersensoren, trekHaak, xenonlampen, geblindeerdeRamen);

                // Display success message
                MessageBox.Show("Offerte succesvol aangemaakt!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Display error message if no item is selected in the ListBox
                MessageBox.Show("Selecteer eerst een auto type uit de lijst.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DataTable offerteDetails = AutoDA.OfferteDetailsOphalen().Tables[0];

            if (offerteDetails.Rows.Count > 0)
            {
                // Generate the PDF in memory
                MemoryStream pdfStream = AutoDA.GeneratePdfInMemory(offerteDetails); // Adjust the method name if necessary

                if (pdfStream != null)
                {
                    // Send email with the generated PDF as an attachment
                    string toEmail = L.Email;
                    string subject = "Your Offerte Details";
                    string body = "<h3>Dear Customer,</h3><p>Attached are the details of your offerte.</p>";

                    // Send the email with PDF attachment
                    AutoDA.SendEmailWithPdf(toEmail, subject, body, pdfStream);
                }
            }
            else
            {
                MessageBox.Show("No offerte details found for this car!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
