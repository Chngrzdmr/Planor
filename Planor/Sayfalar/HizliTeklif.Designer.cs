using System; // Needed for basic system functionality
using System.Drawing; // Needed for graphics functionality
using System.Windows.Forms; // Needed for Windows forms functionality
using System.ComponentModel; // Needed for component functionality

namespace Planor.Sayfalar // The namespace for this class
{
    partial class HizliTeklif // The name of this class
    {
        private IContainer components = null; // A container for the components of this form

        public HizliTeklif() // The constructor for this class
        {
            InitializeComponent(); // Initialize the components of this form
        }

        protected override void Dispose(bool disposing) // The Dispose method for this class
        {
            if (disposing && (components != null)) // If disposing is true and components is not null
            {
                components.Dispose(); // Dispose the components
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() // The InitializeComponent method for this class
        {
            this.label1 = new Label(); // Create a new label component

            // Set the Dock property of the label to fill the form
            this.label1.Dock = DockStyle.Fill;

            // Set the font of the label to Segoe UI Semibold with a size of 48 and a style of Bold
            this.label1.Font = new Font("Segoe UI Semibold", 48F, FontStyle.Bold);

            // Set the ForeColor property of the label to a custom color
            this.label1.ForeColor = Color.FromArgb(41, 44, 81);

            // Set the Location property of the label to (0, 0)
            this.label1.Location = new Point(0, 0);

            // Set the Name property of the label to "label1"
            this.label1.Name = "label1";

            // Set the Size property of the label to (1200, 500)
            this.label1.Size = new Size(1200, 500);

            // Set the Text property of the label to "Hızlı Teklifler çok yakında sizlerle..."
            this.label1.Text = "Hızlı Teklifler çok yakında sizlerle...";

            // Set the TextAlign property of the label to MiddleCenter
            this.label1.TextAlign = ContentAlignment.MiddleCenter;

            // Set the Text property of the form to "Hızlı Teklif"
            this.Text = "Hızlı Teklif";

            // Set the AutoScaleDimensions property of the form to a new SizeF(6F, 13F)
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            // Set the AutoScaleMode property of the form to AutoScaleMode.Font
            this.AutoScaleMode = AutoScaleMode.Font;

            // Set the BackColor property of the form to Color.White
            this.BackColor = Color.White;

            // Add the label to the Controls collection of the form
            this.Controls.Add(this.label1);

            // Set the Name property of the form to "HizliTeklif"
            this.Name = "HizliTeklif";

            // Set the Size property of the form to (1200, 500)
            this.Size = new Size(1200, 500);

            // Add the event handler for the Load event
            this.Load += new EventHandler(HizliTeklif_Load);

            this.ResumeLayout(false);
        }

        private Label label1; // The label component

        private void HizliTeklif_Load(object sender, EventArgs e)
        {
            // Add the initialization code here
        }
    }
}

