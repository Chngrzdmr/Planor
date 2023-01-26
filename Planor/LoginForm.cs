using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Planor.Kalaslar;
using Planor.Sayfalar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Xml;

namespace Planor
{
    public partial class LoginForm : Form
    {
        string ipadresi = "";
        string isim;
        General gn = new General();

        public static string KullaniciID = "";

        public LoginForm()
        {
            InitializeComponent();

            new Guna2DragControl(g2LoginImage);
            LBL_IP.Text = ip_al();
        }

        Version myVersion;


        private void LoginForm_Load(object sender, EventArgs e)
        {
            //Console.Beep(600, 1400);
            if (Properties.Settings.Default.Kullanici != "")
            {
                txt_ka.Text = Properties.Settings.Default.Kullanici;
            }

            try
            {
                cmb_versiyonyaz();
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
            if (txt_ka.Text != "") this.ActiveControl = txt_sifre;
        }




        private void girisyap()
        {
            if (txt_ka.Text != "" && txt_sifre.Text != "")
            {

                versyionkontrolet();

                ipadresi = ip_al();

                //kullanıcılar tablosundan Bayi ID yi kullanıcı adına göre getir
                string bayiid = gn.en_son_kaydi_getir("t_kullanicilar", "bayi", "where adi='" + txt_ka.Text + "'");

                GC.Collect();
                #region  kodlar
                //bayiler tablosundan Bayi ID ye göre IP kısıtlaması olup olmadığına bak
                string ipdurum = gn.en_son_kaydi_getir("t_bayiler", "ip_durum", "where id='" + bayiid + "'");
                string bayiip = gn.en_son_kaydi_getir("t_bayiler", "ip", "where id='" + bayiid + "'");
                bool yetkidurumu = false;

                //Bayinin IP Durumu 1 ise IP sınırlaması yok
                if (ipdurum == "1")
                {
                    //kullanıcılar tablosundan tur alanından kullanıcının yönetici olup olmadığına bak 1 ise yetki ver
                    string kullanicituru = gn.en_son_kaydi_getir("t_kullanicilar", "tur", "where adi='" + txt_ka.Text + "'");
                    if (kullanicituru == "1") yetkidurumu = true;

                    //Bayi IP adresi ile giriş yapılan IP aynı ise yönetici yetkisi ver
                    //METE ye SOR (zaten kullanıcıya yetki veriyoruz yada vermiyoruz) buna ne gerek var?
                    if (bayiip == LBL_IP.Text) yetkidurumu = true;

                    if (yetkidurumu == true)
                    {
                        string tramer_ka = "";
                        string tramer_sifre = "";
                        int deger = 0;
                        //int Kod = 0;
                        string sube_id = "";
                        string tur = "";
                        string muhasebe_ka = "";
                        string muhasebe_sifre = "";


                        MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);

                        MySqlCommand com = new MySqlCommand("Select * from t_kullanicilar where adi='" + txt_ka.Text + "'", con);

                        try
                        {
                            con.Open();
                            MySqlDataReader dr = com.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {

                                    if (dr["adi"].ToString() == txt_ka.Text && dr["sifre"].ToString() == txt_sifre.Text)
                                    {
                                        deger = 1;
                                        sube_id = dr["id"].ToString();

                                        KullaniciID = dr["id"].ToString();
                                        tur = dr["tur"].ToString();
                                        tramer_ka = dr["tramer_ka"].ToString();
                                        tramer_sifre = dr["tramer_sifre"].ToString();
                                        muhasebe_ka = dr["muhasebe_ka"].ToString();
                                        muhasebe_sifre = dr["muhasebe_sifre"].ToString();
                                        isim = dr["adisoyadi"].ToString();


                                        lblsube_id.Text = sube_id;
                                        lbltur.Text = tur;
                                        lbltramerka.Text = tramer_ka;
                                        lbltramersifre.Text = tramer_sifre;
                                        logkayit(KullaniciID, bayiid, "Giriş Başarılı", ipadresi, txt_sifre.Text, txt_ka.Text);

                                    }

                                    else
                                    {
                                        logkayit(KullaniciID, bayiid, "Giriş Başarısız", ipadresi, txt_sifre.Text, txt_ka.Text);
                                        MessageBox.Show("Hatalı Kullanıcı veya Şifre");
                                    }

                                }
                            }
                            else
                            {
                                logkayit(KullaniciID, bayiid, "Giriş Başarısız", ipadresi, txt_sifre.Text, txt_ka.Text);
                                MessageBox.Show("Kullanıcı Kaydı Bulunamamıştır.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            if (con != null)
                            {
                                con.Close();
                                con.Dispose();
                                GC.Collect();
                                if (deger == 1)
                                {

                                    SistemForm sirket_form = new SistemForm();


                                    sirket_form.lbl_id.Text = sube_id;


                                    sirket_form.lbl_tur.Text = tur;
                                    sirket_form.lbl_tramer_adi.Text = tramer_ka;
                                    sirket_form.lbl_tramer_sifre.Text = tramer_sifre;
                                    sirket_form.lbl_ip.Text = ipadresi;
                                    sirket_form.isimLBL.Text = isim;

                                    sirket_form.lb_muhasebe_adi.Text = muhasebe_ka;
                                    sirket_form.lb_muhasebe_sifre.Text = muhasebe_sifre;


                                    gn._kullanici_id = KullaniciID;
                                    this.Hide();
                                    sirket_form.Show();
                                }
                            }
                            con.Dispose();
                            GC.Collect();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Sistemde Kayıtlı Olmayan Bir İpden Girmeye Çalışıyorsunuz");
                    }
                }
                //Bayinin IP Durumu 1 DEĞİLSE 
                else
                {
                    #region
                    string tramer_ka = "";
                    string tramer_sifre = "";


                    int deger = 0;
                    string sube_id = "";
                    string tur = "";
                    MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
                    MySqlCommand com = new MySqlCommand("Select * from t_kullanicilar where adi='" + txt_ka.Text + "'", con);

                    try
                    {
                        con.Open();
                        MySqlDataReader dr = com.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                if (dr["adi"].ToString() == txt_ka.Text && dr["sifre"].ToString() == txt_sifre.Text)
                                {
                                    deger = 1;
                                    sube_id = dr["id"].ToString();
                                    tur = dr["tur"].ToString();
                                    KullaniciID = dr["id"].ToString();
                                    tramer_ka = dr["tramer_ka"].ToString();
                                    tramer_sifre = dr["tramer_sifre"].ToString();


                                    lblsube_id.Text = sube_id;
                                    lbltur.Text = tur;
                                    lbltramerka.Text = tramer_ka;
                                    lbltramersifre.Text = tramer_sifre;
                                    logkayit(KullaniciID, bayiid, "Giriş Başarılı", ipadresi, txt_sifre.Text, txt_ka.Text);

                                }

                                else
                                {
                                    logkayit(KullaniciID, bayiid, "Giriş Başarısız", ipadresi, txt_sifre.Text, txt_ka.Text);
                                    MessageBox.Show("Hatalı Kullanıcı ve ya Şifre");
                                }

                            }
                        }
                        else
                        {
                            logkayit(KullaniciID, bayiid, "Giriş Başarısız", ipadresi, txt_sifre.Text, txt_ka.Text);
                            MessageBox.Show("Kullanıcı Kaydı Bulunamamıştır.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                            if (deger == 1)
                            {
                                //SmsGonder();



                                SistemForm sirket_form = new SistemForm();
                                sirket_form.lbl_id.Text = lblsube_id.Text;
                                sirket_form.lbl_tur.Text = lbltur.Text;
                                sirket_form.lbl_tramer_adi.Text = lbltramerka.Text;
                                sirket_form.lbl_tramer_sifre.Text = lbltramersifre.Text;
                                sirket_form.lbl_ip.Text = ipadresi;
                                sirket_form.LblKullaniciAdi.Text = txt_ka.Text;
                                sirket_form.isimLBL.Text = isim;

                                gn._kullanici_id = KullaniciID;

                                this.Hide();
                                sirket_form.Show();

                                GC.Collect();
                                //this.Dispose();
                            }
                        }

                    }
                    #endregion
                }
                #endregion
                GC.Collect();
            }
            else MessageBox.Show("Lütfen Kullanıcı Adı ve Şifrenizi giriniz...");
        }

        static string GetMacAddress()
        {
            string sMacAddress = "";
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card  
                    {
                        //IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return sMacAddress;

            }
            catch
            {
                return sMacAddress;
            }
        }

        private static string ip_al()
        {
            string externalIP = "";
            try
            {

                string ipAddress = new WebClient().DownloadString("https://api.ipify.org/");
                ipAddress = ipAddress.Replace("\n", "");
                externalIP = ipAddress;
                return externalIP;
            }
            catch
            {

                return externalIP;
            }
        }

        private static string IpAl()
        {
            string externalIP = "";
            try
            {
                string ipAddress = new WebClient().DownloadString("http://icanhazip.com");
                ipAddress = ipAddress.Replace("\n", "");
                externalIP = ipAddress;
                return externalIP;
            }
            catch
            {
                return externalIP;
            }
        }

        private void versyionkontrolet()
        {
            try
            {
                string xmlfile = gn.XmlSurum;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlfile);
                XmlNodeList newXMLNodes = xdoc.SelectNodes("/program/versiyon");

                foreach (XmlNode newXMLNode in newXMLNodes)
                {
                    string deger = newXMLNode.Attributes["name"].Value.ToString();
                    if (LBL_VRSYN.Text != deger)
                    {
                        if (LBL_VRSYN.Text != "")
                        {
                            MessageBox.Show("Programınız Güncel Değildir.Lütfen Güncelleştiriniz");
                            System.Diagnostics.Process.Start("https://www.cm-yazilim.com.tr/sigorta/planor/serte");
                            Application.Exit();
                        }
                        else
                        {
                            Application.DoEvents();
                        }
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
            GC.Collect();
        }


        public void logkayit(string GelenKullaniciID, string GelenBayiID, string GirmeDurum, string GelenIP, string GelenSifre, string GelenKullaniciAdi)
        {

            try
            {
                string saat = DateTime.Now.ToString();
                string BilgisayarAdi = System.Environment.MachineName;
                string MacAdres = GetMacAddress();

                LogKaydet("", "", "", "", saat, BilgisayarAdi, MacAdres, GelenKullaniciID, GelenBayiID, GelenIP, GirmeDurum, GelenKullaniciAdi, GelenSifre);

            }
            catch
            {
                //throw
            }
            GC.Collect();
        }
        private void LogKaydet(string GelenUlke, string GelenIL, string GelenLatitude, string GelenLongitute, string GelenSaat, string GelenBilgisayarAdi, string GelenMacAdres, string GelenKullaniciID, string GelenBayiID, string GelenIp, string GelenDurum, string GelenKullanici, string GelenSifre)
        {

            if (GelenIp == "")
            {
                GelenIp = IpAl();
            }
            List<string> TabloAdlari = new List<string>();
            TabloAdlari.Add("UserID");
            TabloAdlari.Add("BayiID");
            TabloAdlari.Add("il");
            TabloAdlari.Add("ulke");
            TabloAdlari.Add("Latitude");
            TabloAdlari.Add("Longitude");
            TabloAdlari.Add("MacAdres");
            TabloAdlari.Add("Saat");
            TabloAdlari.Add("Gun");
            TabloAdlari.Add("Ay");
            TabloAdlari.Add("Yil");
            TabloAdlari.Add("PcAdi");
            TabloAdlari.Add("GirmeDurumu");
            TabloAdlari.Add("ip");
            TabloAdlari.Add("KullaniciAdi");
            TabloAdlari.Add("Sifre");
            TabloAdlari.Add("tur");

            ArrayList veriler = new ArrayList();
            veriler.Add(GelenKullaniciID);
            veriler.Add(GelenBayiID);
            veriler.Add(GelenIL);
            veriler.Add(GelenUlke);
            veriler.Add(GelenLatitude);
            veriler.Add(GelenLongitute);
            veriler.Add(GelenMacAdres);
            veriler.Add(GelenSaat);
            veriler.Add(DateTime.Now.Day.ToString());
            veriler.Add(DateTime.Now.Month.ToString());
            veriler.Add(DateTime.Now.Year.ToString());
            veriler.Add(GelenBilgisayarAdi);
            veriler.Add(GelenDurum);
            veriler.Add(GelenIp);
            veriler.Add(GelenKullanici);
            veriler.Add(GelenSifre);
            veriler.Add("Giriş");
            string sonuc = gn.db_kaydet(TabloAdlari, "t_logkayitlari", veriler);
            if (sonuc == "islem_tamam")
            {

            }
            GC.Collect();
        }


        private void g2LoginButon_Enter(object sender, EventArgs e)
        {
            g2LoginButon.ForeColor = Color.Black;
        }

        private void g2LoginButon_Leave(object sender, EventArgs e)
        {
            g2LoginButon.ForeColor = Color.White;
        }

        private void g2LoginButon_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Kullanici = txt_ka.Text;
            Properties.Settings.Default.Save();

            try
            {
                girisyap();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            //Firma sitesine falan gidecek
        }

        private void g2LoginButon_MouseLeave(object sender, EventArgs e)
        {
            g2LoginButon.ForeColor = Color.White;
        }

        private void cmb_versiyonyaz()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
                myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            LBL_VRSYN.Text = String.Concat(myVersion);
        }


        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void txt_ka_TextChanged(object sender, EventArgs e)
        {

        }

        private void g2LoginImage_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
