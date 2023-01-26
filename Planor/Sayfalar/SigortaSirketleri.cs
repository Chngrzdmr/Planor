using Planor.Kalaslar;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private void ana_sirketleri_getir()
        {
            gn.grid_view_getir("id,adi from t_ana_sirketler order by adi asc", dgw_sigorta_sirketleri);
        }


        private void SigortaSirketleri_Load(object sender, EventArgs e)
        {
            //ekrana göre ölçüleri ayarlamak için
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height;

            ana_sirketleri_getir();
            gn.grid_view_getir(" id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
            dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgw_sigorta_sirketleri.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgw_sigorta_sirketleri.BackgroundColor = this.BackColor;
            dgw_sirket_listesi.BackgroundColor = this.BackColor;


        }

        private void BtnSirketEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Şirket Seçiniz");
                }
                else
                {
                    string deger = dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value.ToString();
                    sirketlere_ekle(deger);

                    dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void sirketlere_ekle(string deger)
        {
            string adi = gn.en_son_kaydi_getir("t_ana_sirketler", "adi", "where id='" + deger + "'");

            List<string> TabloAdlari = new List<string>();
            TabloAdlari.Add("adi");

            ArrayList veriler = new ArrayList();
            veriler.Add(adi);

            string sonuc = gn.db_kaydet(TabloAdlari, "t_sirketler", veriler);
            if (sonuc == "islem_tamam")
            {
                gn.grid_view_getir(" id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
            }
        }

        private void BtnSirketSil_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Şirketi Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                if (dgw_sirket_listesi.CurrentRow.Cells[0] != null)
                {
                    string sonuc = gn.db_sil("t_sirketler", dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString());
                    if (sonuc == "işlem tamamlandı")
                    {
                        gn.grid_view_getir(" id,adi from t_sirketler", dgw_sirket_listesi);

                        dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        MessageBox.Show("Şirket Silindi");
                    }
                    else
                    {
                        MessageBox.Show("Bir Hata Oluştu");
                    }
                }
            }
            else
            {
            }
        }

        private void dgw_sirket_listesi_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SirketAdıDuzeltPNL.Visible = false;
            SirketAdıDuzeltPNL.Refresh();

            if (dgw_sirket_listesi.CurrentRow.Cells[0] != null)
            {
                string deger = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString();

                LblSirketID.Text = deger.ToString();
                TxtSirketAdi.Text = gn.en_son_kaydi_getir("t_sirketler", "adi", "where id='" + LblSirketID.Text + "'");

                SirketAdıDuzeltPNL.Visible = true;
                SirketAdıDuzeltPNL.Refresh();
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Şirketi Düzenlemek İstediginize Eminmisiniz?", "Düzenle", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result.Equals(DialogResult.OK))
            {

                List<string> TabloAdlari = new List<string>();
                TabloAdlari.Add("adi");

                ArrayList veriler = new ArrayList();
                veriler.Add(TxtSirketAdi.Text);

                string sonuc = gn.db_duzenle(TabloAdlari, "t_sirketler", veriler, "id", LblSirketID.Text);
                if (sonuc == "islem_tamam")
                {
                    gn.grid_view_getir("id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
                    dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    //SirketAdıDuzeltPNL.Visible = false;
                }

            }
        }

        private void BtnSirketEkle_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value == null)
                {
                    MessageBox.Show("Şirket Seçiniz");
                }
                else
                {
                    string deger = dgw_sigorta_sirketleri.CurrentRow.Cells[0].Value.ToString();
                    sirketlere_ekle(deger);

                    dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnSirketSil_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Şirketi Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                if (dgw_sirket_listesi.CurrentRow.Cells[0] != null)
                {
                    string sonuc = gn.db_sil("t_sirketler", dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString());
                    if (sonuc == "işlem tamamlandı")
                    {
                        gn.grid_view_getir(" id,adi from t_sirketler", dgw_sirket_listesi);

                        dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        MessageBox.Show("Şirket Silindi");
                    }
                    else
                    {
                        MessageBox.Show("Bir Hata Oluştu");
                    }
                }
            }
            else
            {
            }
        }

        private void BtnKaydet_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Şirketi Düzenlemek İstediginize Eminmisiniz?", "Düzenle", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result.Equals(DialogResult.OK))
            {

                List<string> TabloAdlari = new List<string>();
                TabloAdlari.Add("adi");

                ArrayList veriler = new ArrayList();
                veriler.Add(TxtSirketAdi.Text);

                string sonuc = gn.db_duzenle(TabloAdlari, "t_sirketler", veriler, "id", LblSirketID.Text);
                if (sonuc == "islem_tamam")
                {
                    gn.grid_view_getir("id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
                    dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    //SirketAdıDuzeltPNL.Visible = false;
                }

            }
        }
    }
}
