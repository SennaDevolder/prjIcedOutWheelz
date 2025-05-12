using Microsoft.VisualBasic;
using MySql.Data;
using prjIcedOutWheelz.DA;
using prjIcedOutWheelz.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjIcedOutWheelz
{
    public partial class frmAdminscherm : Form
    {
        /*
         *  TODO:
         * - informatie weergeven in lsbinfo. Enkel selected auto EN selected engine, niet allemaal!
         * - Engine selection filter cmb toevoegen
         * - 
         */



        string strMerk, strType, strKleur, strMotorvermogen, strBrandstof, strextras, stropvang;
        int intBouwjaar;
        double dblPrijs;

        public frmAdminscherm()
        {
            InitializeComponent();
            this.CenterToScreen();

            DataSet dsTypes = AutoDA.TypesOphalen();
            DataTable dtTypes = dsTypes.Tables[0];

            FillListBoxTypes();
        }

        public void FillListBoxTypes()
        {
            // Get DataTable
            DataTable dtTypes = AutoDA.TypesOphalen().Tables[0];

            // Add a new computed column for the combined display text (Merk | Type | Jaar)
            if (!dtTypes.Columns.Contains("DisplayText"))
            {
                dtTypes.Columns.Add("DisplayText", typeof(string), "Merk + ' | ' + Type + ' | ' + Jaar");
            }

            // Bind directly to DataTable
            lsbautos.DataSource = dtTypes;
            lsbautos.DisplayMember = "DisplayText";  // Show the combined text (Merk | Type | Jaar)
            lsbautos.ValueMember = "typeID";  // Store the unique ID (typeID or other unique identifier)
        }
        private void btnaddimg_Click(object sender, EventArgs e)
        {
            string eindwerkPath = FindEindwerkPath();
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDirectory);

            if (eindwerkPath == null)
            {
                MessageBox.Show("Eindwerk folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string resourcesPath = FindEindwerkPath();

            if (resourcesPath == null)
            {
                MessageBox.Show("Resources folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
                MessageBox.Show("Resources directory created.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select an Image File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(selectedFilePath);

                    string destinationPath = Path.Combine(resourcesPath, fileName);

                    try
                    {
                        File.Copy(selectedFilePath, destinationPath, true); // Overwrite if exists
                        MessageBox.Show($"Image saved to: {destinationPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        picAuto.Image.Dispose();
                        // img in picturebox
                        picAuto.Image = System.Drawing.Image.FromFile(destinationPath);
                        //img aanpassen
                        picAuto.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    catch
                    {
                        MessageBox.Show("This image is already added", "Add" ,MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                   
                }
                else
                {
                    DialogResult resultaat = MessageBox.Show("No file selected.\nWilt u file explorer sluiten", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (resultaat == DialogResult.No)
                    {
                        btnaddimg.PerformClick();
                    }
                }
            }

        }

        private string FindEindwerkPath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDirectory);

            while (dir != null)
            {
                if (dir.GetDirectories("Resources").Any())
                {
                    return Path.Combine(dir.FullName, "Resources");
                }
                dir = dir.Parent;
            }

            return null;
        }

        private void txthooftdpagina_Click(object sender, EventArgs e)
        {
            //openen van de form frmhoofdpagina
            frmhoofdpagina frmhoofdpagina = new frmhoofdpagina();
            frmhoofdpagina.Show();
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            string eindwerkPath = FindEindwerkPath();

            if (eindwerkPath == null)
            {
                MessageBox.Show("Resources folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //maak een openfiledialog
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = eindwerkPath // folder locatie

            };

            //filter op specifieke image types
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";


            //toon dialog en check als de gebruiker een file heeft geselecteerd
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;

                try
                {
                    //Check als file bestaat voor verwijderen
                    if (File.Exists(selectedFile))
                    {
                        picAuto.Image.Dispose();
                        picAuto.Image = Properties.Resources.placeholder_image;
                        File.Delete(selectedFile);
                        MessageBox.Show("File deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("File does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                DialogResult resultaat = MessageBox.Show("No file selected.\nWilt u file explorer sluiten", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultaat == DialogResult.No)
                {
                    btnremove.PerformClick();
                }
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            picAuto.Image.Dispose();
            picAuto.Image = Properties.Resources.placeholder_image;
        }
        private void btnselect_Click(object sender, EventArgs e)
        {
            string eindwerkPath = FindEindwerkPath();

            if (eindwerkPath == null)
            {
                MessageBox.Show("Resources folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //maak OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",

                InitialDirectory = eindwerkPath // folder locatie
            };

            //toon dialog en get result
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //laad img in picturebox
                picAuto.ImageLocation = openFileDialog.FileName;
            }
            else
            {
                DialogResult resultaat = MessageBox.Show("No file selected.\nWilt u file explorer sluiten", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultaat == DialogResult.No)
                {
                    btnselect.PerformClick();
                }
            }

        }


        private void lsbautos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Index van auto in listbox ophalen
            int selectedIndex = lsbautos.SelectedIndex;

            // DataTable halen uit listbox
            DataTable dtListBox = (DataTable)lsbautos.DataSource;

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


        private void btnMerk_Click(object sender, EventArgs e)
        {
            do
            {
                strMerk = Interaction.InputBox("Gelieve het merk van de auto in te vullen", "Merk Auto");

                if (strMerk.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strMerk))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (strMerk.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strMerk)); // Herhaal als er cijfers zijn of als het leeg is

            MessageBox.Show($"Merk opgeslagen: {strMerk}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnModel_Click(object sender, EventArgs e)
        {
            do
            {
                strType = Interaction.InputBox("Gelieve het Type van de auto in te vullen", "Model Auto");

                if (strType.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strType))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (strType.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strType)); // Herhaal als er cijfers zijn of als het leeg is

            MessageBox.Show($"Model opgeslagen: {strType}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBouwjaar_Click(object sender, EventArgs e)
        {
           do
            {
                stropvang = Interaction.InputBox("Wat is de vraagprijs van de auto?", "Prijs");

                // Controleer of de invoer leeg is of niet omgezet kan worden naar een double
                if (string.IsNullOrWhiteSpace(stropvang) || !int.TryParse(stropvang, out intBouwjaar))
                {
                    MessageBox.Show("Gelieve een geldige prijs in te voeren!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

           } while (string.IsNullOrWhiteSpace(stropvang) || !int.TryParse(stropvang, out intBouwjaar)) ;

            MessageBox.Show($"Prijs opgeslagen: {intBouwjaar} EUR", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLoguit_Click(object sender, EventArgs e)
        {
            frmStartscherm frmStartscherm = new frmStartscherm();
            frmStartscherm.Show();
            this.Close();
        }

        private void btnBrandstof_Click(object sender, EventArgs e)
        {
            do
            {
                strBrandstof = Interaction.InputBox("Gelieve het brandstof van de auto in te vullen", "Brandstof Auto");

                if (strBrandstof.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strBrandstof))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (strBrandstof.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strBrandstof)); // Herhaal als er cijfers zijn of als het leeg is

            MessageBox.Show($"Brandstof opgeslagen: {strBrandstof}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            
        }

        private void Motorvermogen_Click(object sender, EventArgs e)
        {
            do
            {
                strMotorvermogen = Interaction.InputBox("Gelieve het motor vermogen van de auto in te vullen", "Motor vermogen van auto");

                if (strMotorvermogen.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strMotorvermogen))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (strMotorvermogen.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strMotorvermogen)); // Herhaal als er cijfers zijn of als het leeg is

            MessageBox.Show($"Motor vermogen opgeslagen: {strMotorvermogen}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExtras_Click(object sender, EventArgs e)
        {
            do
            {
                strextras = Interaction.InputBox("Gelieve de extra eigenschappen van de auto in te vullen", "Extra eigenschappen van de auto");


                if (strextras.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strextras))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (strextras.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strextras)); // Herhaal als er cijfers zijn of als het leeg is

            MessageBox.Show($"Extras opgeslagen: {strextras}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void btnKleur_Click(object sender, EventArgs e)
        {
            do
            {
                strKleur = Interaction.InputBox("Wat is de vraagprijs van de auto?", "Prijs");

                if (strKleur.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strKleur))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (strKleur.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strKleur)); // Herhaal als er cijfers zijn of als het leeg is

            MessageBox.Show($"Kleur opgeslagen: {strKleur}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnPrijs_Click(object sender, EventArgs e)
        {
            do
            {
                stropvang = Interaction.InputBox("Wat is de vraagprijs van de auto?", "Prijs");

                // Controleer of de invoer leeg is of niet omgezet kan worden naar een double
                if (string.IsNullOrWhiteSpace(stropvang) || !double.TryParse(stropvang, out dblPrijs))
                {
                    MessageBox.Show("Gelieve een geldige prijs in te voeren!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (string.IsNullOrWhiteSpace(stropvang) || !double.TryParse(stropvang, out dblPrijs));

            MessageBox.Show($"Prijs opgeslagen: {dblPrijs} EUR", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }    
}
