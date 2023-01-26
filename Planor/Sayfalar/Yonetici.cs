using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class Yonetici : UserControl
    {
        public Yonetici()
        {
            InitializeComponent();

        }

        private void Yonetici_Load(object sender, EventArgs e)
        {
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().PanelSlider.Height;

            try
            {

                yoneticiSliderPNL.Controls.Add(new SigortaSirketleri());
                yoneticiSliderPNL.Controls.Add(new SubeAyarlari());
                yoneticiSliderPNL.Controls.Add(new KullaniciYonetimi());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                yoneticiSliderPNL.Controls.Find("SigortaSirketleri", false)[0].SendToBack();
                yoneticiSliderPNL.Controls.Find("SubeAyarlari", false)[0].SendToBack();
                yoneticiSliderPNL.Controls.Find("KullaniciYonetimi", false)[0].SendToBack();
                //ayarlarSliderPNL.Controls.Find("AnaSayfa2", false)[0].SendToBack();
                //ayarlarSliderPNL.Controls.Find("Hakkimizda2", false)[0].SendToBack();
            }
            YoneticiMenuDegistir("SİGORTA ŞİRKETLERİ");

            //yeniolcekleme();

            //GorunumDegistir();

            //MessageBox.Show("Yonetici _Load Son işlem: " + new SistemForm().PanelSlider.Width.ToString());
        }









        private void YoneticiMenuDegistir(string MenuText)
        {
            switch (MenuText)
            {
                case "SİGORTA ŞİRKETLERİ":
                    yoneticiSliderPNL.Controls.Find("SigortaSirketleri", false)[0].BringToFront();
                    break;
                case "ŞUBE AYARLARI":
                    yoneticiSliderPNL.Controls.Find("SubeAyarlari", false)[0].BringToFront();
                    break;
                case "KULLANICILAR":
                    yoneticiSliderPNL.Controls.Find("KullaniciYonetimi", false)[0].BringToFront();
                    break;
                case "BU BOS CASE KALACAK":
                    yoneticiSliderPNL.Controls.Find("Silme Kalsın", false)[0].BringToFront();
                    break;
            }
            foreach (Guna2GradientButton button in yoneticiMenuPNL.Controls.OfType<Guna2GradientButton>())
            {
                if (button.Text == MenuText)
                {
                    button.Checked = true;
                }
                else
                {
                    button.Checked = false;
                }
                Application.DoEvents();
            }
        }


        private void SigortaSirketleriBTN_Click(object sender, EventArgs e)
        {
            Guna2GradientButton item = (sender as Guna2GradientButton);
            YoneticiMenuDegistir(item.Text);
        }



        private void GorunumDegistir()
        {
            int wdth = new SistemForm().Width;
            
            
            
            int hght = new SistemForm().Height;

            this.yoneticiMenuPNL.Size = new Size(wdth / 9, 697);
            this.YoneticiBTN_1.Size = new Size((wdth / 9) - 6, hght / 10);
            this.YoneticiBTN_2.Size = new Size((wdth / 9) - 6, (hght / 10));
            this.YoneticiBTN_3.Size = new Size((wdth / 9) - 6, (hght / 10));
        }


        private void yeniolcekleme()
        {
            float firstWidth = 1260;
            float firstHeight = 700;

            float size1 = this.Size.Width / firstWidth;
            float size2 = this.Size.Height / firstHeight;

            SizeF scale = new SizeF(size1, size2);
            firstWidth = this.Size.Width;
            firstHeight = this.Size.Height;

            foreach (Control control in this.Controls)
            {

                control.Font = new Font(control.Font.FontFamily, control.Font.Size * ((size1 + size2) / 2));

                control.Scale(scale);


            }
        }


        private void Yonetici_SizeChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(this.Size.ToString());
            }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
