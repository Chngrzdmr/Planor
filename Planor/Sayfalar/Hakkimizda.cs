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
    public partial class Hakkimizda : UserControl
    {
        public Hakkimizda()
        {
            InitializeComponent();
        }

        public Hakkimizda(Screen screen) : this()
        {
            SizeToWorkingArea(screen);
        }

        private void Hakkimizda2_Load(object sender, EventArgs e)
        {
            SizeToWorkingArea(new SystemForm().screens[new SystemForm().ekranno]);
        }

        private void SizeToWorkingArea(Screen screen)
        {
            if (screen == null || screen.WorkingArea.Width == 0 || screen.WorkingArea.Height == 0)
            {
                throw new ArgumentException("Invalid screen object.");
            }

            if (new SystemForm().PanelSlider == null)
            {
                throw new NullReferenceException("PanelSlider property of SystemForm is null.");
            }

            this.Width = CalculateWidth(screen.WorkingArea.Width);
            this.Height = new SystemForm().PanelSlider.Height;
        }

        private int CalculateWidth(int workingAreaWidth)
        {
            return workingAreaWidth - (workingAreaWidth / 8);
        }
    }
}
