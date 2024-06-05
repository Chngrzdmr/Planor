using System;
using System.Windows.Forms;
using Kalaslar;

namespace Planor
{
    public partial class SMSShowerForm : Form
    {
        General gn = new General();

        public SMSShowerForm()
        {
            InitializeComponent();
            LoadSmsMessages();
        }

        private void SMSShowerForm_Enter(object sender, EventArgs e)
        {
            LoadSmsMessages();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSmsMessages(true);
        }

        private void LoadSmsMessages(bool isUnreadOnly = false)
        {
            string query = "SELECT SirketAdi, Mesaj, Tarih FROM GelenMesaj ";
            if (isUnreadOnly)
            {
                query += "WHERE Durum = 0 ";
            }
            query += "ORDER BY id DESC LIMIT 10";
            dgv_smsler.DataSource = gn.DataTableGetir(query);
        }

        private void dgv_smsler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_smsler.CurrentCell != null)
            {
                string message = dgv_smsler.CurrentRow.Cells["Mesaj"].Value.ToString();
                Clipboard.SetText(message);
                this.Close();
            }
        }

        private void dgv_smsler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
