using Microsoft.VisualBasic;
using MySql.Data;
using prjIcedOutWheelz.DA;
using prjIcedOutWheelz.Properties;
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
    public partial class frmAdminscherm : Form
    {
        // Tijdelijke objecten voor gebruikersinvoer
        Model.Type autoType = new Model.Type();
        Model.Motoren motorType = new Model.Motoren();
        Model.Kleuren kleurType = new Model.Kleuren();
        Model.Status statusType = new Model.Status();

        // Variabelen voor invoer en keuzelijsten
        string strMerk, strType, strKleur, strBrandstof, strextras, stropvang, strStatus;
        int intBouwjaar;
        double dblPrijs;
        string[] strKleurenLijst = { "Cream", "Lavendel", "Wit", "Grijs", "Paars", "Geel", "Roze", "Blauw", "Goud", "Groen", "Zalm" };
        string[] strStatusLijst = { "Nieuw", "Tweedehands" };

        public frmAdminscherm()
        {
            InitializeComponent(); // Initialiseer componenten
            this.CenterToScreen(); // Zet het scherm in het midden

            // Haal types op bij opstarten
            DataSet dsTypes = AutoDA.TypesOphalen();
            DataTable dtTypes = dsTypes.Tables[0];

            FillListBoxTypes(); // Vul de lijst met types
        }

        // Vul de lijstbox met autotypes
        public void FillListBoxTypes()
        {
            DataTable dtTypes = AutoDA.TypesOphalen().Tables[0];

            // Voeg samengestelde kolom toe als die nog niet bestaat
            if (!dtTypes.Columns.Contains("DisplayText"))
            {
                dtTypes.Columns.Add("DisplayText", typeof(string), "Merk + ' | ' + Type + ' | ' + Jaar");
            }

            lsbautos.DataSource = dtTypes; // Koppel data aan listbox
            lsbautos.DisplayMember = "DisplayText"; // Toon samengestelde tekst
            lsbautos.ValueMember = "typeID"; // Gebruik typeID als waarde
        }

        // Voeg een afbeelding toe aan een auto
        private void btnaddimg_Click(object sender, EventArgs e)
        {
            string eindwerkPath = FindEindwerkPath(); // Zoek resources-map
            if (eindwerkPath == null)
            {
                MessageBox.Show("Eindwerk map niet gevonden.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Open dialoog voor afbeeldingsselectie
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Afbeeldingsbestanden|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Selecteer een afbeeldingsbestand";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    try
                    {
                        // Laad afbeelding in PictureBox
                        picAuto.Image?.Dispose();
                        picAuto.Image = System.Drawing.Image.FromFile(selectedFilePath);
                        picAuto.SizeMode = PictureBoxSizeMode.StretchImage;

                        // Zet afbeelding om naar byte array
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (Bitmap clone = new Bitmap(picAuto.Image))
                            {
                                clone.Save(ms, picAuto.Image.RawFormat);
                            }
                            autoType.Foto = ms.ToArray();
                        }

                        MessageBox.Show("Afbeelding geselecteerd en opgeslagen in het object.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Fout bij het laden van de afbeelding.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Zoek het pad naar de Resources-map
        private string FindEindwerkPath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDirectory);

            // Loop omhoog tot Resources-map gevonden is
            while (dir != null)
            {
                if (dir.GetDirectories("Resources").Any())
                {
                    return Path.Combine(dir.FullName, "Resources");
                }
                dir = dir.Parent;
            }

            return null; // Niet gevonden
        }

        // Ga terug naar het hoofdscherm
        private void txthooftdpagina_Click(object sender, EventArgs e)
        {
            frmhoofdpagina frmhoofdpagina = new frmhoofdpagina();
            frmhoofdpagina.Show();
            this.Hide();
        }

        // Verwijder een afbeelding uit de resources
        private void btnremove_Click(object sender, EventArgs e)
        {
            string eindwerkPath = FindEindwerkPath();

            if (eindwerkPath == null)
            {
                MessageBox.Show("Resources map niet gevonden.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Open dialoog om bestand te kiezen
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = eindwerkPath
            };

            openFileDialog.Filter = "Afbeeldingsbestanden|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;

                try
                {
                    // Verwijder bestand als het bestaat
                    if (File.Exists(selectedFile))
                    {
                        picAuto.Image.Dispose();
                        picAuto.Image = Properties.Resources.placeholder_image;
                        File.Delete(selectedFile);
                        MessageBox.Show("Bestand succesvol verwijderd!");
                    }
                    else
                    {
                        MessageBox.Show("Bestand bestaat niet.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fout: {ex.Message}");
                }
            }
            else
            {
                // Herhaal dialoog indien gewenst
                DialogResult resultaat = MessageBox.Show("Geen bestand geselecteerd.\nWilt u de verkenner sluiten?", "Waarschuwing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultaat == DialogResult.No)
                {
                    btnremove.PerformClick();
                }
            }
        }

        // Zet afbeelding terug naar standaard
        private void btnclear_Click(object sender, EventArgs e)
        {
            picAuto.Image.Dispose();
            picAuto.Image = Properties.Resources.placeholder_image;
        }

        // Selecteer een afbeelding uit de resources
        private void btnselect_Click(object sender, EventArgs e)
        {
            string eindwerkPath = FindEindwerkPath();

            if (eindwerkPath == null)
            {
                MessageBox.Show("Resources map niet gevonden.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecteer een afbeelding",
                Filter = "Afbeeldingsbestanden|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                InitialDirectory = eindwerkPath
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                picAuto.ImageLocation = openFileDialog.FileName;
            }
            else
            {
                // Herhaal dialoog indien gewenst
                DialogResult resultaat = MessageBox.Show("Geen bestand geselecteerd.\nWilt u de verkenner sluiten?", "Waarschuwing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultaat == DialogResult.No)
                {
                    btnselect.PerformClick();
                }
            }
        }

        // Toon afbeelding van geselecteerde auto
        private void lsbautos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = lsbautos.SelectedIndex;
            DataTable dtListBox = (DataTable)lsbautos.DataSource;
            int autoType = Convert.ToInt32(dtListBox.Rows[selectedIndex]["typeID"]);

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
                            picAuto.Image = Image.FromStream(ms);
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

        // Dialoog voor invoer merk
        private void btnMerk_Click(object sender, EventArgs e)
        {
            do
            {
                strMerk = Interaction.InputBox("Gelieve het merk van de auto in te vullen", "Merk Auto");

                // Validatie: geen cijfers, niet leeg
                if (strMerk.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strMerk))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    autoType.Merk = strMerk;
                }

            } while (strMerk.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strMerk));

            MessageBox.Show($"Merk opgeslagen: {strMerk}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Dialoog voor invoer model
        private void btnModel_Click(object sender, EventArgs e)
        {
            do
            {
                strType = Interaction.InputBox("Gelieve het Type van de auto in te vullen", "Model Auto");

                if (string.IsNullOrWhiteSpace(strType))
                {
                    MessageBox.Show("Gelieve het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    autoType._Type = strType;
                }

            } while (string.IsNullOrWhiteSpace(strType));

            MessageBox.Show($"Model opgeslagen: {strType}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Dialoog voor invoer bouwjaar
        private void btnBouwjaar_Click(object sender, EventArgs e)
        {
           do
            {
                stropvang = Interaction.InputBox("Wat is het bouwjaar van de auto?", "Bouwjaar");

                // Validatie: numeriek en niet leeg
                if (string.IsNullOrWhiteSpace(stropvang) || !int.TryParse(stropvang, out intBouwjaar))
                {
                    MessageBox.Show("Gelieve een geldig bouwjaar in te voeren!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    autoType.Jaar = stropvang;
                }

           } while (string.IsNullOrWhiteSpace(stropvang) || !int.TryParse(stropvang, out intBouwjaar));

            MessageBox.Show($"Bouwjaar opgeslagen: {intBouwjaar}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Log uit en keer terug naar startscherm
        private void btnLoguit_Click(object sender, EventArgs e)
        {
            frmStartscherm frmStartscherm = new frmStartscherm();
            frmStartscherm.Show();
            this.Close();
        }

        // Dialoog voor invoer brandstof
        private void btnBrandstof_Click(object sender, EventArgs e)
        {
            do
            {
                strBrandstof = Interaction.InputBox("Gelieve het brandstof van de auto in te vullen", "Brandstof Auto");

                if (strBrandstof.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strBrandstof))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    motorType.Brandstoftype = strBrandstof;
                }

            } while (strBrandstof.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strBrandstof));

            MessageBox.Show($"Brandstof opgeslagen: {strBrandstof}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Verwijder geselecteerde auto uit database
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lsbautos.SelectedItem is DataRowView selectedRow)
            {
                string strSelectedMerk = selectedRow["Merk"].ToString();
                string strSelectedType = selectedRow["Type"].ToString();

                AdminDA.AutoVerwijderen(strSelectedMerk, strSelectedType);
                FillListBoxTypes();
            }
        }

        private void frmAdminscherm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmhoofdpagina frm = new frmhoofdpagina();
            this.Hide();
            frm.Show();
        }

        // Dialoog voor invoer status
        private void btnAddStatus_Click(object sender, EventArgs e)
        {
            do
            {
                strStatus = Interaction.InputBox("Maak een keuze voor de status van de auto:\nNieuw | Tweedehands", "Status Auto");

                // Validatie: alleen uit lijst, geen cijfers, niet leeg
                if (strStatus.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strStatus) || !strStatusLijst.Contains(strStatus, StringComparer.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!char.IsUpper(strStatus[0]))
                    {
                        strStatus = char.ToUpper(strStatus[0]) + strStatus.Substring(1);
                    }
                }

            } while (strBrandstof.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strBrandstof) || !strStatusLijst.Contains(strStatus, StringComparer.OrdinalIgnoreCase));
            statusType._Status = strStatus;
            MessageBox.Show($"Status opgeslagen: {strStatus}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Voeg nieuwe auto toe aan database
        private void btnadd_Click(object sender, EventArgs e)
        {
            if(!AdminDA.DuplicateCarCheck(autoType.Merk, autoType._Type, autoType.Jaar))
            {
                // Zet afbeelding om naar byte array
                if (picAuto.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Bitmap clone = new Bitmap(picAuto.Image))
                        {
                            clone.Save(ms, picAuto.Image.RawFormat);
                        }
                        autoType.Foto = ms.ToArray();
                    }
                }
                else
                {
                    autoType.Foto = null;
                }

                // Voeg type, motor, kleur en status toe en koppel deze
                AdminDA.TypeToevoegen(autoType);
                autoType.Typeid = AdminDA.GetAutoIdByMerkAndType(autoType.Merk, autoType._Type).ToString();
                motorType.Autoid = autoType.Typeid;
                AdminDA.MotorToevoegen(motorType);
                motorType.Motorid = AdminDA.GetMotorIdByAutoId(Convert.ToInt32(motorType.Autoid)).ToString();
                AdminDA.MotorPerTypeToevoegen(Convert.ToInt32(motorType.Autoid), Convert.ToInt32(motorType.Motorid));

                int typeidkleur = AdminDA.GetAutoIdByMerkAndType(autoType.Merk, autoType._Type);
                int kleuriddd = AdminDA.KleurIDOphalen(kleurType.Kleur);
                AdminDA.KleurPerTypeToevoegen(typeidkleur, kleuriddd);
                int typeidstatus = AdminDA.GetAutoIdByMerkAndType(autoType.Merk, autoType._Type);
                int statusiddd = AdminDA.StatusIDOphalen(statusType._Status);
                AdminDA.StatusPerTypeToevoegen(typeidstatus, statusiddd);
                FillListBoxTypes();
            }
            else
            {
                MessageBox.Show("Deze auto bestaat al in de database!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Dialoog voor invoer motorgegevens
        private void Motorvermogen_Click(object sender, EventArgs e)
        {
            // Motortype opvragen
            string inputMotortype;
            do
            {
                inputMotortype = Interaction.InputBox("Gelieve het motortype van de auto in te vullen", "Motortype");
                if (string.IsNullOrWhiteSpace(inputMotortype))
                {
                    MessageBox.Show("Motortype mag niet leeg zijn!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } while (string.IsNullOrWhiteSpace(inputMotortype));
            motorType.Motortype = inputMotortype;

            // Haal TypeID op uit de database
            int inputAutoid;
            inputAutoid = AdminDA.GetAutoIdByMerkAndType(autoType.Merk, autoType._Type);
            motorType.Autoid = inputAutoid.ToString();

            // Vermogen opvragen
            string inputVermogen;
            do
            {
                inputVermogen = Interaction.InputBox("Gelieve het motorvermogen van de auto in te vullen | formaat VB 11pk (9kw)", "Vermogen");
                if (string.IsNullOrWhiteSpace(inputVermogen))
                {
                    MessageBox.Show("Vermogen mag niet leeg zijn!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } while (string.IsNullOrWhiteSpace(inputVermogen));
            motorType.Vermogen = inputVermogen;

            // Koppel opvragen
            string inputKoppel;
            do
            {
                inputKoppel = Interaction.InputBox("Gelieve het koppel van de auto in te vullen | formaat VB 9Nm ", "Koppel");
                if (string.IsNullOrWhiteSpace(inputKoppel))
                {
                    MessageBox.Show("Koppel mag niet leeg zijn!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } while (string.IsNullOrWhiteSpace(inputKoppel));
            motorType.Koppel = inputKoppel;

            // Batterijcapaciteit opvragen (optioneel)
            string inputBatterij = Interaction.InputBox("Gelieve de batterijcapaciteit in te vullen (enkel in te vullen bij elektrische motor)", "Batterijcapaciteit");
            motorType.Batterijcapaciteit = inputBatterij;

            MessageBox.Show("Motorgegevens opgeslagen!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Dialoog voor invoer extra eigenschappen
        private void btnExtras_Click(object sender, EventArgs e)
        {
            do
            {
                strextras = Interaction.InputBox("Gelieve de extra eigenschappen van de auto in te vullen", "Extra eigenschappen van de auto");

                if (strextras.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strextras))
                {
                    MessageBox.Show("Gelieve alleen letters te gebruiken en het veld niet leeg te laten!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } while (strextras.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strextras));

            MessageBox.Show($"Extras opgeslagen: {strextras}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Dialoog voor kleurkeuze
        private void btnKleur_Click(object sender, EventArgs e)
        {
            do
            {
                strKleur = Interaction.InputBox("Maak een keuze van kleur:\nCream | Lanvendel | Wit | Grijs | Paars | Geel | Roze | Blauw | Goud |Groen | Zalm", "Kleuren keuze");

                if (strKleur.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strKleur) || !strKleurenLijst.Contains(strKleur, StringComparer.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Gelieve een kleur te kiezen uit de lijst!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!char.IsUpper(strKleur[0]))
                    {
                        strKleur = char.ToUpper(strKleur[0]) + strKleur.Substring(1);
                    }
                }

            } while (strKleur.Any(char.IsDigit) || string.IsNullOrWhiteSpace(strKleur) || !strKleurenLijst.Contains(strKleur, StringComparer.OrdinalIgnoreCase));
            kleurType.Kleur = strKleur;
            MessageBox.Show($"Kleur opgeslagen: {strKleur}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        // Dialoog voor invoer prijs
        private void btnPrijs_Click(object sender, EventArgs e)
        {
            do
            {
                stropvang = Interaction.InputBox("Wat is de vraagprijs van de auto?", "Prijs");

                if (string.IsNullOrWhiteSpace(stropvang) || !double.TryParse(stropvang, out dblPrijs))
                {
                    MessageBox.Show("Gelieve een geldige prijs in te voeren!", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    autoType.Prijs = dblPrijs;
                }

            } while (string.IsNullOrWhiteSpace(stropvang) || !double.TryParse(stropvang, out dblPrijs));

            MessageBox.Show($"Prijs opgeslagen: {dblPrijs} EUR", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }    
}
