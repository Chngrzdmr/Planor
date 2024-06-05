using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Planor.Kalaslar;
using Planor.Sayfalar;
using System.Globalization;
using System.Linq;

namespace Planor.Sayfalar
{
    public partial class HizliAraclar : UserControl
    {
        SistemForm sistm;
        General gn;

        public HizliAraclar()
        {
            InitializeComponent();
            sistm = new SistemForm();
            gn = new General();
        }

        private void HizliAraclar_Load(object sender, EventArgs e)
        {
            this.Width = sistm.screens[sistm.ekranno].WorkingArea.Width - (sistm.screens[sistm.ekranno].WorkingArea.Width / 8);
            this.Height = sistm.PanelSlider.Height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sistm.Trayyy1.ShowBalloonTip(1000, "Bekleyiniz...", "Yeniden deneme için 15 saniye daha bekleyiniz.", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while showing the balloon tip: {ex.Message}");
            }
        }

        private void PanodanImageBTN_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                try
                {
                    var image = Clipboard.GetImage();
                    var tempFilePath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToFileTime()}_satis-sozlesmesi_img.png");
                    image.Save(tempFilePath, System.Drawing.Imaging.ImageFormat.Png);
                    PanoImageBox.Image = image;
                    txt_kayityolu.Text = tempFilePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the image: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen ilk önce \"Satış Sözleşmesi\" 'ni Sağtuş ile Kopyala işlemi yapınız...");
            }
        }

        private void SatistanIptaliBaslatBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_kayityolu.Text) ||
                cmb_sirket.SelectedIndex == -1 ||
                string.IsNullOrEmpty(txt_policeno.Text) ||
                string.IsNullOrEmpty(txt_SatisTarihi.Text) ||
                string.IsNullOrEmpty(txt_noter.Text) ||
                string.IsNullOrEmpty(txt_YevmiyeNo.Text))
            {
                MessageBox.Show("Lütfen önce bilgileri eksiksiz doldurunuz...");
                return;
            }

            var uzuncumle = $"{txt_kayityolu.Text};" +
                            $"{cmb_sirket.SelectedItem};" +
                            $"{txt_policeno.Text};" +
                            $"{txt_SatisTarihi.Text};" +
                            $"{txt_noter.Text};" +
                            $"{txt_YevmiyeNo.Text};";

            var sifrelicumle = gn._Encrypt(uzuncumle, true);

            try
            {
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
                MessageBox.Show("An error occurred while starting the process: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            cmb_sirket.Items.Add(new ComboBoxItem<string>("sirket_adi", default(string)));
        }

        public class ComboBoxItem<T>
        {
            public string Name { get; set; }
            public T Value { get; set; }

            public ComboBoxItem(string name, T value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private void txtesas4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtesas4.Text) && txtesas4.Text.Length < 7)
            {
                if (double.TryParse(txtesas4.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double nValue))
                {
                    txts4.Text = String.Format("{0:N0}", nValue);
                    SetTextBoxValue(txts0, nValue * 3.00);
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
                    ClearAllTextboxes();
                }
            }
            else
            {
                ClearAllTextboxes();
            }
        }

        private void SetTextBoxValue(TextBox textBox, double value)
        {
            if (textBox != null)
            {
                textBox.Text = String.Format("{0:N0}", value);
            }
        }

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
