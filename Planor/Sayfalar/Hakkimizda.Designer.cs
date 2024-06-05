using System; // Required for basic system functionalities
using System.ComponentModel; // Provides components for the design-time and run-time behavior of a component
using System.Drawing; // Required for graphics and drawing functionalities
using System.Windows.Forms; // Required for creating Windows-based applications

namespace Planor.Sayfalar // Defining the namespace for the Insurance Companies page
{
    public partial class Hakkimizda : Form // Defining the Hakkimizda class that inherits from the Form class
    {
        #region Private Fields

        // components - A container for components that this form requires.
        private System.ComponentModel.IContainer components;

        // label1 - A label to display a welcome message to the Insurance Companies page.
        private Label label1;

        #endregion

        public Hakkimizda() // Constructor for the Hakkimizda class
        {
            InitializeComponent(); // Initializes the form and its components
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) // If disposing is true and components are not null
            {
                components.Dispose(); // Dispose the components
            }
            base.Dispose(disposing);
        }

        #region Designer-generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label(); // Initialize the label1 component

            // ... (Other code omitted for brevity)

            // Hakkimizda2
            this.AutoScaleDimensions = new SizeF(6F, 13F); // Sets the automatic scaling dimensions
            this.AutoScaleMode = AutoScaleMode.Font; // Sets the automatic scaling mode
            this.AutoSize = true; // Sets the form to automatically size to its contents
            this.BackColor = Color.White; // Sets the form's background color to white
            this.Controls.Add(this.label1); // Adds the label1 component to the form's controls
            this.ImeMode = ImeMode.NoControl; // Sets the input method editor mode
            this.Name = "Hakkimizda"; // Sets the name of the form
            this.Size = new Size(1249, 694); // Sets the size of the form
            this.Load += new EventHandler(this.Hakkimizda2_Load); // Adds an event handler for the form's Load event
            this.ResumeLayout(false); // Resumes the layout of the form
        }

        private void Hakkimizda2_Load(object sender, EventArgs e)
        {
            // Initialize the form when it is loaded.
        }

        #endregion
    }
}
