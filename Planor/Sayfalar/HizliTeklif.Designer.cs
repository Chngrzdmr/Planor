using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    partial class HizliTeklif
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.SuspendLayout();

            this.label1.Dock = DockStyle.Fill;
            this.label1.Font = new Font("Segoe UI Semibold", 48F, FontStyle.Bold);
            this.label1.ForeColor = Color.FromArgb(41, 44, 81);
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(1200, 500);
            this.label1.Text = "Hızlı Teklifler çok yakında sizlerle...";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.label1);
            this.Name = "HizliTeklif";
            this.Size = new Size(1200, 500);
            this.Load += new EventHandler(HizliTeklif_Load);
            this.ResumeLayout(false);
        }

        private Label label1;
    }
}
