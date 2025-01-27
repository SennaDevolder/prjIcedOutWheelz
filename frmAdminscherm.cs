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
            // Create an OpenFileDialog to allow user to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff"; // Filter for image files
            openFileDialog.Title = "Select an Image";

            // Show the dialog and check if user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the selected file path
                    string selectedFilePath = openFileDialog.FileName;

                    // Define the custom Resources folder path
                    string resourcesFolder = @"Z:\oefen\Sofo\Projecten\E\Resources";

                    // Check if the Resources folder exists, and if not, create it
                    if (!Directory.Exists(resourcesFolder))
                    {
                        Directory.CreateDirectory(resourcesFolder);
                    }

                    // Get the file name from the selected file path
                    string fileName = Path.GetFileName(selectedFilePath);
                    string destinationPath = Path.Combine(resourcesFolder, fileName);

                    // If the file already exists, generate a unique name by appending a number
                    int counter = 1;
                    while (File.Exists(destinationPath))
                    {
                        string newFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + counter + Path.GetExtension(fileName);
                        destinationPath = Path.Combine(resourcesFolder, newFileName);
                        counter++;
                    }

                    // Copy the file to the Resources folder
                    File.Copy(selectedFilePath, destinationPath);

                    // Load the selected image into the PictureBox
                    pcbauto.Image = System.Drawing.Image.FromFile(destinationPath);
                    pcbauto.SizeMode = PictureBoxSizeMode.StretchImage; // Adjust image to fit PictureBox

                    // Notify the user where the image was saved
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
