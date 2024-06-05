// HizliAraclar.cs
// This is a C# implementation of a user control named HizliAraclar (Quick Tools) in a Windows Forms application.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Planor.Kalaslar; // Namespace for custom classes
using Planor.Sayfalar; // Namespace for this class
using System.Globalization;
using System.Linq;

namespace Planor.Sayfalar
{
    // HizliAraclar class representing the Quick Tools user control
    public partial class HizliAraclar : UserControl
    {
        // Instance variables
        SistemForm sistm;
        General gn;

        // Constructor
        public HizliAraclar()
        {
            InitializeComponent();
            sistm = new SistemForm();
            gn = new General();
        }

        // Handler for the Load event of the form
        private void HizliAraclar_Load(object sender, EventArgs e)
        {
            // Adjust the width of the form based on the screen size
            this.Width = sistm.screens[sistm.ekranno].WorkingArea.Width - (sistm.screens[sistm.ekranno].WorkingArea.Width / 8);
            this.Height = sistm.PanelSlider.Height;
        }

        // Handler for the Click event of button1
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Show a balloon tip with a message
                sistm.Trayyy1.ShowBalloonTip(1000, "Bekleyiniz...", "Yeniden deneme için 15 saniye daha bekleyiniz.", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                // Display an error message if there's an exception while showing the balloon tip
                MessageBox.Show($"An error occurred while showing the balloon tip: {ex.Message}");
            }
        }

