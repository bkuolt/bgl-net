
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;

using Pixel = SixLabors.ImageSharp.PixelFormats.Rgba32;
using Image = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;


using MessageBoxImage = System.Windows.MessageBoxImage;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace bgl {
    /// <summary>
    /// 
    /// </summary>
    class AnimationBuilder {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public Image BuildAnimation(Image[] images, string outputPath) {
            try
            {      
                Image resizedImage;
                BoundingBox boundingBox = CalculateBoundingBox(images);
                int width = boundingBox.GetWidth();
                int height = boundingBox.GetHeight();

                // create new multi-frame PNG image
                int size = width * height;
                resizedImage = new Image(width, height);
                
                // copy content for each image
                for (int i = 0; i < images.Length; ++i) {
                    var data = new Pixel[width * height];
                    var frame = resizedImage.Frames.AddFrame(data);

                    // copy from source image 
                    var rectangle = new SixLabors.ImageSharp.Rectangle();
                    images[i].Mutate( (image) => image.Crop(rectangle) );
                    images.CopyTo(data, 0);
                }

                SetMetadata(resizedImage);
                // TODO: log savings
                return resizedImage;
            } catch {
                throw new System.Exception("could not process images");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        public void BuildAnimation(Image[] images) {
                string? outputPath = ChooseFile("Where to save the animation?", ".png");
                if (outputPath == null) {
                    return;  // there is nothing to do
                }

                try {
                    using (var image = BuildAnimation(images, outputPath))
                    SaveFile(image, outputPath);
                } 
                catch(System.Exception exception) {
                    ShowError(exception.Message);
                }
        }

        /* ---------------------------------------------------------------------------------- */
        protected static string? ChooseFile(string dialogTile, string extension) {
            string[] extensions = new string[1] { $"() *.{extension}" };
            return ChooseFile(extension, dialogTile, extensions);
        }
  
        protected static string? ChooseFile(string dialogTile, string defaultExension, string[] extensions) {           
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = dialogTile;
            dialog.DefaultExt = defaultExension;
            dialog.Filter = System.String.Join(";", extensions);
            bool? didChoose = dialog.ShowDialog();
            return didChoose.HasValue && didChoose.Value  == true ? dialog.FileName : null;
        }

        void SaveFile(Image image, string path) {
            var numPixels = image.Size.Width * image.Size.Height;
            if (numPixels == 0) {
                ShowWarning("Image is empty.");
                return;
            }

            var fileInfo = new System.IO.FileInfo(path);
            if (fileInfo.Exists) {
                bool allowOverride = PromptOption("Warning", $"Do you really want to override '{fileInfo.Name}'?");
                if (!allowOverride) {
                    return;
                }
            }

            try {
                image.SaveAsPng(path);
            } catch {
                throw new System.Exception("Could not save image.");
            }
        }

        protected MessageBoxResult ShowMessage(MessageBoxImage icon, string caption, string text, MessageBoxButton button = MessageBoxButton.OK) {    
            return System.Windows.MessageBox.Show(text, caption, button, icon);
        }

        protected void ShowError(string text, string caption = "Error") {
            ShowMessage(MessageBoxImage.Error, caption, text);
        }
        protected void ShowWarning(string text, string caption = "Warning") {
            ShowMessage(MessageBoxImage.Warning, caption, text);
        }

        protected bool PromptOption(string caption, string text)
        {
            var result = ShowMessage(MessageBoxImage.Question, caption, text);
            return result == MessageBoxResult.Yes;
        }

        /* ---------------------------------------------------------------------------------- */

        BoundingBox CalculateBoundingBox(BoundingBox a, BoundingBox c) {
            var boundingBox = new BoundingBox();
            // TODO
            return boundingBox;
        }

        BoundingBox CalculateBoundingBox(BoundingBox[] boundingBoxes) 
        {
            var boundingBox = new BoundingBox();
            for (int i = 0; i < boundingBoxes.Length; ++i) {
                boundingBox = CalculateBoundingBox(boundingBox, boundingBoxes[i]);
            }
            return boundingBox;
        }

        BoundingBox[] CalculateBoundingBoxes(Image[] images) 
        {
            return new BoundingBox[0];  // TODO
        }

        public BoundingBox CalculateBoundingBox(Image[] images) 
        {
            BoundingBox[] boundingBoxes = CalculateBoundingBoxes(images);
            var boundingBox = CalculateBoundingBox(boundingBoxes);
            return boundingBox;
        }
        public BoundingBox GetBoundingBox(Image image) 
        {
            var boundingBox = new BoundingBox();

            for (int y = 0; y < image.Height; ++y) {
                for (int x = 0; x < image.Width; ++x) {
                    var pixel = image[y, x];

                    if (boundingBox.max.x > x) {
                        // TODO
                    }
                    else if ( boundingBox.min.x < x) {
                        // TODO;
                    }

                    if (boundingBox.max.y > y) {
                        // TODO
                    } else if (boundingBox.min.y < y) {
                        // TODO
                    }
                }
            }

            return boundingBox;
        }

        private void SetMetadata(Image image) {
            image.Metadata.IptcProfile = new SixLabors.ImageSharp.Metadata.Profiles.Iptc.IptcProfile();
            image.Metadata.IptcProfile.SetValue(IptcTag.Keywords, "FPS");
            image.Metadata.IptcProfile.SetValue(IptcTag.Keywords, "Interpolation");
        }
    }
}