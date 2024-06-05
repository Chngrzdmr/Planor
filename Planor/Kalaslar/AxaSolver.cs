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
        // Static readonly field for the directory containing character images
        private static readonly string CharactersDirectory = "karakterler\\";

        // Static field for the ListBox used in the AxaCaptchaCoz method
        private static ListBox ListBox1;

        /// <summary>
        /// Solves the AXA captcha by analyzing and recognizing characters in the given image
        /// </summary>
        /// <param name="EsasCaptcha">The image containing the AXA captcha</param>
        /// <returns>The solved captcha as a string</returns>
        public static string AxaCaptchaCoz(Image EsasCaptcha)
        {
            InitializeListBox(); // Initialize the ListBox1 before analyzing the captcha
            Bitmap pb1 = (Bitmap)EsasCaptcha;
            List<string> captchaParts = AnalyzeCaptcha(pb1);
            string finalKey = GetFinalKey(captchaParts);
            return finalKey;
        }

        /// <summary>
        /// Initializes the ListBox1 with the character image files in the CharactersDirectory
        /// </summary>
        private static void InitializeListBox()
        {
            ListBox1 = new ListBox();
            string[] files = Directory.GetFiles(CharactersDirectory);

            foreach (string file in files)
            {
                ListBox1.Items.Add(file);
            }
        }

        /// <summary>
        /// Analyzes the captcha image by extracting and recognizing characters
        /// </summary>
        /// <param name="pb1">The captcha image as a Bitmap</param>
        /// <returns>A list of recognized characters in the captcha</returns>
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

        /// <summary>
        /// Extracts and recognizes a single character from the captcha image
        /// </summary>
        /// <param name="pb1">The captcha image as a Bitmap</param>
        /// <param name="partIndex">The index of the character to extract</param>
        /// <returns>The recognized character as a string</returns>
        private static string ExtractCharacter(Bitmap pb1, int partIndex)
        {
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            // Set the coordinates and dimensions of the character to extract based on the partIndex
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

        /// <summary>
        /// Crops a rectangular region from the given image
        /// </summary>
        /// <param name="image">The image to crop</param>
        /// <param name="x">The x-coordinate of the top-left corner of the crop rectangle</param>
        /// <param name="y">The y-coordinate of the top-left corner of the crop rectangle</param>
        /// <param name="width">The width of the crop rectangle</param>
        /// <param name="height">The height of the crop rectangle</param>
        /// <returns>The cropped image as a Bitmap</returns>
        private static Bitmap CropImage(Bitmap image, int x, int y, int width, int height)
        {
            Rectangle cropArea = new Rectangle(x, y, width, height);
            return (Bitmap)image.Clone(cropArea, image.PixelFormat);
        }

        /// <summary>
        /// Recognizes the text in the given image using OCR
        /// </summary>
        /// <param name="image">The image containing the text to recognize</param>
        /// <returns>The recognized text as a string</returns>
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

        /// <summary>
        /// Converts the provided language string to the corresponding Enums.Languages enum value
        /// </summary>
        /// <param name="language">The language as a string</param>
        /// <returns>The Enums.Languages enum value</returns>
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

        /// <summary>
        /// Extracts and recognizes the bottom text in the captcha image
        /// </summary>
        /// <param name="pb1">The captcha image as a Bitmap</param>
        /// <returns>The recognized bottom text as a string</returns>
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

        /// <summary>
        /// Combines the recognized captcha parts to form the final key
        /// </summary>
        /// <param name="captchaParts">A list of recognized characters in the captcha</param>
        /// <returns>The final key as a string</returns>
        private static string GetFinalKey(List<string> captchaParts)
        {
            // Implement the logic to combine captcha parts and return the final key
        }
    }
}
