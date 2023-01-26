using Planor.Kalaslar;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class KullaniciYonetimi : UserControl
    {
        General gn = new General();
        SistemForm ssfr = new SistemForm();
        public KullaniciYonetimi()
        {
            InitializeComponent();

            //ekrana göre ölçüleri ayarlamak için
        }

        private void KullaniciYonetimi_Load(object sender, EventArgs e)
        {
            //ekrana göre ölçüleri ayarlamak için
            this.Width = ssfr.screens[ssfr.ekranno].WorkingArea.Width - ((ssfr.screens[ssfr.ekranno].WorkingArea.Width) / 8);
            this.Height = ssfr.screens[ssfr.ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height;

            KullanicilariGetir();
            SigortaSirketleriGetir();
            SubeGetir();
            YetkiGetir();
            LblMaxSayi.Text = "999";
            LblSuankiSayi.Text = gn.adet_getir("t_kullanicilar", "id", "").ToString();


            //yeniolcekleme();
        }


        public void KullanicilariGetir()
        {
            GwKullanicilar.AllowUserToAddRows = false;
            GwKullanicilar.AutoGenerateColumns = true;
            //GwKullanicilar.TableElement.BeginUpdate();

            gn.grid_view_getir(" t_kullanicilar.id AS ID, t_kullanicilar.adi AS KullaniciAdi, t_bayiler.adi AS Bayi_Adi FROM t_kullanicilar INNER JOIN t_bayiler ON t_bayiler.id = t_kullanicilar.bayi order by t_kullanicilar.adi asc ", GwKullanicilar);
            GwKullanicilar.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            GwKullanicilar.Columns["ID"].HeaderText = "Sıra No";
            GwKullanicilar.Columns["KullaniciAdi"].HeaderText = "Kullanıcı Adı";
            GwKullanicilar.Columns["Bayi_Adi"].HeaderText = "Bayi Adı";
            //GwKullanicilar.TableElement.EndUpdate();


            kullanici_sayilarini_getir();
        }


        private void kullanici_sayilarini_getir()
        {
            // string kullaniciadi = gn.KulllaniciLimitAdi;
            lbl_aktif_sayi.Text = gn.adet_getir("t_kullanicilar", "id", "").ToString();
            lbl_max_sayi.Text = "999";
        }


        private void SigortaSirketleriGetir()
        {
            dgw_sirket_listesi.AllowUserToAddRows = false;
            dgw_sirket_listesi.AutoGenerateColumns = true;
            //this.RgSirketler.TableElement.BeginUpdate();

            gn.grid_view_getir(" id,adi from t_sirketler order by adi asc", dgw_sirket_listesi);
            dgw_sirket_listesi.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            dgw_sirket_listesi.Columns[0].Width = 50;


            dgw_sirket_listesi.Columns["id"].HeaderText = "Sıra No";
            dgw_sirket_listesi.Columns["adi"].HeaderText = "Şirket Adı";
            //dgw_sirket_listesi.TableElement.EndUpdate();
        }

        private void BtnKullaniciSil_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Kullanıcıyı Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result.Equals(DialogResult.OK))
                {
                    if (GwKullanicilar.CurrentRow.Cells[0] != null)
                    {
                        string deger = GwKullanicilar.CurrentRow.Cells[0].Value.ToString();

                        string Sonuc1 = gn.db_sil_deger("t_kullanici_sirketler", deger, "KullaniciID");

                        string sonuc = gn.db_sil("t_kullanicilar", deger);

                        KullanicilariGetir();

                        RgKullaniciSirketler.DataSource = null;
                        RgKullaniciSirketler.Rows.Clear();

                        Temizle();
                    }
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı Silme İşlemi Yapılırken", hata);
            }
        }

        private void GwKullanicilar_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string KullaniciID = GwKullanicilar.CurrentRow.Cells[0].Value.ToString();
                KullaniciSirketGetir(KullaniciID);
                LblKullaniciID.Text = GwKullanicilar.CurrentRow.Cells[0].Value.ToString();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı Tablosuna Click Yapılırken", hata);
            }
            Temizle();
        }



        private void KullaniciSirketGetir(string KullaniciID)
        {
            RgKullaniciSirketler.AllowUserToAddRows = false;
            RgKullaniciSirketler.AutoGenerateColumns = true;
            //RgKullaniciSirketler.TableElement.BeginUpdate();

            gn.grid_view_getir(" id,Adi from t_kullanici_sirketler where KullaniciID='" + KullaniciID + "' order by Adi asc", RgKullaniciSirketler);
            RgKullaniciSirketler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);


            RgKullaniciSirketler.Columns["id"].HeaderText = "Sıra No";
            RgKullaniciSirketler.Columns["adi"].HeaderText = "Şirket Adı";
            //this.RgKullaniciSirketler.TableElement.EndUpdate();

            RgKullaniciSirketler.Columns[0].Width = 50;
        }

        private void GwKullanicilar_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string deger = GwKullanicilar.CurrentRow.Cells[0].Value.ToString();
                LblKullaniciID.Text = deger.ToString();
                IcerikGetir();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı Tablosuna Çift Click Yapılırken", hata);
            }
            KullaniciEklePNL.Text = "Kullanıcı DÜZENLEME İşlemleri";
            Btn_KullaniciKaydet.Text = "Kullanıcı Bilgileri Düzelt";
            Btn_KullaniciKaydet.Location = new Point(180, BtnKullaniciSil.Location.Y);
            Btn_KullaniciKaydet.Width = KullaniciEklePNL.Width - 180 - 6;
            Btn_KullaniciKaydet.Enabled = true;
            Btn_KullaniciKaydet.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Btn_KullaniciKaydet.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            Btn_KullaniciKaydet.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Btn_KullaniciKaydet.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
        }


        private void IcerikGetir()
        {
            string KullaniciID = LblKullaniciID.Text;

            MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select * from t_kullanicilar where id = " + KullaniciID + " ", con);

            try
            {
                con.Open();
                MySqlDataReader oku = com.ExecuteReader();
                while (oku.Read())
                {
                    txt_kullanici_adi.Text = oku["adi"].ToString();
                    txt_sifre.Text = oku["sifre"].ToString();
                    txt_telefon.Text = oku["telefon"].ToString();
                    txt_tramer_ka.Text = oku["tramer_ka"].ToString();
                    txt_tramer_sifre.Text = oku["tramer_sifre"].ToString();

                    string SubeID = oku["bayi"].ToString();
                    string KullaniciTuru = oku["tur"].ToString();

                    if (SubeID != "")
                    {
                        cmb_sube.SelectedValue = SubeID.ToString();
                    }

                    if (KullaniciTuru != "")
                    {
                        cmb_tur.SelectedValue = KullaniciTuru.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı bilgileri DB den çekilirken", hata);
            }
            finally
            {
                con.Close();
            }
        }



        private void sirketlere_ekle(string kullanici_id, string sirket_id)
        {
            string adi = gn.en_son_kaydi_getir("t_sirketler", "adi", "where id='" + sirket_id + "'");
            List<string> TabloAdlari = new List<string>();
            TabloAdlari.Add("SirketID");
            TabloAdlari.Add("KullaniciID");
            TabloAdlari.Add("Adi");

            ArrayList veriler = new ArrayList();
            veriler.Add(sirket_id);
            veriler.Add(kullanici_id);
            veriler.Add(adi);
            string sonuc = gn.db_kaydet(TabloAdlari, "t_kullanici_sirketler", veriler);
            if (sonuc == "islem_tamam")
            {

            }
            else
            {
                MessageBox.Show("Bir Hata Oluştu");
            }
        }

        private void BtnSirketEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (GwKullanicilar.CurrentRow.Cells[0].Value.ToString() != "" & dgw_sirket_listesi.CurrentRow.Cells[0].Value != null)
                {

                    sirketlere_ekle(LblKullaniciID.Text, dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString());
                    KullaniciSirketGetir(LblKullaniciID.Text);
                }
                else
                {
                    MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
                }

            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Şirket ekleme yapılırken", hata);
            }
        }

        private void BtnSirketSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (RgKullaniciSirketler.CurrentRow.Cells[0] != null)
                {
                    string deger = RgKullaniciSirketler.CurrentRow.Cells[0].Value.ToString();

                    string sonuc = gn.db_sil("t_kullanici_sirketler", deger);
                    if (sonuc == "işlem tamamlandı")
                    {
                        KullaniciSirketGetir(LblKullaniciID.Text);
                        MessageBox.Show("İşlem Tamam");
                    }
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Şirket Sil Yapılırken", hata);
            }
        }


        private void toplu_sirket_ekle(string g_sirket_id, string g_sirket_link, string g_link_adi, string g_kullanici_id)
        {
            List<string> TabloAdlari = new List<string>();
            TabloAdlari.Add("SirketID");
            TabloAdlari.Add("KullaniciID");
            TabloAdlari.Add("Adi");
            TabloAdlari.Add("Link");

            ArrayList veriler = new ArrayList();
            veriler.Add(g_sirket_id);
            veriler.Add(g_kullanici_id);
            veriler.Add(g_link_adi);
            veriler.Add(g_sirket_link);
            try
            {
                string sonuc = gn.db_kaydet(TabloAdlari, "t_kullanici_sirketler", veriler);
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Toplu Şirket Ekleme yaparken", hata);
                MessageBox.Show("Bir Hata Oluştu: " + hata);
            }
        }

        private void BtnButunKullanicilaraEkle_Click(object sender, EventArgs e)
        {
            string sirket_id = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString();
            string adi = gn.en_son_kaydi_getir("t_sirketler", "adi", "where id='" + sirket_id + "' order by adi asc");

            MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
            MySqlCommand com = new MySqlCommand("Select id from t_kullanicilar order by id asc", con);

            try
            {
                con.Open();
                MySqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    string id = dr["id"].ToString();
                    toplu_sirket_ekle(sirket_id, "", adi, id);
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bir şirket bütün kullanıcılara eklenirken", hata);
            }
            finally
            {
                con.Close();
                con.Dispose();
                MessageBox.Show("İşlem Tamamlandı");
            }
        }

        private void toplu_sirket_silme(string id)
        {
            try
            {
                string sonuc = gn.db_sil("t_kullanici_sirketler", id);
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bütün şirketler silinmesi yapılırken", hata);
                MessageBox.Show("Bir Hata Oluştu: " + hata);
            }
        }

        private void BtnButunKullanicilardanSil_Click(object sender, EventArgs e)
        {
            string sirket_id = dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString();
            MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
            MySqlCommand com = new MySqlCommand("Select id from t_kullanici_sirketler  where  SirketID='" + sirket_id + "' order by id asc", con);

            try
            {
                con.Open();
                MySqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    string id = dr["id"].ToString();
                    toplu_sirket_silme(id);
                }
                MessageBox.Show("İşlem Tamamlandı");

            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bir şirket bütün kullanıcılardan silinirken", hata);
            }
            finally
            {
                con.Close();
            }
        }

        private void BtnTumSirketleriEkle_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow lastRow1 = dgw_sirket_listesi.Rows[dgw_sirket_listesi.Rows.Count - 1];
                lastRow1.Selected = true;

                foreach (DataGridViewRow rowInfo in dgw_sirket_listesi.Rows)
                {
                    if (LblKullaniciID.Text != "" & rowInfo.Cells["id"].Value != null)
                    {
                        string sirket_id = rowInfo.Cells["id"].Value.ToString();
                        sirketlere_ekle(LblKullaniciID.Text, sirket_id);
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
                    }
                    Application.DoEvents();
                }
                KullaniciSirketGetir(LblKullaniciID.Text);
                MessageBox.Show("İşlem Tamam");
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcıya bir şirket eklenirken", hata);
                MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
            }
        }

        private void BtnTumSirketleriSil_Click(object sender, EventArgs e)
        {
            try
            {

                if (LblKullaniciID.Text != "")
                {
                    MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
                    string comString = @"Delete from t_kullanici_sirketler where KullaniciID=@KullaniciID";
                    MySqlCommand com = new MySqlCommand(comString, con);

                    com.Parameters.Add(new MySqlParameter("@KullaniciID", LblKullaniciID.Text));

                    try
                    {
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string hata = ex.ToString();
                        gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bütün şirketleri silmek için DB ye bağlantı oluşturulurken", hata);
                    }
                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                            con.Dispose();
                        }
                        KullaniciSirketGetir(LblKullaniciID.Text);
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Bütün şirketleri silmek için işlemler yapılırken", hata);
                MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
            }
        }

        private void SubeGetir()
        {
            gn.combo_box_veri_getir(cmb_sube, " * from t_bayiler order by adi asc", "adi", "id");

        }

        private void YetkiGetir()
        {
            gn.combo_box_veri_getir(cmb_tur, " * from t_tur", "adi", "id");

        }

        private void Btn_KullaniciKaydet_Click(object sender, EventArgs e)
        {
            if (KullaniciEklePNL.Text == "Kullanıcı EKLEME İşlemleri")
            {
                int ToplamKullanici = Convert.ToInt16(LblMaxSayi.Text);

                int KullaniciSayisi = Convert.ToInt16(LblSuankiSayi.Text);

                if (ToplamKullanici >= KullaniciSayisi)
                {
                    string kullanici_adi = gn.en_son_kaydi_getir("t_kullanicilar", "adi", "where adi='" + txt_kullanici_adi.Text + "'");
                    string KullaniciTuru = cmb_tur.SelectedValue.ToString();

                    if (kullanici_adi == "")
                    {
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("adi");
                        TabloAdlari.Add("sifre");
                        TabloAdlari.Add("telefon");
                        TabloAdlari.Add("tur");
                        TabloAdlari.Add("bayi");
                        TabloAdlari.Add("tramer_ka");
                        TabloAdlari.Add("tramer_sifre");


                        ArrayList veriler = new ArrayList();
                        veriler.Add(txt_kullanici_adi.Text);
                        veriler.Add(txt_sifre.Text);
                        veriler.Add(txt_telefon.Text);
                        veriler.Add(cmb_tur.SelectedValue.ToString());
                        veriler.Add(cmb_sube.SelectedValue.ToString());
                        veriler.Add(txt_tramer_ka.Text);
                        veriler.Add(txt_tramer_sifre.Text);


                        if (KullaniciTuru == "1")
                        {
                            TabloAdlari.Add("yetki");
                            veriler.Add("1");
                        }

                        string sonuc = gn.db_kaydet(TabloAdlari, "t_kullanicilar", veriler);
                        if (sonuc == "islem_tamam")
                        {

                            KullanicilariGetir();
                            LblMaxSayi.Text = lbl_max_sayi.Text;
                            LblSuankiSayi.Text = lbl_aktif_sayi.Text;

                            Temizle();

                            MessageBox.Show("Kullanıcı Başarı İle Eklendi");

                        }
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
                string KullaniciID = LblKullaniciID.Text;
                string KullaniciTuru = cmb_tur.SelectedValue.ToString();

                List<string> TabloAdlari = new List<string>();
                TabloAdlari.Add("adi");
                TabloAdlari.Add("sifre");
                TabloAdlari.Add("telefon");
                TabloAdlari.Add("tur");
                TabloAdlari.Add("bayi");
                TabloAdlari.Add("tramer_ka");
                TabloAdlari.Add("tramer_sifre");


                ArrayList veriler = new ArrayList();
                veriler.Add(txt_kullanici_adi.Text);
                veriler.Add(txt_sifre.Text);
                veriler.Add(txt_telefon.Text);
                veriler.Add(cmb_tur.SelectedValue.ToString());
                veriler.Add(cmb_sube.SelectedValue.ToString());
                veriler.Add(txt_tramer_ka.Text);
                veriler.Add(txt_tramer_sifre.Text);

                if (KullaniciTuru == "1")
                {
                    TabloAdlari.Add("yetki");
                    veriler.Add("1");
                }

                string sonuc = gn.db_duzenle(TabloAdlari, "t_kullanicilar", veriler, "id", KullaniciID);
                if (sonuc == "islem_tamam")
                {
                    try
                    {
                        KullanicilariGetir();
                        Temizle();
                        MessageBox.Show("Kullanıcı Başarı İle Güncellendi");
                    }
                    catch (Exception ex)
                    {
                        string hata = ex.ToString();
                        gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Kullanıcı Bilgileri Düzenleme yapılırken", hata);
                    }
                }
            }



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
            Btn_KullaniciKaydet.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            Btn_KullaniciKaydet.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
            Btn_KullaniciKaydet.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            Btn_KullaniciKaydet.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));

        }

        private void KullaniciYonetimi_Paint(object sender, PaintEventArgs e)
        {
            Temizle();
        }


        private void yeniolcekleme()
        {
            float firstWidth = 1240;
            float firstHeight = 700;

            float size1 = this.Size.Width / firstWidth;
            float size2 = this.Size.Height / firstHeight;

            SizeF scale = new SizeF(size1, size2);
            firstWidth = this.Size.Width;
            firstHeight = this.Size.Height;

            foreach (Control control in this.Controls)
            {

                control.Font = new Font(control.Font.FontFamily, control.Font.Size * ((size1 + size2) / 2));

                control.Scale(scale);


            }
        }

        private void dgw_sirket_listesi_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GwKullanicilar.CurrentRow.Cells[1].Value.ToString() != "" && GwKullanicilar.CurrentRow.Cells[1].Value.ToString() == txt_kullanici_adi.Text)
            {
                try
                {
                    if (GwKullanicilar.CurrentRow.Cells[0].Value.ToString() != "" & dgw_sirket_listesi.CurrentRow.Cells[0].Value != null)
                    {

                        sirketlere_ekle(LblKullaniciID.Text, dgw_sirket_listesi.CurrentRow.Cells[0].Value.ToString());
                        KullaniciSirketGetir(LblKullaniciID.Text);
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı veya Şirket Seçiniz");
                    }

                }
                catch (Exception ex)
                {
                    string hata = ex.ToString();
                    gn.LocalLoglaAsync(ssfr.isimLBL.Text, "Tanımlı Şirketler Tablosuna çift Click yaptıktan sonra Kullanıcı Adı Boş değilse şirketi kullanıcıya eklerken", hata);
                }
            }
        }

        private void txt_kullanici_adi_TextChanged(object sender, EventArgs e)
        {
            if (txt_kullanici_adi.Text != ""
             && txt_telefon.Text != ""
             && txt_sifre.Text != ""
             && txt_tramer_ka.Text != ""
             && txt_tramer_sifre.Text != "")
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
    }
}
