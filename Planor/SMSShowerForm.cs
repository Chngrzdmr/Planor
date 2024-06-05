using System;
using System.Windows.Forms;
using Kalaslar; // Namespace for General class

namespace Planor
{
    // SMSShowerForm class represents the main form for displaying SMS messages
    public partial class SMSShowerForm : Form
    {
        // Initialize a new instance of the General class
        General gn = new General();

        // SMSShowerForm constructor, initializes the form and loads SMS messages
        public SMSShowerForm()
        {
            InitializeComponent();
            LoadSmsMessages(); // Load SMS messages when the form is created
        }

        // SMSShowerForm_Enter event handler, loads SMS messages when the form is entered
        private void SMSShowerForm_Enter(object sender, EventArgs e)
        {
            LoadSmsMessages();
        }

        // button1_Click event handler, loads SMS messages and marks them as read
        private void button1_Click(object sender, EventArgs e)
        {
            LoadSmsMessages(true);
        }

        // LoadSmsMessages method, loads SMS messages based on the isUnreadOnly parameter
        private void LoadSmsMessages(bool isUnreadOnly = false)
        {
            string query = "SELECT SirketAdi, Mesaj, Tarih FROM GelenMesaj "; // SQL query to select SMS messages
            if (isUnreadOnly)
            {
                query += "WHERE Durum = 0 "; // Add WHERE clause to filter unread messages
            }
            query += "ORDER BY id DESC LIMIT 10"; // Order messages by ID in descending order and limit to 10
            dgv_smsler.DataSource = gn.DataTableGetir(query); // Set DataGridView's data source to the query result
        }

        // dgv_smsler_CellDoubleClick event handler, copies the selected message to the clipboard and closes the form
        private void dgv_smsler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_smsler.CurrentCell != null)
            {
                string message = dgv_smsler.CurrentRow.Cells["Mesaj"].Value.ToString(); // Get the selected message
                Clipboard.SetText(message); // Copy the message to the clipboard
                this.Close(); // Close the form
            }
        }

        // dgv_smsler_CellContentClick event handler, does nothing by default
        private void dgv_smsler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
