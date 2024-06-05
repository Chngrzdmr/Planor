using Planor.Kalaslar;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace Planor.Sayfalar
{
    public partial class SubeAyarlari : UserControl
    {
        General gn = new General();

        public SubeAyarlari()
        {
            InitializeComponent();

            // Set the size of the user control based on the screen size.
            this.Size = new Size(
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8),
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height);

            // Load data into the DataGridView.
            LoadData();
        }

        private void SubeAyarlari_Load(object sender, EventArgs e)
        {
            // Set the size of the user control based on the screen size.
            this.Size = new Size(
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8),
                new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height);

            // Load data into the DataGridView.
            LoadData();
        }

        private void LoadData()
        {
            string query = "SELECT id, adi FROM t_bayiler";

            using (MySqlConnection connection = new MySqlConnection(gn.MySqlBaglanti))
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

        private void Btn_SubeSil_Click(object sender, EventArgs e)
        {
            if (dt_bayiler.CurrentRow == null) return;

            DialogResult result = MessageBox.Show("Bayiyi Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK) return;

            if (dt_bayiler.CurrentRow.Cells[0] == null) return;

            string deger = dt_bayiler.CurrentRow.Cells[0].Value.ToString();

            string query = "DELETE FROM t_bayiler WHERE id = @id";

            using (MySqlConnection connection = new MySqlConnection(gn.MySqlBaglanti))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", deger);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            LoadData();
            Temizle();
            MessageBox.Show("Bayi Silindi!");
        }

        private void dt_bayiler_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dt_bayiler.CurrentRow == null) return;

            if (dt_bayiler.CurrentRow.Cells[0] == null) return;

            string deger = dt_bayiler.CurrentRow.Cells[0].Value.ToString();
            LblSubeID.Text = deger;

            if (dt_bayiler.CurrentRow.Cells[1].Value == null || dt_bayiler.CurrentRow.Cells[1].Value.ToString() != txt_SubeAdi.Text)
            {
                string query = "SELECT * FROM t_bayiler WHERE id = @id";

                using (MySqlConnection connection = new MySqlConnection(gn.MySqlBaglanti))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", deger);

                        try
                        {
                            connection.Open();
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string ip_durum = reader["ip_durum"].ToString();

                                    txt_SubeAdi.Text = reader["adi"].ToString();
                                    txt_adres.Text = reader["adres"].ToString();
                                    txt_gsm.Text = reader["gsm"].ToString();
                                    GwKullanicilar.Text = reader["ip"].ToString();
                                    txt_telefon.Text = reader["telefon"].ToString();
                                    txt_yetkili.Text = reader["yetkili"].ToString();
                                    txt_ip.Text = reader["ip"].ToString();

                                    if (ip_durum == "1")
                                    {
                                        chk_ip.Checked = true;
                                    }
                                    else
                                    {
                                        chk_ip.Checked = false;
                                    }
                                }
                            }
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }

                GwKullanicilar.Visible = false;

                // Change the UI for edit mode.
                SubeEklePNL.Text = "Şube DÜZENLEME İşlemleri";
                Btn_SubeKaydet.Text = "Şube Bilgileri Düzelt";
                Btn_SubeKaydet.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
                Btn_SubeKaydet.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            }
            else
            {
                if (!GwKullanicilar.Visible)
                {
                    GwKullanicilar.Visible = true;
                    LoadKullanicilar(deger);
                }
            }
        }

        private void LoadKullanicilar(string bayiId)
        {
            string query = "SELECT id, adisoyadi, telefon FROM t_kullanicilar WHERE bayi = @bayiId";

            using (MySqlConnection connection = new MySqlConnection(gn.MySqlBaglanti))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bayiId", bayiId);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            GwKullanicilar.DataSource = new BindingSource(new DataTable(), null);
                            GwKullanicilar.DataSource = reader;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            GwKullanicilar.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void Btn_SubeKaydet_Click(object sender, EventArgs e)
        {
            if (SubeEklePNL.Text == "Şube EKLEME İşlemleri")
            {
                string kullanici_adi = gn.en_son_kaydi_getir("t_bayiler", "adi", "where adi='" + txt_SubeAdi.Text + "'");
                string ipdurum = chk_ip.Checked ? "1" : "0";

                if (kullanici_adi == "")
                {
                    List<string> TabloAdlari = new List<string> { "adi", "ip", "yetkili", "telefon", "gsm", "adres", "ip_durum" };
                    ArrayList veriler = new ArrayList { txt_SubeAdi.Text, GwKullanicilar.Text, txt_yetkili.Text, txt_telefon.Text, txt_gsm.Text, txt_adres.Text, ipdurum };

                    string sonuc = gn.db_kaydet(TabloAdlari, "t_bayiler", veriler);

                    if (sonuc == "islem_tamam")
                    {
                        LoadData();
                        Temizle();
                    }
                    else
                    {
                        MessageBox.Show("Bir Hata Oluştu");
                    }
                }
                else
                {
                    MessageBox.Show("Aynı İsimle Kayıtlı Bir Şube Vardır");
                }
            }
            if (SubeEklePNL.Text == "Şube DÜZENLEME İşlemleri")
            {
                DialogResult result = MessageBox.Show("Bayiyi Düzenlemek İstedinize Eminmisiniz?", "Düzenle", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result != DialogResult.OK) return;

                string ipdurum = chk_ip.Checked ? "1" : "0";

                List<string> TabloAdlari = new List<string> { "adi", "adres", "gsm", "ip", "telefon", "yetkili", "ip_durum" };
                ArrayList veriler = new ArrayList { txt_SubeAdi.Text, txt_adres.Text, txt_gsm.Text, txt_ip.Text, txt_telefon.Text, txt_yetkili.Text, ipdurum };

                string sonuc = gn.db_duzenle(TabloAdlari, "t_bayiler", veriler, "id", LblSubeID.Text);

                if (sonuc == "islem_tamam")
                {
                    LoadData();
                    Temizle();
                    MessageBox.Show("İşlem Tamamlandı");
                }
            }
        }

        public void Temizle()
        {
            txt_SubeAdi.Text = "";
            txt_adres.Text = "";
            txt_gsm.Text = "";
            txt_ip.Text = "";
            txt_telefon.Text = "";
            txt_yetkili.Text = "";

            SubeEklePNL.Text = "Şube EKLEME İşlemleri";
            Btn_SubeKaydet.Text = "Şube Kaydını Yap";
            Btn_SubeKaydet.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            Btn_SubeKaydet.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
        }

        private void dt_bayiler_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dt_bayiler.CurrentRow == null) return;

            if (dt_bayiler.CurrentRow.Cells[0] == null) return;

            string deger = dt_bayiler.CurrentRow.Cells[0].Value.ToString();
            if (LblSubeID.Text != deger) GwKullanicilar.Visible = false;
        }
    }
}
