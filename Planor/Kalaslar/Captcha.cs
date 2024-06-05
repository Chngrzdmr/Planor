using Patagames.Ocr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planor.Kalaslar
{
    class Captcha
    {
        private readonly string _tesseractDllPath;

        public Captcha(string tesseractDllPath = @"C:\CMSigorta\tesseract.dll")
        {
            _tesseractDllPath = tesseractDllPath;
        }

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

        public static Bitmap CropImage(Bitmap b, Rectangle r)
        {
            using (var nb = new Bitmap(r.Width, r.Height))
            using (var g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
                return nb;
            }
        }

        public static Bitmap RenkDegistir(Image degiscekimg)
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
                            newBitmap.SetPixel(i, j, Color.White);
                        else
                            newBitmap.SetPixel(i, j, Color.Black);
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap Temizle(Image degiscekimg)
        {
            try
            {
                var newBitmap = new Bitmap(degiscekimg);
                var img1 = new Bitmap(degiscekimg);

                for (int i = 1; i < img1.Width - 1; i++)
                {
                    for (int j = 1; j < img1.Height - 1; j++)
                    {
                        var piksel = img1.GetPixel(i, j);
                        if (piksel.R == 0 && piksel.G == 0 && piksel.B == 0 && piksel.A == 255)
                            newBitmap.SetPixel(i, j, Color.White);
                        else
                            newBitmap.SetPixel(i, j, CalculateAverageColor(img1, i, j));
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        private static Color CalculateAverageColor(Bitmap img1, int i, int j)
        {
            int count = 0;
            int r = 0, g = 0, b = 0;

            for (int x = i - 1; x <= i + 1; x++)
            {
                for (int y = j - 1; y <= j + 1; y++)
                {
                    var piksel = img1.GetPixel(x, y);
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

        public String NeovaAritmetik(string yaziyadonusmuscaptcha)
        {
            try
            {
                int m1 = Int32.Parse(yaziyadonusmuscaptcha.Substring(0, 1));
                int m2 = Int32.Parse(yaziyadonusmuscaptcha.Substring(2, 1));

                return (m1 + m2).ToString();
            }
            catch
            {
                return "NEOVA_ARITMETIK_HATASI";
            }
        }

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

        public string NeovaOku(Image captchresim)
        {
            try
            {
                using (var ocrresim = OcrApi.Create())
                {
                    ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                    ocrresim.SetVariable("tessedit_char_whitelist", "01234567890+-=?");
                    return BosluklariSil(ocrresim.GetTextFromImage((Bitmap)captchresim));
                }
            }
            catch (Exception ex)
            {
                return $"NEOVA_OKUMA_HATASI: {ex.Message}";
            }
        }

        public string Ankara5eBol(Bitmap bolunecekimg)
        {
            try
            {
                var img1 = new Bitmap(bolunecekimg);
                var newBitmap = new Bitmap(bolunecekimg);
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

        private Bitmap ExtractPart(Bitmap img1)
        {
            int parcaBolunmeNoktasi = BolunmeNoktasiBul(img1);
            return CropImage(img1, new Rectangle(parcaBolunmeNoktasi, 0, 18, img1.Height));
        }

        private int BolunmeNoktasiBul(Bitmap img1)
        {
            int count = 0;
            int i = 0;

            while (count < 2)
            {
                var piksel = img1.GetPixel(i, 0);
                if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0)
                    count++;
                i++;
            }

            return i;
        }

        public static string BosluklariSil(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
