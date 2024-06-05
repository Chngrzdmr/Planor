using System;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class HizliTeklif : UserControl
    {
        public HizliTeklif()
        {
            InitializeComponent();
            InitializeFormSize();
        }

        private void HizliTeklif_Load(object sender, EventArgs e)
        {
            InitializeFormSize();
        }

        private void InitializeFormSize()
        {
            try
            {
                // Get the screen dimensions
                var screen = Screen.FromControl(this);

                // Set the form's width and height
                this.Width = screen.WorkingArea.Width - (screen.WorkingArea.Width / 8);
                this.Height = new SistemForm().Size.Height;
            }
            catch (Exception ex)
            {
                // Log the exception or show an error message
                Console.WriteLine("Error initializing form size: " + ex.Message);
            }
        }
    }
}
