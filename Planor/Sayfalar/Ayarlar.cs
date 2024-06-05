using Planor.Kalaslar; // Namespace for custom classes
using MySql.Data.MySqlClient; // Namespace for MySQL database connection
using System; // Namespace for basic system functionality
using System.Collections.Generic; // Namespace for generic collections
using System.Configuration; // Namespace for configuration management
using System.Drawing; // Namespace for graphics functionality
using System.Windows.Forms; // Namespace for Windows Forms functionality

namespace Planor.Sayfalar // Namespace for the forms and controls
{
    public partial class Ayarlar : UserControl // Ayarlar UserControl
    {
        General gn = new General(); // Create an instance of the General class

        // Constants for connection string, SQL queries, and form size
        private const string CONNECTION_STRING = "YourConnectionString";
        private const string SELECT_QUERY = @"Select * from t_kullanicilar where id = @id";
        private const string UPDATE_QUERY = "UPDATE t_kullanicilar SET {0} WHERE id = @id";
        private const string INSERT_QUERY = "INSERT INTO t_kullanici_sifre (KullaniciID, SonDegistirmeTarihi) VALUES (@id, @tarih)";
        private const string DELETE_QUERY = "DELETE FROM t_kullanici_sifre WHERE KullaniciID = @id";

        // Ayarlar UserControl constructor
        public Ayarlar()
        {
            InitializeComponent();

            // Add event handler for SizeChanged event
            SizeChanged += Ayarlar_SizeChanged;

            // Initialize UI elements and load settings
            TramerSifresiDegistirBTN.Enabled = false;
            TramerGetir();
            ID_Label.Text = GetIDFromParentForm();
            SetFormSize();
        }

        // Get the ID from the parent form
        private string GetIDFromParentForm()
        {
            SistemForm fc = (SistemForm)Application.OpenForms["SistemForm"];
            return fc.lbl_id.Text;
        }

        // Set the form size based on the screen size
        private void SetFormSize()
        {
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().PanelSlider.Height;
        }

        // Load settings from the configuration file
        private void LoadSettings()
        {
            ayar1.Checked = ConfigurationManager.AppSettings.Get("AyniPencere") == "1";
            ayar2.Checked = ConfigurationManager.AppSettings.Get("SagTuslaFarkliPencere") == "1";
            ayar3.Checked = ConfigurationManager.AppSettings.Get("Motitor2de") == "1";
        }

        // Save settings to the configuration file
        private void SaveSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            // Remove existing settings and add new ones
            config.AppSettings.Settings.Remove("AyniPencere");
            config.AppSettings.Settings.Remove("SagTuslaFarkliPencere");
            config.AppSettings.Settings.Remove("Motitor2de");

            config.AppSettings.Settings.Add("AyniPencere", ayar1.Checked ? "1" : "0");
            config.AppSettings.Settings.Add("SagTuslaFarkliPencere", ayar2.Checked ? "1" : "0");
            config.AppSettings.Settings.Add("Motitor2de", ayar3.Checked ? "1" : "0");

            config.Save(ConfigurationSaveMode.Modified);
        }

        // Retrieve tramer information from the database
        private void TramerGetir()
        {
            // Open a connection, create a command, and execute the query
            using (MySqlConnection con = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand com = new MySqlCommand(SELECT_QUERY, con);
                com.Parameters.AddWithValue("@id", ID_Label.Text);

                try
                {
                    con.Open();
                    MySqlDataReader oku = com.ExecuteReader();
                    if (oku.Read())
                    {
                        // Set the textboxes with the retrieved data
                        TxtTramerKullaniciAdi.Text = oku["tramer_ka"].ToString();
                        TxtTramerSifre.Text = oku["tramer_sifre"].ToString();
                    }
                }
                catch (Exception exp)
                {
                    // Display the error message in a message box
                    MessageBox.Show(exp.Message);
                }
            }
        }

        // Validate the password
        private bool ValidatePassword(string password)
        {
            int buyukkarakter = 0;
            int kuyukkarakter = 0;
            int ozelkarakter = 0;

            string karaterler = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZXWQ";
            string karaterler2 = "abcçdefgğıijklmnoöprsştuüvyzwq";
            string karaterler3 = "'!#%&/?*\\1234567890";

            // Check if the password contains uppercase, lowercase, and special characters
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

            // Return true if the password meets the requirements
            return password.Length >= 8 && buyukkarakter > 0 && kuyukkarakter > 0 && ozelkarakter > 0;
        }

        // Change the program password
        private void ProgramSifresiDegistirBTN_Click(object sender, EventArgs e)
        {
            // Validate the input
            if (string.IsNullOrEmpty(txt_mevcut_sifre.Text))
            {
                MessageBox.Show("Lütfen Eski Şifrenizi Giriniz");
                return;
            }

            if (txt_yeni_sifre.Text != txt_yeni_sifre_2.Text)
            {
                MessageBox.Show("Girmiş Olduğunuz Şifreler Uyuşmamaktadır.");
                return;
            }

            string sifre = gn.en_son_kaydi_getir("t_kullanicilar", "sifre", $"where id='{ID_Label.Text}'");
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

            // Update the program password in the database
            string updateQuery = string.Format(UPDATE_QUERY, "sifre='" + txt_yeni_sifre.Text + "'");
            List<string> TabloAdlari = new List<string>();
            TabloAdlari.Add("sifre");
            ArrayList veriler = new ArrayList();
            veriler.Add(txt_yeni_sifre.Text);

            string sonuc = gn.db_duzenle(TabloAdlari, "t_kullanicilar", veriler, "id", ID_Label.Text);

            if (sonuc == "islem_tamam")
            {
                // Insert a new record in the t_kullanici_sifre table
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

        // Change the tramer password
        private void TramerSifresiDegistirBTN_Click(object sender, EventArgs e)
        {
            // Validate the input
            if (txt_yeni_sifre.Text == txt_yeni_sifre_2.Text)
            {
                List<string> TabloAdlari = new List<string>();
                TabloAdlari.Add("tramer_sifre");
                ArrayList veriler = new ArrayList();
                veriler.Add(txt_yeni_sifre.Text);

                // Update the tramer password in the database
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

        // Event handler for the CheckedChanged event of the radio buttons
        private void ayarlar_CheckedChanged(object sender, EventArgs e)
        {
            AyarKaydetBTN.Enabled = true;
        }

        // Save settings to the configuration file
        private void AyarKaydetBTN_Click(object sender, EventArgs e)
        {
            SaveSettings();
            AyarKaydetBTN.Enabled = false;
        }

        // Event handler for the TextChanged event of the tramer username textbox
        private void TxtTramerKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            // Enable the TramerSifresiDegistirBTN if all input fields are filled
            TramerSifresiDegistirBTN.Enabled = TxtTramerKullaniciAdi.Text.Length > 0 &&
                                               TxtTramerSifre.Text.Length > 0 &&
                                               txt_yeni_sifre_t.Text.Length > 0 &&
                                               txt_yeni_sifre_2_t.Text.Length > 0;
        }
    }
}
