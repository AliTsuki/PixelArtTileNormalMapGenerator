using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        public static string DefaultNormalMapImageDirectoryPath = Application.StartupPath + "\\DefaultNormal\\";
        public static string DefaultNormalMapImageName = "DefaultNormal.png";
        // Bitmaps
        public static Bitmap OriginalImageBitmap;
        public static Bitmap DefaultNormalMapImageBitmap;
        public static Bitmap NormalMapImageBitmap;
        // Dimensions
        public static int ImageWidth;
        public static int ImageHeight;
        public static int DefaultNormalMapImageSize = 0;
        // Colors
        public static Color DefaultNormalMapBGColor;
        public static Color BackgroundColor;
        public static Color SeparatorColor;
        public static Color IndividualColor;
        // Difference checks
        public static int BackgroundColorMaxDifference;
        public static int SeparatorColorMaxDifference;
        public static int IndividualColorMaxDifference;
        // Lists
        public static Dictionary<Vector2Int, Tile> Tiles = new Dictionary<Vector2Int, Tile>();
        // Flags
        public static bool CreatingNormalMap = false;
        // Cancellation tokens
        public static CancellationTokenSource cTokenSource = new CancellationTokenSource();
        public static CancellationToken cToken = new CancellationToken();

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
            OriginalImageBitmap = null;
            this.OriginalImagePreviewBox.Image = OriginalImageBitmap;
            NormalMapImageBitmap = null;
            this.NormalMapImagePreviewBox.Image = NormalMapImageBitmap;
            DefaultNormalMapBGColor = Color.FromArgb(255, 118, 137, 249);
            BackgroundColor = Color.FromArgb(255, 240, 202, 163);
            this.BackgroundColorBox.BackColor = BackgroundColor;
            SeparatorColor = Color.FromArgb(255, 95, 52, 33);
            this.SeparatorColorBox.BackColor = SeparatorColor;
            IndividualColor = Color.FromArgb(255, 199, 119, 82);
            this.IndividualColorBox.BackColor = IndividualColor;
            BackgroundColorMaxDifference = 10;
            this.BackgroundColorMaxDifferencePicker.Value = BackgroundColorMaxDifference;
            SeparatorColorMaxDifference = 10;
            this.SeparatorColorMaxDifferencePicker.Value = SeparatorColorMaxDifference;
            IndividualColorMaxDifference = 10;
            this.IndividualColorMaxDifferencePicker.Value = IndividualColorMaxDifference;
            this.NormalMapGenerationProgressLabel.Text = "Idle";
            this.NormalMapGenerationProgressBar.Value = 0;
            this.NormalMapGenerationProgressDetailLabel.Text = @"0% --- 0 / 0";
            Tiles.Clear();
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
                    OriginalImageBitmap = new Bitmap(this.OpenFileDialog.FileName);
                    this.OriginalImagePreviewBox.Image = OriginalImageBitmap;
                    this.OriginalImagePreviewBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    ImageWidth = OriginalImageBitmap.Width;
                    ImageHeight = OriginalImageBitmap.Height;
                }
                catch(Exception)
                {
                    MessageBox.Show($@"Unable to read image!{Environment.NewLine}Please select a valid image format.{Environment.NewLine}For best results use PNG format.");
                }
            }
        }

        // TODO: Tidy up the form and make it look nicer, make sure elements are aligned etc.
        // TODO: Add a better way to select colors, ideas: color dropper select color from image preview box, a dropdown of all the colors in the image to select from etc.

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
                BackgroundColor = this.BackgroundColorDialog.Color;
                this.BackgroundColorBox.BackColor = BackgroundColor;
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
                SeparatorColor = this.SeparatorColorDialog.Color;
                this.SeparatorColorBox.BackColor = SeparatorColor;
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
                IndividualColor = this.IndividualColorDialog.Color;
                this.IndividualColorBox.BackColor = IndividualColor;
            }
        }

        /// <summary>
        /// Starts the normal map generation process and displays created normal map to normal map preview box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GenerateNormalMapButton_Click(object sender, EventArgs args)
        {
            BackgroundColorMaxDifference = (int)this.BackgroundColorMaxDifferencePicker.Value;
            SeparatorColorMaxDifference = (int)this.SeparatorColorMaxDifferencePicker.Value;
            IndividualColorMaxDifference = (int)this.IndividualColorMaxDifferencePicker.Value;
            if(CreatingNormalMap == false)
            {
                if(OriginalImageBitmap != null)
                {
                    NormalMapImageBitmap = new Bitmap(ImageWidth, ImageHeight);
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
            if(CreatingNormalMap == true)
            {
                cTokenSource.Cancel();
            }
            else
            {
                MessageBox.Show("No operation currently running to cancel.");
            }
        }

        /// <summary>
        /// Saves the current normal map to disk.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(NormalMapImageBitmap != null)
            {
                if(CreatingNormalMap == false)
                {
                    this.SaveFileDialog.Filter = "PNG Image (*.PNG, *.png)|";
                    this.SaveFileDialog.Title = "Save Normal Map...";
                    this.SaveFileDialog.FileName = Path.GetFileNameWithoutExtension(this.OpenFileDialog.FileName) + " Normal.PNG";
                    DialogResult result = this.SaveFileDialog.ShowDialog();
                    if(result == DialogResult.OK)
                    {
                        NormalMapImageBitmap.Save(this.SaveFileDialog.FileName, ImageFormat.Png);
                    }
                }
                else
                {
                    MessageBox.Show($@"The normal map is currently being generated!{Environment.NewLine}Please wait until it finishes to save.");
                }
            }
            else
            {
                MessageBox.Show($@"There is no normal map to save!{Environment.NewLine}Please generate a normal map first.");
            }
        }

        /// <summary>
        /// Loads the default normal map texture as bitmap.
        /// </summary>
        private void LoadDefaultNormal()
        {
            try
            {
                if(Directory.Exists(DefaultNormalMapImageDirectoryPath))
                {
                    DefaultNormalMapImageBitmap = new Bitmap(DefaultNormalMapImageDirectoryPath + DefaultNormalMapImageName);
                    DefaultNormalMapImageSize = DefaultNormalMapImageBitmap.Width;
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

        /// <summary>
        /// Creates a normal map from an original image using the selected colors to traverse.
        /// </summary>
        private async void CreateNormalMap()
        {
            Tiles.Clear();
            CreatingNormalMap = true;
            cToken = cTokenSource.Token;
            this.NormalMapGenerationProgressBar.Minimum = 0;
            this.NormalMapGenerationProgressBar.Maximum = (ImageWidth * ImageHeight) + 1;
            this.NormalMapGenerationProgressBar.Step = this.NormalMapGenerationProgressBar.Maximum / 100;
            Progress<string> nmgProgressLabelText = new Progress<string>(s => this.NormalMapGenerationProgressLabel.Text = s);
            Progress<string> nmgProgressLabelDetailText = new Progress<string>(s => this.NormalMapGenerationProgressDetailLabel.Text = s);
            Progress<int> nmgProgressBarValue = new Progress<int>(i => this.NormalMapGenerationProgressBar.Value = i);
            await Task.Factory.StartNew(() => GeneratorWorker.CreateNormalMap(nmgProgressLabelText, nmgProgressBarValue, nmgProgressLabelDetailText), TaskCreationOptions.RunContinuationsAsynchronously);
            this.UpdateNormalMapPreviewBox();
            Tiles.Clear();
        }

        /// <summary>
        /// Updates the normal map image preview box with the current normal map bitmap image.
        /// </summary>
        public void UpdateNormalMapPreviewBox()
        {
            this.NormalMapImagePreviewBox.Image = NormalMapImageBitmap;
        }
    }
}
