using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Planor.Kalaslar;
using Planor.Sayfalar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Planor
{
    public partial class LoginForm : Form
    {
        string ipAddress = "";
        string username;
        General general = new General();

        public static string UserID = "";

        public LoginForm()
        {
            InitializeComponent();

            new Guna2DragControl(g2LoginImage);
            LBL_IP.Text = GetIPAddress();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Username))
            {
                txt_username.Text = Properties.Settings.Default.Username;
            }

            try
            {
                cmb_versionSelect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (txt_username.Text != "") this.ActiveControl = txt_password;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_username.Text) && !string.IsNullOrEmpty(txt_password.Text))
            {
                VerifyVersion();

                ipAddress = GetIPAddress();

                string userID = general.GetLastRecord("t_users", "bayi", $"where adi='{txt_username.Text}'");

                using (MySqlConnection connection = new MySqlConnection(general.MySqlConnectionString))
                {
                    MySqlCommand command = new MySqlCommand($"Select * from t_users where adi='{txt_username.Text}'", connection);

                    try
                    {
                        connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader["adi"].ToString() == txt_username.Text && reader["sifre"].ToString() == txt_password.Text)
                                {
                                    UserID = reader["id"].ToString();
                                    username = reader["adi"].ToString();
                                    LogIn(UserID, ipAddress, txt_password.Text, txt_username.Text);

                                    SistemForm sistemForm = new SistemForm();

                                    sistemForm.lbl_id.Text = UserID;
                                    sistemForm.lbl_tur.Text = reader["tur"].ToString();
                                    sistemForm.lbl_tramer_adi.Text = reader["tramer_adi"].ToString();
                                    sistemForm.lbl_tramer_sifre.Text = reader["tramer_sifre"].ToString();
                                    sistemForm.lbl_ip.Text = ipAddress;
                                    sistemForm.isimLBL.Text = reader["adisoyadi"].ToString();

                                    general._kullanici_id = UserID;
                                    this.Hide();
                                    sistemForm.Show();
                                }
                                else
                                {
                                    LogIn(UserID, ipAddress, txt_password.Text, txt_username.Text);
                                    MessageBox.Show("Invalid username or password");
                                }
                            }
                        }
                        else
                        {
                            LogIn(UserID, ipAddress, txt_password.Text, txt_username.Text);
                            MessageBox.Show("User not found");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else MessageBox.Show("Please enter username and password");
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

        private void cmb_versiyonyaz()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
                myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            LBL_VRSYN.Text = myVersion.ToString();
        }
    }
}
