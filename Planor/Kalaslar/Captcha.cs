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

        public string AnkaraOku(Bitmap captchresim)
        {
            string karakterx;

            //try
            //{
            //try
            //{
            //    OcrApi.PathToEngine = @"C:\CMSigorta\tesseract.dll";
            //}
            //catch
            //{
            //    karakterx = "DLL_BULMA_HATASI";
            //}

            using (var ocrresim = OcrApi.Create())
            {
                try
                {
                    ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                }
                catch
                {
                    karakterx = "TURKCE_PAKET_HATASI";
                }

                try
                {
                    ocrresim.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                }
                catch
                {
                    karakterx = "KARAKTER_LISTESI_OLUSTURMA_HATASI";
                }

                try
                {
                    karakterx = ocrresim.GetTextFromImage((Bitmap)captchresim);
                }
                catch
                {
                    karakterx = "3._SATIR_HATALI";
                }

                karakterx = BosluklariSil(karakterx);

                return karakterx;
            }
            //}
            //catch
            //{
            //    return "NEOVA_YAZIYA_DÖNÜŞTÜRME_HATASI";
            //}
        }

        public static Bitmap cropImage(Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
                return nb;
            }

        }

        public static Bitmap NeovaRenkDegistirx(Image degiscekimg)
        {
            try
            {
                Color piksel;
                //string img1_ref, RR, GG, BB;
                int count1 = 0, count2 = 0, RiR, GiG, BiB;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                //MessageBox.Show(img1.GetPixel(0, 16).ToString());
                //MessageBox.Show(img1.GetPixel(20, 5).ToString());

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);


                        /* 
                        if (piksel == Color.Transparent)  çalışmıyor transparet sorgusu
                        {
                            
                            Application.DoEvents();
                        }
   
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img1_ref = img1_ref.Replace("Color [A", "A");
                        img1_ref = img1_ref.Replace(" ", "");
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 2));
                        img1_ref = img1_ref.Replace("R=", "");
                        img1_ref = img1_ref.Replace("G=", "");
                        img1_ref = img1_ref.Replace("B=", "");
                        RR = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",")+1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        GG = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        BB = img1_ref;
                        RiR = Convert.ToInt32(RR);
                        GiG = Convert.ToInt32(GG);
                        BiB = Convert.ToInt32(BB);
                        */

                        //if (piksel.R == 0 || piksel.G == 0 || piksel.B == 0 || piksel.A == 0)
                        if (piksel.R == 0 && piksel.G == 0 && piksel.B == 0)
                        {
                            //if (piksel.R != 0 && piksel.G != 0 && piksel.B != 0) newBitmap.SetPixel(i, j, Color.White);
                            if (piksel.A != 255) newBitmap.SetPixel(i, j, Color.White);
                            //newBitmap.SetPixel(i, j, Color.Black);
                        }
                        else
                        {

                            newBitmap.SetPixel(i, j, Color.White);
                        }




                        /*

                        if (piksel.R < 200 || piksel.G < 200 || piksel.B < 200)
                        {
                            if (piksel.R != 0 && piksel.G != 0 && piksel.B != 0) newBitmap.SetPixel(i, j, Color.White);

                            //newBitmap.SetPixel(i, j, Color.Black);
                        }
                        else
                        {
                            newBitmap.SetPixel(i, j, Color.Black);
                            //if (piksel.G < 150 && piksel.B < 150)
                            //{
                            //    newBitmap.SetPixel(i, j, Color.White);
                            //}
                        }
                        if (piksel.A == 255 && piksel.R == 0) newBitmap.SetPixel(i, j, Color.White);

                        */





                        count1++;
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap NeovaTemizlex(Image degiscekimg)
        {
            try
            {
                Color piksel, pikselsol, pikselsag, pikselust, pikselalt;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 1; i < img1.Width - 1; i++)
                {
                    for (int j = 1; j < img1.Height - 1; j++)
                    {
                        piksel = img1.GetPixel(i, j);
                        pikselsol = img1.GetPixel(i - 1, j);
                        pikselsag = img1.GetPixel(i + 1, j);
                        pikselust = img1.GetPixel(i, j - 1);
                        pikselalt = img1.GetPixel(i, j + 1);

                        /*    
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img1_ref = img1_ref.Replace("Color [A", "A");
                        img1_ref = img1_ref.Replace(" ", "");
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 2));
                        img1_ref = img1_ref.Replace("R=", "");
                        img1_ref = img1_ref.Replace("G=", "");
                        img1_ref = img1_ref.Replace("B=", "");
                        RR = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",")+1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        GG = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        BB = img1_ref;
                        RiR = Convert.ToInt32(RR);
                        GiG = Convert.ToInt32(GG);
                        BiB = Convert.ToInt32(BB);
                        */

                        if (piksel.R == 0 && piksel.G == 0 && piksel.B == 0 && piksel.A == 255) //eğer siyahsa
                        //if (piksel.A == 255) //eğer siyahsa
                        {
                            //newBitmap.SetPixel(i, j, Color.White);
                        }
                        else // eğer siyah değilse
                        {
                            if (pikselsol.R == 0 && pikselsol.G == 0 && pikselsol.B == 0 && pikselsag.R == 0 && pikselsag.G == 0 && pikselsag.B == 0)
                            {
                                newBitmap.SetPixel(i, j, Color.Black);
                            }
                            else
                            {
                                if (pikselust.R == 0 && pikselust.G == 0 && pikselust.B == 0 && pikselalt.R == 0 && pikselalt.G == 0 && pikselalt.B == 0)
                                {
                                    newBitmap.SetPixel(i, j, Color.Black);
                                }
                                else newBitmap.SetPixel(i, j, Color.White);
                            }
                        }
                        if (piksel == null) newBitmap.SetPixel(i, j, Color.White);
                        count1++;
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public String NeovaAritmetik(string yaziyadonusmuscaptcha)
        {
            string sonuc = "NEOVA ARİTMETİK HATA";
            int m1 = 0, m2 = 0;

            try
            {
                m1 = Int32.Parse(yaziyadonusmuscaptcha.Substring(0, 1));
            }
            catch
            {
                sonuc = "1. KARAKTER HATASI";
            }

            try
            {
                m2 = Int32.Parse(yaziyadonusmuscaptcha.Substring(2, 1));
            }
            catch
            {
                sonuc = "2. KARAKTER HATASI";
            }

            if (m1 > 0 && m2 > 0)
            {
                sonuc = (m1 + m2).ToString();
            }

            return sonuc;
        }

        static Image FixedSizeTo500(Image imgPhoto)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            //int sourceX = 0;
            //int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)490 / (float)sourceWidth);
            nPercentH = ((float)490 / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((490 -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((490 -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(imgPhoto, new Size(destWidth, destHeight));

            return bmPhoto;
        }

        // sonuç Image olduğu için try catch ile cevap verilmedi
        public Image Base64tenResime(string base64)
        {
            Image resim;
            ImageConverter converter = new ImageConverter();

            resim = (Image)converter.ConvertFrom(Convert.FromBase64String(base64));

            return resim;
        }

        public string ResimdenBase64e(Image file)
        {
            ImageConverter converter = new ImageConverter();
            byte[] raw = new byte[1];
            Bitmap resim = new Bitmap(file);

            try
            {
                raw = (byte[])converter.ConvertTo(resim, typeof(byte[]));

                string output = Convert.ToBase64String(raw);
                return output;
            }
            catch
            {
                return "ResimdenBase64e fonksiyon HATA sı";
            }
        }

        public string NeovaOku(Image captchresim)
        {
            string karakterx = "HATA";

            ////try
            ////{
            //try
            //{
            //    OcrApi.PathToEngine = @"C:\CMSigorta\tesseract.dll";
            //}
            //catch
            //{
            //    karakterx = "DLL BULMA HATASI";
            //}

            try
            {
                using (var ocrresim = OcrApi.Create())
                {

                    try
                    {
                        ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                    }
                    catch
                    {
                        karakterx = "DİL PAKETİ HATASI";
                    }

                    try
                    {
                        ocrresim.SetVariable("tessedit_char_whitelist", "01234567890+-=?");
                    }
                    catch
                    {
                        karakterx = "KARAKTER LISTESI OLUSTURMA HATASI";
                    }

                    try
                    {
                        karakterx = ocrresim.GetTextFromImage((Bitmap)captchresim);
                    }
                    catch
                    {
                        karakterx = "3. SATIR HATALI";
                    }

                    karakterx = BosluklariSil(karakterx);

                    return karakterx;
                }
            }
            catch (Exception ex)
            {
                

                //throw;
            }
            return karakterx;
            //}
            //catch
            //{
            //    return "NEOVA YAZIYA DÖNÜŞTÜRME HATASI";
            //}
        }

        public static string BosluklariSil(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public string Ankara5eBol(Bitmap bolunecekimg)
        {
            Color piksel;
            string sonuc = "";
            int count1 = 0, ii = 20, parca1bn, parca2bn, parca3bn, parca4bn, parca5bn, aranokta1, aranokta2, aranokta3, aranokta4;
            Bitmap img1 = new Bitmap(bolunecekimg);
            Bitmap newBitmap = new Bitmap(bolunecekimg);
            Bitmap parca1, parca2, parca3, parca4, parca5;

            try
            {

                // parca1 i bulmak için  döngü
                for (int i = ii; i < img1.Width - 20; i++)
                {
                    count1 = 0;

                    //parca1bn yi bulmak için döngü
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 > 3)
                    {
                        parca1bn = i;
                        parca1 = cropImage(img1, new Rectangle(i, 0, 18, img1.Height - 1));
                        string ParcaSonucu = AnkaraOku(parca1);
                        if (ParcaSonucu.Length < 2) ParcaSonucu = ParcaSonucu;
                        else ParcaSonucu = ParcaSonucu.Substring(0, 1);
                        sonuc = sonuc + ParcaSonucu;
                        ii = i + 12;
                        i = img1.Width;
                    }
                }


                //-----------------------------------------------------------------------------------------

                // aranokta1 i bulmak için döngü
                for (int i = ii; i < img1.Width - 20; i++)
                {
                    count1 = 0;

                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 < 2)
                    {
                        aranokta1 = i;
                        ii = i;
                        i = img1.Width;
                    }
                }

                //-----------------------------------------------------------------------------------------

                for (int i = ii; i < img1.Width - 5; i++)
                {
                    count1 = 0;

                    //parca2bn yi bulmak için döngü
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 > 2)
                    {
                        parca2bn = i;
                        parca2 = cropImage(img1, new Rectangle(i, 0, 18, img1.Height - 1));
                        string ParcaSonucu = AnkaraOku(parca2);
                        if (ParcaSonucu.Length < 2) ParcaSonucu = ParcaSonucu;
                        else ParcaSonucu = ParcaSonucu.Substring(0, 1);
                        sonuc = sonuc + ParcaSonucu;
                        ii = i + 12;
                        i = img1.Width;
                    }
                }

                //-----------------------------------------------------------------------------------------

                // aranokta2 i bulmak için döngü
                for (int i = ii; i < img1.Width - 20; i++)
                {
                    count1 = 0;

                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 < 2)
                    {
                        aranokta2 = i;
                        ii = i;
                        i = img1.Width;
                    }
                }

                //-----------------------------------------------------------------------------------------

                //parca3 ü bulmak için döngü
                for (int i = ii; i < img1.Width - 5; i++)
                {
                    count1 = 0;

                    //parca3bn yi bulmak için döngü
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 > 2)
                    {
                        parca3bn = i;
                        parca3 = cropImage(img1, new Rectangle(i, 0, 18, img1.Height));
                        string ParcaSonucu = AnkaraOku(parca3);
                        if (ParcaSonucu.Length < 2) ParcaSonucu = ParcaSonucu;
                        else ParcaSonucu = ParcaSonucu.Substring(0, 1);
                        sonuc = sonuc + ParcaSonucu;
                        ii = i + 12;
                        i = img1.Width;
                    }
                }

                //-----------------------------------------------------------------------------------------

                // aranokta3 i bulmak için döngü
                for (int i = ii; i < img1.Width - 20; i++)
                {
                    count1 = 0;

                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 < 2)
                    {
                        aranokta3 = i;
                        ii = i;
                        i = img1.Width;
                    }
                }

                //-----------------------------------------------------------------------------------------

                //parca4 ü bulmak için döngü
                for (int i = ii; i < img1.Width - 5; i++)
                {
                    count1 = 0;

                    //parca4bn yi bulmak için döngü
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 > 2)
                    {
                        parca4bn = i;
                        parca4 = cropImage(img1, new Rectangle(i, 0, 18, img1.Height));
                        string ParcaSonucu = AnkaraOku(parca4);
                        if (ParcaSonucu.Length < 2) ParcaSonucu = ParcaSonucu;
                        else ParcaSonucu = ParcaSonucu.Substring(0, 1);
                        sonuc = sonuc + ParcaSonucu;
                        ii = i + 12;
                        i = img1.Width;
                    }
                }

                //-----------------------------------------------------------------------------------------

                // aranokta4 i bulmak için döngü
                for (int i = ii; i < img1.Width - 20; i++)
                {
                    count1 = 0;

                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 < 2)
                    {
                        aranokta4 = i;
                        ii = i;
                        i = img1.Width;
                    }
                }

                //-----------------------------------------------------------------------------------------

                //parca5 ü bulmak için döngü
                for (int i = ii; i < img1.Width - 5; i++)
                {
                    count1 = 0;

                    //parca5bn yi bulmak için döngü
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.A == 255 && piksel.R == 0 && piksel.G == 0 && piksel.B == 0) //eğer siyah ise
                        {
                            count1++;
                        }
                    }

                    if (count1 > 2)
                    {
                        parca5bn = i;
                        parca5 = cropImage(img1, new Rectangle(i, 0, 18, img1.Height));
                        string ParcaSonucu = AnkaraOku(parca5);
                        if (ParcaSonucu.Length < 2) ParcaSonucu = ParcaSonucu;
                        else ParcaSonucu = ParcaSonucu.Substring(0, 1);
                        sonuc = sonuc + ParcaSonucu;
                        ii = i + 12;
                        i = img1.Width;
                    }
                }


                return sonuc;
            }
            catch
            {
                return "ANKARA GENEL OKUMA HATASI";
                //return newBitmap;
            }
        }

    }
}
