using Planor.Kalaslar; // Planor library for classes
using System; // Base system library
using System.Collections.Generic; // Collection library
using System.Data.SqlClient; // SQL Server connection library
using System.Drawing; // Drawing library
using System.Windows.Forms; // Windows Forms library

namespace Planor.Sayfalar // Planor's Pages library
{
    public partial class SigortaSirketleri : UserControl // Insurance Companies user control
    {
        General gn = new General(); // General class object for common functions

        public SigortaSirketleri() // Constructor
        {
            InitializeComponent(); // Initialize the user control components
        }

        private void PopulateGridView(string query, DataGridView gridView) // Method to populate a DataGridView with a given query
        {
            try
            {
                gn.grid_view_getir(query, gridView); // Call General class method to execute the query and fill the grid view
                gridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells); // Auto-resize the columns
                gridView.BackgroundColor = this.BackColor; // Set the background color of the grid view
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Display any exceptions
            }
        }

        private void SigortaSirketleri_Load(object sender, EventArgs e) // User control load event
        {
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8); // Set the user control width
            this.Height = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height; // Set the user control height

            string anaSirketlerQuery = "id, adi from t_ana_sirketler order by adi asc"; // Query for the main insurance companies
            string sirketlerQuery = " id,adi from t_sirketler order by adi asc"; // Query for the sub-insurance companies

            PopulateGridView(anaSirketlerQuery, dgw_sigorta_sirketleri); // Populate the main insurance companies grid view
            PopulateGridView(sirketlerQuery, dgw_sirket_listesi); // Populate the sub-insurance companies grid view
        }

        private void BtnSirketEkle_Click(object sender, EventArgs e) // Button click event for adding a new insurance company
        {
            if (dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value == null) // Check if a main insurance company is selected
            {
                MessageBox.Show("Şirket Seçiniz"); // Display a message to select a main insurance company
                return;
            }

            try
            {
                string anaSirketId = dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value.ToString(); // Get the selected main insurance company ID
                string anaSirketAdi = gn.en_son_kaydi_getir("t_ana_sirketler", "adi", $"where id='{anaSirketId}'"); // Get the selected main insurance company name

                List<string> tabloAdlari = new List<string> { "adi" }; // Column names to insert
                ArrayList veriler = new ArrayList { anaSirketAdi }; // Values to insert

                string sonuc = gn.db_kaydet(tabloAdlari, "t_sirketler", veriler); // Call General class method to insert the new sub-insurance company

                if (sonuc == "islem_tamam") // Check if the insertion was successful
                {
                    PopulateGridView(" id,adi from t_sirketler order by adi asc", dgw_sirket_listesi); // Refresh the sub-insurance companies grid view
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Display any exceptions
            }
        }

        private void BtnSirketSil_Click(object sender, EventArgs e) // Button click event for deleting an insurance company
        {
            if (dgw_sirket_listesi.CurrentRow.Cells[0] == null) // Check if a sub-insurance company is selected
            {
                return;
            }

            DialogResult result = MessageBox.Show("Şirketi Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information); // Display a confirmation message

            if (result.Equals(DialogResult.OK)) // If the user confirms
            {
                try
                {
                    string sirketId = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString(); // Get the selected sub-insurance company ID
                    string sonuc = gn.db_sil("t_sirketler", sirketId); // Call General class method to delete the sub-insurance company

                    if (sonuc == "işlem tamamlandı") // Check if the deletion was successful
                    {
                        PopulateGridView(" id,adi from t_sirketler", dgw_sirket_listesi); // Refresh the sub-insurance companies grid view
                        MessageBox.Show("Şirket Silindi"); // Display a success message
                    }
                    else
                    {
                        MessageBox.Show("Bir Hata Oluştu"); // Display an error message
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); // Display any exceptions
                }
            }
        }

        private void dgw_sirket_listesi_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) // Double-click event for editing a sub-insurance company
        {
            if (dgw_sirket_listesi.CurrentRow.Cells[0] == null) // Check if a sub-insurance company is selected
            {
                return;
            }

            string sirketId = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString(); // Get the selected sub-insurance company ID
            string sirketAdi = gn.en_son_kaydi_getir("t_sirketler", "adi", $"where id='{sirketId}'"); // Get the selected sub-insurance company name

            LblSirketID.Text = sirketId; // Set the sub-insurance company ID label
            TxtSirketAdi.Text = sirketAdi; // Set the sub-insurance company name text box

            SirketAdıDuzeltPNL.Visible = true; // Show the sub-insurance company edit panel
            SirketAdıDuzeltPNL.Refresh(); // Refresh the sub-insurance company edit panel
        }

        private void BtnKaydet_Click(object sender, EventArgs e) // Button click event for saving changes to a sub-insurance company
        {
            if (string.IsNullOrEmpty(TxtSirketAdi.Text)) // Check if the sub-insurance company name is empty
            {
                return;
            }

            DialogResult result = MessageBox.Show("Şirketi Düzenlemek İstediginize Eminmisiniz?", "Düzenle", MessageBoxButtons.OKCancel, MessageBoxIcon.Information); // Display a confirmation message

            if (result.Equals(DialogResult.OK)) // If the user confirms
            {
                try
                {
                    List<string> tabloAdlari = new List<string> { "adi" }; // Column names to update
                    ArrayList veriler = new ArrayList { TxtSirketAdi.Text }; // Values to update

                    string sonuc = gn.db_duzenle(tabloAdlari, "t_sirketler", veriler, "id", LblSirketID.Text); // Call General class method to update the sub-insurance company

                    if (sonuc == "islem_tamam") // Check if the update was successful
                    {
                        PopulateGridView("id,adi from t_sirketler order by adi asc", dgw_sirket_listesi); // Refresh the sub-insurance companies grid view
                        SirketAdıDuzeltPNL.Visible = false; // Hide the sub-insurance company edit panel
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); // Display any exceptions
                }
            }
        }
    }
}
