namespace PixelArtTileNormalMapGenerator
{
    partial class NormalMapGeneratorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OriginalImagePreviewBox = new System.Windows.Forms.PictureBox();
            this.NormalMapImagePreviewBox = new System.Windows.Forms.PictureBox();
            this.LoadImageButton = new System.Windows.Forms.Button();
            this.GenerateNormalMapButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.SeparatorColorDialog = new System.Windows.Forms.ColorDialog();
            this.IndividualColorDialog = new System.Windows.Forms.ColorDialog();
            this.SelectBackgroundColorButton = new System.Windows.Forms.Button();
            this.SelectSeparatorColorButton = new System.Windows.Forms.Button();
            this.SelectIndividualColorButton = new System.Windows.Forms.Button();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BackgroundColorBox = new System.Windows.Forms.PictureBox();
            this.SeparatorColorBox = new System.Windows.Forms.PictureBox();
            this.IndividualColorBox = new System.Windows.Forms.PictureBox();
            this.OriginalImageLabel = new System.Windows.Forms.Label();
            this.NormalMapImageLabel = new System.Windows.Forms.Label();
            this.BackgroundColorMaxDifferencePicker = new System.Windows.Forms.NumericUpDown();
            this.SeparatorColorMaxDifferencePicker = new System.Windows.Forms.NumericUpDown();
            this.IndividualColorMaxDifferencePicker = new System.Windows.Forms.NumericUpDown();
            this.NormalMapGenerationProgressBar = new System.Windows.Forms.ProgressBar();
            this.NormalMapGenerationProgressLabel = new System.Windows.Forms.Label();
            this.NormalMapGenerationProgressDetailLabel = new System.Windows.Forms.Label();
            this.CancelGeneratorButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.OriginalImagePreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NormalMapImagePreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SeparatorColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorMaxDifferencePicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SeparatorColorMaxDifferencePicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualColorMaxDifferencePicker)).BeginInit();
            this.SuspendLayout();
            // 
            // OriginalImagePreviewBox
            // 
            this.OriginalImagePreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OriginalImagePreviewBox.Location = new System.Drawing.Point(207, 47);
            this.OriginalImagePreviewBox.Name = "OriginalImagePreviewBox";
            this.OriginalImagePreviewBox.Size = new System.Drawing.Size(512, 512);
            this.OriginalImagePreviewBox.TabIndex = 0;
            this.OriginalImagePreviewBox.TabStop = false;
            // 
            // NormalMapImagePreviewBox
            // 
            this.NormalMapImagePreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NormalMapImagePreviewBox.Location = new System.Drawing.Point(745, 47);
            this.NormalMapImagePreviewBox.Name = "NormalMapImagePreviewBox";
            this.NormalMapImagePreviewBox.Size = new System.Drawing.Size(512, 512);
            this.NormalMapImagePreviewBox.TabIndex = 1;
            this.NormalMapImagePreviewBox.TabStop = false;
            // 
            // LoadImageButton
            // 
            this.LoadImageButton.Location = new System.Drawing.Point(25, 28);
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.Size = new System.Drawing.Size(128, 23);
            this.LoadImageButton.TabIndex = 2;
            this.LoadImageButton.Text = "Load Image";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // GenerateNormalMapButton
            // 
            this.GenerateNormalMapButton.Location = new System.Drawing.Point(25, 476);
            this.GenerateNormalMapButton.Name = "GenerateNormalMapButton";
            this.GenerateNormalMapButton.Size = new System.Drawing.Size(129, 23);
            this.GenerateNormalMapButton.TabIndex = 3;
            this.GenerateNormalMapButton.Text = "Generate Normal Map";
            this.GenerateNormalMapButton.UseVisualStyleBackColor = true;
            this.GenerateNormalMapButton.Click += new System.EventHandler(this.GenerateNormalMapButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(25, 536);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(128, 23);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // SelectBackgroundColorButton
            // 
            this.SelectBackgroundColorButton.Location = new System.Drawing.Point(25, 143);
            this.SelectBackgroundColorButton.Name = "SelectBackgroundColorButton";
            this.SelectBackgroundColorButton.Size = new System.Drawing.Size(129, 23);
            this.SelectBackgroundColorButton.TabIndex = 6;
            this.SelectBackgroundColorButton.Text = "Select Background Color";
            this.SelectBackgroundColorButton.UseVisualStyleBackColor = true;
            this.SelectBackgroundColorButton.Click += new System.EventHandler(this.SelectBackgroundColorButton_Click);
            // 
            // SelectSeparatorColorButton
            // 
            this.SelectSeparatorColorButton.Location = new System.Drawing.Point(24, 198);
            this.SelectSeparatorColorButton.Name = "SelectSeparatorColorButton";
            this.SelectSeparatorColorButton.Size = new System.Drawing.Size(129, 23);
            this.SelectSeparatorColorButton.TabIndex = 7;
            this.SelectSeparatorColorButton.Text = "Select Separator Color";
            this.SelectSeparatorColorButton.UseVisualStyleBackColor = true;
            this.SelectSeparatorColorButton.Click += new System.EventHandler(this.SelectSeparatorColorButton_Click);
            // 
            // SelectIndividualColorButton
            // 
            this.SelectIndividualColorButton.Location = new System.Drawing.Point(25, 253);
            this.SelectIndividualColorButton.Name = "SelectIndividualColorButton";
            this.SelectIndividualColorButton.Size = new System.Drawing.Size(129, 23);
            this.SelectIndividualColorButton.TabIndex = 8;
            this.SelectIndividualColorButton.Text = "Select Individual Color";
            this.SelectIndividualColorButton.UseVisualStyleBackColor = true;
            this.SelectIndividualColorButton.Click += new System.EventHandler(this.SelectIndividualColorButton_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "OpenFileDialog";
            // 
            // BackgroundColorBox
            // 
            this.BackgroundColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BackgroundColorBox.Location = new System.Drawing.Point(160, 143);
            this.BackgroundColorBox.Name = "BackgroundColorBox";
            this.BackgroundColorBox.Size = new System.Drawing.Size(25, 23);
            this.BackgroundColorBox.TabIndex = 10;
            this.BackgroundColorBox.TabStop = false;
            // 
            // SeparatorColorBox
            // 
            this.SeparatorColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SeparatorColorBox.Location = new System.Drawing.Point(159, 198);
            this.SeparatorColorBox.Name = "SeparatorColorBox";
            this.SeparatorColorBox.Size = new System.Drawing.Size(25, 22);
            this.SeparatorColorBox.TabIndex = 11;
            this.SeparatorColorBox.TabStop = false;
            // 
            // IndividualColorBox
            // 
            this.IndividualColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IndividualColorBox.Location = new System.Drawing.Point(160, 253);
            this.IndividualColorBox.Name = "IndividualColorBox";
            this.IndividualColorBox.Size = new System.Drawing.Size(25, 23);
            this.IndividualColorBox.TabIndex = 12;
            this.IndividualColorBox.TabStop = false;
            // 
            // OriginalImageLabel
            // 
            this.OriginalImageLabel.AutoSize = true;
            this.OriginalImageLabel.Location = new System.Drawing.Point(426, 28);
            this.OriginalImageLabel.Name = "OriginalImageLabel";
            this.OriginalImageLabel.Size = new System.Drawing.Size(74, 13);
            this.OriginalImageLabel.TabIndex = 13;
            this.OriginalImageLabel.Text = "Original Image";
            // 
            // NormalMapImageLabel
            // 
            this.NormalMapImageLabel.AutoSize = true;
            this.NormalMapImageLabel.Location = new System.Drawing.Point(963, 28);
            this.NormalMapImageLabel.Name = "NormalMapImageLabel";
            this.NormalMapImageLabel.Size = new System.Drawing.Size(64, 13);
            this.NormalMapImageLabel.TabIndex = 14;
            this.NormalMapImageLabel.Text = "Normal Map";
            // 
            // BackgroundColorMaxDifferencePicker
            // 
            this.BackgroundColorMaxDifferencePicker.Location = new System.Drawing.Point(135, 172);
            this.BackgroundColorMaxDifferencePicker.Name = "BackgroundColorMaxDifferencePicker";
            this.BackgroundColorMaxDifferencePicker.Size = new System.Drawing.Size(50, 20);
            this.BackgroundColorMaxDifferencePicker.TabIndex = 16;
            // 
            // SeparatorColorMaxDifferencePicker
            // 
            this.SeparatorColorMaxDifferencePicker.Location = new System.Drawing.Point(135, 227);
            this.SeparatorColorMaxDifferencePicker.Name = "SeparatorColorMaxDifferencePicker";
            this.SeparatorColorMaxDifferencePicker.Size = new System.Drawing.Size(49, 20);
            this.SeparatorColorMaxDifferencePicker.TabIndex = 17;
            // 
            // IndividualColorMaxDifferencePicker
            // 
            this.IndividualColorMaxDifferencePicker.Location = new System.Drawing.Point(135, 282);
            this.IndividualColorMaxDifferencePicker.Name = "IndividualColorMaxDifferencePicker";
            this.IndividualColorMaxDifferencePicker.Size = new System.Drawing.Size(50, 20);
            this.IndividualColorMaxDifferencePicker.TabIndex = 18;
            // 
            // NormalMapGenerationProgressBar
            // 
            this.NormalMapGenerationProgressBar.Location = new System.Drawing.Point(400, 571);
            this.NormalMapGenerationProgressBar.Name = "NormalMapGenerationProgressBar";
            this.NormalMapGenerationProgressBar.Size = new System.Drawing.Size(100, 23);
            this.NormalMapGenerationProgressBar.TabIndex = 19;
            // 
            // NormalMapGenerationProgressLabel
            // 
            this.NormalMapGenerationProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NormalMapGenerationProgressLabel.AutoSize = true;
            this.NormalMapGenerationProgressLabel.Location = new System.Drawing.Point(316, 574);
            this.NormalMapGenerationProgressLabel.Name = "NormalMapGenerationProgressLabel";
            this.NormalMapGenerationProgressLabel.Size = new System.Drawing.Size(78, 13);
            this.NormalMapGenerationProgressLabel.TabIndex = 21;
            this.NormalMapGenerationProgressLabel.Text = "Not processing";
            // 
            // NormalMapGenerationProgressDetailLabel
            // 
            this.NormalMapGenerationProgressDetailLabel.AutoSize = true;
            this.NormalMapGenerationProgressDetailLabel.Location = new System.Drawing.Point(506, 574);
            this.NormalMapGenerationProgressDetailLabel.Name = "NormalMapGenerationProgressDetailLabel";
            this.NormalMapGenerationProgressDetailLabel.Size = new System.Drawing.Size(59, 13);
            this.NormalMapGenerationProgressDetailLabel.TabIndex = 22;
            this.NormalMapGenerationProgressDetailLabel.Text = "0% --- 0 / 0";
            // 
            // CancelGeneratorButton
            // 
            this.CancelGeneratorButton.Location = new System.Drawing.Point(25, 506);
            this.CancelGeneratorButton.Name = "CancelGeneratorButton";
            this.CancelGeneratorButton.Size = new System.Drawing.Size(128, 23);
            this.CancelGeneratorButton.TabIndex = 23;
            this.CancelGeneratorButton.Text = "Cancel";
            this.CancelGeneratorButton.UseVisualStyleBackColor = true;
            this.CancelGeneratorButton.Click += new System.EventHandler(this.CancelGeneratorButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(950, 569);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(118, 23);
            this.SaveButton.TabIndex = 24;
            this.SaveButton.Text = "Save Normal Map";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // NormalMapGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 606);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CancelGeneratorButton);
            this.Controls.Add(this.NormalMapGenerationProgressDetailLabel);
            this.Controls.Add(this.NormalMapGenerationProgressLabel);
            this.Controls.Add(this.NormalMapGenerationProgressBar);
            this.Controls.Add(this.IndividualColorMaxDifferencePicker);
            this.Controls.Add(this.SeparatorColorMaxDifferencePicker);
            this.Controls.Add(this.BackgroundColorMaxDifferencePicker);
            this.Controls.Add(this.NormalMapImageLabel);
            this.Controls.Add(this.OriginalImageLabel);
            this.Controls.Add(this.IndividualColorBox);
            this.Controls.Add(this.SeparatorColorBox);
            this.Controls.Add(this.BackgroundColorBox);
            this.Controls.Add(this.SelectIndividualColorButton);
            this.Controls.Add(this.SelectSeparatorColorButton);
            this.Controls.Add(this.SelectBackgroundColorButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.GenerateNormalMapButton);
            this.Controls.Add(this.LoadImageButton);
            this.Controls.Add(this.NormalMapImagePreviewBox);
            this.Controls.Add(this.OriginalImagePreviewBox);
            this.Name = "NormalMapGeneratorForm";
            this.Text = "Pixel Art Tile Normal Map Generator";
            this.Load += new System.EventHandler(this.NormalMapGeneratorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OriginalImagePreviewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NormalMapImagePreviewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SeparatorColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorMaxDifferencePicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SeparatorColorMaxDifferencePicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualColorMaxDifferencePicker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox OriginalImagePreviewBox;
        private System.Windows.Forms.PictureBox NormalMapImagePreviewBox;
        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.Button GenerateNormalMapButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private System.Windows.Forms.ColorDialog SeparatorColorDialog;
        private System.Windows.Forms.ColorDialog IndividualColorDialog;
        private System.Windows.Forms.Button SelectBackgroundColorButton;
        private System.Windows.Forms.Button SelectSeparatorColorButton;
        private System.Windows.Forms.Button SelectIndividualColorButton;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.PictureBox BackgroundColorBox;
        private System.Windows.Forms.PictureBox SeparatorColorBox;
        private System.Windows.Forms.PictureBox IndividualColorBox;
        private System.Windows.Forms.Label OriginalImageLabel;
        private System.Windows.Forms.Label NormalMapImageLabel;
        private System.Windows.Forms.NumericUpDown BackgroundColorMaxDifferencePicker;
        private System.Windows.Forms.NumericUpDown SeparatorColorMaxDifferencePicker;
        private System.Windows.Forms.NumericUpDown IndividualColorMaxDifferencePicker;
        private System.Windows.Forms.ProgressBar NormalMapGenerationProgressBar;
        private System.Windows.Forms.Label NormalMapGenerationProgressLabel;
        private System.Windows.Forms.Label NormalMapGenerationProgressDetailLabel;
        private System.Windows.Forms.Button CancelGeneratorButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}

