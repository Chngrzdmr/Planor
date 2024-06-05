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
        private readonly General _general = new General();
        private readonly string _connectionString = _general.MySqlBaglanti;

        public SubeAyarlari()
        {
            InitializeComponent();

            // Set the size of the user control based on the screen size.
            SetUserControlSize();

            // Load data into the DataGridView.
            LoadData();
        }

        private void SubeAyarlari_Load(object sender, EventArgs e)
        {
            // Set the size of the user control based on the screen size.
            SetUserControlSize();

            // Load data into the DataGridView.
            LoadData();
        }

        private void SetUserControlSize()
        {
            this.Size = new Size(
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8),
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height);
        }

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

            dt_bayiler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        // Other methods omitted for brevity
    }
}
