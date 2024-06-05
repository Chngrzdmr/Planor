using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Planor.Kalaslar;
using Planor.Sayfalar;
using System.Globalization;

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
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().PanelSlider.Height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sistm.Trayyy1. ShowBalloonTip(1000, "Bekleyiniz...", "Yeniden deneme için 15 saniye daha bekleyiniz.", ToolTipIcon.Info);
        }

        private void PanodanImageBTN_Click(object sender, EventArgs e)
        {
            Image pp1;
            if (Clipboard.ContainsImage())
            {
                string kayityolu = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToFileTime()}_satis-sozlesmesi_img.png");

                pp1 = Clipboard.GetImage();
                try
                {
                    pp1.Save(kayityolu, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("\"Satış Sözleşmesi\" kaydedilerken hata oluştu, lütfen yeniden deneyiniz.");
                    MessageBox.Show(ex.ToString());
                }
                if (File.Exists(kayityolu))
                {
                    PanoImageBox.Image = pp1;
                    txt_kayityolu.Text = kayityolu;
                }
                else
                {
                    MessageBox.Show("\"Satış Sözleşmesi\" kaydedilerken hata oluştu, lütfen yeniden deneyiniz.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen ilk önce \"Satış Sözleşmesi\" 'ni Sağtuş ile Kopyala işlemi yapınız...");
            }
        }

        private void SatistanIptaliBaslatBTN_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_kayityolu.Text) &&
                cmb_sirket.SelectedIndex != -1 &&
                !string.IsNullOrEmpty(txt_policeno.Text) &&
                !string.IsNullOrEmpty(txt_SatisTarihi.Text) &&
                !string.IsNullOrEmpty(txt_noter.Text) &&
                !string.IsNullOrEmpty(txt_YevmiyeNo.Text))
            {
                string uzuncumle = $"{txt_kayityolu.Text};" +
                    $"{cmb_sirket.SelectedItem};" +
                    $"{txt_policeno.Text};" +
                    $"{txt_SatisTarihi.Text};" +
                    $"{txt_noter.Text};" +
                    $"{txt_YevmiyeNo.Text};";

                string sifrelicumle = gn._Encrypt(uzuncumle, true);

                var ProcessUNCS = new Process();
                ProcessUNCS.StartInfo.WorkingDirectory = @"C:\Users\Cihangir ÖZDEMİR\Source\Repos\SatistaNiptaLRobotu\SatistaNiptaLRobotu\bin\Debug\net5.0-windows";                
                ProcessUNCS.StartInfo.FileName = "SatistaNiptaLRobotu.exe";
                ProcessUNCS.StartInfo.Arguments = sifrelicumle;
                ProcessUNCS.Start();

            }
            else
            {
                MessageBox.Show("Lütfen önce bilgileri eksiksiz doldurunuz...");
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            cmb_sirket.Items.Add(new ComboBoxItem<string>("sirket_adi", default(string)));
        }

        public class ComboBoxItem<T>
        {
            public string Name { get; set; }
            public T value { get; set; }

            public ComboBoxItem(string Name, T value)
            {
                this.Name = Name;
                this.value = value;
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
                    txts0.Text = String.Format("{0:N0}", Math.Truncate(nValue * 3.00));
                    txts1.Text = String.Format("{0:N0}", Math.Truncate(nValue * 2.35));
                    txts2.Text = String.Format("{0:N0}", Math.Truncate(nValue * 1.90));
                    txts3.Text = String.Format("{0:N0}", Math.Truncate(nValue * 1.45));
                    txts5.Text = String.Format("{0:N0}", Math.Truncate(nValue * 0.90));
                    txts6.Text = String.Format("{0:N0}", Math.Truncate(nValue * 0.78));
                    txts7.Text = String.Format("{0:N0}", Math.Truncate(nValue * 0.58));
                    txts8.Text = String.Format("{0:N0}", Math.Truncate(nValue * 0.50));

                    txt1a4.Text = String.Format("{0:N0}", Math.Truncate(nValue * 1.0475));
                    txt1a0.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 3.00));
                    txt1a1.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 2.35));
                    txt1a2.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 1.90));
                    txt1a3.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 1.45));
                    txt1a5.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 0.90));
                    txt1a6.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 0.78));
                    txt1a7.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 0.58));
                    txt1a8.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1a4.Text.Replace(".", "")) * 0.50));

                    txty4.Text = String.Format("{0:N0}", Math.Truncate(nValue * 1.696));
                    txty0.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 3.00));
                    txty1.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 2.35));
                    txty2.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 1.90));
                    txty3.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 1.45));
                    txty5.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 0.90));
                    txty6.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 0.78));
                    txty7.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 0.58));
                    txty8.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 0.50));

                    txt1k4.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txty4.Text.Replace(".", "")) * 1.696));
                    txt1k0.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 3.00));
                    txt1k1.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 2.35));
                    txt1k2.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 1.90));
                    txt1k3.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 1.45));
                    txt1k5.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 0.90));
                    txt1k6.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 0.78));
                    txt1k7.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 0.58));
                    txt1k8.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 0.50));

                    txt2k4.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt1k4.Text.Replace(".", "")) * 1.696));
                    txt2k0.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 3.00));
                    txt2k1.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 2.35));
                    txt2k2.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 1.90));
                    txt2k3.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 1.45));
                    txt2k5.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 0.90));
                    txt2k6.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 0.78));
                    txt2k7.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 0.58));
                    txt2k8.Text = String.Format("{0:N0}", Math.Truncate(Convert.ToInt32(txt2k4.Text.Replace(".", "")) * 0.50));
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

        private void ClearAllTextboxes()
        {
            txts0.Text = "0";
            txts1.Text = "0";
            txts2.Text = "0";
            txts3.Text = "0";
            txts4.Text = "0";
            txts5.Text = "0";
            txts6.Text = "0";
            txts7.Text = "0";
            txts8.Text = "0";
            txty4.Text = "0";
            txty0.Text = "0";
            txty1.Text = "0";
            txty2.Text = "0";
            txty3.Text = "0";
            txty5.Text = "0";
            txty6.Text = "0";
            txty7.Text = "0";
            txty8.Text = "0";
            txt1k4.Text = "0";
            txt1k0.Text = "0";
            txt1k1.Text = "0";
            txt1k2.Text = "0";
            txt1k3.Text = "0";
            txt1k5.Text = "0";
            txt1k6.Text = "0";
            txt1k7.Text = "0";
            txt1k8.Text = "0";
            txt2k4.Text = "0";
            txt2k0.Text = "0";
            txt2k1.Text = "0";
            txt2k2.Text = "0";
            txt2k3.Text = "0";
            txt2k5.Text = "0";
            txt2k6.Text = "0";
            txt2k7.Text = "0";
            txt2k8.Text = "0";
            txt1a0.Text = "0";
            txt1a1.Text = "0";
            txt1a2.Text = "0";
            txt1a3.Text = "0";
            txt1a4.Text = "0";
            txt1a5.Text = "0";
            txt1a6.Text = "0";
            txt1a7.Text = "0";
            txt1a8.Text = "0";
        }
    }
}
