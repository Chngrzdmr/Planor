using Guna.UI2.WinForms; // For using Guna UI components
using MySql.Data.MySqlClient; // For working with MySQL databases
using Planor.Kalaslar; // Planor.Kalaslar namespace import
using Planor.Sayfalar; // Planor.Sayfalar namespace import
using System; // For general system namespaces
using System.Collections.Generic; // For working with collections
using System.Drawing; // For working with graphics and colors
using System.Linq; // For using LINQ querying capabilities
using System.Net; // For working with network connections
using System.Windows.Forms; // For working with Windows Forms

namespace Planor // Planor application namespace
{
    public partial class LoginForm : Form // LoginForm class definition
    {
        // Fields
        private string ipAddress; // Holds the user's IP address
        private General general = new General(); // General class object for common functions
        private MySqlConnection connection; // MySQL connection object
        private string username; // Holds the username

        // Properties
        public static string UserID { get; private set; } // Holds the user's ID

        // Constructor
        public LoginForm()
        {
            InitializeComponent(); // Initialize the form components

            // Set the drag control for the Guna2Image component
            new Guna2DragControl(g2LoginImage);
            LBL_IP.Text = GetIPAddress(); // Set the IP address label text

            LoadSettings(); // Load settings
            LoadVersion(); // Load the application version

            if (IsLoggedIn()) // Check if the user is already logged in
            {
                ShowSistemForm(); // Show the SistemForm
                Close(); // Close the LoginForm
            }
        }

        // Event handlers
        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (txt_username.Text != "") this.ActiveControl = txt_password; // Set the focus to the password textbox if the username textbox is not empty
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (!ValidateFormFields()) return; // Validate form fields and exit the method if invalid

            VerifyVersion(); // Verify the application version

            ipAddress = GetIPAddress(); // Get the user's IP address

            using (connection = new MySqlConnection(general.MySqlConnectionString)) // Create a new MySQL connection
            {
                connection.Open(); // Open the connection

                using (MySqlCommand command = new MySqlCommand("Select * from t_users where adi=@username", connection)) // Create a new MySQL command
                {
                    command.Parameters.AddWithValue("@username", txt_username.Text); // Add the username parameter

                    using (MySqlDataReader reader = command.ExecuteReader()) // Execute the command and get the reader
                    {
                        if (reader.HasRows) // Check if there are any rows
                        {
                            while (reader.Read()) // Iterate through the rows
                            {
                                if (reader["adi"].ToString() == txt_username.Text && reader["sifre"].ToString() == txt_password.Text) // Check if the username and password match
                                {
                                    UserID = reader["id"].ToString(); // Set the UserID property
                                    username = reader["adi"].ToString(); // Set the username variable
                                    LogIn(UserID, ipAddress, txt_password.Text, txt_username.Text); // Log the user in

                                    ShowSistemForm(); // Show the SistemForm
                                    Hide(); // Hide the LoginForm
                                }
                                else
                                {
                                    LogFailedLoginAttempt(UserID, ipAddress, txt_password.Text, txt_username.Text); // Log the failed login attempt
                                    MessageBox.Show("Invalid username or password"); // Show an error message
                                }
                            }
                        }
                        else
                        {
                            LogFailedLoginAttempt(UserID, ipAddress, txt_password.Text, txt_username.Text); // Log the failed login attempt
                            MessageBox.Show("User not found"); // Show an error message
                        }
                    }
                }
            }
        }

        // Methods
        private string GetIPAddress()
        {
            string externalIP = ""; // Initialize the external IP variable

            try
            {
                externalIP = new WebClient().DownloadString("https://api.ipify.org/"); // Try to get the IP address from the API
                externalIP = externalIP.Replace("\n", ""); // Remove newline characters
            }
            catch
            {
                externalIP = new WebClient().DownloadString("http://icanhazip.com"); // If the API fails, get the IP address from another source
                externalIP = externalIP.Replace("\n", ""); // Remove newline characters
            }

            return externalIP; // Return the IP address
        }

