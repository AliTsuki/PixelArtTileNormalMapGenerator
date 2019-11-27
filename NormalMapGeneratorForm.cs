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
        private string DefaultNormalMapImageDirectoryPath = Application.StartupPath + "\\DefaultNormal\\";
        private static string DefaultNormalMapImageName = "DefaultNormal.png";
        // Bitmaps
        public Bitmap OriginalImageBitmap;
        public Bitmap NormalMapDefaultImageBitmap;
        public Bitmap NormalMapImageBitmap;
        // Colors
        public Color NormalMapDefaultBGColor;
        public Color IgnoreColor;
        public Color BackgroundColor;
        public Color SeparatorColor;
        public Color IndividualColor;
        // Difference checks
        public int IgnoreColorMaxDifference;
        public int BackgroundColorMaxDifference;
        public int SeparatorColorMaxDifference;
        public int IndividualColorMaxDifference;
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
            this.OriginalImageBitmap = null;
            this.NormalMapImageBitmap = null;
            this.NormalMapDefaultBGColor = Color.FromArgb(255, 118, 137, 249);
            this.IgnoreColor = Color.White;
            this.BackgroundColor = Color.White;
            this.SeparatorColor = Color.White;
            this.IndividualColor = Color.White;
            tileObjects.Clear();
        }

        private void LoadImageButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.OpenFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                try
                {
                    this.OriginalImageBitmap = new Bitmap(this.OpenFileDialog.FileName);
                    this.OriginalImagePreviewBox.Image = this.OriginalImageBitmap;
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
                this.IgnoreColor = this.IgnoreColorDialog.Color;
                this.IgnoreColorBox.BackColor = this.IgnoreColor;
            }
        }

        private void SelectBackgroundColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.BackgroundColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.BackgroundColor = this.BackgroundColorDialog.Color;
                this.BackgroundColorBox.BackColor = this.BackgroundColor;
            }
        }

        private void SelectSeparatorColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.SeparatorColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.SeparatorColor = this.SeparatorColorDialog.Color;
                this.SeparatorColorBox.BackColor = this.SeparatorColor;
            }
        }

        private void SelectIndividualColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.IndividualColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.IndividualColor = this.IndividualColorDialog.Color;
                this.IndividualColorBox.BackColor = this.IndividualColor;
            }
        }

        private void GenerateNormalMapButton_Click(object sender, EventArgs args)
        {
            if(this.OriginalImageBitmap != null)
            {
                this.NormalMapImageBitmap = new Bitmap(this.OriginalImageBitmap.Width, this.OriginalImageBitmap.Height);
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
                if(Directory.Exists(this.DefaultNormalMapImageDirectoryPath))
                {
                    this.NormalMapDefaultImageBitmap = new Bitmap(this.DefaultNormalMapImageDirectoryPath + DefaultNormalMapImageName);
                }
                else
                {
                    Directory.CreateDirectory(this.DefaultNormalMapImageDirectoryPath);
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
                for(int x = 0; x < this.OriginalImageBitmap.Width; x++)
                {
                    for(int y = 0; y < this.OriginalImageBitmap.Height; y++)
                    {
                        Color currentPixelColor = this.OriginalImageBitmap.GetPixel(x, y);
                        if(GetColorDistance(currentPixelColor, this.BackgroundColor) <= this.BackgroundColorMaxDifference)
                        {
                            this.NormalMapImageBitmap.SetPixel(x, y, this.NormalMapDefaultBGColor);
                        }
                        else if(GetColorDistance(currentPixelColor, this.IndividualColor) <= this.IndividualColorMaxDifference)
                        {
                            if(HasPixelAlreadyBeenAdded(x, y) == false)
                            {
                                TileObject newTileObject = new TileObject(this);
                                newTileObject.AddNewIndividualPixel(x, y);
                                newTileObject.FloodFill(x, y);
                            }
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

        public static int GetColorDistance(Color currentColor, Color colorToCheckAgainst)
        {
            int distance = Math.Abs(currentColor.R - colorToCheckAgainst.R) + Math.Abs(currentColor.G - colorToCheckAgainst.G) + Math.Abs(currentColor.B - colorToCheckAgainst.B) + Math.Abs(currentColor.A - colorToCheckAgainst.A);
            return distance;
        }

        private static bool HasPixelAlreadyBeenAdded(int x, int y)
        {
            foreach(TileObject tileObject in tileObjects)
            {
                if(tileObject.DoesPixelExistInThisObject(x, y) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPixelWithinBounds(Vector2 xy)
        {
            if(xy.X >= 0 && xy.X < this.OriginalImageBitmap.Width && xy.Y >= 0 && xy.Y < this.OriginalImageBitmap.Height)
            {
                return true;
            }
            return false;
        }
    }

    // TODO: come up with a better name than TileObjects, yuck
    public class TileObject
    {
        public static NormalMapGeneratorForm NMG;
        public List<Vector2> EdgePixels { get; private set; }
        public List<Vector2> IndividualPixels { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ScaleVsDefault { get; private set; }
        private List<Vector2> PixelsAlreadyChecked;
        private List<Vector2> PixelsToCheck;

        public TileObject(NormalMapGeneratorForm nmg)
        {
            NMG = nmg;
            this.EdgePixels = new List<Vector2>();
            this.IndividualPixels = new List<Vector2>();
            this.Width = 0;
            this.Height = 0;
            this.ScaleVsDefault = 0;
            this.PixelsAlreadyChecked = new List<Vector2>();
            this.PixelsToCheck = new List<Vector2>();
        }

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

        public void FloodFill(int x, int y)
        {
            /* TODO: Try to find a nearby individual color pixel, if so and individual color pixel does not belong to another tileobject, 
             * create a new tileobject, add this pixel to edges, flood fill to find all individual colored pixels, stop at other separator color pixels, 
             * add all individual and separator pixels to respective lists, once done, get dimensions of shape, get scale of shape in comparison
             * to default normal map image, combine all edge and individual pixels into one list, get color for each pixel from default
             * normal map image using scale and relative location, write all pixel colors to new normal map image bitmap using pixel coords
            */
            Vector2 xPlus = new Vector2(x + 1, y);
            Vector2 xMinus = new Vector2(x - 1, y);
            Vector2 yPlus = new Vector2(x, y + 1);
            Vector2 yMinus = new Vector2(x, y - 1);
            if(NMG.IsPixelWithinBounds(xPlus) && this.IsIndividualColorWithinColorDistance(xPlus) && this.PixelsAlreadyChecked.Contains(xPlus) == false)
            {
                this.PixelsToCheck.Add(xPlus);
            }

        }

        private bool IsIndividualColorWithinColorDistance(Vector2 xy)
        {
            if(NormalMapGeneratorForm.GetColorDistance(NMG.OriginalImageBitmap.GetPixel(xy.X, xy.Y), NMG.IndividualColor) <= NMG.IndividualColorMaxDifference)
            {
                return true;
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
