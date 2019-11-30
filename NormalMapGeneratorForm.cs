using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelArtTileNormalMapGenerator
{
    /// <summary>
    /// Class specifying the behaviour of the Normal Map Generator Form.
    /// </summary>
    public partial class NormalMapGeneratorForm : Form
    {
        // Path
        public string DefaultNormalMapImageDirectoryPath = Application.StartupPath + "\\DefaultNormal\\";
        public static string DefaultNormalMapImageName = "DefaultNormal.png";
        // Bitmaps
        public Bitmap OriginalImageBitmap;
        public Bitmap NormalMapDefaultImageBitmap;
        public Bitmap NormalMapImageBitmap;
        // Dimensions
        public int ImageWidth;
        public int ImageHeight;
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
        public Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();
        // Flags
        public bool CreatingNormalMap = false;
        // Cancellation tokens
        private CancellationTokenSource cTokenSource = new CancellationTokenSource();
        private CancellationToken cToken = new CancellationToken();

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
            this.OriginalImagePreviewBox.Image = this.OriginalImageBitmap;
            this.NormalMapImageBitmap = null;
            this.NormalMapImagePreviewBox.Image = this.NormalMapImageBitmap;
            this.NormalMapDefaultBGColor = Color.FromArgb(255, 118, 137, 249);
            this.BackgroundColor = Color.FromArgb(255, 240, 202, 163);
            this.BackgroundColorBox.BackColor = this.BackgroundColor;
            this.SeparatorColor = Color.FromArgb(255, 95, 52, 33);
            this.SeparatorColorBox.BackColor = this.SeparatorColor;
            this.IndividualColor = Color.FromArgb(255, 199, 119, 82);
            this.IndividualColorBox.BackColor = this.IndividualColor;
            this.BackgroundColorMaxDifference = 10;
            this.BackgroundColorMaxDifferencePicker.Value = this.BackgroundColorMaxDifference;
            this.SeparatorColorMaxDifference = 10;
            this.SeparatorColorMaxDifferencePicker.Value = this.SeparatorColorMaxDifference;
            this.IndividualColorMaxDifference = 10;
            this.IndividualColorMaxDifferencePicker.Value = this.IndividualColorMaxDifference;
            this.NormalMapGenerationProgressLabel.Text = "Idle";
            this.NormalMapGenerationProgressBar.Value = 0;
            this.NormalMapGenerationProgressDetailLabel.Text = @"0% --- 0 / 0";
            this.Tiles.Clear();
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
                    this.ImageWidth = this.OriginalImageBitmap.Width;
                    this.ImageHeight = this.OriginalImageBitmap.Height;
                }
                catch(Exception)
                {
                    MessageBox.Show($@"Unable to read image!{Environment.NewLine}Please select a valid image format.{Environment.NewLine}For best results use PNG format.");
                }
            }
        }

        // TODO: create a better interface to get color values, add a dropper tool to select color from loaded image

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
            this.BackgroundColorMaxDifference = (int)this.BackgroundColorMaxDifferencePicker.Value;
            this.SeparatorColorMaxDifference = (int)this.SeparatorColorMaxDifferencePicker.Value;
            this.IndividualColorMaxDifference = (int)this.IndividualColorMaxDifferencePicker.Value;
            if(this.CreatingNormalMap == false)
            {
                if(this.OriginalImageBitmap != null)
                {
                    this.NormalMapImageBitmap = new Bitmap(this.ImageWidth, this.ImageHeight);
                    this.CreateNormalMap();
                }
                else
                {
                    MessageBox.Show($@"No image loaded!{Environment.NewLine}Please load an image first.");
                }
            }
            else
            {
                MessageBox.Show($@"Operation already in progress!{Environment.NewLine}Please wait for operation to finish before starting another.");
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
        /// Cancels the ongoing normal map generation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelGeneratorButton_Click(object sender, EventArgs e)
        {
            if(this.CreatingNormalMap == true)
            {
                this.cTokenSource.Cancel();
            }
            else
            {
                MessageBox.Show("No operation currently running to cancel.");
            }
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
        private async void CreateNormalMap()
        {
            this.cToken = this.cTokenSource.Token;
            this.CreatingNormalMap = true;
            this.NormalMapGenerationProgressBar.Minimum = 0;
            this.NormalMapGenerationProgressBar.Maximum = this.ImageWidth * this.ImageHeight;
            this.NormalMapGenerationProgressBar.Step = 1;
            Progress<string> nmgProgressLabelText = new Progress<string>(s => this.NormalMapGenerationProgressLabel.Text = s);
            Progress<string> nmgProgressLabelDetailText = new Progress<string>(s => this.NormalMapGenerationProgressDetailLabel.Text = s);
            Progress<int> nmgProgressBarValue = new Progress<int>(i => this.NormalMapGenerationProgressBar.Value = i);
            await Task.Factory.StartNew(() => GeneratorWorker.CreateNormalMap(this, this.cToken, nmgProgressLabelText, nmgProgressBarValue, nmgProgressLabelDetailText), TaskCreationOptions.LongRunning);
            this.UpdateNormalMapPreviewBox();
        }

        /// <summary>
        /// Updates the normal map image preview box with the current normal map bitmap image.
        /// </summary>
        public void UpdateNormalMapPreviewBox()
        {
            this.NormalMapImagePreviewBox.Image = this.NormalMapImageBitmap;
        }
    }



    /// <summary>
    /// Class run in the background as a Task that analyzes the image and creates a normal map from the image.
    /// </summary>
    public static class GeneratorWorker
    {
        // Form reference
        private static NormalMapGeneratorForm nmg;
        // Progress data
        private static int CurrentProgress = 0;
        private static int MaximumPixelsToCheck = 0;
        // Color type enum
        private enum ColorType
        {
            Background,
            Separator,
            Individual
        }
        // Debug
        public static Stopwatch watch = new Stopwatch();

        /// <summary>
        /// Creates a normal map from a loaded bitmap image.
        /// </summary>
        /// <param name="nmgf">Reference to the parent form.</param>
        /// <param name="cToken">Reference to the cancellation token.</param>
        /// <param name="progressLabelText">Reference to progress label.</param>
        /// <param name="progressBarValue">Reference to progress bar.</param>
        /// <param name="progressLabelDetailText">Reference to progress detail label.</param>
        public static void CreateNormalMap(NormalMapGeneratorForm nmgf, CancellationToken cToken, IProgress<string> progressLabelText, IProgress<int> progressBarValue, IProgress<string> progressLabelDetailText)
        {
            nmg = nmgf;
            CurrentProgress = 0;
            MaximumPixelsToCheck = nmg.ImageWidth * nmg.ImageHeight;
            progressLabelText.Report("In Progress...");
            progressBarValue.Report(CurrentProgress);
            float percent = (float)CurrentProgress / (float)MaximumPixelsToCheck * 100f;
            progressLabelDetailText.Report($@"{percent.ToString("00.00")}%  --- {CurrentProgress} / {MaximumPixelsToCheck}");
            try
            {
                CurrentProgress = 1;
                for(int x = 0; x < nmg.ImageWidth; x++)
                {
                    // DEBUG STUFF
                    watch.Start();
                    // DEBUG STUFF END
                    for(int y = 0; y < nmg.ImageHeight; y++)
                    {
                        cToken.ThrowIfCancellationRequested();
                        Color currentPixelColor = nmg.OriginalImageBitmap.GetPixel(x, y);
                        if(IsColorWithinColorDistance(currentPixelColor, ColorType.Background))
                        {
                            nmg.NormalMapImageBitmap.SetPixel(x, y, nmg.NormalMapDefaultBGColor);
                        }
                        else if(IsColorWithinColorDistance(currentPixelColor, ColorType.Individual))
                        {
                            if(HasPixelAlreadyBeenAdded(x, y) == false)
                            {
                                ConvexObject co = new ConvexObject();
                                CheckAdjacentPixels(x, y, co);
                                FloodFill(co);
                                co.CalculateBounds(nmg.ImageWidth, nmg.ImageHeight);
                                AddToTile(co);
                                CreateNormalMapForConvexObject(co);
                            }
                        }
                        CurrentProgress++;
                        progressBarValue.Report(CurrentProgress);
                        percent = (float)CurrentProgress / (float)MaximumPixelsToCheck * 100f;
                        progressLabelDetailText.Report($@"{percent.ToString("00.00")}%  --- {CurrentProgress} / {MaximumPixelsToCheck}");
                    }
                    // DEBUG STUFF
                    watch.Stop();
                    float avgTotalPixelsPerCO = 0;
                    float avgCOPerTile = 0;
                    foreach(KeyValuePair<Vector2, Tile> tile in nmg.Tiles)
                    {
                        avgCOPerTile += tile.Value.ConvexObjects.Count;
                        foreach(ConvexObject co in tile.Value.ConvexObjects)
                        {
                            avgTotalPixelsPerCO += co.EdgePixels.Count + co.IndividualPixels.Count;
                        }
                    }
                    avgTotalPixelsPerCO /= avgCOPerTile;
                    avgCOPerTile /= nmg.Tiles.Count;
                    Console.WriteLine($@"Elapsed: {watch.ElapsedMilliseconds}, Current pos: {x}, Tiles: {nmg.Tiles.Count}, Avg CO per Tile: {avgCOPerTile.ToString("00.00")}, Avg Total Pixels per CO: {avgTotalPixelsPerCO.ToString("00.00")}");
                    // DEBUG STUFF END
                }
                nmg.CreatingNormalMap = false;
            }
            catch(OperationCanceledException)
            {
                MessageBox.Show($@"Operation cancelled!{Environment.NewLine}");
                nmg.CreatingNormalMap = false;
                progressLabelText.Report("Stopped");
            }
            catch(Exception e)
            {
                MessageBox.Show($@"Error creating Normal Map!{Environment.NewLine}{e.ToString()}");
                nmg.CreatingNormalMap = false;
                progressLabelText.Report("Error!");
            }
        }

        /// <summary>
        /// Determines which tile should be parent to this object based on its center, then checks if a parent tile exists at that location, if it does then it adds
        /// this object to that tile, if not then it creates a new tile at that position and adds this object to the tile.
        /// </summary>
        /// <param name="co">The Convex Object to add to a Tile.</param>
        private static void AddToTile(ConvexObject co)
        {
            Vector2 tilePos = co.Center.ConvertToTilePosition();
            if(nmg.Tiles.ContainsKey(tilePos))
            {
                nmg.Tiles[tilePos].AddObject(co);
            }
            else
            {
                nmg.Tiles.Add(tilePos, new Tile(tilePos));
                nmg.Tiles[tilePos].AddObject(co);
            }
        }

        /// <summary>
        /// Checks if a pixel coordinate has already been added to a Convex Object.
        /// </summary>
        /// <param name="x">X coordinate of pixel.</param>
        /// <param name="y">Y coordinate of pixel.</param>
        /// <returns>Returns true if pixel coords have already been added to a Convex Object.</returns>
        private static bool HasPixelAlreadyBeenAdded(int x, int y)
        {
            Vector2 xy = new Vector2(x, y);
            Vector2 tilePos = new Vector2(x, y).ConvertToTilePosition();
            foreach(KeyValuePair<Vector2, Tile> tile in nmg.Tiles)
            {
                if(tilePos.IsWithinDistance(tile.Key, 2))
                {
                    foreach(ConvexObject co in tile.Value.ConvexObjects)
                    {
                        if(co.DoesPixelExistInThisObject(xy) == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a pixel coordinate is within the bounds of the original image.
        /// </summary>
        /// <param name="xy">Vector2 pixel coordinates.</param>
        /// <returns>Returns true if pixel coordinates are within the bounds of the original image.</returns>
        private static bool IsPixelWithinBounds(Vector2 xy)
        {
            if(xy.X >= 0 && xy.X < nmg.ImageWidth && xy.Y >= 0 && xy.Y < nmg.ImageHeight)
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
        private static int GetColorDistance(Color currentColor, Color colorToCheckAgainst)
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
        private static bool IsColorWithinColorDistance(Vector2 xy, ColorType colorType)
        {
            if(colorType == ColorType.Background)
            {
                if(GetColorDistance(nmg.OriginalImageBitmap.GetPixel(xy.X, xy.Y), nmg.BackgroundColor) <= nmg.BackgroundColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Separator)
            {
                if(GetColorDistance(nmg.OriginalImageBitmap.GetPixel(xy.X, xy.Y), nmg.SeparatorColor) <= nmg.SeparatorColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Individual)
            {
                if(GetColorDistance(nmg.OriginalImageBitmap.GetPixel(xy.X, xy.Y), nmg.IndividualColor) <= nmg.IndividualColorMaxDifference)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if given color is within maximum color difference.
        /// </summary>
        /// <param name="color">The color to check.</param>
        /// <param name="colorType">The color type to use maximum difference for.</param>
        /// <returns>Returns true if the color is within max difference allowed.</returns>
        private static bool IsColorWithinColorDistance(Color color, ColorType colorType)
        {
            if(colorType == ColorType.Background)
            {
                if(GetColorDistance(color, nmg.BackgroundColor) <= nmg.BackgroundColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Separator)
            {
                if(GetColorDistance(color, nmg.SeparatorColor) <= nmg.SeparatorColorMaxDifference)
                {
                    return true;
                }
            }
            else if(colorType == ColorType.Individual)
            {
                if(GetColorDistance(color, nmg.IndividualColor) <= nmg.IndividualColorMaxDifference)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds pixels adjacent to the one specified to the list of pixels needing to be checked if they meet the criteria. Used by flood fill.
        /// </summary>
        /// <param name="x">X coordinate of pixel.</param>
        /// <param name="y">Y coordinate of pixel.</param>
        /// <param name="co">Convex Object the parent pixel belongs to.</param>
        private static void CheckAdjacentPixels(int x, int y, ConvexObject co)
        {
            co.AddNewIndividualPixel(new Vector2(x, y));
            co.AddPixelAlreadyChecked(new Vector2(x, y));
            Vector2[] adjacentPixelCoords = new Vector2[4]
            {
                new Vector2(x + 1, y),
                new Vector2(x - 1, y),
                new Vector2(x, y + 1),
                new Vector2(x, y - 1)
            };
            for(int i = 0; i < adjacentPixelCoords.Length; i++)
            {
                if(IsPixelWithinBounds(adjacentPixelCoords[i]) && IsColorWithinColorDistance(adjacentPixelCoords[i], ColorType.Individual) && co.PixelsAlreadyChecked.Contains(adjacentPixelCoords[i]) == false)
                {
                    co.AddPixelToCheck(adjacentPixelCoords[i]);
                }
                else if(IsPixelWithinBounds(adjacentPixelCoords[i]) && IsColorWithinColorDistance(adjacentPixelCoords[i], ColorType.Separator) && co.PixelsAlreadyChecked.Contains(adjacentPixelCoords[i]) == false)
                {
                    co.AddNewEdgePixel(adjacentPixelCoords[i]);
                    co.AddPixelAlreadyChecked(adjacentPixelCoords[i]);
                }
                else
                {
                    co.AddPixelAlreadyChecked(adjacentPixelCoords[i]);
                }
            }
        }

        /// <summary>
        /// Runs the check adjacent pixels method on each pixel that needs to be checked. Works recursively to find all adjacent pixels for a convex object.
        /// </summary>
        /// <param name="co">Convex Object the parent pixel belongs to.</param>
        private static void FloodFill(ConvexObject co)
        {
            // TODO: The average total pixels per CO is showing 300+, this should not be the case, something in this algorithm/CheckAdjacentPixels is bugged and adding too many pixels to the CO, slowdown especiallary around x:305
            while(co.PixelsToCheck.Count > 0)
            {
                List<Vector2> currentPixelsToCheck = co.PixelsToCheck.ToList();
                co.ClearPixelsToCheck();
                foreach(Vector2 pixel in currentPixelsToCheck)
                {
                    CheckAdjacentPixels(pixel.X, pixel.Y, co);
                }
            }
            co.ClearPixelsToCheck();
            co.ClearPixelsAlreadyChecked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="co">Convex Object the parent pixel belongs to.</param>
        private static void CreateNormalMapForConvexObject(ConvexObject co)
        {
            // TODO: Translate normal map color from default normal map by scale to current normal map and add the pixels to bitmap
            // Solution: Loop through all edge pixels, convert them to lines, use change in linear shape to denote vertices, get center point using width and height,
            // assign UV coordinates based off angle from center point to each vertex, use barycentric projection for each pixel in relation to the 2 closest vertices
            // and the center point to get the UV for each pixel, scale default UV map texture to fit width/height of tile object, use UVs to select pixel color
            List<Vector2> allPixels = co.EdgePixels.Concat(co.IndividualPixels).ToList();
            foreach(Vector2 pixel in allPixels)
            {
                nmg.NormalMapImageBitmap.SetPixel(pixel.X, pixel.Y, Color.Red);
            }
        }
    }



    /// <summary>
    /// Class representing a connected group of pixels of varied shape.
    /// </summary>
    public class ConvexObject
    {
        public List<Vector2> EdgePixels { get; private set; }
        public List<Vector2> IndividualPixels { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2 Center { get; private set; }
        public Vector2 ScaleVsDefault { get; private set; }
        public List<Vector2> PixelsAlreadyChecked { get; private set; }
        public List<Vector2> PixelsToCheck { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConvexObject()
        {
            this.EdgePixels = new List<Vector2>();
            this.IndividualPixels = new List<Vector2>();
            this.Width = 0;
            this.Height = 0;
            this.Center = new Vector2(0, 0);
            this.ScaleVsDefault = new Vector2(0, 0);
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
        /// Gets the width, height, center, and scale by iterating through all the pixels in this object.
        /// </summary>
        /// <param name="maxWidth">Maximum width of parent image.</param>
        /// <param name="maxHeight">Maximum height of parent image.</param>
        public void CalculateBounds(int maxWidth, int maxHeight)
        {
            List<Vector2> allPixels = this.EdgePixels.Concat(this.IndividualPixels).ToList();
            int rightEdge = 0, lowerEdge = 0, leftEdge = maxWidth, upperEdge = maxHeight;
            foreach(Vector2 pixel in allPixels)
            {
                int currentPixelX = pixel.X, currentPixelY = pixel.Y;
                if(currentPixelX < leftEdge)
                {
                    leftEdge = currentPixelX;
                }
                if(currentPixelX > rightEdge)
                {
                    rightEdge = currentPixelX;
                }
                if(currentPixelY > lowerEdge)
                {
                    lowerEdge = currentPixelY;
                }
                if(currentPixelY < upperEdge)
                {
                    upperEdge = currentPixelY;
                }
            }
            this.Width = rightEdge - leftEdge;
            this.Height = lowerEdge - upperEdge;
            this.Center = new Vector2((rightEdge + leftEdge) / 2, (lowerEdge + upperEdge) / 2);
        }

        /// <summary>
        /// Checks if a given set of pixel coordinates has been added to the Individual Pixels list of this object.
        /// </summary>
        /// <param name="xy">The Vector2 coordinates for the pixel.</param>
        /// <returns>Returns true if the specified pixel exists in the list of this object.</returns>
        public bool DoesPixelExistInThisObject(Vector2 xy)
        {
            if(this.IndividualPixels.Contains(xy))
            {
                return true;
            }
            return false;
        }
    }



    /// <summary>
    /// Class representing one tile on the tile palette. Stores individual Convex Objects.
    /// </summary>
    public class Tile
    {
        // Tile Dimensions
        public const int TileSize = 32;
        // Address of tile
        public Vector2 Position;
        // Objects within this tiles boundary
        public List<ConvexObject> ConvexObjects = new List<ConvexObject>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position">Vector2 position of tile on tile palette.</param>
        public Tile(Vector2 position)
        {
            this.Position = position;
        }

        /// <summary>
        /// Adds a Convex Object to this Tile's Convex Object list.
        /// </summary>
        /// <param name="co">The Convex Object to add.</param>
        public void AddObject(ConvexObject co)
        {
            this.ConvexObjects.Add(co);
        }
    }



    /// <summary>
    /// Class representing 2 integers: X, Y. Used for pixel coordinates.
    /// </summary>
    public struct Vector2
    {
        // X, Y
        public int X;
        public int Y;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Vector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Checks if this Vector2 is within distance parameter of Vector2 parameter.
        /// </summary>
        /// <param name="xy">The Vector2 to check against.</param>
        /// <param name="distance">The maximum distance allowed between these 2 Vector2.</param>
        /// <returns></returns>
        public bool IsWithinDistance(Vector2 xy, int distance)
        {
            if((((this.X - xy.X) * (this.X - xy.X)) + ((this.Y - xy.Y) * (this.Y - xy.Y))) < distance * distance)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Divides these coordinates by 32 to get tile coordinates.
        /// </summary>
        /// <returns>Returns a new Vector2 corresponding to the tile coordinates the original coordinates belong to.</returns>
        public Vector2 ConvertToTilePosition()
        {
            return new Vector2((int)Math.Floor((float)this.X / (float)Tile.TileSize), (int)Math.Floor((float)this.Y / (float)Tile.TileSize));
        }
    }
}
