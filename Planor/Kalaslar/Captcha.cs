using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patagames.Ocr;
using System.IO;

namespace Planor.Kalaslar
{
    /// <summary>
    /// A class that provides methods for captcha recognition and manipulation.
    /// </summary>
    public class CaptchaService
    {
        private readonly string _tesseractDllPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaService"/> class with the default Tesseract DLL path.
        /// </summary>
        public CaptchaService() : this(@"C:\CMSigorta\tesseract.dll")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaService"/> class with the specified Tesseract DLL path.
        /// </summary>
        /// <param name="tesseractDllPath">The path of the Tesseract DLL file.</param>
        public CaptchaService(string tesseractDllPath)
        {
            _tesseractDllPath = tesseractDllPath;
        }

        #region Public Methods

        /// <summary>
        /// Recognizes the captcha image using the Turkish language and the whitelist of characters.
        /// </summary>
        /// <param name="captchresim">The captcha image to recognize.</param>
        /// <returns>The recognized text of the captcha image, or an error message if any.</returns>
        public string AnkaraOku(Bitmap captchresim)
        {
            try
            {
                using (var ocrresim = OcrApi.Create())
                {
                    ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                    ocrresim.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    return BosluklariSil(ocrresim.GetTextFromImage(captchresim));
                }
            }
            catch (Exception ex)
            {
                return $"ANKARA_OKUMA_HATASI: {ex.Message}";
            }
        }

        /// <summary>
        /// Recognizes the captcha image using the English language and the whitelist of characters.
        /// </summary>
        /// <param name="captchresim">The captcha image to recognize.</param>
        /// <returns>The recognized text of the captcha image, or an error message if any.</returns>
        public string NeovaOku(Bitmap captchresim)
        {
            try
            {
                using (var ocrresim = OcrApi.Create())
                {
                    ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                    ocrresim.SetVariable("tessedit_char_whitelist", "01234567890+-=?");
                    return BosluklariSil(ocrresim.GetTextFromImage(captchresim));
                }
            }
            catch (Exception ex)
            {
                return $"NEOVA_OKUMA_HATASI: {ex.Message}";
            }
        }

        /// <summary>
        /// Extracts the fifth part of the captcha image by cropping it.
        /// </summary>
        /// <param name="img1">The captcha image to extract the part from.</param>
        /// <returns>The extracted part of the captcha image, or null if any error occurred.</returns>
        public Bitmap ExtractPart(Bitmap img1)
        {
            try
            {
                int parcaBolunmeNoktasi = BolunmeNoktasiBul(img1);
                return CropImage(img1, new Rectangle(parcaBolunmeNoktasi, 0, 18, img1.Height));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Concatenates the recognized texts of the five parts of the captcha image.
        /// </summary>
        /// <param name="captchresim">The captcha image to recognize.</param>
        /// <returns>The concatenated text of the five parts of the captcha image, or an error message if any.</returns>
        public string Ankara5eBol(Bitmap captchresim)
        {
            try
            {
                var img1 = new Bitmap(captchresim);
                var newBitmap = new Bitmap(captchresim);
                var sonuc = new StringBuilder();

                for (int i = 0; i < 5; i++)
                {
                    var parca = ExtractPart(img1);
                    var parcaSonucu = AnkaraOku(parca);
                    sonuc.Append(parcaSonucu);
                }

                return sonuc.ToString();
            }
            catch
            {
                return "ANKARA_GENEL_OKUMA_HATASI";
            }
        }

        /// <summary>
        /// Converts the base64 string to an image.
        /// </summary>
        /// <param name="base64">The base64 string to convert.</param>
        /// <returns>The image of the base64 string, or null if any error occurred.</returns>
        public Image Base64tenResime(string base64)
        {
            try
            {
                var converter = new ImageConverter();
                return (Image)converter.ConvertFrom(Convert.FromBase64String(base64));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the image to a base64 string.
        /// </summary>
        /// <param name="file">The image to convert.</param>
        /// <returns>The base64 string of the image, or an error message if any.</returns>
        public string ResimdenBase64e(Image file)
        {
            try
            {
                var converter = new ImageConverter();
                var raw = (byte[])converter.ConvertTo(file, typeof(byte[]));
                return Convert.ToBase64String(raw);
            }
            catch
            {
                return "RESIMDENBASE64E_HATASI";
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the pixel color of the specified coordinates of the image.
        /// </summary>
        /// <param name="img1">The image to get the pixel color from.</param>
        /// <param name="x">The x-coordinate of the pixel.</param>
        /// <param name="y">The y-coordinate of the pixel.</param>
        /// <returns>The pixel color of the specified coordinates of the image.</returns>
        private Color GetPixelColor(Bitmap img1, int x, int y)
        {
            return img1.GetPixel(x, y);
        }

        /// <summary>
        /// Sets the pixel color of the specified coordinates of the image.
        /// </summary>
        /// <param name="img1">The image to set the pixel color to.</param>
        /// <param name="x">The x-coordinate of the pixel.</param>
        /// <param name="y">The y-coordinate of the pixel.</param>
        /// <param name="color">The pixel color to set.</param>
        private void SetPixelColor(Bitmap img1, int x, int y, Color color)
        {
            img1.SetPixel(x, y, color);
        }

        /// <summary>
        /// Calculates the average color of the neighboring pixels of the specified coordinates of the image.
        /// </summary>
        /// <param name="img1">The image to calculate the average color from.</param>
        /// <param name="x">The x-coordinate of the center pixel.</param>
        /// <param name="y">The y-coordinate of the center pixel.</param>
        /// <returns>The average color of the neighboring pixels of the specified coordinates of the image.</returns>
        private Color CalculateAverageColor(Bitmap img1, int x, int y)
        {
            int count = 0;
            int r = 0, g = 0, b = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    var piksel = GetPixelColor(img1, i, j);
                    if (piksel.A == 255)
                    {
                        r += piksel.R;
                        g += piksel.G;
                        b += piksel.B;
                        count++;
                    }
                }
            }

            return Color.FromArgb(255, r / count, g / count, b / count);
        }

        /// <summary>
        /// Crops the image by the specified rectangle.
        /// </summary>
        /// <param name="b">The image to crop.</param>
        /// <param name="r">The rectangle to crop the image by.</param>
        /// <returns>The cropped image.</returns>
        private Bitmap CropImage(Bitmap b, Rectangle r)
        {
            using (var nb = new Bitmap(r.Width, r.Height))
            using (var g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
                return nb;
            }
        }

        /// <summary>
        /// Changes the color of the pixels that match the specified condition.
        /// </summary>
        /// <param name="degiscekimg">The image to change the color of.</param>
        /// <returns>The image with the changed color.</returns>
        private Bitmap RenkDegistir(Image degiscekimg)
        {
            try
            {
                var newBitmap = new Bitmap(degiscekimg);
                var img1 = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        var piksel = img1.GetPixel(i, j);
                        if (piksel.R == 0 && piksel.G == 0 && piksel.B == 0 && piksel.A == 0)
                            SetPixelColor(newBitmap, i, j, Color.White);
                        else
                            SetPixelColor(newBitmap, i, j, Color.Black);
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        /// <summary>
        /// Cleans the image by changing the color of the pixels that match the specified condition.
        /// </summary>
        /// <param name="degiscekimg">The image to clean.</param>
        /// <returns>The cleaned image.</returns>
        private Bitmap Temizle(Image degiscekimg)
        {
            try
            {
                var newBitmap = new Bitmap(degiscekimg);
                var img1 = new Bitmap(degisce
