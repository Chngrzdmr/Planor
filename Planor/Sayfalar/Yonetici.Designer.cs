using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Planor.Pages
{
    partial class Administrator
    {
        private IContainer components = null;

        private void InitializeComponent()
        {
            this.yoneticiSliderPNL = new Panel();
            this.yoneticiMenuPNL = new Guna2GradientPanel();
            this.YoneticiBTN_3 = new Guna2GradientButton();
            this.YoneticiBTN_2 = new Guna2GradientButton();
            this.YoneticiBTN_1 = new Guna2GradientButton();

            // yoneticiMenuPNL
            this.yoneticiMenuPNL.Controls.Add(this.YoneticiBTN_3);
            this.yoneticiMenuPNL.Controls.Add(this.YoneticiBTN_2);
            this.yoneticiMenuPNL.Controls.Add(this.YoneticiBTN_1);

            // YoneticiBTN_1
            this.YoneticiBTN_1.Text = nameof(YoneticiBTN_1);
            this.YoneticiBTN_1.Click += new EventHandler(YoneticiBTN_1_Click);

            // YoneticiBTN_2
            this.YoneticiBTN_2.Text = nameof(YoneticiBTN_2);
            this.YoneticiBTN_2.Click += new EventHandler(YoneticiBTN_2_Click);

            // YoneticiBTN_3
            this.YoneticiBTN_3.Text = nameof(YoneticiBTN_3);
            this.YoneticiBTN_3.Click += new EventHandler(YoneticiBTN_3_Click);

            // Administrator
            this.Controls.Add(this.yoneticiSliderPNL);
            this.Controls.Add(this.yoneticiMenuPNL);
            this.Name = nameof(Administrator);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Panel yoneticiSliderPNL;
        private Guna2GradientButton YoneticiBTN_1;
        private Guna2GradientButton YoneticiBTN_2;
        private Guna2GradientButton YoneticiBTN_3;
        private Guna2GradientPanel yoneticiMenuPNL;
    }
}
