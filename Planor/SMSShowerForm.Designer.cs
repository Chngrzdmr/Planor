using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace Planor
{
    partial class SMSShowerForm
    {
        private IContainer components = null;

        private DataGridView dgvSmsList;
        private Button btnRefresh;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem trayRefreshItem;
        private ToolStripMenuItem trayExitItem;
        private Label lblInfo;

        public SMSShowerForm()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            trayRefreshItem = new System.Windows.Forms.ToolStripMenuItem();
            trayExitItem = new System.Windows.Forms.ToolStripMenuItem();

            // ...

            // dgvSmsList
            //
            dgvSmsList.AllowUserToAddRows = false;
            dgvSmsList.AllowUserToDeleteRows = false;
            dgvSmsList.AllowUserToResizeColumns = false;
            dgvSmsList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dgvSmsList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSmsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dgvSmsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSmsList.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvSmsList.DefaultCellStyle = dataGridViewCellStyle2;
            dgvSmsList.Dock = System.Windows.Forms.DockStyle.Bottom;
            dgvSmsList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            dgvSmsList.Location = new System.Drawing.Point(0, 88);
            dgvSmsList.Margin = new System.Windows.Forms.Padding(2);
            dgvSmsList.MultiSelect = false;
            dgvSmsList.Name = "dgvSmsList";
            dgvSmsList.ReadOnly = true;
            dgvSmsList.RowHeadersVisible = false;
            dgvSmsList.RowTemplate.Height = 24;
            dgvSmsList.Size = new System.Drawing.Size(314, 318);
            dgvSmsList.TabIndex = 153;
            dgvSmsList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvSmsList_CellContentClick);
            dgvSmsList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvSmsList_CellDoubleClick);

            // ...

            // contextMenuStrip
            //
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            trayRefreshItem,
            trayExitItem});
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(117, 48);

            // ...

            // trayRefreshItem
            //
            trayRefreshItem.Name = "trayRefreshItem";
            trayRefreshItem.Size = new System.Drawing.Size(116, 22);
            trayRefreshItem.Text = "Güncelle";
            trayRefreshItem.Click += new System.EventHandler(trayRefreshItem_Click);

            // trayExitItem
            //
            trayExitItem.Name = "trayExitItem";
            trayExitItem.Size = new System.Drawing.Size(116, 22);
            trayExitItem.Text = "Çık";
            trayExitItem.Click += new System.EventHandler(trayExitItem_Click);

            // SMSShowerForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 406);
            this.Controls.Add(lblInfo);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(dgvSmsList);
            this.Name = "SMSShowerForm";
            this.Text = "Gelen SMS\'ler";
            this.Enter += new System.EventHandler(SMSShowerForm_Enter);
            ((System.ComponentModel.ISupportInitialize)(dgvSmsList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private void trayRefreshItem_Click(object sender, EventArgs e)
        {
            // Handle the tray refresh item click event.
        }

        private void trayExitItem_Click(object sender, EventArgs e)
        {
            // Handle the tray exit item click event.
        }
    }
}
