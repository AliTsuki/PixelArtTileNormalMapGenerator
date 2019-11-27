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

namespace PixelArtTileNormalMapGenerator
{
    public partial class NormalMapGeneratorForm : Form
    {
        private static string DefaultNormalMapImageDirectoryPath = Application.StartupPath + "\\DefaultNormal\\";
        private static string DefaultNormalMapImageName = "DefaultNormal.png";
        private static Bitmap OriginalImageBitmap;
        private static Bitmap NormalMapDefaultImageBitmap;
        private static Bitmap NormalMapImageBitmap;
        private static Color NormalMapBackgroundDefaultColor;
        private static Color IgnoreColor;
        private static Color BackgroundColor;
        private static Color SeparatorColor;
        private static Color IndividualColor;

        public NormalMapGeneratorForm()
        {
            this.InitializeComponent();
        }

        // Set default values
        // On Load Image button clicked select image from disk
        // Load image into memory
        // Show image in preview box
        // Save selected colors, Ignore, background, separator, individual
        // On Generate Normal Map button clicked, generate new image, same size as original, go through each pixel in image
        // -ignore ignore colors, turn background colors into normal bluish color, on separator color create new tile object
        // -store pixel location, do recursive flood fill, check against pixels already added, check all nearby pixels
        // -if they are individual color add them to object, if separator color, add to object in separate list and don't seek farther
        // -in that direction. Upon completion of flood fill, add separator list to individual list, get width and height of individual tile object
        // -get scale difference between object and normal map default texture provided, for each pixel in object, translate to pixel location in
        // -normal map default, use scale also to determin how many pixels would be averaged in shrinking, grab all pixels around selected in normal default,
        // -average them to get final value to be assigned to pixel of object, loop through all objects like that, once all objects are instantiated and have
        // - their colors selected, go through new blank normal map image and place all the pixels in place
        // Show normal map in preview box
        // On Save Normal Map button clicked, save normal map image as png in same path as original image
        // On Reset button clicked, clear all saved colors and images
        private void NormalMapGeneratorForm_Load(object sender, EventArgs args)
        {
            this.InitializeValues();
        }

        private void InitializeValues()
        {
            LoadDefaultNormal();
            OriginalImageBitmap = null;
            NormalMapImageBitmap = null;
            IgnoreColor = Color.White;
            BackgroundColor = Color.White;
            SeparatorColor = Color.White;
            IndividualColor = Color.White;
        }

        private void LoadImageButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.OpenFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                try
                {
                    OriginalImageBitmap = new Bitmap(this.OpenFileDialog.FileName);
                    this.OriginalImagePreviewBox.Image = OriginalImageBitmap;
                }
                catch(Exception)
                {
                    MessageBox.Show($@"Unable to read image!{Environment.NewLine}Please select a valid image format.{Environment.NewLine}For best results use PNG format.");
                }
            }
        }

        private void IgnoreColorDialogButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.IgnoreColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                IgnoreColor = this.IgnoreColorDialog.Color;
                this.IgnoreColorBox.BackColor = IgnoreColor;
            }
        }

        private void SelectBackgroundColorButton_Click(object sender, EventArgs args)
        {
            this.BackgroundColorDialog.ShowDialog();
            BackgroundColor = this.BackgroundColorDialog.Color;
            this.BackgroundColorBox.BackColor = BackgroundColor;
        }

        private void SelectSeparatorColorButton_Click(object sender, EventArgs args)
        {
            this.SeparatorColorDialog.ShowDialog();
            SeparatorColor = this.SeparatorColorDialog.Color;
            this.SeparatorColorBox.BackColor = SeparatorColor;
        }

        private void SelectIndividualColorButton_Click(object sender, EventArgs args)
        {
            this.IndividualColorDialog.ShowDialog();
            IndividualColor = this.IndividualColorDialog.Color;
            this.IndividualColorBox.BackColor = IndividualColor;
        }

        private void GenerateNormalMapButton_Click(object sender, EventArgs args)
        {
            if(OriginalImageBitmap != null)
            {
                NormalMapImageBitmap = new Bitmap(OriginalImageBitmap.Width, OriginalImageBitmap.Height);
                this.CreateNormalMap();
            }
        }

        private void ResetButton_Click(object sender, EventArgs args)
        {
            this.InitializeValues();
        }

        private void LoadDefaultNormal()
        {
            try
            {
                if(Directory.Exists(DefaultNormalMapImageDirectoryPath))
                {
                    NormalMapDefaultImageBitmap = new Bitmap(DefaultNormalMapImageDirectoryPath + DefaultNormalMapImageName);
                }
                else
                {
                    Directory.CreateDirectory(DefaultNormalMapImageDirectoryPath);
                    throw new Exception("Default Normal Map Image Directory did not exist! Creating Directory!");
                }
            }
            catch(Exception)
            {
                MessageBox.Show($@"Error reading default normal map!{Environment.NewLine}Has the image been moved or deleted?{Environment.NewLine}Please restore the default normal map image to \DefaultNormal\DefaultNormal.PNG and press the RESET button to continue.");
            }
        }

        private void CreateNormalMap()
        {
            try
            {
                for(int x = 0; x < OriginalImageBitmap.Width; x++)
                {
                    for(int y = 0; y < OriginalImageBitmap.Height; y++)
                    {
                        Color currentPixelColor = OriginalImageBitmap.GetPixel(x, y);
                        if(currentPixelColor == IgnoreColor)
                        {
                            break;
                        }
                        else if(currentPixelColor == BackgroundColor)
                        {
                            NormalMapImageBitmap.SetPixel(x, y, NormalMapBackgroundDefaultColor);
                        }
                        else if(currentPixelColor == SeparatorColor)
                        {

                        }
                        else if(currentPixelColor == IndividualColor)
                        {

                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($@"Error creating Normal Map!{Environment.NewLine}{e.ToString()}");
            }
        }

        private int GetColorDistance(Color currentColor, Color colorToCheckAgainst)
        {
            return 0;
        }
    }

    public class TileObject
    {

    }
}
