using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Planor
{
    partial class LoginForm
    {
        #region Private Fields

        private IContainer components;
        private Guna2Panel panelContainer;
        private Guna2TextBox txtUsername;
        private Guna2GradientButton btnLogin;
        private Label lblPassword;
        private Guna2TextBox txtPassword;
        private Guna2Elipse elipseForm;
        private Guna2DragControl dragControl;
        private Guna2AnimateWindow animateWindow;
        private Guna2ControlBox controlBox;
        private Guna2HtmlLabel htmlLabelVersion;
        private Label lblVersion;
        private Label lblIP;
        private Label lblVRSYN;
        private Label lblSubeID;
        private Label lblTur;
        private Label lblTramerka;
        private Label lblTramerSifre;
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
            components = new Container();
            panelContainer = new Guna2Panel();
            txtUsername = new Guna2TextBox();
            btnLogin = new Guna2GradientButton();
            lblPassword = new Label();
            txtPassword = new Guna2TextBox();
            elipseForm = new Guna2Elipse(components);
            dragControl = new Guna2DragControl(components);
            animateWindow = new Guna2AnimateWindow(components);
            controlBox = new Guna2ControlBox();
            htmlLabelVersion = new Guna2HtmlLabel();
            lblVersion = new Label();
            lblIP = new Label();
            lblVRSYN = new Label();
            lblSubeID = new Label();
            lblTur = new Label();
            lblTramerka = new Label();
            lblTramerSifre = new Label();
            pictureBoxBackground = new Guna2PictureBox();

            //
            // panelContainer
            //
            panelContainer.Controls.Add(txtUsername);
            panelContainer.Controls.Add(btnLogin);
            panelContainer.Controls.Add(lblPassword);
            panelContainer.Controls.Add(txtPassword);
            panelContainer.Location = new Point(147, 120);
            panelContainer.Name = "panelContainer";
            panelContainer.ShadowDecoration.Parent = panelContainer;
            panelContainer.Size = new Size(261, 213);

            //
            // txtUsername
            //
            txtUsername.AutoRoundedCorners = true;
            txtUsername.AutoValidate = AutoValidate.EnableAllowFocusChange;
            txtUsername.BorderRadius = 18;
            txtUsername.Cursor = Cursors.IBeam;
            txtUsername.DefaultText = "";
            txtUsername.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtUsername.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtUsername.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtUsername.DisabledState.Parent = txtUsername;
            txtUsername.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtUsername.FillColor = Color.FromArgb(229, 229, 229);
            txtUsername.FocusedState.BorderColor = Color.FromArgb(94, 148, 254);
            txtUsername.FocusedState.Parent = txtUsername;
            txtUsername.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtUsername.ForeColor = Color.Black;
            txtUsername.HoverState.BorderColor = Color.FromArgb(94, 148, 254);
            txtUsername.HoverState.Parent = txtUsername;
            txtUsername.IconLeft = Properties.Resources.Lock;
            txtUsername.IconLeftSize = new Size(30, 30);
            txtUsername.Location = new Point(4, 106);
            txtUsername.Margin = new Padding(4);
            txtUsername.Name = "txtUsername";
            txtUsername.PasswordChar = '*';
            txtUsername.PlaceholderForeColor = Color.FromArgb(64, 64, 64);
            txtUsername.PlaceholderText = "Kullanıcı Adı";
            txtUsername.SelectedText = "";
            txtUsername.ShadowDecoration.Parent = txtUsername;
            txtUsername.Size = new Size(253, 38);
            txtUsername.TabIndex = 2;

            //
            // btnLogin
            //
            btnLogin.AutoRoundedCorners = true;
            btnLogin.BackColor = Color.Transparent;
            btnLogin.BorderRadius = 21;
            btnLogin.CheckedState.Parent = btnLogin;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.CustomImages.Parent = btnLogin;
            btnLogin.FillColor = Color.FromArgb(90, 172, 228);
            btnLogin.FillColor2 = Color.FromArgb(25, 35, 132);
            btnLogin.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.HoverState.BorderColor = Color.Black;
            btnLogin.HoverState.CustomBorderColor = Color.Black;
            btnLogin.HoverState.FillColor = Color.FromArgb(25, 35, 132);
            btnLogin.HoverState.FillColor2 = Color.FromArgb(90, 172, 228);
            btnLogin.HoverState.Parent = btnLogin;
            btnLogin.Location = new Point(5, 161);
            btnLogin.Name = "btnLogin";
            btnLogin.ShadowDecoration.Depth = 20;
            btnLogin.ShadowDecoration.Parent = btnLogin;
            btnLogin.ShadowDecoration.Shadow = new Padding(0, 0, 0, 3);
            btnLogin.Size = new Size(253, 45);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "GİRİŞ YAP";
            btnLogin.Click += btnLogin_Click;
            btnLogin.Enter += btnLogin_Enter;
            btnLogin.Leave += btnLogin_Leave;
            btnLogin.MouseLeave += btnLogin_MouseLeave;

            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.BackColor = Color.Transparent;
            lblPassword.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            lblPassword.Location = new Point(48, 9);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(138, 30);

            //
            // txtPassword
            //
            txtPassword.AutoRoundedCorners = true;
            txtPassword.AutoValidate = AutoValidate.EnableAllowFocusChange;
            txtPassword.BorderRadius = 18;
            txtPassword.Cursor = Cursors.IBeam;
            txtPassword.DefaultText = "";
            txtPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.DisabledState.Parent = txtPassword;
            txtPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.FillColor = Color.FromArgb(229, 229, 229);
            txtPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.FocusedState.Parent = txtPassword;
            txtPassword.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtPassword.ForeColor = Color.Black;
            txtPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.HoverState.Parent = txtPassword;
            txtPassword.IconLeft = Properties.Resources.Lock;
            txtPassword.IconLeftSize = new Size(30, 30);
            txtPassword.Location = new Point(4, 52);
            txtPassword.Margin = new Padding(4);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PlaceholderForeColor = Color.FromArgb(64, 64, 64);
            txtPassword.PlaceholderText = "Şifre";
            txtPassword.SelectedText = "";
            txtPassword.ShadowDecoration.Parent = txtPassword;
            txtPassword.Size = new Size(253, 38);
            txtPassword.TabIndex = 1;

            //
            // elipseForm
            //
            elipseForm.TargetControl = this;

            //
            // dragControl
            //
            dragControl.TargetControl = this;

            //
            // animateWindow
            //
            animateWindow.AnimationType = Guna2AnimateWindow.AnimateWindowType.AW_SLIDE;

            //
            // controlBox
            //
            controlBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            controlBox.FillColor = Color.Transparent;
            controlBox.HoverState.Parent = controlBox;
            controlBox.IconColor = Color.Navy;
            controlBox.Location = new Point(383, 0);
            controlBox.Name = "controlBox";
            controlBox.ShadowDecoration.Parent = controlBox;
            controlBox.Size = new Size(30, 29);
            controlBox.TabIndex = 3;

            //
            // htmlLabelVersion
            //
            htmlLabelVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            htmlLabelVersion.BackColor = Color.Transparent;
            htmlLabelVersion.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            htmlLabelVersion.ForeColor = Color.Blue;
            htmlLabelVersion.Location = new Point(343, 351);
            htmlLabelVersion.Name = "htmlLabelVersion";
            htmlLabelVersion.Size = new Size(68, 17);

            //
            // lblVersion
            //
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Bold);
            lblVersion.ForeColor = Color.FromArgb(64, 64, 64);
            lblVersion.Location = new Point(54, 373);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(87, 15);

            //
            // lblIP
            //
            lblIP.AutoSize = true;
            lblIP.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Bold);
            lblIP.ForeColor = Color.FromArgb(64, 64, 64);
            lblIP.Location = new Point(151, 373);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(67, 15);

            //
            // lblVRSYN
            //
            lblVRSYN.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblVRSYN.AutoSize = true;
            lblVRSYN.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Bold);
            lblVRSYN.ForeColor = Color.FromArgb(64, 64, 64);
            lblVRSYN.Location = new Point(348, 370);
            lblVRSYN.Name = "lblVRSYN";
            lblVRSYN.Size = new Size(61, 15);

            //
            // lblSubeID
            //
            lblSubeID.AutoSize = true;
            lblSubeID.Location = new Point(297, 9);
            lblSubeID.Name = "lblSubeID";
            lblSubeID.Size = new Size(54, 13);
            lblSubeID.TabIndex = 8;
            lblSubeID.Visible = false;

            //
            // lblTur
            //
            lblTur.AutoSize = true;
            lblTur.Location = new Point(297, 22);
            lblTur.Name = "lblTur";
            lblTur.Size = new Size(29, 13);
            lblTur.TabIndex = 9;
            lblTur.Visible = false;

            //
            // lblTramerka
            //
            lblTramerka.AutoSize = true;
            lblTramerka.Location = new Point(297, 35);
            lblTramerka.Name = "lblTramerka";
            lblTramerka.Size = new Size(58, 13);
            lblTramerka.TabIndex = 10;
            lblTramerka.Visible = false;

            //
            // lblTramerSifre
            //
            lblTramerSifre.AutoSize = true;
            lblTramerSifre.Location = new Point(297, 48);
            lblTramerSifre.Name = "lblTramerSifre";
            lblTramerSifre.Size = new Size(65, 13);
            lblTramerSifre.TabIndex = 11;
            lblTramerSifre.Visible = false;

            //
            // pictureBoxBackground
            //
            pictureBoxBackground.Dock = DockStyle.Left;
            pictureBoxBackground.Image = Properties.Resources.login_blue_abstract_small;
            pictureBoxBackground.Location = new Point(0, 0);
            pictureBoxBackground.Name = "pictureBoxBackground";
            pictureBoxBackground.ShadowDecoration.Parent = pictureBoxBackground;
            pictureBoxBackground.Size = new Size(291, 390);

            //
            // LoginForm
            //
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(249, 248, 254);
            ClientSize = new Size(413, 390);
            Controls.Add(lblTramerSifre);
            Controls.Add(lblTramerka);
            Controls.Add(lblTur);
            Controls.Add(lblSubeID);
            Controls.Add(lblVRSYN);
            Controls.Add(lblIP);
            Controls.Add(lblVersion);
            Controls.Add(htmlLabelVersion);
            Controls.Add(controlBox);
            Controls.Add(panelContainer);
            Controls.Add(pictureBoxBackground);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)Properties.Resources.ResourceManager.GetObject("$this.Icon");
            Location = new Point(500, 50);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Load += LoginForm_Load;
        }

        #endregion

        #region Event Handlers

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Handle login button click event
        }

        private void btnLogin_Enter(object sender, EventArgs e)
        {
            // Handle login button enter event
        }

        private void btnLogin_Leave(object sender, EventArgs e)
        {
            // Handle login button leave event
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            // Handle login button mouse leave event
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Handle form load event
        }

        #endregion
    }
}
