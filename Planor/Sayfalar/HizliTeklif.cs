using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            //sayfa ölçeklendirme
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().PanelSlider.Height;
        }
    }
}
