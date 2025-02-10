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
            //maak een openfiledialog zodat je een img kan selecteren
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff"; // Filter
            openFileDialog.Title = "Select an Image";


            //toon en check of er een file is geselecteerd
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //file path
                    string selectedFilePath = openFileDialog.FileName;

                    //waar opslaan
                    string resourcesFolder = @"Z:\oefen\Sofo\Projecten\Eindwerk\Resources";

                    //check of locatie bestaat anders word er een gemaakt
                    if (!Directory.Exists(resourcesFolder))
                    {
                        Directory.CreateDirectory(resourcesFolder);
                    }

                    //naam file van path
                    string fileName = Path.GetFileName(selectedFilePath);
                    string destinationPath = Path.Combine(resourcesFolder, fileName);

                    //maak naam uniek als die al bestaat
                    int counter = 1;
                    while (File.Exists(destinationPath))
                    {
                        string newFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + counter + Path.GetExtension(fileName);
                        destinationPath = Path.Combine(resourcesFolder, newFileName);
                        counter++;
                    }

                    //copieer file
                    File.Copy(selectedFilePath, destinationPath);

                    // img in picturebox
                    pcbauto.Image = System.Drawing.Image.FromFile(destinationPath);
                    //img aanpassen
                    pcbauto.SizeMode = PictureBoxSizeMode.StretchImage;
                    //messagebox om te melden dat de img is opgeslagen
                    MessageBox.Show($"Image saved to: {destinationPath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading or saving image: " + ex.Message);
                }
            }
        }

        private void txthooftdpagina_Click(object sender, EventArgs e)
        {
            frmhoofdpagina frmhoofdpagina = new frmhoofdpagina();
            frmhoofdpagina.Show();
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            //maak een openfiledialog
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = @"Z:\oefen\Sofo\Projecten\Eindwerk\Resources" // folder locatie
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
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            lsbautos.Items.Clear();
            lsbinfo.Items.Clear();
            pcbauto.Image = Properties.Resources.placeholder_image;
        }

        private void btnselect_Click(object sender, EventArgs e)
        {
            //maak OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                InitialDirectory = @"Z:\oefen\Sofo\Projecten\Eindwerk\Resources" // folder locatie
            };

            //toon dialog en get result
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //laad img in picturebox
                pcbauto.ImageLocation = openFileDialog.FileName;
            }

        }

    }
    
}
