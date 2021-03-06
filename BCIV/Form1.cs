﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BCIV
{
    public partial class BCIV_form : Form
    {
        private string[] supportedFormats = new string[]
        {
            "jpg",
            "jpeg",
            "png",
            "bmp",
            "gif",
            "tiff",
            "tif",
        };

        private List<string> images = new List<string>();

        private Image loadedImage;
        private int currentIndex;
        private double zoomScale;
        private bool isGroupImages;

        //Called without init image
        public BCIV_form()
        {
            InitializeComponent();
            init();
        }

        //Called when there is one init image OR a directory
        public BCIV_form(string imagePath)
        {
            InitializeComponent();
            init();

            loadOneImage(imagePath);
        }

        //Called when there are more init images
        public BCIV_form(string[] imagesPath)
        {
            InitializeComponent();
            init();

            loadMultipleImages(imagesPath);
        }

        private void init()
        {
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            imagePanel.AutoScroll = true;

            nextImageButton.Location = new Point(this.Width / 2 + 5, this.Height - 75);
            previousImageButton.Location = new Point(this.Width / 2 - 86, this.Height - 75);

            this.KeyPreview = true;
            editButton.Enabled = false;

            zoomScale = 1.0;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

            this.previousImageButton.MouseWheel += new System.Windows.Forms.MouseEventHandler(mouseWheelEvent);
            this.nextImageButton.MouseWheel += new System.Windows.Forms.MouseEventHandler(mouseWheelEvent);
            this.editButton.MouseWheel += new System.Windows.Forms.MouseEventHandler(mouseWheelEvent);
        }

        //Adds the image to the list and adds all the images in the directory to the list
        private void loadOneImage(string imagePath)
        {
            images.Clear();

            if (File.Exists(imagePath))
            {
                bool isSupported = false;
                isGroupImages = false;

                for (int i = 0; i < supportedFormats.Length; i++)
                {
                    if (imagePath.ToLower().EndsWith(supportedFormats[i]))
                    {
                        isSupported = true;
                        break;
                    }
                }

                if (isSupported)
                {
                    loadAllImagesInDirectory(imagePath);

                    loadImageToPictureBox(images[0]);
                    currentIndex = 0;
                }
                else
                {
                    MessageBox.Show("File format not supported!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (Directory.Exists(imagePath))
            {
                loadAllImagesInDirectory(imagePath);

                if(images.Count > 0)
                {
                    loadImageToPictureBox(images[0]);
                }
            }
        }

        //Adds the path of the file to the list if the file has a supported fileformat
        private void loadMultipleImages(string[] imagesPath)
        {
            images.Clear();

            isGroupImages = true;

            foreach(string filePath in imagesPath)
            {
                for(int i = 0; i < supportedFormats.Length; i++)
                {
                    if(filePath.ToLower().EndsWith(supportedFormats[i]))
                    {
                        images.Add(filePath);
                        break;
                    }
                }
            }

            if(images.Count > 0)
            {
                loadImageToPictureBox(images[0]);
                currentIndex = 0;
            }

            images = images.Distinct().ToList();
        }

        //Load image to pictureBox
        private void loadImageToPictureBox(string imagePath)
        {
            zoomScale = 1.0;

            loadedImage = Image.FromFile(imagePath);
            resizeWindowToLoadedImage();

            int newX = (imagePanel.Width / 2) - (pictureBox.Width / 2);
            int newY = (imagePanel.Height / 2) - (pictureBox.Height / 2);

            pictureBox.Location = new Point(newX, newY);

            //pictureBox.Image = loadedImage;

            this.Text = "[BCIV] - " + Path.GetFileName(images[currentIndex]);
        }

        private void resizeWindowToLoadedImage()
        {
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

            editButton.Enabled = true;

            if (loadedImage.Width > screenWidth || loadedImage.Height > screenHeight)
            {
                float ratio;
                int newWidth, newHeight;
                
                ratio = (float)screenHeight / loadedImage.Height;

                newWidth = (int)(ratio * loadedImage.Width);
                newHeight = (int)(ratio * loadedImage.Height);

                this.Width = newWidth;
                this.Height = newHeight;




//TODO: GIF image resized will display as static image
                //FrameDimension dimension = new FrameDimension(loadedImage.FrameDimensionsList[0]);

                //if(images[currentIndex].ToLower().EndsWith("gif") && loadedImage.GetFrameCount(dimension) > 1)
                //{
                    
                //}



                pictureBox.Image = (Image)(new Bitmap(loadedImage, new Size(imagePanel.Width - 7, imagePanel.Height - 7)));

                this.Location = new Point(this.Location.X, 0);
            }
            else
            {
                this.Width = loadedImage.Width + 50;
                this.Height = loadedImage.Height + 103;

                pictureBox.Image = loadedImage;
            }

            if (this.Location.X + this.Width > screenWidth)
            {
                this.Location = new Point(screenWidth - this.Width, this.Location.Y);
            }

            if(this.Location.X < 0)
            {
                this.Location = new Point(0, this.Location.Y);
            }

            if(this.Location.Y + this.Height > screenHeight)
            {
                this.Location = new Point(this.Location.X, screenHeight - this.Height);
            }
        }

        //Adds all the images in the directory to the list
        private void loadAllImagesInDirectory(string imagePath)
        {
            images.Clear();
            string[] filesInDirectory = null;

            if(File.Exists(imagePath))
            {
                images.Add(imagePath);
                filesInDirectory = supportedFormats.SelectMany(filter => Directory.GetFiles(Path.GetDirectoryName(imagePath), "*." + filter, SearchOption.TopDirectoryOnly)).ToArray();
            }

            if(Directory.Exists(imagePath))
            {
                isGroupImages = true;
                filesInDirectory = supportedFormats.SelectMany(filter => Directory.GetFiles(imagePath, "*." + filter, SearchOption.TopDirectoryOnly)).ToArray();
            }

            images.AddRange(filesInDirectory);
            images = images.Distinct().ToList();
        }

        private void BCIV_form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if(files.Length == 1)
            {
                loadOneImage(files[0]);
            }
            else
            {
                loadMultipleImages(files);
            }
        }

        private void BCIV_form_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void nextImageButton_Click(object sender, EventArgs e)
        {
            if(images.Count > 0)
            {
                if (currentIndex + 1 < images.Count)
                {
                    currentIndex++;
                }
                else
                {
                    currentIndex = 0;
                }

                if(!File.Exists(images[currentIndex]))
                {
                    checkExistingImages();
                }

                loadImageToPictureBox(images[currentIndex]);
            }
        }

        private void previousImageButton_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                }
                else
                {
                    currentIndex = images.Count - 1;
                }

                if (!File.Exists(images[currentIndex]))
                {
                    checkExistingImages();
                }

                loadImageToPictureBox(images[currentIndex]);
            }
        }

        private void checkExistingImages()
        {
            if(isGroupImages)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    if (!File.Exists(images[i]))
                    {
                        images.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                loadAllImagesInDirectory(images[currentIndex]);                
            }

            currentIndex = 0;
        }

        private void BCIV_form_Resize(object sender, EventArgs e)
        {
            

            imagePanel.Size = new Size(this.Width - 40, this.Height - 93);

            nextImageButton.Location = new Point(this.Width / 2 + 5, this.Height - 75);
            previousImageButton.Location = new Point(this.Width / 2 - 86, this.Height - 75);
            editButton.Location = new Point(this.Width - 103, this.Height - 74);

            if (pictureBox.Image == null || loadedImage == null)
            {
                return;
            }
                        
            float ratio;
            int newWidth, newHeight;

            if(pictureBox.Width > pictureBox.Height)
            {                   
                ratio = (float)imagePanel.Width / loadedImage.Width;

                newWidth = (int)(ratio * loadedImage.Width) - 5;
                newHeight = (int)(ratio * loadedImage.Height) - 5;

                if (newHeight > imagePanel.Height)
                {
                    ratio = (float)imagePanel.Height / loadedImage.Height;

                    newWidth = (int)(ratio * loadedImage.Width) - 5;
                    newHeight = (int)(ratio * loadedImage.Height) - 5;
                }
            }
            else
            {
                ratio = (float)imagePanel.Height / loadedImage.Height;

                newWidth = (int)(ratio * loadedImage.Width) - 5;
                newHeight = (int)(ratio * loadedImage.Height) - 5;

                if (newWidth > imagePanel.Width)
                {
                    ratio = (float)imagePanel.Width / loadedImage.Width;

                    newWidth = (int)(ratio * loadedImage.Width) - 5;
                    newHeight = (int)(ratio * loadedImage.Height) - 5;
                }
            }

            int newX = (imagePanel.Width / 2) - (pictureBox.Width / 2);
            int newY = (imagePanel.Height / 2) - (pictureBox.Height / 2);

            pictureBox.Location = new Point(newX, newY);

            if (zoomScale != 1.0)
            {
                return;
            }

            pictureBox.Image = (Image)(new Bitmap(loadedImage, new Size(newWidth, newHeight)));
        }

        private void previousImageButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            keyEvents(e.KeyCode);
        }

        private void nextImageButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            keyEvents(e.KeyCode);
        }
                
        private void editButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            keyEvents(e.KeyCode);
        }
        
        private void keyEvents(Keys key)
        {
            if (key == Keys.Left || key == Keys.Up)
            {
                previousImageButton_Click(null, null);
            }
            else if (key == Keys.Right || key == Keys.Down)
            {
                nextImageButton_Click(null, null);
            }
            else if (key == Keys.Add)
            {
                zoomImage();
            }
            else if (key == Keys.Subtract)
            {
                unzoomImage();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mspaint.exe", images[currentIndex]);
        }

        private void mouseWheelEvent(object sender, MouseEventArgs e)
        {
            if(e.Delta * SystemInformation.MouseWheelScrollLines / 120 > 0)
            {
                zoomImage();
            }
            else
            {
                unzoomImage();
            }
        }

        private void zoomImage()
        {
            if (loadedImage != null)
            {
                zoomScale += 0.1;
                //this.Size = new Size(this.Width + zoomScale * 10, this.Height + zoomScale * 10);

                pictureBox.Image = (Image)(new Bitmap(loadedImage, new Size((int)(loadedImage.Width * zoomScale), (int)(loadedImage.Height * zoomScale))));

                //resizeWindowToPictureBoxImage();
            }
        }

        private void unzoomImage()
        {
            if (loadedImage != null)
            {
                zoomScale -= 0.1;
                //this.Size = new Size(this.Width + zoomScale * 10, this.Height + zoomScale * 10);

                pictureBox.Image = (Image)(new Bitmap(loadedImage, new Size((int)(loadedImage.Width * zoomScale), (int)(loadedImage.Height * zoomScale))));

                //resizeWindowToPictureBoxImage();
            }
        }
    }
}
