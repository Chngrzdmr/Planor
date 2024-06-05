using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Planor.Kalaslar
{
    class General
    {
        public DataSet DsCr { get; set; }
        public DataTable DtCr { get; set; }

        public string Veri1 { get; set; }
        public string Value1 { get; set; }

        public string Deger_Veri { get; set; }

        public string SelenXml { get; set; }
        public string Chrmium_vers { get; set; }

        public string Acenteadi { get; set; }
        public string MySqlBaglanti { get; set; }

        public string PlanorKullanicisi { get; set; }

        public General()
        {
            DsCr = new DataSet();
            DtCr = new DataTable();
            SelenXml = "http://www.cm-yazilim.com.tr//chngr-mt//SelenBilgiler.xml";
            Chrmium_vers = "108.0.5359.0";
            MySqlBaglanti = GetConnectionString("MySqlConnection");
            Acenteadi = "ansimsigorta";
        }

        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public string En_Son_Kaydi_Getir(string veri_tabani_adi, string okunacak, string kosul)
        {
            string okunan = "";
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand($@"Select {okunacak} from {veri_tabani_adi} {kosul}", con);
                try
                {
                    con.Open();
                    MySqlDataReader oku = com.ExecuteReader();
                    while (oku.Read())
                    {
                        okunan = oku[okunacak].ToString();
                    }
                }
                catch (MySqlException exp)
                {
                    LogError(exp);
                }
            }
            return okunan;
        }

        public string En_Son_Kaydi_Getir2(string veri_tabani_adi, string okunacak, string kosul)
        {
            string okunan = "";
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti1))
            {
                MySqlCommand com = new MySqlCommand($@"Select {okunacak} from {veri_tabani_adi} {kosul}", con);
                try
                {
                    con.Open();
                    MySqlDataReader oku = com.ExecuteReader();
                    while (oku.Read())
                    {
                        okunan = oku[okunacak].ToString();
                    }
                }
                catch (MySqlException exp)
                {
                    LogError(exp);
                }
            }
            return okunan;
        }

        public string Db_Kaydet(List<string> tablo_adlari, string veritabani_adi, ArrayList deger)
        {
            string comString = "";
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                comString = CreateInsertQuery(tablo_adlari, veritabani_adi);
                MySqlCommand com = new MySqlCommand(comString, con);
                for (int i = 0; i < tablo_adlari.Count; i++)
                {
                    string tablo_adi = tablo_adlari[i].ToString();
                    string veri1 = "@" + tablo_adi;
                    if (tablo_adi != "BrutPrim" && tablo_adi != "NetPrim" && tablo_adi != "Komisyon" && tablo_adi != "Tanzim_Tarihi" && tablo_adi != "Baslangic_Tarihi" && tablo_adi != "Bitis_Tarihi" && tablo_adi != "maas" && tablo_adi != "borckesintisi" && tablo_adi != "muhasebe" && tablo_adi != "stopaj" && tablo_adi != "digerkesinti" && tablo_adi != "vergi" && tablo_adi != "Saat")
                    {
                        com.Parameters.Add(new MySqlParameter(veri1, deger[i]));
                    }
                    if (tablo_adi == "BrutPrim" || tablo_adi == "NetPrim" || tablo_adi == "Komisyon" || tablo_adi == "maas" || tablo_adi == "borckesintisi" || tablo_adi == "muhasebe" || tablo_adi == "stopaj" || tablo_adi == "digerkesinti" || tablo_adi == "vergi")
                    {
                        com.Parameters.Add(new MySqlParameter("@" + tablo_adi, MySqlDbType.Decimal));
                        com.Parameters["@" + tablo_adi].Value = deger[i];
                    }
                    if (tablo_adi == "Tanzim_Tarihi" || tablo_adi == "Baslangic_Tarihi" || tablo_adi == "Bitis_Tarihi")
                    {
                        com.Parameters.Add(new MySqlParameter("@" + tablo_adi, MySqlDbType.Date));
                        com.Parameters["@" + tablo_adi].Value = deger[i];
                    }
                    if (tablo_adi == "Saat")
                    {
                        com.Parameters.Add(new MySqlParameter("@" + tablo_adi, MySqlDbType.Timestamp));
                        com.Parameters["@" + tablo_adi].Value = deger[i];
                    }
                }
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "islem_tamam";
                }
                catch (MySqlException exp)
                {
                    LogError(exp);
                    return "islem_hatali";
                }
            }
        }

        private string CreateInsertQuery(List<string> tablo_adlari, string veritabani_adi)
        {
            string veri1 = "";
            string value1 = "";
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
                    value1 += "," + "@" + tablo_adlari[i].ToString();
                }
            }
            return $@"Insert into {veritabani_adi} ({veri1}) values ({value1})";
        }

        public string Combo_Box_Veri_Getir(ComboBox cmb_box, string sql_sorgu, string gosterilecek_satir, string value_satir)
        {
            string durum = "0";
            DataTable DSet = new DataTable();
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand(sql_sorgu, con);
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
                catch (MySqlException exp)
                {
                    LogError(exp);
                }
            }
            return durum;
        }

        public string Grid_View_Getir(string sql_sorgu, DataGridView gd_data)
        {
            BindingSource bd_source = new BindingSource();
            string durum = "0";
            DataTable DSet = new DataTable();
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand(sql_sorgu, con);
                try
                {
                    con.Open();
                    MySqlDataAdapter oku = new MySqlDataAdapter(com);
                    oku.Fill(DSet);
                    gd_data.DataSource = bd_source;
                    bd_source.DataSource = DSet;
                    durum = "1";
                }
                catch (MySqlException exp)
                {
                    LogError(exp);
                }
            }
            return durum;
        }

        public int Toplam_Sayi_Getir(string db_adi, string okunacak, string kosul)
        {
            int sayi = 0;
            int okunan_adet = 0;
            int gelme_sayisi = 0;
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand($@"Select {okunacak} from {db_adi} {kosul}", con);
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
                catch (MySqlException exp)
                {
                    LogError(exp);
                }
            }
            return sayi;
        }

        public int Eklenen_Sayisi_Getir(string db_adi, string okunacak, string kosul)
        {
            int gelme_sayisi = 0;
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand($@"Select {okunacak} from {db_adi} {kosul}", con);
                try
                {
                    con.Open();
                    MySqlDataReader oku = com.ExecuteReader();
                    while (oku.Read())
                    {
                        gelme_sayisi++;
                    }
                }
                catch (MySqlException exp)
                {
                    LogError(exp);
                }
            }
            return gelme_sayisi;
        }

        public string Db_Duzenle(List<string> tablo_adlari, string veritabani_adi, ArrayList deger, string parametre, string parametre_deger)
        {
            string veri1 = "";
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                veri1 = CreateUpdateQuery(tablo_adlari, veritabani_adi, parametre, parametre_deger);
                MySqlCommand com = new MySqlCommand(veri1, con);
                for (int i = 0; i < tablo_adlari.Count; i++)
                {
                    string tablo_adi = tablo_adlari[i].ToString();
                    string veri2 = "@" + tablo_adi;
                    if (tablo_adi != "Saat" && tablo_adi != "Tarih" && tablo_adi != "Toplam" && tablo_adi != "Bedel" && tablo_adi != "BirimFiyati" && tablo_adi != "NetToplam" && tablo_adi != "ToplamKDV" && tablo_adi != "indirim" && tablo_adi != "ToplamTutar")
                    {
                        com.Parameters.Add(new MySqlParameter(veri2, deger[i]));
                    }
                    if (tablo_adi == "Toplam" || tablo_adi == "Bedel" || tablo_adi == "BirimFiyati" || tablo_adi == "NetToplam" || tablo_adi == "ToplamKDV" || tablo_adi == "indirim" || tablo_adi == "ToplamTutar")
                    {
                        com.Parameters.Add(new MySqlParameter("@" + tablo_adi, MySqlDbType.Decimal));
                        com.Parameters["@" + tablo_adi].Value = deger[i];
                    }
                    if (tablo_adi == "Tarih")
                    {
                        com.Parameters.Add(new MySqlParameter("@" + tablo_adi, MySqlDbType.Date));
                        com.Parameters["@" + tablo_adi].Value = deger[i];
                    }
                    if (tablo_adi == "Saat")
                    {
                        com.Parameters.Add(new MySqlParameter("@" + tablo_adi, MySqlDbType.Time));
                        com.Parameters["@" + tablo_adi].Value = deger[i];
                    }
                }
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "islem_tamam";
                }
                catch (MySqlException exp)
                {
                    LogError(exp);
                    return "islem_hatali";
                }
            }
        }

        private string CreateUpdateQuery(List<string> tablo_adlari, string veritabani_adi, string parametre, string parametre_deger)
        {
            string veri1 = "";
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
            return $@"update {veritabani_adi} set {veri1} where {parametre}=@{parametre}";
        }

        public string Db_Sil(string veritabani_adi, string deger)
        {
            string comString = $@"Delete from {veritabani_adi} where id={deger}";
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand(comString, con);
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
            }
        }

        public string Db_Sil_Deger(string veritabani_adi, string deger, string Parametre)
        {
            string comString = $@"Delete from {veritabani_adi} where {Parametre}={deger}";
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                MySqlCommand com = new MySqlCommand(comString, con);
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
            }
        }

        public string Db_Duzenle_2(string veritabaniadi, string tablo_adi, string deger, string kosul_adi, string kosul_deger)
        {
            string sonuc = "";
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti))
            {
                string comString = $@"Update {veritabaniadi} set {tablo_adi}=@
