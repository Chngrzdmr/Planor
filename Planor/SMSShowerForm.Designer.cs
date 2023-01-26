
namespace Planor
{
    partial class SMSShowerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMSShowerForm));
            this.dgv_smsler = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.Trayyysms = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_smsler)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_smsler
            // 
            this.dgv_smsler.AllowUserToAddRows = false;
            this.dgv_smsler.AllowUserToDeleteRows = false;
            this.dgv_smsler.AllowUserToResizeColumns = false;
            this.dgv_smsler.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dgv_smsler.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_smsler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_smsler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_smsler.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_smsler.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_smsler.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv_smsler.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv_smsler.Location = new System.Drawing.Point(0, 88);
            this.dgv_smsler.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_smsler.MultiSelect = false;
            this.dgv_smsler.Name = "dgv_smsler";
            this.dgv_smsler.ReadOnly = true;
            this.dgv_smsler.RowHeadersVisible = false;
            this.dgv_smsler.RowTemplate.Height = 24;
            this.dgv_smsler.Size = new System.Drawing.Size(314, 318);
            this.dgv_smsler.TabIndex = 153;
            this.dgv_smsler.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_smsler_CellContentClick);
            this.dgv_smsler.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_smsler_CellDoubleClick);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(314, 53);
            this.button1.TabIndex = 154;
            this.button1.Text = "SMS Tablosunu Güncelle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Trayyysms
            // 
            this.Trayyysms.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Trayyysms.Icon = ((System.Drawing.Icon)(resources.GetObject("Trayyysms.Icon")));
            this.Trayyysms.Text = "...";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 26);
            this.label1.TabIndex = 155;
            this.label1.Text = "** Şifre içeren hücrenin üzerine çift tıklama yaparak şifreyi Panoya kopyalayabil" +
    "irsiniz.";
            // 
            // SMSShowerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 406);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgv_smsler);
            this.Name = "SMSShowerForm";
            this.Text = "Gelen SMS\'ler";
            this.Enter += new System.EventHandler(this.SMSShowerForm_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_smsler)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgv_smsler;
        public System.Windows.Forms.NotifyIcon Trayyysms;
        private System.Windows.Forms.Label label1;
    }
}