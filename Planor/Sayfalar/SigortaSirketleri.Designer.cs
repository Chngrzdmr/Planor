using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Planor.Sayfalar
{
    partial class SigortaSirketleri
    {
        private IContainer components = null;

        public SigortaSirketleri()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // ... (the same code as before)
        }

        #region Event handlers

        private void dgw_sirket_listesi_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // ... (the same code as before)
        }

        private void BtnSirketSil_Click_1(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        private void BtnSirketEkle_Click_1(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        private void BtnKaydet_Click_1(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        private void SigortaSirketleri_Load(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
