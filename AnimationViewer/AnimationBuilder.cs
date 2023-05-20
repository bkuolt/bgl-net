
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;

using RGBA = SixLabors.ImageSharp.PixelFormats.Rgba32;

namespace bgl {

    class AnimationBuilder {
        public struct vec2 {
            public vec2() {
                x = 0;
                y = 0;
            }
            public int x;
            public int y;
        }

        public struct BoundingBox {
            public BoundingBox() {
                min.x = System.Int32.MaxValue;
                min.y = System.Int32.MaxValue;
                max.x = System.Int32.MinValue;
                max.y = System.Int32.MinValue;
            }


            public vec2 Max(BoundingBox a, BoundingBox b)
            {
                vec2 max;
                max.x = System.Math.Max(a.max.x, b.max.x);
                max.y = System.Math.Max(a.max.y, b.max.y);
                return max;
            }

            public int GetWidth() {
                return max.x - min.x;
            }

            public int GetHeight() {
                return max.x - min.x;;
            }            

             public vec2 min;
             public vec2 max;
        }

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

        BoundingBox[] CalculateBoundingBoxes(SixLabors.ImageSharp.Image<RGBA>[] images) 
        {
            return new BoundingBox[0];
        }

        public BoundingBox CalculateBoundingBox(SixLabors.ImageSharp.Image<RGBA>[] images) 
        {
  
            BoundingBox[] boundingBoxes = CalculateBoundingBoxes(images);
            var boundingBox = CalculateBoundingBox(boundingBoxes);
            return boundingBox;
        }
        public BoundingBox GetBoundingBox(SixLabors.ImageSharp.Image<RGBA> image) 
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

        /// <summary>
        ///////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////
        
        // TODO: makeStatic
        public SixLabors.ImageSharp.Image<RGBA> BuildAnimation(SixLabors.ImageSharp.Image<RGBA>[] images, string outputPath) {
            BoundingBox boundingBox = CalculateBoundingBox(images);
            int width = boundingBox.GetWidth();
            int height = boundingBox.GetHeight();

            // create new multi-frame PNG image
            int size = width * height;
            var resizedImage = new SixLabors.ImageSharp.Image<RGBA>(width, height);
            
            // copy content for each image
            for (int i = 0; i < images.Length; ++i) {
                var data = new RGBA[width * height];
                var frame = resizedImage.Frames.AddFrame(data);
                // TODO: copy from source image    

                var rectangle = new SixLabors.ImageSharp.Rectangle();
                images[i].Mutate( (image) => image.Crop(rectangle) );
                images.CopyTo(data, 0);
            }

            resizedImage.SaveAsPng(outputPath, null /* TODO: Create custom PNGDecoder */);

            resizedImage.Metadata.IptcProfile = new SixLabors.ImageSharp.Metadata.Profiles.Iptc.IptcProfile();

            // TODO
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Name, "Pokemon");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Byline, "Thimo Pedersen");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Caption, "Classic Pokeball Toy on a bunch of Pokemon Cards. Zapdos, Ninetales and a Trainercard visible.");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Source, @"https://rb.gy/hgkqhy");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Keywords, "Pokemon");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Keywords, "Pokeball");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Keywords, "Cards");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Keywords, "Zapdos");
            resizedImage.Metadata.IptcProfile.SetValue(IptcTag.Keywords, "Ninetails");

            // TODO: log savings
            return resizedImage;
        }
    }
}