using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Planor.Kalaslar;
using Planor.Sayfalar;

namespace Planor.Sayfalar
{
    public partial class SubeAyarlari : UserControl
    {
        // Initialize a new instance of the General class.
        private readonly General _general = new General();

        // Store the MySQL connection string.
        private readonly string _connectionString = _general.MySqlBaglanti;

        // Constructor for the SubeAyarlari user control.
        public SubeAyarlari()
        {
            InitializeComponent();

            // Set the size of the user control based on the screen size.
            SetUserControlSize();

            // Load data into the DataGridView.
            LoadData();
        }

        // Event handler for the SubeAyarlari_Load event.
        private void SubeAyarlari_Load(object sender, EventArgs e)
        {
            // Set the size of the user control based on the screen size.
            SetUserControlSize();

            // Load data into the DataGridView.
            LoadData();
        }

        // Set the size of the user control based on the screen size.
        private void SetUserControlSize()
        {
            this.Size = new Size(
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8),
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height);
        }

        // Load data from the MySQL database into the DataGridView.
        private void LoadData()
        {
            string query = "SELECT id, adi FROM t_bayiler";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            // Set the DataGridView's data source to a new BindingSource with a new DataTable.
                            dt_bayiler.DataSource = new BindingSource(new DataTable(), null);
                            dt_bayiler.DataSource = reader;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            // Auto-resize the DataGridView columns to fit the content.
            dt_bayiler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        // Other methods omitted for brevity
    }
}
