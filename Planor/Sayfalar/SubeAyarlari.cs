using Planor.Kalaslar;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Planor.Sayfalar
{
    public partial class SubeAyarlari : UserControl
    {
        General gn = new General();
        public SubeAyarlari()
        {
            InitializeComponent();

            //ekrana göre ölçüleri ayarlamak için
            this.Width = new Yonetici().yoneticiSliderPNL.Width;
            this.Height = new Yonetici().yoneticiSliderPNL.Height;
        }

        private void SubeAyarlari_Load(object sender, EventArgs e)
        {
            //ekrana göre ölçüleri ayarlamak için
            this.Width = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width - ((new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Width) / 8);
            this.Height = new SistemForm().screens[new SistemForm().ekranno].WorkingArea.Height - 111 - new Yonetici().yoneticiMenuPNL.Height;


            //gn.grid_view_getir(" id,adi from t_bayiler order by adi asc", dt_bayiler);
            gn.grid_view_getir(" id,adi from t_bayiler", dt_bayiler);
            dt_bayiler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //yeniolcekleme();
        }

        private void Btn_SubeSil_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bayiyi Silmek İstedinize Eminmisiniz?", "Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                if (dt_bayiler.CurrentRow.Cells[0] != null)
                {
                    string deger = dt_bayiler.CurrentRow.Cells[0].Value.ToString();
                    string sonuc = gn.db_sil("t_bayiler", deger);
                    if (sonuc == "işlem tamamlandı")
                    {
                        gn.grid_view_getir(" id,adi from t_bayiler", dt_bayiler);
                        dt_bayiler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        Temizle();

                        MessageBox.Show("Bayi Silindi!");
                    }
                    else
                    {
                        MessageBox.Show("Bir Hata Oluştu");
                    }
                }
            }
        }

        private void dt_bayiler_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string deger = dt_bayiler.CurrentRow.Cells[0].Value.ToString();
            LblSubeID.Text = deger.ToString();

            if (dt_bayiler.CurrentRow.Cells[1].Value.ToString() != txt_SubeAdi.Text)
            {
                MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
                MySqlCommand com = new MySqlCommand(@"Select * from t_bayiler where id = " + LblSubeID.Text + " ", con);

                try
                {
                    con.Open();
                    MySqlDataReader oku = com.ExecuteReader();
                    while (oku.Read())
                    {
                        string ip_durum = "0";

                        txt_SubeAdi.Text = oku["adi"].ToString();
                        txt_adres.Text = oku["adres"].ToString();
                        txt_gsm.Text = oku["gsm"].ToString();
                        GwKullanicilar.Text = oku["ip"].ToString();
                        txt_telefon.Text = oku["telefon"].ToString();
                        txt_yetkili.Text = oku["yetkili"].ToString();
                        txt_ip.Text = oku["ip"].ToString();
                        ip_durum = oku["ip_durum"].ToString();

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
                catch (Exception exp)
                {
                    string hata = exp.ToString();
                }
                finally
                {
                    con.Close();
                }
                GwKullanicilar.Visible = false;

                //Buraya ekleme kutusunu düzeltme kutusunu dönüştürecek kodlar yazılacak
                SubeEklePNL.Text = "Şube DÜZENLEME İşlemleri";
                Btn_SubeKaydet.Text = "Şube Bilgileri Düzelt";
                Btn_SubeKaydet.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
                //Btn_SubeKaydet.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
                //Btn_SubeKaydet.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
                Btn_SubeKaydet.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));

            }
            else
            {
                if (!GwKullanicilar.Visible)
                {
                    GwKullanicilar.Visible = true;
                    gn.grid_view_getir(" id,adisoyadi,telefon from t_kullanicilar where bayi =" + deger.ToString() + " order by adi asc", GwKullanicilar);
                    GwKullanicilar.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
        }

        private void Btn_SubeKaydet_Click(object sender, EventArgs e)
        {
            if (SubeEklePNL.Text == "Şube EKLEME İşlemleri")
            {
                string kullanici_adi = gn.en_son_kaydi_getir("t_bayiler", "adi", "where adi='" + txt_SubeAdi.Text + "'");
                string ipdurum = "1";
                if (chk_ip.Checked == true)
                {
                    ipdurum = "1";
                }
                else
                {
                    ipdurum = "0";
                }
                if (kullanici_adi == "")
                {
                    List<string> TabloAdlari = new List<string>();
                    TabloAdlari.Add("adi");
                    TabloAdlari.Add("ip");
                    TabloAdlari.Add("yetkili");
                    TabloAdlari.Add("telefon");
                    TabloAdlari.Add("gsm");
                    TabloAdlari.Add("adres");
                    TabloAdlari.Add("ip_durum");

                    ArrayList veriler = new ArrayList();
                    veriler.Add(txt_SubeAdi.Text);
                    veriler.Add(GwKullanicilar.Text);
                    veriler.Add(txt_yetkili.Text);
                    veriler.Add(txt_telefon.Text);
                    veriler.Add(txt_gsm.Text);
                    veriler.Add(txt_adres.Text);
                    veriler.Add(ipdurum);
                    string sonuc = gn.db_kaydet(TabloAdlari, "t_bayiler", veriler);
                    if (sonuc == "islem_tamam")
                    {
                        gn.grid_view_getir(" id,adi from t_bayiler order by adi asc", dt_bayiler);
                        dt_bayiler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

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
                if (result.Equals(DialogResult.OK))
                {
                    string ipdurum = "1";
                    if (chk_ip.Checked == true)
                    {
                        ipdurum = "1";
                    }
                    else
                    {
                        ipdurum = "0";
                    }
                    string deger = LblSubeID.Text;

                    List<string> TabloAdlari = new List<string>();
                    TabloAdlari.Add("adi");
                    TabloAdlari.Add("adres");
                    TabloAdlari.Add("gsm");
                    TabloAdlari.Add("ip");
                    TabloAdlari.Add("telefon");
                    TabloAdlari.Add("yetkili");
                    TabloAdlari.Add("ip_durum");

                    ArrayList veriler = new ArrayList();
                    veriler.Add(txt_SubeAdi.Text);
                    veriler.Add(txt_adres.Text);
                    veriler.Add(txt_gsm.Text);
                    veriler.Add(txt_ip.Text);
                    veriler.Add(txt_telefon.Text);
                    veriler.Add(txt_yetkili.Text);
                    veriler.Add(ipdurum);

                    string sonuc = gn.db_duzenle(TabloAdlari, "t_bayiler", veriler, "id", deger);
                    if (sonuc == "islem_tamam")
                    {
                        gn.grid_view_getir(" id,adi from t_bayiler", dt_bayiler);
                        dt_bayiler.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        MessageBox.Show("İşlem Tamamlandı");

                        Temizle();
                    }
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
            //Btn_SubeKaydet.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
            //Btn_SubeKaydet.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(172)))), ((int)(((byte)(228)))));
            Btn_SubeKaydet.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(132)))));
            
        }

        private void SubeAyarlari_Resize(object sender, EventArgs e)
        {

        }

        private void SubeAyarlari_Paint(object sender, PaintEventArgs e)
        {
            Temizle();
        }

        private void dt_bayiler_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string deger = dt_bayiler.CurrentRow.Cells[0].Value.ToString();
            if (LblSubeID.Text != deger.ToString()) GwKullanicilar.Visible = false;
        }


        private void yeniolcekleme()
        {
            float firstWidth = 1260;
            float firstHeight = 700;

            float size1 = this.Size.Width / firstWidth;
            float size2 = this.Size.Height / firstHeight;

            SizeF scale = new SizeF(size1, size2);
            firstWidth = this.Size.Width;
            firstHeight = this.Size.Height;

            foreach (Control control in this.Controls)
            {

                control.Font = new Font(control.Font.FontFamily, control.Font.Size * ((size1 + size2) / 2));

                control.Scale(scale);


            }
        }
    }
}
