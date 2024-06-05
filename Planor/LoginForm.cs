using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Planor.Kalaslar;
using Planor.Sayfalar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Planor
{
    public partial class LoginForm : Form
    {
        private string ipAddress = "";
        private General general = new General();
        private MySqlConnection connection;
        private string username;

        public static string UserID { get; private set; }

        public LoginForm()
        {
            InitializeComponent();

            new Guna2DragControl(g2LoginImage);
            LBL_IP.Text = GetIPAddress();

            LoadSettings();
            LoadVersion();

            if (IsLoggedIn())
            {
                ShowSistemForm();
                Close();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (txt_username.Text != "") this.ActiveControl = txt_password;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (!ValidateFormFields()) return;

            VerifyVersion();

            ipAddress = GetIPAddress();

            using (connection = new MySqlConnection(general.MySqlConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("Select * from t_users where adi=@username", connection))
                {
                    command.Parameters.AddWithValue("@username", txt_username.Text);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader["adi"].ToString() == txt_username.Text && reader["sifre"].ToString() == txt_password.Text)
                                {
                                    UserID = reader["id"].ToString();
                                    username = reader["adi"].ToString();
                                    LogIn(UserID, ipAddress, txt_password.Text, txt_username.Text);

                                    ShowSistemForm();
                                    Hide();
                                }
                                else
                                {
                                    LogFailedLoginAttempt(UserID, ipAddress, txt_password.Text, txt_username.Text);
                                    MessageBox.Show("Invalid username or password");
                                }
                            }
                        }
                        else
                        {
                            LogFailedLoginAttempt(UserID, ipAddress, txt_password.Text, txt_username.Text);
                            MessageBox.Show("User not found");
                        }
                    }
                }
            }
        }

        private string GetIPAddress()
        {
            string externalIP = "";

            try
            {
                externalIP = new WebClient().DownloadString("https://api.ipify.org/");
                externalIP = externalIP.Replace("\n", "");
            }
            catch
            {
                externalIP = new WebClient().DownloadString("http://icanhazip.com");
                externalIP = externalIP.Replace("\n", "");
            }

            return externalIP;
        }

        private void LogIn(string userID, string ipAddress, string password, string username)
        {
            List<string> columnNames = new List<string>
            {
                "UserID",
                "BayiID",
                "Saat",
                "Gun",
                "Ay",
                "Yil",
                "PcAdi",
                "GirmeDurumu",
                "ip",
                "KullaniciAdi",
                "Sifre",
                "tur"
            };

            ArrayList values = new ArrayList
            {
                userID,
                general.GetLastRecord("t_bayiler", "id", $"where ip_durum=1 and ip='{ipAddress}'"),
                DateTime.Now.ToString(),
                DateTime.Now.Day.ToString(),
                DateTime.Now.Month.ToString(),
                DateTime.Now.Year.ToString(),
                Environment.MachineName,
                "Giriş Başarısız",
                ipAddress,
                username,
                password,
                "Giriş"
            };

            general.InsertData(columnNames, "t_logkayitlari", values);
        }

        private void LogFailedLoginAttempt(string userID, string ipAddress, string password, string username)
        {
            List<string> columnNames = new List<string>
            {
                "UserID",
                "BayiID",
                "Saat",
                "Gun",
                "Ay",
                "Yil",
                "PcAdi",
                "GirmeDurumu",
                "ip",
                "KullaniciAdi",
                "Sifre",
                "tur"
            };

            ArrayList values = new ArrayList
            {
                userID,
                null,
                DateTime.Now.ToString(),
                DateTime.Now.Day.ToString(),
                DateTime.Now.Month.ToString(),
                DateTime.Now.Year.ToString(),
                Environment.MachineName,
                "Giriş Başarısız",
                ipAddress,
                username,
                password,
                "Giriş"
            };

            general.InsertData(columnNames, "t_logkayitlari", values);
        }

        private void ClearFormFields()
        {
            txt_password.Clear();
            txt_username.Clear();
            txt_username.Focus();
        }

        private void LoadSettings()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Username))
            {
                txt_username.Text = Properties.Settings.Default.Username;
            }
        }

        private void LoadVersion()
        {
            var myVersion = System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed
                ? System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion
                : System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            LBL_VRSYN.Text = myVersion.ToString();
        }

        private bool ValidateFormFields()
        {
            if (string.IsNullOrEmpty(txt_username.Text) || string.IsNullOrEmpty(txt_password.Text))
            {
                MessageBox.Show("Please enter username and password");
                return false;
            }

            return true;
        }

        private bool IsLoggedIn()
        {
            if (UserID != "") return true;

            return false;
        }

        private void ShowSistemForm()
        {
            var sistemForm = new SistemForm
            {
                lbl_id = { Text = UserID },
                lbl_tur = { Text = general.GetLastRecord("t_users", "tur", $"where id='{UserID}'") },
                lbl_tramer_adi = { Text = general.GetLastRecord("t_users", "tramer_adi", $"where id='{UserID}'") },
                lbl_tramer_sifre = { Text = general.GetLastRecord("t_users", "tramer_sifre", $"where id='{UserID}'") },
                lbl_ip = { Text = ipAddress },
                isimLBL = { Text = general.GetLastRecord("t_users", "adisoyadi", $"where id='{UserID}'") }
            };

            general._kullanici_id = UserID;
            Hide();
            sistemForm.Show();
        }
    }
}


<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="mysql" type="MySql.Data.MySqlClient.MySqlConnectionStringBuilder, MySql.Data" />
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
  </startup>
  <connectionStrings>
    <add name="MySqlConnection" connectionString="server=localhost;database=planor;uid=root;pwd=password;" providerName="MySql.Data.MySqlClient" />
  </connectionStrings>
  <appSettings>
    <add key="Username" value="" />
  </appSettings>
</configuration>
