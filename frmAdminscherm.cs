using MySql.Data;
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
        public frmAdminscherm()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btnaddimg_Click(object sender, EventArgs e)
        {
            //    //maak een openfiledialog zodat je een img kan selecteren
            //    OpenFileDialog openFileDialog = new OpenFileDialog();
            //    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff"; // Filter
            //    openFileDialog.Title = "Select an Image";


            //    //toon en check of er een file is geselecteerd
            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        try
            //        {
            //            //file path
            //            string selectedFilePath = openFileDialog.FileName;

            //            //waar opslaan
            //            string resourcesFolder = @"Z:\oefen\Sofo\Projecten\Eindwerk\Resources";

            //            //check of locatie bestaat anders word er een gemaakt
            //            if (!Directory.Exists(resourcesFolder))
            //            {
            //                Directory.CreateDirectory(resourcesFolder);
            //            }

            //            //naam file van path
            //            string fileName = Path.GetFileName(selectedFilePath);
            //            string destinationPath = Path.Combine(resourcesFolder, fileName);

            //            //maak naam uniek als die al bestaat
            //            int counter = 1;
            //            while (File.Exists(destinationPath))
            //            {
            //                string newFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + counter + Path.GetExtension(fileName);
            //                destinationPath = Path.Combine(resourcesFolder, newFileName);
            //                counter++;
            //            }

            //            //copieer file
            //            File.Copy(selectedFilePath, destinationPath);

            //            // img in picturebox
            //            pcbauto.Image = System.Drawing.Image.FromFile(destinationPath);
            //            //img aanpassen
            //            pcbauto.SizeMode = PictureBoxSizeMode.StretchImage;
            //            //messagebox om te melden dat de img is opgeslagen
            //            MessageBox.Show($"Image saved to: {destinationPath}");
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Error loading or saving image: " + ex.Message);
            //        }
            //    }
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
                        pcbauto.Image.Dispose();
                        // img in picturebox
                        pcbauto.Image = System.Drawing.Image.FromFile(destinationPath);
                        //img aanpassen
                        pcbauto.SizeMode = PictureBoxSizeMode.StretchImage;
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

        [STAThread]
        static void start()
        {
            Application.EnableVisualStyles();
            Application.Run(new frmAdminscherm());
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
                        pcbauto.Image.Dispose();
                        pcbauto.Image = Properties.Resources.placeholder_image;
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
            lsbautos.Items.Clear();
            lsbinfo.Items.Clear();
            pcbauto.Image.Dispose();
            pcbauto.Image = Properties.Resources.placeholder_image;
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
                pcbauto.ImageLocation = openFileDialog.FileName;
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

    }
    
}
