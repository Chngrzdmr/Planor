using Planor.Kalaslar;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class KullaniciYonetimi : UserControl, IDisposable
    {
        private General gn;
        private SistemForm ssfr;

        public KullaniciYonetimi()
        {
            InitializeComponent();
            gn = new General();
            ssfr = new SistemForm();
        }

        private void KullaniciYonetimi_Load(object sender, EventArgs e)
        {
            SetFormSize();
            KullanicilariGetir();
            SigortaSirketleriGetir();
            SubeGetir();
            YetkiGetir();
            LblMaxSayi.Text = "999";
            LblSuankiSayi.Text = gn.adet_getir("t_kullanicilar", "id", "").ToString();
        }

        private void SetFormSize()
        {
            this.Width = ssfr.screens[ssfr.ekranno].WorkingArea.Width - ((ssfr.screens[ssfr.ekranno].WorkingArea.Width) / 8);
            this.Height = ssfr.screens[ssfr.ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height;
        }

        private void KullanicilariGetir()
        {
            GwKullanicilar.AllowUserToAddRows = false;
            GwKullanicilar.AutoGenerateColumns = true;

            gn.grid_view_getir(" t_kullanicilar.id AS ID, t_kullanicilar.adi AS KullaniciAdi, t_bayiler.adi AS Bayi_Adi FROM t_kullanicilar INNER JOIN t_bayiler ON t_bayiler.id = t_kullanicilar.bayi order by t_kullanicilar.adi asc ", GwKullanicilar);
            GwKullanicilar.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            SetGridViewColumns();
            kullanici_sayilarini_getir();
        }

        private void SetGridViewColumns()
        {
            GwKullanicilar.Columns["ID"].HeaderText = "Sıra No";
            GwKullanicilar.Columns["KullaniciAdi"].HeaderText = "Kullanıcı Adı";
            GwKullanicilar.Columns["Bayi_Adi"].HeaderText = "Bayi Adı";
        }

        private void kullanici_sayilarini_getir()
        {
            lbl_aktif_sayi.Text = gn.adet_getir("t_kullanicilar", "id", "").ToString();
            lbl_max_sayi.Text = "999";
        }

        private void SigortaSirketleriGetir()
        {
            dgw_sirket_listesi.AllowUserToAddRows = false;
            dgw_sirket_listesi.AutoGenerateColumns = true;

            gn.grid_view_getir(" id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
            dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            SetSirketListeColumns();
        }

        private void SetSirketListeColumns()
        {
            dgw_sirket_listesi.Columns[0].Width = 50;
            dgw_sirket_listesi.Columns["id"].HeaderText = "Sıra No";
            dgw_sirket_listesi.Columns["adi"].HeaderText = "Şirket Adı";
        }

        private void BtnKullaniciSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (GwKullanicilar.CurrentRow != null)
                {
                    DialogResult result = MessageBox.Show("Kullanıcıyı Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        SilKullanici();
                        KullanicilariGetir();
                        Temizle();
                    }
                }
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı Silme İşlemi Yapılırken", ex.Message);
            }
        }

        private void SilKullanici()
        {
            string deger = GwKullanicilar.CurrentRow.Cells[0].Value.ToString();

            string Sonuc1 = gn.db_sil_deger("t_kullanici_sirketler", deger, "KullaniciID");
            string sonuc = gn.db_sil("t_kullanicilar", deger);

            if (sonuc != "işlem tamamlandı")
            {
                throw new Exception("Kullanıcı silinirken bir hata oluştu.");
            }
        }

        private void GwKullanicilar_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (GwKullanicilar.CurrentRow != null)
                {
                    LblKullaniciID.Text = GwKullanicilar.CurrentRow.Cells[0].Value.ToString();
                    KullaniciSirketGetir(LblKullaniciID.Text);
                    Temizle();
                }
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı Tablosuna Click Yapılırken", ex.Message);
            }
        }

        private void KullaniciSirketGetir(string KullaniciID)
        {
            RgKullaniciSirketler.AllowUserToAddRows = false;
            RgKullaniciSirketler.AutoGenerateColumns = true;

            gn.grid_view_getir($" id,Adi from t_kullanici_sirketler where KullaniciID='{KullaniciID}' order by Adi asc", RgKullaniciSirketler);
            RgKullaniciSirketler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            SetSirketKullaniciColumns();
        }

        private void SetSirketKullaniciColumns()
        {
            RgKullaniciSirketler.Columns[0].Width = 50;
            RgKullaniciSirketler.Columns["id"].HeaderText = "Sıra No";
            RgKullaniciSirketler.Columns["adi"].HeaderText = "Şirket Adı";
        }

        private void GwKullanicilar_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (GwKullanicilar.CurrentRow != null)
                {
                    LblKullaniciID.Text = GwKullanicilar.CurrentRow.Cells[0].Value.ToString();
                    IcerikGetir();
                    KullaniciEklePNL.Text = "Kullanıcı DÜZENLEME İşlemleri";
                    Btn_KullaniciKaydet.Text = "Kullanıcı Bilgileri Düzelt";
                    Btn_KullaniciKaydet.Location = new Point(180, BtnKullaniciSil.Location.Y);
                    Btn_KullaniciKaydet.Width = KullaniciEklePNL.Width - 180 - 6;
                    Btn_KullaniciKaydet.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı Tablosuna Çift Click Yapılırken", ex.Message);
            }
            Temizle();
        }

        private void IcerikGetir()
        {
            string KullaniciID = LblKullaniciID.Text;

            using (MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti))
            {
                using (MySqlCommand com = new MySqlCommand(@"Select * from t_kullanicilar where id = @KullaniciID", con))
                {
                    com.Parameters.AddWithValue("@KullaniciID", KullaniciID);

                    try
                    {
                        con.Open();
                        using (MySqlDataReader oku = com.ExecuteReader())
                        {
                            if (oku.Read())
                            {
                                txt_kullanici_adi.Text = oku["adi"].ToString();
                                txt_sifre.Text = oku["sifre"].ToString();
                                txt_telefon.Text = oku["telefon"].ToString();
                                txt_tramer_ka.Text = oku["tramer_ka"].ToString();
                                txt_tramer_sifre.Text = oku["tramer_sifre"].ToString();

                                string SubeID = oku["bayi"].ToString();
                                string KullaniciTuru = oku["tur"].ToString();

                                if (!string.IsNullOrEmpty(SubeID))
                                {
                                    cmb_sube.SelectedValue = SubeID;
                                }

                                if (!string.IsNullOrEmpty(KullaniciTuru))
                                {
                                    cmb_tur.SelectedValue = KullaniciTuru;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı bilgileri DB den çekilirken", ex.Message);
                    }
                }
            }
        }

        private void sirketlere_ekle(string kullanici_id, string sirket_id)
        {
            string adi = gn.en_son_kaydi_getir("t_sirketler", "adi", $"where id='{sirket_id}'");
            List<string> TabloAdlari = new List<string> { "SirketID", "KullaniciID", "Adi" };
            ArrayList veriler = new ArrayList { sirket_id, kullanici_id, adi };

            string sonuc = gn.db_kaydet(TabloAdlari, "t_kullanici_sirketler", veriler);

            if (sonuc != "islem_tamam")
            {
                throw new Exception("Şirket ekleme işlemi sırasında bir hata oluştu.");
            }
        }

        private void BtnSirketEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (GwKullanicilar.CurrentRow != null && dgw_sirket_listesi.CurrentRow != null)
                {
                    sirketlere_ekle(LblKullaniciID.Text, dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString());
                    KullaniciSirketGetir(LblKullaniciID.Text);
                    Temizle();
                }
                else
                {
                    MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
                }
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Şirket ekleme yapılırken", ex.Message);
            }
        }

        private void BtnSirketSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (RgKullaniciSirketler.CurrentRow != null)
                {
                    string deger = RgKullaniciSirketler.CurrentRow.Cells[0].Value.ToString();

                    string sonuc = gn.db_sil("t_kullanici_sirketler", deger);

                    if (sonuc != "işlem tamamlandı")
                    {
                        throw new Exception("Şirket silme işlemi sırasında bir hata oluştu.");
                    }

                    KullaniciSirketGetir(LblKullaniciID.Text);
                    Temizle();
                }
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Şirket Sil Yapılırken", ex.Message);
            }
        }

        private void toplu_sirket_ekle(string g_sirket_id, string g_sirket_link, string g_link_adi, string g_kullanici_id)
        {
            List<string> TabloAdlari = new List<string> { "SirketID", "KullaniciID", "Adi", "Link", "LinkAdi" };
            ArrayList veriler = new ArrayList { g_sirket_id, g_kullanici_id, g_sirket_link, g_link_adi };

            try
            {
                gn.db_kaydet(TabloAdlari, "t_kullanici_sirketler", veriler);
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Toplu Şirket Ekleme yaparken", ex.Message);
                MessageBox.Show("Bir Hata Oluştu: " + ex.Message);
            }
        }

        private void BtnButunKullanicilaraEkle_Click(object sender, EventArgs e)
        {
            string sirket_id = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString();
            string adi = gn.en_son_kaydi_getir("t_sirketler", "adi", $"where id='{sirket_id}'");

            try
            {
                using (MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti))
                {
                    using (MySqlCommand com = new MySqlCommand("Select id from t_kullanicilar order by id asc", con))
                    {
                        con.Open();
                        using (MySqlDataReader dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string id = dr["id"].ToString();
                                toplu_sirket_ekle(sirket_id, "", adi, id);
                            }
                        }
                    }
                }

                MessageBox.Show("İşlem Tamamlandı");
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bir şirket bütün kullanıcılara eklenirken", ex.Message);
            }
        }

        private void toplu_sirket_silme(string id)
        {
            try
            {
                gn.db_sil("t_kullanici_sirketler", id);
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bütün şirketleri silmek için işlemler yapılırken", ex.Message);
                MessageBox.Show("Bir Hata Oluştu: " + ex.Message);
            }
        }

        private void BtnButunKullanicilardanSil_Click(object sender, EventArgs e)
        {
            string sirket_id = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString();

            try
            {
                using (MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti))
                {
                    using (MySqlCommand com = new MySqlCommand($"Delete from t_kullanici_sirketler where SirketID='{sirket_id}'", con))
                    {
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("İşlem Tamamlandı");
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bir şirket bütün kullanıcılardan silinirken", ex.Message);
            }
        }

        private void BtnTumSirketleriEkle_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow rowInfo in dgw_sirket_listesi.Rows)
                {
                    if (LblKullaniciID.Text != "" && rowInfo.Cells["id"].Value != null)
                    {
                        sirketlere_ekle(LblKullaniciID.Text, rowInfo.Cells["id"].Value.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
                    }
                    Application.DoEvents();
                }

                KullaniciSirketGetir(LblKullaniciID.Text);
                MessageBox.Show("İşlem Tamam");
                Temizle();
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcıya bir şirket eklenirken", ex.Message);
                MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
            }
        }

        private void BtnTumSirketleriSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (LblKullaniciID.Text != "")
                {
                    using (MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti))
                    {
                        using (MySqlCommand com = new MySqlCommand("Delete from t_kullanici_sirketler where KullaniciID=@KullaniciID", con))
                        {
                            com.Parameters.AddWithValue("@KullaniciID", LblKullaniciID.Text);

                            con.Open();
                            com.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
                }
            }
            catch (Exception ex)
            {
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bütün şirketleri silmek için işlemler yapılırken", ex.Message);
                MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
            }

            KullaniciSirketGetir(LblKullaniciID.Text);
            Temizle();
        }

        private void SubeGetir()
        {
            gn.combo_box_veri_getir(cmb_sube, " * from t_bayiler order by adi asc", "adi", "id");
        }

        private void YetkiGetir()
        {
            gn.combo_box_veri_getir(cmb_tur, " * from t_tur order by adi asc", "adi", "id");
        }

        private void Btn_KullaniciKaydet_Click(object sender, EventArgs e)
        {
            if (KullaniciEklePNL.Text == "Kullanıcı EKLEME İşlemleri")
            {
                int ToplamKullanici = Convert.ToInt16(LblMaxSayi.Text);
                int KullaniciSayisi = Convert.ToInt16(LblSuankiSayi.Text);

                if (ToplamKullanici >= KullaniciSayisi)
                {
                    string kullanici_adi = gn.en_son_kaydi_getir("t_kullanicilar", "adi", $"where adi='{txt_kullanici_adi.Text}'");

                    if (string.IsNullOrEmpty(kullanici_adi))
                    {
                        KaydetKullanici();
                        KullanicilariGetir();
                        Temizle();
                        MessageBox.Show("Kullanıcı Başarı İle Eklendi");
                    }
                    else
                    {
                        MessageBox.Show("Böyle Bir Kullanıcı Bulunmamaktadır");
                    }
                }
                else
                {
                    MessageBox.Show("Maximum Kullanıcı Sayısına Ulaştınız!" + Environment.NewLine + "Lütfen Yeni Kullanıcı Satın Alınız", "Hata");
                }
            }

            if (KullaniciEklePNL.Text == "Kullanıcı DÜZENLEME İşlemleri")
            {
                KaydetKullanici();
                KullanicilariGetir();
                Temizle();
                MessageBox.Show("Kullanıcı Başarı İle Güncellendi");
            }
        }

        private void KaydetKullanici()
        {
            List<string> TabloAdlari = new List<string> { "adi", "sifre", "telefon", "tur", "bayi", "tramer_ka", "tramer_sifre" };
            ArrayList veriler = new ArrayList { txt_kullanici_adi.Text, txt_sifre.Text, txt_telefon.Text, cmb_tur.SelectedValue, cmb_sube.SelectedValue, txt_tramer_ka.Text, txt_tramer_sifre.Text };

            if (cmb_tur.SelectedValue.ToString() == "1")
            {
                TabloAdlari.Add("yetki");
                veriler.Add("1");
            }

            gn.db_kaydet(TabloAdlari, "t_kullanicilar", veriler, "id", LblKullaniciID.Text);
        }

        public void Temizle()
        {
            txt_kullanici_adi.Text = "";
            txt_telefon.Text = "";
            txt_sifre.Text = "";
            txt_tramer_ka.Text = "";
            txt_tramer_sifre.Text = "";

            KullaniciEklePNL.Text = "Kullanıcı EKLEME İşlemleri";
            Btn_KullaniciKaydet.Text = "Kullanıcı Kaydını Yap";
            Btn_KullaniciKaydet.Location = BtnKullaniciSil.Location;
            Btn_KullaniciKaydet.Width = KullaniciEklePNL.Width - 13;
            Btn_KullaniciKaydet.Enabled = false;
        }

        private void KullaniciYonetimi_Paint(object sender, PaintEventArgs e)
        {
            Temizle();
        }

        private void dgw_sirket_listesi_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GwKullanicilar.CurrentRow != null && dgw_sirket_listesi.CurrentRow != null)
            {
                sirketlere_ekle(LblKullaniciID.Text, dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString());
                KullaniciSirketGetir(LblKullaniciID.Text);
                Temizle();
            }
        }

        private void txt_kullanici_adi_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_kullanici_adi.Text)
                && !string.IsNullOrEmpty(txt_telefon.Text)
                && !string.IsNullOrEmpty(txt_sifre.Text)
                && !string.IsNullOrEmpty(txt_tramer_ka.Text)
                && !string.IsNullOrEmpty(txt_tramer_sifre.Text))
            {
                Btn_KullaniciKaydet.Enabled = true;
            }
            else
            {
                Btn_KullaniciKaydet.Enabled = false;
            }
        }

        private void listeyiGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SigortaSirketleriGetir();
        }

        public void Dispose()
        {
            gn?.Dispose();
            ssfr?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
