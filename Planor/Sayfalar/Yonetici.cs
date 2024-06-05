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
        private const int PanelMargin = 8;

        public Yonetici()
        {
            InitializeComponent();
            SizeChanged += Yonetici_SizeChanged;
        }

        private void Yonetici_Load(object sender, EventArgs e)
        {
            AdjustSize();
            AddChildControls();
            YoneticiMenuDegistir("SİGORTA ŞİRKETLERİ");
        }

        private void AdjustSize()
        {
            var screen = Screen.FromControl(this);
            Width = screen.WorkingArea.Width - (screen.WorkingArea.Width / 8);
            Height = new SistemForm().PanelSlider.Height;
        }

        private void AddChildControls()
        {
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
                foreach (Control control in yoneticiSliderPNL.Controls)
                {
                    control.SendToBack();
                }
            }
        }

        private void YoneticiMenuDegistir(string MenuText)
        {
            foreach (Guna2GradientButton button in yoneticiMenuPNL.Controls.OfType<Guna2GradientButton>())
            {
                button.Checked = button.Text == MenuText;
            }

            foreach (Control control in yoneticiSliderPNL.Controls)
            {
                control.Visible = false;
                if (control.GetType().Name == MenuText.ToLower())
                {
                    control.Visible = true;
                    control.BringToFront();
                    break;
                }
            }
        }

        private void SigortaSirketleriBTN_Click(object sender, EventArgs e)
        {
            var button = sender as Guna2GradientButton;
            YoneticiMenuDegistir(button.Text);
        }

        private void GorunumDegistir()
        {
            var screen = Screen.FromControl(this);
            int wdth = screen.WorkingArea.Width;
            int hght = screen.WorkingArea.Height;

            yoneticiMenuPNL.Size = new Size(wdth / 9, hght - PanelMargin);
            YoneticiBTN_1.Size = new Size((wdth / 9) - PanelMargin, hght / 10);
            YoneticiBTN_2.Size = new Size((wdth / 9) - PanelMargin, hght / 10);
            YoneticiBTN_3.Size = new Size((wdth / 9) - PanelMargin, hght / 10);
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
            GorunumDegistir();
            yeniolcekleme();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
