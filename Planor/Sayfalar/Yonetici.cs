using Guna.UI2.WinForms; // Guna UI library for WinForms
using System; // Base class library for common language runtime
using System.Collections.Generic; // Provides classes for working with collections of objects
using System.ComponentModel; // Provides classes for component and property model
using System.Data; // Provides classes for working with data
using System.Drawing; // Provides classes for working with graphics and images
using System.Linq; // Provides classes for querying and working with data sources
using System.Text; // Provides classes for working with strings
using System.Threading.Tasks; // Provides types for working with tasks and parallelism
using System.Windows.Forms; // Base class library for Windows Forms

namespace Planor.Sayfalar // Planor's Pages namespace
{
    public partial class Yonetici : UserControl // Yonetici class that inherits from UserControl
    {
        private const int PanelMargin = 8; // Constant for panel margin

        public Yonetici() // Constructor for Yonetici class
        {
            InitializeComponent(); // Initializes the component
            SizeChanged += Yonetici_SizeChanged; // Subscribes to the SizeChanged event
        }

        private void Yonetici_Load(object sender, EventArgs e) // Load event handler for Yonetici class
        {
            AdjustSize(); // Calls AdjustSize method
            AddChildControls(); // Calls AddChildControls method
            YoneticiMenuDegistir("SİGORTA ŞİRKETLERİ"); // Calls YoneticiMenuDegistir method with "SİGORTA ŞİRKETLERİ" as parameter
        }

        private void AdjustSize() // Method for adjusting size
        {
            var screen = Screen.FromControl(this); // Gets the screen from the current control
            Width = screen.WorkingArea.Width - (screen.WorkingArea.Width / 8); // Sets the width of the control
            Height = new SistemForm().PanelSlider.Height; // Sets the height of the control
        }

        private void AddChildControls() // Method for adding child controls
        {
            try // Try block
            {
                yoneticiSliderPNL.Controls.Add(new SigortaSirketleri()); // Adds a new SigortaSirketleri control to the yoneticiSliderPNL
                yoneticiSliderPNL.Controls.Add(new SubeAyarlari()); // Adds a new SubeAyarlari control to the yoneticiSliderPNL
                yoneticiSliderPNL.Controls.Add(new KullaniciYonetimi()); // Adds a new KullaniciYonetimi control to the yoneticiSliderPNL
            }
            catch (Exception ex) // Catch block for exceptions
            {
                MessageBox.Show(ex.ToString()); // Shows the exception message
            }
            finally // Finally block
            {
                foreach (Control control in yoneticiSliderPNL.Controls) // Loop through all controls in yoneticiSliderPNL
                {
                    control.SendToBack(); // Sends the control to the back
                }
            }
        }

        private void YoneticiMenuDegistir(string MenuText) // Method for changing the Yonetici menu
        {
            foreach (Guna2GradientButton button in yoneticiMenuPNL.Controls.OfType<Guna2GradientButton>()) // Loop through all Guna2GradientButton controls in yoneticiMenuPNL
            {
                button.Checked = button.Text == MenuText; // Sets the Checked property of the button based on the MenuText parameter
            }

            foreach (Control control in yoneticiSliderPNL.Controls) // Loop through all controls in yoneticiSliderPNL
            {
                control.Visible = false; // Sets the Visible property of the control to false
                if (control.GetType().Name == MenuText.ToLower()) // If the name of the control's type is equal to the MenuText parameter in lowercase
                {
                    control.Visible = true; // Sets the Visible property of the control to true
                    control.BringToFront(); // Brings the control to the front
                    break; // Exits the loop
                }
            }
        }

        private void SigortaSirketleriBTN_Click(object sender, EventArgs e) // Click event handler for SigortaSirketleri button
        {
            var button = sender as Guna2GradientButton; // Gets the button that was clicked
            YoneticiMenuDegistir(button.Text); // Calls YoneticiMenuDegistir method with the button's text as parameter
        }

        private void GorunumDegistir() // Method for changing the appearance
        {
            var screen = Screen.FromControl(this); // Gets the screen from the current control
            int wdth = screen.WorkingArea.Width; // Gets the width of the working area
            int hght = screen.WorkingArea.Height; // Gets the height of the working area

            yoneticiMenuPNL.Size = new Size(wdth / 9, hght - PanelMargin); // Sets the size of the yoneticiMenuPNL
            YoneticiBTN_1.Size = new Size((wdth / 9) - PanelMargin, hght / 10); // Sets the size of the YoneticiBTN_1
            YoneticiBTN_2.Size = new Size((wdth / 9) - PanelMargin, hght / 10); // Sets the size of the YoneticiBTN_2
            YoneticiBTN_3.Size = new Size((wdth / 9) - PanelMargin, hght / 10); // Sets the size of the YoneticiBTN_3
        }

        private void yeniolcekleme() // Method for scaling the controls
        {
            float firstWidth = 1260; // Original width of the form
            float firstHeight = 700; // Original height of the form

            float size1 = this.Size.Width / firstWidth; // Calculates the size factor for the width
            float size2 = this.Size.Height / firstHeight; // Calculates the size factor for the height

            SizeF scale = new SizeF(size1, size2); // Creates a new SizeF object with the size factors
            firstWidth = this.Size.Width; // Sets the firstWidth variable to the current width of the form
            firstHeight = this.Size.Height; // Sets the firstHeight variable to the current height of the form

            foreach (Control control in this.Controls) // Loop through all controls in the form
            {
                control.Font = new Font(control.Font.FontFamily, control.Font.Size * ((size1 + size2) / 2)); // Scales the font size of the control
                control.Scale(scale); // Scales the control
            }
        }

        private void Yonetici_SizeChanged(object sender, EventArgs e) // SizeChanged event handler for Yonetici class
        {
            GorunumDegistir(); // Calls GorunumDegistir method
            yeniolcekleme(); // Calls yeniolcekleme method
        }

        private void button1_Click(object sender, EventArgs e) // Click event handler for button1
        {
            // Code for button1 click event
        }
    }
}
