using Planor.Kalaslar;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class Ayarlar : UserControl
    {
        General gn = new General();
        public Ayarlar()
        {
            InitializeComponent();


        }

        [Obsolete]
        private void Ayarlar_Load(object sender, EventArgs e)
        {

            if (ConfigurationSettings.AppSettings.Get("AyniPencere") == "1") ayar1.Checked = true;
            else ayar1.Checked = false;

            if (ConfigurationSettings.AppSettings.Get("SagTuslaFarkliPencere") == "1") ayar2.Checked = true;
            else ayar2.Checked = false;

            if (ConfigurationSettings.AppSettings.Get("Motitor2de") == "1") ayar3.Checked = true;
            else ayar3.Checked = false;

            AyarKaydetBTN.Enabled = false;

            //yeniolcekleme();

            TramerGetir();

            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().PanelSlider.Height;

            SistemForm fc = (SistemForm)Application.OpenForms["SistemForm"];
            //MessageBox.Show(fc.lbl_id.Text);
            ID_Label.Text = fc.lbl_id.Text;
        }





        bool sifredehata = false;
        string kullaniciID = "";

        private void TramerGetir()
        {
            MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select * from t_kullanicilar where id = " + kullaniciID + " ", con);

            try
            {
                con.Open();
                MySqlDataReader oku = com.ExecuteReader();
                while (oku.Read())
                {
                    TxtTramerKullaniciAdi.Text = oku["tramer_ka"].ToString();
                    TxtTramerSifre.Text = oku["tramer_sifre"].ToString();
                }

            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
            }
            finally
            {
                con.Close();
            }
        }


        private void kontrolet(string sifre)
        {
            int buyukkarakter = 0;
            int kuyukkarakter = 0;
            int ozelkarakter = 0;

            string karaterler = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZXWQ";
            string karaterler2 = "abcçdefgğıijklmnoöprsştuüvyzwq";
            string karaterler3 = "'!#%&/?*\\1234567890";


            foreach (var item in karaterler)
            {
                if (sifre.Contains(item.ToString()))
                {
                    kuyukkarakter++;
                }
            }

            foreach (var item in karaterler2)
            {
                if (sifre.Contains(item.ToString()))
                {
                    buyukkarakter++;
                }
            }

            foreach (var item in karaterler3)
            {
                if (sifre.Contains(item.ToString()))
                {
                    ozelkarakter++;
                }
            }

            if (sifre.Length < 8 || buyukkarakter == 0 || kuyukkarakter == 0 || ozelkarakter == 0)
            {
                sifredehata = true;
            }
            else
            {
                sifredehata = false;
            }

        }


        private void ProgramSifresiDegistirBTN_Click(object sender, EventArgs e)
        {
            if (txt_mevcut_sifre.Text == "")
            {
                MessageBox.Show("Lütfen Eski Şifrenizi Giriniz");
            }
            else
            {
                if (txt_yeni_sifre.Text == txt_yeni_sifre_2.Text)
                {
                    string sifre = gn.en_son_kaydi_getir("t_kullanicilar", "sifre", "where id='" + ID_Label.Text + "'");
                    if (sifre == txt_mevcut_sifre.Text)
                    {
                        if (txt_mevcut_sifre.Text != txt_yeni_sifre.Text)
                        {
                            kontrolet(txt_yeni_sifre.Text);
                            if (sifredehata == false)
                            {
                                List<string> TabloAdlari = new List<string>();
                                TabloAdlari.Add("sifre");
                                ArrayList veriler = new ArrayList();
                                veriler.Add(txt_yeni_sifre.Text);

                                string sonuc = gn.db_duzenle(TabloAdlari, "t_kullanicilar", veriler, "id", ID_Label.Text);

                                if (sonuc == "islem_tamam")
                                {
                                    List<string> TabloAdlari2 = new List<string>();
                                    TabloAdlari2.Add("KullaniciID");
                                    TabloAdlari2.Add("SonDegistirmeTarihi");
                                    ArrayList veriler2 = new ArrayList();
                                    veriler2.Add(ID_Label.Text);
                                    veriler2.Add(DateTime.Now.ToString());
                                    string sonuc2 = gn.db_kaydet(TabloAdlari2, "t_kullanici_sifre", veriler2);
                                    MessageBox.Show("Şifreniz Başarı İle Değiştirildi.");
                                    txt_mevcut_sifre.Text = "";
                                    txt_yeni_sifre.Text = "";
                                    txt_yeni_sifre_2.Text = "";
                                }
                                else
                                {
                                    MessageBox.Show("Bir Hata Oluştu");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Şifreniz Gerekli Koşulları Karşılamıyor \n 1 Büyük Harf \n 1 Küçük Harf \n 1 Özel Karakter Gerekli");
                                txt_yeni_sifre.Text = "";
                                txt_yeni_sifre_2.Text = "";
                                txt_yeni_sifre.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Yeni Şifreniz Son Şifreniz İle Aynı Olmaz");
                            txt_yeni_sifre.Text = "";
                            txt_yeni_sifre_2.Text = "";
                            txt_yeni_sifre.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Şifreniz yanlış");
                    }
                }
                else
                {
                    MessageBox.Show("Girmiş Olduğunuz Şifreler Uyuşmamaktadır.");
                }
            }

        }




        private void yeniolcekleme()
        {
            float firstWidth = 1230;
            float firstHeight = 640;

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


        private void TramerSifresiDegistirBTN_Click(object sender, EventArgs e)
        {
            if (txt_yeni_sifre.Text == txt_yeni_sifre_2.Text)
            {
                List<string> TabloAdlari = new List<string>();
                TabloAdlari.Add("tramer_sifre");
                ArrayList veriler = new ArrayList();
                veriler.Add(txt_yeni_sifre.Text);
                string sonuc = gn.db_duzenle(TabloAdlari, "t_kullanicilar", veriler, "id", ID_Label.Text);

                if (sonuc == "islem_tamam")
                {
                    MessageBox.Show("İşlem Tamamlandı");
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("Bir Hata Oluştu");
                }

            }
            else
            {
                MessageBox.Show("Girmiş Olduğunuz Şifreler Uyuşmamaktadır.");
            }
        }

        private void ID_Label_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Sonunda değişti sanırım: " + ID_Label.Text);
            kullaniciID = ID_Label.Text;
        }


        private void ayarlar_CheckedChanged(object sender, EventArgs e)
        {
            AyarKaydetBTN.Enabled = true;
        }
        private void AyarKaydetBTN_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            config.AppSettings.Settings.Remove("AyniPencere");
            config.AppSettings.Settings.Remove("SagTuslaFarkliPencere");
            config.AppSettings.Settings.Remove("Motitor2de");

            if (ayar1.Checked) config.AppSettings.Settings.Add("AyniPencere", "1");
            else config.AppSettings.Settings.Add("AyniPencere", "0");

            if (ayar2.Checked) config.AppSettings.Settings.Add("SagTuslaFarkliPencere", "1");
            else config.AppSettings.Settings.Add("SagTuslaFarkliPencere", "0");

            if (ayar3.Checked) config.AppSettings.Settings.Add("Motitor2de", "1");
            else config.AppSettings.Settings.Add("Motitor2de", "0");

            config.Save(ConfigurationSaveMode.Modified);


            AyarKaydetBTN.Enabled = false;
        }

        private void gunaLabel2_Click(object sender, EventArgs e)
        {

        }

        private void ayar1_CheckedChanged(object sender, EventArgs e)
        {
            AyarKaydetBTN.Enabled = true;

            try
            {
                if (ayar1.Checked == false) ayar2.Checked = false;
            }
            catch
            {
                //throw
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void gunaLabel10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void ayar2_CheckedChanged(object sender, EventArgs e)
        {
            AyarKaydetBTN.Enabled = true;
        }

        private void ayar1_Click(object sender, EventArgs e)
        {

        }

        private void txt_mevcut_sifre_TextChanged(object sender, EventArgs e)
        {
            if (txt_mevcut_sifre.Text != "" && txt_yeni_sifre.Text != "" && txt_yeni_sifre_2.Text != "")
            {
                ProgramSifresiDegistirBTN.Enabled = true;
            }
            else ProgramSifresiDegistirBTN.Enabled = false;
        }

        private void Ayarlar_Enter(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SistemForm fc = (SistemForm)Application. OpenForms["SistemForm"];
            //MessageBox.Show(fc.lbl_id.Text);
        }

        private void TxtTramerKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            if (TxtTramerKullaniciAdi.Text != "" && TxtTramerSifre.Text != "" && txt_yeni_sifre_t.Text != "" && txt_yeni_sifre_2_t.Text != "")
            {
                TramerSifresiDegistirBTN.Enabled = true;
            }
            else TramerSifresiDegistirBTN.Enabled = false;
        }
    }
}
