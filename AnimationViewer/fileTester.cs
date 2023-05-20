using SixLabors;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp;
    using RGBA = SixLabors.ImageSharp.PixelFormats.Rgba32;

      
namespace bgl {

    class S {
    



        struct vec2 {
            uint x;
            uint y;
        }

        struct Bounds {
            vec2 min;
            vec2 max;
        }
            
        Bounds GetBounds(SixLabors.ImageSharp.Image<RGBA> image) 
        {
            Bounds bounds;

            for (int y = 0; y < image.Height; ++y) {
                for (int x = 0; x < image.Width; ++x) {
                    var pixel = image[y, x];
                    // TODO: check maximum
                    // TODO: check minimum
                }
            }

            return bounds;
        }


        Bounds GetBounds(SixLabors.ImageSharp.Image<RGBA>[] image) 
        {
            var bounds = new Bounds[images.Length];
            for (int i = 0; i < bounds.Length; ++i) {
                bounds[i] = GetBounds(images[i]);
            }

            Bounds imageBounds;
            return imageBounds;
        }


        void Algo(SixLabors.ImageSharp.Image<RGBA>[] images) {

            Bounds bounds = GetBounds(images);
            int width = 0;
            int height = 0;

            // create new images
            var resizedImage = new SixLabors.ImageSharp.Image<RGBA>(width, height);
            
            // copy content for each image
            for (int i = 0; i < images.Length; ++i) {
                var data = new RGBA[width * height];
                var frame = resizedImage.Frames.AddFrame(data);
                // TODO: copy from source image            
            }

        
            // create new multi frame PNG image
            resizedImage.SaveAsPng("TODO", null /* pngDeoder */)
            // save multi image PNG image
        }







    }
}