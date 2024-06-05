using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    partial class Hakkimizda
    {
        #region Private Fields

        private System.ComponentModel.IContainer components;
        private Label label1;

        #endregion

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // label1
            this.label1.Dock = DockStyle.Fill;
            this.label1.Font = new Font("Segoe UI Semibold", 48F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = Color.FromArgb(41, 44, 81);
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(1249, 694);
            this.label1.TabIndex = 1;
            this.label1.Text = "Welcome to our Insurance Companies page!";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;

            // Hakkimizda2
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = Color.White;
            this.Controls.Add(this.label1);
            this.ImeMode = ImeMode.NoControl;
            this.Name = "Hakkimizda";
            this.Size = new Size(1249, 694);
            this.Load += new EventHandler(this.Hakkimizda2_Load);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
