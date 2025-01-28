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
                    string resourcesFolder = @"Z:\oefen\Sofo\Projecten\E\Resources";

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
    }
}
