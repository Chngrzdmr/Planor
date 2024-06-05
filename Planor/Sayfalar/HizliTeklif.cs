using System;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class HizliTeklif : UserControl
    {
        public HizliTeklif()
        {
            InitializeComponent();
        }

        private void HizliTeklif_Load(object sender, EventArgs e)
        {
            // Get the screen dimensions
            var screen = Screen.FromControl(this);
            this.Width = screen.WorkingArea.Width - (screen.WorkingArea.Width / 8);
            this.Height = new SistemForm().PanelSlider.Height;
        }
    }
}
