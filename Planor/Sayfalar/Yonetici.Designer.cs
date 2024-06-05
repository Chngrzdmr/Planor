using System; // Required for the System namespace
using System.ComponentModel; // Required for the ComponentModel namespace
using System.Drawing; // Required for the Drawing namespace
using System.Windows.Forms; // Required for the WindowsForms namespace
using Guna.UI2.WinForms; // Required for the Guna.UI2.WinForms namespace

namespace Planor.Pages // Start of the Planor.Pages namespace
{
    partial class Administrator // Start of the Administrator class
    {
        private IContainer components = null; // Declaration of the components variable

        private void InitializeComponent() // Start of the InitializeComponent method
        {
            // Declaration and initialization of the yoneticiSliderPNL Panel
            this.yoneticiSliderPNL = new Panel();

            // Declaration and initialization of the yoneticiMenuPNL Guna2GradientPanel
            this.yoneticiMenuPNL = new Guna2GradientPanel();

            // Declaration and initialization of the YoneticiBTN_1 Guna2GradientButton
            this.YoneticiBTN_1 = new Guna2GradientButton();

            // Declaration and initialization of the YoneticiBTN_2 Guna2GradientButton
            this.YoneticiBTN_2 = new Guna2GradientButton();

            // Declaration and initialization of the YoneticiBTN_3 Guna2GradientButton
            this.YoneticiBTN_3 = new Guna2GradientButton();

            // Configuration of the yoneticiMenuPNL Guna2GradientPanel
            // Adding the YoneticiBTN_1, YoneticiBTN_2, and YoneticiBTN_3 buttons to the yoneticiMenuPNL panel
            this.yoneticiMenuPNL.Controls.Add(this.YoneticiBTN_3);
            this.yoneticiMenuPNL.Controls.Add(this.YoneticiBTN_2);
            this.yoneticiMenuPNL.Controls.Add(this.YoneticiBTN_1);

            // Configuration of the YoneticiBTN_1 Guna2GradientButton
            // Setting the text of the YoneticiBTN_1 button to "YoneticiBTN_1"
            this.YoneticiBTN_1.Text = nameof(YoneticiBTN_1);

            // Adding a Click event handler to the YoneticiBTN_1 button
            this.YoneticiBTN_1.Click += new EventHandler(YoneticiBTN_1_Click);

            // Configuration of the YoneticiBTN_2 Guna2GradientButton
            // Setting the text of the YoneticiBTN_2 button to "YoneticiBTN_2"
            this.YoneticiBTN_2.Text = nameof(YoneticiBTN_2);

            // Adding a Click event handler to the YoneticiBTN_2 button
            this.YoneticiBTN_2.Click += new EventHandler(YoneticiBTN_2_Click);

            // Configuration of the YoneticiBTN_3 Guna2GradientButton
            // Setting the text of the YoneticiBTN_3 button to "YoneticiBTN_3"
            this.YoneticiBTN_3.Text = nameof(YoneticiBTN_3);

            // Adding a Click event handler to the YoneticiBTN_3 button
            this.YoneticiBTN_3.Click += new EventHandler(YoneticiBTN_3_Click);

            // Configuration of the Administrator class
            // Adding the yoneticiSliderPNL panel and yoneticiMenuPNL panel to the Administrator class
            this.Controls.Add(this.yoneticiSliderPNL);
            this.Controls.Add(this.yoneticiMenuPNL);

            // Setting the name of the Administrator class
            this.Name = nameof(Administrator);
        }

        // Dispose method for the Administrator class
        protected override void Dispose(bool disposing)
        {
            // Check if the components variable is not null
            if (disposing && (components != null))
            {
                // Dispose the components variable
                components.Dispose();
            }

            // Call the base Dispose method
            base.Dispose(disposing);
        }

        // Declaration of the yoneticiSliderPNL Panel
        private Panel yoneticiSliderPNL;

        // Declaration of the YoneticiBTN_1 Guna2GradientButton
        private Guna2GradientButton YoneticiBTN_1;

        // Declaration of the YoneticiBTN_2 Guna2GradientButton
        private Guna2GradientButton YoneticiBTN_2;

        // Declaration of the YoneticiBTN_3 Guna2GradientButton
        private Guna2GradientButton YoneticiBTN_3;

        // Declaration of the yoneticiMenuPNL Guna2GradientPanel
        private Guna2GradientPanel yoneticiMenuPNL;
    }
}

