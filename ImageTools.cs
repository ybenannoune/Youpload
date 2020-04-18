using System;
using ShareX.HelpersLib;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ShareX
{
    class ImageTools
    {
        public class ImageSettings
        {           
            public EImageFormat ImageFormat = EImageFormat.PNG;
            public int ImageJPEGQuality = 80; 
            public bool ImageAutoUseJPEG = true;
            public int ImageAutoUseJPEGSize = 150; //kilo-bytes
        }

        public static ImageData PrepareImage(Image img, ImageSettings imageSettings)
        {
            ImageData imageData = new ImageData();
            imageData.ImageStream = SaveImage(img, imageSettings);
            imageData.ImageFormat = imageSettings.ImageFormat;

            if (imageSettings.ImageAutoUseJPEG && imageSettings.ImageFormat != EImageFormat.JPEG &&
                imageData.ImageStream.Length > imageSettings.ImageAutoUseJPEGSize * 1000)
            {
                imageData.ImageStream = SaveImage(img, imageSettings);
                imageData.ImageFormat = EImageFormat.JPEG;
            }

            return imageData;
        }

        public static MemoryStream SaveImage(Image img, ImageSettings imageSettings)
        {
            return SaveImage(img, imageSettings.ImageFormat, imageSettings.ImageJPEGQuality);
        }

        public static MemoryStream SaveImage(Image img, EImageFormat imageFormat, int jpegQuality = 90)
        {
            MemoryStream stream = new MemoryStream();

            switch (imageFormat)
            {
                case EImageFormat.PNG:
                    img.Save(stream, ImageFormat.Png);
                    break;
                case EImageFormat.JPEG:
                    img = ImageHelpers.FillBackground(img, Color.White);
                    img.SaveJPG(stream, jpegQuality);
                    break;                
            }
            return stream;
        }
    }
}
