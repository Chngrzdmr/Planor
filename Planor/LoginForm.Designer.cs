using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms; // Guna.UI2 library is used for stylized controls

namespace Planor
{
    partial class LoginForm
    {
        #region Private Fields

        // Component container for the form
        private IContainer components;

        // Container panel for the login form elements
        private Guna2Panel panelContainer;

        // Username and password textboxes
        private Guna2TextBox txtUsername, txtPassword;

        // Login button
        private Guna2GradientButton btnLogin;

        // Elipse shape decoration for the form
        private Guna2Elipse elipseForm;

        // Drag control for the form
        private Guna2DragControl dragControl;

        // Animate window for the form
        private Guna2AnimateWindow animateWindow;

        // Control box for the form
        private Guna2ControlBox controlBox;

        // HTML label for displaying the version
        private Guna2HtmlLabel htmlLabelVersion;

        // Labels for displaying version, IP, and other information
        private Label lblVersion, lblIP, lblVRSYN, lblSubeID, lblTur, lblTramerka, lblTramerSifre;

        // Picture box for the background image
        private Guna2PictureBox pictureBoxBackground;

        #endregion

        #region Constructors

        public LoginForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void InitializeComponent()
        {
            // Initialize the components using the designer
            // This method contains the visual studio generated code for initializing the components

            // ...
        }

        #endregion

        #region Event Handlers

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Handle login button click event
            // Implement the logic for user authentication here
        }

        private void btnLogin_Enter(object sender, EventArgs e)
        {
            // Handle login button enter event
            // Implement any visual changes when the button is entered
        }

        private void btnLogin_Leave(object sender, EventArgs e)
        {
            // Handle login button leave event
            // Implement any visual changes when the button is left
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            // Handle login button mouse leave event
            // Implement any visual changes when the mouse leaves the button
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Handle form load event
            // Implement any logic that should be executed when the form is loaded
        }

        #endregion
    }
}