        private void LogIn(string userID, string ipAddress, string password, string username)
        {
            List<string> columnNames = new List<string> // Initialize the column names list
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

            ArrayList values = new ArrayList // Initialize the values array list
            {
                userID,
                general.GetLastRecord("t_bayiler", "id", $"where ip_durum=1 and ip='{ipAddress}'"), // Get the last record from the t_bayiler table
                DateTime.Now.ToString(), // Get the current date and time
                DateTime.Now.Day.ToString(), // Get the current day
                DateTime.Now.Month.ToString(), // Get the current month
                DateTime.Now.Year.ToString(), // Get the current year
                Environment.MachineName, // Get the current machine name
                "Giriş Başarısız", // Set the login status to unsuccessful
                ipAddress, // Set the IP address
                username, // Set the username
                password, // Set the password
                "Giriş" // Set the login type
            };

            general.InsertData(columnNames, "t_logkayitlari", values); // Insert the data into the t_logkayitlari table
        }

        private void LogFailedLoginAttempt(string userID, string ipAddress, string password, string username)
        {
            List<string> columnNames = new List<string> // Initialize the column names list
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

            ArrayList values = new ArrayList // Initialize the values array list
            {
                userID,
                null, // Set the BayiID to null
                DateTime.Now.ToString(), // Get the current date and time
                DateTime.Now.Day.ToString(), // Get the current day
                DateTime.Now.Month.ToString(), // Get the current month
                DateTime.Now.Year.ToString(), // Get the current year
                Environment.MachineName, // Get the current machine name
                "Giriş Başarısız", // Set the login status to unsuccessful
                ipAddress, // Set the IP address
                username, // Set the username
                password, // Set the password
                "Giriş" // Set the login type
            };

            general.InsertData(columnNames, "t_logkayitlari", values); // Insert the data into the t_logkayitlari table
        }

        private void ClearFormFields()
        {
            txt_password.Clear(); // Clear the password textbox
            txt_username.Clear(); // Clear the username textbox
            txt_username.Focus(); // Set the focus to the username textbox
        }

        private void LoadSettings()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Username)) // Check if the Username setting is not empty
            {
                txt_username.Text = Properties.Settings.Default.Username; // Set the username textbox text
            }
        }

        private void LoadVersion()
        {
            var myVersion = System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed
                ? System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion
                : System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; // Get the application version

            LBL_VRSYN.Text = myVersion.ToString(); // Set the version label text
        }

        private bool ValidateFormFields()
        {
            if (string.IsNullOrEmpty(txt_username.Text) || string.IsNullOrEmpty(txt_password.Text)) // Check if the username and password textboxes are empty
            {
                MessageBox.Show("Please enter username and password"); // Show an error message
                return false; // Return false
            }

            return true; // Return true
        }

        private bool IsLoggedIn()
        {
            if (UserID != "") return true; // Return true if the UserID is not empty

            return false; // Return false otherwise
        }

        private void ShowSistemForm()
        {
            var sistemForm = new SistemForm // Create a new SistemForm object
            {
                lbl_id = { Text = UserID }, // Set the lbl_id text
                lbl_tur = { Text = general.GetLastRecord("t_users", "tur", $"where id='{UserID}'") }, // Set the lbl_tur text
                lbl_tramer_adi = { Text = general.GetLastRecord("t_users", "tramer_adi", $"where id='{UserID}'") }, // Set the lbl_tramer_adi text
                lbl_tramer_sifre = { Text = general.GetLastRecord("t_users", "tramer_sifre", $"where id='{UserID}'") }, // Set the lbl_tramer_sifre text
                lbl_ip = { Text = ipAddress }, // Set the lbl_ip text
                isimLBL = { Text = general.GetLastRecord("t_users", "adisoyadi", $"where id='{UserID}'") } // Set the isimLBL text
            };

            general._kullanici_id = UserID; // Set the general._kullanici_id property
            Hide(); // Hide the LoginForm
            sistemForm.Show(); // Show the SistemForm
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

