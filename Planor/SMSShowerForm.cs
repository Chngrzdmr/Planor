using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planor
{
    public partial class SMSShowerForm : Form
    {

        Kalaslar.General gn = new Kalaslar.General();

        public SMSShowerForm()
        {
            InitializeComponent();
            gn.grid_view_getir(" SirketAdi,Mesaj,Tarih from GelenMesaj order by id desc limit 10", dgv_smsler);
        }

        private void SMSShowerForm_Enter(object sender, EventArgs e)
        {
            //gn.grid_view_getir(" * from GelenMesaj order by id asc", dgv_smsler);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            gn.grid_view_getir(" SirketAdi,Mesaj,Tarih from GelenMesaj where Durum = 0 order by id desc limit 10", dgv_smsler);
        }

        private void dgv_smsler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string msg = String.Format("Row: {0}, Column: {1}", dgv_smsler.CurrentCell.RowIndex, dgv_smsler.CurrentCell.ColumnIndex);
            //MessageBox.Show(msg, "Current Cell");
            //MessageBox.Show(dgv_smsler.CurrentCell.Value.ToString());
            //Trayyysms.ShowBalloonTip(700, "Şifre Panoya Kopyalandı", dgv_smsler.CurrentCell.Value.ToString(), ToolTipIcon.Info);
            Clipboard.SetText(dgv_smsler.CurrentCell.Value.ToString());
            this.Close();
        }

        private void dgv_smsler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
