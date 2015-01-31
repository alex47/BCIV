using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        //Called without init image
        public BCIV_form()
        {
            InitializeComponent();
            init();
        }

        //Called when there is one init image and I expect this to be the most case
        public BCIV_form(string imagePath)
        {
            InitializeComponent();
            init();
        }

        //Called when there are more init images
        public BCIV_form(string[] imagesPath)
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            imagePanel.AutoScroll = true;

            nextImageButton.Location = new Point(this.Width / 2 + 5, this.Height - 75);
            previousImageButton.Location = new Point(this.Width / 2 - 86, this.Height - 75);

//TODO: set window start location to (0, 0)            
        }

        //Adds the image to the list and adds all the images in the directory to the list
        private void loadOneImage(string imagePath)
        {
            bool isSupported = false;

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

                //resizeWindowToLoadedImage();
            }
            else
            {
                MessageBox.Show("File format not supported!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //Adds the path of the file to the list if the file has a supported fileformat
        private void loadMultipleImages(string[] imagesPath)
        {
            images.Clear();

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

                //resizeWindowToLoadedImage();
            }

            images = images.Distinct().ToList();
        }

        //Load image to pictureBox
        private void loadImageToPictureBox(string imagePath)
        {
            loadedImage = Image.FromFile(imagePath);
            resizeWindowToLoadedImage();

            pictureBox.Image = loadedImage;            
        }

        private void resizeWindowToLoadedImage()
        {
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

            if (loadedImage.Width > screenWidth || loadedImage.Height > screenHeight)
            {
                int newWidth, newHeight;

                if (loadedImage.Width > loadedImage.Height)
                {
                    float ratio = (float)screenHeight / loadedImage.Height;

                    newWidth = (int)(ratio * loadedImage.Width);
                    newHeight = (int)(ratio * loadedImage.Height);

                    //Some resolutions work in a mysterious way
                    if (newWidth < screenWidth)
                    {
                        ratio = (float)screenWidth / loadedImage.Width;

                        newWidth = (int)(ratio * loadedImage.Width);
                        newHeight = (int)(ratio * loadedImage.Height);
                    }
                }
                else
                {
                    float ratio = (float)screenWidth / loadedImage.Width;

                    newWidth = (int)(ratio * loadedImage.Width);
                    newHeight = (int)(ratio * loadedImage.Height);


                    if (newHeight < screenHeight)
                    {
                        ratio = (float)screenHeight / loadedImage.Height;

                        newWidth = (int)(ratio * loadedImage.Width);
                        newHeight = (int)(ratio * loadedImage.Height);
                    }
                }

                this.Width = newWidth;
                this.Height = newHeight;                
            }
            else
            {
                this.Width = loadedImage.Width + 50;
                this.Height = loadedImage.Height + 103;
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
            if(currentIndex + 1 < images.Count)
            {
                currentIndex++;                
            }
            else
            {
                currentIndex = 0;
            }

            loadImageToPictureBox(images[currentIndex]);
        }

        private void previousImageButton_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;                              
            }
            else
            {
                currentIndex = images.Count - 1;
            }

            loadImageToPictureBox(images[currentIndex]);
        }

        private void BCIV_form_Resize(object sender, EventArgs e)
        {
            imagePanel.Size = new Size(this.Width - 40, this.Height - 93);

            nextImageButton.Location = new Point(this.Width / 2 + 5, this.Height - 75);
            previousImageButton.Location = new Point(this.Width / 2 - 86, this.Height - 75);
        }
    }
}
