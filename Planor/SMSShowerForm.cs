using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            gn.GridViewGetir(query, dgv_smsler);
        }

        private void dgv_smsler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_smsler.CurrentCell != null)
            {
                string message = dgv_smsler.CurrentCell.Value.ToString();
                Clipboard.SetText(message);
                this.Close();
            }
        }

        private void dgv_smsler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
