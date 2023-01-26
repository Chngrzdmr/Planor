
namespace Planor.Sayfalar
{
    partial class SigortaSirketleri
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgw_sirket_listesi = new Guna.UI2.WinForms.Guna2DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgw_sigorta_sirketleri = new Guna.UI2.WinForms.Guna2DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.BtnSirketSil = new Guna.UI2.WinForms.Guna2Button();
            this.BtnSirketEkle = new Guna.UI2.WinForms.Guna2Button();
            this.BtnSirketSil2 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.BtnSirketEkle2 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.SirketAdıDuzeltPNL = new System.Windows.Forms.GroupBox();
            this.BtnKaydet = new Guna.UI2.WinForms.Guna2Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.LblSirketID = new System.Windows.Forms.Label();
            this.TxtSirketAdi = new Guna.UI2.WinForms.Guna2TextBox();
            this.BtnKaydet2 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgw_sirket_listesi)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgw_sigorta_sirketleri)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SirketAdıDuzeltPNL.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.dgw_sirket_listesi);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(550, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 620);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Şirket Tanımlama Listeniz";
            // 
            // dgw_sirket_listesi
            // 
            this.dgw_sirket_listesi.AllowUserToAddRows = false;
            this.dgw_sirket_listesi.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgw_sirket_listesi.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgw_sirket_listesi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgw_sirket_listesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgw_sirket_listesi.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgw_sirket_listesi.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgw_sirket_listesi.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgw_sirket_listesi.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgw_sirket_listesi.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgw_sirket_listesi.ColumnHeadersHeight = 4;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgw_sirket_listesi.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgw_sirket_listesi.EnableHeadersVisualStyles = false;
            this.dgw_sirket_listesi.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgw_sirket_listesi.Location = new System.Drawing.Point(6, 19);
            this.dgw_sirket_listesi.MultiSelect = false;
            this.dgw_sirket_listesi.Name = "dgw_sirket_listesi";
            this.dgw_sirket_listesi.ReadOnly = true;
            this.dgw_sirket_listesi.RowHeadersVisible = false;
            this.dgw_sirket_listesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgw_sirket_listesi.Size = new System.Drawing.Size(294, 595);
            this.dgw_sirket_listesi.TabIndex = 18;
            this.dgw_sirket_listesi.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Default;
            this.dgw_sirket_listesi.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgw_sirket_listesi.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgw_sirket_listesi.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgw_sirket_listesi.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgw_sirket_listesi.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgw_sirket_listesi.ThemeStyle.BackColor = System.Drawing.Color.LightGray;
            this.dgw_sirket_listesi.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgw_sirket_listesi.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgw_sirket_listesi.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgw_sirket_listesi.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dgw_sirket_listesi.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgw_sirket_listesi.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgw_sirket_listesi.ThemeStyle.HeaderStyle.Height = 4;
            this.dgw_sirket_listesi.ThemeStyle.ReadOnly = true;
            this.dgw_sirket_listesi.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgw_sirket_listesi.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgw_sirket_listesi.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dgw_sirket_listesi.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgw_sirket_listesi.ThemeStyle.RowsStyle.Height = 22;
            this.dgw_sirket_listesi.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgw_sirket_listesi.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgw_sirket_listesi.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgw_sirket_listesi_CellMouseDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.dgw_sigorta_sirketleri);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox2.Location = new System.Drawing.Point(14, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 620);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sigorta Şirketleri";
            // 
            // dgw_sigorta_sirketleri
            // 
            this.dgw_sigorta_sirketleri.AllowUserToDeleteRows = false;
            this.dgw_sigorta_sirketleri.AllowUserToResizeColumns = false;
            this.dgw_sigorta_sirketleri.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgw_sigorta_sirketleri.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgw_sigorta_sirketleri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgw_sigorta_sirketleri.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgw_sigorta_sirketleri.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgw_sigorta_sirketleri.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgw_sigorta_sirketleri.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgw_sigorta_sirketleri.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgw_sigorta_sirketleri.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgw_sigorta_sirketleri.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgw_sigorta_sirketleri.ColumnHeadersHeight = 4;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgw_sigorta_sirketleri.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgw_sigorta_sirketleri.EnableHeadersVisualStyles = false;
            this.dgw_sigorta_sirketleri.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgw_sigorta_sirketleri.Location = new System.Drawing.Point(6, 19);
            this.dgw_sigorta_sirketleri.Name = "dgw_sigorta_sirketleri";
            this.dgw_sigorta_sirketleri.RowHeadersVisible = false;
            this.dgw_sigorta_sirketleri.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgw_sigorta_sirketleri.Size = new System.Drawing.Size(276, 595);
            this.dgw_sigorta_sirketleri.TabIndex = 19;
            this.dgw_sigorta_sirketleri.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Default;
            this.dgw_sigorta_sirketleri.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgw_sigorta_sirketleri.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgw_sigorta_sirketleri.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgw_sigorta_sirketleri.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgw_sigorta_sirketleri.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgw_sigorta_sirketleri.ThemeStyle.BackColor = System.Drawing.Color.LightGray;
            this.dgw_sigorta_sirketleri.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgw_sigorta_sirketleri.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgw_sigorta_sirketleri.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgw_sigorta_sirketleri.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dgw_sigorta_sirketleri.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgw_sigorta_sirketleri.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgw_sigorta_sirketleri.ThemeStyle.HeaderStyle.Height = 4;
            this.dgw_sigorta_sirketleri.ThemeStyle.ReadOnly = false;
            this.dgw_sigorta_sirketleri.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgw_sigorta_sirketleri.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgw_sigorta_sirketleri.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dgw_sigorta_sirketleri.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgw_sigorta_sirketleri.ThemeStyle.RowsStyle.Height = 22;
            this.dgw_sigorta_sirketleri.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgw_sigorta_sirketleri.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.BtnSirketSil);
            this.groupBox4.Controls.Add(this.BtnSirketEkle);
            this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox4.Location = new System.Drawing.Point(308, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(236, 150);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "İşlemler";
            // 
            // BtnSirketSil
            // 
            this.BtnSirketSil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSirketSil.CheckedState.Parent = this.BtnSirketSil;
            this.BtnSirketSil.CustomImages.Parent = this.BtnSirketSil;
            this.BtnSirketSil.FillColor = System.Drawing.Color.Firebrick;
            this.BtnSirketSil.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnSirketSil.ForeColor = System.Drawing.Color.White;
            this.BtnSirketSil.HoverState.Parent = this.BtnSirketSil;
            this.BtnSirketSil.Location = new System.Drawing.Point(6, 93);
            this.BtnSirketSil.Name = "BtnSirketSil";
            this.BtnSirketSil.ShadowDecoration.Parent = this.BtnSirketSil;
            this.BtnSirketSil.Size = new System.Drawing.Size(224, 46);
            this.BtnSirketSil.TabIndex = 158;
            this.BtnSirketSil.Text = "<<<- Seçili Şirketi Sil";
            this.BtnSirketSil.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.BtnSirketSil.Click += new System.EventHandler(this.BtnSirketSil_Click_1);
            // 
            // BtnSirketEkle
            // 
            this.BtnSirketEkle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSirketEkle.CheckedState.Parent = this.BtnSirketEkle;
            this.BtnSirketEkle.CustomImages.Parent = this.BtnSirketEkle;
            this.BtnSirketEkle.FillColor = System.Drawing.SystemColors.Highlight;
            this.BtnSirketEkle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnSirketEkle.ForeColor = System.Drawing.Color.White;
            this.BtnSirketEkle.HoverState.Parent = this.BtnSirketEkle;
            this.BtnSirketEkle.Location = new System.Drawing.Point(6, 32);
            this.BtnSirketEkle.Name = "BtnSirketEkle";
            this.BtnSirketEkle.ShadowDecoration.Parent = this.BtnSirketEkle;
            this.BtnSirketEkle.Size = new System.Drawing.Size(224, 46);
            this.BtnSirketEkle.TabIndex = 157;
            this.BtnSirketEkle.Text = "Şirketi Ekle ->>>";
            this.BtnSirketEkle.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.BtnSirketEkle.Click += new System.EventHandler(this.BtnSirketEkle_Click_1);
            // 
            // BtnSirketSil2
            // 
            this.BtnSirketSil2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnSirketSil2.AutoRoundedCorners = true;
            this.BtnSirketSil2.BackColor = System.Drawing.Color.Transparent;
            this.BtnSirketSil2.BorderRadius = 22;
            this.BtnSirketSil2.CheckedState.Parent = this.BtnSirketSil2;
            this.BtnSirketSil2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSirketSil2.CustomImages.Parent = this.BtnSirketSil2;
            this.BtnSirketSil2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BtnSirketSil2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnSirketSil2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnSirketSil2.ForeColor = System.Drawing.Color.White;
            this.BtnSirketSil2.HoverState.BorderColor = System.Drawing.Color.Black;
            this.BtnSirketSil2.HoverState.CustomBorderColor = System.Drawing.Color.Black;
            this.BtnSirketSil2.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
            this.BtnSirketSil2.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            this.BtnSirketSil2.HoverState.Parent = this.BtnSirketSil2;
            this.BtnSirketSil2.Location = new System.Drawing.Point(314, 502);
            this.BtnSirketSil2.Name = "BtnSirketSil2";
            this.BtnSirketSil2.ShadowDecoration.Depth = 20;
            this.BtnSirketSil2.ShadowDecoration.Parent = this.BtnSirketSil2;
            this.BtnSirketSil2.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.BtnSirketSil2.Size = new System.Drawing.Size(224, 46);
            this.BtnSirketSil2.TabIndex = 19;
            this.BtnSirketSil2.Text = "<<<- Seçili Şirketi Sil";
            this.BtnSirketSil2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.BtnSirketSil2.Visible = false;
            this.BtnSirketSil2.Click += new System.EventHandler(this.BtnSirketSil_Click);
            // 
            // BtnSirketEkle2
            // 
            this.BtnSirketEkle2.AutoRoundedCorners = true;
            this.BtnSirketEkle2.BackColor = System.Drawing.Color.Transparent;
            this.BtnSirketEkle2.BorderRadius = 22;
            this.BtnSirketEkle2.CheckedState.Parent = this.BtnSirketEkle2;
            this.BtnSirketEkle2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSirketEkle2.CustomImages.Parent = this.BtnSirketEkle2;
            this.BtnSirketEkle2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            this.BtnSirketEkle2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
            this.BtnSirketEkle2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnSirketEkle2.ForeColor = System.Drawing.Color.White;
            this.BtnSirketEkle2.HoverState.BorderColor = System.Drawing.Color.Black;
            this.BtnSirketEkle2.HoverState.CustomBorderColor = System.Drawing.Color.Black;
            this.BtnSirketEkle2.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
            this.BtnSirketEkle2.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            this.BtnSirketEkle2.HoverState.Parent = this.BtnSirketEkle2;
            this.BtnSirketEkle2.Location = new System.Drawing.Point(314, 450);
            this.BtnSirketEkle2.Name = "BtnSirketEkle2";
            this.BtnSirketEkle2.ShadowDecoration.Depth = 20;
            this.BtnSirketEkle2.ShadowDecoration.Parent = this.BtnSirketEkle2;
            this.BtnSirketEkle2.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.BtnSirketEkle2.Size = new System.Drawing.Size(224, 46);
            this.BtnSirketEkle2.TabIndex = 18;
            this.BtnSirketEkle2.Text = "Şirketi Ekle ->>>";
            this.BtnSirketEkle2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.BtnSirketEkle2.Visible = false;
            this.BtnSirketEkle2.Click += new System.EventHandler(this.BtnSirketEkle_Click);
            // 
            // SirketAdıDuzeltPNL
            // 
            this.SirketAdıDuzeltPNL.Controls.Add(this.BtnKaydet);
            this.SirketAdıDuzeltPNL.Controls.Add(this.richTextBox1);
            this.SirketAdıDuzeltPNL.Controls.Add(this.LblSirketID);
            this.SirketAdıDuzeltPNL.Controls.Add(this.TxtSirketAdi);
            this.SirketAdıDuzeltPNL.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.SirketAdıDuzeltPNL.Location = new System.Drawing.Point(862, 20);
            this.SirketAdıDuzeltPNL.Name = "SirketAdıDuzeltPNL";
            this.SirketAdıDuzeltPNL.Size = new System.Drawing.Size(236, 226);
            this.SirketAdıDuzeltPNL.TabIndex = 125;
            this.SirketAdıDuzeltPNL.TabStop = false;
            this.SirketAdıDuzeltPNL.Text = "Tanımlı Şirket Adını Düzelt";
            // 
            // BtnKaydet
            // 
            this.BtnKaydet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnKaydet.CheckedState.Parent = this.BtnKaydet;
            this.BtnKaydet.CustomImages.Parent = this.BtnKaydet;
            this.BtnKaydet.FillColor = System.Drawing.Color.Chocolate;
            this.BtnKaydet.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.BtnKaydet.ForeColor = System.Drawing.Color.White;
            this.BtnKaydet.HoverState.Parent = this.BtnKaydet;
            this.BtnKaydet.Location = new System.Drawing.Point(6, 85);
            this.BtnKaydet.Name = "BtnKaydet";
            this.BtnKaydet.ShadowDecoration.Parent = this.BtnKaydet;
            this.BtnKaydet.Size = new System.Drawing.Size(224, 46);
            this.BtnKaydet.TabIndex = 158;
            this.BtnKaydet.Text = "Tanımlı Şirket Adını Değiştir";
            this.BtnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.richTextBox1.Location = new System.Drawing.Point(6, 154);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(224, 69);
            this.richTextBox1.TabIndex = 131;
            this.richTextBox1.Text = "*** Tanımlı şirketin adını düzeltmek için\n\"Şirket Tanımlama Listeniz\"\nbölümünden," +
    " düzenlemek istediğiniz \nşirket adının üzerine çift tıklama yapınız.";
            // 
            // LblSirketID
            // 
            this.LblSirketID.AutoSize = true;
            this.LblSirketID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblSirketID.Location = new System.Drawing.Point(162, 21);
            this.LblSirketID.Name = "LblSirketID";
            this.LblSirketID.Size = new System.Drawing.Size(68, 17);
            this.LblSirketID.TabIndex = 129;
            this.LblSirketID.Text = "SIRKET ID";
            // 
            // TxtSirketAdi
            // 
            this.TxtSirketAdi.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TxtSirketAdi.DefaultText = "";
            this.TxtSirketAdi.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.TxtSirketAdi.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.TxtSirketAdi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TxtSirketAdi.DisabledState.Parent = this.TxtSirketAdi;
            this.TxtSirketAdi.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TxtSirketAdi.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TxtSirketAdi.FocusedState.Parent = this.TxtSirketAdi;
            this.TxtSirketAdi.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.TxtSirketAdi.ForeColor = System.Drawing.Color.Black;
            this.TxtSirketAdi.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TxtSirketAdi.HoverState.Parent = this.TxtSirketAdi;
            this.TxtSirketAdi.Location = new System.Drawing.Point(6, 42);
            this.TxtSirketAdi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtSirketAdi.Name = "TxtSirketAdi";
            this.TxtSirketAdi.PasswordChar = '\0';
            this.TxtSirketAdi.PlaceholderText = "";
            this.TxtSirketAdi.SelectedText = "";
            this.TxtSirketAdi.ShadowDecoration.Parent = this.TxtSirketAdi;
            this.TxtSirketAdi.Size = new System.Drawing.Size(224, 36);
            this.TxtSirketAdi.TabIndex = 127;
            // 
            // BtnKaydet2
            // 
            this.BtnKaydet2.AutoRoundedCorners = true;
            this.BtnKaydet2.BackColor = System.Drawing.Color.Transparent;
            this.BtnKaydet2.BorderRadius = 22;
            this.BtnKaydet2.CheckedState.Parent = this.BtnKaydet2;
            this.BtnKaydet2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKaydet2.CustomImages.Parent = this.BtnKaydet2;
            this.BtnKaydet2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.BtnKaydet2.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.BtnKaydet2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BtnKaydet2.ForeColor = System.Drawing.Color.White;
            this.BtnKaydet2.HoverState.BorderColor = System.Drawing.Color.Black;
            this.BtnKaydet2.HoverState.CustomBorderColor = System.Drawing.Color.Black;
            this.BtnKaydet2.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
            this.BtnKaydet2.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            this.BtnKaydet2.HoverState.Parent = this.BtnKaydet2;
            this.BtnKaydet2.Location = new System.Drawing.Point(314, 554);
            this.BtnKaydet2.Name = "BtnKaydet2";
            this.BtnKaydet2.ShadowDecoration.Depth = 20;
            this.BtnKaydet2.ShadowDecoration.Parent = this.BtnKaydet2;
            this.BtnKaydet2.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.BtnKaydet2.Size = new System.Drawing.Size(224, 46);
            this.BtnKaydet2.TabIndex = 126;
            this.BtnKaydet2.Text = "Tanımlı Şirket Adını Değiştir";
            this.BtnKaydet2.Visible = false;
            this.BtnKaydet2.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(1092, 624);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 126;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(323, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 17);
            this.label1.TabIndex = 130;
            this.label1.Text = "Bu düğmeler silinecek";
            this.label1.Visible = false;
            // 
            // SigortaSirketleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnKaydet2);
            this.Controls.Add(this.BtnSirketSil2);
            this.Controls.Add(this.BtnSirketEkle2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SirketAdıDuzeltPNL);
            this.Controls.Add(this.groupBox2);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "SigortaSirketleri";
            this.Size = new System.Drawing.Size(1110, 641);
            this.Load += new System.EventHandler(this.SigortaSirketleri_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgw_sirket_listesi)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgw_sigorta_sirketleri)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.SirketAdıDuzeltPNL.ResumeLayout(false);
            this.SirketAdıDuzeltPNL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private Guna.UI2.WinForms.Guna2DataGridView dgw_sirket_listesi;
        private Guna.UI2.WinForms.Guna2GradientButton BtnSirketSil2;
        private Guna.UI2.WinForms.Guna2GradientButton BtnSirketEkle2;
        private Guna.UI2.WinForms.Guna2DataGridView dgw_sigorta_sirketleri;
        private System.Windows.Forms.GroupBox SirketAdıDuzeltPNL;
        private Guna.UI2.WinForms.Guna2TextBox TxtSirketAdi;
        private Guna.UI2.WinForms.Guna2GradientButton BtnKaydet2;
        private System.Windows.Forms.Label LblSirketID;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private Guna.UI2.WinForms.Guna2Button BtnSirketEkle;
        private Guna.UI2.WinForms.Guna2Button BtnSirketSil;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button BtnKaydet;
    }
}
