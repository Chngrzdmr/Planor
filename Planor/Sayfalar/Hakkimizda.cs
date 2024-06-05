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
    // Hakkimizda class represents a UserControl that displays information about the application.
    public partial class Hakkimizda : UserControl
    {
        // Constructor for Hakkimizda class that initializes the UserControl.
        public Hakkimizda()
        {
            InitializeComponent();
        }

        // Overloaded constructor for Hakkimizda class that initializes the UserControl and sets its size to fit the specified screen's working area.
        public Hakkimizda(Screen screen) : this()
        {
            SizeToWorkingArea(screen);
        }

        // Event handler for the Load event of the UserControl. It sets the size of the UserControl to fit the primary screen's working area.
        private void Hakkimizda2_Load(object sender, EventArgs e)
        {
            SizeToWorkingArea(new SystemForm().screens[new SystemForm().ekranno]);
        }

        // Method that sets the size of the UserControl to fit the specified screen's working area.
        private void SizeToWorkingArea(Screen screen)
        {
            // Check if the screen object is valid.
            if (screen == null || screen.WorkingArea.Width == 0 || screen.WorkingArea.Height == 0)
            {
                throw new ArgumentException("Invalid screen object.");
            }

            // Check if the PanelSlider property of SystemForm is not null.
            if (new SystemForm().PanelSlider == null)
            {
                throw new NullReferenceException("PanelSlider property of SystemForm is null.");
            }

            // Set the width of the UserControl to fit the working area of the screen.
            this.Width = CalculateWidth(screen.WorkingArea.Width);

            // Set the height of the UserControl to the height of the PanelSlider property of SystemForm.
            this.Height = new SystemForm().PanelSlider.Height;
        }

        // Method that calculates the width of the UserControl based on the specified working area width.
        private int CalculateWidth(int workingAreaWidth)
        {
            // Calculate the width of the UserControl by subtracting 12.5% of the working area width from it.
            return workingAreaWidth - (workingAreaWidth / 8);
        }
    }
}
