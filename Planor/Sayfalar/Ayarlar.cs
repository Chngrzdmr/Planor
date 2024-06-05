using Planor.Kalaslar;
using MySql.Data.MySqlClient;
using System;
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
            SizeChanged += Ayarlar_SizeChanged; // Add event handler for SizeChanged event
            TramerSifresiDegistirBTN.Enabled = false;
            TramerGetir();
            ID_Label.Text = GetIDFromParentForm();
            SetFormSize();
        }

        private string GetIDFromParentForm()
        {
            SistemForm fc = (SistemForm)Application.OpenForms["SistemForm"];
            return fc.lbl_id.Text;
        }

        private void Ayarlar_SizeChanged(object sender, EventArgs e)
        {
            SetFormSize();
        }

        private void SetFormSize()
        {
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().PanelSlider.Height;
        }

        private void Ayarlar_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (ConfigurationManager.AppSettings.Get("AyniPencere") == "1") ayar1.Checked = true;
            if (ConfigurationManager.AppSettings.Get("SagTuslaFarkliPencere") == "1") ayar2.Checked = true;
            if (ConfigurationManager.AppSettings.Get("Motitor2de") == "1") ayar3.Checked = true;
        }

        private void SaveSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            config.AppSettings.Settings.Remove("AyniPencere");
            config.AppSettings.Settings.Remove("SagTuslaFarkliPencere");
            config.AppSettings.Settings.Remove("Motitor2de");

            config.AppSettings.Settings.Add("AyniPencere", ayar1.Checked ? "1" : "0");
            config.AppSettings.Settings.Add("SagTuslaFarkliPencere", ayar2.Checked ? "1" : "0");
            config.AppSettings.Settings.Add("Motitor2de", ayar3.Checked ? "1" : "0");

            config.Save(ConfigurationSaveMode.Modified);
        }

        private void TramerGetir()
        {
            using (MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand(@"Select * from t_kullanicilar where id = @id", con);
                com.Parameters.AddWithValue("@id", ID_Label.Text);

                try
                {
                    con.Open();
                    MySqlDataReader oku = com.ExecuteReader();
                    if (oku.Read())
                    {
                        TxtTramerKullaniciAdi.Text = oku["tramer_ka"].ToString();
                        TxtTramerSifre.Text = oku["tramer_sifre"].ToString();
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }

        private bool ValidatePassword(string password)
        {
            int buyukkarakter = 0;
            int kuyukkarakter = 0;
            int ozelkarakter = 0;

            string karaterler = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZXWQ";
            string karaterler2 = "abcçdefgğıijklmnoöprsştuüvyzwq";
            string karaterler3 = "'!#%&/?*\\1234567890";

            foreach (var item in karaterler)
            {
                if (password.Contains(item.ToString()))
                {
                    kuyukkarakter++;
                }
            }

            foreach (var item in karaterler2)
            {
                if (password.Contains(item.ToString()))
                {
                    buyukkarakter++;
                }
            }

            foreach (var item in karaterler3)
            {
                if (password.Contains(item.ToString()))
                {
                    ozelkarakter++;
                }
            }

            return password.Length >= 8 && buyukkarakter > 0 && kuyukkarakter > 0 && ozelkarakter > 0;
        }

        private void ProgramSifresiDegistirBTN_Click(object sender, EventArgs e)
        {
            if (txt_mevcut_sifre.Text == "")
            {
                MessageBox.Show("Lütfen Eski Şifrenizi Giriniz");
                return;
            }

            if (txt_yeni_sifre.Text != txt_yeni_sifre_2.Text)
            {
                MessageBox.Show("Girmiş Olduğunuz Şifreler Uyuşmamaktadır.");
                return;
            }

            string sifre = gn.en_son_kaydi_getir("t_kullanicilar", "sifre", "where id='" + ID_Label.Text + "'");
            if (sifre != txt_mevcut_sifre.Text)
            {
                MessageBox.Show("Şifreniz yanlış");
                return;
            }

            if (!ValidatePassword(txt_yeni_sifre.Text))
            {
                MessageBox.Show("Şifreniz Gerekli Koşulları Karşılamıyor \n 1 Büyük Harf \n 1 Küçük Harf \n 1 Özel Karakter Gerekli");
                return;
            }

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
                gn.db_kaydet(TabloAdlari2, "t_kullanici_sifre", veriler2);

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

        private void ayarlar_CheckedChanged(object sender, EventArgs e)
        {
            AyarKaydetBTN.Enabled = true;
        }

        private void AyarKaydetBTN_Click(object sender, EventArgs e)
        {
            SaveSettings();
            AyarKaydetBTN.Enabled = false;
        }

        private void TxtTramerKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            TramerSifresiDegistirBTN.Enabled = TxtTramerKullaniciAdi.Text.Length > 0 &&
                                               TxtTramerSifre.Text.Length > 0 &&
                                               txt_yeni_sifre_t.Text.Length > 0 &&
                                               txt_yeni_sifre_2_t.Text.Length > 0;
        }
    }
}
