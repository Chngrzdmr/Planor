using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace Planor.Kalaslar
{
    class General
    {
        public DataSet DsCr = new DataSet();

        public DataTable DtCr = new DataTable();


        public string veri1 { get; set; }
        public string value1 { get; set; }


        /*
        public string SmsKullaniciAdi = "mertgrup";
        public string SmsSifre = "mert2018";
        */

        public string deger_Veri { get; set; }

        public string SelenXml = "http://www.cm-yazilim.com.tr//chngr-mt//SelenBilgiler.xml";

        public string chrmium_vers = "108.0.5359.0";


        ///// <summary>
        ///// SERTE ASİSTANS İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        ///// </summary>
        //public string acenteadi = "sertesigorta";

        //public string MySqlBaglanti = "SERVER=89.43.31.134;" + "DATABASE=sertesigorta;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //string MySqlBaglanti1 = "SERVER=89.43.31.134;" + "DATABASE=sertesigorta;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //public string XmlSirketler = "http://cm-yazilim.com.tr//chngr-mt//serte//556677ada2a55212xxx.xml";

        //public string XmlSurum = "http://cm-yazilim.com.tr//chngr-mt//serte//surum2.xml";

        //public string KareKod = "http://cm-yazilim.com.tr//chngr-mt//serte//KareKod.xml";

        ////public string SelenXml = "http://cm-yazilim.com.tr//chngr-mt//serte//SelenBilgiler.xml";

        ///// <summary>
        ///// SERTE ASİSTANS İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        ///// </summary>







        ///// <summary>
        ///// KİS SİGORTA İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        ///// </summary>
        //public string acenteadi = "kissigorta";

        //public string MySqlBaglanti = "SERVER=89.43.31.134;" + "DATABASE=kissigorta;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //string MySqlBaglanti1 = "SERVER=89.43.31.134;" + "DATABASE=kissigorta;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //public string XmlSirketler = "http://muhasebe.kissigorta.com//aqbokvar//22//33//44//66//556677ada2a55212xxx.xml";

        //public string XmlSurum = "http://muhasebe.kissigorta.com//Ayarlar//surumAnsimPlanor.xml";

        //public string KareKod = "http://muhasebe.kissigorta.com//Ayarlar//KareKod.xml";

        ////public string SelenXml = "http://muhasebe.kissigorta.com//Ayarlar//SelenBilgiler.xml";

        ///// <summary>
        ///// KİS SİGORTA İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        ///// </summary>










        /////// <summary>
        /////// SİTEM USTA İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR.
        /////// </summary>
        //public string acenteadi = "sitemustasigorta";

        //public string MySqlBaglanti = "SERVER=89.43.31.134;" + "DATABASE=sitemustasigorta;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //string MySqlBaglanti1 = "SERVER=89.43.31.134;" + "DATABASE=sitemustasigorta;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //public string XmlSirketler = "http://cm-yazilim.com.tr//chngr-mt//sitemusta//556677ada2a55212xxx.xml";

        //public string XmlSurum = "http://cm-yazilim.com.tr//chngr-mt//sitemusta//surum2.xml";

        //public string KareKod = "http://cm-yazilim.com.tr//chngr-mt//sitemusta//KareKod.xml";

        ////public string SelenXml = "http://cm-yazilim.com.tr//chngr-mt//sitemusta//SelenBilgiler.xml";
        /////// <summary>
        /////// SİTEM USTA
        ////İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        /////// </summary>






        /////// <summary>
        /////// TİMURLAR İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        /////// </summary>
        //public string acenteadi = "timurlarsigorta";

        //public string MySqlBaglanti = "SERVER=89.43.31.134;" + "DATABASE=timurlarekran;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //string MySqlBaglanti1 = "SERVER=89.43.31.134;" + "DATABASE=timurlarekran;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        //public string XmlSirketler = "http://muhasebe.timurlarsigorta.com//aqbokvar/22/33/44/66/556677ada2a55212xxx.xml";
        ////public string XmlSirketler = "http://www.cm-yazilim.com.tr//chngr-mt//556677ada2a55212xxx.xml";

        //public string XmlSurum = "http://www.cm-yazilim.com.tr//chngr-mt//surumtimurlar.xml";

        //public string KareKod = "http://muhasebe.timurlarsigorta.com/Ayarlar/KareKod.xml";
        ////public string KareKod = "http://cm-yazilim.com.tr//chngr-mt//KareKod.xml";

        ////public string SirketGirisSistem = "http://muhasebe.timurlarsigorta.com/Ayarlar/SirketXmlYapi.xml";
        ////public string BrowserTur = "http://muhasebe.timurlarsigorta.com/Ayarlar/BrowserTur.xml";

        ////public string SelenXml = "http://muhasebe.timurlarsigorta.com/Ayarlar/SelenBilgiler.xml";
        ////public string SelenXml = "http://www.cm-yazilim.com.tr//chngr-mt//SelenBilgiler.xml";
        /////// <summary>
        /////// TİMURLAR İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        /////// </summary>




        /// <summary>
        /// ANSİM İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        /// </summary>
        public string acenteadi = "ansimsigorta";

        public string MySqlBaglanti = "SERVER=89.43.31.134;" + "DATABASE=ansimsigorta;" + "UID=root;" + "PASSWORD=Aq3ublswt**//;";

        string MySqlBaglanti1 = "SERVER=185.9.157.55;" + "DATABASE=kissigorta;" + "UID=Wrenuser;" + "PASSWORD=wolkacena2019!;";

        //public string XmlSirketler = "http://muhasebe.ansimsigorta.com//aqbokvar//22//33//44//66//556677ada2a55212xxx.xml";
        public string XmlSirketler = "http://www.cm-yazilim.com.tr//chngr-mt//ansim//556677ada2a55212xxx.xml";

        public string XmlSurum = "http://muhasebe.ansimsigorta.com//Ayarlar//surumAnsimPlanor.xml";

        public string KareKod = "http://muhasebe.ansimsigorta.com//Ayarlar//KareKod.xml";

        //public string SelenXml = "http://muhasebe.ansimsigorta.com//Ayarlar//SelenBilgiler.xml";
        /// <summary>
        /// ANSİM İÇİN BİZİM WEB DEKİ XML LER İLE AYARLAR
        /// </summary>



        public string PlanorKullanicisi = "";
        public string _kullanici_id
        {
            get
            {
                return PlanorKullanicisi;
            }
            set
            {
                PlanorKullanicisi = value;
            }
        }



        public string en_son_kaydi_getir(string veri_tabani_adi, string okunacak, string kosul)
        {
            string okunan = "";
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select " + okunacak + " from " + veri_tabani_adi + " " + kosul + " ", con);
            //MessageBox.Show(@"Select " + okunacak + " from " + veri_tabani_adi + " " + kosul + " ");
            //Console.Beep();
            //Clipboard.SetText(@"Select " + okunacak + " from " + veri_tabani_adi + " " + kosul + " ");
            com.CommandTimeout = 0;
            Console.WriteLine(DateTime.Now.ToString());
            Console.WriteLine("Select " + okunacak + " from " + veri_tabani_adi + " " + kosul + " ");

            try
            {
                con.Open();
                MySqlDataReader oku = com.ExecuteReader();
                while (oku.Read())
                {
                    okunan = oku[okunacak].ToString();
                }

            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return okunan;
        }



        public string en_son_kaydi_getir2(string veri_tabani_adi, string okunacak, string kosul)
        {
            string okunan = "";
            MySqlConnection con = new MySqlConnection(MySqlBaglanti1);
            MySqlCommand com = new MySqlCommand(@"Select " + okunacak + " from " + veri_tabani_adi + " " + kosul + " ", con);

            try
            {
                con.Open();
                MySqlDataReader oku = com.ExecuteReader();
                while (oku.Read())
                {
                    okunan = oku[okunacak].ToString();
                }

            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return okunan;


        }

        public string db_kaydet(List<string> tablo_adlari, string veritabani_adi, ArrayList deger)
        {
            veri1 = "";
            value1 = "";
            for (int i = 0; i < tablo_adlari.Count; i++)
            {
                if (i == 0)
                {
                    veri1 += tablo_adlari[i].ToString();
                    value1 += "@" + tablo_adlari[i].ToString();

                }
                else
                {
                    veri1 += "," + tablo_adlari[i].ToString();
                    value1 += ",@" + tablo_adlari[i].ToString();
                }

            }
            string comString = @"Insert into " + veritabani_adi + " (" + veri1 + ") values (" + value1 + ")";

            MySqlConnection con = new MySqlConnection(MySqlBaglanti);

            MySqlCommand com = new MySqlCommand(comString, con);
            for (int i = 0; i < tablo_adlari.Count; i++)
            {

                deger_Veri = deger[i].ToString();
                string tablo_adi = tablo_adlari[i].ToString();

                if (tablo_adi != "BrutPrim" & tablo_adi != "NetPrim" & tablo_adi != "Komisyon" & tablo_adi != "Tanzim_Tarihi" & tablo_adi != "Baslangic_Tarihi" & tablo_adi != "Bitis_Tarihi" & tablo_adi != "maas" & tablo_adi != "borckesintisi" & tablo_adi != "muhasebe" & tablo_adi != "stopaj" & tablo_adi != "digerkesinti" & tablo_adi != "vergi" & tablo_adi != "Saat")
                {
                    veri1 = "@" + tablo_adlari[i].ToString();
                    com.Parameters.Add(new MySqlParameter(veri1, deger_Veri));

                }

                if (tablo_adi == "BrutPrim" || tablo_adi == "NetPrim" || tablo_adi == "Komisyon" || tablo_adi == "maas" || tablo_adi == "borckesintisi" || tablo_adi == "muhasebe" || tablo_adi == "stopaj" || tablo_adi == "digerkesinti" || tablo_adi == "vergi")
                {
                    com.Parameters.Add(new MySqlParameter("@" + tablo_adlari[i].ToString() + "", MySqlDbType.Decimal));
                    com.Parameters["@" + tablo_adlari[i].ToString() + ""].Value = deger_Veri;
                }
                if (tablo_adi == "Tanzim_Tarihi" || tablo_adi == "Baslangic_Tarihi" || tablo_adi == "Bitis_Tarihi")
                {
                    com.Parameters.Add(new MySqlParameter("@" + tablo_adlari[i].ToString() + "", MySqlDbType.Date));
                    com.Parameters["@" + tablo_adlari[i].ToString() + ""].Value = deger_Veri;
                }
                if (tablo_adi == "Saat")
                {
                    com.Parameters.Add(new MySqlParameter("@" + tablo_adlari[i].ToString() + "", MySqlDbType.Timestamp));
                    com.Parameters["@" + tablo_adlari[i].ToString() + ""].Value = deger_Veri;
                }
            }

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return "islem_tamam";
            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
                return "islem_hatali";
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public string combo_box_veri_getir(ComboBox cmb_box, string sql_sorgu, string gosterilecek_satir, string value_satir)
        {

            string durum = "0";
            DataTable DSet = new DataTable();

            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select " + sql_sorgu + "", con);

            try
            {

                con.Open();
                MySqlDataAdapter oku = new MySqlDataAdapter(com);

                oku.Fill(DSet);
                if (DSet.Rows.Count > 0)
                {
                    cmb_box.DataSource = DSet;
                    cmb_box.DisplayMember = gosterilecek_satir;
                    cmb_box.ValueMember = value_satir;
                }

                durum = "1";
            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
            }
            finally
            {
                con.Close();

            }
            return durum;
        }
        public string rad_combo_box_veri_getir(ComboBox cmb_box, string sql_sorgu, string gosterilecek_satir, string value_satir)
        {

            string durum = "0";
            DataTable DSet = new DataTable();

            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select " + sql_sorgu + "", con);

            try
            {

                con.Open();
                MySqlDataAdapter oku = new MySqlDataAdapter(com);

                oku.Fill(DSet);
                if (DSet.Rows.Count > 0)
                {
                    cmb_box.DataSource = DSet;
                    cmb_box.DisplayMember = gosterilecek_satir;
                    cmb_box.ValueMember = value_satir;
                }

                durum = "1";
            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
            }
            finally
            {
                con.Close();

            }
            return durum;
        }

        public string grid_view_getir(string sql_sorgu, DataGridView gd_data)
        {
            BindingSource bd_source = new BindingSource();
            string durum = "0";
            DataTable DSet = new DataTable();
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select " + sql_sorgu + "", con);
            try
            {
                con.Open();
                MySqlDataAdapter oku = new MySqlDataAdapter(com);
                oku.Fill(DSet);
                gd_data.DataSource = bd_source;
                bd_source.DataSource = DSet;
                durum = "1";
            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
            }
            finally
            {
                con.Close();
            }
            return durum;
        }


        public int toplam_sayi_getir(string db_adi, string okunacak, string kosul)
        {
            int sayi = 0;
            int okunan_adet = 0;
            int gelme_sayisi = 0;
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select " + okunacak + " from " + db_adi + " " + kosul + " ", con);


            try
            {
                con.Open();
                MySqlDataReader oku = com.ExecuteReader();
                while (oku.Read())
                {
                    gelme_sayisi++;
                    okunan_adet = int.Parse(oku[okunacak].ToString());
                    sayi += okunan_adet;
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

            return sayi;
        }

        public int eklenen_sayisi_getir(string db_adi, string okunacak, string kosul)
        {
            //int sayi = 0;
            //int okunan_adet = 0;
            int gelme_sayisi = 0;
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"Select " + okunacak + " from " + db_adi + " " + kosul + " ", con);


            try
            {
                con.Open();
                MySqlDataReader oku = com.ExecuteReader();
                while (oku.Read())
                {
                    gelme_sayisi++;
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

            return gelme_sayisi;
        }

        public string db_duzenle(List<string> tablo_adlari, string veritabani_adi, ArrayList deger, string parametre, string parametre_deger)
        {
            veri1 = "";
            for (int i = 0; i < tablo_adlari.Count; i++)
            {
                if (i == 0)
                {
                    veri1 += tablo_adlari[i].ToString() + "=@" + tablo_adlari[i].ToString();


                }
                else
                {
                    veri1 += "," + tablo_adlari[i].ToString() + "=@" + tablo_adlari[i].ToString();
                }

            }
            string comString = @"update " + veritabani_adi + " set " + veri1 + " where " + parametre + "=@" + parametre + "";
            Console.WriteLine("update " + veritabani_adi + " set " + veri1 + " where " + parametre + "=@" + parametre + "");
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(comString, con);
            for (int i = 0; i < tablo_adlari.Count; i++)
            {

                deger_Veri = deger[i].ToString();
                string tablo_adi = tablo_adlari[i].ToString();
                //BirimFiyati
                if (tablo_adi != "Saat" & tablo_adi != "Tarih" & tablo_adi != "Toplam" & tablo_adi != "Bedel" & tablo_adi != "BirimFiyati" & tablo_adi != "NetToplam" & tablo_adi != "ToplamKDV" & tablo_adi != "indirim" & tablo_adi != "ToplamTutar")
                {
                    veri1 = "@" + tablo_adlari[i].ToString();
                    com.Parameters.Add(new MySqlParameter(veri1, deger_Veri));
                }

                if (tablo_adi == "Toplam" || tablo_adi == "Bedel" || tablo_adi == "BirimFiyati" || tablo_adi == "NetToplam" || tablo_adi == "ToplamKDV" || tablo_adi == "indirim" || tablo_adi == "ToplamTutar")
                {
                    com.Parameters.Add(new MySqlParameter("@" + tablo_adlari[i].ToString() + "", MySqlDbType.Decimal));
                    com.Parameters["@" + tablo_adlari[i].ToString() + ""].Value = deger_Veri;
                }
                if (tablo_adi == "Tarih")
                {
                    com.Parameters.Add(new MySqlParameter("@" + tablo_adlari[i].ToString() + "", MySqlDbType.Date));
                    com.Parameters["@" + tablo_adlari[i].ToString() + ""].Value = deger_Veri;
                }
                if (tablo_adi == "Saat")
                {
                    com.Parameters.Add(new MySqlParameter("@" + tablo_adlari[i].ToString() + "", MySqlDbType.Time));
                    com.Parameters["@" + tablo_adlari[i].ToString() + ""].Value = deger_Veri;
                }
            }

            string parametre_veri = parametre_deger;
            string parametre_isim = "@" + parametre;

            com.Parameters.Add(new MySqlParameter(parametre_isim, parametre_veri));
            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return "islem_tamam";
            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
                return "islem_hatali";
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public string db_sil(string veritabani_adi, string deger)
        {
            string comString = @"Delete from " + veritabani_adi + " where id=@id";
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(comString, con);

            com.Parameters.Add(new MySqlParameter("@id", deger));

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return "işlem tamamlandı";

            }
            catch
            {
                return "işlem hatalı";
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public string db_sil_deger(string veritabani_adi, string deger, string Parametre)
        {
            string comString = @"Delete from " + veritabani_adi + " where " + Parametre + "=" + deger + "";
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(comString, con);

            com.Parameters.Add(new MySqlParameter("@" + Parametre + "", deger));

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                return "işlem tamamlandı";
            }
            catch
            {
                return "işlem hatalı";
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }



        }

        public string db_duzenle_2(string veritabaniadi, string tablo_adi, string deger, string kosul_adi, string kosul_deger)
        {
            string sonuc = "";
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            string comString = @"Update " + veritabaniadi + " set " + tablo_adi + "=@" + tablo_adi + " where " + kosul_adi + "=@" + kosul_adi + "";
            MySqlCommand com = new MySqlCommand(comString, con);
            com.Parameters.Add(new MySqlParameter("@" + kosul_adi + "", kosul_deger));
            com.Parameters.Add(new MySqlParameter("@" + tablo_adi + "", deger));
            try
            {
                con.Open();
                com.ExecuteNonQuery();
                sonuc = "islem_tamam";
            }
            catch (Exception exp)
            {
                sonuc = "işlem hatalı";
                string hata = exp.ToString();
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return sonuc;
        }

        public int adet_getir(string veri_tabani_adi, string okunacak, string kosul)
        {
            int okunan = 0;
            MySqlConnection con = new MySqlConnection(MySqlBaglanti);
            MySqlCommand com = new MySqlCommand(@"SELECT COUNT(" + okunacak + ")  from " + veri_tabani_adi + " " + kosul + " ", con);
            com.CommandTimeout = 0;
            try
            {
                con.Open();
                object scalar = com.ExecuteScalar();
                okunan = int.Parse(scalar.ToString());
            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return okunan;
        }


        //////////// TXT E LOG YAZMA FONKSİYONU //////////
        public async Task LocalLoglaAsync(string logkullanici, string logneden, string logmetin)
        {
            string path = Directory.GetCurrentDirectory() + "/LocalLoog.txt";
            string loog = "";

            if (!File.Exists(path))
            {
                using (StreamWriter swlg = File.CreateText(path))
                {
                    loog = (DateTime.Now.ToString() + ":  " + logkullanici + "  ||  Log Dosyası Oluşturuldu.");
                    swlg.WriteLine(loog);
                    new SistemForm().DurumLBL.Text = loog;
                    await Task.Delay(1000);
                    new SistemForm().DurumLBL.Text = "";
                }
            }

            using (StreamWriter swlg = File.AppendText(path))
            {
                loog = (DateTime.Now.ToString() + ":  " + logkullanici + "  ||  " + logneden + "  ||  " + logmetin);
                swlg.WriteLine(loog);
                new SistemForm().DurumLBL.Text = loog;
                await Task.Delay(1000);
                new SistemForm().DurumLBL.Text = "";
            }
        }






        //////////// şifreleme ve şifre çözme fonksiyonları //////////
        public string themasterkey = "ChngrMFO-1453"; //esasanahtar

        public string _Encrypt(string ToEncrypt, bool useHasing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(ToEncrypt);

            if (useHasing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(themasterkey));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(themasterkey);
            }
            TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tDes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tDes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        public string _Decrypt(string cypherString, bool useHasing)
        {
            byte[] keyArray;
            byte[] toDecryptArray = Convert.FromBase64String(cypherString.Replace(' ', '+'));


            if (useHasing)
            {
                MD5CryptoServiceProvider hashmd = new MD5CryptoServiceProvider();
                keyArray = hashmd.ComputeHash(UTF8Encoding.UTF8.GetBytes(themasterkey));
                hashmd.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(themasterkey);
            }
            TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tDes.CreateDecryptor();
            try
            {
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                tDes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        ////burası son
    }
}
