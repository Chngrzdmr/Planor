using Planor.Kalaslar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class SigortaSirketleri : UserControl
    {
        General gn = new General();
        public SigortaSirketleri()
        {
            InitializeComponent();
        }

        private void PopulateGridView(string query, DataGridView gridView)
        {
            try
            {
                gn.grid_view_getir(query, gridView);
                gridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                gridView.BackgroundColor = this.BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SigortaSirketleri_Load(object sender, EventArgs e)
        {
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height;

            string anaSirketlerQuery = "id, adi from t_ana_sirketler order by adi asc";
            string sirketlerQuery = " id,adi from t_sirketler order by adi asc";

            PopulateGridView(anaSirketlerQuery, dgw_sigorta_sirketleri);
            PopulateGridView(sirketlerQuery, dgw_sirket_listesi);
        }

        private void BtnSirketEkle_Click(object sender, EventArgs e)
        {
            if (dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("Şirket Seçiniz");
                return;
            }

            try
            {
                string anaSirketId = dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value.ToString();
                string anaSirketAdi = gn.en_son_kaydi_getir("t_ana_sirketler", "adi", $"where id='{anaSirketId}'");

                List<string> tabloAdlari = new List<string> { "adi" };
                ArrayList veriler = new ArrayList { anaSirketAdi };

                string sonuc = gn.db_kaydet(tabloAdlari, "t_sirketler", veriler);

                if (sonuc == "islem_tamam")
                {
                    PopulateGridView(" id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSirketSil_Click(object sender, EventArgs e)
        {
            if (dgw_sirket_listesi.CurrentRow.Cells[0] == null)
            {
                return;
            }

            DialogResult result = MessageBox.Show("Şirketi Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result.Equals(DialogResult.OK))
            {
                try
                {
                    string sirketId = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString();
                    string sonuc = gn.db_sil("t_sirketler", sirketId);

                    if (sonuc == "işlem tamamlandı")
                    {
                        PopulateGridView(" id,adi from t_sirketler", dgw_sirket_listesi);
                        MessageBox.Show("Şirket Silindi");
                    }
                    else
                    {
                        MessageBox.Show("Bir Hata Oluştu");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dgw_sirket_listesi_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgw_sirket_listesi.CurrentRow.Cells[0] == null)
            {
                return;
            }

            string sirketId = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString();
            string sirketAdi = gn.en_son_kaydi_getir("t_sirketler", "adi", $"where id='{sirketId}'");

            LblSirketID.Text = sirketId;
            TxtSirketAdi.Text = sirketAdi;

            SirketAdıDuzeltPNL.Visible = true;
            SirketAdıDuzeltPNL.Refresh();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSirketAdi.Text))
            {
                return;
            }

            DialogResult result = MessageBox.Show("Şirketi Düzenlemek İstediginize Eminmisiniz?", "Düzenle", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result.Equals(DialogResult.OK))
            {
                try
                {
                    List<string> tabloAdlari = new List<string> { "adi" };
                    ArrayList veriler = new ArrayList { TxtSirketAdi.Text };

                    string sonuc = gn.db_duzenle(tabloAdlari, "t_sirketler", veriler, "id", LblSirketID.Text);

                    if (sonuc == "islem_tamam")
                    {
                        PopulateGridView("id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
                        SirketAdıDuzeltPNL.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
