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
        private static readonly string CharactersDirectory = "karakterler\\";
        private static ListBox ListBox1;

        public static string AxaCaptchaCoz(Image EsasCaptcha)
        {
            InitializeListBox();
            Bitmap pb1 = (Bitmap)EsasCaptcha;
            List<string> captchaParts = AnalyzeCaptcha(pb1);
            string finalKey = GetFinalKey(captchaParts);
            return finalKey;
        }

        private static void InitializeListBox()
        {
            ListBox1 = new ListBox();
            string[] files = Directory.GetFiles(CharactersDirectory);

            foreach (string file in files)
            {
                ListBox1.Items.Add(file);
            }
        }

        private static List<string> AnalyzeCaptcha(Bitmap pb1)
        {
            List<string> captchaParts = new List<string>();

            for (int i = 0; i < 9; i++)
            {
                captchaParts.Add(ExtractCharacter(pb1, i));
            }

            captchaParts.Add(ExtractBottomText(pb1));
            return captchaParts;
        }

        private static string ExtractCharacter(Bitmap pb1, int partIndex)
        {
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            switch (partIndex)
            {
                case 0:
                    x = 3;
                    y = 3;
                    width = 73;
                    height = 73;
                    break;
                case 1:
                    x = 85;
                    y = 3;
                    width = 73;
                    height = 73;
                    break;
                // Add cases for other parts here
            }

            Bitmap croppedImage = CropImage(pb1, x, y, width, height);
            string character = RecognizeCharacter(croppedImage);
            return character;
        }

        private static Bitmap CropImage(Bitmap image, int x, int y, int width, int height)
        {
            Rectangle cropArea = new Rectangle(x, y, width, height);
            return (Bitmap)image.Clone(cropArea, image.PixelFormat);
        }

        private static string RecognizeCharacter(Bitmap image)
        {
            string character = string.Empty;
            string ocrLanguage = "English";

            using (var ocrEngine = OcrApi.Create())
            {
                ocrEngine.Init(GetEnumLanguage(ocrLanguage));
                character = ocrEngine.GetTextFromImage(image);
            }

            return character;
        }

        private static Enums.Languages GetEnumLanguage(string language)
        {
            switch (language)
            {
                case "English":
                    return Enums.Languages.English;
                case "Turkish":
                    return Enums.Languages.Turkish;
                default:
                    throw new ArgumentException("Invalid language provided");
            }
        }

        private static string ExtractBottomText(Bitmap pb1)
        {
            int x = 3;
            int y = 167;
            int width = 325;
            int height = 27;

            Bitmap croppedImage = CropImage(pb1, x, y, width, height);
            string bottomText = RecognizeCharacter(croppedImage);
            return bottomText;
        }

        private static string GetFinalKey(List<string> captchaParts)
        {
            // Implement the logic to combine captcha parts and return the final key
        }
    }
}
