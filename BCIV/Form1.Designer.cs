namespace BCIV
{
    partial class BCIV_form
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
            if (disposing && (components != null))
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
            this.imagePanel = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.previousImageButton = new System.Windows.Forms.Button();
            this.nextImageButton = new System.Windows.Forms.Button();
            this.imagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imagePanel
            // 
            this.imagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imagePanel.Controls.Add(this.pictureBox);
            this.imagePanel.Location = new System.Drawing.Point(12, 12);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(760, 537);
            this.imagePanel.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(4, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(753, 530);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // previousImageButton
            // 
            this.previousImageButton.Location = new System.Drawing.Point(314, 555);
            this.previousImageButton.Name = "previousImageButton";
            this.previousImageButton.Size = new System.Drawing.Size(75, 22);
            this.previousImageButton.TabIndex = 1;
            this.previousImageButton.Text = "Previous";
            this.previousImageButton.UseVisualStyleBackColor = true;
            this.previousImageButton.Click += new System.EventHandler(this.previousImageButton_Click);
            this.previousImageButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.previousImageButton_PreviewKeyDown);
            // 
            // nextImageButton
            // 
            this.nextImageButton.Location = new System.Drawing.Point(395, 555);
            this.nextImageButton.Name = "nextImageButton";
            this.nextImageButton.Size = new System.Drawing.Size(75, 22);
            this.nextImageButton.TabIndex = 2;
            this.nextImageButton.Text = "Next";
            this.nextImageButton.UseVisualStyleBackColor = true;
            this.nextImageButton.Click += new System.EventHandler(this.nextImageButton_Click);
            this.nextImageButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.nextImageButton_PreviewKeyDown);
            // 
            // BCIV_form
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 591);
            this.Controls.Add(this.nextImageButton);
            this.Controls.Add(this.previousImageButton);
            this.Controls.Add(this.imagePanel);
            this.MinimumSize = new System.Drawing.Size(320, 240);
            this.Name = "BCIV_form";
            this.Text = "BCIV";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.BCIV_form_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.BCIV_form_DragEnter);
            this.Resize += new System.EventHandler(this.BCIV_form_Resize);
            this.imagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button previousImageButton;
        private System.Windows.Forms.Button nextImageButton;
    }
}

