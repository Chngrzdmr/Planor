using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Planor.Sayfalar
{
    partial class SigortaSirketleri
    {
        // The 'components' field is used to hold components that need to be disposed when the form is disposed.
        private IContainer components = null;

        // The SigortaSirketleri constructor initializes the form and its components.
        public SigortaSirketleri()
        {
            InitializeComponent();
        }

        // The InitializeComponent method initializes the form's components, such as the DataGridView and buttons.
        private void InitializeComponent()
        {
            // ... (the same code as before)

            // dgw_sirket_listesi_CellMouseDoubleClick event handler
            // This event handler is called when a user double-clicks a cell in the DataGridView.
            this.dgw_sirket_listesi.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgw_sirket_listesi_CellMouseDoubleClick);

            // BtnSirketSil_Click_1 event handler
            // This event handler is called when the 'BtnSirketSil' button is clicked.
            this.BtnSirketSil.Click += new System.EventHandler(this.BtnSirketSil_Click_1);

            // BtnSirketEkle_Click_1 event handler
            // This event handler is called when the 'BtnSirketEkle' button is clicked.
            this.BtnSirketEkle.Click += new System.EventHandler(this.BtnSirketEkle_Click_1);

            // BtnKaydet_Click_1 event handler
            // This event handler is called when the 'BtnKaydet' button is clicked.
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click_1);

            // SigortaSirketleri_Load event handler
            // This event handler is called when the form is loaded.
            this.SigortaSirketleri.Load += new System.EventHandler(this.SigortaSirketleri_Load);
        }

        #region Event handlers

        // dgw_sirket_listesi_CellMouseDoubleClick event handler
        // This method is called when a user double-clicks a cell in the DataGridView.
        // It retrieves the selected row's data and displays it in the text boxes for editing.
        private void dgw_sirket_listesi_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // ... (the same code as before)
        }

        // BtnSirketSil_Click_1 event handler
        // This method is called when the 'BtnSirketSil' button is clicked.
        // It deletes the selected row from the DataGridView and the underlying data source.
        private void BtnSirketSil_Click_1(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        // BtnSirketEkle_Click_1 event handler
        // This method is called when the 'BtnSirketEkle' button is clicked.
        // It adds a new row to the DataGridView and enables editing in the text boxes.
        private void BtnSirketEkle_Click_1(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        // BtnKaydet_Click_1 event handler
        // This method is called when the 'BtnKaydet' button is clicked.
        // It saves any changes made to the DataGridView back to the underlying data source.
        private void BtnKaydet_Click_1(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        // SigortaSirketleri_Load event handler
        // This method is called when the form is loaded.
        // It initializes the DataGridView with data from the underlying data source.
        private void SigortaSirketleri_Load(object sender, EventArgs e)
        {
            // ... (the same code as before)
        }

        #endregion

        // The Dispose method is called when the form is disposed.
        // It releases the resources used by the form and its components.
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
