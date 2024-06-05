using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace Planor
{
    partial class SMSShowerForm
    {
        // The IContainer components field is used to store the components that are needed for this form.
        private IContainer components = null;

        // The dgvSmsList DataGridView displays the list of SMS messages.
        private DataGridView dgvSmsList;

        // The btnRefresh Button is used to refresh the list of SMS messages.
        private Button btnRefresh;

        // The notifyIcon is used to display an icon in the system tray.
        private NotifyIcon notifyIcon;

        // The contextMenuStrip is used to display a context menu when the user clicks the notifyIcon.
        private ContextMenuStrip contextMenuStrip;

        // The trayRefreshItem ToolStripMenuItem is used to refresh the list of SMS messages from the context menu.
        private ToolStripMenuItem trayRefreshItem;

        // The trayExitItem ToolStripMenuItem is used to close the application from the context menu.
        private ToolStripMenuItem trayExitItem;

        // The lblInfo Label is used to display additional information to the user.
        private Label lblInfo;

        public SMSShowerForm()
        {
            // InitializeComponent is called to create the components required for this form.
            InitializeComponent();
        }

        // The Dispose method is called when the form is closed. It releases the resources used by the form.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // InitializeComponent is called by the constructor to initialize the form's components.
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // The dgvSmsList DataGridView is configured with various properties.
            dgvSmsList.AllowUserToAddRows = false;
            dgvSmsList.AllowUserToDeleteRows = false;
            dgvSmsList.AllowUserToResizeColumns = false;
            dgvSmsList.AllowUserToResizeRows = false;
            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dgvSmsList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSmsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dgvSmsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSmsList.ColumnHeadersVisible = false;
            DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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

            // The contextMenuStrip is configured with two ToolStripMenuItems.
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            trayRefreshItem = new System.Windows.Forms.ToolStripMenuItem();
            trayExitItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            trayRefreshItem,
            trayExitItem});
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(117, 48);
            trayRefreshItem.Name = "trayRefreshItem";
            trayRefreshItem.Size = new System.Drawing.Size(116, 22);
            trayRefreshItem.Text = "Güncelle";
            trayRefreshItem.Click += new System.EventHandler(trayRefreshItem_Click);
            trayExitItem.Name = "trayExitItem";
            trayExitItem.Size = new System.Drawing.Size(116, 22);
            trayExitItem.Text = "Çık";
            trayExitItem.Click += new System.EventHandler(trayExitItem_Click);

            // The SMSShowerForm is configured with various properties.
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

        // The trayRefreshItem_Click method is called when the user clicks the trayRefreshItem from the context menu.
        private void trayRefreshItem_Click(object sender, EventArgs e)
        {
            // Handle the tray refresh item click event.
        }

        // The trayExitItem_Click method is called when the user clicks the trayExitItem from the context menu.
        private void trayExitItem_Click(object sender, EventArgs e)
        {
            // Handle the tray exit item click event.
        }
    }
}
