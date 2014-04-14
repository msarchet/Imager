namespace Imager
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    public class Generator
    {
        private static ConcurrentDictionary<string, byte[]> ImageCache = new ConcurrentDictionary<string, byte[]>();
        private static readonly Dictionary<string, ImageFormat> MimeToFormat;
        private static readonly Dictionary<ImageFormat, string> FormatToMime;

        static Generator()
        {
            MimeToFormat = new Dictionary<string, ImageFormat>(StringComparer.InvariantCultureIgnoreCase);
            FormatToMime = new Dictionary<ImageFormat, string>();

            MimeToFormat.Add(GetMimeType(ImageFormat.Bmp), ImageFormat.Bmp);
            MimeToFormat.Add(GetMimeType(ImageFormat.Emf), ImageFormat.Emf);
            MimeToFormat.Add(GetMimeType(ImageFormat.Exif), ImageFormat.Exif);
            MimeToFormat.Add(GetMimeType(ImageFormat.Gif), ImageFormat.Gif);
            MimeToFormat.Add(GetMimeType(ImageFormat.Icon), ImageFormat.Icon);
            MimeToFormat.Add(GetMimeType(ImageFormat.Jpeg), ImageFormat.Jpeg);
            MimeToFormat.Add(GetMimeType(ImageFormat.MemoryBmp), ImageFormat.MemoryBmp);
            MimeToFormat.Add(GetMimeType(ImageFormat.Png), ImageFormat.Png);
            MimeToFormat.Add(GetMimeType(ImageFormat.Tiff), ImageFormat.Tiff);
            MimeToFormat.Add(GetMimeType(ImageFormat.Wmf), ImageFormat.Wmf);

            FormatToMime.Add(ImageFormat.Bmp, GetMimeType(ImageFormat.Bmp));
            FormatToMime.Add(ImageFormat.Emf, GetMimeType(ImageFormat.Emf));
            FormatToMime.Add(ImageFormat.Exif, GetMimeType(ImageFormat.Exif));
            FormatToMime.Add(ImageFormat.Gif, GetMimeType(ImageFormat.Gif));
            FormatToMime.Add(ImageFormat.Icon, GetMimeType(ImageFormat.Icon));
            FormatToMime.Add(ImageFormat.Jpeg, GetMimeType(ImageFormat.Jpeg));
            FormatToMime.Add(ImageFormat.MemoryBmp, GetMimeType(ImageFormat.MemoryBmp));
            FormatToMime.Add(ImageFormat.Png, GetMimeType(ImageFormat.Png));
            FormatToMime.Add(ImageFormat.Tiff, GetMimeType(ImageFormat.Tiff));
            FormatToMime.Add(ImageFormat.Wmf, GetMimeType(ImageFormat.Wmf));
        }

        /// <summary>
        /// Create an Image
        /// </summary>
        /// <param name="MimeType">Mime Type</param>
        /// <param name="width">Width of the image in pixels</param>
        /// <param name="height">Height of the pixels</param>
        /// <returns>An Image with the specificed requirements</returns>
        public async static Task<Image> GenerateAsync(string mimetype, int width, int height)
        {
            var imageFormat = MimeToFormat[mimetype];
            return await GenerateAsync(imageFormat, width, height);
        }

        /// <summary>
        /// Create an Image
        /// </summary>
        /// <param name="format">A</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public async static Task<Image> GenerateAsync(ImageFormat format, int width, int height)
        {
            var bytes = await GenerateAsBytesAsync(format, width, height);
            return BytesToImage(bytes);
        }

        public async static Task<byte[]> GenerateAsBytesAsync(string mimetype, int width, int height)
        {
            var imageFormat = MimeToFormat[mimetype];
            return await GenerateAsBytesAsync(imageFormat, width, height);

        }

        public async static Task<byte[]> GenerateAsBytesAsync(ImageFormat format, int width, int height)
        {
            byte[] existingImage;
            var key = ImageId(format, width, height);
            using (var stream = new MemoryStream())
            {
                if (ImageCache.TryGetValue(key, out existingImage))
                {
                    return existingImage;
                }

                using (var bitmap = new Bitmap(width, height))
                {
                    bitmap.Save(stream, format);
                    existingImage = stream.ToArray();
                    ImageCache.AddOrUpdate(key, existingImage, (k, v) => existingImage);
                }

                return existingImage;
            }
        }

        public static Image BytesToImage(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private static string ImageId(ImageFormat format, int width, int height)
        {
            return string.Format("{0}{1}{2}", FormatToMime[format], width, height);
        }

        private static string GetMimeType(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Bmp))
            {
                return "image/bmp";
            }
            if (format.Equals(ImageFormat.Emf))
            {
                return "image/emf";
            }
            if (format.Equals(ImageFormat.Exif))
            {
                return "image/exif";
            }
            if (format.Equals(ImageFormat.Gif))
            {
                return "image/gif";
            }
            if (format.Equals(ImageFormat.Icon))
            {
                return "image/icon";
            }
            if (format.Equals(ImageFormat.Jpeg))
            {
                return "image/jpeg";
            }
            if (format.Equals(ImageFormat.MemoryBmp))
            {
                return "image/memorybmp";
            }
            if (format.Equals(ImageFormat.Png))
            {
                return "image/png";
            }
            if (format.Equals(ImageFormat.Tiff))
            {
                return "image/tiff";
            }
            if (format.Equals(ImageFormat.Wmf))
            {
                return "image/Wmf";
            }

            throw new Exception("unsupported image type");
        }
    }
}