        // Handler for the Click event of PanodanImageBTN
        private void PanodanImageBTN_Click(object sender, EventArgs e)
        {
            // Check if there's an image in the clipboard
            if (Clipboard.ContainsImage())
            {
                try
                {
                    // Get the image from the clipboard, save it to a temporary file, and display it in the PictureBox
                    var image = Clipboard.GetImage();
                    var tempFilePath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToFileTime()}_satis-sozlesmesi_img.png");
                    image.Save(tempFilePath, System.Drawing.Imaging.ImageFormat.Png);
                    PanoImageBox.Image = image;
                    txt_kayityolu.Text = tempFilePath;
                }
                catch (Exception ex)
                {
                    // Display an error message if there's an exception while saving the image
                    MessageBox.Show("An error occurred while saving the image: " + ex.Message);
                }
            }
            else
            {
                // Display a message if there's no image in the clipboard
                MessageBox.Show("Lütfen ilk önce \"Satış Sözleşmesi\" 'ni Sağtuş ile Kopyala işlemi yapınız...");
            }
        }

        // Handler for the Click event of SatistanIptaliBaslatBTN
        private void SatistanIptaliBaslatBTN_Click(object sender, EventArgs e)
        {
            // Check if required fields are filled
            if (string.IsNullOrEmpty(txt_kayityolu.Text) ||
                cmb_sirket.SelectedIndex == -1 ||
                string.IsNullOrEmpty(txt_policeno.Text) ||
                string.IsNullOrEmpty(txt_SatisTarihi.Text) ||
                string.IsNullOrEmpty(txt_noter.Text) ||
                string.IsNullOrEmpty(txt_YevmiyeNo.Text))
            {
                // Display a message if required fields are not filled
                MessageBox.Show("Lütfen önce bilgileri eksiksiz doldurunuz...");
                return;
            }

            // Concatenate the input values into a single string
            var uzuncumle = $"{txt_kayityolu.Text};" +
                            $"{cmb_sirket.SelectedItem};" +
                            $"{txt_policeno.Text};" +
                            $"{txt_SatisTarihi.Text};" +
                            $"{txt_noter.Text};" +
                            $"{txt_YevmiyeNo.Text};";

            // Encrypt the concatenated string
            var sifrelicumle = gn._Encrypt(uzuncumle, true);

            try
            {
                // Start the SatistaNiptaLRobotu.exe process with the encrypted string as an argument
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = @"C:\Users\Cihangir ÖZDEMİR\Source\Repos\SatistaNiptaLRobotu\SatistaNiptaLRobotu\bin\Debug\net5.0-windows",
                        FileName = "SatistaNiptaLRobotu.exe",
                        Arguments = sifrelicumle
                    }
                };
                process.Start();
            }
            catch (Exception ex)
            {
                // Display an error message if there's an exception while starting the process
                MessageBox.Show("An error occurred while starting the process: " + ex.Message);
            }
        }

        // Handler for the Click event of button1 (duplicate)
        private void button1_Click_1(object sender, EventArgs e)
        {
            // Add a new item to the cmb_sirket ComboBox
            cmb_sirket.Items.Add(new ComboBoxItem<string>("sirket_adi", default(string)));
        }

        // ComboBoxItem class definition
        public class ComboBoxItem<T>
        {
            // Properties
            public string Name { get; set; }
            public T Value { get; set; }

            // Constructor
            public ComboBoxItem(string name, T value)
            {
                Name = name;
                Value = value;
            }

            // Override ToString method to display the Name property
            public override string ToString()
            {
                return Name;
            }
        }

        // Handler for the TextChanged event of txtesas4
        private void txtesas4_TextChanged(object sender, EventArgs e)
        {
            // Check if the input is a valid number and less than 7 characters long
            if (!string.IsNullOrEmpty(txtesas4.Text) && txtesas4.Text.Length < 7)
            {
                if (double.TryParse(txtesas4.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double nValue))
                {
                    // Set TextBox values based on calculations
                    SetTextBoxValue(txts4, nValue * 3.00);
                    SetTextBoxValue(txts1, nValue * 2.35);
                    SetTextBoxValue(txts2, nValue * 1.90);
                    SetTextBoxValue(txts3, nValue * 1.45);
                    SetTextBoxValue(txts5, nValue * 0.90);
                    SetTextBoxValue(txts6, nValue * 0.78);
                    SetTextBoxValue(txts7, nValue * 0.58);
                    SetTextBoxValue(txts8, nValue * 0.50);

                    var value1a4 = nValue * 1.0475;
                    SetTextBoxValue(txt1a4, value1a4);
                    SetTextBoxValue(txt1a0, Convert.ToInt32(value1a4 * 3.00));
                    SetTextBoxValue(txt1a1, Convert.ToInt32(value1a4 * 2.35));
                    SetTextBoxValue(txt1a2, Convert.ToInt32(value1a4 * 1.90));
                    SetTextBoxValue(txt1a3, Convert.ToInt32(value1a4 * 1.45));
                    SetTextBoxValue(txt1a5, Convert.ToInt32(value1a4 * 0.90));
                    SetTextBoxValue(txt1a6, Convert.ToInt32(value1a4 * 0.78));
                    SetTextBoxValue(txt1a7, Convert.ToInt32(value1a4 * 0.58));
                    SetTextBoxValue(txt1a8, Convert.ToInt32(value1a4 * 0.50));

                    var valuey4 = nValue * 1.696;
                    SetTextBoxValue(txty4, valuey4);
                    SetTextBoxValue(txty0, Convert.ToInt32(valuey4 * 3.00));
                    SetTextBoxValue(txty1, Convert.ToInt32(valuey4 * 2.35));
                    SetTextBoxValue(txty2, Convert.ToInt32(valuey4 * 1.90));
                    SetTextBoxValue(txty3, Convert.ToInt32(valuey4 * 1.45));
                    SetTextBoxValue(txty5, Convert.ToInt32(valuey4 * 0.90));
                    SetTextBoxValue(txty6, Convert.ToInt32(valuey4 * 0.78));
                    SetTextBoxValue(txty7, Convert.ToInt32(valuey4 * 0.58));
                    SetTextBoxValue(txty8, Convert.ToInt32(valuey4 * 0.50));

                    var value1k4 = Convert.ToInt32(valuey4 * 1.696);
                    SetTextBoxValue(txt1k4, value1k4);
                    SetTextBoxValue(txt1k0, value1k4 * 3.00);
                    SetTextBoxValue(txt1k1, value1k4 * 2.35);
                    SetTextBoxValue(txt1k2, value1k4 * 1.90);
                    SetTextBoxValue(txt1k3, value1k4 * 1.45);
                    SetTextBoxValue(txt1k5, value1k4 * 0.90);
                    SetTextBoxValue(txt1k6, value1k4 * 0.78);
                    SetTextBoxValue(txt1k7, value1k4 * 0.58);
                    SetTextBoxValue(txt1k8, value1k4 * 0.50);

                    var value2k4 = Convert.ToInt32(value1k4 * 1.696);
                    SetTextBoxValue(txt2k4, value2k4);
                    SetTextBoxValue(txt2k0, value2k4 * 3.00);
                    SetTextBoxValue(txt2k1, value2k4 * 2.35);
                    SetTextBoxValue(txt2k2, value2k4 * 1.90);
                    SetTextBoxValue(txt2k3, value2k4 * 1.45);
                    SetTextBoxValue(txt2k5, value2k4 * 0.90);
                    SetTextBoxValue(txt2k6, value2k4 * 0.78);
                    SetTextBoxValue(txt2k7, value2k4 * 0.58);
                    SetTextBoxValue(txt2k8, value2k4 * 0.50);
                }
                else
                {
                    // Clear all TextBoxes if the input is not a valid number
                    ClearAllTextboxes();
                }
            }
            else
            {
                // Clear all TextBoxes if the input is empty or has more than 6 characters
                ClearAllTextboxes();
            }
        }

        // Method for setting TextBox values
        private void SetTextBoxValue(TextBox textBox, double value)
        {
            if (textBox != null)
            {
                textBox.Text = String.Format("{0:N0}", value);
            }
        }

        // Method for clearing all TextBoxes
        private void ClearAllTextboxes()
        {
            var textBoxes = this.Controls.OfType<TextBox>().ToList();
            foreach (var textBox in textBoxes)
            {
                textBox.Text = "0";
            }
        }
    }
}
