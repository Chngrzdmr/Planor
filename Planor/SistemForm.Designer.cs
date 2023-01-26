
using System;

namespace Planor
{
    partial class SistemForm
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SistemForm));
            this.AnaMenuPaneli = new Guna.UI2.WinForms.Guna2Panel();
            this.g2LogoBTN = new Guna.UI2.WinForms.Guna2GradientButton();
            this.dgv_sirketler = new System.Windows.Forms.DataGridView();
            this.lbl_ip = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.g2anaMenuBTN5 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.g2anaMenuBTN4 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.g2anaMenuBTN1 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.g2anaMenuBTN3 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.g2anaMenuBTN2 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.g2DP1 = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.ScreenButtonX = new System.Windows.Forms.Button();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.ANAicerikBaslikLBL = new System.Windows.Forms.Label();
            this.isimLBL = new System.Windows.Forms.Label();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.BaslikLabel = new System.Windows.Forms.Label();
            this.LblKullaniciAdi = new System.Windows.Forms.Label();
            this.guna2PictureBox3 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.TimerKareKod = new System.Windows.Forms.Timer(this.components);
            this.TimerKareKod2 = new System.Windows.Forms.Timer(this.components);
            this.TimerSMS = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.DurumLBL = new System.Windows.Forms.Label();
            this.PanelSlider = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_tramer_sifre = new System.Windows.Forms.Label();
            this.lbl_tramer_adi = new System.Windows.Forms.Label();
            this.lb_muhasebe_adi = new System.Windows.Forms.Label();
            this.lbl_tur = new System.Windows.Forms.Label();
            this.lb_muhasebe_sifre = new System.Windows.Forms.Label();
            this.lblSirket = new System.Windows.Forms.Label();
            this.AltPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.lbl_id = new System.Windows.Forms.Label();
            this.Trayyy1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.sagtikmenusu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.çıkışToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sMSŞifrelerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.AnaMenuPaneli.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_sirketler)).BeginInit();
            this.g2DP1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).BeginInit();
            this.PanelSlider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            this.AltPanel.SuspendLayout();
            this.sagtikmenusu.SuspendLayout();
            this.SuspendLayout();
            // 
            // AnaMenuPaneli
            // 
            this.AnaMenuPaneli.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(81)))));
            this.AnaMenuPaneli.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.AnaMenuPaneli.BorderThickness = 1;
            this.AnaMenuPaneli.Controls.Add(this.g2LogoBTN);
            this.AnaMenuPaneli.Controls.Add(this.dgv_sirketler);
            this.AnaMenuPaneli.Controls.Add(this.lbl_ip);
            this.AnaMenuPaneli.Controls.Add(this.label1);
            this.AnaMenuPaneli.Controls.Add(this.g2anaMenuBTN5);
            this.AnaMenuPaneli.Controls.Add(this.g2anaMenuBTN4);
            this.AnaMenuPaneli.Controls.Add(this.g2anaMenuBTN1);
            this.AnaMenuPaneli.Controls.Add(this.g2anaMenuBTN3);
            this.AnaMenuPaneli.Controls.Add(this.g2anaMenuBTN2);
            this.AnaMenuPaneli.Dock = System.Windows.Forms.DockStyle.Left;
            this.AnaMenuPaneli.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.AnaMenuPaneli.Location = new System.Drawing.Point(0, 0);
            this.AnaMenuPaneli.Name = "AnaMenuPaneli";
            this.AnaMenuPaneli.ShadowDecoration.Parent = this.AnaMenuPaneli;
            this.AnaMenuPaneli.Size = new System.Drawing.Size(205, 671);
            this.AnaMenuPaneli.TabIndex = 8;
            // 
            // g2LogoBTN
            // 
            this.g2LogoBTN.Animated = true;
            this.g2LogoBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.g2LogoBTN.BorderColor = System.Drawing.Color.Cyan;
            this.g2LogoBTN.CheckedState.ForeColor = System.Drawing.Color.Black;
            this.g2LogoBTN.CheckedState.Parent = this.g2LogoBTN;
            this.g2LogoBTN.CustomImages.Parent = this.g2LogoBTN;
            this.g2LogoBTN.FillColor = System.Drawing.Color.Empty;
            this.g2LogoBTN.FillColor2 = System.Drawing.Color.Transparent;
            this.g2LogoBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.g2LogoBTN.ForeColor = System.Drawing.Color.White;
            this.g2LogoBTN.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.g2LogoBTN.HoverState.Parent = this.g2LogoBTN;
            this.g2LogoBTN.ImageSize = new System.Drawing.Size(100, 90);
            this.g2LogoBTN.Location = new System.Drawing.Point(3, 3);
            this.g2LogoBTN.Name = "g2LogoBTN";
            this.g2LogoBTN.ShadowDecoration.Parent = this.g2LogoBTN;
            this.g2LogoBTN.Size = new System.Drawing.Size(200, 100);
            this.g2LogoBTN.TabIndex = 153;
            this.g2LogoBTN.Text = "HAKKIMIZDA";
            this.g2LogoBTN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.g2LogoBTN.TextOffset = new System.Drawing.Point(0, 290);
            this.g2LogoBTN.Click += new System.EventHandler(this.menuButon_Click);
            // 
            // dgv_sirketler
            // 
            this.dgv_sirketler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dgv_sirketler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_sirketler.Location = new System.Drawing.Point(145, 652);
            this.dgv_sirketler.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_sirketler.Name = "dgv_sirketler";
            this.dgv_sirketler.RowTemplate.Height = 24;
            this.dgv_sirketler.Size = new System.Drawing.Size(55, 21);
            this.dgv_sirketler.TabIndex = 152;
            this.dgv_sirketler.Visible = false;
            // 
            // lbl_ip
            // 
            this.lbl_ip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_ip.AutoSize = true;
            this.lbl_ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_ip.ForeColor = System.Drawing.Color.White;
            this.lbl_ip.Location = new System.Drawing.Point(3, 641);
            this.lbl_ip.Name = "lbl_ip";
            this.lbl_ip.Size = new System.Drawing.Size(37, 13);
            this.lbl_ip.TabIndex = 17;
            this.lbl_ip.Text = "lbl_ip";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 656);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Versiyon: 1.0.1";
            // 
            // g2anaMenuBTN5
            // 
            this.g2anaMenuBTN5.Animated = true;
            this.g2anaMenuBTN5.BorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN5.CheckedState.CustomBorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN5.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN5.CheckedState.FillColor2 = System.Drawing.Color.White;
            this.g2anaMenuBTN5.CheckedState.ForeColor = System.Drawing.Color.Black;
            this.g2anaMenuBTN5.CheckedState.Image = global::Planor.Properties.Resources.avatar;
            this.g2anaMenuBTN5.CheckedState.Parent = this.g2anaMenuBTN5;
            this.g2anaMenuBTN5.CustomBorderThickness = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.g2anaMenuBTN5.CustomImages.Parent = this.g2anaMenuBTN5;
            this.g2anaMenuBTN5.FillColor = System.Drawing.Color.Empty;
            this.g2anaMenuBTN5.FillColor2 = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.g2anaMenuBTN5.ForeColor = System.Drawing.Color.White;
            this.g2anaMenuBTN5.HoverState.BorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN5.HoverState.CustomBorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN5.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN5.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(174)))), ((int)(((byte)(250)))));
            this.g2anaMenuBTN5.HoverState.Image = global::Planor.Properties.Resources.avatar;
            this.g2anaMenuBTN5.HoverState.Parent = this.g2anaMenuBTN5;
            this.g2anaMenuBTN5.Image = global::Planor.Properties.Resources.avatargry;
            this.g2anaMenuBTN5.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.g2anaMenuBTN5.ImageSize = new System.Drawing.Size(48, 48);
            this.g2anaMenuBTN5.Location = new System.Drawing.Point(3, 550);
            this.g2anaMenuBTN5.Name = "g2anaMenuBTN5";
            this.g2anaMenuBTN5.ShadowDecoration.Parent = this.g2anaMenuBTN5;
            this.g2anaMenuBTN5.Size = new System.Drawing.Size(199, 110);
            this.g2anaMenuBTN5.TabIndex = 15;
            this.g2anaMenuBTN5.Text = "YÖNETİCİ";
            this.g2anaMenuBTN5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.g2anaMenuBTN5.Click += new System.EventHandler(this.menuButon_Click);
            // 
            // g2anaMenuBTN4
            // 
            this.g2anaMenuBTN4.Animated = true;
            this.g2anaMenuBTN4.BorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN4.CheckedState.CustomBorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN4.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN4.CheckedState.FillColor2 = System.Drawing.Color.White;
            this.g2anaMenuBTN4.CheckedState.ForeColor = System.Drawing.Color.Black;
            this.g2anaMenuBTN4.CheckedState.Image = global::Planor.Properties.Resources.team;
            this.g2anaMenuBTN4.CheckedState.Parent = this.g2anaMenuBTN4;
            this.g2anaMenuBTN4.CustomBorderThickness = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.g2anaMenuBTN4.CustomImages.Parent = this.g2anaMenuBTN4;
            this.g2anaMenuBTN4.FillColor = System.Drawing.Color.Empty;
            this.g2anaMenuBTN4.FillColor2 = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.g2anaMenuBTN4.ForeColor = System.Drawing.Color.White;
            this.g2anaMenuBTN4.HoverState.BorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN4.HoverState.CustomBorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN4.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN4.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(174)))), ((int)(((byte)(250)))));
            this.g2anaMenuBTN4.HoverState.Image = global::Planor.Properties.Resources.team;
            this.g2anaMenuBTN4.HoverState.Parent = this.g2anaMenuBTN4;
            this.g2anaMenuBTN4.Image = global::Planor.Properties.Resources.teamgry;
            this.g2anaMenuBTN4.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.g2anaMenuBTN4.ImageSize = new System.Drawing.Size(48, 48);
            this.g2anaMenuBTN4.Location = new System.Drawing.Point(3, 328);
            this.g2anaMenuBTN4.Name = "g2anaMenuBTN4";
            this.g2anaMenuBTN4.ShadowDecoration.Parent = this.g2anaMenuBTN4;
            this.g2anaMenuBTN4.Size = new System.Drawing.Size(199, 110);
            this.g2anaMenuBTN4.TabIndex = 14;
            this.g2anaMenuBTN4.Text = "HIZLI ARAÇLAR";
            this.g2anaMenuBTN4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.g2anaMenuBTN4.Click += new System.EventHandler(this.menuButon_Click);
            // 
            // g2anaMenuBTN1
            // 
            this.g2anaMenuBTN1.Animated = true;
            this.g2anaMenuBTN1.BorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN1.Checked = true;
            this.g2anaMenuBTN1.CheckedState.CustomBorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN1.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN1.CheckedState.FillColor2 = System.Drawing.Color.White;
            this.g2anaMenuBTN1.CheckedState.ForeColor = System.Drawing.Color.Black;
            this.g2anaMenuBTN1.CheckedState.Image = global::Planor.Properties.Resources.network;
            this.g2anaMenuBTN1.CheckedState.Parent = this.g2anaMenuBTN1;
            this.g2anaMenuBTN1.CustomBorderThickness = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.g2anaMenuBTN1.CustomImages.Parent = this.g2anaMenuBTN1;
            this.g2anaMenuBTN1.FillColor = System.Drawing.Color.Empty;
            this.g2anaMenuBTN1.FillColor2 = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.g2anaMenuBTN1.ForeColor = System.Drawing.Color.White;
            this.g2anaMenuBTN1.HoverState.BorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN1.HoverState.CustomBorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN1.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN1.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(174)))), ((int)(((byte)(250)))));
            this.g2anaMenuBTN1.HoverState.Image = global::Planor.Properties.Resources.network;
            this.g2anaMenuBTN1.HoverState.Parent = this.g2anaMenuBTN1;
            this.g2anaMenuBTN1.Image = global::Planor.Properties.Resources.networkgry;
            this.g2anaMenuBTN1.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.g2anaMenuBTN1.ImageSize = new System.Drawing.Size(48, 48);
            this.g2anaMenuBTN1.Location = new System.Drawing.Point(3, 106);
            this.g2anaMenuBTN1.Name = "g2anaMenuBTN1";
            this.g2anaMenuBTN1.ShadowDecoration.Parent = this.g2anaMenuBTN1;
            this.g2anaMenuBTN1.Size = new System.Drawing.Size(199, 110);
            this.g2anaMenuBTN1.TabIndex = 13;
            this.g2anaMenuBTN1.Text = "ANA SAYFA";
            this.g2anaMenuBTN1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.g2anaMenuBTN1.Click += new System.EventHandler(this.menuButon_Click);
            // 
            // g2anaMenuBTN3
            // 
            this.g2anaMenuBTN3.Animated = true;
            this.g2anaMenuBTN3.BorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN3.CheckedState.CustomBorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN3.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN3.CheckedState.FillColor2 = System.Drawing.Color.White;
            this.g2anaMenuBTN3.CheckedState.ForeColor = System.Drawing.Color.Black;
            this.g2anaMenuBTN3.CheckedState.Image = global::Planor.Properties.Resources.settings;
            this.g2anaMenuBTN3.CheckedState.Parent = this.g2anaMenuBTN3;
            this.g2anaMenuBTN3.CustomBorderThickness = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.g2anaMenuBTN3.CustomImages.Parent = this.g2anaMenuBTN3;
            this.g2anaMenuBTN3.FillColor = System.Drawing.Color.Empty;
            this.g2anaMenuBTN3.FillColor2 = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.g2anaMenuBTN3.ForeColor = System.Drawing.Color.White;
            this.g2anaMenuBTN3.HoverState.BorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN3.HoverState.CustomBorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN3.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN3.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(174)))), ((int)(((byte)(250)))));
            this.g2anaMenuBTN3.HoverState.Image = global::Planor.Properties.Resources.settings;
            this.g2anaMenuBTN3.HoverState.Parent = this.g2anaMenuBTN3;
            this.g2anaMenuBTN3.Image = global::Planor.Properties.Resources.settingsgry;
            this.g2anaMenuBTN3.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.g2anaMenuBTN3.ImageSize = new System.Drawing.Size(48, 48);
            this.g2anaMenuBTN3.Location = new System.Drawing.Point(3, 439);
            this.g2anaMenuBTN3.Name = "g2anaMenuBTN3";
            this.g2anaMenuBTN3.ShadowDecoration.Parent = this.g2anaMenuBTN3;
            this.g2anaMenuBTN3.Size = new System.Drawing.Size(199, 110);
            this.g2anaMenuBTN3.TabIndex = 12;
            this.g2anaMenuBTN3.Text = "AYARLAR";
            this.g2anaMenuBTN3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.g2anaMenuBTN3.Click += new System.EventHandler(this.menuButon_Click);
            // 
            // g2anaMenuBTN2
            // 
            this.g2anaMenuBTN2.Animated = true;
            this.g2anaMenuBTN2.BorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN2.CheckedState.CustomBorderColor = System.Drawing.Color.Cyan;
            this.g2anaMenuBTN2.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN2.CheckedState.FillColor2 = System.Drawing.Color.White;
            this.g2anaMenuBTN2.CheckedState.ForeColor = System.Drawing.Color.Black;
            this.g2anaMenuBTN2.CheckedState.Image = global::Planor.Properties.Resources.search;
            this.g2anaMenuBTN2.CheckedState.Parent = this.g2anaMenuBTN2;
            this.g2anaMenuBTN2.CustomBorderThickness = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.g2anaMenuBTN2.CustomImages.Parent = this.g2anaMenuBTN2;
            this.g2anaMenuBTN2.Enabled = false;
            this.g2anaMenuBTN2.FillColor = System.Drawing.Color.Empty;
            this.g2anaMenuBTN2.FillColor2 = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.g2anaMenuBTN2.ForeColor = System.Drawing.Color.White;
            this.g2anaMenuBTN2.HoverState.BorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN2.HoverState.CustomBorderColor = System.Drawing.Color.White;
            this.g2anaMenuBTN2.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.g2anaMenuBTN2.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(174)))), ((int)(((byte)(250)))));
            this.g2anaMenuBTN2.HoverState.Image = global::Planor.Properties.Resources.search;
            this.g2anaMenuBTN2.HoverState.Parent = this.g2anaMenuBTN2;
            this.g2anaMenuBTN2.Image = global::Planor.Properties.Resources.searchgry;
            this.g2anaMenuBTN2.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.g2anaMenuBTN2.ImageSize = new System.Drawing.Size(48, 48);
            this.g2anaMenuBTN2.Location = new System.Drawing.Point(3, 217);
            this.g2anaMenuBTN2.Name = "g2anaMenuBTN2";
            this.g2anaMenuBTN2.ShadowDecoration.Parent = this.g2anaMenuBTN2;
            this.g2anaMenuBTN2.Size = new System.Drawing.Size(199, 110);
            this.g2anaMenuBTN2.TabIndex = 0;
            this.g2anaMenuBTN2.Text = "HIZLI TEKLİF";
            this.g2anaMenuBTN2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.g2anaMenuBTN2.Click += new System.EventHandler(this.menuButon_Click);
            // 
            // g2DP1
            // 
            this.g2DP1.BackColor = System.Drawing.Color.Transparent;
            this.g2DP1.Controls.Add(this.ScreenButtonX);
            this.g2DP1.Controls.Add(this.guna2ControlBox2);
            this.g2DP1.Controls.Add(this.guna2ControlBox1);
            this.g2DP1.Controls.Add(this.ANAicerikBaslikLBL);
            this.g2DP1.Controls.Add(this.isimLBL);
            this.g2DP1.Controls.Add(this.guna2PictureBox1);
            this.g2DP1.Controls.Add(this.guna2PictureBox2);
            this.g2DP1.Controls.Add(this.BaslikLabel);
            this.g2DP1.Controls.Add(this.LblKullaniciAdi);
            this.g2DP1.Controls.Add(this.guna2PictureBox3);
            this.g2DP1.Dock = System.Windows.Forms.DockStyle.Top;
            this.g2DP1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(81)))));
            this.g2DP1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(81)))));
            this.g2DP1.Location = new System.Drawing.Point(205, 0);
            this.g2DP1.Name = "g2DP1";
            this.g2DP1.ShadowDecoration.Parent = this.g2DP1;
            this.g2DP1.Size = new System.Drawing.Size(1055, 75);
            this.g2DP1.TabIndex = 15;
            // 
            // ScreenButtonX
            // 
            this.ScreenButtonX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenButtonX.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ScreenButtonX.ForeColor = System.Drawing.Color.Firebrick;
            this.ScreenButtonX.Location = new System.Drawing.Point(966, 4);
            this.ScreenButtonX.Name = "ScreenButtonX";
            this.ScreenButtonX.Size = new System.Drawing.Size(27, 26);
            this.ScreenButtonX.TabIndex = 35;
            this.ScreenButtonX.Text = "1";
            this.ScreenButtonX.UseVisualStyleBackColor = true;
            this.ScreenButtonX.Click += new System.EventHandler(this.button1_Click);
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.HoverState.FillColor = System.Drawing.Color.Red;
            this.guna2ControlBox2.HoverState.Parent = this.guna2ControlBox2;
            this.guna2ControlBox2.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox2.Location = new System.Drawing.Point(998, 1);
            this.guna2ControlBox2.Name = "guna2ControlBox2";
            this.guna2ControlBox2.ShadowDecoration.Parent = this.guna2ControlBox2;
            this.guna2ControlBox2.Size = new System.Drawing.Size(28, 35);
            this.guna2ControlBox2.TabIndex = 16;
            this.guna2ControlBox2.UseTransparentBackground = true;
            this.guna2ControlBox2.Click += new System.EventHandler(this.guna2ControlBox2_Click);
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.HoverState.FillColor = System.Drawing.Color.Red;
            this.guna2ControlBox1.HoverState.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1026, 1);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.ShadowDecoration.Parent = this.guna2ControlBox1;
            this.guna2ControlBox1.Size = new System.Drawing.Size(28, 35);
            this.guna2ControlBox1.TabIndex = 15;
            this.guna2ControlBox1.UseTransparentBackground = true;
            this.guna2ControlBox1.Click += new System.EventHandler(this.guna2ControlBox1_Click);
            // 
            // ANAicerikBaslikLBL
            // 
            this.ANAicerikBaslikLBL.AutoSize = true;
            this.ANAicerikBaslikLBL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.ANAicerikBaslikLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(162)));
            this.ANAicerikBaslikLBL.ForeColor = System.Drawing.Color.Black;
            this.ANAicerikBaslikLBL.Location = new System.Drawing.Point(62, 49);
            this.ANAicerikBaslikLBL.Name = "ANAicerikBaslikLBL";
            this.ANAicerikBaslikLBL.Size = new System.Drawing.Size(177, 23);
            this.ANAicerikBaslikLBL.TabIndex = 33;
            this.ANAicerikBaslikLBL.Text = "SliderPanel Başlıkları...";
            this.ANAicerikBaslikLBL.Visible = false;
            // 
            // isimLBL
            // 
            this.isimLBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.isimLBL.AutoSize = true;
            this.isimLBL.BackColor = System.Drawing.Color.Transparent;
            this.isimLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.isimLBL.ForeColor = System.Drawing.Color.Silver;
            this.isimLBL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.isimLBL.Location = new System.Drawing.Point(491, 3);
            this.isimLBL.Name = "isimLBL";
            this.isimLBL.Size = new System.Drawing.Size(28, 30);
            this.isimLBL.TabIndex = 28;
            this.isimLBL.Text = "...";
            this.isimLBL.Visible = false;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.BackColor = System.Drawing.Color.White;
            this.guna2PictureBox1.BackgroundImage = global::Planor.Properties.Resources.banner_sol;
            this.guna2PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.guna2PictureBox1.Location = new System.Drawing.Point(0, 35);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.ShadowDecoration.Parent = this.guna2PictureBox1;
            this.guna2PictureBox1.Size = new System.Drawing.Size(56, 40);
            this.guna2PictureBox1.TabIndex = 31;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.guna2PictureBox1_MouseDoubleClick);
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.Image = global::Planor.Properties.Resources.User;
            this.guna2PictureBox2.Location = new System.Drawing.Point(0, 2);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.ShadowDecoration.Color = System.Drawing.Color.Empty;
            this.guna2PictureBox2.ShadowDecoration.Parent = this.guna2PictureBox2;
            this.guna2PictureBox2.Size = new System.Drawing.Size(40, 40);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox2.TabIndex = 29;
            this.guna2PictureBox2.TabStop = false;
            // 
            // BaslikLabel
            // 
            this.BaslikLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BaslikLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BaslikLabel.ForeColor = System.Drawing.Color.White;
            this.BaslikLabel.Location = new System.Drawing.Point(899, 0);
            this.BaslikLabel.Name = "BaslikLabel";
            this.BaslikLabel.Padding = new System.Windows.Forms.Padding(0, 10, 60, 0);
            this.BaslikLabel.Size = new System.Drawing.Size(156, 35);
            this.BaslikLabel.TabIndex = 17;
            this.BaslikLabel.Text = "Planör";
            // 
            // LblKullaniciAdi
            // 
            this.LblKullaniciAdi.AutoSize = true;
            this.LblKullaniciAdi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblKullaniciAdi.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblKullaniciAdi.Location = new System.Drawing.Point(46, 11);
            this.LblKullaniciAdi.Name = "LblKullaniciAdi";
            this.LblKullaniciAdi.Size = new System.Drawing.Size(126, 21);
            this.LblKullaniciAdi.TabIndex = 18;
            this.LblKullaniciAdi.Text = "LblKullaniciAdi";
            // 
            // guna2PictureBox3
            // 
            this.guna2PictureBox3.BackColor = System.Drawing.Color.White;
            this.guna2PictureBox3.BackgroundImage = global::Planor.Properties.Resources.banner_orta;
            this.guna2PictureBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2PictureBox3.Location = new System.Drawing.Point(0, 35);
            this.guna2PictureBox3.Name = "guna2PictureBox3";
            this.guna2PictureBox3.Padding = new System.Windows.Forms.Padding(90);
            this.guna2PictureBox3.ShadowDecoration.Parent = this.guna2PictureBox3;
            this.guna2PictureBox3.Size = new System.Drawing.Size(1055, 40);
            this.guna2PictureBox3.TabIndex = 32;
            this.guna2PictureBox3.TabStop = false;
            this.guna2PictureBox3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.guna2PictureBox3_MouseDoubleClick);
            // 
            // TimerKareKod
            // 
            this.TimerKareKod.Interval = 1000;
            this.TimerKareKod.Tick += new System.EventHandler(this.TimerKareKod_Tick);
            // 
            // TimerKareKod2
            // 
            this.TimerKareKod2.Interval = 1000;
            this.TimerKareKod2.Tick += new System.EventHandler(this.TimerKareKod2_Tick);
            // 
            // TimerSMS
            // 
            this.TimerSMS.Interval = 10000;
            this.TimerSMS.Tick += new System.EventHandler(this.TimerSMS_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 80);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(60, 60);
            this.tableLayoutPanel1.TabIndex = 23;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.Tomato;
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1055, 36);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 27;
            // 
            // DurumLBL
            // 
            this.DurumLBL.AutoSize = true;
            this.DurumLBL.BackColor = System.Drawing.Color.Transparent;
            this.DurumLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(162)));
            this.DurumLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(81)))));
            this.DurumLBL.Location = new System.Drawing.Point(7, 10);
            this.DurumLBL.Name = "DurumLBL";
            this.DurumLBL.Size = new System.Drawing.Size(17, 17);
            this.DurumLBL.TabIndex = 28;
            this.DurumLBL.Text = "...";
            // 
            // PanelSlider
            // 
            this.PanelSlider.BackColor = System.Drawing.Color.White;
            this.PanelSlider.Controls.Add(this.button2);
            this.PanelSlider.Controls.Add(this.pictureBox11);
            this.PanelSlider.Controls.Add(this.button1);
            this.PanelSlider.Controls.Add(this.tableLayoutPanel1);
            this.PanelSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelSlider.FillColor = System.Drawing.Color.Transparent;
            this.PanelSlider.FillColor2 = System.Drawing.Color.Transparent;
            this.PanelSlider.ForeColor = System.Drawing.Color.White;
            this.PanelSlider.Location = new System.Drawing.Point(205, 75);
            this.PanelSlider.Name = "PanelSlider";
            this.PanelSlider.ShadowDecoration.Parent = this.PanelSlider;
            this.PanelSlider.Size = new System.Drawing.Size(1055, 560);
            this.PanelSlider.TabIndex = 16;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(768, 494);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 60);
            this.button2.TabIndex = 26;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox11
            // 
            this.pictureBox11.Location = new System.Drawing.Point(592, 494);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(170, 60);
            this.pictureBox11.TabIndex = 25;
            this.pictureBox11.TabStop = false;
            this.pictureBox11.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(39, 304);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 44);
            this.button1.TabIndex = 24;
            this.button1.Text = "şirket buton";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.sirketButon_Click);
            // 
            // lbl_tramer_sifre
            // 
            this.lbl_tramer_sifre.AutoSize = true;
            this.lbl_tramer_sifre.ForeColor = System.Drawing.Color.Black;
            this.lbl_tramer_sifre.Location = new System.Drawing.Point(480, 22);
            this.lbl_tramer_sifre.Name = "lbl_tramer_sifre";
            this.lbl_tramer_sifre.Size = new System.Drawing.Size(93, 13);
            this.lbl_tramer_sifre.TabIndex = 15;
            this.lbl_tramer_sifre.Text = "lbl_tramer_sifre";
            this.lbl_tramer_sifre.Visible = false;
            // 
            // lbl_tramer_adi
            // 
            this.lbl_tramer_adi.AutoSize = true;
            this.lbl_tramer_adi.ForeColor = System.Drawing.Color.Black;
            this.lbl_tramer_adi.Location = new System.Drawing.Point(369, 22);
            this.lbl_tramer_adi.Name = "lbl_tramer_adi";
            this.lbl_tramer_adi.Size = new System.Drawing.Size(86, 13);
            this.lbl_tramer_adi.TabIndex = 12;
            this.lbl_tramer_adi.Text = "lbl_tramer_adi";
            this.lbl_tramer_adi.Visible = false;
            // 
            // lb_muhasebe_adi
            // 
            this.lb_muhasebe_adi.AutoSize = true;
            this.lb_muhasebe_adi.ForeColor = System.Drawing.Color.Black;
            this.lb_muhasebe_adi.Location = new System.Drawing.Point(605, 22);
            this.lb_muhasebe_adi.Name = "lb_muhasebe_adi";
            this.lb_muhasebe_adi.Size = new System.Drawing.Size(105, 13);
            this.lb_muhasebe_adi.TabIndex = 16;
            this.lb_muhasebe_adi.Text = "lb_muhasebe_adi";
            this.lb_muhasebe_adi.Visible = false;
            // 
            // lbl_tur
            // 
            this.lbl_tur.AutoSize = true;
            this.lbl_tur.ForeColor = System.Drawing.Color.Black;
            this.lbl_tur.Location = new System.Drawing.Point(307, 22);
            this.lbl_tur.Name = "lbl_tur";
            this.lbl_tur.Size = new System.Drawing.Size(42, 13);
            this.lbl_tur.TabIndex = 14;
            this.lbl_tur.Text = "lbl_tur";
            this.lbl_tur.Visible = false;
            // 
            // lb_muhasebe_sifre
            // 
            this.lb_muhasebe_sifre.AutoSize = true;
            this.lb_muhasebe_sifre.ForeColor = System.Drawing.Color.Black;
            this.lb_muhasebe_sifre.Location = new System.Drawing.Point(723, 22);
            this.lb_muhasebe_sifre.Name = "lb_muhasebe_sifre";
            this.lb_muhasebe_sifre.Size = new System.Drawing.Size(112, 13);
            this.lb_muhasebe_sifre.TabIndex = 17;
            this.lb_muhasebe_sifre.Text = "lb_muhasebe_sifre";
            this.lb_muhasebe_sifre.Visible = false;
            // 
            // lblSirket
            // 
            this.lblSirket.AutoSize = true;
            this.lblSirket.ForeColor = System.Drawing.Color.Black;
            this.lblSirket.Location = new System.Drawing.Point(973, 22);
            this.lblSirket.Name = "lblSirket";
            this.lblSirket.Size = new System.Drawing.Size(53, 13);
            this.lblSirket.TabIndex = 19;
            this.lblSirket.Text = "lblSirket";
            this.lblSirket.Visible = false;
            // 
            // AltPanel
            // 
            this.AltPanel.AutoSize = true;
            this.AltPanel.Controls.Add(this.lbl_id);
            this.AltPanel.Controls.Add(this.lbl_tur);
            this.AltPanel.Controls.Add(this.lbl_tramer_adi);
            this.AltPanel.Controls.Add(this.lbl_tramer_sifre);
            this.AltPanel.Controls.Add(this.lb_muhasebe_adi);
            this.AltPanel.Controls.Add(this.lb_muhasebe_sifre);
            this.AltPanel.Controls.Add(this.DurumLBL);
            this.AltPanel.Controls.Add(this.lblSirket);
            this.AltPanel.Controls.Add(this.progressBar);
            this.AltPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AltPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.AltPanel.ForeColor = System.Drawing.Color.White;
            this.AltPanel.Location = new System.Drawing.Point(205, 635);
            this.AltPanel.Name = "AltPanel";
            this.AltPanel.ShadowDecoration.Parent = this.AltPanel;
            this.AltPanel.Size = new System.Drawing.Size(1055, 36);
            this.AltPanel.TabIndex = 19;
            // 
            // lbl_id
            // 
            this.lbl_id.AutoSize = true;
            this.lbl_id.ForeColor = System.Drawing.Color.Black;
            this.lbl_id.Location = new System.Drawing.Point(252, 22);
            this.lbl_id.Name = "lbl_id";
            this.lbl_id.Size = new System.Drawing.Size(37, 13);
            this.lbl_id.TabIndex = 30;
            this.lbl_id.Text = "lbl_id";
            this.lbl_id.Visible = false;
            // 
            // Trayyy1
            // 
            this.Trayyy1.ContextMenuStrip = this.sagtikmenusu;
            this.Trayyy1.Icon = ((System.Drawing.Icon)(resources.GetObject("Trayyy1.Icon")));
            this.Trayyy1.Text = "...";
            this.Trayyy1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Trayyy1_MouseDoubleClick);
            // 
            // sagtikmenusu
            // 
            this.sagtikmenusu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.çıkışToolStripMenuItem,
            this.toolStripSeparator1,
            this.sMSŞifrelerToolStripMenuItem,
            this.toolStripSeparator2});
            this.sagtikmenusu.Name = "contextMenuStrip1";
            this.sagtikmenusu.Size = new System.Drawing.Size(137, 60);
            this.sagtikmenusu.Opening += new System.ComponentModel.CancelEventHandler(this.sagtikmenusu_Opening);
            // 
            // çıkışToolStripMenuItem
            // 
            this.çıkışToolStripMenuItem.Name = "çıkışToolStripMenuItem";
            this.çıkışToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.çıkışToolStripMenuItem.Tag = "Çıkış";
            this.çıkışToolStripMenuItem.Text = "Çıkış";
            this.çıkışToolStripMenuItem.Click += new System.EventHandler(this.çıkışToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            this.toolStripSeparator1.Tag = "Çıkış";
            // 
            // sMSŞifrelerToolStripMenuItem
            // 
            this.sMSŞifrelerToolStripMenuItem.Name = "sMSŞifrelerToolStripMenuItem";
            this.sMSŞifrelerToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.sMSŞifrelerToolStripMenuItem.Text = "SMS Şifreler";
            this.sMSŞifrelerToolStripMenuItem.Click += new System.EventHandler(this.sMSŞifrelerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(133, 6);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // SistemForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1260, 671);
            this.Controls.Add(this.PanelSlider);
            this.Controls.Add(this.AltPanel);
            this.Controls.Add(this.g2DP1);
            this.Controls.Add(this.AnaMenuPaneli);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ForeColor = System.Drawing.Color.DarkOrange;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SistemForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Planör";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SistemForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SistemForm_FormClosed);
            this.Load += new System.EventHandler(this.SistemForm_Load);
            this.Resize += new System.EventHandler(this.SistemForm_Resize);
            this.AnaMenuPaneli.ResumeLayout(false);
            this.AnaMenuPaneli.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_sirketler)).EndInit();
            this.g2DP1.ResumeLayout(false);
            this.g2DP1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).EndInit();
            this.PanelSlider.ResumeLayout(false);
            this.PanelSlider.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            this.AltPanel.ResumeLayout(false);
            this.AltPanel.PerformLayout();
            this.sagtikmenusu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2GradientButton g2anaMenuBTN2;
        private Guna.UI2.WinForms.Guna2GradientPanel g2DP1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2GradientButton g2anaMenuBTN1;
        private Guna.UI2.WinForms.Guna2GradientButton g2anaMenuBTN3;
        private Guna.UI2.WinForms.Guna2GradientButton g2anaMenuBTN4;
        private Guna.UI2.WinForms.Guna2GradientButton g2anaMenuBTN5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label BaslikLabel;
        public System.Windows.Forms.Label lbl_ip;
        private System.Windows.Forms.DataGridView dgv_sirketler;
        private System.Windows.Forms.Timer TimerKareKod;
        private System.Windows.Forms.Timer TimerKareKod2;
        public System.Windows.Forms.Label isimLBL;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private System.Windows.Forms.Timer TimerSMS;
        private Guna.UI2.WinForms.Guna2GradientButton g2LogoBTN;
        public Guna.UI2.WinForms.Guna2Panel AnaMenuPaneli;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox3;
        private System.Windows.Forms.Label ANAicerikBaslikLBL;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ProgressBar progressBar;
        public Guna.UI2.WinForms.Guna2GradientPanel PanelSlider;
        private Guna.UI2.WinForms.Guna2Panel AltPanel;
        public System.Windows.Forms.Label lblSirket;
        public System.Windows.Forms.Label LblKullaniciAdi;
        public System.Windows.Forms.Label lb_muhasebe_sifre;
        public System.Windows.Forms.Label lbl_tur;
        public System.Windows.Forms.Label lb_muhasebe_adi;
        public System.Windows.Forms.Label lbl_tramer_adi;
        public System.Windows.Forms.Label lbl_tramer_sifre;
        private System.Windows.Forms.ContextMenuStrip sagtikmenusu;
        private System.Windows.Forms.ToolStripMenuItem çıkışToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.Label DurumLBL;
        public System.Windows.Forms.NotifyIcon Trayyy1;
        public System.Windows.Forms.Label lbl_id;
        private System.Windows.Forms.ToolStripMenuItem sMSŞifrelerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button ScreenButtonX;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Button button2;
    }
}

