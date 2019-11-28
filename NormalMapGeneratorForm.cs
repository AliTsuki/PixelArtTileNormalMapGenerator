using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PixelArtTileNormalMapGenerator
{
    /// <summary>
    /// Class specifying the behaviour of the Normal Map Generator Form.
    /// </summary>
    public partial class NormalMapGeneratorForm : Form
    {
        // Path
        private string DefaultNormalMapImageDirectoryPath = Application.StartupPath + "\\DefaultNormal\\";
        private static string DefaultNormalMapImageName = "DefaultNormal.png";
        // Bitmaps
        private Bitmap OriginalImageBitmap;
        private Bitmap NormalMapDefaultImageBitmap;
        private Bitmap NormalMapImageBitmap;
        // Colors
        private Color NormalMapDefaultBGColor;
        private Color IgnoreColor;
        private Color BackgroundColor;
        private Color SeparatorColor;
        private Color IndividualColor;
        // Difference checks
        private int IgnoreColorMaxDifference;
        private int BackgroundColorMaxDifference;
        private int SeparatorColorMaxDifference;
        private int IndividualColorMaxDifference;
        // Lists
        private static List<TileObject> tileObjects = new List<TileObject>();
        // Enum
        private enum ColorType
        {
            Separator,
            Individual
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NormalMapGeneratorForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Loads the normal map generator form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NormalMapGeneratorForm_Load(object sender, EventArgs args)
        {
            this.InitializeValues();
        }

        /// <summary>
        /// Initialize colors and bitmap images to default values.
        /// </summary>
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

        /// <summary>
        /// Opens a file select dialog, selected image is read to bitmap and shown in original image preview box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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

        // TODO: maybe get rid of ignore color, create a better interface to get color values, add a dropper tool to select color from loaded image

        /// <summary>
        /// Opens a color select dialog box for ignore color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IgnoreColorDialogButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.IgnoreColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.IgnoreColor = this.IgnoreColorDialog.Color;
                this.IgnoreColorBox.BackColor = this.IgnoreColor;
            }
        }

        /// <summary>
        /// Opens a color select dialog box for background color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SelectBackgroundColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.BackgroundColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.BackgroundColor = this.BackgroundColorDialog.Color;
                this.BackgroundColorBox.BackColor = this.BackgroundColor;
            }
        }

        /// <summary>
        /// Opens a color select dialog box for separator color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SelectSeparatorColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.SeparatorColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.SeparatorColor = this.SeparatorColorDialog.Color;
                this.SeparatorColorBox.BackColor = this.SeparatorColor;
            }
        }

        /// <summary>
        /// Opens a color select dialog box for individual color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SelectIndividualColorButton_Click(object sender, EventArgs args)
        {
            DialogResult result = this.IndividualColorDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.IndividualColor = this.IndividualColorDialog.Color;
                this.IndividualColorBox.BackColor = this.IndividualColor;
            }
        }

        /// <summary>
        /// Starts the normal map generation process and displays created normal map to normal map preview box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GenerateNormalMapButton_Click(object sender, EventArgs args)
        {
            if(this.OriginalImageBitmap != null)
            {
                this.NormalMapImageBitmap = new Bitmap(this.OriginalImageBitmap.Width, this.OriginalImageBitmap.Height);
                this.CreateNormalMap();
                this.NormalMapImagePreviewBox.Image = this.NormalMapImageBitmap;
            }
            else
            {
                MessageBox.Show($@"No image loaded!{Environment.NewLine}Please load an image first.");
            }
        }

        /// <summary>
        /// Runs initialize values method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ResetButton_Click(object sender, EventArgs args)
        {
            this.InitializeValues();
        }

        /// <summary>
        /// Loads the default normal map texture as bitmap.
        /// </summary>
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

        /// <summary>
        /// Creates a normal map from an original image using the selected colors to traverse.
        /// </summary>
        private void CreateNormalMap()
        {
            try
            {
                for(int x = 0; x < this.OriginalImageBitmap.Width; x++)
                {
                    for(int y = 0; y < this.OriginalImageBitmap.Height; y++)
                    {
                        Color currentPixelColor = this.OriginalImageBitmap.GetPixel(x, y);
                        if(this.GetColorDistance(currentPixelColor, this.BackgroundColor) <= this.BackgroundColorMaxDifference)
                        {
                            this.NormalMapImageBitmap.SetPixel(x, y, this.NormalMapDefaultBGColor);
                        }
                        else if(this.GetColorDistance(currentPixelColor, this.IndividualColor) <= this.IndividualColorMaxDifference)
                        {
                            if(this.HasPixelAlreadyBeenAdded(x, y) == false)
                            {
                                TileObject newTileObject = new TileObject();
                                tileObjects.Add(newTileObject);
                                this.CheckAdjacentPixels(x, y, newTileObject);
                                this.FloodFill(newTileObject);
                                this.CreateNormalMapForTileObject(newTileObject);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($@"Error creating Normal Map!{Environment.NewLine}{e.ToString()}");
            }
        }

        /// <summary>
        /// Checks if a pixel coordinate has already been added to a TileObject.
        /// </summary>
        /// <param name="x">X coordinate of pixel.</param>
        /// <param name="y">Y coordinate of pixel.</param>
        /// <returns>Returns true if pixel coords have already been added to a Tile Object.</returns>
        private bool HasPixelAlreadyBeenAdded(int x, int y)
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

        /// <summary>
        /// Checks if a pixel coordinate is within the bounds of the original image.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        /// <returns>Returns true if pixel coordinates are within the bounds of the original image.</returns>
        private bool IsPixelWithinBounds(Vector2 xy)
        {
            if(xy.X >= 0 && xy.X < this.OriginalImageBitmap.Width && xy.Y >= 0 && xy.Y < this.OriginalImageBitmap.Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the difference between two colors as an integer.
        /// </summary>
        /// <param name="currentColor">The color to check.</param>
        /// <param name="colorToCheckAgainst">The color to measure against.</param>
        /// <returns>Returns an integer denoting the amount of difference between 2 colors. Increasing values mean increasingly different.</returns>
        private int GetColorDistance(Color currentColor, Color colorToCheckAgainst)
        {
            int distance = Math.Abs(currentColor.R - colorToCheckAgainst.R) + Math.Abs(currentColor.G - colorToCheckAgainst.G) + Math.Abs(currentColor.B - colorToCheckAgainst.B) + Math.Abs(currentColor.A - colorToCheckAgainst.A);
            return distance;
        }

        /// <summary>
        /// Checks if a specified pixel, by coordinate, has a color that is within the specified distance of a given color type.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        /// <param name="colorType">Color type: Separator, Individual</param>
        /// <returns>Returns true if the pixel's color is within the maximum allowed color difference.</returns>
        private bool IsColorWithinColorDistance(Vector2 xy, ColorType colorType)
        {
            if(colorType == ColorType.Individual)
            {
                if(this.GetColorDistance(this.OriginalImageBitmap.GetPixel(xy.X, xy.Y), this.IndividualColor) <= this.IndividualColorMaxDifference)
                {
                    return true;
                }
                return false;
            }
            else if(colorType == ColorType.Separator)
            {
                if(this.GetColorDistance(this.OriginalImageBitmap.GetPixel(xy.X, xy.Y), this.SeparatorColor) <= this.SeparatorColorMaxDifference)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Adds pixels adjacent to the one specified to the list of pixels needing to be checked if they meet the criteria. Used by flood fill.
        /// </summary>
        /// <param name="x">X coordinate of pixel.</param>
        /// <param name="y">Y coordinate of pixel.</param>
        /// <param name="tileObject">Tile Object the parent pixel belongs to.</param>
        private void CheckAdjacentPixels(int x, int y, TileObject tileObject)
        {
            tileObject.AddNewIndividualPixel(new Vector2(x, y));
            tileObject.AddPixelAlreadyChecked(new Vector2(x, y));
            Vector2[] adjacentPixelCoords = new Vector2[4]
            {
                new Vector2(x + 1, y),
                new Vector2(x - 1, y),
                new Vector2(x, y + 1),
                new Vector2(x, y - 1)
            };
            for(int i = 0; i < adjacentPixelCoords.Length; i++)
            {
                if(this.IsPixelWithinBounds(adjacentPixelCoords[i]) && this.IsColorWithinColorDistance(adjacentPixelCoords[i], ColorType.Individual) && tileObject.PixelsAlreadyChecked.Contains(adjacentPixelCoords[i]) == false)
                {
                    tileObject.AddPixelToCheck(adjacentPixelCoords[i]);
                }
                else if(this.IsPixelWithinBounds(adjacentPixelCoords[i]) && this.IsColorWithinColorDistance(adjacentPixelCoords[i], ColorType.Separator) && tileObject.PixelsAlreadyChecked.Contains(adjacentPixelCoords[i]) == false)
                {
                    tileObject.AddNewEdgePixel(adjacentPixelCoords[i]);
                    tileObject.AddPixelAlreadyChecked(adjacentPixelCoords[i]);
                }
                else
                {
                    tileObject.AddPixelAlreadyChecked(adjacentPixelCoords[i]);
                }
            }
        }

        /// <summary>
        /// Runs the check adjacent pixels method on each pixel that needs to be checked. Works recursively to find all adjacent pixels for a tile object.
        /// </summary>
        /// <param name="tileObject">Tile Object the parent pixel belongs to.</param>
        private void FloodFill(TileObject tileObject)
        {
            while(tileObject.PixelsToCheck.Count > 0)
            {
                List<Vector2> currentPixelsToCheck = tileObject.PixelsToCheck.ToList();
                tileObject.ClearPixelsToCheck();
                foreach(Vector2 pixel in currentPixelsToCheck)
                {
                    this.CheckAdjacentPixels(pixel.X, pixel.Y, tileObject);
                }
            }
            tileObject.ClearPixelsToCheck();
            tileObject.ClearPixelsAlreadyChecked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileObject">Tile Object the parent pixel belongs to.</param>
        private void CreateNormalMapForTileObject(TileObject tileObject)
        {
            // TODO: Translate normal map color from default normal map by scale to current normal map and add the pixels to bitmap
        }
    }

    // TODO: come up with a better name than TileObjects, yuck
    /// <summary>
    /// Class representing a connected group of pixels of varied shape.
    /// </summary>
    public class TileObject
    {
        public List<Vector2> EdgePixels { get; private set; }
        public List<Vector2> IndividualPixels { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ScaleVsDefault { get; private set; }
        public List<Vector2> PixelsAlreadyChecked { get; private set; }
        public List<Vector2> PixelsToCheck { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TileObject()
        {
            this.EdgePixels = new List<Vector2>();
            this.IndividualPixels = new List<Vector2>();
            this.Width = 0;
            this.Height = 0;
            this.ScaleVsDefault = 0;
            this.PixelsAlreadyChecked = new List<Vector2>();
            this.PixelsToCheck = new List<Vector2>();
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Edge Pixels list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddNewEdgePixel(Vector2 xy)
        {
            this.EdgePixels.Add(xy);
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Individual Pixels list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddNewIndividualPixel(Vector2 xy)
        {
            this.IndividualPixels.Add(xy);
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Pixels To Check list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddPixelToCheck(Vector2 xy)
        {
            this.PixelsToCheck.Add(xy);
        }

        /// <summary>
        /// Adds a set of pixel coordinates to the Pixels Already Checked list.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        public void AddPixelAlreadyChecked(Vector2 xy)
        {
            this.PixelsAlreadyChecked.Add(xy);
        }

        /// <summary>
        /// Clears the Pixels To Check list.
        /// </summary>
        public void ClearPixelsToCheck()
        {
            this.PixelsToCheck.Clear();
        }

        /// <summary>
        /// Clears the Pixels Already Checked list.
        /// </summary>
        public void ClearPixelsAlreadyChecked()
        {
            this.PixelsAlreadyChecked.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetTotalDimensions()
        {
            // TODO: Get lowest x, highest x, lowest y, highest y, set width to highest x minus lowest x, height to highest y minus lowest y, get scale compared to default normal map image
        }

        /// <summary>
        /// Checks if a given set of pixel coordinates has been added to either the Edge Pixels or Individual Pixels lists of this object.
        /// </summary>
        /// <param name="x">X coordinate of pixel.</param>
        /// <param name="y">Y coordinate of pixel.</param>
        /// <returns>Returns true if the specified pixel exists in the lists of this object.</returns>
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

    /// <summary>
    /// Class representing 2 integers: X, Y. Used for pixel coordinates.
    /// </summary>
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
