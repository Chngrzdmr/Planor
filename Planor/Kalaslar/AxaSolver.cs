using Patagames.Ocr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Planor.Kalaslar
{
    class AxaSolver
    {


        public static Bitmap araimg1, araimg2;
        public static Bitmap pb1, pb2, pb3, pb4, pb5, pb6, pb7, pb8, pb9, pb10, pb11, pb12, pb13, pb14, pb15, pb16, pb17, pb18, pb19;
        public static string tb1, tb2, tb3, tb4, tb5, tb6, tb7, tb8, tb9, tb10, tb11, tb12, tb13, tb14, tb15, tb16, tb17, tb18;
        public static string karakteradi;
        public static List<string> karakterList = new List<string>();
        public static List<string> keyList = new List<string>();
        public static string esasanahtar;

        public static ListBox ListBox1 = new ListBox();


        public static String[] files = Directory.GetFiles("karakterler\\");


        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }


        public static string BulBakalim(Image buluncakimg)
        {
            int dngsayisi = 0;
            araimg1 = new Bitmap(buluncakimg);
            while (dngsayisi < 53)
            {
                araimg2 = new Bitmap(ListBox1.Items[dngsayisi].ToString());
                //pb20 = araimg2;
                //pb20.Refresh();
                if (Karsilastir(araimg1, araimg2))
                {
                    karakteradi = ListBox1.Items[dngsayisi].ToString();
                    break;
                }
                else
                {
                    //HATA yazmak için  bu şekilde kullanıldı.
                    karakteradi = "c:\\HATA.png";
                }

                dngsayisi++;
            }
            karakteradi = Path.GetFileNameWithoutExtension(karakteradi);
            //MessageBox.Show(dngsayisi.ToString() + " kez döndü");
            return karakteradi;
        }


        public static bool Karsilastir(Image anahtarimg, Image hedefimg)
        {
            try
            {
                string img1_ref, img2_ref;
                int count1 = 0, count2 = 0;
                bool flag = true;
                Bitmap img1 = new Bitmap(anahtarimg);
                Bitmap img2 = new Bitmap(hedefimg);

                if (img1.Width == img2.Width && img1.Height == img2.Height)
                {
                    for (int i = 20; i < img1.Width; i++)
                    {
                        for (int j = 20; j < img1.Height; j++)
                        {
                            img1_ref = img1.GetPixel(i, j).ToString();
                            img2_ref = img2.GetPixel(i, j).ToString();
                            if (img1_ref != img2_ref)
                            {
                                count2++;
                                flag = false;
                                if (count2 > 100)
                                {
                                    break;
                                }

                            }
                            count1++;
                        }
                    }

                    if (flag == false)
                    {
                        if (count2 > 100)
                        {
                            //MessageBox.Show("Sorry, Images are not same , " + count2 + " wrong pixels found");
                            return false;
                        }
                        else
                        {
                            //MessageBox.Show("Resimler çok yakın, " + count1 + " same pixels found  and " + count2 + " wrong pixels found");
                            return true;
                        }
                    }
                    else
                    {
                        //MessageBox.Show(" Images are same , " + count1 + " same pixels found  and " + count2 + " wrong pixels found");
                        return true;
                    }

                }
                else
                {
                    MessageBox.Show("Bu Resim Karşılaştırmaya Uygun Değil!!!");
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }




        public static string AxaCaptchaCoz(Image EsasCaptcha)
        {

            for (int i = 0; i < files.Length; i++)
            {
                ListBox1.Items.Add(files[i]);
            }

            pb1 = (Bitmap)EsasCaptcha;
            Bitmap img1;
            esasanahtar = "";
            karakterList.Clear();
            keyList.Clear();
            pb2 = (Bitmap)cropImage((Bitmap)EsasCaptcha, new Rectangle(3, 3, 73, 73));
            tb2 = BulBakalim(pb2);
            karakterList.Add(tb2);
            pb12 = (Bitmap)cropImage(pb2, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb12);
                //karakter = karakter.Replace("l", "").Replace("|", "");
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb12);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb12);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb12);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb1 = karakter;
                keyList.Add(tb1);
            }
            pb3 = (Bitmap)cropImage(pb1, new Rectangle(85, 3, 73, 73));
            tb3 = BulBakalim(pb3);
            karakterList.Add(tb3);
            pb13 = (Bitmap)cropImage(pb3, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb13);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb13);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb13);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb13);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb4 = karakter;
                keyList.Add(tb4);
            }
            pb4 = (Bitmap)cropImage(pb1, new Rectangle(167, 3, 73, 73));
            tb5 = BulBakalim(pb4);
            karakterList.Add(tb5);
            pb14 = (Bitmap)cropImage(pb4, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb14);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb14);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb14);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb14);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb6 = karakter;
                keyList.Add(tb6);
            }
            pb5 = (Bitmap)cropImage(pb1, new Rectangle(249, 3, 73, 73));
            tb7 = BulBakalim(pb5);
            karakterList.Add(tb7);
            pb15 = (Bitmap)cropImage(pb5, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb15);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb15);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb15);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb15);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb8 = karakter;
                keyList.Add(tb8);
            }
            pb6 = (Bitmap)cropImage(pb1, new Rectangle(3, 85, 73, 73));
            tb10 = BulBakalim(pb6);
            karakterList.Add(tb10);
            pb16 = (Bitmap)cropImage(pb6, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb16);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb16);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb16);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb16);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb9 = karakter;
                keyList.Add(tb9);
            }
            pb7 = (Bitmap)cropImage(pb1, new Rectangle(85, 85, 73, 73));
            tb12 = BulBakalim(pb7);
            karakterList.Add(tb12);
            pb17 = (Bitmap)cropImage(pb7, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb17);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb17);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb17);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb17);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb11 = karakter;
                keyList.Add(tb11);
            }
            pb8 = (Bitmap)cropImage(pb1, new Rectangle(167, 85, 73, 73));
            tb14 = BulBakalim(pb8);
            karakterList.Add(tb14);
            pb18 = (Bitmap)cropImage(pb8, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb18);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb18);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb18);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb18);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb13 = karakter;
                keyList.Add(tb13);
            }
            pb9 = (Bitmap)cropImage(pb1, new Rectangle(249, 85, 73, 73));
            tb16 = BulBakalim(pb9);
            karakterList.Add(tb16);
            pb19 = (Bitmap)cropImage(pb9, new Rectangle(3, 3, 13, 11));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb19);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb19);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb19);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb19);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb15 = karakter;
                keyList.Add(tb15);
            }
            pb10 = (Bitmap)cropImage(pb1, new Rectangle(3, 167, 325, 27));
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb10);
                //MessageBox.Show(karakter);
                karakter = karakter.Replace("\n", "").Replace(" ", "");
                karakter = karakter.Replace("Kiıaı", "Kiraz,");
                karakter = karakter.Replace("Kanal", "Kartal,");
                karakter = karakter.Replace("KanaI", "Kartal,");
                karakter = karakter.Replace("KartaL", "Kartal,");
                karakter = karakter.Replace("An,", "Arı,");
                karakter = karakter.Replace("Aıaba,", "Araba,");
                karakter = karakter.Replace("Kolluk", "Koltuk,");
                karakter = karakter.Replace("Kolmk", "Koltuk,");
                karakter = karakter.Replace("Kohık", "Koltuk,");
                karakter = karakter.Replace("Tüık", "Türk");
                karakter = karakter.Replace("SaaL", "Saat,");
                karakter = karakter.Replace("Fııça", "Fırça,");
                karakter = karakter.Replace("Ehıa", "Elma,");
                karakter = karakter.Replace("Dohp", "Dolap,");
                karakter = karakter.Replace("Güveıcin", "Güvercin,");
                karakter = karakter.Replace("HeIİmp|er", "Helikopter,");
                karakter = karakter.Replace("He|ikcpter", "Helikopter,");
                karakter = karakter.Replace("İnsanhr", "İnsanlar");
                karakter = karakter.Replace("Zebıa", "Zebra,");
                karakter = karakter.Replace("Zebıı", "Zebra,");
                karakter = karakter.Replace("Karpuı", "Karpuz,");
                karakter = karakter.Replace("Tıen", "Tren,");
                karakter = karakter.Replace("Bakhva", "Baklava");
                karakter = karakter.Replace("Paıa", "Para,");
                karakter = karakter.Replace("Pala", "Para,");
                karakter = karakter.Replace("Kanno-", ",Karınca,");
                karakter = karakter.Replace("Kannoı", ",Karınca,");
                karakter = karakter.Replace("Çioek", "Çiçek,");
                karakter = karakter.Replace("Çbek", "Çiçek,");
                karakter = karakter.Replace("Elmıek", "Ekmek,");
                karakter = karakter.Replace("Sanda|ye", "Sandalye,");
                karakter = karakter.Replace("Kelebek", "Kelebek,");
                karakter = karakter.Replace("Mçak", ",Uçak,");
                karakter = karakter.Replace("Kuhklık", "Kulaklık,");
                karakter = karakter.Replace("Kuthık", "Kulaklık,");
                karakter = karakter.Replace("KuhHık", "Kulaklık,");
                karakter = karakter.Replace("Kiıaz", "Kiraz,");
                karakter = karakter.Replace("BiskleL", "Bisiklet,");
                karakter = karakter.Replace("Ayakkabı", "Ayakkabı,");
                karakter = karakter.Replace("Masa", "Masa,");
                karakter = karakter.Replace("Balık", "Balık,");
                karakter = karakter.Replace("Meyve", "Meyve,");
                karakter = karakter.Replace("Uçak", "Uçak,");
                karakter = karakter.Replace("Şemsiye", "Şemsiye,");
                karakter = karakter.Replace("çilek", "Çilek,");
                karakter = karakter.Replace("Çilek", "Çilek,");
                karakter = karakter.Replace("Ağaç", "Ağaç,");
                karakter = karakter.Replace("Hellmpîer", "Helikopter,");
                karakter = karakter.Replace("Yel<enli", "Yelkenli,");
                karakter = karakter.Replace("Yel(enli", "Yelkenli,");
                karakter = karakter.Replace("Makas", "Makas,");
                karakter = karakter.Replace("Ağaç", "Ağaç,");
                karakter = karakter.Replace("Tüık", "Türk");
                karakter = karakter.Replace("Kuhklık", "Kulaklık,");
                karakter = karakter.Replace("SaaL", "Saat,");
                karakter = karakter.Replace("jnsanhr", "İnsanlar,");
                karakter = karakter.Replace("Kahvesi", "Kahvesi,");
                karakter = karakter.Replace("Kann-ı", "Karınca,");
                karakter = karakter.Replace("Bisİdet", "Bisiklet,");
                karakter = karakter.Replace("BisİdeL", "Bisiklet,");
                karakter = karakter.Replace("KalemleL", "Kalemler,");
                karakter = karakter.Replace("Pandı", "Panda,");
                karakter = karakter.Replace(",,", ",");
                karakter = karakter.Replace(",,", ",");
                //MessageBox.Show(karakter);
                if (karakter.Substring(0, 1) == ",") { karakter = karakter.Substring(1, karakter.Length - 1); };
                if (karakter.Substring(karakter.Length - 1, 1) == ",") { karakter = karakter.Substring(0, karakter.Length - 1); };
                tb17 = karakter;
            }

            List<string> anahtarLists = tb17.Split(',').ToList();


            foreach (string anahtarList in anahtarLists)
            {
                esasanahtar += keyList[karakterList.IndexOf(anahtarList)];
            }

            return esasanahtar;
        }


    }
}
