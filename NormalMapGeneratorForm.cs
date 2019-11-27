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
        // Path
        private static string DefaultNormalMapImageDirectoryPath = Application.StartupPath + "\\DefaultNormal\\";
        private static string DefaultNormalMapImageName = "DefaultNormal.png";
        // Bitmaps
        private static Bitmap OriginalImageBitmap;
        private static Bitmap NormalMapDefaultImageBitmap;
        private static Bitmap NormalMapImageBitmap;
        // Colors
        private static Color NormalMapDefaultBGColor;
        private static Color IgnoreColor;
        private static Color BackgroundColor;
        private static Color SeparatorColor;
        private static Color IndividualColor;
        // Difference checks
        private static int IgnoreColorMaxDifference;
        private static int BackgroundColorMaxDifference;
        private static int SeparatorColorMaxDifference;
        private static int IndividualColorMaxDifference;
        // Lists
        private static List<TileObject> tileObjects = new List<TileObject>();

        // Default constructor
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
            this.LoadDefaultNormal();
            OriginalImageBitmap = null;
            NormalMapImageBitmap = null;
            NormalMapDefaultBGColor = Color.FromArgb(255, 118, 137, 249);
            IgnoreColor = Color.White;
            BackgroundColor = Color.White;
            SeparatorColor = Color.White;
            IndividualColor = Color.White;
            tileObjects.Clear();
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
                    this.OriginalImagePreviewBox.SizeMode = PictureBoxSizeMode.StretchImage;
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
            DialogResult result = this.BackgroundColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                BackgroundColor = this.BackgroundColorDialog.Color;
                this.BackgroundColorBox.BackColor = BackgroundColor;
            }
        }

        private void SelectSeparatorColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.SeparatorColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                SeparatorColor = this.SeparatorColorDialog.Color;
                this.SeparatorColorBox.BackColor = SeparatorColor;
            }
        }

        private void SelectIndividualColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.IndividualColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                IndividualColor = this.IndividualColorDialog.Color;
                this.IndividualColorBox.BackColor = IndividualColor;
            }
        }

        private void GenerateNormalMapButton_Click(object sender, EventArgs args)
        {
            if(OriginalImageBitmap != null)
            {
                NormalMapImageBitmap = new Bitmap(OriginalImageBitmap.Width, OriginalImageBitmap.Height);
                this.CreateNormalMap();
            }
            else
            {
                MessageBox.Show($@"No image loaded!{Environment.NewLine}Please load an image first.");
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
                    throw new Exception("Default Normal Map Image Directory did not exist! Creating Directory now!");
                }
            }
            catch(Exception)
            {
                MessageBox.Show($@"Error reading default normal map!{Environment.NewLine}Has the image been moved or deleted?{Environment.NewLine}Please restore the default normal map image to \DefaultNormal\DefaultNormal.png and press the RESET button to continue.");
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
                        if(this.GetColorDistance(currentPixelColor, IgnoreColor) <= IgnoreColorMaxDifference)
                        {
                            break;
                        }
                        else if(this.GetColorDistance(currentPixelColor, BackgroundColor) <= BackgroundColorMaxDifference)
                        {
                            NormalMapImageBitmap.SetPixel(x, y, NormalMapDefaultBGColor);
                        }
                        else if(this.GetColorDistance(currentPixelColor, SeparatorColor) <= SeparatorColorMaxDifference)
                        {

                            /* TODO: Try to find a nearby individual color pixel, if so and individual color pixel does not belong to another tileobject, 
                             * create a new tileobject, add this pixel to edges, flood fill to find all individual colored pixels, stop at other separator color pixels, 
                             * add all individual and separator pixels to respective lists, once done, get dimensions of shape, get scale of shape in comparison
                             * to default normal map image, combine all edge and individual pixels into one list, get color for each pixel from default
                             * normal map image using scale and relative location, write all pixel colors to new normal map image bitmap using pixel coords
                            */
                        }
                        else if(this.GetColorDistance(currentPixelColor, IndividualColor) <= IndividualColorMaxDifference)
                        {
                            // Same as above mostly
                        }
                    }
                }
                // After looping through all pixels of original image, all tile objects should be created, so write final normal map image
            }
            catch(Exception e)
            {
                MessageBox.Show($@"Error creating Normal Map!{Environment.NewLine}{e.ToString()}");
            }
        }

        private int GetColorDistance(Color currentColor, Color colorToCheckAgainst)
        {
            int distance = Math.Abs(currentColor.R - colorToCheckAgainst.R) + Math.Abs(currentColor.G - colorToCheckAgainst.G) + Math.Abs(currentColor.B - colorToCheckAgainst.B) + Math.Abs(currentColor.A - colorToCheckAgainst.A);
            return distance;
        }
    }

    // TODO: come up with a better name than TileObjects, yuck
    public class TileObject
    {
        public List<Vector2> EdgePixels { get; private set; }
        public List<Vector2> IndividualPixels { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ScaleVsDefault { get; private set; }

        public void AddNewEdgePixel(int x, int y)
        {
            this.EdgePixels.Add(new Vector2(x, y));
        }

        public void AddNewIndividualPixel(int x, int y)
        {
            this.IndividualPixels.Add(new Vector2(x, y));
        }

        public void GetTotalDimensions()
        {
            // TODO: Get lowest x, highest x, lowest y, highest y, set width to highest x minus lowest x, height to highest y minus lowest y, get scale compared to default normal map image
        }

        public bool DoesPixelExistInThisObject(int x, int y)
        {
            List<Vector2> allPixels = this.EdgePixels.Concat(this.IndividualPixels).ToList();
            foreach(Vector2 pixel in allPixels)
            {
                if(pixel.X == x && pixel.Y == y)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public struct Vector2
    {
        public int X;
        public int Y;

        public Vector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
