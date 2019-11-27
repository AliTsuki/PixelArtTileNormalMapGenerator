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
            this.IgnoreColorDialog = new System.Windows.Forms.ColorDialog();
            this.GenerateNormalMapButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.IgnoreColorDialogButton = new System.Windows.Forms.Button();
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.SeparatorColorDialog = new System.Windows.Forms.ColorDialog();
            this.IndividualColorDialog = new System.Windows.Forms.ColorDialog();
            this.SelectBackgroundColorButton = new System.Windows.Forms.Button();
            this.SelectSeparatorColorButton = new System.Windows.Forms.Button();
            this.SelectIndividualColorButton = new System.Windows.Forms.Button();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.IgnoreColorBox = new System.Windows.Forms.PictureBox();
            this.BackgroundColorBox = new System.Windows.Forms.PictureBox();
            this.SeparatorColorBox = new System.Windows.Forms.PictureBox();
            this.IndividualColorBox = new System.Windows.Forms.PictureBox();
            this.OriginalImageLabel = new System.Windows.Forms.Label();
            this.NormalMapImageLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OriginalImagePreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NormalMapImagePreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IgnoreColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SeparatorColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualColorBox)).BeginInit();
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
            this.LoadImageButton.Location = new System.Drawing.Point(25, 47);
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.Size = new System.Drawing.Size(128, 23);
            this.LoadImageButton.TabIndex = 2;
            this.LoadImageButton.Text = "Load Image";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // GenerateNormalMapButton
            // 
            this.GenerateNormalMapButton.Location = new System.Drawing.Point(25, 383);
            this.GenerateNormalMapButton.Name = "GenerateNormalMapButton";
            this.GenerateNormalMapButton.Size = new System.Drawing.Size(128, 23);
            this.GenerateNormalMapButton.TabIndex = 3;
            this.GenerateNormalMapButton.Text = "Generate Normal Map";
            this.GenerateNormalMapButton.UseVisualStyleBackColor = true;
            this.GenerateNormalMapButton.Click += new System.EventHandler(this.GenerateNormalMapButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(50, 429);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // IgnoreColorDialogButton
            // 
            this.IgnoreColorDialogButton.Location = new System.Drawing.Point(24, 120);
            this.IgnoreColorDialogButton.Name = "IgnoreColorDialogButton";
            this.IgnoreColorDialogButton.Size = new System.Drawing.Size(129, 25);
            this.IgnoreColorDialogButton.TabIndex = 5;
            this.IgnoreColorDialogButton.Text = "Select Ignore Color";
            this.IgnoreColorDialogButton.UseVisualStyleBackColor = true;
            this.IgnoreColorDialogButton.Click += new System.EventHandler(this.IgnoreColorDialogButton_Click);
            // 
            // SelectBackgroundColorButton
            // 
            this.SelectBackgroundColorButton.Location = new System.Drawing.Point(24, 152);
            this.SelectBackgroundColorButton.Name = "SelectBackgroundColorButton";
            this.SelectBackgroundColorButton.Size = new System.Drawing.Size(129, 23);
            this.SelectBackgroundColorButton.TabIndex = 6;
            this.SelectBackgroundColorButton.Text = "Select Background Color";
            this.SelectBackgroundColorButton.UseVisualStyleBackColor = true;
            this.SelectBackgroundColorButton.Click += new System.EventHandler(this.SelectBackgroundColorButton_Click);
            // 
            // SelectSeparatorColorButton
            // 
            this.SelectSeparatorColorButton.Location = new System.Drawing.Point(24, 182);
            this.SelectSeparatorColorButton.Name = "SelectSeparatorColorButton";
            this.SelectSeparatorColorButton.Size = new System.Drawing.Size(129, 23);
            this.SelectSeparatorColorButton.TabIndex = 7;
            this.SelectSeparatorColorButton.Text = "Select Separator Color";
            this.SelectSeparatorColorButton.UseVisualStyleBackColor = true;
            this.SelectSeparatorColorButton.Click += new System.EventHandler(this.SelectSeparatorColorButton_Click);
            // 
            // SelectIndividualColorButton
            // 
            this.SelectIndividualColorButton.Location = new System.Drawing.Point(24, 212);
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
            // IgnoreColorBox
            // 
            this.IgnoreColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IgnoreColorBox.Location = new System.Drawing.Point(160, 120);
            this.IgnoreColorBox.Name = "IgnoreColorBox";
            this.IgnoreColorBox.Size = new System.Drawing.Size(25, 25);
            this.IgnoreColorBox.TabIndex = 9;
            this.IgnoreColorBox.TabStop = false;
            // 
            // BackgroundColorBox
            // 
            this.BackgroundColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BackgroundColorBox.Location = new System.Drawing.Point(160, 152);
            this.BackgroundColorBox.Name = "BackgroundColorBox";
            this.BackgroundColorBox.Size = new System.Drawing.Size(25, 23);
            this.BackgroundColorBox.TabIndex = 10;
            this.BackgroundColorBox.TabStop = false;
            // 
            // SeparatorColorBox
            // 
            this.SeparatorColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SeparatorColorBox.Location = new System.Drawing.Point(160, 182);
            this.SeparatorColorBox.Name = "SeparatorColorBox";
            this.SeparatorColorBox.Size = new System.Drawing.Size(25, 22);
            this.SeparatorColorBox.TabIndex = 11;
            this.SeparatorColorBox.TabStop = false;
            // 
            // IndividualColorBox
            // 
            this.IndividualColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IndividualColorBox.Location = new System.Drawing.Point(160, 212);
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
            // NormalMapGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 606);
            this.Controls.Add(this.NormalMapImageLabel);
            this.Controls.Add(this.OriginalImageLabel);
            this.Controls.Add(this.IndividualColorBox);
            this.Controls.Add(this.SeparatorColorBox);
            this.Controls.Add(this.BackgroundColorBox);
            this.Controls.Add(this.IgnoreColorBox);
            this.Controls.Add(this.SelectIndividualColorButton);
            this.Controls.Add(this.SelectSeparatorColorButton);
            this.Controls.Add(this.SelectBackgroundColorButton);
            this.Controls.Add(this.IgnoreColorDialogButton);
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
            ((System.ComponentModel.ISupportInitialize)(this.IgnoreColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SeparatorColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualColorBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox OriginalImagePreviewBox;
        private System.Windows.Forms.PictureBox NormalMapImagePreviewBox;
        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.ColorDialog IgnoreColorDialog;
        private System.Windows.Forms.Button GenerateNormalMapButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button IgnoreColorDialogButton;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private System.Windows.Forms.ColorDialog SeparatorColorDialog;
        private System.Windows.Forms.ColorDialog IndividualColorDialog;
        private System.Windows.Forms.Button SelectBackgroundColorButton;
        private System.Windows.Forms.Button SelectSeparatorColorButton;
        private System.Windows.Forms.Button SelectIndividualColorButton;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.PictureBox IgnoreColorBox;
        private System.Windows.Forms.PictureBox BackgroundColorBox;
        private System.Windows.Forms.PictureBox SeparatorColorBox;
        private System.Windows.Forms.PictureBox IndividualColorBox;
        private System.Windows.Forms.Label OriginalImageLabel;
        private System.Windows.Forms.Label NormalMapImageLabel;
    }
}

