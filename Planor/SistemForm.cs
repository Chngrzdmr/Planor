using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Patagames.Ocr;
using Planor.Kalaslar;
using Planor.Sayfalar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Planor
{
    public partial class SistemForm : Form
    {
        // bu blok sayesinde form style borderles bile olsa taskbar üzerinden minimize and maximize olabiliyor
        //--------------------------------------------------------------------------------------------------------
        #region borderles_minimize_maximize
        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        } 
        #endregion
        //--------------------------------------------------------------------------------------------------------

        General gn = new General();

        private IWebDriver ie_driver;
        private IWebDriver chrm_driver;
        public IWebDriver driver;

        ChromeDriverService chrm_service = ChromeDriverService.CreateDefaultService();
        ChromeOptions chrm_opt = new ChromeOptions();
        Proxy chrm_pr = new Proxy();

        InternetExplorerDriverService ie_service = InternetExplorerDriverService.CreateDefaultService();
        InternetExplorerOptions ie_opt = new InternetExplorerOptions();
        Proxy ie_pr = new Proxy();

        public Screen[] screens = Screen.AllScreens;
        public int ekranno;

        public SistemForm()
        {
            InitializeComponent();

            //hangi motörde açılacak
            if (System.Configuration.ConfigurationSettings.AppSettings.Get("Motitor2de") == "1" && screens.Count() > 1)
            {
                if (ekranno < screens.Count())
                {
                    ekranno = ekranno + 1;
                }
                else
                {
                    ekranno = 0;
                }
                ScreenButtonX.Text = ekranno.ToString();
                this.Size = screens[ekranno].WorkingArea.Size;
            }
            else
            {
                if (ekranno < screens.Count())
                {
                    ekranno = ekranno + 1;
                }
                else
                {
                    ekranno = ekranno - 1;
                }
                ekranno = 0;
                ScreenButtonX.Text = ekranno.ToString();
                this.Size = screens[ekranno].WorkingArea.Size;
            }
        }

        string sirketadi = "";
        string hangiDriver = "";
        string KullaniciAdi = "";
        string smsnumberno = "";
        string Sifre = "";
        string acente_kodu = "";
        string ip = "";
        string port = "";
        string iemitarayici = "";
        string ip_durumu = "";
        string link = "";
        string sirketgirmedurumu = "";
        const string passphrase = "KorOz89**@123";

        string GelenKullaniciAdi = "";
        string anahtar = "";

        bool ipdegisti = false;

        [Obsolete]
        private void SistemForm_Load(object sender, EventArgs e)
        {
            Trayyy1.Visible = true;

            try
            {
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData("http://cm-yazilim.com.tr//sigorta//planor//" + gn.acenteadi + "-logo.png");
                MemoryStream ms = new MemoryStream(bytes);
                Image logoimg = Image.FromStream(ms);

                g2LogoBTN.BackgroundImage = logoimg;
                g2LogoBTN.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception)
            {
                //hata
                Trayyy1.ShowBalloonTip(100, "PLANÖR", "Şirket logonuz bulunamadı. Lütfen logo yüklemesi talep ediniz !!!", ToolTipIcon.None);

                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData("http://cm-yazilim.com.tr//sigorta//planor//planor-logo.png");
                MemoryStream ms = new MemoryStream(bytes);
                Image logoimg = Image.FromStream(ms);

                g2LogoBTN.BackgroundImage = logoimg;
                g2LogoBTN.BackgroundImageLayout = ImageLayout.Stretch;

            }

            GorunumDegistir();
            setFormLocation(this, screens[ekranno]);

            if (screens.Count() > 1)
            {
                ScreenButtonX.Visible = true;
            }
            else ScreenButtonX.Visible = false;


            tableLayoutPanel1.Dock = DockStyle.Fill;
            //tableLayoutPanel1.Visible = false;

            //yonetici DEĞİL İSE gizlenecekler
            if (lbl_tur.Text != "1")
            {
                g2anaMenuBTN5.Visible = false;
            }


            //this.Location = new Point(0, 0);
            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            gn.grid_view_getir(" * from t_sirketler order by id asc", dgv_sirketler);
            LblKullaniciAdi.Text = gn.en_son_kaydi_getir("t_kullanicilar", "adi", "where id =" + lbl_id.Text + "");

            //meteye sor bu neden?
            Label.CheckForIllegalCrossThreadCalls = false;

            sirketekle();
            Application.DoEvents();

            //PanelSlider.Controls.Add(new AnaSayfa2());
            PanelSlider.Controls.Add(new Ayarlar());
            PanelSlider.Controls.Add(new HizliTeklif());
            PanelSlider.Controls.Add(new Hakkimizda());
            PanelSlider.Controls.Add(new Yonetici());
            PanelSlider.Controls.Add(new HizliAraclar());


            new Ayarlar().Size = PanelSlider.Size;
            new HizliTeklif().Size = PanelSlider.Size;
            new Hakkimizda().Size = PanelSlider.Size;
            new Yonetici().Size = PanelSlider.Size;
            new HizliAraclar().Size = PanelSlider.Size;

            //PanelSlider.Controls.Find("AnaSayfa2", false)[0].SendToBack();
            PanelSlider.Controls.Find("HizliTeklif", false)[0].SendToBack();
            PanelSlider.Controls.Find("Ayarlar", false)[0].SendToBack();
            PanelSlider.Controls.Find("Hakkimizda", false)[0].SendToBack();
            PanelSlider.Controls.Find("Yonetici", false)[0].SendToBack();
            PanelSlider.Controls.Find("HizliAraclar", false)[0].SendToBack();



            Chrm_Driver_Olustur();
            ie_Driver_Olustur();
        }



        static bool is64BitProcess = (IntPtr.Size == 8);
        static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );


        //---------------------------------------------------------------------------------------------
        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void Chrm_Driver_Olustur()
        {
            string directory = @"C://CMSigorta/chrm/chrome-win";
            string directory2 = @"C://CMSigorta/chrm";
            string chrm_versiyonu = "";

            // Klasör içindeki tüm dosyaları al
            if (Directory.Exists(directory))
            {
                string[] allFiles = Directory.GetFiles(directory);

                // Adında manifest geçen dosyaları bul
                List<string> manifestFiles = new List<string>();
                foreach (string file in allFiles)
                {
                    if (file.Contains("manifest"))
                    {
                        manifestFiles.Add(file);
                    }
                }

                if (manifestFiles.Count > 0)
                {
                    chrm_versiyonu = manifestFiles[0].Substring((manifestFiles[0].IndexOf("win")) + 4, (manifestFiles[0].IndexOf("manifest")) - ((manifestFiles[0].IndexOf("win")) + 4) - 1);
                    //Console.WriteLine("Manifest dosyasının tam adı: " + manifestFiles[0]);
                    //MessageBox.Show("___" + chrm_versiyonu + "___");
                }
            }

            try
            {
                if (Directory.Exists(directory) && chrm_versiyonu != gn.chrmium_vers)
                {
                    Directory.Delete(directory2, true);
                }
            }
            catch (Exception ex)
            {

                //throw;
            }


            //if (!Directory.Exists(Directory.GetCurrentDirectory() + "/chrm"))
            if (!File.Exists(@"C://CMSigorta/chrm/chrome-win/chrome-cms.exe") || chrm_versiyonu != gn.chrmium_vers)
            {
                tableLayoutPanel1.Enabled = false;

                string chrmlink = "http://cm-yazilim.com.tr//chngr-mt//planor_chromium.zip";
                // https://chromium.cypress.io/
                // Chromium eski sürümlerini indirme adresi

                if (InternalCheckIsWow64())
                {
                    chrmlink = "http://cm-yazilim.com.tr//chngr-mt//planor_chromium.zip";
                }
                else
                {
                    //chrmlink = "http://cm-yazilim.com.tr//chngr-mt//planor_chromium-x86.zip";
                    chrmlink = "http://cm-yazilim.com.tr//chngr-mt//planor_chromium.zip";
                }


                //bu dosya yoksa chromium yoktur zip olarak ftp den indirilecek  .
                try
                {
                    DurumLBL.Text = "PROGRAM İLK KULLANIM İÇİN GEREKLİ EKSİK DOSYALARI İNDİRİYOR, LÜTFEN BEKLEYİNİZ...";

                    WebClient wc = new WebClient();
                    wc.DownloadFileCompleted += wc_AsyncCompletedEventHandler;
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileAsync(
                        // Param1 = Link of file                     
                        new Uri(chrmlink),
                        // Param2 = Path to save
                        Directory.GetCurrentDirectory() + "/chrm.zip"
                    );
                }
                catch (Exception ex)
                {
                    string hata = ex.ToString();
                    gn.LocalLoglaAsync(isimLBL.Text, "Chrome Driver ilk kullanım için indirilmeye çalışılırken hata oluştu.", hata);
                }
                //Thread.Sleep(1000);
            }
            else
            {
                chrm_opt.BinaryLocation = @"C://CMSigorta/chrm/chrome-win/chrome-cms.exe";
            }

            chrm_opt.AddArguments("--test-type");
            chrm_opt.AddArguments("--disable-dev-tools");
            //chrm_opt.AddArguments("--kiosk"); //bu satır f12 iptal eder ama pencereyi full screen yapıyor yaramaz
            //chrm_opt.AddArguments("--single-process"); //bu satır bazı chrome - windows arası ilişkilerde sorun yarattığı için iptal edildi.
            chrm_opt.AddArguments("--start-maximized");
            chrm_opt.AddArguments("--js-flags=--expose-gc");
            chrm_opt.AddArguments("--enable-precise-memory-info");
            chrm_opt.AddArguments("--disable-popup-blocking");
            chrm_opt.AddArguments("--disable-default-apps");
            chrm_opt.AddArguments("--disable-plugins-discovery");
            chrm_opt.AddArguments("--disable-extensions");
            chrm_opt.AddArguments("--disable-infobars");
            chrm_opt.AddArguments("--mute-audio");
            chrm_opt.AddArguments("--disable-gpu");
            chrm_opt.AddArguments("--disable-gpu-early-init");
            chrm_opt.AddArguments("--disable-gpu-compositing");
            chrm_opt.AddArguments("--disable-features=TranslateUI,ExtensionsToolbarMenu");
            chrm_opt.AddArguments("--disable-software-rasterizer");
            //chrm_opt.AddArguments("--disable-web-security");
            chrm_opt.AddArguments("--process-per-site");
            chrm_opt.AddArguments("--disable-dev-shm-usage");
            //chrm_opt.AddArguments("--allow-popups-during-page-unload");

            //opt.AddArguments("--disable-dev-shm-usage"); // overcome limited resource problems
            // opt.AddArguments("--no-sandbox");
            // opt.AddArguments("--remote-debugging-port=0");
            // opt.AddArguments("--disable-extensions-file-access-check");
            // opt.AddArguments("--log-level=3");
            // opt.AddArguments("--silent");
            // opt.AddArguments("--disable-extensions-except");
            // // opt.AddArguments("--blink-settings=scriptEnabled=false");

            chrm_opt.AddExcludedArguments("enable-automation");
            chrm_opt.AddAdditionalCapability("useAutomationExtension", false);
            chrm_opt.AddUserProfilePreference("credentials_enable_service", false);
            chrm_opt.AddUserProfilePreference("profile.password_manager_enabled", false);


            //chrm_opt.LeaveBrowserRunning = true;

            //opt.AddUserProfilePreference("extensions", false);

            //("useAutomationExtension", false);
            //opt.AddUserProfilePreference("useAutomationExtension", false);
            //  opt.AddAdditionalCapability("chrome.switches", ("--disable-extensions"));

            chrm_service.SuppressInitialDiagnosticInformation = true;
            chrm_service.HideCommandPromptWindow = true;

        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void ie_Driver_Olustur()
        {
            ie_service.SuppressInitialDiagnosticInformation = true;
            ie_service.HideCommandPromptWindow = true;
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        public string decrypt(string message)
        {
            try
            {
                byte[] results;
                UTF8Encoding utf8 = new UTF8Encoding();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] deskey = md5.ComputeHash(utf8.GetBytes(passphrase));
                TripleDESCryptoServiceProvider desalg = new TripleDESCryptoServiceProvider();
                desalg.Key = deskey;
                desalg.Mode = CipherMode.ECB;
                desalg.Padding = PaddingMode.PKCS7;
                byte[] decrypt_data = Convert.FromBase64String(message);
                try
                {
                    ICryptoTransform decryptor = desalg.CreateDecryptor();
                    results = decryptor.TransformFinalBlock(decrypt_data, 0, decrypt_data.Length);
                }
                finally
                {
                    desalg.Clear();
                    md5.Clear();
                }
                return utf8.GetString(results);
            }
            catch
            {
                return message;
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void LogKaydet(string KullaniciIDG, string SirketAdiG, string SirketIDG)
        {
            string saat = DateTime.Now.ToString();

            List<string> TabloAdlari = new List<string>();
            TabloAdlari.Add("KullaniciID");
            TabloAdlari.Add("KullaniciAdi");
            TabloAdlari.Add("SirketAdi");
            TabloAdlari.Add("SirketID");

            TabloAdlari.Add("Saat");
            TabloAdlari.Add("Gun");
            TabloAdlari.Add("Ay");
            TabloAdlari.Add("Yil");
            TabloAdlari.Add("tur");

            ArrayList veriler = new ArrayList();
            veriler.Add(KullaniciIDG);
            veriler.Add(LblKullaniciAdi.Text);
            veriler.Add(SirketAdiG);
            veriler.Add(SirketIDG);

            veriler.Add(saat);
            veriler.Add(DateTime.Now.Day.ToString());
            veriler.Add(DateTime.Now.Month.ToString());
            veriler.Add(DateTime.Now.Year.ToString());
            veriler.Add("giriş");

            string sonuc = gn.db_kaydet(TabloAdlari, "t_logkayitlari_sirket", veriler);
            if (sonuc == "islem_tamam")
            {

            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        [Obsolete]
        public void sirketekle()
        {
            //var columnCount = tableLayoutPanel1.Width / 180;
            var columnCount = tableLayoutPanel1.Width / (screens[ekranno].Bounds.Width / 11);
            //var rowCount = tableLayoutPanel1.Height / 180;
            var rowCount = tableLayoutPanel1.Height / (screens[ekranno].Bounds.Height / 6);
            tableLayoutPanel1.ColumnCount = columnCount;
            tableLayoutPanel1.RowCount = rowCount;
            tableLayoutPanel1.Visible = false;

            DataTable DSet = new DataTable();

            MySqlConnection con = new MySqlConnection(gn.MySqlBaglanti);
            string kullanici_id = gn.en_son_kaydi_getir("t_kullanicilar", "id", "where adi='" + LblKullaniciAdi.Text + "'");
            MySqlCommand com = new MySqlCommand(@"Select * from t_kullanici_sirketler where KullaniciID='" + kullanici_id + "' order by Adi asc", con);

            int sayac = 0;
            //MessageBox.Show(LblKullaniciAdi.Text);
            try
            {

                con.Open();
                MySqlDataReader oku2 = com.ExecuteReader();

                HizliAraclar hs = new HizliAraclar();

                while (oku2.Read())
                {
                    string sirket_adi = oku2["Adi"].ToString();

                    if (screens[ekranno].Bounds.Height < 960)
                    {
                        tableLayoutPanel1.Padding = new Padding(10, 0, 0, 0);
                    }

                    Guna2TileButton sirketButon = new Guna2TileButton();
                    sirketButon.Width = (tableLayoutPanel1.Width / columnCount) - (columnCount + 3);
                    sirketButon.Height = (tableLayoutPanel1.Height / rowCount) - (rowCount + 3);
                    //sirketButon.Text = sirket_adi.ToUpper();
                    sirketButon.Text = sirket_adi;

                    sirketButon.FillColor = Color.White;
                    //sirketButon.FillColor2 = Color.White;
                    sirketButon.ForeColor = Color.Black;
                    sirketButon.BackColor = Color.White;
                    sirketButon.UseTransparentBackground = true;
                    sirketButon.Image = (Image)Properties.Resources.ResourceManager.GetObject(sirket_adi.Substring(0, sirket_adi.IndexOf(" ")).ToLower() + "-logo");
                    sirketButon.ImageSize = new Size((screens[ekranno].Bounds.Width / 14), (Screen.PrimaryScreen.Bounds.Height / 10));
                    sirketButon.ImageOffset = new Point(0, 40);

                    //sirketButon.TextOffset = new Point(0, 30);
                    sirketButon.ShadowDecoration.BorderRadius = 0;
                    sirketButon.ShadowDecoration.Color = Color.DimGray;
                    sirketButon.ShadowDecoration.Depth = 20;
                    sirketButon.ShadowDecoration.Enabled = true;
                    sirketButon.HoverState.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
                    sirketButon.ShadowDecoration.Parent = sirketButon;
                    if (screens[ekranno].Bounds.Width > 1600)
                    {
                        sirketButon.ShadowDecoration.Shadow = new Padding(8, 0, 8, 8);
                        sirketButon.Font = new Font("Verdana", 10, FontStyle.Bold);
                        sirketButon.TextOffset = new Point(0, 20);
                        sirketButon.HoverState.Font = new Font("Verdana", 10, ((FontStyle)((FontStyle.Bold))), GraphicsUnit.Point, ((byte)(162)));
                    }
                    else
                    {
                        sirketButon.ShadowDecoration.Shadow = new Padding(5, 0, 5, 5);
                        sirketButon.Font = new Font("Verdana", 8, FontStyle.Bold);
                        sirketButon.TextOffset = new Point(0, 15);
                        sirketButon.HoverState.Font = new Font("Verdana", 8, ((FontStyle)((FontStyle.Bold))), GraphicsUnit.Point, ((byte)(162)));

                        if (screens[ekranno].Bounds.Height < 960)
                        {
                            sirketButon.ImageOffset = new Point(0, 30);
                            sirketButon.TextOffset = new Point(0, 15);
                        }
                    }
                    sirketButon.Click += new EventHandler(sirketButon_Click);
                    sirketButon.DoubleClick += new EventHandler(sirketButon_DoubleClick);
                    sirketButon.SizeChanged += new EventHandler(sirketButon_SizeChanged);
                    sirketButon.BackColor = Color.Red;
                    tableLayoutPanel1.Controls.Add(sirketButon);

                    //////////// Create a new "ToolStripMenuItem" object:
                    ToolStripMenuItem newMenuItem = new ToolStripMenuItem();
                    //////////// Set a name, for identification purposes:
                    newMenuItem.Name = sayac.ToString() + "_item";
                    //////////// Sets the text that will appear in the new context menu option:
                    newMenuItem.Text = sirket_adi;
                    newMenuItem.Click += new EventHandler(TraysirketButon_Click);
                    //////////// Add this new item to your context menu:
                    sagtikmenusu.Items.Add(newMenuItem);

                    //this.Refresh();
                    Application.DoEvents();

                    //hs.cmb_sirket.Items.Add(sirket_adi);

                    sayac++;
                    //tableLayoutPanel1.ResumeLayout();
                }
                tableLayoutPanel1.Visible = true;
            }
            catch (Exception exp)
            {
                string hata = exp.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Şirketler DB den okunup, şirket butonlar oluşturulurken", hata);
            }
            finally
            {
                con.Close();
                con.Dispose();
                //tableLayoutPanel1.ResumeLayout(true);
            }

            GC.Collect();
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void HtmlInputTemizleVeDoldur(IWebDriver driveer, string xpathID, string yazilacak)
        {
            HtmlInputBosalt(driveer, xpathID);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driveer;
            string jsstrng;
            jsstrng = "var elem = document.evaluate(\"" + xpathID + "\",document,null,XPathResult.FIRST_ORDERED_NODE_TYPE,null); elem; elem.singleNodeValue.value = ''; ";
            jsstrng += "elem; ";
            jsstrng += "elem.singleNodeValue.value = '" + yazilacak + "';";

            try
            {
                js.ExecuteScript(jsstrng);
            }
            catch (Exception exp1)
            {
                js.ExecuteScript("window.alert('" + exp1.ToString() + "');");
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void HtmlInputBosalt(IWebDriver driveer, string xpathID)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driveer;
            string jsstrng;
            jsstrng = "var elem = document.evaluate(\"" + xpathID + "\",document,null,XPathResult.FIRST_ORDERED_NODE_TYPE,null); elem; elem.singleNodeValue.value = ''; ";
            jsstrng += "elem; ";
            jsstrng += "elem.singleNodeValue.value = '';";

            try
            {
                js.ExecuteScript(jsstrng);
            }
            catch (Exception exp1)
            {
                js.ExecuteScript("window.alert('" + exp1.ToString() + "');");
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private bool IsElementPresent(By by, IWebDriver driveer)
        {
            try
            {
                driveer.FindElement(by);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "WebDriver Element Sorgularken", hata);
                return false;
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private bool IsElementsPresent(By by, IWebDriver driveer)
        {
            try
            {
                driveer.FindElements(by);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "WebDriver Elementler Sorgularken", hata);
                return false;
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        //[Obsolete]
        private async void GirisYap(IWebDriver driveer)
        {
            WebDriverWait wait = new WebDriverWait(driveer, new TimeSpan(0, 0, 30));
            WebDriverWait wait2 = new WebDriverWait(driveer, new TimeSpan(0, 0, 10));
            SelectElement select;

            string UrlAdres = "";
            //driver = chrm_driver;
            //wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));


            try
            {
                UrlAdres = driveer.Url.ToString();
                //if (tarayici == "ie") UrlAdres = ie_driver.Url.ToString();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }
            if (UrlAdres == "") MessageBox.Show("URL Adresi Boş. Panelden Şirket ayarlarını kontrol edin...");

            string link = "";

            try
            {
                string xmlfile = gn.SelenXml;
                System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                xdoc.Load(xmlfile);
                System.Xml.XmlNodeList newXMLNodes = xdoc.SelectNodes("/sirketler/sirket");

                foreach (System.Xml.XmlNode newXMLNode in newXMLNodes)
                {
                    link = newXMLNode["link"].InnerText;
                    if (UrlAdres.Contains(link))
                    {
                        string KullaniciID = newXMLNode["kullaniciadi"].InnerText;
                        string SifreID = newXMLNode["sifre"].InnerText;
                        string LoginID = newXMLNode["login"].InnerText;


                        //Elementte yazan yazıyı görene kadar bekleteceğimiz durum sabit duran ama yazısı değişen elementler için
                        //IWebElement formDurummm = driver.FindElement(By.XPath(XPathyolu));
                        //wait.Until(ExpectedConditions.TextToBePresentInElement(formDurummm, "Form"));

                        //LOGIN ID dolu olduğunda - yani LOGIN buttonu standart olarak XML de sunulduğunda
                        if (LoginID != "")
                        {
                            switch (UrlAdres)
                            {
                                case string x when UrlAdres.Contains("online.sbm.org.tr"):
                                    wait2.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                    driveer.FindElement(By.Id(KullaniciID)).SendKeys(lbl_tramer_adi.Text);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(lbl_tramer_sifre.Text);
                                    driveer.FindElement(By.Id(LoginID)).Click();
                                    break;

                                case string x when UrlAdres.Contains("quicksigorta.com/SFS"):
                                //Etiket_QuickSFS_LoginDeneme:
                                //    try
                                //    {
                                //        try
                                //        {
                                //            //Zaten giriş yapılmış mı diye kontrol et
                                //            Thread.Sleep(2000);
                                //            if (IsElementPresent(By.XPath("//a[@id='nv-login']"), driveer))
                                //            {
                                //                //driveer.FindElement(By.XPath("//a[@id='nv-login']")).Click();
                                //                driveer.Navigate().GoToUrl("https://neoport.neova.com.tr/_layouts/15/Smartiks.SSO/Sso.aspx?a=net");
                                //                Thread.Sleep(2000);
                                //                if (IsElementPresent(By.XPath("//div[@id='siteMenu']"), driveer))
                                //                {
                                //                    //Neova Sigorta Zaten Açık
                                //                    goto Neova_BeklenmeyenSonlanma;
                                //                }
                                //                else
                                //                {
                                //                    goto Etiket_Neova_2FAistedimi;
                                //                }
                                //            }
                                //            else
                                //            {
                                //                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                //            }

                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise                                   
                                //            Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                //            goto Neova_BeklenmeyenSonlanma;
                                //        }
                                //        if (IsElementPresent(By.Id(KullaniciID), driveer))
                                //        {
                                //            try
                                //            {
                                //                driveer.FindElement(By.Id(KullaniciID)).Clear();
                                //                driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                //                driveer.FindElement(By.Id(SifreID)).Clear();
                                //                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);


                                //                if (IsElementPresent(By.XPath("//input[@placeholder='Güvenlik Kodu']"), driveer))
                                //                {
                                //                    String neovacaptchasonuc = "";
                                //                    IWebElement neovacaptcharesim = driveer.FindElement(By.Id("ctl00_PlaceHolderMain_imgCptc"));
                                //                    Image img = GetElementScreenShot(driveer, neovacaptcharesim);
                                //                    //string base64bu;
                                //                    //base64bu = cptch.ResimdenBase64e(img);
                                //                    Captcha cptch = new Captcha();
                                //                    Image captchresim;

                                //                    captchresim = NeovaRenkDegistir((Bitmap)img);
                                //                    captchresim = NeovaTemizle((Bitmap)captchresim);
                                //                    captchresim = NeovaTemizle((Bitmap)captchresim);
                                //                    captchresim = FixedSizeTo500(captchresim);
                                //                    neovacaptchasonuc = cptch.NeovaAritmetik(cptch.NeovaOku(captchresim));

                                //                    if (neovacaptchasonuc != "")
                                //                    {
                                //                        if (neovacaptchasonuc.Contains("KARAKTER"))
                                //                        {
                                //                            goto Etiket_Neova_LoginDeneme;
                                //                        }
                                //                        else
                                //                        {
                                //                            //Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Captcha çözüldü. " + neovacaptchasonuc, ToolTipIcon.Error);
                                //                            driveer.FindElement(By.XPath("//input[@placeholder='Güvenlik Kodu']")).Clear();
                                //                            driveer.FindElement(By.XPath("//input[@placeholder='Güvenlik Kodu']")).SendKeys(neovacaptchasonuc);
                                //                        }
                                //                    }
                                //                    else
                                //                    {
                                //                        Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Captcha Çözülemedi. Lütfen manuel giriş yapınız !!!", ToolTipIcon.Error);
                                //                    }

                                //                }
                                //                driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);

                                //                Thread.Sleep(500);
                                //                if (IsElementPresent(By.XPath("//div[@aria-hidden='false']"), driveer))
                                //                {
                                //                    if (IsElementPresent(By.XPath("//div[contains(text(),'Güvenlik')]"), driveer))
                                //                    {
                                //                        //Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                //                        driveer.FindElement(By.XPath("//div[@aria-hidden='false']")).SendKeys(OpenQA.Selenium.Keys.Escape);
                                //                        driveer.Navigate().GoToUrl("https://neoport.neova.com.tr/_layouts/15/Neova.Authentication/CustomLogin.aspx?ReturnUrl=%2f_layouts%2f15%2fAuthenticate.aspx%3fSource%3d%252F&Source=%2F");
                                //                        goto Etiket_Neova_LoginDeneme;
                                //                    }
                                //                    else if (IsElementPresent(By.XPath("//div[contains(text(),'şifre')]"), driveer))
                                //                    {
                                //                        Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Girişte ŞİFRE HATASI mevcut. Şifre ayarı kontrol edilmeli.", ToolTipIcon.Error);
                                //                    }
                                //                    goto Neova_BeklenmeyenSonlanma;
                                //                }
                                //            }
                                //            catch (Exception)
                                //            {
                                //                //hata ayıklama
                                //                goto Etiket_Neova_LoginDeneme;
                                //            }
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        //hata ayıklama
                                //    }

                                Etiket_QuickSFS_LoginDeneme:
                                    try
                                    {
                                        try
                                        {
                                            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("fraBottom")));
                                            driveer.SwitchTo().Frame("fraBottom");
                                        }
                                        catch (Exception exp)
                                        {
                                            //ana Framee ulaşılamadı şirkette hata var
                                            MessageBox.Show("Quick SFS Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!");
                                            goto QuickSFS_BeklenmeyenSonlanma;
                                        }

                                        //Zaten giriş yapılmış mı diye kontrol et
                                        //burası kodlanacak

                                        wait2.Until(ExpectedConditions.ElementIsVisible(By.XPath(KullaniciID)));

                                        if (IsElementPresent(By.XPath(KullaniciID), driveer))
                                        {
                                            //driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                            //driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                            //driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);
                                            HtmlInputTemizleVeDoldur(driveer, KullaniciID, KullaniciAdi);
                                            HtmlInputTemizleVeDoldur(driveer, SifreID, Sifre);
                                            driveer.FindElement(By.XPath(LoginID)).Click();
                                        }
                                        else
                                        {
                                            goto Etiket_QuickSFS_LoginDeneme;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Etiket_QuickSFS_LoginDeneme hata ayıklama
                                    }

                                Etiket_QuickSFS_Neistedi:
                                    try
                                    {
                                        try
                                        {
                                            wait2.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='txtCaptcha']")));
                                            goto Etiket_QuickSFS_CaptchaCoz;
                                        }
                                        catch (Exception exx)
                                        {
                                            Console.WriteLine(exx.ToString());
                                            goto Etiket_QuickSFS_Neistedi;
                                        }
                                    }
                                    catch (Exception expx)
                                    {
                                        //hata ayıklama
                                    }

                                Etiket_QuickSFS_CaptchaCoz:
                                    try
                                    {
                                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                        Trayyy1.ShowBalloonTip(100, "QUİCK SFS SİGORTA", "Captcha çözme deneniyor, Lütfen bekleyiniz...", ToolTipIcon.None);

                                        Actions actionz = new Actions(driveer);
                                        actionz.MoveToElement(driveer.FindElement(By.XPath("//input[@id='txtCaptcha']"))).Click().Perform();



                                        string quickanahtar = "BOSLUK";
                                        int dongumax = 0;
                                        while (quickanahtar == "BOSLUK" && dongumax < 5)
                                        {
                                            dongumax++;
                                            Thread.Sleep(4000);
                                            IWebElement quickcaptcharesim = driveer.FindElement(By.XPath("//img[@id='imgCaptcha']"));
                                            Image img = GetElementScreenShot(driveer, quickcaptcharesim);
                                            //string base64bu;
                                            //base64bu = cptch.ResimdenBase64e(img);
                                            Captcha cptch = new Captcha();
                                            Image captchresim;


                                            captchresim = Contrastit((Bitmap)img, 40);
                                            //captchresim = Contrastit((Bitmap)captchresim, 40);
                                            captchresim = QuickRenkDegistir((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizleWithMargin((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizleWithMargin((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = SiyahDikeyCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatay2xCizgiTemizleQuickMarginli((Bitmap)captchresim);

                                            //captchresim = Contrastit((Bitmap)captchresim, 40);
                                            //captchresim = RemoveNoise((Bitmap)captchresim);


                                            //string base64laniste = ResimdenBase64e(captchresim);
                                            //img.Save(@"C:\CMSigorta\" + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_1.png", ImageFormat.Png);
                                            //captchresim.Save(@"C:\CMSigorta\" + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_2.png", ImageFormat.Png);


                                            try
                                            {
                                                quickanahtar = OcrSpaceileCoz(captchresim, "5");
                                                quickanahtar = quickanahtar.Replace(" ", "");
                                                quickanahtar = quickanahtar.Replace("×", "X");
                                                quickanahtar = quickanahtar.Replace(".", "");
                                                quickanahtar = quickanahtar.ToUpper();
                                                quickanahtar = RemoveSpecialCharacters(quickanahtar);

                                                //captcha resmin ilk halini kaydetmeye şuan gerek yok olursa açılabilr.
                                                //img.Save(@"C:\CMSigorta\" + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_1.png", ImageFormat.Png);
                                                //captchresim.Save(@"C:\CMSigorta\" + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_2.png", ImageFormat.Png);
                                            }
                                            catch
                                            {
                                                // bir yere goto olacak ama bakalım

                                            }

                                            if (quickanahtar != "bosluk" && quickanahtar.Length == 6)
                                            {
                                                //Trayyy1.ShowBalloonTip(1000, "Türkiye Anahtar Çözüldü", turkiyeanahtar, ToolTipIcon.Info);
                                                //driveer.FindElement(By.XPath("//input[@id='txtCaptcha']")).SendKeys(quickanahtar);
                                                HtmlInputTemizleVeDoldur(driveer, "//input[@id='txtCaptcha']", quickanahtar);
                                                //driveer.FindElement(By.XPath("//input[@id='btnOkCaptcha']")).Click();
                                                driveer.FindElement(By.XPath("//input[@id='txtCaptcha']")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                                Thread.Sleep(500);

                                                //bütün sayfalarda olan script elementi sayfanın gelip gelmediğini kontrol edebiliyoruz
                                                wait.Until(driveeer => ((IJavaScriptExecutor)driveer).ExecuteScript("return document.readyState").Equals("complete"));

                                                //eğer captcha yanlışsa
                                                if (IsElementPresent(By.XPath("//img[@id='imgCaptcha']"), driveer))
                                                {
                                                    string folderName = @"C:\CMSigorta\cptchaerror\";
                                                    // If directory does not exist, create it
                                                    if (!Directory.Exists(folderName))
                                                    {
                                                        Directory.CreateDirectory(folderName);
                                                    }

                                                    img.Save(folderName + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_1.png", ImageFormat.Png);
                                                    captchresim.Save(folderName + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_2.png", ImageFormat.Png);

                                                    //captcha hatası
                                                    if (dongumax == 5)
                                                    {
                                                        Trayyy1.ShowBalloonTip(100, "QUİCK SFS SİGORTA", "Quick SFS Sigorta'da Captcha çözülemedi !!!", ToolTipIcon.Warning);
                                                    }
                                                    else
                                                    {
                                                        goto Etiket_QuickSFS_CaptchaCoz;
                                                    }
                                                }
                                                else
                                                {
                                                    if (IsElementPresent(By.Id("txtOtp"), driveer))
                                                    {
                                                        DoLikeSMStimer(driveer);
                                                    }
                                                    else
                                                    {
                                                        goto QuickSFS_BeklenmeyenSonlanma;
                                                    }
                                                    break;
                                                }

                                                ////eğer şifre yanlışsa
                                                //if (IsElementPresent(By.XPath("//span[contains(text(),'şifre')]"), driveer))
                                                //{
                                                //    Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Türkiye Sigorta'da şifre hatası mevcut. Lütfen şirket ayarlarını kontrol ediniz !!!", ToolTipIcon.Warning);
                                                //    goto QuickSFS_BeklenmeyenSonlanma;
                                                //}

                                                //goto Etiket_Turkiye_SUSPUS_istedimi;
                                            }
                                            else
                                            {
                                                //driveer.FindElement(By.XPath("//i[@class='icon-refresh']")).Click();
                                                //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//i[@class='icon-refresh']")));

                                                string folderName = @"C:\CMSigorta\cptchaerror\";
                                                // If directory does not exist, create it
                                                if (!Directory.Exists(folderName))
                                                {
                                                    Directory.CreateDirectory(folderName);
                                                }

                                                img.Save(folderName + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_1.png", ImageFormat.Png);
                                                captchresim.Save(folderName + DateTime.Now.ToFileTime() + "_" + quickanahtar + "_2.png", ImageFormat.Png);

                                                driveer.FindElement(By.XPath("//img[@id='imgCaptcha']")).Click();
                                                Thread.Sleep(2000);
                                                goto Etiket_QuickSFS_CaptchaCoz;
                                            }
                                        }
                                        Trayyy1.ShowBalloonTip(100, "QUİCK SFS SİGORTA", "Lütfen 1 dakika içinde manuel olarak Captcha girişi yapınız !!!", ToolTipIcon.Info);
                                        Actions action = new Actions(driveer);
                                        action.MoveToElement(driveer.FindElement(By.XPath("//input[@id='txtCaptcha']"))).Click().Perform();

                                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    }
                                    catch (Exception expx)
                                    {
                                        //hata ayıklama
                                    }


                                QuickSFS_BeklenmeyenSonlanma:
                                    try
                                    {
                                        //Console.Beep(350,150);
                                        //Console.Beep(350,150);
                                    }
                                    catch (Exception expx)
                                    {
                                        //hata ayıklama
                                    }



                                    try
                                    {
                                        wait2.Until(ExpectedConditions.ElementIsVisible(By.Id("fraMain")));
                                        //Console.Beep();
                                    }
                                    catch (WebDriverTimeoutException ex)
                                    {
                                        string hata = ex.ToString();
                                        gn.LocalLoglaAsync(isimLBL.Text, "Quick fraMain yakalamada hata", hata);
                                    }
                                    if (IsElementPresent(By.XPath("/html/body/form/table/tbody/tr[4]/td[3]/div[1]/div[3]/table/tbody/tr[2]/td[2]/input"), driveer))
                                    {
                                        TimerSMS.Start();
                                    }
                                    else
                                    {
                                        gn.LocalLoglaAsync(isimLBL.Text, "Quick SMS şifresi yazılacak input Box bulunamadı", "X");
                                    }

                                    driveer.SwitchTo().DefaultContent();



                                    break;


                                case string x when UrlAdres.Contains("rayport.raysigorta.com.tr")
                                                || UrlAdres.Contains("ac.sompojapan.com.tr")
                                                || UrlAdres.Contains("online.gulfsigorta.com.tr")
                                                || UrlAdres.Contains("galaksi.turknippon.com"):
                                    //Eğer yukardakilerden biri olduğunda ama NİPPON değilken
                                    if (!x.Contains("galaksi.turknippon.com"))
                                    {
                                        // SFS içermediğinde ama ve QUICK olduğunda - yani QUİCK web sitesinde isen
                                        if (!x.Contains("SFS") && x.Contains("quicksigorta.com"))
                                        {
                                            driveer.FindElement(By.LinkText("Acente Girişi")).Click();
                                        }
                                        //QUİCK SFS de olduğunda
                                        else
                                        {
                                            wait2.Until(ExpectedConditions.ElementIsVisible(By.Id("fraBottom")));
                                            driveer.SwitchTo().Frame("fraBottom");
                                        }
                                    }

                                    IWebElement beklenen = wait2.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                    if (IsElementPresent(By.Id(KullaniciID), driveer))
                                    {
                                        driveer.FindElement(By.Id(KullaniciID)).Clear();
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                        //HtmlInputTemizleVeDoldur(driveer, KullaniciID, KullaniciAdi);
                                        //HtmlInputTemizleVeDoldur(driveer, SifreID, Sifre);


                                        if (x.Contains("galaksi.turknippon.com"))
                                        {
                                            driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            GelenKullaniciAdi = KullaniciAdi;
                                            TimerKareKod.Start();
                                        }
                                        else
                                        {
                                            if (!x.Contains("SFS") && x.Contains("quicksigorta.com"))
                                            {
                                                driveer.FindElement(By.Id(SifreID)).Click();
                                                driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                                try
                                                {
                                                    wait2.Until(ExpectedConditions.ElementIsVisible(By.Id("agency-gsm")));
                                                    if (IsElementPresent(By.Name("gsm"), driveer))
                                                    {
                                                        select = new SelectElement(driveer.FindElement(By.Name("gsm")));
                                                        select.SelectByValue("5");
                                                        driveer.FindElement(By.XPath("/html/body/div[4]/div[12]/div[2]/form[2]/button")).Click();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    string hata = ex.ToString();
                                                    gn.LocalLoglaAsync(isimLBL.Text, "Quick Sigorta SMS giriş sayfasına ulaşılırken hata", hata);
                                                }
                                            }
                                            else
                                            {
                                                driveer.FindElement(By.Id(LoginID)).Click();
                                                await Task.Delay(2000);
                                                if (x.Contains("quicksigorta.com/SFS"))
                                                {
                                                    try
                                                    {
                                                        wait2.Until(ExpectedConditions.ElementIsVisible(By.Id("fraMain")));
                                                        //Console.Beep();
                                                    }
                                                    catch (WebDriverTimeoutException ex)
                                                    {
                                                        string hata = ex.ToString();
                                                        gn.LocalLoglaAsync(isimLBL.Text, "Quick fraMain yakalamada hata", hata);
                                                    }
                                                    if (IsElementPresent(By.XPath("/html/body/form/table/tbody/tr[4]/td[3]/div[1]/div[3]/table/tbody/tr[2]/td[2]/input"), driveer))
                                                    {
                                                        TimerSMS.Start();
                                                    }
                                                    else
                                                    {
                                                        gn.LocalLoglaAsync(isimLBL.Text, "Quick SMS şifresi yazılacak input Box bulunamadı", "X");
                                                    }
                                                }
                                                driveer.SwitchTo().DefaultContent();
                                            }

                                        }
                                    }
                                    else GirisYap(ie_driver);
                                    break;

                                case string x when UrlAdres.Contains("online.ankarasigorta.com.tr"):
                                    foreach (var element in driveer.FindElements(By.ClassName("form-control")))
                                    {
                                        string KlasAdi = element.GetAttribute("className");
                                        string PlaceAdi = element.GetAttribute("placeholder");
                                        if (!KlasAdi.Contains("hidden") && PlaceAdi.Contains("Kullanıcı Adı"))
                                        {
                                            if (element.Displayed) element.SendKeys(KullaniciAdi);
                                        }
                                        if (!KlasAdi.Contains("hidden") && PlaceAdi.Contains("Şifre"))
                                        {
                                            if (element.Displayed)
                                            {
                                                element.SendKeys(Sifre);
                                                element.SendKeys(OpenQA.Selenium.Keys.Tab);
                                                element.SendKeys(OpenQA.Selenium.Keys.Tab);
                                            }
                                        }
                                    }
                                    //driveer.FindElement(By.XPath(LoginID)).Click();
                                    //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Code")));
                                    GelenKullaniciAdi = KullaniciAdi;
                                    TimerKareKod.Start();

                                    break;

                                case string x when UrlAdres.Contains("login.microsoftonline.com"):
                                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                    if (IsElementPresent(By.Id(KullaniciID), driveer))
                                    {
                                        //await Task.Delay(2000);
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id(SifreID)));
                                        //driveer.FindElement(By.Id(SifreID)).Click();
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@data-value='PhoneAppOTP']")));
                                        driveer.FindElement(By.XPath("//div[@data-value='PhoneAppOTP']")).Click();
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idTxtBx_SAOTCC_OTC")));
                                        //driveer.FindElement(By.Id(LoginID)).Click();
                                        GelenKullaniciAdi = KullaniciAdi;
                                        //KullaniciAdi = "";
                                        TimerKareKod.Start();
                                    }
                                    else GirisYap(chrm_driver);
                                    break;

                                case string x when UrlAdres.Contains("unicosigorta.com.tr/online-islemler/acente/unikolay-giris"):
                                    wait.Until(ExpectedConditions.ElementIsVisible(By.TagName(KullaniciID)));
                                    driveer.FindElements(By.TagName(KullaniciID))[0].SendKeys(KullaniciAdi);
                                    driveer.FindElements(By.TagName(SifreID))[1].SendKeys(Sifre);
                                    driveer.FindElement(By.Id(LoginID)).Click();
                                    TimerKareKod2.Start();
                                    break;

                                case string x when UrlAdres.Contains("biz.hdisigorta.com.tr"):
                                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                    if (IsElementPresent(By.Id(KullaniciID), driveer))
                                    {
                                        driveer.FindElement(By.Id(KullaniciID)).Clear();
                                        driveer.FindElement(By.Id(KullaniciID)).Click();
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                                        GelenKullaniciAdi = KullaniciAdi;
                                        KullaniciAdi = "";
                                        driveer.FindElement(By.Id(SifreID)).Click();
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                        driveer.FindElements(By.TagName("button"))[0].Click();
                                        TimerKareKod.Start();
                                    }
                                    else GirisYap(chrm_driver);
                                    break;
                                // Ray 3 satırda da standart giriş yönteminden farklı olduğu için burada
                                case string x when UrlAdres.Contains("rayexpress.com.tr"):
                                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[1]/form[1]/div[1]/input")));
                                    driveer.FindElements(By.ClassName(KullaniciID))[0].SendKeys(KullaniciAdi);
                                    driveer.FindElements(By.ClassName(SifreID))[1].SendKeys(Sifre);
                                    driveer.FindElements(By.ClassName(LoginID))[0].Click();

                                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[1]/form[2]/center/div/select")));
                                    select = new SelectElement(driveer.FindElement(By.XPath("/html/body/div[1]/form[2]/center/div/select")));
                                    select.SelectByValue("object:198");
                                    driveer.FindElement(By.XPath("/html/body/div[1]/form[2]/button")).Click();
                                    TimerSMS.Start();
                                    break;

                                // standart giriş yöntemi ile halledilebilen diğer şirketler
                                default:

                                    try
                                    {
                                        // Selen XML den gelen  Kullanıcı input ID nin varlığını kontrol edip 
                                        // Şirketlerin XML inden gelen şifreleri inputlara yaz
                                        wait2.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    }
                                    catch
                                    {
                                        if (UrlAdres.Contains("pusula.turkiyesigorta.com.tr"))
                                        {
                                            // Eğer zaten oturum açıksa Anasayfa linki varmı diye bakıyor
                                            if (IsElementPresent(By.LinkText("Ana Sayfa"), driveer))
                                            {
                                                driveer.FindElement(By.LinkText("Ana Sayfa")).Click();
                                                wait.Until(ExpectedConditions.UrlContains("base"));
                                                if (IsElementPresent(By.Id("j_id9:j_id67"), driveer)) driveer.FindElement(By.Id("j_id9:j_id67")).Click();
                                            }
                                        }
                                        break;
                                    }


                                    switch (UrlAdres)
                                    {
                                        #region allianz eski yöntemlerden biri
                                        // allianz standart giriş yönteminden birazcık ayrılıyor üstü aynı
                                        //////////////////////case string y when UrlAdres.Contains("allianz"):
                                        //////////////////////    driveer.FindElement(By.ClassName("inputButton")).Click();
                                        //////////////////////    await Task.Delay(1000);
                                        //////////////////////    if (IsElementPresent(By.Id("smsToken"), driveer))
                                        //////////////////////    {
                                        //////////////////////        string tipp = driveer.FindElement(By.Id("smsToken")).GetAttribute("type");
                                        //////////////////////        if (tipp == "hidden")
                                        //////////////////////        {
                                        //////////////////////            if (IsElementPresent(By.Id("redirect"), driveer)) driveer.FindElement(By.XPath("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////            else
                                        //////////////////////            {
                                        //////////////////////                await Task.Delay(1000);
                                        //////////////////////                if (IsElementPresent(By.Id("redirect"), driveer)) driveer.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////                else
                                        //////////////////////                {
                                        //////////////////////                    await Task.Delay(1000);
                                        //////////////////////                    if (IsElementPresent(By.Id("redirect"), driveer)) driveer.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////                    else
                                        //////////////////////                    {
                                        //////////////////////                        await Task.Delay(1000);
                                        //////////////////////                        driveer.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////                    }
                                        //////////////////////                }
                                        //////////////////////            }
                                        //////////////////////            break;
                                        //////////////////////        }
                                        //////////////////////        else TimerSMS.Start();
                                        //////////////////////        break;
                                        //////////////////////    }
                                        //////////////////////    else
                                        //////////////////////    {
                                        //////////////////////        if (IsElementPresent(By.Id("redirect"), driveer)) driveer.FindElement(By.XPath("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////        else
                                        //////////////////////        {
                                        //////////////////////            await Task.Delay(1000);
                                        //////////////////////            if (IsElementPresent(By.Id("redirect"), driveer)) driveer.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////            else
                                        //////////////////////            {
                                        //////////////////////                await Task.Delay(1000);
                                        //////////////////////                if (IsElementPresent(By.Id("redirect"), driveer)) driveer.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////                else
                                        //////////////////////                {
                                        //////////////////////                    await Task.Delay(1000);
                                        //////////////////////                    driveer.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                                        //////////////////////                }
                                        //////////////////////            }
                                        //////////////////////        }
                                        //////////////////////        break;
                                        //////////////////////    }
                                        //////////////////////    break; 
                                        #endregion

                                        case string y when UrlAdres.Contains("allianz"):
                                            driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            await Task.Delay(1000);
                                            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("redirect")));
                                            TimerSMS.Start();
                                            break;

                                        // axa resim cacpcha yazıcı
                                        case string y when UrlAdres.Contains("oasis.axasigorta.com.tr"):

                                            if (IsElementPresent(By.Id("CPH_MiddlePane_Im1"), driveer))
                                            {

                                                string axaanahtar = "BOSLUK";
                                                int dongumax = 0;
                                                while (axaanahtar == "BOSLUK" && dongumax < 3)
                                                {
                                                    dongumax++;
                                                    IWebElement resimkodu = driveer.FindElement(By.Id("CPH_MiddlePane_Im1"));
                                                    var img = GetElementScreenShot(driveer, resimkodu);
                                                    try
                                                    {
                                                        axaanahtar = AxaCaptchaCoz(img);
                                                    }
                                                    catch
                                                    {
                                                        /*
                                                        using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"PNG|*.png" })
                                                        {
                                                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                                            {
                                                                img.Save(saveFileDialog.FileName);
                                                            }
                                                        }
                                                        */

                                                        // çözülemeyen Axa Captcha yı FTP ye analiz için gönder
                                                        //uploadFile(img);
                                                        //çözülemeyen Axa Captcha yı TEMP e kaydet
                                                        img.Save(Path.GetTempPath() + DateTime.Now.ToFileTime() + "_img.png", System.Drawing.Imaging.ImageFormat.Png);

                                                        img = null;

                                                        driveer.FindElement(By.Id("CPH_MiddlePane_imgrefresh")).Click();

                                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id(SifreID)));
                                                        driveer.FindElement(By.Id(SifreID)).Clear();
                                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);

                                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CPH_MiddlePane_Im1")));
                                                    }
                                                }
                                                if (axaanahtar != "BOSLUK")
                                                {
                                                    Trayyy1.ShowBalloonTip(100, "Axa Anahtar Çözüldü", axaanahtar, ToolTipIcon.Info);
                                                    driveer.FindElement(By.Id("CPH_MiddlePane_edtCaptcha")).SendKeys(axaanahtar);
                                                    driveer.FindElement(By.Id(LoginID)).Click();
                                                    TimerSMS.Start();
                                                }
                                            }
                                            break;

                                        // axa resim cacpcha yazıcı
                                        case string y when UrlAdres.Contains("axatek.axasigorta.com.tr"):

                                            if (IsElementPresent(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[4]/div/div/div[1]/div/img"), driveer))
                                            {
                                                string axaanahtar = "BOSLUK";
                                                string resimkodu = "";
                                                int dongumax = 0;
                                                while (axaanahtar == "BOSLUK" && dongumax < 3)
                                                {

                                                    dongumax++;
                                                    await Task.Delay(1000);
                                                    resimkodu = chrm_driver.FindElement(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[4]/div/div/div[1]/div/img")).GetAttribute("src");
                                                    //var img = GetElementScreenShot(driveer, resimkodu);
                                                    resimkodu = resimkodu.Remove(0, 23);


                                                    byte[] bytes = Convert.FromBase64String(resimkodu);
                                                    Image img;
                                                    using (MemoryStream ms = new MemoryStream(bytes))
                                                    {
                                                        img = Image.FromStream(ms);
                                                    }
                                                    try
                                                    {
                                                        guna2PictureBox1.Image = img;
                                                        axaanahtar = AxaCaptchaCoz(img);
                                                        // TEST
                                                    }
                                                    catch
                                                    {
                                                        /*
                                                        using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"PNG|*.png" })
                                                        {
                                                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                                            {
                                                                img.Save(saveFileDialog.FileName);
                                                            }
                                                        }
                                                        */

                                                        // çözülemeyen Axa Captcha yı FTP ye analiz için gönder
                                                        //uploadFile(img);
                                                        //çözülemeyen Axa Captcha yı TEMP e kaydet
                                                        img.Save(Path.GetTempPath() + DateTime.Now.ToFileTime() + "_img.png", System.Drawing.Imaging.ImageFormat.Png);


                                                    }
                                                }
                                                if (axaanahtar != "BOSLUK")
                                                {
                                                    Trayyy1.ShowBalloonTip(1000, "Axa Anahtar Çözüldü", axaanahtar, ToolTipIcon.Info);
                                                    driveer.FindElement(By.Id("input-36")).SendKeys(axaanahtar);
                                                    driveer.FindElement(By.XPath(LoginID)).Click();
                                                    TimerSMS.Start();
                                                    break;
                                                }
                                                else
                                                {
                                                    resimkodu = "";

                                                    driveer.FindElement(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[4]/div/div/div[2]/button")).Click();

                                                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id(SifreID)));
                                                    driveer.FindElement(By.Id(SifreID)).Clear();
                                                    //driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);

                                                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[4]/div/div/div[1]/div/img")));
                                                }
                                            }
                                            break;

                                        // bu şirketler login işlevinde ID yerine XPATH kullanıyorlar
                                        case string y when UrlAdres.Contains("auth.korusigortaportal.com")
                                                        || UrlAdres.Contains("adaauth.dogasigorta.com")
                                                        || UrlAdres.Contains("auth.grisigorta.com.tr")
                                                        || UrlAdres.Contains("pusula.turkiyesigorta.com.tr")
                                                        || UrlAdres.Contains("kolayekran.mapfre.com.tr"):
                                            //Türkiye ekrana girerken captcha sorarsa yada sormaz kontrolü
                                            if (y.Contains("pusula.turkiyesigorta.com.tr") && !y.Contains("pusula.turkiyesigorta.com.tr/base"))
                                            {
                                                //Eğer Türkiye ekrana girerken captcha sormazsa click
                                                if (!IsElementPresent(By.Id("j_id9:j_id83:captchaImage"), driveer)) driveer.FindElement(By.XPath(LoginID)).Click();
                                                else driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                                                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("j_id9:j_id39:MFAValue")));
                                                //Thread.Sleep(5000);
                                                TimerSMS.Start();

                                            }
                                            else
                                            {
                                                driveer.FindElement(By.XPath(LoginID)).Click();
                                                TimerSMS.Start();
                                            }

                                            ////Türkiye Sigorta'da kullanıcı captcha yı yazdıktan sonra illa base içeren bir linke gider o zamana kadar bekle
                                            //if (y.Contains("pusula.turkiyesigorta.com.tr"))
                                            //{
                                            //    wait.Until(ExpectedConditions.UrlContains("base"));
                                            //    if (IsElementPresent(By.Id("j_id9:j_id67"), driveer)) driveer.FindElement(By.Id("j_id9:j_id67")).Click();
                                            //}
                                            // Bunlar XPATH kullanların Google 2 Faktör ile giriş yaptıranları
                                            if (y.Contains("auth.korusigortaportal.com")
                                             || y.Contains("adaauth.dogasigorta.com")
                                             || y.Contains("auth.privesigorta.com")
                                             || y.Contains("auth.aveonglobalsigorta.com")
                                             || y.Contains("auth.privesigorta.com")
                                             || y.Contains("auth.grisigorta.com.tr"))
                                            {
                                                GelenKullaniciAdi = KullaniciAdi;
                                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Code")));
                                                //MessageBox.Show("Bulundu");
                                                TimerKareKod.Start();
                                            }
                                            break;

                                        case string y when UrlAdres.Contains("anka.groupama.com.tr"):
                                            driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                                            //driveer.FindElement(By.Id("recaptcha-anchor")).SendKeys(OpenQA.Selenium.Keys.Space);
                                            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("txtGAKod")));
                                            GelenKullaniciAdi = KullaniciAdi;
                                            TimerKareKod.Start();
                                            break;

                                        case string y when UrlAdres.Contains("giris.anadolusigorta.com.tr")
                                                        || UrlAdres.Contains("sat2.aksigorta.com.tr"):
                                            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(LoginID)));
                                            driveer.FindElement(By.Id(LoginID)).Click();
                                            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("smsPassword")));
                                            driveer.FindElement(By.Id("smsPassword")).Click();
                                            //driveer.FindElement(By.Id("selectOtpType")).Click();
                                            TimerSMS.Start();

                                            break;

                                        // LoginID işlevini Id element ile yapan diğer bütün şirketlere
                                        default:
                                            driveer.FindElement(By.Id(LoginID)).Click();
                                            break;
                                    }
                                    break;
                            }
                            //loginID işlevlerini bitirdikten sonra 2 faktör yapması için timer gitmesi gereken siteler
                            if (UrlAdres.Contains("nareks.bereket.com.tr")
                             || UrlAdres.Contains("acente.atlasmutuel.com.tr/")
                             || UrlAdres.Contains("anka.groupama.com.tr")
                             || UrlAdres.Contains("neova.com.tr")
                             || UrlAdres.Contains("orientsigorta.com.tr"))
                            {
                                GelenKullaniciAdi = KullaniciAdi;
                                TimerKareKod.Start();
                            }
                        }
                        //LoginID değeri XML yok yada boş ise
                        else
                        {
                            //Console.Beep(8000, 100);
                            //Console.Beep(8000, 100);
                            //Console.Beep(8000, 100);
                            switch (UrlAdres)
                            {
                                case string x when UrlAdres.Contains("ssoswepweb.anadolusigorta"):
                                    wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("master"));

                                    //await Task.Delay(3000);
                                    //driveer.SwitchTo().Frame("master");
                                    //await Task.Delay(10000);

                                    driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    driveer.SwitchTo().DefaultContent();
                                    wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("master"));
                                    driveer.FindElement(By.Id("login:subBut")).Click();
                                    driveer.FindElement(By.Id("login:subBut")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                    driveer.SwitchTo().DefaultContent();
                                    break;

                                case string x when UrlAdres.Contains("acenteportal.groupama.com.tr"):
                                    driveer.FindElement(By.Id("txtAgencyCode")).SendKeys(acente_kodu);
                                    driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    break;

                                case string x when UrlAdres.Contains("quicksigorta"):
                                    driveer.SwitchTo().Frame("fraBottom");
                                    driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    driveer.SwitchTo().DefaultContent();
                                    break;

                                case string x when UrlAdres.Contains("turknippon"):
                                    driveer.FindElement(By.Name(KullaniciID)).SendKeys(KullaniciAdi);
                                    driveer.FindElement(By.Name(SifreID)).SendKeys(Sifre);
                                    IWebElement button = driveer.FindElements(By.ClassName("loginSend"))[0];
                                    button.Click();
                                    break;

                                case string x when UrlAdres.Contains("ejento.sompojapan.com.tr"):
                                    driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                                    //wait.Until(ExpectedConditions.())
                                    TimerSMS.Start();
                                    break;

                                default:
                                    switch (UrlAdres)
                                    {
                                        case string x when UrlAdres.Contains("acente.atlasmutuel.com.tr")
                                                        || UrlAdres.Contains("anka.groupama.com.tr"):
                                            driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                            driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                            GelenKullaniciAdi = KullaniciAdi;
                                            TimerKareKod.Start();
                                            break;

                                        case string x when UrlAdres.Contains("biz.hdisigorta.com.tr"):
                                            driveer.FindElements(By.TagName("button"))[0].Click();
                                            driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                            driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                            driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            GelenKullaniciAdi = KullaniciAdi;
                                            TimerKareKod.Start();
                                            break;

                                        case string x when UrlAdres.Contains("neova.com.tr"):
                                            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                            if (IsElementPresent(By.Id(KullaniciID), driveer))
                                            {
                                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                                wait.Until(ExpectedConditions.TitleContains("Home"));
                                                //try
                                                //{
                                                //    driveer.FindElements(By.ClassName("popup_wrapper"))[0].SendKeys(OpenQA.Selenium.Keys.Escape);
                                                //}
                                                //catch
                                                //{
                                                //    ////Neova girişye popup yok
                                                //}                                                
                                            }
                                            driveer.Navigate().GoToUrl("https://neoport.neova.com.tr/_layouts/15/Smartiks.SSO/Sso.aspx?a=net");
                                            GelenKullaniciAdi = KullaniciAdi;
                                            TimerKareKod.Start();
                                            break;
                                        default:
                                            try
                                            {
                                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                            }
                                            catch (NoSuchElementException ex)
                                            {
                                                string hata = ex.ToString();
                                                gn.LocalLoglaAsync(isimLBL.Text, "GirisYap Fonksiyonu / Switch - Default / Switch - Default / Kullanıcı ID veya Şifre girişinde", hata);
                                            }


                                            if (UrlAdres.Contains("sigorta.corpussigorta.com.tr"))
                                            {
                                                if (IsElementPresent(By.Id("txtGuvenlikKod"), driveer))
                                                {
                                                    driveer.FindElement(By.Id("txtGuvenlikKod")).SendKeys("ChNgR");
                                                    driveer.FindElement(By.Id("txtGuvenlikKod")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                                }
                                                driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            }
                                            GelenKullaniciAdi = KullaniciAdi;
                                            TimerKareKod.Start();
                                            break;
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Şirketin login bilgilerini XML den çektikten sonra şirkete göre login işlemi yaparken", hata);
                //MessageBox.Show(ex.ToString());
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        [Obsolete]
        void menuItem_Click(object sender, EventArgs e)
        {
            ContextMenuStrip item = (sender as ContextMenuStrip);
            sirketadi = item.Text;
            sirketegitAsync(sirketadi);
            // MessageBox.Show(item.Text + " was clicked.");
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void SistemForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cikisyap();
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void SistemForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //cikisyap();
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void DoLikeKareKodtimer(IWebDriver driveer)
        {
            string UrlAdres = "";

            driver = driveer;

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 60));


        DoLike_KareKod_Timer:
            try
            {
                UrlAdres = driver.Url.ToString();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Timer KareKod içinde WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }


            string xmlfile = gn.KareKod;
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlfile);
            XmlNodeList newXMLNodes = xdoc.SelectNodes("/sirketler/sirket");
            string gelenlink = "";
            string adi = "";
            string key = "";
            foreach (XmlNode newXMLNode in newXMLNodes)
            {
                gelenlink = newXMLNode["link"].InnerText;
                adi = newXMLNode["kullaniciadi"].InnerText;
                key = newXMLNode["key"].InnerText;
                if (UrlAdres.Contains(gelenlink) && GelenKullaniciAdi == adi)
                {
                    switch (gelenlink)
                    {
                        case string x when gelenlink.Contains("auth.grisigorta.com.tr")
                                        || gelenlink.Contains("auth.aveonglobalsigorta.com")
                                        || gelenlink.Contains("auth.privesigorta.com")
                                        || gelenlink.Contains("adaauth.dogasigorta.com")
                                        || gelenlink.Contains("auth.korusigortaportal.com"):
                            if (IsElementPresent(By.Id("Code"), driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);
                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("Code")).SendKeys(result);
                                driver.FindElement(By.XPath("/html/body/div[2]/div/div/div/div/div[2]/form/button")).Click();
                                //await Task.Delay(1000);
                                Task.Delay(1000);
                                if (IsElementPresent(By.XPath("/html/body/div[2]/div/h1[2]/div/a"), driver))
                                {
                                    driver.FindElement(By.XPath("/html/body/div[2]/div/h1[2]/div/a")).Click();
                                }
                            }
                            else
                            {
                                //TimerKareKod.Start();
                                goto DoLike_KareKod_Timer;
                            }
                            break;

                        case string x when gelenlink.Contains("bereket")
                                        || gelenlink.Contains("atlasmutuel.com.tr")
                                        || gelenlink.Contains("sistem.generali.com.tr")
                                        || gelenlink.Contains("sistem.orientsigorta.com.tr")
                                        || gelenlink.Contains("sigorta.corpussigorta")
                                        || gelenlink.Contains("sigorta.neova")
                                        || gelenlink.Contains("anka.groupama.com.tr"):
                            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("txtGAKod")));
                            if (IsElementPresent(By.Id("txtGAKod"), driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);
                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("txtGAKod")).SendKeys(result);
                                driver.FindElement(By.Id("txtGAKod")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                //driver.FindElement(By.Id("ext-gen69")).Click();
                            }
                            else
                            {
                                if (!IsElementPresent(By.Id("Head1"), driver))
                                {
                                    //TimerKareKod.Start();
                                    goto DoLike_KareKod_Timer;
                                }
                            }
                            break;

                        case string x when gelenlink.Contains("hdisigorta"):
                            if (IsElementPresent(By.XPath("/html/body/div/div/div/div/div[2]/div[2]/div[2]/div[1]/form/div/div[2]/input"), driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);
                                var result = totp.ComputeTotp();
                                driver.FindElement(By.XPath("/html/body/div/div/div/div/div[2]/div[2]/div[2]/div[1]/form/div/div[2]/input")).SendKeys(result);
                                driver.FindElement(By.XPath("/html/body/div/div/div/div/div[2]/div[2]/div[2]/div[1]/form/div/div[5]/input")).Click();
                                ////driver.FindElement(By.Name("token")).SendKeys(result);
                                ////driver.FindElement(By.ClassName("form-control")).SendKeys(OpenQA.Selenium.Keys.Enter);
                            }
                            else
                            {
                                //TimerKareKod.Start();
                                goto DoLike_KareKod_Timer;
                            }
                            break;

                        case string x when gelenlink.Contains("galaksi.turknippon.com"):
                            if (IsElementPresent(By.Id("Gauthcode"), ie_driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("Gauthcode")).SendKeys(result);
                                driver.FindElements(By.ClassName("loginSend"))[0].Click();
                            }
                            else
                            {
                                //TimerKareKod.Start();
                                goto DoLike_KareKod_Timer;
                            }
                            break;
                        case string x when gelenlink.Contains("portal.generali.com.tr"):
                            if (IsElementPresent(By.Id("login-password"), driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("login-password")).SendKeys(result);
                                driver.FindElement(By.Id("login-password")).SendKeys(OpenQA.Selenium.Keys.Enter);

                            }
                            else
                            {
                                //TimerKareKod.Start();
                                goto DoLike_KareKod_Timer;
                            }
                            break;

                        case string x when gelenlink.Contains("login.microsoftonline.com"):
                            if (IsElementPresent(By.Id("idTxtBx_SAOTCC_OTC"), driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("idTxtBx_SAOTCC_OTC")).SendKeys(result);
                                driver.FindElement(By.Id("idTxtBx_SAOTCC_OTC")).SendKeys(OpenQA.Selenium.Keys.Enter);

                                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("idSIButton9")));
                                driver.FindElement(By.Id("idSIButton9")).Click();

                                //await Task.Delay(2000);
                                //Task.Delay(2000);
                                //driver.Navigate().GoToUrl("https://map.mapfre.com.tr");

                            }
                            else
                            {
                                //TimerKareKod.Start();
                                goto DoLike_KareKod_Timer;
                            }
                            break;
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        //[Obsolete]
        private void TimerKareKod_Tick(object sender, EventArgs e)
        {
            TimerKareKod.Stop();


            string UrlAdres = "";
            if (hangiDriver == "chrm") driver = chrm_driver;
            if (hangiDriver == "ie") driver = ie_driver;
            try
            {
                UrlAdres = driver.Url.ToString();
                //MessageBox.Show(UrlAdres);
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Timer Karekod içinde WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }

            if (UrlAdres == "")
            {

            }


            string xmlfile = gn.KareKod;
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlfile);
            XmlNodeList newXMLNodes = xdoc.SelectNodes("/sirketler/sirket");
            string gelenlink = "";
            string adi = "";
            string key = "";
            foreach (XmlNode newXMLNode in newXMLNodes)
            {
                //Console.Beep(8000, 100);
                gelenlink = newXMLNode["link"].InnerText;
                adi = newXMLNode["kullaniciadi"].InnerText;
                key = newXMLNode["key"].InnerText;
                if (UrlAdres.Contains(gelenlink) && GelenKullaniciAdi == adi)
                {
                    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 60));

                    switch (gelenlink)
                    {
                        case string x when gelenlink.Contains("auth.grisigorta.com.tr")
                                        || gelenlink.Contains("auth.aveonglobalsigorta.com")
                                        || gelenlink.Contains("auth.privesigorta.com")
                                        || gelenlink.Contains("adaauth.dogasigorta.com")
                                        || gelenlink.Contains("auth.korusigortaportal.com"):
                            if (IsElementPresent(By.Id("Code"), chrm_driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);
                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("Code")).SendKeys(result);
                                driver.FindElement(By.XPath("/html/body/div[2]/div/div/div/div/div[2]/form/button")).Click();
                                //await Task.Delay(1000);
                                Task.Delay(1000);
                                if (IsElementPresent(By.XPath("/html/body/div[2]/div/h1[2]/div/a"), chrm_driver))
                                {
                                    driver.FindElement(By.XPath("/html/body/div[2]/div/h1[2]/div/a")).Click();
                                }
                            }
                            else TimerKareKod.Start();
                            break;

                        case string x when gelenlink.Contains("bereket")
                                        || gelenlink.Contains("atlasmutuel.com.tr")
                                        || gelenlink.Contains("sistem.generali.com.tr")
                                        || gelenlink.Contains("sistem.orientsigorta.com.tr")
                                        || gelenlink.Contains("sigorta.corpussigorta")
                                        || gelenlink.Contains("anka.groupama.com.tr"):
                            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("txtGAKod")));
                            if (IsElementPresent(By.Id("txtGAKod"), chrm_driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);
                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("txtGAKod")).SendKeys(result);
                                driver.FindElement(By.Id("txtGAKod")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                //driver.FindElement(By.Id("ext-gen69")).Click();
                            }
                            else
                            {
                                if (!IsElementPresent(By.Id("Head1"), chrm_driver)) TimerKareKod.Start();
                            }

                            break;

                        case string x when gelenlink.Contains("hdisigorta"):
                            if (IsElementPresent(By.XPath("/html/body/div/div/div/div/div[2]/div[2]/div[2]/div[1]/form/div/div[2]/input"), chrm_driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);
                                var result = totp.ComputeTotp();
                                driver.FindElement(By.XPath("/html/body/div/div/div/div/div[2]/div[2]/div[2]/div[1]/form/div/div[2]/input")).SendKeys(result);
                                driver.FindElement(By.XPath("/html/body/div/div/div/div/div[2]/div[2]/div[2]/div[1]/form/div/div[5]/input")).Click();
                                ////driver.FindElement(By.Name("token")).SendKeys(result);
                                ////driver.FindElement(By.ClassName("form-control")).SendKeys(OpenQA.Selenium.Keys.Enter);
                            }
                            else TimerKareKod.Start();
                            break;

                        case string x when gelenlink.Contains("galaksi.turknippon.com"):
                            if (IsElementPresent(By.Id("Gauthcode"), ie_driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("Gauthcode")).SendKeys(result);
                                driver.FindElements(By.ClassName("loginSend"))[0].Click();
                            }
                            else TimerKareKod.Start();
                            break;

                        case string x when gelenlink.Contains("login.microsoftonline.com"):
                            if (IsElementPresent(By.Id("idTxtBx_SAOTCC_OTC"), chrm_driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("idTxtBx_SAOTCC_OTC")).SendKeys(result);
                                driver.FindElement(By.Id("idTxtBx_SAOTCC_OTC")).SendKeys(OpenQA.Selenium.Keys.Enter);

                                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("idSIButton9")));
                                driver.FindElement(By.Id("idSIButton9")).Click();

                                //await Task.Delay(2000);
                                Task.Delay(2000);
                                driver.Navigate().GoToUrl("https://map.mapfre.com.tr");

                            }
                            else TimerKareKod.Start();
                            break;

                        case string x when gelenlink.Contains("neova.com.tr"):
                            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("txtGAKod")));
                            if (IsElementPresent(By.Id("txtGACode"), chrm_driver))
                            {
                                var bytes = OtpNet.Base32Encoding.ToBytes(key);
                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();
                                driver.FindElement(By.Id("txtGACode")).SendKeys(result);
                                driver.FindElement(By.Id("txtGACode")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                //driver.FindElement(By.Id("ext-gen69")).Click();
                                break;
                            }
                            else TimerKareKod.Start();

                            //if (IsElementPresent(By.XPath("/html/body/form/div[5]/div/div[3]/header/div/div/div/div[2]/ul/li[2]/a"), chrm_driver))
                            //{
                            //    driver.FindElement(By.XPath("/html/body/form/div[5]/div/div[3]/header/div/div/div/div[2]/ul/li[2]/a")).Click();
                            //    await Task.Delay(2000);
                            //    driver.SwitchTo().Window(driver.WindowHandles.Last());
                            //    anahtar = key;
                            //    TimerKareKod2.Start();
                            //}
                            //else TimerKareKod.Start();

                            break;

                        case string x when gelenlink.Contains("online.ankarasigorta.com"):
                            Trayyy1.ShowBalloonTip(1000, "Giriş Bekletme Süresi", "Lütfen 1 dakika içinde resimde görünen kodu giriniz.", ToolTipIcon.Warning);
                            wait.Until(ExpectedConditions.TitleContains("2FA"));

                            //foreach (var element in driver.FindElements(By.ClassName("form-control")))
                            //{
                            //    string KlasAdi = element.GetAttribute("name");
                            //    string PlaceAdi = element.GetAttribute("type");
                            //    if (KlasAdi.Contains("Code") && PlaceAdi.Contains("password"))
                            //    {
                            //        try
                            //        {
                            //            Trayyy1.ShowBalloonTip(1000, "Key", key, ToolTipIcon.Info);
                            //            var bytes = OtpNet.Base32Encoding.ToBytes(key);
                            //            var totp = new OtpNet.Totp(bytes);
                            //            var remainingTime = totp.RemainingSeconds();
                            //            int sayac = Convert.ToInt32(remainingTime);


                            //            if (sayac <= 3) await Task.Delay(4000);

                            //            var result = totp.ComputeTotp();

                            //            if (element.Displayed)
                            //            {
                            //                element.SendKeys(result);
                            //                element.SendKeys(OpenQA.Selenium.Keys.Enter);
                            //            }
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            string hata = ex.ToString();
                            //            gn.LocalLoglaAsync(isimLBL.Text, "XML 'den okunan Secret Key ile 2FA şifresi üretilirken", hata);
                            //        }
                            //    }
                            //}



                            if (IsElementPresent(By.Id("Code"), ie_driver))
                            {
                                //Console.Beep(600, 1400);

                                var bytes = OtpNet.Base32Encoding.ToBytes(key);

                                var totp = new OtpNet.Totp(bytes);
                                var remainingTime = totp.RemainingSeconds();
                                int sayac = Convert.ToInt32(remainingTime);
                                //if (sayac <= 3) await Task.Delay(4000);
                                if (sayac <= 3) Task.Delay(4000);

                                var result = totp.ComputeTotp();

                                driver.FindElement(By.Id("Code")).SendKeys(result);
                                driver.FindElement(By.Id("Code")).SendKeys(OpenQA.Selenium.Keys.Enter);

                                //butonu bulup click yapmak zor class olmuyor ID yok Xpath belli olmaz değişir 
                                //en iyisi yukardaki gibi Enter göndermek TextBox a
                                //driver.FindElements(By.ClassName("btn-primary btn"))[0].Click();//
                            }
                            else
                            {
                                TimerKareKod.Start();
                            }
                            break;
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private async void TimerKareKod2_Tick(object sender, EventArgs e)
        {
            TimerKareKod2.Stop();

            string UrlAdres = "";
            if (hangiDriver == "chrm") driver = chrm_driver;
            if (hangiDriver == "ie") driver = ie_driver;
            try
            {
                UrlAdres = driver.Url.ToString();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Timer Karekod içinde WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }
            switch (UrlAdres)
            {
                case string x when UrlAdres.Contains("sigorta.neova.com.tr"):
                    if (IsElementPresent(By.Id("txtGACode"), chrm_driver))
                    {
                        var bytes = OtpNet.Base32Encoding.ToBytes(anahtar);
                        var totp = new OtpNet.Totp(bytes);
                        var remainingTime = totp.RemainingSeconds();
                        int sayac = Convert.ToInt32(remainingTime);
                        if (sayac <= 3) await Task.Delay(4000);

                        var result = totp.ComputeTotp();
                        driver.FindElement(By.Id("txtGACode")).SendKeys(result);
                        driver.FindElement(By.Id("btnValidateTwoFactor")).Click();
                    }
                    else TimerKareKod2.Start();
                    break;

                case string x when UrlAdres.Contains("unico"):
                    if (IsElementPresent(By.Id("userOtpPass"), chrm_driver)) driver.Navigate().GoToUrl("http://www.unicosigorta.com.tr/online-islemler/acente/ana-sayfa");
                    else TimerKareKod.Start();
                    break;
            }
            GC.Collect();
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            YeniSirketeGitFonk(sirketadi);
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        private void YeniSirketeGitFonk(string sirketinadi)
        {
            string eski_ip = "xxx";
            if (ip != "")
            {
                eski_ip = ip;
            }
            string sirket_id = gn.en_son_kaydi_getir("t_kullanici_sirketler", "SirketID", "where Adi='" + sirketinadi + "'");
            try
            {
                foreach (DataGridViewRow g1 in dgv_sirketler.Rows)
                {
                    if (g1.Cells["id"].Value.ToString() == sirket_id)
                    {
                        // gereksiz bir değişken atama olmuş zaten sirket_id = g1.Cells["id"].Value.ToString()
                        string SirketID = g1.Cells["id"].Value.ToString();
                        string xmlfile = gn.XmlSirketler;
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(xmlfile);
                        XmlNodeList newXMLNodes = xdoc.SelectNodes("/sirketler/sirket");

                        foreach (XmlNode newXMLNode in newXMLNodes)
                        {
                            if (newXMLNode["id"].InnerText == SirketID)
                            {
                                link = decrypt(newXMLNode["link"].InnerText);
                                KullaniciAdi = decrypt(newXMLNode["kullanici"].InnerText);
                                Sifre = decrypt(newXMLNode["sifre"].InnerText);
                                acente_kodu = decrypt(newXMLNode["acentekodu"].InnerText);
                                ip = decrypt(newXMLNode["ip"].InnerText);
                                port = decrypt(newXMLNode["port"].InnerText);
                                smsnumberno = decrypt(newXMLNode["smstelno"].InnerText);
                                iemitarayici = decrypt(newXMLNode["iemitarayici"].InnerText);
                                ip_durumu = newXMLNode["ipdurum"].InnerText;
                                sirketgirmedurumu = newXMLNode["sirketdurum"].InnerText;
                                string SirketAdi = decrypt(newXMLNode["adi"].InnerText);

                                //Kullanıcı ID - Şirket Adı - Şirket ID olarak log kaydet fonksiyonuna gönder
                                LogKaydet(lbl_id.Text, SirketAdi, SirketID);

                                break;
                            }
                        }
                        break;
                    }
                }

                if (sirketgirmedurumu != "1")
                {
                    lblSirket.Text = sirketadi;

                    if (eski_ip != "xxx" && ip != eski_ip)
                    {
                        ipdegisti = true;
                    }
                    else
                    {
                        ipdegisti = false;
                    }

                    //Eğer SFS ekranların adresleri yakalanırsa
                    if (iemitarayici == "1")
                    {
                        ie_Git(link.ToString());
                        hangiDriver = "ie";
                        GirisYap(ie_driver);
                    }
                    // SFS ekranların adresleri değilse
                    else
                    {
                        Yeni_Chrm_Git(link.ToString());
                        hangiDriver = "chrm";
                        Yeni_GirisYap(chrm_driver);
                    }
                }
                else
                {
                    MessageBox.Show("Şirket Kapalı");
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Şirket XML ile Şirket Buton Adını eşleştirip şirkete gitmeye çalışırken", hata);
                //MessageBox.Show(hata);
            }

        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        //Chrome ile açılacak şirket sayfalarını chrome driver ile açan fonksiyon
        private void Yeni_Chrm_Git(string adres)
        {
            bool chrm_driver_kapalimiydi;

            //if (gn.MySqlBaglanti.Contains("sitemusta"))
            //{
            //    chrm_driver_kapalimiydi = true;
            //}
            //else chrm_driver_kapalimiydi = false;

            if (ipdegisti)
            {
                chrm_driver_kapalimiydi = true;
            }
            else chrm_driver_kapalimiydi = false;


            string haniymis = "";

            try //chrm_driver handle yakalamaya çalış
            {
                //int adettt = chrm_driver.WindowHandles.Count;
                //if (adettt > 0) haniymis = chrm_driver.WindowHandles.Count.ToString();
                //else haniymis = "YOK";
                string driveradresi = "x";
                try
                {
                    driveradresi = chrm_driver.CurrentWindowHandle;
                }
                catch
                {
                    chrm_driver_kapalimiydi = true;
                }

                if (driveradresi != "x") haniymis = chrm_driver.WindowHandles.Count.ToString();
                else haniymis = "YOK";
            }
            catch (NotFoundException ex)
            {
                string hata = ex.ToString();
                //eğer Nesne hatası varsa daha heniz hiç açılmamış demektir. 
                if (hata.Contains("Nesne"))
                {
                    haniymis = "YOK";
                }
                else
                {
                    //MessageBox.Show("Kapalı Driver Hata: " + hata.ToString());
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Chrome WebDriver ile şirkete gitmeye çalışırken Handle edilemeyen sayfa", ".");
                //eğer reachable hatası varsa chrome penceresi açılkdıktan sonra kapatılmış demektir.
                //açık olanları kapatıp yenisini hazırla
                if (hata.Contains("reachable"))
                {
                    haniymis = "KAPALI";

                    //chrm_driver.Close();
                    //chrm_driver.Quit();
                    //chrm_driver.Dispose();

                    /*
                    System.Diagnostics.ProcessStartInfo kmt = new System.Diagnostics.ProcessStartInfo();
                    kmt.CreateNoWindow = true;
                    kmt.FileName = "cmd.exe";
                    kmt.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    kmt.Arguments = @"/c taskkill /f /im chromedriver.exe";
                    System.Diagnostics.Process.Start(kmt);
                    kmt.Arguments = @"/c taskkill /f /im conhost.exe";
                    System.Diagnostics.Process.Start(kmt);
                    //kmt.Arguments = @"/c taskkill /f /im chrome-cms.exe";
                    //System.Diagnostics.Process.Start(kmt);
                    //await System.Threading.Tasks.Task.Delay(1000);
                    */
                }
                else
                {
                    //eğer Nesne hatası varsa daha henüz hiç açılmamış demektir. 
                    if (hata.Contains("Nesne")) haniymis = "YOK";
                    else
                    {
                        //MessageBox.Show("Kapalı Driver Hata: " + hata.ToString());
                    }
                }
                chrm_driver_kapalimiydi = true;

            } //chrm_driver handle yakalamaya çalış


            if (chrm_driver_kapalimiydi)
            {

                //hatanın içindeyiz yani driver oluşturulacak 
                try
                {
                    if (ip != lbl_ip.Text)
                    {
                        chrm_pr.Kind = ProxyKind.Manual;
                        chrm_pr.IsAutoDetect = false;
                        chrm_pr.SslProxy = ip + ":" + port;
                        chrm_opt.Proxy = chrm_pr;
                        chrm_opt.AddArguments("ignore-certificate-errors");
                        //if (adres.Contains("neova")) chrm_opt.PageLoadStrategy = PageLoadStrategy.Eager;
                    }
                    chrm_driver = new ChromeDriver(chrm_service, chrm_opt);
                }
                catch (Exception ex)
                {
                    string hata = ex.ToString();
                    gn.LocalLoglaAsync(isimLBL.Text, "Chrome WebDriver kapalıysa IP eşit değilse Proxy ayarlayıp yeni Chrome WebDriver oluştururken", hata);
                    //MessageBox.Show("Driver Oluşturma Diğer Hata: " + hata);
                }
                //chrm_driver.Manage().Timeouts().PageLoad.
                try
                {
                    chrm_driver.Navigate().GoToUrl(adres);
                }
                catch (Exception ex)
                {
                    //hataları belirle CHNGR
                    string hata = ex.ToString();
                    gn.LocalLoglaAsync(isimLBL.Text, "Chrome WebDriver NAVİGATE ile gitmeye çalışırken", hata);
                }
                //new WebDriverWait(chrm_driver, new TimeSpan(0, 0, 250)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            else
            {
                chrm_driver.SwitchTo().Window(chrm_driver.WindowHandles[chrm_driver.WindowHandles.Count - 1]);
                IJavaScriptExecutor js = (IJavaScriptExecutor)chrm_driver;
                js.ExecuteScript("window.open();");
                chrm_driver.SwitchTo().Window(chrm_driver.WindowHandles.Last());
                chrm_driver.Navigate().GoToUrl(adres);
            }

            haniymis = chrm_driver.WindowHandles.Count.ToString();
        }
        //---------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------
        public static Bitmap SiyahYatayCizgiTemizle(Image degiscekimg)
        {
            try
            {
                Color piksel, pikselust, pikselalt;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.R == 0)  //eğer siyahsa
                        {
                            pikselust = img1.GetPixel(i, j - 1);
                            pikselalt = img1.GetPixel(i, j + 1);

                            if (pikselalt.R == 255 && pikselust.R == 255)
                            {
                                newBitmap.SetPixel(i, j, Color.White);
                            }

                        }
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap SiyahYatayCizgiTemizleWithMargin(Image degiscekimg)
        {
            Bitmap newBitmap = new Bitmap(degiscekimg);
            try
            {
                Color piksel, pikselust, pikselalt;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        //if (i < 3 || j < 3 || i > img1.Width - 1 - 3 || j > img1.Height - 1 - 3)
                        //if (i < 3 || j < 3 || i > img1.Width - 1 - 3 || j > img1.Height)
                        if (i < 5 || j < 3)
                        {
                            newBitmap.SetPixel(i, j, Color.White);
                        }
                        else if (i > img1.Width - 5 || j > img1.Height - 2)
                        {
                            newBitmap.SetPixel(i, j, Color.White);
                        }
                        else
                        {
                            piksel = img1.GetPixel(i, j);

                            if (piksel.R == 0)  //eğer siyahsa
                            {
                                pikselust = newBitmap.GetPixel(i, j - 1);
                                pikselalt = newBitmap.GetPixel(i, j + 1);

                                if (pikselalt.R == 255 && pikselust.R == 255)
                                {
                                    newBitmap.SetPixel(i, j, Color.White);
                                }
                            }
                        }
                    }
                }
                return newBitmap;
            }
            catch (Exception exx)
            {
                return (Bitmap)newBitmap;
            }
        }

        public static Bitmap SiyahYatayCizgiTemizleQuickMarginli(Image degiscekimg)
        {
            Bitmap newBitmap = new Bitmap(degiscekimg);
            try
            {
                Color piksel, pikselust, pikselalt;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        if (i < 3 || j < 3 || i > img1.Width - 1 - 3 || j > img1.Height - 1 - 3)
                        {
                            newBitmap.SetPixel(i, j, Color.White);
                        }
                        else
                        {
                            piksel = img1.GetPixel(i, j);

                            if (piksel.R == 0)  //eğer siyahsa
                            {
                                pikselust = img1.GetPixel(i, j - 1);
                                pikselalt = img1.GetPixel(i, j + 1);

                                if (pikselalt.R == 255 && pikselust.R == 255)
                                {
                                    newBitmap.SetPixel(i, j, Color.White);
                                }

                            }
                        }
                    }
                }
                return newBitmap;
            }
            catch (Exception exx)
            {
                return (Bitmap)newBitmap;
            }
        }

        public static Bitmap SiyahYatay2xCizgiTemizleQuickMarginli(Image degiscekimg)
        {
            Bitmap image = new Bitmap(degiscekimg);
            Bitmap newBitmap = new Bitmap(degiscekimg);
            Color piksel, piksel2, piksel3, pikselalt, pikselust, renkla;
            int x, y, pos1x, pos1y, pos2x, pos2y;


            //degiscekimg.Save(@"C:\CMSigorta\" + DateTime.Now.ToFileTime() + "_______________" + "chngrq" + "______________.png", ImageFormat.Png);

            // Her pikseli tarayarak arama yap
            try
            {
                for (x = 5; x < image.Width - 5; x++)
                {
                    for (y = 3; y < image.Height - 1; y++)
                    {

                        piksel = image.GetPixel(x, y);

                        if (piksel.R == 0)  //eğer siyahsa
                        {
                            piksel2 = image.GetPixel(x, y + 1);
                            pikselalt = image.GetPixel(x, y + 2);
                            pikselust = image.GetPixel(x, y - 1);

                            // hemen altındaki piksel siyah mı diye kontrol edeceğiz
                            if (piksel2.R == 0 && pikselalt.R == 255 && pikselust.R == 255)  //eğer hemen altı siyahsa ve bu iki beyazın altı ve üstleri beyazsa
                            {
                                if (image.GetPixel(x - 1, y).R == 255 && image.GetPixel(x - 1, y + 1).R == 255)
                                {
                                    pos1x = x;
                                    pos1y = y;

                                    for (int dx = x + 1; dx < image.Width - 1; dx++)
                                    {
                                        //Console.WriteLine(dx.ToString());

                                        if (image.GetPixel(dx, y).R == 0 && image.GetPixel(dx, y + 1).R == 0 && image.GetPixel(dx, y - 1).R == 255 && image.GetPixel(dx, y + 2).R == 255)
                                        {
                                            //image.SetPixel(dx, y, Color.Red);
                                            //image.SetPixel(dx, y + 1, Color.Red);
                                            //Application.DoEvents();
                                        }
                                        else
                                        {
                                            if (image.GetPixel(dx, y).R == 255 && image.GetPixel(dx, y + 1).R == 255 && image.GetPixel(dx, y - 1).R == 255 && image.GetPixel(dx, y + 2).R == 255)
                                            {
                                                pos2x = dx - 1;
                                                pos2y = y;

                                                // Dikdörtgeni oluşturmak için bir Rectangle nesnesi oluşturun
                                                Rectangle rect = new Rectangle(pos1x, pos1y, pos2x - pos1x + 1, 2);

                                                // Dikdörtgenin rengini kırmızı yapmak için Graphics nesnesini kullanın
                                                using (Graphics g = Graphics.FromImage(image))
                                                {
                                                    g.FillRectangle(Brushes.White, rect);
                                                }
                                                //break;
                                            }
                                            break;
                                        }
                                    }
                                    //break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception expt)
            {
                //MessageBox.Show(expt.GetBaseException().ToString());
                //throw;
            }

            // İşlem tamamlandıktan sonra resmi kaydet
            return image;
        }


        public static Bitmap SiyahDikeyCizgiTemizle(Image degiscekimg)
        {
            try
            {
                Color piksel, pikselsag, pikselsol;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.R == 0)  //eğer siyahsa
                        {
                            pikselsag = img1.GetPixel(i + 1, j);
                            pikselsol = img1.GetPixel(i - 1, j);

                            if (pikselsag.R == 255 && pikselsol.R == 255)
                            {
                                newBitmap.SetPixel(i, j, Color.White);
                            }

                        }
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }


        public static Bitmap DikeyYatayTekTemizle(Image degiscekimg)
        {
            try
            {
                Color piksel, pikselsol, pikselsag, pikselust, pikselalt;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 1; i < img1.Width - 1; i++)
                {
                    for (int j = 1; j < img1.Height - 1; j++)
                    {
                        count1 = 0;
                        piksel = img1.GetPixel(i, j);
                        pikselsol = img1.GetPixel(i - 1, j);
                        pikselsag = img1.GetPixel(i + 1, j);
                        pikselust = img1.GetPixel(i, j - 1);
                        pikselalt = img1.GetPixel(i, j + 1);


                        if (piksel.R == 0) //eğer siyahsa
                        {
                            if (pikselsol.R == 0) count1++;
                            if (pikselsag.R == 0) count1++;
                            if (pikselust.R == 0) count1++;
                            if (pikselalt.R == 0) count1++;

                            if (count1 <= 1) newBitmap.SetPixel(i, j, Color.White);
                            //newBitmap.SetPixel(i, j, Color.White);
                        }
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap AnkaraRenkDegistir(Image degiscekimg)
        {
            try
            {
                Color piksel;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                //MessageBox.Show(img1.GetPixel(0, 16).ToString());
                //MessageBox.Show(img1.GetPixel(20, 5).ToString());

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.G > 200 && piksel.B > 200) //eğer gri nokta ise
                        {
                            newBitmap.SetPixel(i, j, Color.White);
                            //if (piksel.R != 0 && piksel.G != 0 && piksel.B != 0) newBitmap.SetPixel(i, j, Color.White);
                            //if (piksel.A != 255) newBitmap.SetPixel(i, j, Color.White);
                            //newBitmap.SetPixel(i, j, Color.Black);
                        }
                        else // eğer gri nokta değilse
                        {
                            if (piksel.R < 255 && piksel.G < 255 && piksel.B < 255)
                            {
                                newBitmap.SetPixel(i, j, Color.Black);
                            }
                            //newBitmap.SetPixel(i, j, Color.White);
                        }
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap QuickRenkDegistir(Image degiscekimg)
        {
            try
            {
                Color piksel;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.R > 145) //eğer pixel açık renk ise
                        {
                            newBitmap.SetPixel(i, j, Color.White);
                        }
                        else // eğer değilse
                        {
                            newBitmap.SetPixel(i, j, Color.Black);
                        }
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap AnkaraTemizle(Image degiscekimg)
        {
            try
            {
                Color piksel, pikselsol, pikselsag, pikselust, pikselalt;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                #region bütün resimlerde olan ortadaki çizgiyi silmek için dön
                //-------------------------------------------------------
                for (int i = 0; i < img1.Width; i++)
                {
                    count1 = 0;
                    pikselust = img1.GetPixel(i, 18);
                    pikselalt = img1.GetPixel(i, 20);

                    //if (pikselust.R == 255 && pikselust.G == 255 && pikselust.B == 255 && pikselalt.R == 255 && pikselalt.G == 255 && pikselalt.B == 255)
                    if (pikselust.R == 255 && pikselust.G == 255 && pikselust.B == 255)
                    {
                        count1++;
                    }

                    if (pikselalt.R == 255 && pikselalt.G == 255 && pikselalt.B == 255)
                    {
                        count1++;
                    }

                    if (count1 >= 1)
                    {
                        newBitmap.SetPixel(i, 19, Color.White);
                    }
                }
                //------------------------------------------------------- 
                #endregion


                img1 = newBitmap;


                //------------------------------------------------------- 
                for (int j = 0; j < img1.Height; j++)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        newBitmap.SetPixel(i, j, Color.White);
                        newBitmap.SetPixel((img1.Width - 1) - i, j, Color.White);
                    }
                }
                //------------------------------------------------------- 



                #region nokta lekeleri temizlemek için dön
                //-------------------------------------------------------
                for (int i = 1; i < img1.Width - 1; i++)
                {
                    for (int j = 1; j < img1.Height - 1; j++)
                    {
                        piksel = img1.GetPixel(i, j);
                        pikselsol = img1.GetPixel(i - 1, j);
                        pikselsag = img1.GetPixel(i + 1, j);
                        pikselust = img1.GetPixel(i, j - 1);
                        pikselalt = img1.GetPixel(i, j + 1);

                        /*
                        #region eski hali
                        if (pikselsol.R == 255 && pikselsol.G == 255 && pikselsol.B == 255 && pikselsol.A == 255 && pikselsag.R == 255 && pikselsag.G == 255 && pikselsag.B == 255 && pikselsag.A == 255)
                        {
                            //newBitmap.SetPixel(i, j, Color.White);
                            //Console.Beep(8000, 100);
                        }
                        else
                        {
                            if (pikselust.R == 255 && pikselust.G == 255 && pikselust.B == 255 && pikselust.A == 255 && pikselalt.R == 255 && pikselalt.G == 255 && pikselalt.B == 255 && pikselalt.A == 255)
                            {
                                newBitmap.SetPixel(i, j, Color.White);
                            }
                            else newBitmap.SetPixel(i, j, Color.Black);
                        } 
                        #endregion
*/

                        //şuan her yer siyah yada beyaz
                        #region yeni hali 
                        if (piksel.R == 0) // eğer siyah ise
                        {
                            if (pikselust.R == 255 && pikselalt.R == 255) //altı ve üstü beyaz ise 
                            {
                                if (pikselsol.R == 255 || pikselsag.R == 255) // solu yada sağı beyaz ise
                                {
                                    newBitmap.SetPixel(i, j, Color.White);
                                }
                                else
                                {
                                    newBitmap.SetPixel(i, j, Color.White);
                                }
                            }

                            /*
                            if (pikselsol.R == 0 && pikselsol.G == 255 && pikselsol.B == 255 && pikselsol.A == 255 && pikselsag.R == 255 && pikselsag.G == 255 && pikselsag.B == 255 && pikselsag.A == 255)
                            {
                                //newBitmap.SetPixel(i, j, Color.White);
                                //Console.Beep(8000, 100);
                            }
                            else
                            {
                                if (pikselust.R == 255 && pikselust.G == 255 && pikselust.B == 255 && pikselust.A == 255 && pikselalt.R == 255 && pikselalt.G == 255 && pikselalt.B == 255 && pikselalt.A == 255)
                                {
                                    newBitmap.SetPixel(i, j, Color.White);
                                }
                                else newBitmap.SetPixel(i, j, Color.Black);
                            }
                            */
                        }




                        #endregion


                        count1++;
                        //if (piksel != Color.Black) newBitmap.SetPixel(i, j, Color.White);
                    }

                }
                //-------------------------------------------------------
                #endregion



                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap AnkaraDikey2liTemizle(Image degiscekimg)
        {
            Color piksel1, piksel2, px1, px2, px3, px4, px5, px6;
            //string img1_ref, RR, GG, BB;
            int count1 = 0;
            Bitmap img1 = new Bitmap(degiscekimg);
            Bitmap newBitmap = new Bitmap(degiscekimg);

            try
            {
                for (int i = 1; i < img1.Width - 2; i++)
                {
                    for (int j = 1; j < img1.Height - 2; j++)
                    {
                        //if (i == 100 && j == 5) Console.Beep();

                        piksel1 = img1.GetPixel(i, j);
                        piksel2 = img1.GetPixel(i, j + 1);
                        px1 = img1.GetPixel(i, j - 1);
                        px2 = img1.GetPixel(i + 1, j);
                        px3 = img1.GetPixel(i + 1, j + 1);
                        px4 = img1.GetPixel(i, j + 2);
                        px5 = img1.GetPixel(i - 1, j + 1);
                        px6 = img1.GetPixel(i - 1, j);
                        count1 = 0;

                        if (piksel1.A == 255 && piksel1.R == 0 && piksel1.G == 0 && piksel1.B == 0) //eğer siyah ise
                        {
                            if (piksel2.A == 255 && piksel2.R == 0 && piksel2.G == 0 && piksel2.B == 0) // buda siyah ise
                            {
                                if (px1.A == 255 && px1.R == 255 && px1.G == 255 && px1.B == 255)
                                {
                                    count1++;
                                }
                                if (px2.A == 255 && px2.R == 255 && px2.G == 255 && px2.B == 255)
                                {
                                    count1++;
                                }
                                if (px3.A == 255 && px3.R == 255 && px3.G == 255 && px3.B == 255)
                                {
                                    count1++;
                                }
                                if (px4.A == 255 && px4.R == 255 && px4.G == 255 && px4.B == 255)
                                {
                                    count1++;
                                }
                                if (px5.A == 255 && px5.R == 255 && px5.G == 255 && px5.B == 255)
                                {
                                    count1++;
                                }
                                if (px6.A == 255 && px6.R == 255 && px6.G == 255 && px6.B == 255)
                                {
                                    count1++;
                                }
                            }


                            if (count1 > 5)
                            {
                                newBitmap.SetPixel(i, j, Color.White);
                                newBitmap.SetPixel(i, j + 1, Color.White);
                            }


                        }
                    }
                }


                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
                //return newBitmap;
            }
        }

        public static Bitmap NeovaRenkDegistir(Image degiscekimg)
        {
            try
            {
                Color piksel;
                //string img1_ref, RR, GG, BB;
                int count1 = 0, count2 = 0, RiR, GiG, BiB;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                //MessageBox.Show(img1.GetPixel(0, 16).ToString());
                //MessageBox.Show(img1.GetPixel(20, 5).ToString());

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);


                        /* 
                        if (piksel == Color.Transparent)  çalışmıyor transparet sorgusu
                        {
                            
                            Application.DoEvents();
                        }
   
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img1_ref = img1_ref.Replace("Color [A", "A");
                        img1_ref = img1_ref.Replace(" ", "");
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 2));
                        img1_ref = img1_ref.Replace("R=", "");
                        img1_ref = img1_ref.Replace("G=", "");
                        img1_ref = img1_ref.Replace("B=", "");
                        RR = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",")+1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        GG = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        BB = img1_ref;
                        RiR = Convert.ToInt32(RR);
                        GiG = Convert.ToInt32(GG);
                        BiB = Convert.ToInt32(BB);
                        */

                        //if (piksel.R == 0 || piksel.G == 0 || piksel.B == 0 || piksel.A == 0)
                        if (piksel.R == 0 && piksel.G == 0 && piksel.B == 0)
                        {
                            //if (piksel.R != 0 && piksel.G != 0 && piksel.B != 0) newBitmap.SetPixel(i, j, Color.White);
                            if (piksel.A != 255) newBitmap.SetPixel(i, j, Color.White);
                            //newBitmap.SetPixel(i, j, Color.Black);
                        }
                        else
                        {

                            newBitmap.SetPixel(i, j, Color.White);
                        }




                        /*

                        if (piksel.R < 200 || piksel.G < 200 || piksel.B < 200)
                        {
                            if (piksel.R != 0 && piksel.G != 0 && piksel.B != 0) newBitmap.SetPixel(i, j, Color.White);

                            //newBitmap.SetPixel(i, j, Color.Black);
                        }
                        else
                        {
                            newBitmap.SetPixel(i, j, Color.Black);
                            //if (piksel.G < 150 && piksel.B < 150)
                            //{
                            //    newBitmap.SetPixel(i, j, Color.White);
                            //}
                        }
                        if (piksel.A == 255 && piksel.R == 0) newBitmap.SetPixel(i, j, Color.White);

                        */





                        count1++;
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap NeovaTemizle(Image degiscekimg)
        {
            try
            {
                Color piksel, pikselsol, pikselsag, pikselust, pikselalt;
                //string img1_ref, RR, GG, BB;
                int count1 = 0;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 1; i < img1.Width - 1; i++)
                {
                    for (int j = 1; j < img1.Height - 1; j++)
                    {
                        piksel = img1.GetPixel(i, j);
                        pikselsol = img1.GetPixel(i - 1, j);
                        pikselsag = img1.GetPixel(i + 1, j);
                        pikselust = img1.GetPixel(i, j - 1);
                        pikselalt = img1.GetPixel(i, j + 1);

                        /*    
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img1_ref = img1_ref.Replace("Color [A", "A");
                        img1_ref = img1_ref.Replace(" ", "");
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 2));
                        img1_ref = img1_ref.Replace("R=", "");
                        img1_ref = img1_ref.Replace("G=", "");
                        img1_ref = img1_ref.Replace("B=", "");
                        RR = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",")+1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        GG = img1_ref.Substring(0, img1_ref.IndexOf(","));
                        img1_ref = img1_ref.Substring(img1_ref.IndexOf(",") + 1, (img1_ref.Length) - (img1_ref.IndexOf(",") + 1));
                        BB = img1_ref;
                        RiR = Convert.ToInt32(RR);
                        GiG = Convert.ToInt32(GG);
                        BiB = Convert.ToInt32(BB);
                        */

                        if (piksel.R == 0 && piksel.G == 0 && piksel.B == 0 && piksel.A == 255) //eğer siyahsa
                        //if (piksel.A == 255) //eğer siyahsa
                        {
                            //newBitmap.SetPixel(i, j, Color.White);
                        }
                        else // eğer siyah değilse
                        {
                            if (pikselsol.R == 0 && pikselsol.G == 0 && pikselsol.B == 0 && pikselsag.R == 0 && pikselsag.G == 0 && pikselsag.B == 0)
                            {
                                newBitmap.SetPixel(i, j, Color.Black);
                            }
                            else
                            {
                                if (pikselust.R == 0 && pikselust.G == 0 && pikselust.B == 0 && pikselalt.R == 0 && pikselalt.G == 0 && pikselalt.B == 0)
                                {
                                    newBitmap.SetPixel(i, j, Color.Black);
                                }
                                else newBitmap.SetPixel(i, j, Color.White);
                            }
                        }
                        if (piksel == null) newBitmap.SetPixel(i, j, Color.White);
                        count1++;
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        static Image FixedSizeTo500(Image imgPhoto)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            //int sourceX = 0;
            //int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)490 / (float)sourceWidth);
            nPercentH = ((float)490 / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((490 -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((490 -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(imgPhoto, new Size(destWidth, destHeight));

            return bmPhoto;
        }

        public static Bitmap Contrastit(Bitmap sourceBitmap, int threshold)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);


            double blue = 0;
            double green = 0;
            double red = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                red = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }


                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }


                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static Bitmap AxaYaziRenkDegistir(Image degiscekimg)
        {
            try
            {
                Color piksel;
                int count1 = 0, count2 = 0, RiR, GiG, BiB;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);
                        int degerxxx = 110;

                        if (piksel.R > degerxxx && piksel.G > degerxxx && piksel.B > degerxxx)
                        {
                            newBitmap.SetPixel(i, j, Color.White);
                        }
                        else
                        {
                            newBitmap.SetPixel(i, j, Color.Black);
                        }
                        count1++;
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap CropWhiteSpace(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int white = 0xffffff;

            Func<int, bool> allWhiteRow = r =>
            {
                for (int i = 0; i < w; ++i)
                    if ((bmp.GetPixel(i, r).ToArgb() & white) != white)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = c =>
            {
                for (int i = 0; i < h; ++i)
                    if ((bmp.GetPixel(c, i).ToArgb() & white) != white)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (!allWhiteRow(row))
                    break;
                topmost = row;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (!allWhiteRow(row))
                    break;
                bottommost = row;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (!allWhiteColumn(col))
                    break;
                leftmost = col;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (!allWhiteColumn(col))
                    break;
                rightmost = col;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                //throw
                return bmp;
            }
        }

        public Bitmap RemoveNoise(Bitmap bmap)
        {

            for (var x = 0; x < bmap.Width; x++)
            {
                for (var y = 0; y < bmap.Height; y++)
                {
                    var pixel = bmap.GetPixel(x, y);
                    if (pixel.R < 162 && pixel.G < 162 && pixel.B < 162)
                        bmap.SetPixel(x, y, Color.Black);
                    else if (pixel.R > 162 && pixel.G > 162 && pixel.B > 162)
                        bmap.SetPixel(x, y, Color.White);
                }
            }

            return bmap;
        }

        public Bitmap SetGrayscale(Bitmap img)
        {

            Bitmap temp = (Bitmap)img;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return (Bitmap)bmap.Clone();

        }

        public static Bitmap AyarliRenkDegistir(Image degiscekimg)
        {
            try
            {
                Color piksel;
                Bitmap img1 = new Bitmap(degiscekimg);
                Bitmap newBitmap = new Bitmap(degiscekimg);

                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        piksel = img1.GetPixel(i, j);

                        if (piksel.R > 195)
                        {
                            newBitmap.SetPixel(i, j, Color.White);
                        }
                        else
                        {
                            //if (piksel.R < 255 && piksel.G < 255 && piksel.B < 255)
                            //{
                            //    newBitmap.SetPixel(i, j, Color.Black);
                            //}
                            newBitmap.SetPixel(i, j, Color.Black);
                        }
                    }
                }
                return newBitmap;
            }
            catch
            {
                return (Bitmap)degiscekimg;
            }
        }

        public static Bitmap ProcessImageOhaa(Image degiscekimg)
        {
            // Bitmap resmini yükle
            Bitmap image = new Bitmap(degiscekimg);
            Bitmap newBitmap = new Bitmap(degiscekimg);
            Color piksel, piksel2, piksel3, pikselalt, pikselust, renkla;
            int x, y, pos1x, pos1y, pos2x, pos2y;


            degiscekimg.Save(@"C:\CMSigorta\" + DateTime.Now.ToFileTime() + "_______________" + "chngrq" + "______________.png", ImageFormat.Png);

            // Her pikseli tarayarak arama yap
            try
            {
                for (x = 5; x < image.Width - 5; x++)
                {
                    for (y = 3; y < image.Height - 1; y++)
                    {

                        piksel = image.GetPixel(x, y);

                        if (piksel.R == 0)  //eğer siyahsa
                        {
                            piksel2 = image.GetPixel(x, y + 1);
                            pikselalt = image.GetPixel(x, y + 2);
                            pikselust = image.GetPixel(x, y - 1);

                            // hemen altındaki piksel siyah mı diye kontrol edeceğiz
                            if (piksel2.R == 0 && pikselalt.R == 255 && pikselust.R == 255)  //eğer hemen altı siyahsa ve bu iki beyazın altıve üstleri beyazsa
                            {
                                if (image.GetPixel(x - 1, y).R == 255 && image.GetPixel(x - 1, y + 1).R == 255)
                                {
                                    pos1x = x;
                                    pos1y = y;

                                    for (int dx = x; dx < image.Width - 1; dx++)
                                    {
                                        Console.WriteLine(dx.ToString());

                                        if (image.GetPixel(dx, y).R == 0 && image.GetPixel(dx, y + 1).R == 0 && image.GetPixel(dx, y - 1).R == 255 && image.GetPixel(dx, y + 2).R == 255)
                                        {
                                            //image.SetPixel(dx, y, Color.Red);
                                            //image.SetPixel(dx, y + 1, Color.Red);
                                            //Application.DoEvents();
                                        }
                                        else
                                        {
                                            if (image.GetPixel(dx, y).R == 255 && image.GetPixel(dx, y + 1).R == 255 && image.GetPixel(dx, y - 1).R == 255 && image.GetPixel(dx, y + 2).R == 255)
                                            {
                                                pos2x = dx - 1;
                                                pos2y = y;

                                                // Dikdörtgeni oluşturmak için bir Rectangle nesnesi oluşturun
                                                Rectangle rect = new Rectangle(pos1x, pos1y, pos2x - pos1x + 1, 2);

                                                // Dikdörtgenin rengini kırmızı yapmak için Graphics nesnesini kullanın
                                                using (Graphics g = Graphics.FromImage(image))
                                                {
                                                    g.FillRectangle(Brushes.White, rect);
                                                }

                                                //break;
                                            }
                                            break;
                                        }

                                    }
                                    //break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception expt)
            {
                //MessageBox.Show(expt.GetBaseException().ToString());
                //throw;
            }
            // İşlem tamamlandıktan sonra resmi kaydet
            return image;
        }

        private void Yeni_GirisYap(IWebDriver driveer)
        {
            WebDriverWait wait = new WebDriverWait(driveer, new TimeSpan(0, 0, 30));
            WebDriverWait wait2 = new WebDriverWait(driveer, new TimeSpan(0, 0, 10));
            WebDriverWait wait3 = new WebDriverWait(driveer, new TimeSpan(0, 0, 60));
            SelectElement select;

            string UrlAdres = "";
            string gelenlink = "";
            string adi = "";
            string key = "";
            //driver = chrm_driver;
            //wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));


            try
            {
                UrlAdres = driveer.Url.ToString();
                //if (tarayici == "ie") UrlAdres = ie_driver.Url.ToString();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }
            if (UrlAdres == "") MessageBox.Show("URL Adresi Boş. Panelden Şirket ayarlarını kontrol edin...");

            string link = "";
            string KullaniciID = "";
            string SifreID = "";
            string LoginID = "";

            // şirketin login ayarlarnı çekmek için XML okuma döngüsü yapılacak
            try
            {
                string xmlfile = gn.SelenXml;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlfile);
                XmlNodeList newXMLNodes = xdoc.SelectNodes("/sirketler/sirket");

                foreach (XmlNode newXMLNode in newXMLNodes)
                {
                    link = newXMLNode["link"].InnerText;
                    if (UrlAdres.Contains(link))
                    {
                        KullaniciID = newXMLNode["kullaniciadi"].InnerText;
                        SifreID = newXMLNode["sifre"].InnerText;
                        LoginID = newXMLNode["login"].InnerText;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Giriş yapılan şirketin linkine göre Login Ayarlarını çekerken hata: ", hata);

            }

            //Elementte yazan yazıyı görene kadar bekleteceğimiz durum sabit duran ama yazısı değişen elementler için
            //IWebElement formDurummm = driver.FindElement(By.XPath(XPathyolu));
            //wait.Until(ExpectedConditions.TextToBePresentInElement(formDurummm, "Form"));


            // RAY SFS kalmadı İE ile açılacak bir RAY yok artık. Yeni SFS Ray Express ten Chrome ile açılıyor zaten.

            int smsDenemeAdet = 0;
            // şirket girişlerini yapan döngüler
            try
            {
                //LOGIN ID dolu olduğunda - yani LOGIN buttonu standart olarak XML de sunulduğunda
                if (LoginID != "")
                {
                    switch (UrlAdres)
                    {
                        //--------------------------------------------------------------------------------------
                        //TRAMER     GİRİŞ
                        #region TRAMER
                        case string x when UrlAdres.Contains("online.sbm.org.tr"):
                            wait2.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                            driveer.FindElement(By.Id(KullaniciID)).Clear();
                            driveer.FindElement(By.Id(KullaniciID)).SendKeys(lbl_tramer_adi.Text);
                            driveer.FindElement(By.Id(SifreID)).SendKeys(lbl_tramer_sifre.Text);
                            driveer.FindElement(By.Id(LoginID)).Click();
                            break;
                        #endregion

                        //-------------------------------------------------------------------------------------- // Ray Ekspress'de ID ler pek düzgün olmadığı için XPATH kullandık
                        //RAY EXPRESS     GİRİŞ
                        #region RAY EXPRESS
                        case string x when UrlAdres.Contains("rayexpress"):
                        Etiket_RayExpress_LoginDeneme:
                            try
                            {
                                try
                                {
                                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(KullaniciID)));
                                }
                                catch (Exception)
                                {
                                    //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise
                                    MessageBox.Show("Ray Express Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!");
                                    goto RayExpress_BeklenmeyenSonlanma;
                                }
                                if (IsElementPresent(By.XPath(KullaniciID), driveer))
                                {
                                    try
                                    {
                                        //JavaScript ile giriş yaptırınca Login dğmesi aktif olmuyor. 
                                        //HtmlInputTemizleVeDoldur(driveer, KullaniciID, KullaniciAdi);
                                        //HtmlInputTemizleVeDoldur(driveer, SifreID, Sifre);
                                        driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                        driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.XPath(SifreID)).Clear();
                                        driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);
                                        driveer.FindElement(By.XPath(LoginID)).Click();
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama
                                        goto Etiket_RayExpress_LoginDeneme;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama
                            }

                        Etiket_RayExpress_SMSistedimi:
                            try
                            {
                                try
                                {
                                    // SMS gönder yazan buton var mı diye bak
                                    wait2.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[contains(@ng-click,'SmsSend') and @type='button']")));
                                }
                                catch (Exception ex)
                                {
                                    // SMS gönderimi yapılacak button bulunamaz ise
                                    goto Etiket_RayExpress_Girdimi;
                                }
                                try
                                {
                                    //eski ekranın eslect seçme yönetmi burada dursun.
                                    //select = new SelectElement(driveer.FindElement(By.XPath("//select[@ng-model='OtpTelefonNumarasi']")));
                                    //select.SelectByText((smsnumberno.Substring(0, 1) + "**" + smsnumberno.Substring(3, 1) + "***" + smsnumberno.Substring(smsnumberno.Length - 3)), true);
                                    //driveer.FindElement(By.XPath("//button[contains(text(),'SMS Gönder')]")).Click();
                                    driveer.FindElement(By.XPath("//span[contains(@class,'selection__arrow')]")).Click();
                                    Thread.Sleep(500);
                                    string maskelitelefonno = (smsnumberno.Substring(0, 1) + "**" + smsnumberno.Substring(3, 1) + "***" + smsnumberno.Substring(smsnumberno.Length - 3));
                                    driveer.FindElement(By.XPath("//li[contains(text(),'" + maskelitelefonno + "')]")).Click();
                                    Thread.Sleep(500);
                                    driveer.FindElement(By.XPath("//input[contains(@ng-click,'SmsSend') and @type='button']")).Click();
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla                                
                            }



                        Etiket_RayExpress_SMSGiris:
                            try
                            {
                                Thread.Sleep(5000);
                                //Console.Beep();
                                try
                                {
                                    wait2.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@ng-model='OtpSmsSifre']")));
                                }
                                catch (Exception ex)
                                {
                                    // SMS şifre girlecek kutu bulanamaz ise başa dön
                                    goto Etiket_RayExpress_SMSGiris;
                                }
                                if (IsElementPresent(By.XPath("//input[@ng-model='OtpSmsSifre']"), driveer))
                                {
                                    try
                                    {
                                        //SMS giriş işlemler eski Timer buraya Alınacak
                                        string sonuc = "";
                                        string sonuc2 = "";

                                        Thread.Sleep(3000);

                                        // SMS gönderildikten sonraki ilk zamanı oluşturalım.
                                        DateTime myDatee = DateTime.Now;
                                        DateTime d1 = myDatee.AddSeconds(-3);
                                        string myZaman1r = d1.ToString();
                                        myZaman1r = myZaman1r.Replace(".", "-");
                                        myZaman1r = myZaman1r.Replace(" 0", " ");
                                        DateTime d2 = myDatee.AddSeconds(3);
                                        string myZaman2r = d2.ToString();
                                        myZaman2r = myZaman2r.Replace(".", "-");
                                        myZaman2r = myZaman2r.Replace(" 0", " ");

                                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and TelNo='" + smsnumberno + "' and Durum='0' and Tarih BETWEEN '" + myZaman1r + "' AND '" + myZaman2r + "'");
                                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and TelNo='" + smsnumberno + "' and Durum='0' order by id desc limit 1");
                                        if (sonuc != "")
                                        {
                                            driveer.FindElement(By.XPath("//input[@ng-model='OtpSmsSifre']")).Clear();
                                            driveer.FindElement(By.XPath("//input[@ng-model='OtpSmsSifre']")).SendKeys(sonuc);
                                            //driveer.FindElement(By.XPath("//input[@ng-model='OtpSmsSifre']")).SendKeys(OpenQA.Selenium.Keys.Enter);    // enter çalışmadı   
                                            driveer.FindElement(By.XPath("//input[contains(@ng-click,'SmsGiris')]")).Click();
                                        }
                                        else
                                        {
                                            goto Etiket_RayExpress_SMSGiris;
                                        }

                                        //driveer.FindElement(By.Id("BtnOtpSmsGiris")).Click();
                                        List<string> TabloAdlari = new List<string>();
                                        TabloAdlari.Add("Durum");
                                        ArrayList veriler = new ArrayList();
                                        veriler.Add("1");
                                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama logla
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                            }

                        Etiket_RayExpress_Girdimi:
                            try
                            {
                                wait2.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='i-welcome']")));
                                if (IsElementPresent(By.XPath("//div[@class='i-welcome']"), driveer))
                                {
                                    //Giriş başarılı
                                    //MessageBox.Show("Ray Express AnaSayfaya Hoşgeldiniz");
                                }
                            }
                            catch
                            {

                            }

                        RayExpress_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch
                            {

                            }

                            break;
                        #endregion

                        //--------------------------------------------------------------------------------------  // Ak Sigorta uzun zamandır aynı ID leri kullandığı için ID kullandık
                        //AK SİGORTA     GİRİŞ
                        #region AK SİGORTA
                        case string x when UrlAdres.Contains("sat2.aksigorta.com.tr"):
                        Etiket_AkSigorta_LoginDeneme:
                            try
                            {
                                try
                                {
                                    //Zaten giriş yapılmış mı diye kontrol et
                                    if (IsElementPresent(By.XPath("//a[@id='dls-lnk-menu']"), driveer))
                                    {
                                        //driveer.FindElement(By.XPath("//a[@id='dls-lnk-menu']")).Click();
                                        driveer.Navigate().GoToUrl("https://sat2.aksigorta.com.tr/urun/musteri-sorgula");
                                        Thread.Sleep(2000);
                                        if (IsElementPresent(By.XPath("//input[@id='msb-musteri-tc-no']"), driveer))
                                        {
                                            //Ak Sigorta Zaten Açık
                                            goto AkSigorta_BeklenmeyenSonlanma;
                                        }
                                        else
                                        {
                                            goto Etiket_AkSigorta_LoginDeneme;
                                        }
                                    }
                                    else
                                    {
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise AK SİGORTA da sorun var demek sonlandırma yap                                
                                    Trayyy1.ShowBalloonTip(100, "AK SİGORTA", "Ak Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                    goto AkSigorta_BeklenmeyenSonlanma;
                                }
                                if (IsElementPresent(By.Id(KullaniciID), driveer))
                                {
                                    try
                                    {
                                        driveer.FindElement(By.Id(KullaniciID)).Clear();
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(SifreID)).Clear();
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(LoginID)));
                                        driveer.FindElement(By.Id(LoginID)).Click();
                                    }
                                    catch (Exception)
                                    {
                                        //hata ayıklama
                                        goto Etiket_AkSigorta_LoginDeneme;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama
                            }

                        Etiket_AkSigorta_SMSistedimi:
                            try
                            {
                                try
                                {
                                    wait2.Until(ExpectedConditions.ElementToBeClickable(By.Id("smsPassword")));
                                }
                                catch (Exception ex)
                                {
                                    // SMS girişi yapılacak input bulunamaz ise başa döneceğiz ama burası KONTROL edilmeli

                                    if (IsElementPresent(By.XPath("//div[@role='alertdialog']"), driveer))
                                    {
                                        Trayyy1.ShowBalloonTip(100, "AK SİGORTA", "Ak Sigorta Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                        goto AkSigorta_BeklenmeyenSonlanma;

                                    }
                                    else
                                    {
                                        goto Etiket_AkSigorta_SMSistedimi;
                                    }
                                }
                                try
                                {
                                    driveer.FindElement(By.Id("smsPassword")).Click(); // sadece kutuyu tıklıyoruz bişey yapmıyoruz şuan, kutu bulundu SMS çekilecek                                   
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla                                
                            }


                            //Thread.Sleep(5000);
                            smsDenemeAdet = 0;

                        Etiket_AkSigorta_SMSGiris:
                            try
                            {
                                smsDenemeAdet = smsDenemeAdet + 1;
                                //Console.Beep();

                                if (smsDenemeAdet < 10)
                                {
                                    if (IsElementPresent(By.XPath("//button[contains(text(),'Gönder')]"), driveer))
                                    {
                                        try
                                        {
                                            //SMS giriş işlemler eski Timer buraya Alınacak
                                            string sonuc = "";
                                            string sonuc2 = "";

                                            Thread.Sleep(3000);

                                            DateTime myDatee = DateTime.Now;
                                            DateTime d1 = myDatee.AddSeconds(-4);
                                            string myZaman1r = d1.ToString();
                                            myZaman1r = myZaman1r.Replace(".", "-");
                                            myZaman1r = myZaman1r.Replace(" 0", " ");
                                            DateTime d2 = myDatee.AddSeconds(3);
                                            string myZaman2r = d2.ToString();
                                            myZaman2r = myZaman2r.Replace(".", "-");
                                            myZaman2r = myZaman2r.Replace(" 0", " ");


                                            sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='AKSIGORTA' and TelNo='" + smsnumberno + "' and Durum='0' and Tarih BETWEEN '" + myZaman1r + "' AND '" + myZaman2r + "'");

                                            if (sonuc != "")
                                            {
                                                driveer.FindElement(By.Id("smsPassword")).Clear();
                                                driveer.FindElement(By.Id("smsPassword")).SendKeys(sonuc);
                                                //driveer.FindElement(By.XPath("//input[@ng-model='OtpSmsSifre']")).SendKeys(OpenQA.Selenium.Keys.Enter);    // enter çalışmadı   
                                                driveer.FindElement(By.XPath("//button[contains(text(),'Gönder')]")).Click();
                                            }
                                            else
                                            {
                                                goto Etiket_AkSigorta_SMSGiris;
                                            }

                                            List<string> TabloAdlari = new List<string>();
                                            TabloAdlari.Add("Durum");
                                            ArrayList veriler = new ArrayList();
                                            veriler.Add("1");
                                            sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                                        }
                                        catch (Exception ex)
                                        {
                                            //hata ayıklama logla
                                        }
                                    }
                                }
                                else
                                {
                                    Trayyy1.ShowBalloonTip(100, "AK SİGORTA SMS", "AK SİGORTA Sigorta SMS sorunu oluştu. Lütfen yeniden Açmayı deneyiniz!!!", ToolTipIcon.Error);
                                    goto AkSigorta_BeklenmeyenSonlanma;
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                goto Etiket_AkSigorta_SMSGiris;
                            }


                        AkSigorta_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch
                            {

                            }

                            break;
                        #endregion

                        //--------------------------------------------------------------------------------------  // Allianz Sigorta uzun zamandır aynı ID leri kullandığı için ID kullandık
                        //ALLİANZ SİGORTA     GİRİŞ
                        #region ALLİANZ SİGORTA
                        case string x when UrlAdres.Contains("allianz"):
                        Etiket_Allianz_LoginDeneme:
                            try
                            {
                                try
                                {
                                    //Zaten giriş yapılmış mı diye kontrol et
                                    if (IsElementPresent(By.XPath("//a[@id='dls-lnk-menu']"), driveer))
                                    {
                                        //driveer.FindElement(By.XPath("//a[@id='dls-lnk-menu']")).Click();
                                        driveer.Navigate().GoToUrl("https://sat2.aksigorta.com.tr/urun/musteri-sorgula");
                                        Thread.Sleep(2000);
                                        if (IsElementPresent(By.XPath("//input[@id='msb-musteri-tc-no']"), driveer))
                                        {
                                            //Allianz Sigorta Zaten Açık
                                            goto Allianz_BeklenmeyenSonlanma;
                                        }
                                        else
                                        {
                                            goto Etiket_Allianz_LoginDeneme;
                                        }
                                    }
                                    else
                                    {
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                    }

                                }
                                catch (Exception ex)
                                {
                                    //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise                                   
                                    Trayyy1.ShowBalloonTip(100, "ALLİANZ SİGORTA", "Allianz Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                    goto Allianz_BeklenmeyenSonlanma;
                                }
                                if (IsElementPresent(By.Id(KullaniciID), driveer))
                                {
                                    try
                                    {
                                        driveer.FindElement(By.Id(KullaniciID)).Clear();
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(SifreID)).Clear();
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                        //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(LoginID)));
                                        //driveer.FindElement(By.Id(LoginID)).Click();
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                    }
                                    catch (Exception)
                                    {
                                        //hata ayıklama
                                        goto Etiket_Allianz_LoginDeneme;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama
                            }

                        Etiket_Allianz_SMSistedimi:
                            try
                            {
                                try
                                {
                                    wait2.Until(ExpectedConditions.ElementToBeClickable(By.Id("smsToken")));
                                }
                                catch (Exception ex)
                                {
                                    // SMS girişi yapılacak input bulunamaz ise başa döneceğiz ama burası KONTROL edilmeli
                                    if (IsElementPresent(By.XPath("//div[@id='msg']"), driveer))
                                    {
                                        Trayyy1.ShowBalloonTip(100, "ALLİANZ SİGORTA", "Allianz Sigorta Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                        goto Allianz_BeklenmeyenSonlanma;
                                    }
                                    else
                                    {
                                        goto Etiket_Allianz_SMSistedimi;
                                    }
                                }
                                try
                                {
                                    driveer.FindElement(By.Id("smsToken")).Click(); // sadece kutuyu tıklıyoruz bişey yapmıyoruz şuan, kutu bulundu SMS çekilecek                                   
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla                                
                            }


                            //Thread.Sleep(5000);
                            smsDenemeAdet = 0;

                        Etiket_Allianz_SMSGiris:
                            try
                            {
                                smsDenemeAdet = smsDenemeAdet + 1;
                                //Console.Beep();

                                if (smsDenemeAdet < 10)
                                {
                                    if (IsElementPresent(By.XPath("//button[@id='redirect']"), driveer))
                                    {
                                        try
                                        {
                                            //SMS giriş işlemler eski Timer buraya Alınacak
                                            string sonuc = "";
                                            string sonuc2 = "";

                                            Thread.Sleep(3000);

                                            DateTime myDatee = DateTime.Now;
                                            DateTime d1 = myDatee.AddSeconds(-4);
                                            string myZaman1r = d1.ToString();
                                            myZaman1r = myZaman1r.Replace(".", "-");
                                            myZaman1r = myZaman1r.Replace(" 0", " ");
                                            DateTime d2 = myDatee.AddSeconds(3);
                                            string myZaman2r = d2.ToString();
                                            myZaman2r = myZaman2r.Replace(".", "-");
                                            myZaman2r = myZaman2r.Replace(" 0", " ");


                                            sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='ALLIANZ' and TelNo='" + smsnumberno + "' and Durum='0' and Tarih BETWEEN '" + myZaman1r + "' AND '" + myZaman2r + "'");

                                            if (sonuc != "")
                                            {
                                                driveer.FindElement(By.Id("smsToken")).Clear();
                                                driveer.FindElement(By.Id("smsToken")).SendKeys(sonuc);
                                                //driveer.FindElement(By.XPath("//input[@ng-model='OtpSmsSifre']")).SendKeys(OpenQA.Selenium.Keys.Enter);    // enter çalışmadı   
                                                driveer.FindElement(By.XPath("//button[@id='redirect']")).Click();
                                            }
                                            else
                                            {
                                                goto Etiket_Allianz_SMSGiris;
                                            }

                                            List<string> TabloAdlari = new List<string>();
                                            TabloAdlari.Add("Durum");
                                            ArrayList veriler = new ArrayList();
                                            veriler.Add("1");
                                            sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                                        }
                                        catch (Exception ex)
                                        {
                                            //hata ayıklama logla
                                        }
                                    }
                                }
                                else
                                {
                                    Trayyy1.ShowBalloonTip(100, "ALLİANZ SİGORTA SMS", "SMS sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                    goto Allianz_BeklenmeyenSonlanma;
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                goto Etiket_Allianz_SMSGiris;
                            }


                        Allianz_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch
                            {

                            }

                            break;
                        #endregion

                        //--------------------------------------------------------------------------------------  // Neova Sigorta uzun zamandır aynı ID leri kullandığı için ID kullandık
                        //NEOVA SİGORTA     GİRİŞ
                        #region NEOVA SİGORTA
                        case string x when UrlAdres.Contains("neoport.neova"):
                        Etiket_Neova_LoginDeneme:
                            try
                            {
                                //chngr silinecek
                                driveer.Navigate().GoToUrl("https://neoport.neova.com.tr");
                                try
                                {
                                    //Zaten giriş yapılmış mı diye kontrol et
                                    Thread.Sleep(2000);
                                    if (IsElementPresent(By.XPath("//a[@id='nv-login']"), driveer))
                                    {
                                        //driveer.FindElement(By.XPath("//a[@id='nv-login']")).Click();
                                        driveer.Navigate().GoToUrl("https://neoport.neova.com.tr/_layouts/15/Smartiks.SSO/Sso.aspx?a=net");
                                        Thread.Sleep(2000);
                                        if (IsElementPresent(By.XPath("//div[@id='siteMenu']"), driveer))
                                        {
                                            //Neova Sigorta Zaten Açık
                                            goto Neova_BeklenmeyenSonlanma;
                                        }
                                        else
                                        {
                                            goto Etiket_Neova_2FAistedimi;
                                        }
                                    }
                                    else
                                    {
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                    }

                                }
                                catch (Exception ex)
                                {
                                    //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise                                   
                                    Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                    goto Neova_BeklenmeyenSonlanma;
                                }
                                if (IsElementPresent(By.Id(KullaniciID), driveer))
                                {
                                    try
                                    {
                                        driveer.FindElement(By.Id(KullaniciID)).Clear();
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(SifreID)).Clear();
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);


                                        if (IsElementPresent(By.XPath("//input[@placeholder='Güvenlik Kodu']"), driveer))
                                        {
                                            String neovacaptchasonuc = "";
                                            IWebElement neovacaptcharesim = driveer.FindElement(By.Id("ctl00_PlaceHolderMain_imgCptc"));
                                            //Image img = GetElementScreenShot(driveer, neovacaptcharesim);
                                            //string base64bu;
                                            //base64bu = cptch.ResimdenBase64e(img);

                                            string resimkodu = neovacaptcharesim.GetAttribute("src");
                                            //var img = GetElementScreenShot(driveer, resimkodu);
                                            resimkodu = resimkodu.Remove(0, 22);


                                            byte[] bytes = Convert.FromBase64String(resimkodu);
                                            Image img;
                                            using (MemoryStream ms = new MemoryStream(bytes))
                                            {
                                                img = Image.FromStream(ms);
                                            }

                                            Captcha cptchcs = new Captcha();
                                            Image captchresim;

                                            captchresim = NeovaRenkDegistir((Bitmap)img);
                                            captchresim = NeovaTemizle((Bitmap)captchresim);
                                            captchresim = NeovaTemizle((Bitmap)captchresim);
                                            captchresim = FixedSizeTo500(captchresim);
                                            neovacaptchasonuc = cptchcs.NeovaAritmetik(cptchcs.NeovaOku(captchresim));


                                            if (neovacaptchasonuc != "")
                                            {
                                                if (neovacaptchasonuc.Contains("KARAKTER"))
                                                {
                                                    //////////////////////////////////////////////////////////////////////////////
                                                    string folderName = @"C:\CMSigorta\cptchaerror\";
                                                    // If directory does not exist, create it
                                                    if (!Directory.Exists(folderName))
                                                    {
                                                        Directory.CreateDirectory(folderName);
                                                    }

                                                    img.Save(folderName + DateTime.Now.ToFileTime() + "_.png", ImageFormat.Png);
                                                    Thread.Sleep(100);
                                                    captchresim.Save(folderName + DateTime.Now.ToFileTime() + "__" + neovacaptchasonuc + "__.png", ImageFormat.Png);
                                                    //////////////////////////////////////////////////////////////////////////////
                                                    ///
                                                    goto Etiket_Neova_LoginDeneme;
                                                }
                                                else
                                                {
                                                    //Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Captcha çözüldü. " + neovacaptchasonuc, ToolTipIcon.Error);
                                                    driveer.FindElement(By.XPath("//input[@placeholder='Güvenlik Kodu']")).Clear();
                                                    driveer.FindElement(By.XPath("//input[@placeholder='Güvenlik Kodu']")).SendKeys(neovacaptchasonuc);
                                                }
                                            }
                                            else
                                            {
                                                Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Captcha Çözülemedi. Lütfen manuel giriş yapınız !!!", ToolTipIcon.Error);
                                            }

                                        }
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);

                                        Thread.Sleep(500);
                                        if (IsElementPresent(By.XPath("//div[@aria-hidden='false']"), driveer))
                                        {
                                            if (IsElementPresent(By.XPath("//div[contains(text(),'Güvenlik')]"), driveer))
                                            {
                                                //Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                                driveer.FindElement(By.XPath("//div[@aria-hidden='false']")).SendKeys(OpenQA.Selenium.Keys.Escape);
                                                driveer.Navigate().GoToUrl("https://neoport.neova.com.tr/_layouts/15/Neova.Authentication/CustomLogin.aspx?ReturnUrl=%2f_layouts%2f15%2fAuthenticate.aspx%3fSource%3d%252F&Source=%2F");
                                                goto Etiket_Neova_LoginDeneme;
                                            }
                                            else if (IsElementPresent(By.XPath("//div[contains(text(),'şifre')]"), driveer))
                                            {
                                                Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Girişte ŞİFRE HATASI mevcut. Şifre ayarı kontrol edilmeli.", ToolTipIcon.Error);
                                            }
                                            goto Neova_BeklenmeyenSonlanma;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama
                                        goto Etiket_Neova_LoginDeneme;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama
                            }

                        Etiket_Neova_2FAistedimi:
                            try
                            {
                                try
                                {
                                    wait2.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@id='nv-login']")));
                                    driveer.Navigate().GoToUrl("https://neoport.neova.com.tr/_layouts/15/Smartiks.SSO/Sso.aspx?a=net");
                                }
                                catch
                                {
                                    //buraya girmesini beklemiyorum
                                }

                                try
                                {
                                    wait2.Until(ExpectedConditions.ElementToBeClickable(By.Id("txtGACode")));

                                }
                                catch (Exception ex)
                                {
                                    if (IsElementPresent(By.XPath("//div[@id='siteMenu']"), driveer))
                                    {
                                        //Neova Sigorta Zaten Açık
                                        goto Neova_BeklenmeyenSonlanma;
                                    }
                                    // 2FA girişi yapılacak input wait2 ile bulunamaz ise başa döneceğiz
                                    goto Etiket_Neova_2FAistedimi;
                                }
                                try
                                {
                                    driveer.FindElement(By.Id("txtGACode")).Click(); // sadece kutuyu tıklıyoruz bişey yapmıyoruz şuan, kutu bulundu SMS çekilecek                                   
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla                                
                            }

                            GelenKullaniciAdi = KullaniciAdi;

                            string xmlfile = gn.KareKod;
                            XmlDocument xdoc = new XmlDocument();
                            xdoc.Load(xmlfile);
                            XmlNodeList newXMLNodes = xdoc.SelectNodes("/sirketler/sirket");
                            gelenlink = "";
                            adi = "";
                            key = "";
                            foreach (XmlNode newXMLNode in newXMLNodes)
                            {
                                gelenlink = newXMLNode["link"].InnerText;
                                adi = newXMLNode["kullaniciadi"].InnerText;
                                key = newXMLNode["key"].InnerText;
                                if (UrlAdres.Contains(gelenlink) && GelenKullaniciAdi == adi)
                                {
                                    break;
                                }
                            }

                            //Thread.Sleep(5000);
                            smsDenemeAdet = 0;

                        Etiket_Neova_2FAGiris:
                            try
                            {
                                smsDenemeAdet = smsDenemeAdet + 1;
                                Console.WriteLine("Neova 2FA deneme sayısı:" + smsDenemeAdet);

                                if (smsDenemeAdet < 3)
                                {
                                    try
                                    {
                                        if (IsElementPresent(By.Id("txtGACode"), driveer))
                                        {
                                            var bytes = OtpNet.Base32Encoding.ToBytes(key);
                                            var totp = new OtpNet.Totp(bytes);
                                            var remainingTime = totp.RemainingSeconds();
                                            int sayac = Convert.ToInt32(remainingTime);
                                            //if (sayac <= 3) await Task.Delay(4000);
                                            if (sayac <= 3) Task.Delay(4000);

                                            var result = totp.ComputeTotp();
                                            driveer.FindElement(By.Id("txtGACode")).Clear();
                                            driveer.FindElement(By.Id("txtGACode")).SendKeys(result);
                                            driveer.FindElement(By.Id("txtGACode")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            Thread.Sleep(500);
                                            //2FA denendikten sonra Hata varmı diye kontrol et
                                            if (IsElementPresent(By.XPath("//p[contains(text(),'Geçersiz')]"), driveer))
                                            {
                                                Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta 2FA Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                                //goto Neova_BeklenmeyenSonlanma;
                                                goto Etiket_Neova_2FAGiris;
                                            }
                                        }
                                        else
                                        {
                                            goto Etiket_Neova_2FAGiris;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama logla
                                        goto Etiket_Neova_2FAGiris;
                                    }
                                }
                                else
                                {
                                    Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta 2FA sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                    goto Neova_BeklenmeyenSonlanma;
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                goto Etiket_Neova_2FAGiris;
                            }


                        Neova_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch
                            {

                            }

                            break;
                        #endregion

                        //--------------------------------------------------------------------------------------  // Ankara Sigorta her girişte ID değiştirdiği için karışıkta olsa Rel xPath kullandık
                        //ANKARA SİGORTA     GİRİŞ
                        #region ANKARA SİGORTA
                        case string x when UrlAdres.Contains("online.ankarasigorta.com.tr"):
                        Etiket_Ankara_LoginDeneme:
                            try
                            {
                                try
                                {
                                    //Zaten giriş yapılmış mı diye kontrol et
                                    Thread.Sleep(2000);
                                    if (IsElementPresent(By.XPath("//header[@class='main-header']"), driveer))
                                    {
                                        goto Etiket_Ankara_2FAistedimi;
                                    }
                                    else
                                    {
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(KullaniciID)));
                                    }

                                }
                                catch (Exception ex)
                                {
                                    //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise                                   
                                    Trayyy1.ShowBalloonTip(100, "ANKARA SİGORTA", "Ankara Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                    goto Ankara_BeklenmeyenSonlanma;
                                }
                                if (IsElementPresent(By.XPath(KullaniciID), driveer))
                                {
                                    Image img = null;
                                    try
                                    {
                                        driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                        driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.XPath(SifreID)).Clear();
                                        driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);

                                        if (IsElementPresent(By.XPath("//input[@id='Captcha' and @class='form-control']"), driveer))
                                        {
                                            Trayyy1.ShowBalloonTip(100, "ANKARA SİGORTA", "Captcha çözme deneniyor, Lütfen Bekleyiniz...", ToolTipIcon.None);

                                            Actions actionz = new Actions(driveer);
                                            actionz.MoveToElement(driveer.FindElement(By.XPath("//input[@id='Captcha' and @class='form-control']"))).Click().Perform();


                                            String ankaracaptchasonuc = "";

                                            IWebElement ankaracaptcharesim = driveer.FindElement(By.XPath("//img[@class='captcha-image']"));
                                            img = GetElementScreenShot(driveer, ankaracaptcharesim);
                                            //string base64bu;
                                            //base64bu = cptch.ResimdenBase64e(img);
                                            Captcha cptch = new Captcha();
                                            //Image captchresim;

                                            img = AnkaraRenkDegistir((Bitmap)img);
                                            img = AnkaraTemizle((Bitmap)img);
                                            img = SiyahYatayCizgiTemizleWithMargin((Bitmap)img);
                                            img = SiyahYatayCizgiTemizleWithMargin((Bitmap)img);
                                            img = DikeyYatayTekTemizle((Bitmap)img);
                                            img = DikeyYatayTekTemizle((Bitmap)img);
                                            img = SiyahDikeyCizgiTemizle((Bitmap)img);
                                            img = SiyahYatayCizgiTemizle((Bitmap)img);
                                            img = AnkaraDikey2liTemizle((Bitmap)img);
                                            img = ProcessImageOhaa((Bitmap)img);


                                            //captchresim = FixedSizeTo500(captchresim);

                                            //ankaracaptchasonuc = cptch.NeovaAritmetik(cptch.NeovaOku(captchresim));
                                            //ankaracaptchasonuc = cptch.AnkaraOku((Bitmap)captchresim);


                                            try
                                            {
                                                ankaracaptchasonuc = OcrSpaceileCoz(img, "5");
                                                ankaracaptchasonuc = ankaracaptchasonuc.Replace(" ", "");
                                                ankaracaptchasonuc = RemoveSpecialCharacters(ankaracaptchasonuc);
                                            }
                                            catch
                                            {
                                                // bir yere goto olacak ama bakalım

                                            }

                                            //using (var ocrresim = OcrApi.Create())
                                            //{
                                            //    ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                                            //    ocrresim.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                                            //    string karakterx = ocrresim.GetTextFromImage((Bitmap)img);
                                            //    ankaracaptchasonuc = karakterx;
                                            //}

                                            if (ankaracaptchasonuc != "" && ankaracaptchasonuc.Length == 5)
                                            {
                                                //Trayyy1.ShowBalloonTip(100, "", ankaracaptchasonuc, ToolTipIcon.None);
                                                if (ankaracaptchasonuc.Contains("HATA"))
                                                {
                                                    goto Etiket_Ankara_LoginDeneme;
                                                }
                                                else
                                                {
                                                    //Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Captcha çözüldü. " + neovacaptchasonuc, ToolTipIcon.Error);
                                                    driveer.FindElement(By.XPath("//input[@id='Captcha' and @class='form-control']")).Clear();
                                                    driveer.FindElement(By.XPath("//input[@id='Captcha' and @class='form-control']")).SendKeys(ankaracaptchasonuc);
                                                    driveer.FindElement(By.XPath("//input[@id='Captcha' and @class='form-control']")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                                }
                                            }
                                            else
                                            {

                                                //Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Captcha Çözülemedi. Lütfen manuel giriş yapınız !!!", ToolTipIcon.Error);
                                                img.Save(@"C:\CMSigorta\cptchaerror\" + DateTime.Now.ToFileTime() + "__son.png", ImageFormat.Png);
                                                goto Etiket_Ankara_LoginDeneme;
                                            }

                                        }

                                        //sayfa yüklenmesini bekle
                                        wait.Until(driveeer => ((IJavaScriptExecutor)driveer).ExecuteScript("return document.readyState").Equals("complete"));

                                        //Thread.Sleep(1000);
                                        if (IsElementPresent(By.XPath("//div[@class='alert-danger alert']"), driveer))
                                        {
                                            if (IsElementPresent(By.XPath("//div[contains(text(),'Güvenlik')]"), driveer))
                                            {
                                                img.Save(@"C:\CMSigorta\cptchaerror\" + DateTime.Now.ToFileTime() + "__son.png", ImageFormat.Png);
                                                goto Etiket_Ankara_LoginDeneme;
                                            }
                                            else if (IsElementPresent(By.XPath("//li[contains(text(),'şifre')]"), driveer))
                                            {
                                                Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Girişte ŞİFRE HATASI mevcut. Şifre ayarı kontrol edilmeli.", ToolTipIcon.Error);
                                                goto Ankara_BeklenmeyenSonlanma;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        //hata ayıklama
                                        goto Etiket_Ankara_LoginDeneme;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama
                            }

                        Etiket_Ankara_2FAistedimi:
                            try
                            {
                                try
                                {
                                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='Code' and @class='form-control']")));
                                }
                                catch (Exception ex)
                                {
                                    if (IsElementPresent(By.XPath("//header[@class='main-header']"), driveer) && !IsElementPresent(By.XPath("//input[@id='Code' and @class='form-control']"), driveer))
                                    {
                                        //Ankara Sigorta Zaten Açık
                                        goto Ankara_BeklenmeyenSonlanma;
                                    }
                                    // 2FA girişi yapılacak input wait2 ile bulunamaz ise başa döneceğiz
                                    goto Etiket_Ankara_2FAistedimi;
                                }
                                try
                                {
                                    driveer.FindElement(By.XPath("//input[@id='Code' and @class='form-control']")).Click(); // sadece kutuyu tıklıyoruz bişey yapmıyoruz şuan, kutu bulundu 2FA çekilecek                                   
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla                                
                            }

                            GelenKullaniciAdi = KullaniciAdi;

                            xmlfile = gn.KareKod;
                            XmlDocument xdoca = new XmlDocument();
                            xdoca.Load(xmlfile);
                            XmlNodeList newXMLNodesa = xdoca.SelectNodes("/sirketler/sirket");
                            gelenlink = "";
                            adi = "";
                            key = "";
                            foreach (XmlNode newXMLNode in newXMLNodesa)
                            {
                                gelenlink = newXMLNode["link"].InnerText;
                                adi = newXMLNode["kullaniciadi"].InnerText;
                                key = newXMLNode["key"].InnerText;
                                if (UrlAdres.Contains(gelenlink) && GelenKullaniciAdi == adi)
                                {
                                    break;
                                }
                            }

                            //Thread.Sleep(5000);
                            smsDenemeAdet = 0;

                        Etiket_Ankara_2FAGiris:
                            try
                            {
                                smsDenemeAdet = smsDenemeAdet + 1;
                                Console.WriteLine("Ankara 2FA deneme sayısı:" + smsDenemeAdet);

                                if (smsDenemeAdet < 3)
                                {
                                    try
                                    {
                                        if (IsElementPresent(By.XPath("//input[@id='Code' and @class='form-control']"), driveer))
                                        {
                                            var bytes = OtpNet.Base32Encoding.ToBytes(key);
                                            var totp = new OtpNet.Totp(bytes);
                                            var remainingTime = totp.RemainingSeconds();
                                            int sayac = Convert.ToInt32(remainingTime);
                                            //if (sayac <= 3) await Task.Delay(4000);
                                            if (sayac <= 3) Task.Delay(4000);

                                            var result = totp.ComputeTotp();
                                            driveer.FindElement(By.XPath("//input[@id='Code' and @class='form-control']")).Clear();
                                            driveer.FindElement(By.XPath("//input[@id='Code' and @class='form-control']")).SendKeys(result);
                                            driveer.FindElement(By.XPath("//input[@id='Code' and @class='form-control']")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            Thread.Sleep(1000);
                                            //2FA denendikten sonra Hata varmı diye kontrol et
                                            if (IsElementPresent(By.XPath("//div[@class='error-message']"), driveer))
                                            {
                                                Trayyy1.ShowBalloonTip(100, "ANKARA SİGORTA", "Ankara Sigorta 2FA Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                                //goto Neova_BeklenmeyenSonlanma;
                                                goto Etiket_Ankara_2FAGiris;
                                            }
                                        }
                                        else
                                        {
                                            goto Etiket_Ankara_2FAGiris;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama logla
                                        goto Etiket_Ankara_2FAGiris;
                                    }
                                }
                                else
                                {
                                    Trayyy1.ShowBalloonTip(100, "ANKARA SİGORTA", "Ankara Sigorta 2FA sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                    goto Ankara_BeklenmeyenSonlanma;
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                goto Etiket_Ankara_2FAGiris;
                            }


                        Ankara_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch
                            {

                            }

                            break;
                        #endregion


                        //--------------------------------------------------------------------------------------  // Ankara Sigorta her girişte ID değiştirdiği için karışıkta olsa Rel xPath kullandık
                        //AXA TEK SİGORTA     GİRİŞ
                        #region AXA SİGORTA AXATEK
                        case string x when UrlAdres.Contains("axatek.axasigorta.com.tr"):
                        Etiket_AxaTek_LoginDeneme:
                            try
                            {
                                try
                                {
                                    //bütün sayfalarda olan div
                                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='app']")));

                                    //Zaten giriş yapılmış mı diye kontrol et
                                    Thread.Sleep(1000);
                                    if (IsElementPresent(By.XPath("//div[@class='page-container']"), driveer))
                                    {
                                        goto AxaTek_BeklenmeyenSonlanma;
                                    }
                                    else
                                    {
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(KullaniciID)));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise                                   
                                    Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Axa Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                    goto AxaTek_BeklenmeyenSonlanma;
                                }
                                if (IsElementPresent(By.XPath(KullaniciID), driveer))
                                {
                                    try
                                    {
                                        driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                        driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.XPath(SifreID)).Clear();
                                        driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);

                                        string axaanahtar = "BOSLUK";
                                        string resimkodu = "";
                                        int dongumax = 0;
                                        while (axaanahtar == "BOSLUK" && dongumax < 3)
                                        {
                                            dongumax++;
                                            Thread.Sleep(1000);
                                            resimkodu = driveer.FindElement(By.XPath("//img[@class='char-list-img']")).GetAttribute("src");
                                            //var img = GetElementScreenShot(driveer, resimkodu);
                                            resimkodu = resimkodu.Remove(0, 23);


                                            byte[] bytes = Convert.FromBase64String(resimkodu);
                                            Image img;
                                            using (MemoryStream ms = new MemoryStream(bytes))
                                            {
                                                img = Image.FromStream(ms);
                                            }
                                            try
                                            {
                                                Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Captcha çözme deneniyor, Lütfen bekleyiniz...", ToolTipIcon.None);

                                                Actions actionz = new Actions(driveer);
                                                actionz.MoveToElement(driveer.FindElement(By.XPath("//input[@id='input-36']"))).Click().Perform();

                                                axaanahtar = AxaCaptchaCoz(img);
                                            }
                                            catch (Exception eaxa)
                                            {
                                                //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                string folderName = @"C:\CMSigorta\cptchaerror\";
                                                // If directory does not exist, create it
                                                if (!Directory.Exists(folderName))
                                                {
                                                    Directory.CreateDirectory(folderName);
                                                }

                                                img.Save(folderName + DateTime.Now.ToFileTime() + "__axatek__1.png", ImageFormat.Png);
                                                //captchresim.Save(folderName + DateTime.Now.ToFileTime() + "_" + axaanahtar + "_2.png", ImageFormat.Png);
                                                //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                ///
                                                //img.Save(Path.GetTempPath() + DateTime.Now.ToFileTime() + "_img.png", ImageFormat.Png);
                                            }

                                            if (axaanahtar != "BOSLUK")
                                            {
                                                //Trayyy1.ShowBalloonTip(1000, "Axa Anahtar Çözüldü", axaanahtar, ToolTipIcon.Info);
                                                driveer.FindElement(By.XPath("//input[@id='input-36']")).SendKeys(axaanahtar);
                                                driveer.FindElement(By.XPath(LoginID)).Click();
                                                Thread.Sleep(500);
                                                if (IsElementPresent(By.XPath("//div[@class='alert-text' and contains(text(),'Captcha')]"), driveer))
                                                {
                                                    //captcha hatası
                                                    if (dongumax == 3)
                                                    {
                                                        Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Axa Sigorta'da Captcha çözülemedi !!!", ToolTipIcon.Warning);
                                                    }
                                                }
                                                if (IsElementPresent(By.XPath("//div[@class='alert-text' and contains(text(),'şifre')]"), driveer))
                                                {
                                                    Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Axa Sigorta'da şifre hatası mevcut. Lütfen şirket ayarlarını kontrol ediniz !!!", ToolTipIcon.Warning);
                                                    goto AxaTek_BeklenmeyenSonlanma;
                                                }
                                                goto Etiket_AxaTek_SMSistedimi;
                                            }
                                            else
                                            {
                                                resimkodu = "";
                                                driveer.FindElement(By.XPath("//button[@type='button' and contains(@class,'refresh')]")).Click();
                                                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//img[@class='char-list-img']")));
                                            }
                                        }
                                        Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Lütfen 1 dakika içinde manuel olarak Captcha girişi yapınız !!!", ToolTipIcon.Info);
                                        Actions action = new Actions(driveer);
                                        action.MoveToElement(driveer.FindElement(By.XPath("//input[@id='input-36']"))).Click().Perform();
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama
                                        Console.WriteLine(ex.ToString());
                                        goto Etiket_AxaTek_LoginDeneme;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama
                            }

                        Etiket_AxaTek_SMSistedimi:
                            try
                            {
                                wait3.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='app']")));
                                try
                                {
                                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")));
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                                try
                                {
                                    driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).Click(); // sadece kutuyu tıklıyoruz bişey yapmıyoruz şuan, kutu bulundu 2FA çekilecek                                   
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Axa Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                goto AxaTek_BeklenmeyenSonlanma;
                            }

                            GelenKullaniciAdi = KullaniciAdi;

                            //Thread.Sleep(5000);
                            smsDenemeAdet = 0;

                        Etiket_AxaTek_SMSGiris:
                            try
                            {
                                smsDenemeAdet++;
                                Console.WriteLine("AxaTek SMS deneme sayısı:" + smsDenemeAdet);

                                if (smsDenemeAdet < 10)
                                {
                                    try
                                    {
                                        if (IsElementPresent(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']"), driveer))
                                        {
                                            string sonuc = "";
                                            string sonuc2 = "";

                                            Thread.Sleep(3000);

                                            //DateTime myDatee = DateTime.Now;
                                            //DateTime d1 = myDatee.AddSeconds(-40);
                                            //string myZaman1r = d1.ToString();
                                            //myZaman1r = myZaman1r.Replace(".", "-");
                                            //myZaman1r = myZaman1r.Replace(" 0", " ");
                                            //DateTime d2 = myDatee.AddSeconds(3);
                                            //string myZaman2r = d2.ToString();
                                            //myZaman2r = myZaman2r.Replace(".", "-");
                                            //myZaman2r = myZaman2r.Replace(" 0", " ");


                                            //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi like 'AXA' and TelNo='" + smsnumberno + "' and Durum='0' and Tarih BETWEEN '" + myZaman1r + "' AND '" + myZaman2r + "' order by id desc limit 1");
                                            sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi like '%AXA%' and TelNo='" + smsnumberno + "' and Durum='0' order by id desc limit 1");

                                            if (sonuc != "")
                                            {
                                                //driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).Clear();
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(sonuc);
                                                driveer.FindElement(By.XPath("//input[@placeholder='Telefonunuza gönderilen şifreyi giriniz']")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            }
                                            else
                                            {
                                                goto Etiket_AxaTek_SMSGiris;
                                            }

                                            Thread.Sleep(1000);
                                            //SMS denendikten sonra Hata varmı diye kontrol et
                                            if (IsElementPresent(By.XPath("//div[@class='alert-text' and contains(text(),'Şifre')]"), driveer))
                                            {
                                                //if (smsDenemeAdet == 9)
                                                //{
                                                //    Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Axa Sigorta SMS Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                                //}                                                   

                                                List<string> TabloAdlari = new List<string>();
                                                TabloAdlari.Add("Durum");
                                                ArrayList veriler = new ArrayList();
                                                veriler.Add("1");
                                                sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);

                                                goto Etiket_AxaTek_SMSGiris;
                                            }
                                            else
                                            {
                                                wait3.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='app']")));
                                                if (IsElementPresent(By.XPath("//div[@class='page-container']"), driveer))
                                                {
                                                    List<string> TabloAdlari = new List<string>();
                                                    TabloAdlari.Add("Durum");
                                                    ArrayList veriler = new ArrayList();
                                                    veriler.Add("1");
                                                    sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);

                                                    goto AxaTek_BeklenmeyenSonlanma;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            goto Etiket_AxaTek_SMSGiris;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama logla
                                        goto Etiket_AxaTek_SMSGiris;
                                    }
                                }
                                else
                                {
                                    Trayyy1.ShowBalloonTip(100, "AXA SİGORTA", "Axa Sigorta SMS sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                    goto AxaTek_BeklenmeyenSonlanma;
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                goto Etiket_Ankara_2FAGiris;
                            }


                        AxaTek_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch
                            {

                            }

                            break;
                        #endregion


                        //--------------------------------------------------------------------------------------  // Ankara Sigorta her girişte ID değiştirdiği için karışıkta olsa Rel xPath kullandık
                        //TÜRKİYE SİGORTA     GİRİŞ
                        #region TÜRKİYE SİGORTA
                        case string x when UrlAdres.Contains("pusula.turkiyesigorta.com.tr"):
                        Etiket_Turkiye_LoginDeneme:
                            try
                            {
                                try
                                {
                                    Thread.Sleep(2000);

                                    //bütün sayfalarda olan script elementi sayfanın gelip gelmediğini kontrol edebiliyoruz
                                    //wait2.Until(ExpectedConditions.ElementExists(By.XPath("//script[@id='f5_cspm']")));
                                    wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='javax.faces.ViewState']")));
                                    //Console.Beep(350, 150);

                                    //Zaten giriş yapılmış mı diye kontrol et
                                    // Eğer zaten oturum açıksa Anasayfa linki varmı diye bakıyor
                                    if (IsElementPresent(By.LinkText("Ana Sayfa"), driveer))
                                    {
                                        driveer.FindElement(By.LinkText("Ana Sayfa")).Click();
                                        wait.Until(ExpectedConditions.UrlContains("base"));
                                        goto Etiket_Turkiye_Giris_Basarili;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Kullanıcı adı girişi yapılacak input 30 saniye içinde gelmez ise                                   
                                    Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Türkiye Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                    goto Turkiye_BeklenmeyenSonlanma;
                                }
                                if (IsElementPresent(By.XPath(KullaniciID), driveer))
                                {
                                    try
                                    {
                                        //driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                        //driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                        //driveer.FindElement(By.XPath(SifreID)).Clear();
                                        //driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);

                                        HtmlInputTemizleVeDoldur(driveer, KullaniciID, KullaniciAdi);
                                        HtmlInputTemizleVeDoldur(driveer, SifreID, Sifre);


                                        //Eğer Türkiye ekrana girerken captcha sormazsa click      
                                        if (!IsElementPresent(By.XPath("//img[contains(@src,'captcha')]"), driveer))
                                        {
                                            driveer.FindElement(By.XPath(LoginID)).Click();
                                            goto Etiket_Turkiye_SUSPUS_istedimi;
                                        }
                                        else
                                        {
                                            //eskiden kullanıcı girişi için kutuya geçiş için kullanıyorduk artık biz çözeceğiz
                                            //driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                                            Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Captcha çözme deneniyor, Lütfen bekleyiniz...", ToolTipIcon.None);

                                            Actions actionz = new Actions(driveer);
                                            actionz.MoveToElement(driveer.FindElement(By.XPath("//input[contains(@id,'captcha')]"))).Click().Perform();
                                        }


                                        string turkiyeanahtar = "BOSLUK";
                                        int dongumax = 0;
                                        while (turkiyeanahtar == "BOSLUK" && dongumax < 5)
                                        {
                                            dongumax++;
                                            Thread.Sleep(1000);
                                            IWebElement turkiyecaptcharesim = driveer.FindElement(By.XPath("//img[contains(@src,'captcha')]"));
                                            Image img = GetElementScreenShot(driveer, turkiyecaptcharesim);
                                            //string base64bu;
                                            //base64bu = cptch.ResimdenBase64e(img);
                                            Captcha cptch = new Captcha();
                                            Image captchresim;


                                            captchresim = Contrastit((Bitmap)img, 40);
                                            captchresim = Contrastit((Bitmap)captchresim, 40);
                                            captchresim = AnkaraRenkDegistir((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = DikeyYatayTekTemizle((Bitmap)captchresim);
                                            captchresim = SiyahDikeyCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizle((Bitmap)captchresim);
                                            captchresim = Contrastit((Bitmap)captchresim, 40);
                                            captchresim = RemoveNoise((Bitmap)captchresim);
                                            captchresim = SiyahYatayCizgiTemizleWithMargin((Bitmap)captchresim);
                                            captchresim = FixedSizeTo500((Bitmap)captchresim);
                                            captchresim = Contrastit((Bitmap)captchresim, 40);
                                            captchresim = RemoveNoise((Bitmap)captchresim);


                                            //string base64laniste = ResimdenBase64e(captchresim);


                                            try
                                            {
                                                turkiyeanahtar = OcrSpaceileCoz(captchresim, "5");
                                                turkiyeanahtar = turkiyeanahtar.Replace(" ", "");
                                                turkiyeanahtar = turkiyeanahtar.ToLower();
                                                turkiyeanahtar = RemoveSpecialCharacters(turkiyeanahtar);
                                            }
                                            catch
                                            {
                                                // bir yere goto olacak ama bakalım

                                            }

                                            if (turkiyeanahtar != "bosluk" && turkiyeanahtar.Length == 6)
                                            {
                                                //Trayyy1.ShowBalloonTip(1000, "Türkiye Anahtar Çözüldü", turkiyeanahtar, ToolTipIcon.Info);
                                                //driveer.FindElement(By.XPath("//input[contains(@id,'captcha')]")).SendKeys(turkiyeanahtar);
                                                HtmlInputTemizleVeDoldur(driveer, "//input[contains(@id,'captcha')]", turkiyeanahtar);
                                                driveer.FindElement(By.XPath(LoginID)).Click();
                                                Thread.Sleep(500);

                                                //bütün sayfalarda olan script elementi sayfanın gelip gelmediğini kontrol edebiliyoruz
                                                wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='javax.faces.ViewState']")));

                                                //eğer captcha yanlışsa
                                                if (IsElementPresent(By.XPath("//span[contains(text(),'Güvenlik')]"), driveer))
                                                {
                                                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                    string folderName = @"C:\CMSigorta\cptchaerror\";
                                                    // If directory does not exist, create it
                                                    if (!Directory.Exists(folderName))
                                                    {
                                                        Directory.CreateDirectory(folderName);
                                                    }

                                                    img.Save(folderName + DateTime.Now.ToFileTime() + "_" + turkiyeanahtar + "_1.png", ImageFormat.Png);
                                                    captchresim.Save(folderName + DateTime.Now.ToFileTime() + "_" + turkiyeanahtar + "_2.png", ImageFormat.Png);
                                                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////


                                                    //captcha hatası
                                                    if (dongumax == 5)
                                                    {
                                                        Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Türkiye Sigorta'da Captcha çözülemedi !!!", ToolTipIcon.Warning);
                                                    }
                                                    else
                                                    {
                                                        goto Etiket_Turkiye_LoginDeneme;
                                                    }
                                                }

                                                //eğer şifre yanlışsa
                                                if (IsElementPresent(By.XPath("//span[contains(text(),'şifre')]"), driveer))
                                                {
                                                    Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Türkiye Sigorta'da şifre hatası mevcut. Lütfen şirket ayarlarını kontrol ediniz !!!", ToolTipIcon.Warning);
                                                    goto Turkiye_BeklenmeyenSonlanma;
                                                }

                                                goto Etiket_Turkiye_SUSPUS_istedimi;
                                            }
                                            else
                                            {
                                                driveer.FindElement(By.XPath("//i[@class='icon-refresh']")).Click();
                                                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//i[@class='icon-refresh']")));

                                                string folderName = @"C:\CMSigorta\cptchaerror\";
                                                // If directory does not exist, create it
                                                if (!Directory.Exists(folderName))
                                                {
                                                    Directory.CreateDirectory(folderName);
                                                }

                                                img.Save(folderName + DateTime.Now.ToFileTime() + "_" + turkiyeanahtar + "_1.png", ImageFormat.Png);
                                                captchresim.Save(folderName + DateTime.Now.ToFileTime() + "_" + turkiyeanahtar + "_2.png", ImageFormat.Png);

                                                goto Etiket_Turkiye_LoginDeneme;
                                            }
                                        }
                                        Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Lütfen 1 dakika içinde manuel olarak Captcha girişi yapınız !!!", ToolTipIcon.None);
                                        Actions action = new Actions(driveer);
                                        action.MoveToElement(driveer.FindElement(By.XPath("//input[contains(@id,'captcha')]"))).Click().Perform();
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama
                                        Console.WriteLine(ex.ToString());
                                        goto Etiket_Turkiye_LoginDeneme;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama
                            }

                        Etiket_Turkiye_SUSPUS_istedimi:
                            try
                            {
                                //bütün sayfalarda olan script elementi sayfanın gelip gelmediğini kontrol edebiliyoruz
                                wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='javax.faces.ViewState']")));
                                try
                                {
                                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@id,'MFAValue')]")));
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                                try
                                {
                                    driveer.FindElement(By.XPath("//input[contains(@id,'MFAValue')]")).Click(); // sadece kutuyu tıklıyoruz bişey yapmıyoruz şuan, kutu bulundu SUSPUS çekilecek                                   
                                }
                                catch (Exception ex)
                                {
                                    //hata ayıklama logla
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Türkiye Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                goto Turkiye_BeklenmeyenSonlanma;
                            }

                            GelenKullaniciAdi = KullaniciAdi;

                            //Thread.Sleep(5000);
                            smsDenemeAdet = 0;

                        Etiket_Turkiye_SUSPUS_Giris:
                            try
                            {
                                smsDenemeAdet++;
                                Console.WriteLine("Türkiye SUSPUS deneme sayısı:" + smsDenemeAdet);

                                if (smsDenemeAdet < 2)
                                {
                                    try
                                    {
                                        if (IsElementPresent(By.XPath("//input[contains(@id,'MFAValue')]"), driveer))
                                        {
                                            string sonuc = "";
                                            string sonuc2 = "";

                                            Thread.Sleep(3000);

                                            DateTime myDatee = DateTime.Now;
                                            DateTime d1 = myDatee.AddSeconds(-4);
                                            string myZaman1r = d1.ToString();
                                            myZaman1r = myZaman1r.Replace(".", "-");
                                            myZaman1r = myZaman1r.Replace(" 0", " ");
                                            DateTime d2 = myDatee.AddSeconds(3);
                                            string myZaman2r = d2.ToString();
                                            myZaman2r = myZaman2r.Replace(".", "-");
                                            myZaman2r = myZaman2r.Replace(" 0", " ");


                                            sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi like 'TURKIYESGRT' and TelNo='" + smsnumberno + "' order by id desc limit 1");

                                            if (sonuc != "")
                                            {
                                                driveer.FindElement(By.XPath("//input[contains(@id,'MFAValue')]")).Clear();
                                                driveer.FindElement(By.XPath("//input[contains(@id,'MFAValue')]")).SendKeys(sonuc);
                                                driveer.FindElement(By.XPath("//input[contains(@id,'MFAValue')]")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                            }
                                            else
                                            {
                                                goto Etiket_Turkiye_SUSPUS_Giris;
                                            }

                                            Thread.Sleep(1000);
                                            //bütün sayfalarda olan script elementi sayfanın gelip gelmediğini kontrol edebiliyoruz
                                            wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='javax.faces.ViewState']")));

                                            //SUSPUS denendikten sonra Hata varmı diye kontrol et
                                            if (IsElementPresent(By.XPath("//span[contains(text(),'şifre')]"), driveer))
                                            {
                                                Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Türkiye Sigorta SUSPUS Girişte UYARI mevcut. Uyarı mesajı kontrol edilmeli.", ToolTipIcon.Error);
                                                goto Etiket_Turkiye_SUSPUS_Giris;
                                            }
                                            else
                                            {
                                                //wait3.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='app']")));
                                                if (IsElementPresent(By.XPath("//li[contains(text(),'Hoşgeldiniz')]"), driveer))
                                                {
                                                    List<string> TabloAdlari = new List<string>();
                                                    TabloAdlari.Add("Durum");
                                                    ArrayList veriler = new ArrayList();
                                                    veriler.Add("1");
                                                    sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);

                                                    goto Etiket_Turkiye_Giris_Basarili;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            goto Etiket_Turkiye_SUSPUS_Giris;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama logla
                                        goto Etiket_Turkiye_SUSPUS_Giris;
                                    }
                                }
                                else
                                {
                                    Trayyy1.ShowBalloonTip(100, "TÜRKİYE SİGORTA", "Türkiye Sigorta SUSPUS sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                    goto Turkiye_BeklenmeyenSonlanma;
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                goto Etiket_Turkiye_SUSPUS_Giris;
                            }
                        Etiket_Turkiye_Giris_Basarili:
                            try
                            {
                                //Burada şirket açılınca acente tarafında ayarlanacak giriş mesajları uyarılar felan ne olursa onlar çalıştırılacak.
                                Trayyy1.ShowBalloonTip(50, "", "Türkiye Sigorta hazır...", ToolTipIcon.Info);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }

                        Turkiye_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch
                            {

                            }

                            break;
                        #endregion



                        //--------------------------------------------------------------------------------------  // Ankara Sigorta her girişte ID değiştirdiği için karışıkta olsa Rel xPath kullandık
                        //ANADOLU SİGORTA     GİRİŞ
                        #region ANADOLU SİGORTA
                        case string x when UrlAdres.Contains("giris.anadolusigorta.com.tr"):
                        Etiket_Anadolu_LoginDeneme:
                            try
                            {
                                //sayfanın tamamen yüklenmesini bekle
                                wait.Until(driveeer => ((IJavaScriptExecutor)driveer).ExecuteScript("return document.readyState").Equals("complete"));

                                try
                                {
                                    //Zaten giriş yapılmış mı diye kontrol et
                                    if (IsElementPresent(By.XPath("//div[contains(@class,'overlay')]"), driveer))
                                    {
                                        goto Etiket_Anadolu_Giris_Basarili;
                                    }

                                    if (IsElementPresent(By.XPath(SifreID), driveer))
                                    {
                                        //bu yöntem ile input doldurma yapılırsa Captcha gitmiyor o yüzden send keys kullanıyoruz
                                        //HtmlInputTemizleVeDoldur(driveer, KullaniciID, KullaniciAdi);
                                        //HtmlInputTemizleVeDoldur(driveer, SifreID, Sifre);

                                        driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                        driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.XPath(SifreID)).Clear();
                                        driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);

                                        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//img[@id='captchaImage2']")));

                                        driveer.FindElement(By.XPath(LoginID)).Click();

                                        //Thread.Sleep(100);
                                        //string modalgeldimi1 = driveer.FindElement(By.XPath("//div[@id='modalDialogComponent']")).GetAttribute("style");
                                        //if (modalgeldimi1.Contains("block"))
                                        //{
                                        //    driveer.FindElement(By.XPath("//a[@data-dismiss='modal' and @aria-label='Kapat']")).Click();

                                        //    Trayyy1.ShowBalloonTip(100, "ANADOLU SİGORTA", "Anadolu Sigorta kullanıcısında sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                        //    goto Anadolu_BeklenmeyenSonlanma; ;
                                        //}

                                        try
                                        {
                                            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='sms']")));
                                        }
                                        catch (Exception)
                                        {
                                            Trayyy1.ShowBalloonTip(100, "ANADOLU SİGORTA", "Anadolu Sigorta kullanıcısında sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                            goto Anadolu_BeklenmeyenSonlanma; ;
                                            //throw;
                                        }

                                        driveer.FindElement(By.XPath("//input[@id='sms']")).Click();
                                        driveer.FindElement(By.XPath("//button[@id='selectOtpType']")).Click();

                                        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@name='smsValidationCode']")));

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Etiket Anadolu Login Deneme Try Hatası: " + ex.ToString());
                                    Thread.Sleep(1000);
                                    goto Etiket_Anadolu_LoginDeneme;
                                }

                                //wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@id='password']")));

                                //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//input[@name='javax.faces.ViewState']")));

                            }
                            catch (Exception ex)
                            {
                                Trayyy1.ShowBalloonTip(100, "ANADOLU SİGORTA", "Anadolu Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                goto Anadolu_BeklenmeyenSonlanma;
                            }


                            smsDenemeAdet = 0;

                        Etiket_Anadolu_SMSGiris:
                            try
                            {
                                Thread.Sleep(2000);
                                smsDenemeAdet++;
                                Console.WriteLine("Anadolu SMS deneme sayısı:" + smsDenemeAdet);

                                if (smsDenemeAdet < 10)
                                {
                                    try
                                    {
                                        if (IsElementPresent(By.XPath("//input[@name='smsValidationCode']"), driveer))
                                        {
                                            string sonuc = "";
                                            string sonuc2 = "";

                                            //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi like 'AXA' and TelNo='" + smsnumberno + "' and Durum='0' and Tarih BETWEEN '" + myZaman1r + "' AND '" + myZaman2r + "' order by id desc limit 1");
                                            sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='ANADOLUSIG' and TelNo='" + smsnumberno + "' and Durum='0' order by id desc limit 1");

                                            if (sonuc != "")
                                            {
                                                Console.WriteLine(sonuc);

                                                // HtmlInputTemizleVeDoldur fonksiyonu burada çalışmıyor.
                                                //HtmlInputTemizleVeDoldur(driveer, "//input[@name='smsValidationCode']", sonuc);
                                                driveer.FindElement(By.XPath("//input[@name='smsValidationCode']")).Clear();
                                                driveer.FindElement(By.XPath("//input[@name='smsValidationCode']")).SendKeys(sonuc);
                                                driveer.FindElement(By.XPath("//input[@name='smsValidationCode']")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                                //Alternatif yöntem
                                                //driveer.FindElement(By.XPath("//button[@id='verifySmsValidationCodeButton']")).Click();
                                            }
                                            else
                                            {
                                                goto Etiket_Anadolu_SMSGiris;
                                            }

                                            Thread.Sleep(1000);
                                            //SMS denendikten sonra Hata varmı diye kontrol et
                                            string modalgeldimi = "";
                                            if (IsElementPresent(By.XPath("//div[@id='modalDialogComponent']"), driveer))
                                            {
                                                modalgeldimi = driveer.FindElement(By.XPath("//div[@id='modalDialogComponent']")).GetAttribute("style");
                                            }

                                            if (modalgeldimi.Contains("block") && IsElementPresent(By.XPath("//div[@class='container']"), driveer))
                                            {
                                                driveer.FindElement(By.XPath("//a[@data-dismiss='modal' and @aria-label='Kapat']")).Click();

                                                goto Etiket_Anadolu_SMSGiris;
                                            }
                                            else
                                            {
                                                wait3.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'overlay')]")));
                                                if (IsElementPresent(By.XPath("//div[contains(@class,'overlay')]"), driveer))
                                                {
                                                    List<string> TabloAdlari = new List<string>();
                                                    TabloAdlari.Add("Durum");
                                                    ArrayList veriler = new ArrayList();
                                                    veriler.Add("1");
                                                    sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);

                                                    goto Etiket_Anadolu_Giris_Basarili;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            goto Etiket_Anadolu_SMSGiris;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //hata ayıklama logla
                                        goto Etiket_Anadolu_SMSGiris;
                                    }
                                }
                                else
                                {
                                    Trayyy1.ShowBalloonTip(50, "ANADOLU SİGORTA", "Anadolu Sigorta SMS sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                    goto Anadolu_BeklenmeyenSonlanma;
                                }
                            }
                            catch (Exception ex)
                            {
                                //hata ayıklama logla
                                //goto Etiket_Ankara_2FAGiris;
                            }
                        Etiket_Anadolu_Giris_Basarili:
                            try
                            {
                                //Burada şirket açılınca acente tarafında ayarlanacak giriş mesajları uyarılar felan ne olursa onlar çalıştırılacak.
                                Trayyy1.ShowBalloonTip(100, "", "Anadolu Sigorta hazır...", ToolTipIcon.Info);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }


                        Anadolu_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }

                            break;

                        #endregion



                        //--------------------------------------------------------------------------------------  // Ankara Sigorta her girişte ID değiştirdiği için karışıkta olsa Rel xPath kullandık
                        //GULF SİGORTA YENİ SFS     GİRİŞ
                        #region GULF SİGORTA SFS 2
                        case string x when UrlAdres.Contains("insureeonline.gulfsigorta.com.tr"):
                        Etiket_GulfSFS2_LoginDeneme:
                            try
                            {
                                //sayfanın tamamen yüklenmesini bekle
                                wait.Until(driveeer => ((IJavaScriptExecutor)driveer).ExecuteScript("return document.readyState").Equals("complete"));

                                try
                                {
                                    //Zaten giriş yapılmış mı diye kontrol et
                                    if (IsElementPresent(By.XPath("//div[contains(@class,'base__sidebar-nav')]"), driveer))
                                    {
                                        goto Etiket_GulfSFS2_Giris_Basarili;
                                    }

                                    if (IsElementPresent(By.XPath(SifreID), driveer))
                                    {
                                        //bu yöntem ile input doldurma yapılırsa Captcha gitmiyor o yüzden send keys kullanıyoruz
                                        HtmlInputTemizleVeDoldur(driveer, KullaniciID, KullaniciAdi);
                                        HtmlInputTemizleVeDoldur(driveer, SifreID, Sifre);

                                        driveer.FindElement(By.XPath(LoginID)).Click();

                                        //driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                        //driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                        //driveer.FindElement(By.XPath(SifreID)).Clear();
                                        //driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);                                      

                                        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='txtCaptcha']")));

                                    Etiket_GulfSFS2_Captcha_Deneme:
                                        Image img = null;
                                        if (IsElementPresent(By.XPath("//input[@id='txtCaptcha']"), driveer))
                                        {
                                            Trayyy1.ShowBalloonTip(100, "GULF SİGORTA YENİ SFS", "Captcha çözme deneniyor, Lütfen Bekleyiniz...", ToolTipIcon.None);

                                            Actions actionz = new Actions(driveer);
                                            actionz.MoveToElement(driveer.FindElement(By.XPath("//input[@id='txtCaptcha']"))).Click().Perform();


                                            String gulfcaptchasonuc = "";

                                            IWebElement gulfcaptcharesim = driveer.FindElement(By.XPath("//img[@id='imgCaptcha']"));
                                            img = GetElementScreenShot(driveer, gulfcaptcharesim);
                                            //string base64bu;
                                            //base64bu = cptch.ResimdenBase64e(img);
                                            Captcha cptch = new Captcha();
                                            //Image captchresim;

                                            img = RemoveNoise((Bitmap)img);
                                            img = Contrastit((Bitmap)img, 40);
                                            img = Contrastit((Bitmap)img, 40);
                                            img = AyarliRenkDegistir((Bitmap)img);
                                            img = SiyahYatayCizgiTemizle((Bitmap)img);
                                            img = DikeyYatayTekTemizle((Bitmap)img);
                                            img = SiyahYatayCizgiTemizle((Bitmap)img);
                                            img = DikeyYatayTekTemizle((Bitmap)img);
                                            img = SiyahYatayCizgiTemizle((Bitmap)img);
                                            img = DikeyYatayTekTemizle((Bitmap)img);
                                            img = SiyahDikeyCizgiTemizle((Bitmap)img);
                                            img = ProcessImageOhaa((Bitmap)img);


                                            //captchresim = FixedSizeTo500(captchresim);

                                            //ankaracaptchasonuc = cptch.NeovaAritmetik(cptch.NeovaOku(captchresim));
                                            //ankaracaptchasonuc = cptch.AnkaraOku((Bitmap)captchresim);


                                            try
                                            {
                                                gulfcaptchasonuc = OcrSpaceileCoz(img, "5");
                                                gulfcaptchasonuc = gulfcaptchasonuc.Replace(" ", "");
                                                gulfcaptchasonuc = gulfcaptchasonuc.Replace(".", "");
                                                gulfcaptchasonuc = gulfcaptchasonuc.ToUpper();
                                                gulfcaptchasonuc = RemoveSpecialCharacters(gulfcaptchasonuc);
                                            }
                                            catch
                                            {
                                                // bir yere goto olacak ama bakalım

                                            }

                                            //using (var ocrresim = OcrApi.Create())
                                            //{
                                            //    ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                                            //    ocrresim.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                                            //    string karakterx = ocrresim.GetTextFromImage((Bitmap)img);
                                            //    ankaracaptchasonuc = karakterx;
                                            //}

                                            if (gulfcaptchasonuc != "" && gulfcaptchasonuc.Length == 7)
                                            {
                                                //Trayyy1.ShowBalloonTip(100, "", ankaracaptchasonuc, ToolTipIcon.None);
                                                if (gulfcaptchasonuc.Contains("HATA"))
                                                {
                                                    goto Etiket_GulfSFS2_LoginDeneme;
                                                }
                                                else
                                                {
                                                    HtmlInputTemizleVeDoldur(driveer, "//input[@id='txtCaptcha']", gulfcaptchasonuc);
                                                    driveer.FindElement(By.XPath("//button[@id='btnCaptchaLogin']")).Click();
                                                }
                                            }
                                            else
                                            {
                                                //Trayyy1.ShowBalloonTip(100, "GULF SİGORTA SFS", "Gulf Sigorta Captcha Çözülemedi. Lütfen manuel giriş yapınız !!!", ToolTipIcon.Error);
                                                img.Save(@"C:\CMSigorta\cptchaerror\" + DateTime.Now.ToFileTime() + "__" + gulfcaptchasonuc + "__.png", ImageFormat.Png);
                                                goto Etiket_GulfSFS2_LoginDeneme;
                                            }
                                        }

                                        Thread.Sleep(1000);
                                        string hatamesaj = "";
                                        hatamesaj = driveer.FindElement(By.XPath("//div[@class='userLoginError']")).Text;

                                        if (hatamesaj != "")
                                        {
                                            //MessageBox.Show("---" + hatamesaj + "---");
                                            if (hatamesaj.Contains("şifre"))
                                            {
                                                Trayyy1.ShowBalloonTip(100, "GULF SİGORTA SFS", "Kullanıcı şifre hatası var. Gulf Sigorta SFS Planor ayarları kontrol edilmeli!!!", ToolTipIcon.Error);
                                                goto GulfSFS2_BeklenmeyenSonlanma;
                                            }
                                            if (hatamesaj.Contains("güvenlik"))
                                            {
                                                goto Etiket_GulfSFS2_Captcha_Deneme;
                                            }
                                        }

                                        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='txtOtpCode']")));

                                        smsDenemeAdet = 0;

                                    Etiket_GulfSFS2_SMS_Giris:
                                        try
                                        {
                                            Thread.Sleep(4000);

                                            smsDenemeAdet++;

                                            if (smsDenemeAdet < 3)
                                            {
                                                Console.WriteLine("GULF SFS SMS deneme sayısı:" + smsDenemeAdet);
                                                try
                                                {
                                                    if (IsElementPresent(By.XPath("//input[@id='txtOtpCode']"), driveer))
                                                    {
                                                        string sonuc = "";
                                                        string sonuc2 = "";

                                                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi like 'AXA' and TelNo='" + smsnumberno + "' and Durum='0' and Tarih BETWEEN '" + myZaman1r + "' AND '" + myZaman2r + "' order by id desc limit 1");
                                                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='GULFSIGORTA' and TelNo='" + smsnumberno + "' and Durum='0' order by id desc limit 1");

                                                        if (sonuc != "")
                                                        {
                                                            Console.WriteLine(sonuc);

                                                            HtmlInputTemizleVeDoldur(driveer, "//input[@id='txtOtpCode']", sonuc);
                                                            driveer.FindElement(By.XPath("//button[@id='btnSendOtpCode']")).Click();
                                                        }
                                                        else
                                                        {
                                                            Thread.Sleep(2000);
                                                            goto Etiket_GulfSFS2_SMS_Giris;
                                                        }

                                                        Thread.Sleep(1000);

                                                        if (IsElementPresent(By.XPath("//div[@class='userLoginError']"), driveer))
                                                        {
                                                            hatamesaj = driveer.FindElement(By.XPath("//div[@class='userLoginError']")).Text;
                                                            goto Etiket_GulfSFS2_SMS_Giris;
                                                        }
                                                        else
                                                        {
                                                            wait3.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'base__sidebar-nav')]")));
                                                            if (IsElementPresent(By.XPath("//div[contains(@class,'base__sidebar-nav')]"), driveer))
                                                            {
                                                                List<string> TabloAdlari = new List<string>();
                                                                TabloAdlari.Add("Durum");
                                                                ArrayList veriler = new ArrayList();
                                                                veriler.Add("1");
                                                                sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);

                                                                goto Etiket_GulfSFS2_Giris_Basarili;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        goto Etiket_GulfSFS2_SMS_Giris;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    //hata ayıklama logla
                                                    goto Etiket_GulfSFS2_SMS_Giris;
                                                }
                                            }
                                            else
                                            {
                                                Trayyy1.ShowBalloonTip(50, "GULF SİGORTA SFS", "Gulf Sigorta SMS sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                                goto GulfSFS2_BeklenmeyenSonlanma;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //hata ayıklama logla
                                            //goto Etiket_Ankara_2FAGiris;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Etiket Gulf SFS Login Deneme Try Hatası: " + ex.ToString());
                                    Thread.Sleep(1000);
                                    goto Etiket_GulfSFS2_LoginDeneme;
                                }
                            }
                            catch (Exception ex)
                            {
                                Trayyy1.ShowBalloonTip(100, "GULF SİGORTA SFS", "Gulf Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                goto GulfSFS2_BeklenmeyenSonlanma;
                            }

                        Etiket_GulfSFS2_Giris_Basarili:
                            try
                            {
                                //Burada şirket açılınca acente tarafında ayarlanacak giriş mesajları uyarılar felan ne olursa onlar çalıştırılacak.
                                Trayyy1.ShowBalloonTip(100, "", "GULF Sigorta SFS hazır...", ToolTipIcon.Info);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }


                        GulfSFS2_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }

                            break;

                        #endregion



                        //--------------------------------------------------------------------------------------  // Ankara Sigorta her girişte ID değiştirdiği için karışıkta olsa Rel xPath kullandık
                        //GULF SİGORTA PORTAL     GİRİŞ
                        #region GULF SİGORTA PORTAL
                        case string x when UrlAdres.Contains("portal.gulfsigorta.com.tr"):
                            try
                            {
                                //sayfanın tamamen yüklenmesini bekle
                                wait.Until(driveeer => ((IJavaScriptExecutor)driveer).ExecuteScript("return document.readyState").Equals("complete"));
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }

                        Etiket_GulfPort_LoginDeneme:
                            try
                            {
                                //IWebElement gulfcaptcharesimx = driveer.FindElement(By.XPath("//canvas[@id='reCaptchaCanvas']"));
                                //Image imgx = GetElementScreenShot(driveer, gulfcaptcharesimx);
                                //imgx.Save(@"C:\CMSigorta\cptchaerror\" + DateTime.Now.ToFileTime() + "___.png", ImageFormat.Png);

                                //gulfcaptcharesimx.Click();
                                //Thread.Sleep(1000);

                                //imgx = GetElementScreenShot(driveer, gulfcaptcharesimx);
                                //imgx.Save(@"C:\CMSigorta\cptchaerror\" + DateTime.Now.ToFileTime() + "___.png", ImageFormat.Png);

                                //gulfcaptcharesimx.Click();
                                //Thread.Sleep(1000);

                                //imgx = GetElementScreenShot(driveer, gulfcaptcharesimx);
                                //imgx.Save(@"C:\CMSigorta\cptchaerror\" + DateTime.Now.ToFileTime() + "___.png", ImageFormat.Png);

                                try
                                {
                                    //Zaten giriş yapılmış mı diye kontrol et
                                    if (IsElementPresent(By.XPath("//div[contains(@class,'base__sidebar-nav')]"), driveer))
                                    {
                                        goto Etiket_GulfSFS2_Giris_Basarili;
                                    }

                                    if (IsElementPresent(By.XPath(SifreID), driveer))
                                    {
                                        //bu yöntem ile input doldurma yapılırsa Captcha gitmiyor o yüzden send keys kullanıyoruz
                                        HtmlInputTemizleVeDoldur(driveer, KullaniciID, KullaniciAdi);
                                        HtmlInputTemizleVeDoldur(driveer, SifreID, Sifre);



                                    //driveer.FindElement(By.XPath(KullaniciID)).Clear();
                                    //driveer.FindElement(By.XPath(KullaniciID)).SendKeys(KullaniciAdi);
                                    //driveer.FindElement(By.XPath(SifreID)).Clear();
                                    //driveer.FindElement(By.XPath(SifreID)).SendKeys(Sifre);


                                    Etiket_GulfSFS2_Captcha_Deneme:
                                        Image img = null;
                                        if (IsElementPresent(By.XPath("//input[@id='HashCode']"), driveer))
                                        {
                                            Trayyy1.ShowBalloonTip(100, "GULF SİGORTA PORTAL", "Captcha çözme deneniyor, Lütfen Bekleyiniz...", ToolTipIcon.None);

                                            Actions actionz = new Actions(driveer);
                                            actionz.MoveToElement(driveer.FindElement(By.XPath("//input[@id='HashCode']"))).Click().Perform();


                                            String gulfcaptchasonuc = "";

                                            IWebElement gulfcaptcharesim = driveer.FindElement(By.XPath("//canvas[@id='reCaptchaCanvas']"));
                                            img = GetElementScreenShot(driveer, gulfcaptcharesim);
                                            //string base64bu;
                                            //base64bu = cptch.ResimdenBase64e(img);
                                            Captcha cptch = new Captcha();
                                            //Image captchresim;

                                            img = FixedSizeTo500((Bitmap)img);
                                            img = RemoveNoise((Bitmap)img);
                                            img = Contrastit((Bitmap)img, 40);
                                            img = Contrastit((Bitmap)img, 40);
                                            img = Contrastit((Bitmap)img, 40);
                                            img = Contrastit((Bitmap)img, 40);
                                            img = Contrastit((Bitmap)img, 40);
                                            img = QuickRenkDegistir((Bitmap)img);
                                            img = AnkaraTemizle((Bitmap)img);
                                            img = CropWhiteSpace((Bitmap)img);
                                            img = AnkaraTemizle((Bitmap)img);
                                            img = AnkaraDikey2liTemizle((Bitmap)img);
                                            img = AnkaraDikey2liTemizle((Bitmap)img);
                                            img = SiyahYatayCizgiTemizle((Bitmap)img);
                                            img = SiyahYatayCizgiTemizle((Bitmap)img);
                                            img = DikeyYatayTekTemizle((Bitmap)img);
                                            img = DikeyYatayTekTemizle((Bitmap)img);
                                            img = SiyahDikeyCizgiTemizle((Bitmap)img);
                                            img = SiyahDikeyCizgiTemizle((Bitmap)img);
                                            img = SiyahYatayCizgiTemizleWithMargin((Bitmap)img);
                                            img = ProcessImageOhaa((Bitmap)img);


                                            //captchresim = FixedSizeTo500(captchresim);

                                            //ankaracaptchasonuc = cptch.NeovaAritmetik(cptch.NeovaOku(captchresim));
                                            //ankaracaptchasonuc = cptch.AnkaraOku((Bitmap)captchresim);


                                            try
                                            {
                                                gulfcaptchasonuc = OcrSpaceileCoz(img, "5");
                                                gulfcaptchasonuc = gulfcaptchasonuc.Replace(" ", "");
                                                gulfcaptchasonuc = gulfcaptchasonuc.Replace(".", "");
                                                gulfcaptchasonuc = gulfcaptchasonuc.Replace("_", "");
                                                //gulfcaptchasonuc = gulfcaptchasonuc.ToUpper();
                                                gulfcaptchasonuc = RemoveSpecialCharacters(gulfcaptchasonuc);

                                                if (gulfcaptchasonuc == "bosluk")
                                                {
                                                    OcrApi.PathToEngine = @"C://CMSigorta/tesseract.dll";
                                                    using (var ocrresim = OcrApi.Create())
                                                    {
                                                        ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                                                        ocrresim.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                                                        string karakterx = ocrresim.GetTextFromImage((Bitmap)img);

                                                        gulfcaptchasonuc = karakterx;
                                                        gulfcaptchasonuc = gulfcaptchasonuc.Replace(" ", "");
                                                        gulfcaptchasonuc = gulfcaptchasonuc.Replace(".", "");
                                                        gulfcaptchasonuc = gulfcaptchasonuc.Replace("_", "");
                                                        //gulfcaptchasonuc = gulfcaptchasonuc.ToUpper();
                                                        gulfcaptchasonuc = RemoveSpecialCharacters(gulfcaptchasonuc);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // bir yere goto olacak ama bakalım
                                            }

                                            //using (var ocrresim = OcrApi.Create())
                                            //{
                                            //    ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                                            //    ocrresim.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                                            //    string karakterx = ocrresim.GetTextFromImage((Bitmap)img);
                                            //    ankaracaptchasonuc = karakterx;
                                            //}

                                            if (gulfcaptchasonuc != "" && gulfcaptchasonuc.Length == 5)
                                            {
                                                //Trayyy1.ShowBalloonTip(100, "", ankaracaptchasonuc, ToolTipIcon.None);
                                                if (gulfcaptchasonuc.Contains("HATA"))
                                                {
                                                    goto Etiket_GulfSFS2_Captcha_Deneme;
                                                }
                                                else
                                                {
                                                    HtmlInputTemizleVeDoldur(driveer, "//input[@id='HashCode']", gulfcaptchasonuc);
                                                    //driveer.FindElement(By.XPath("//button[@id='btnCaptchaLogin']")).Click();
                                                    Thread.Sleep(2000);
                                                    if (IsElementPresent(By.XPath("//div[contains(@class,'toast-error')]"), driveer))
                                                    {
                                                        Trayyy1.ShowBalloonTip(100, "GULF SİGORTA PORTAL", "Kullanıcı şifre hatası var. Gulf Sigorta SFS Planor ayarları kontrol edilmeli!!!", ToolTipIcon.Error);
                                                        goto GulfPort_BeklenmeyenSonlanma;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //Trayyy1.ShowBalloonTip(100, "GULF SİGORTA SFS", "Gulf Sigorta Captcha Çözülemedi. Lütfen manuel giriş yapınız !!!", ToolTipIcon.Error);
                                                img.Save(@"C:\CMSigorta\cptchaerror\" + DateTime.Now.ToFileTime() + "__" + gulfcaptchasonuc + "__.png", ImageFormat.Png);
                                                driveer.FindElement(By.XPath("//canvas[@id='reCaptchaCanvas']")).Click();
                                                goto Etiket_GulfSFS2_Captcha_Deneme;
                                            }
                                        }


                                        wait2.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//select[@id='GsmNo']")));

                                        driveer.FindElement(By.XPath(LoginID)).Click();

                                        wait.Until(driveeer => ((IJavaScriptExecutor)driveer).ExecuteScript("return document.readyState").Equals("complete"));

                                        Thread.Sleep(1000);
                                        if (IsElementPresent(By.XPath("//div[contains(@class,'toast-error')]"), driveer))
                                        {
                                            //Trayyy1.ShowBalloonTip(100, "GULF SİGORTA PORTAL", "Captcha hatası var. Yeniden deneniyor!!!", ToolTipIcon.Error);
                                            goto Etiket_GulfPort_LoginDeneme;
                                        }



                                        ////Thread.Sleep(1000);
                                        string hatamesaj = "";
                                        //hatamesaj = driveer.FindElement(By.XPath("//div[@class='userLoginError']")).Text;

                                        //if (hatamesaj != "")
                                        //{
                                        //    //MessageBox.Show("---" + hatamesaj + "---");
                                        //    if (hatamesaj.Contains("şifre"))
                                        //    {
                                        //        goto GulfSFS2_BeklenmeyenSonlanma;
                                        //    }
                                        //    if (hatamesaj.Contains("güvenlik"))
                                        //    {
                                        //        goto Etiket_GulfSFS2_Captcha_Deneme;
                                        //    }
                                        //}

                                        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='ValidationCode']")));

                                        smsDenemeAdet = 0;

                                    Etiket_GulfPortal_SMS_Giris:
                                        try
                                        {
                                            Thread.Sleep(4000);

                                            smsDenemeAdet++;

                                            if (smsDenemeAdet < 3)
                                            {
                                                Console.WriteLine("GULF PORTAL SMS deneme sayısı:" + smsDenemeAdet);
                                                try
                                                {
                                                    if (IsElementPresent(By.XPath("//input[@id='ValidationCode']"), driveer))
                                                    {
                                                        string sonuc = "";
                                                        string sonuc2 = "";

                                                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi like 'AXA' and TelNo='" + smsnumberno + "' and Durum='0' and Tarih BETWEEN '" + myZaman1r + "' AND '" + myZaman2r + "' order by id desc limit 1");
                                                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='GULFSIGORTA' and TelNo='" + smsnumberno + "' and Durum='0' order by id desc limit 1");

                                                        if (sonuc != "")
                                                        {
                                                            Console.WriteLine(sonuc);

                                                            //HtmlInputTemizleVeDoldur(driveer, "//input[@id='ValidationCode']", sonuc);
                                                            driveer.FindElement(By.XPath("//input[@id='ValidationCode']")).SendKeys(sonuc);
                                                            driveer.FindElement(By.XPath("//input[@id='ValidationCode']")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                                        }
                                                        else
                                                        {
                                                            Thread.Sleep(2000);
                                                            goto Etiket_GulfPortal_SMS_Giris;
                                                        }

                                                        wait.Until(driveeer => ((IJavaScriptExecutor)driveer).ExecuteScript("return document.readyState").Equals("complete"));

                                                        Thread.Sleep(1000);
                                                        if (IsElementPresent(By.XPath("//div[contains(@class,'toast-error')]"), driveer))
                                                        {
                                                            //Trayyy1.ShowBalloonTip(100, "GULF SİGORTA PORTAL", "Captcha hatası var. Yeniden deneniyor!!!", ToolTipIcon.Error);
                                                            hatamesaj = driveer.FindElement(By.XPath("//div[contains(@class,'toast-error')]")).Text;
                                                            goto Etiket_GulfPortal_SMS_Giris;
                                                        }
                                                        else
                                                        {
                                                            //wait3.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'base__sidebar-nav')]")));
                                                            //if (IsElementPresent(By.XPath("//div[contains(@class,'base__sidebar-nav')]"), driveer))
                                                            //{
                                                            //    List<string> TabloAdlari = new List<string>();
                                                            //    TabloAdlari.Add("Durum");
                                                            //    ArrayList veriler = new ArrayList();
                                                            //    veriler.Add("1");
                                                            //    sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);

                                                            //    goto Etiket_GulfSFS2_Giris_Basarili;
                                                            //}

                                                            List<string> TabloAdlari = new List<string>();
                                                            TabloAdlari.Add("Durum");
                                                            ArrayList veriler = new ArrayList();
                                                            veriler.Add("1");
                                                            sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);

                                                            goto Etiket_GulfPort_Giris_Basarili;
                                                        }
                                                    }
                                                    else
                                                    {

                                                        //goto Etiket_GulfSFS2_SMS_Giris;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    //hata ayıklama logla
                                                    goto Etiket_GulfPortal_SMS_Giris;
                                                }
                                            }
                                            else
                                            {
                                                Trayyy1.ShowBalloonTip(50, "GULF SİGORTA SFS", "Gulf Sigorta SMS sorunu oluştu. Lütfen yeniden açmayı deneyiniz !!!", ToolTipIcon.Error);
                                                goto GulfPort_BeklenmeyenSonlanma;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //hata ayıklama logla
                                            //goto Etiket_Ankara_2FAGiris;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Etiket Gulf Portal Login Deneme Try Hatası: " + ex.ToString());
                                    Thread.Sleep(1000);
                                    goto Etiket_GulfPort_LoginDeneme;
                                }
                            }
                            catch (Exception ex)
                            {
                                Trayyy1.ShowBalloonTip(100, "GULF SİGORTA PORTAL", "Gulf Sigorta Sistemlerinde sorun mevcut. Lütfen daha sonra tekrar deneyiniz !!!", ToolTipIcon.Error);
                                goto GulfPort_BeklenmeyenSonlanma;
                            }

                        Etiket_GulfPort_Giris_Basarili:
                            try
                            {
                                //Burada şirket açılınca acente tarafında ayarlanacak giriş mesajları uyarılar felan ne olursa onlar çalıştırılacak.
                                Trayyy1.ShowBalloonTip(100, "", "GULF Sigorta Portal hazır...", ToolTipIcon.Info);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }


                        GulfPort_BeklenmeyenSonlanma:
                            try
                            {
                                //Console.Beep();
                                backgroundWorker1.CancelAsync();
                                backgroundWorker2.CancelAsync();
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }

                            break;

                        #endregion
















                        #region SFS ekranlar 
                        case string x when UrlAdres.Contains("rayport.raysigorta.com.tr")
                                        || UrlAdres.Contains("ac.sompojapan.com.tr")
                                        || UrlAdres.Contains("online.gulfsigorta.com.tr")
                                        || UrlAdres.Contains("quicksigorta.com")
                                        || UrlAdres.Contains("galaksi.turknippon.com"):
                            //Eğer yukardakilerden biri olduğunda ama NİPPON değilken
                            if (!x.Contains("galaksi.turknippon.com"))
                            {
                                // SFS içermediğinde ama ve QUICK olduğunda - yani QUİCK web sitesinde isen
                                if (!x.Contains("SFS") && x.Contains("quicksigorta.com"))
                                {
                                    driveer.FindElement(By.LinkText("Acente Girişi")).Click();
                                }
                                //QUİCK SFS de olduğunda
                                else
                                {
                                    wait2.Until(ExpectedConditions.ElementIsVisible(By.Id("fraBottom")));
                                    driveer.SwitchTo().Frame("fraBottom");
                                }
                            }

                            IWebElement beklenen = wait2.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                            if (IsElementPresent(By.Id(KullaniciID), driveer))
                            {
                                driveer.FindElement(By.Id(KullaniciID)).Clear();
                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);

                                if (x.Contains("galaksi.turknippon.com"))
                                {
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                    GelenKullaniciAdi = KullaniciAdi;
                                    //TimerKareKod.Start();
                                    DoLikeKareKodtimer(driveer);
                                }
                                else
                                {
                                    if (!x.Contains("SFS") && x.Contains("quicksigorta.com"))
                                    {
                                        driveer.FindElement(By.Id(SifreID)).Click();
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                        try
                                        {
                                            wait2.Until(ExpectedConditions.ElementIsVisible(By.Id("agency-gsm")));
                                            if (IsElementPresent(By.Name("gsm"), driveer))
                                            {
                                                select = new SelectElement(driveer.FindElement(By.Name("gsm")));
                                                select.SelectByValue("5");
                                                driveer.FindElement(By.XPath("/html/body/div[4]/div[12]/div[2]/form[2]/button")).Click();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            string hata = ex.ToString();
                                            gn.LocalLoglaAsync(isimLBL.Text, "Quick Sigorta SMS giriş sayfasına ulaşılırken hata", hata);
                                        }
                                    }
                                    else
                                    {
                                        driveer.FindElement(By.Id(LoginID)).Click();
                                        Thread.Sleep(2000);
                                        if (x.Contains("quicksigorta.com/SFS"))
                                        {
                                            try
                                            {
                                                wait2.Until(ExpectedConditions.ElementIsVisible(By.Id("fraMain")));
                                                //Console.Beep();
                                            }
                                            catch (WebDriverTimeoutException ex)
                                            {
                                                string hata = ex.ToString();
                                                gn.LocalLoglaAsync(isimLBL.Text, "Quick fraMain yakalamada hata", hata);
                                            }
                                            if (IsElementPresent(By.XPath("/html/body/form/table/tbody/tr[4]/td[3]/div[1]/div[3]/table/tbody/tr[2]/td[2]/input"), driveer))
                                            {
                                                //TimerSMS.Start();
                                                DoLikeSMStimer(driveer);
                                            }
                                            else
                                            {
                                                gn.LocalLoglaAsync(isimLBL.Text, "Quick SMS şifresi yazılacak input Box bulunamadı", "X");
                                            }
                                        }
                                        driveer.SwitchTo().DefaultContent();
                                    }

                                }
                            }
                            else GirisYap(ie_driver);
                            break;
                        #endregion

                        case string x when UrlAdres.Contains("login.microsoftonline.com"):
                            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                            if (IsElementPresent(By.Id(KullaniciID), driveer))
                            {
                                //await Task.Delay(2000);
                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(SifreID)));
                                //driveer.FindElement(By.Id(SifreID)).Click();
                                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@data-value='PhoneAppOTP']")));
                                driveer.FindElement(By.XPath("//div[@data-value='PhoneAppOTP']")).Click();
                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idTxtBx_SAOTCC_OTC")));
                                //driveer.FindElement(By.Id(LoginID)).Click();
                                GelenKullaniciAdi = KullaniciAdi;
                                //KullaniciAdi = "";
                                DoLikeKareKodtimer(driveer);
                                //TimerKareKod.Start();
                            }
                            else GirisYap(chrm_driver);
                            break;

                        case string x when UrlAdres.Contains("unicosigorta.com.tr/online-islemler/acente/unikolay-giris"):
                            wait.Until(ExpectedConditions.ElementIsVisible(By.TagName(KullaniciID)));
                            driveer.FindElements(By.TagName(KullaniciID))[0].SendKeys(KullaniciAdi);
                            driveer.FindElements(By.TagName(SifreID))[1].SendKeys(Sifre);
                            driveer.FindElement(By.Id(LoginID)).Click();
                            TimerKareKod2.Start();
                            break;

                        case string x when UrlAdres.Contains("biz.hdisigorta.com.tr"):
                            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                            if (IsElementPresent(By.Id(KullaniciID), driveer))
                            {
                                driveer.FindElement(By.Id(KullaniciID)).Clear();
                                driveer.FindElement(By.Id(KullaniciID)).Click();
                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                                GelenKullaniciAdi = KullaniciAdi;
                                KullaniciAdi = "";
                                driveer.FindElement(By.Id(SifreID)).Click();
                                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                driveer.FindElements(By.TagName("button"))[0].Click();
                                TimerKareKod.Start();
                            }
                            else GirisYap(chrm_driver);
                            break;

                        // standart giriş yöntemi ile halledilebilen diğer şirketler
                        default:

                            try
                            {
                                // Selen XML den gelen  Kullanıcı input ID nin varlığını kontrol edip 
                                // Şirketlerin XML inden gelen şifreleri inputlara yaz
                                wait2.Until(ExpectedConditions.ElementIsVisible(By.Id(KullaniciID)));
                                driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                            }
                            catch (Exception EEEXXX)
                            {
                                LogKaydet(KullaniciID, sirketadi, EEEXXX.ToString());
                            }


                            switch (UrlAdres)
                            {
                                // axa resim cacpcha yazıcı
                                case string y when UrlAdres.Contains("oasis.axasigorta.com.tr"):

                                    if (IsElementPresent(By.Id("CPH_MiddlePane_Im1"), driveer))
                                    {

                                        string axaanahtar = "BOSLUK";
                                        int dongumax = 0;
                                        while (axaanahtar == "BOSLUK" && dongumax < 3)
                                        {
                                            dongumax++;
                                            IWebElement resimkodu = driveer.FindElement(By.Id("CPH_MiddlePane_Im1"));
                                            var img = GetElementScreenShot(driveer, resimkodu);
                                            try
                                            {
                                                axaanahtar = AxaCaptchaCoz(img);
                                            }
                                            catch
                                            {
                                                /*
                                                using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"PNG|*.png" })
                                                {
                                                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                                    {
                                                        img.Save(saveFileDialog.FileName);
                                                    }
                                                }
                                                */

                                                // çözülemeyen Axa Captcha yı FTP ye analiz için gönder
                                                //uploadFile(img);
                                                //çözülemeyen Axa Captcha yı TEMP e kaydet
                                                img.Save(Path.GetTempPath() + DateTime.Now.ToFileTime() + "_img.png", System.Drawing.Imaging.ImageFormat.Png);

                                                img = null;

                                                driveer.FindElement(By.Id("CPH_MiddlePane_imgrefresh")).Click();

                                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id(SifreID)));
                                                driveer.FindElement(By.Id(SifreID)).Clear();
                                                driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);

                                                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CPH_MiddlePane_Im1")));
                                            }
                                        }
                                        if (axaanahtar != "BOSLUK")
                                        {
                                            Trayyy1.ShowBalloonTip(100, "Axa Anahtar Çözüldü", axaanahtar, ToolTipIcon.Info);
                                            driveer.FindElement(By.Id("CPH_MiddlePane_edtCaptcha")).SendKeys(axaanahtar);
                                            driveer.FindElement(By.Id(LoginID)).Click();
                                            //TimerSMS.Start();
                                            DoLikeSMStimer(driveer);
                                        }
                                    }
                                    break;

                                // bu şirketler login işlevinde ID yerine XPATH kullanıyorlar
                                case string y when UrlAdres.Contains("auth.korusigortaportal.com")
                                                || UrlAdres.Contains("adaauth.dogasigorta.com")
                                                || UrlAdres.Contains("auth.aveonglobalsigorta.com")
                                                || UrlAdres.Contains("auth.privesigorta.com")
                                                || UrlAdres.Contains("portal.anasigorta.com.tr")
                                                || UrlAdres.Contains("auth.grisigorta.com.tr")
                                                || UrlAdres.Contains("kolayekran.mapfre.com.tr"):

                                    driveer.FindElement(By.XPath(LoginID)).Click();

                                    // Bunlar XPATH kullanların Google 2 Faktör ile giriş yaptıranları
                                    if (y.Contains("auth.korusigortaportal.com")
                                     || y.Contains("adaauth.dogasigorta.com")
                                     || y.Contains("auth.aveonglobalsigorta.com")
                                     || y.Contains("auth.privesigorta.com")
                                     || y.Contains("portal.anasigorta.com.tr")
                                     || y.Contains("auth.grisigorta.com.tr"))
                                    {
                                        GelenKullaniciAdi = KullaniciAdi;
                                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Code")));
                                        //MessageBox.Show("Bulundu");
                                        //TimerKareKod.Start();
                                        DoLikeKareKodtimer(driveer);
                                    }
                                    break;

                                case string y when UrlAdres.Contains("anka.groupama.com.tr"):
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                                    //driveer.FindElement(By.Id("recaptcha-anchor")).SendKeys(OpenQA.Selenium.Keys.Space);
                                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("txtGAKod")));
                                    GelenKullaniciAdi = KullaniciAdi;
                                    //TimerKareKod.Start();
                                    DoLikeKareKodtimer(driveer);
                                    break;

                                // LoginID işlevini Id element ile yapan diğer bütün şirketlere
                                default:
                                    driveer.FindElement(By.Id(LoginID)).Click();
                                    break;
                            }
                            break;
                    }
                    //loginID işlevlerini bitirdikten sonra 2 faktör yapması için timer gitmesi gereken siteler
                    if (UrlAdres.Contains("nareks.bereket.com.tr")
                     || UrlAdres.Contains("acente.atlasmutuel.com.tr/")
                     || UrlAdres.Contains("anka.groupama.com.tr")
                     || UrlAdres.Contains("generali.com.tr")
                     || UrlAdres.Contains("sigorta.neova.com.tr")
                     || UrlAdres.Contains("orientsigorta.com.tr"))
                    {
                        GelenKullaniciAdi = KullaniciAdi;
                        //TimerKareKod.Start();
                        DoLikeKareKodtimer(driveer);
                    }
                }
                //LoginID değeri XML yok yada boş ise
                else
                {
                    //Console.Beep(8000, 100);
                    //Console.Beep(8000, 100);
                    //Console.Beep(8000, 100);
                    switch (UrlAdres)
                    {
                        case string x when UrlAdres.Contains("ssoswepweb.anadolusigorta"):
                            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("master"));

                            //await Task.Delay(3000);
                            //driveer.SwitchTo().Frame("master");
                            //await Task.Delay(10000);

                            driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                            driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                            driveer.SwitchTo().DefaultContent();
                            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("master"));
                            driveer.FindElement(By.Id("login:subBut")).Click();
                            driveer.FindElement(By.Id("login:subBut")).SendKeys(OpenQA.Selenium.Keys.Enter);
                            driveer.SwitchTo().DefaultContent();
                            break;

                        case string x when UrlAdres.Contains("acenteportal.groupama.com.tr"):
                            driveer.FindElement(By.Id("txtAgencyCode")).SendKeys(acente_kodu);
                            driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                            driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                            break;

                        case string x when UrlAdres.Contains("quicksigorta"):
                            driveer.SwitchTo().Frame("fraBottom");
                            driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                            driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                            driveer.SwitchTo().DefaultContent();
                            break;

                        case string x when UrlAdres.Contains("turknippon"):
                            driveer.FindElement(By.Name(KullaniciID)).SendKeys(KullaniciAdi);
                            driveer.FindElement(By.Name(SifreID)).SendKeys(Sifre);
                            IWebElement button = driveer.FindElements(By.ClassName("loginSend"))[0];
                            button.Click();
                            break;

                        case string x when UrlAdres.Contains("ejento.sompojapan.com.tr"):
                            driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                            driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                            driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Tab);
                            //TimerSMS.Start();
                            DoLikeSMStimer(driveer);
                            break;

                        default:
                            switch (UrlAdres)
                            {
                                case string x when UrlAdres.Contains("acente.atlasmutuel.com.tr")
                                                || UrlAdres.Contains("anka.groupama.com.tr"):
                                    driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    GelenKullaniciAdi = KullaniciAdi;
                                    //TimerKareKod.Start();
                                    DoLikeKareKodtimer(driveer);
                                    break;

                                case string x when UrlAdres.Contains("biz.hdisigorta.com.tr"):
                                    driveer.FindElements(By.TagName("button"))[0].Click();
                                    driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                    GelenKullaniciAdi = KullaniciAdi;
                                    //TimerKareKod.Start();
                                    DoLikeKareKodtimer(driveer);
                                    break;

                                default:
                                    try
                                    {
                                        driveer.FindElement(By.Id(KullaniciID)).SendKeys(KullaniciAdi);
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(Sifre);
                                    }
                                    catch (NoSuchElementException ex)
                                    {
                                        string hata = ex.ToString();
                                        gn.LocalLoglaAsync(isimLBL.Text, "GirisYap Fonksiyonu / Switch - Default / Switch - Default / Kullanıcı ID veya Şifre girişinde", hata);
                                    }


                                    if (UrlAdres.Contains("sigorta.corpussigorta.com.tr"))
                                    {
                                        if (IsElementPresent(By.Id("txtGuvenlikKod"), driveer))
                                        {
                                            driveer.FindElement(By.Id("txtGuvenlikKod")).SendKeys("ChNgR");
                                            driveer.FindElement(By.Id("txtGuvenlikKod")).SendKeys(OpenQA.Selenium.Keys.Enter);
                                        }
                                        driveer.FindElement(By.Id(SifreID)).SendKeys(OpenQA.Selenium.Keys.Enter);
                                    }
                                    GelenKullaniciAdi = KullaniciAdi;
                                    //TimerKareKod.Start();
                                    DoLikeKareKodtimer(driveer);
                                    break;
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Şirketin login bilgilerini XML den çektikten sonra şirkete göre login işlemi yaparken", hata);
                //MessageBox.Show(ex.ToString());
            }
        }




        [Obsolete]
        private async Task sirketegitAsync(string sirketinadi)
        {
            string sirket_id = gn.en_son_kaydi_getir("t_kullanici_sirketler", "SirketID", "where Adi='" + sirketinadi + "'");
            try
            {
                //await Task.Delay(1000);
                foreach (DataGridViewRow g1 in dgv_sirketler.Rows)
                {
                    // chngr messagebox
                    //MessageBox.Show(g1.Cells["id"].Value.ToString());

                    if (g1.Cells["id"].Value.ToString() == sirket_id)
                    {
                        // gereksiz bir değişken atama olmuş zaten sirket_id = g1.Cells["id"].Value.ToString()
                        string SirketID = g1.Cells["id"].Value.ToString();
                        string xmlfile = gn.XmlSirketler;
                        System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                        xdoc.Load(xmlfile);
                        System.Xml.XmlNodeList newXMLNodes = xdoc.SelectNodes("/sirketler/sirket");

                        foreach (System.Xml.XmlNode newXMLNode in newXMLNodes)
                        {
                            if (newXMLNode["id"].InnerText == SirketID)
                            {
                                link = decrypt(newXMLNode["link"].InnerText);
                                KullaniciAdi = decrypt(newXMLNode["kullanici"].InnerText);
                                Sifre = decrypt(newXMLNode["sifre"].InnerText);
                                acente_kodu = decrypt(newXMLNode["acentekodu"].InnerText);
                                ip = decrypt(newXMLNode["ip"].InnerText);
                                port = decrypt(newXMLNode["port"].InnerText);
                                smsnumberno = decrypt(newXMLNode["smstelno"].InnerText);
                                iemitarayici = decrypt(newXMLNode["iemitarayici"].InnerText);
                                ip_durumu = newXMLNode["ipdurum"].InnerText;
                                sirketgirmedurumu = newXMLNode["sirketdurum"].InnerText;
                                string SirketAdi = decrypt(newXMLNode["adi"].InnerText);

                                //Kullanıcı ID - Şirket Adı - Şirket ID olarak log kaydet fonksiyonuna gönder
                                LogKaydet(lbl_id.Text, SirketAdi, SirketID);

                                break;
                            }
                        }
                        break;
                    }
                }

                if (sirketgirmedurumu != "1")
                {
                    lblSirket.Text = sirketadi;

                    //Eğer SFS ekranların adresleri yakalanırsa
                    if (iemitarayici == "1")
                    {
                        ie_Git(link.ToString());
                        hangiDriver = "ie";
                        GirisYap(ie_driver);
                    }
                    // SFS ekranların adresleri değilse
                    else
                    {
                        Chrm_Git(link.ToString());
                        hangiDriver = "chrm";
                        GirisYap(chrm_driver);
                    }
                }
                else
                {
                    MessageBox.Show("Şirket Kapalı");
                }
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Şirket XML ile Şirket Buton Adını eşleştirip şirkete gitmeye çalışırken", hata);
                //MessageBox.Show(hata);
            }
        }




        //burası yeni ekrana özel yada değiştirilmiş fonksiyonlar
        [Obsolete]
        void sirketButon_Click(object sender, EventArgs e)
        {
            if (Form.ModifierKeys != System.Windows.Forms.Keys.Control)
            {
                Guna2TileButton item = (sender as Guna2TileButton);
                sirketadi = item.Text;
                //sirketegitAsync(sirketadi);
                try
                {
                    if (!backgroundWorker1.IsBusy)
                    {
                        backgroundWorker1.RunWorkerAsync();
                    }
                    else
                    {
                        backgroundWorker2.RunWorkerAsync();
                    }
                }
                catch (Exception ex)
                {
                    string xxx = ex.ToString();
                }
            }
            else
            {
                if (System.Configuration.ConfigurationSettings.AppSettings.Get("SagTuslaFarkliPencere") == "1")
                {
                    Guna2TileButton itemsag = (sender as Guna2TileButton);
                    MessageBox.Show("Açılır Ya Bu:  " + itemsag.Text);
                }
            }
        }

        void sirketButon_DoubleClick(object sender, EventArgs e)
        {
            //sirketButon_Click(sender,e);
            //Console.Beep();

            //Guna2TileButton item = (sender as Guna2TileButton);
            //item.PerformClick();

            Application.DoEvents();

        }


        //burası yeni ekrana özel yada değiştirilmiş fonksiyonlar
        [Obsolete]
        void TraysirketButon_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (sender as ToolStripMenuItem);
            sirketadi = item.Text;
            //sirketegitAsync(sirketadi);
            try
            {
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    backgroundWorker2.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                string xxx = ex.ToString();
            }
        }

        void menuButon_Click(object sender, EventArgs e)
        {
            Guna2GradientButton item = (sender as Guna2GradientButton);
            MenuDegistir(item.Text);
        }

        private void MenuDegistir(string MenuText)
        {
            switch (MenuText)
            {
                case "ANA SAYFA":
                    //AnaSayfa2.cs şimdilik kullanılmayacak devredışı Anasayfa Panel Slider oldu
                    //PanelSlider.Controls.Find("AnaSayfa2", false)[0].BringToFront();
                    if (g2anaMenuBTN1.Checked == false)
                    {
                        PanelSlider.Controls.Find("Hakkimizda", false)[0].SendToBack();
                        Application.DoEvents();
                        PanelSlider.Controls.Find("Ayarlar", false)[0].SendToBack();
                        Application.DoEvents();
                        PanelSlider.Controls.Find("HizliTeklif", false)[0].SendToBack();
                        Application.DoEvents();
                        PanelSlider.Controls.Find("Yonetici", false)[0].SendToBack();
                        Application.DoEvents();
                        PanelSlider.Controls.Find("HizliAraclar", false)[0].SendToBack();
                        Application.DoEvents();
                    }
                    break;
                case "HIZLI TEKLİF":
                    PanelSlider.Controls.Find("HizliTeklif", false)[0].BringToFront();
                    break;
                case "AYARLAR":
                    PanelSlider.Controls.Find("Ayarlar", false)[0].BringToFront();
                    new Ayarlar().Size = new Size(PanelSlider.Width, PanelSlider.Height);
                    break;
                case "HAKKIMIZDA":
                    PanelSlider.Controls.Find("Hakkimizda", false)[0].BringToFront();
                    break;
                case "YÖNETİCİ":
                    PanelSlider.Controls.Find("Yonetici", false)[0].BringToFront();
                    break;
                case "HIZLI ARAÇLAR":
                    PanelSlider.Controls.Find("HizliAraclar", false)[0].BringToFront();
                    break;
                case "BU BOS CASE KALACAK":
                    PanelSlider.Controls.Find("Silme Kalsın", false)[0].BringToFront();
                    break;
                default:
                    //bunlardan hiçbiri ise yapılacak işlem
                    break;
            }
            foreach (Guna2GradientButton button in AnaMenuPaneli.Controls.OfType<Guna2GradientButton>())
            {
                if (button.Text == MenuText)
                {
                    button.Checked = true;
                }
                else
                {
                    button.Checked = false;
                }
                //Ana Menü düğmelerine hızlı basışlarda menü efekti takılmasını düzeltti.
                Application.DoEvents();
            }
            GC.Collect();
        }

        private void cikisyap()
        {
            try
            {
                Trayyy1.Visible = false;

                ProcessStartInfo kmt = new ProcessStartInfo();
                kmt.CreateNoWindow = true;
                kmt.FileName = "cmd.exe";
                kmt.WindowStyle = ProcessWindowStyle.Hidden;
                kmt.Arguments = @"/c taskkill /f /im chromedriver.exe";
                Process.Start(kmt);
                kmt.Arguments = @"/c taskkill /f /im conhost.exe";
                Process.Start(kmt);
                kmt.Arguments = @"/c taskkill /f /im chrome-cms.exe";
                Process.Start(kmt);
                kmt.Arguments = @"/c taskkill /f /im iexplore.exe";
                Process.Start(kmt);

                //if (driver != null)
                //{
                //    driver.Close();
                //    if (driver != null) driver.Quit();
                //    if (driver != null) driver.Dispose();
                //    if (chrm_driver != null) chrm_driver.Quit();
                //    if (chrm_driver != null) chrm_driver.Dispose();
                //    if (ie_driver != null) ie_driver.Quit();
                //    if (ie_driver != null) ie_driver.Dispose();
                //}
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Çıkış yapmaya çalışırken", hata);
            }
            finally
            {
                GC.Collect();
            }

            Application.Exit();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            cikisyap();
        }

        private void GorunumDegistir()
        {
            int wdth = screens[ekranno].WorkingArea.Width;
            int hght = screens[ekranno].WorkingArea.Height;

            AnaMenuPaneli.Size = new Size(wdth / 8, hght);
            g2LogoBTN.Size = new Size((wdth / 8) - 6, hght / 10);
            g2anaMenuBTN1.Size = new Size((wdth / 8) - 6, hght / 10);
            g2anaMenuBTN2.Size = new Size((wdth / 8) - 6, (hght / 10));
            g2anaMenuBTN3.Size = new Size((wdth / 8) - 6, (hght / 10));
            g2anaMenuBTN4.Size = new Size((wdth / 8) - 6, (hght / 10));
            g2anaMenuBTN5.Size = new Size((wdth / 8) - 6, (hght / 10));
            //g2LogoBTN.Size = new Size((wdth / 8) - 6, (hght / 10) - 22);


            if (wdth > 1600)
            {
                this.ANAicerikBaslikLBL.Font = new Font("Segoe UI Semibold", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            }
            else
            {
                this.ANAicerikBaslikLBL.Font = new Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            }
        }

        //Elemet resize DENEME
        public void Olcekle(IWebElement element, int offsetX, int offsetY = 0)
        {
            int width = element.Size.Width;
            Actions action = new Actions(driver);
            action.MoveToElement(element, width, 1);
            action.ClickAndHold().MoveByOffset(offsetX, offsetY).Release();
            action.Build().Perform();
        }

        //Element Resmi alma fonksiyonu
        public static Bitmap GetElementScreenShot(IWebDriver driver, IWebElement element)
        {
            Screenshot sc = ((ITakesScreenshot)driver).GetScreenshot();
            var imag = Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap;
            return imag.Clone(new Rectangle(element.Location, element.Size), imag.PixelFormat);
        }

        // çözülemeyen Axa Captcha yı FTP ye analiz için gönder
        private void uploadFile(Image unsolvedimage)
        {
            string tempfolder = Path.GetTempPath();
            string uri = "ftp://cm-yazilim.com.tr/axahataimg/";
            string fileNameUpload = DateTime.Now.ToFileTime() + "_img.png";
            string login = "webliyor";
            string password = "ocfl6xpb";

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri + fileNameUpload);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(login, password);
            request.UsePassive = true;
            //request.UseBinary = true;
            request.KeepAlive = false;

            // Copy the contents of the file to the request stream.
            string tempfile = tempfolder + fileNameUpload;
            unsolvedimage.Save(tempfile, System.Drawing.Imaging.ImageFormat.Png);

            //request. UploadFile("ftp://127.0.0.1/Image/", filepath);    //<= Pass this path here


            StreamReader sourceStream = new StreamReader(tempfile);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }


        private void DoLikeSMStimer(IWebDriver driveer)
        {
            string UrlAdres = "";
            string sonuc = "";
            string sonuc2 = "";

            //if (hangiDriver == "chrm") driver = chrm_driver;
            //if (hangiDriver == "ie") driver = ie_driver;

            driver = driveer;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 20));


        DoLike_SMS_Timer:
            try
            {
                UrlAdres = driver.Url.ToString();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Timer SMS içinde WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }

            switch (UrlAdres)
            {
                case string x when UrlAdres.Contains("oasis.axasigorta.com.tr"):
                    if (IsElementPresent(By.Id("edtSmsToken"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='.AXA.' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("edtSmsToken")).SendKeys(sonuc);
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }
                        //Console.Beep(8000, 100);
                        driver.FindElement(By.Id("btnSms_Validate")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                        Thread.Sleep(500);

                        if (IsElementPresent(By.Id("StatusMessage"), driver))
                        {
                            driver.FindElement(By.Id("edtSmsToken")).Clear();
                            TabloAdlari.Clear();
                            veriler.Clear();
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;

                case string x when UrlAdres.Contains("axatek.axasigorta.com.tr"):
                    if (IsElementPresent(By.Id("input-45"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='.AXA.' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("input-45")).SendKeys(sonuc);
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }


                        if (IsElementPresent(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[2]/div[2]/button"), driver))
                            driver.FindElement(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[2]/div[2]/button")).Click();
                        else
                            driver.FindElement(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[3]/div[2]/button")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                        Thread.Sleep(500);

                        if (IsElementPresent(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[1]/div/div/div"), driver))
                        {
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);

                            TabloAdlari.Clear();
                            veriler.Clear();
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;

                case string x when UrlAdres.Contains("giris.anadolusigorta.com.tr"):
                    if (IsElementPresent(By.Name("smsValidationCode"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='ANADOLUSIG' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Name("smsValidationCode")).SendKeys(sonuc);
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }

                        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("verifySmsValidationCodeButton")));
                        driver.FindElement(By.Id("verifySmsValidationCodeButton")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;

                case string x when UrlAdres.Contains("sat2.aksigorta.com.tr"):
                    //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/ngb-modal-window/div/div/sat-sms-dogrulama/div/div/div/div/form/div/div[3]/button")));
                    if (IsElementPresent(By.Id("smsPassword"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='AKSIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("smsPassword")).SendKeys(sonuc);
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }

                        driver.FindElement(By.XPath("/html/body/ngb-modal-window/div/div/sat-sms-dogrulama/div/div/div/div/form/div/div[3]/button")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;

                case string x when UrlAdres.Contains("ejento.sompojapan.com.tr"):
                    try
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("verifyButton")));
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        string hata = ex.ToString();
                        gn.LocalLoglaAsync(isimLBL.Text, "Sompo verifyButton beklenirken zaman aşımı", hata);
                    }
                    if (IsElementPresent(By.Name("txtSmsCode"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='SOMPO' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("txtSmsCode")).SendKeys(sonuc);
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }

                        driver.FindElement(By.Id("verifyButton")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;

                case string x when UrlAdres.Contains("rayexpress.com.tr"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/form[3]/center/div/input")));
                    if (IsElementPresent(By.XPath("/html/body/div[1]/form[3]/center/div/input"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.XPath("/html/body/div[1]/form[3]/center/div/input")).SendKeys(sonuc);
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }

                        driver.FindElement(By.Id("BtnOtpSmsGiris")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;

                case string x when UrlAdres.Contains("araci.quicksigorta.com"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("txtOtp")));
                    if (IsElementPresent(By.Id("txtOtp"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='QUICKSGORTA' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("txtOtp")).SendKeys(sonuc);
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }

                        driver.FindElement(By.Id("btnOkOtp")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;

                case string x when UrlAdres.Contains("allianz.com.tr"):
                    if (IsElementPresent(By.Id("smsToken"), driver))
                    {
                        // Allianz SMS şifresinin yazıldığı İnputBox 'ı hidden yapıyor
                        // O yüzden var ama hidden mı diye bakıyoruz
                        string tipp = driver.FindElement(By.Id("smsToken")).GetAttribute("type");
                        if (tipp == "hidden") driver.FindElement(By.Id("redirect")).Click();
                        else
                        {
                            DateTime myDate = DateTime.Now;
                            string myZaman2 = myDate.ToString();
                            //MessageBox.Show(myZaman2);
                            myZaman2 = myZaman2.Replace(".", "-");
                            myZaman2 = myZaman2.Replace(" 0", " ");
                            myDate = myDate.AddSeconds(-25);
                            string myZaman1 = myDate.ToString();
                            myZaman1 = myZaman1.Replace(".", "-");
                            myZaman1 = myZaman1.Replace(" 0", " ");

                            sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='ALLIANZ' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                            if (sonuc != "")
                            {
                                Trayyy1.ShowBalloonTip(1000, "Allianz SMS Şifre", sonuc, ToolTipIcon.Info);
                                driver.FindElement(By.Id("smsToken")).SendKeys(sonuc);
                            }
                            else
                            {
                                //TimerSMS.Start();
                                goto DoLike_SMS_Timer;
                                break;
                            }


                            driver.FindElement(By.Id("redirect")).Click();
                        }


                        //Allianz giriş ekranında bişeyleri değiştirmeden önce çalışan düğmeye tıklama kodları
                        //////////if (IsElementPresent(By.Id("redirect"), driver)) driver.FindElement(By.XPath("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////else
                        //////////{
                        //////////    await Task.Delay(1000);
                        //////////    if (IsElementPresent(By.Id("redirect"), driver)) driver.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////    else
                        //////////    {
                        //////////        await Task.Delay(1000);
                        //////////        if (IsElementPresent(By.Id("redirect"), driver)) driver.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////        else
                        //////////        {
                        //////////            await Task.Delay(1000);
                        //////////            driver.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////        }
                        //////////    }
                        //////////}
                        ///



                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        if (IsElementPresent(By.Id("redirect"), driver))
                        {
                            driver.FindElement(By.Id("redirect")).Click();
                        }
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                        }
                    }
                    break;

                case string x when UrlAdres.Contains("pusula.turkiyesigorta.com.tr"):
                    //Console.Beep();
                    //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("j_id9:j_id39:MFAValue")));
                    if (IsElementPresent(By.Id("j_id9:j_id39:MFAValue"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-35);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='TURKIYESGRT' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "")
                        {
                            driver.FindElement(By.Id("j_id9:j_id39:MFAValue")).SendKeys(sonuc);
                            driver.FindElement(By.Id("j_id9:j_id49:mfaButton")).Click();
                        }
                        else
                        {
                            //TimerSMS.Start();
                            goto DoLike_SMS_Timer;
                            break;
                        }


                        //türkiye sigortanın gelen mesaj tablasundaki kaydının Durum değişkenini 1 yapmıyoruz.
                        //List<string> TabloAdlari = new List<string>();
                        //TabloAdlari.Add("Durum");
                        //ArrayList veriler = new ArrayList();
                        //veriler.Add("1");
                        //sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        //TimerSMS.Start();
                        goto DoLike_SMS_Timer;
                    }
                    break;
            }


        }


        [Obsolete]
        private async void TimerSMS_Tick(object sender, EventArgs e)
        {
            TimerSMS.Stop();

            string UrlAdres = "";
            string sonuc = "";
            string sonuc2 = "";

            if (hangiDriver == "chrm") driver = chrm_driver;
            if (hangiDriver == "ie") driver = ie_driver;

            try
            {
                UrlAdres = driver.Url.ToString();
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Timer SMS içinde WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }

            if (UrlAdres == "")
            {

            }

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 20));

            switch (UrlAdres)
            {
                case string x when UrlAdres.Contains("oasis.axasigorta.com.tr"):
                    if (IsElementPresent(By.Id("edtSmsToken"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='.AXA.' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("edtSmsToken")).SendKeys(sonuc);
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }
                        //Console.Beep(8000, 100);
                        driver.FindElement(By.Id("btnSms_Validate")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                        await Task.Delay(500);

                        if (IsElementPresent(By.Id("StatusMessage"), driver))
                        {
                            driver.FindElement(By.Id("edtSmsToken")).Clear();
                            TabloAdlari.Clear();
                            veriler.Clear();
                            TimerSMS.Start();
                            break;
                        }
                    }
                    else TimerSMS.Start();
                    break;

                case string x when UrlAdres.Contains("axatek.axasigorta.com.tr"):
                    if (IsElementPresent(By.Id("input-45"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='.AXA.' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("input-45")).SendKeys(sonuc);
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }


                        if (IsElementPresent(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[2]/div[2]/button"), driver))
                            driver.FindElement(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[2]/div[2]/button")).Click();
                        else
                            driver.FindElement(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[3]/div[2]/button")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                        await Task.Delay(500);

                        if (IsElementPresent(By.XPath("/html/body/div/div[1]/div/main/div/div/div/main/div/div/div/div/div[2]/div/form/div[1]/div/div/div"), driver))
                        {
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);
                            driver.FindElement(By.Id("input-45")).SendKeys(OpenQA.Selenium.Keys.Backspace);

                            TabloAdlari.Clear();
                            veriler.Clear();
                            TimerSMS.Start();
                            break;
                        }
                    }
                    else TimerSMS.Start();
                    break;

                case string x when UrlAdres.Contains("giris.anadolusigorta.com.tr"):
                    if (IsElementPresent(By.Name("smsValidationCode"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='ANADOLUSIG' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Name("smsValidationCode")).SendKeys(sonuc);
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }


                        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("verifySmsValidationCodeButton")));
                        driver.FindElement(By.Id("verifySmsValidationCodeButton")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else TimerSMS.Start();
                    break;

                case string x when UrlAdres.Contains("sat2.aksigorta.com.tr"):
                    //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/ngb-modal-window/div/div/sat-sms-dogrulama/div/div/div/div/form/div/div[3]/button")));
                    if (IsElementPresent(By.Id("smsPassword"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='AKSIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("smsPassword")).SendKeys(sonuc);
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }

                        driver.FindElement(By.XPath("/html/body/ngb-modal-window/div/div/sat-sms-dogrulama/div/div/div/div/form/div/div[3]/button")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else TimerSMS.Start();
                    break;

                case string x when UrlAdres.Contains("ejento.sompojapan.com.tr"):
                    try
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("verifyButton")));
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        string hata = ex.ToString();
                        gn.LocalLoglaAsync(isimLBL.Text, "Sompo verifyButton beklenirken zaman aşımı", hata);
                    }
                    if (IsElementPresent(By.Name("txtSmsCode"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='SOMPO' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("txtSmsCode")).SendKeys(sonuc);
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }

                        driver.FindElement(By.Id("verifyButton")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else TimerSMS.Start();
                    break;

                case string x when UrlAdres.Contains("rayexpress.com.tr"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/form[3]/center/div/input")));
                    if (IsElementPresent(By.XPath("/html/body/div[1]/form[3]/center/div/input"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.XPath("/html/body/div[1]/form[3]/center/div/input")).SendKeys(sonuc);
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }

                        driver.FindElement(By.Id("BtnOtpSmsGiris")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else TimerSMS.Start();
                    break;

                case string x when UrlAdres.Contains("araci.quicksigorta.com"):
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("txtOtp")));
                    if (IsElementPresent(By.Id("txtOtp"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-20);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='QUICKSGORTA' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "") driver.FindElement(By.Id("txtOtp")).SendKeys(sonuc);
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }

                        driver.FindElement(By.Id("btnOkOtp")).Click();
                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else TimerSMS.Start();
                    break;

                case string x when UrlAdres.Contains("allianz.com.tr"):
                    if (IsElementPresent(By.Id("smsToken"), driver))
                    {
                        // Allianz SMS şifresinin yazıldığı İnputBox 'ı hidden yapıyor
                        // O yüzden var ama hidden mı diye bakıyoruz
                        string tipp = driver.FindElement(By.Id("smsToken")).GetAttribute("type");
                        if (tipp == "hidden") driver.FindElement(By.Id("redirect")).Click();
                        else
                        {
                            DateTime myDate = DateTime.Now;
                            string myZaman2 = myDate.ToString();
                            //MessageBox.Show(myZaman2);
                            myZaman2 = myZaman2.Replace(".", "-");
                            myZaman2 = myZaman2.Replace(" 0", " ");
                            myDate = myDate.AddSeconds(-25);
                            string myZaman1 = myDate.ToString();
                            myZaman1 = myZaman1.Replace(".", "-");
                            myZaman1 = myZaman1.Replace(" 0", " ");

                            sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='ALLIANZ' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                            if (sonuc != "")
                            {
                                Trayyy1.ShowBalloonTip(1000, "Allianz SMS Şifre", sonuc, ToolTipIcon.Info);
                                driver.FindElement(By.Id("smsToken")).SendKeys(sonuc);
                            }
                            else
                            {
                                TimerSMS.Start();
                                break;
                            }


                            driver.FindElement(By.Id("redirect")).Click();
                        }


                        //Allianz giriş ekranında bişeyleri değiştirmeden önce çalışan düğmeye tıklama kodları
                        //////////if (IsElementPresent(By.Id("redirect"), driver)) driver.FindElement(By.XPath("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////else
                        //////////{
                        //////////    await Task.Delay(1000);
                        //////////    if (IsElementPresent(By.Id("redirect"), driver)) driver.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////    else
                        //////////    {
                        //////////        await Task.Delay(1000);
                        //////////        if (IsElementPresent(By.Id("redirect"), driver)) driver.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////        else
                        //////////        {
                        //////////            await Task.Delay(1000);
                        //////////            driver.FindElement(By.Id("/html/body/div[3]/div[1]/div[2]/div[2]/div[1]/div/div[3]/button")).Click();
                        //////////        }
                        //////////    }
                        //////////}
                        ///



                        List<string> TabloAdlari = new List<string>();
                        TabloAdlari.Add("Durum");
                        ArrayList veriler = new ArrayList();
                        veriler.Add("1");
                        sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else
                    {
                        if (IsElementPresent(By.Id("redirect"), driver))
                        {
                            driver.FindElement(By.Id("redirect")).Click();
                        }
                        else
                        {
                            TimerSMS.Start();
                        }
                    }
                    break;

                case string x when UrlAdres.Contains("pusula.turkiyesigorta.com.tr"):
                    //Console.Beep();
                    //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("j_id9:j_id39:MFAValue")));
                    if (IsElementPresent(By.Id("j_id9:j_id39:MFAValue"), driver))
                    {
                        DateTime myDate = DateTime.Now;
                        string myZaman2 = myDate.ToString();
                        myZaman2 = myZaman2.Replace(".", "-");
                        myZaman2 = myZaman2.Replace(" 0", " ");
                        myDate = myDate.AddSeconds(-35);
                        string myZaman1 = myDate.ToString();
                        myZaman1 = myZaman1.Replace(".", "-");
                        myZaman1 = myZaman1.Replace(" 0", " ");

                        //sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='RAY SIGORTA' and Durum='0' and TelNo='" + smsnumberno + "' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        sonuc = gn.en_son_kaydi_getir("GelenMesaj", "Mesaj", "where SirketAdi='TURKIYESGRT' and Durum='0' and Tarih BETWEEN '" + myZaman1 + "' AND '" + myZaman2 + "'");
                        if (sonuc != "")
                        {
                            driver.FindElement(By.Id("j_id9:j_id39:MFAValue")).SendKeys(sonuc);
                            driver.FindElement(By.Id("j_id9:j_id49:mfaButton")).Click();
                        }
                        else
                        {
                            TimerSMS.Start();
                            break;
                        }


                        //türkiye sigortanın gelen mesaj tablasundaki kaydının Durum değişkenini 1 yapmıyoruz.
                        //List<string> TabloAdlari = new List<string>();
                        //TabloAdlari.Add("Durum");
                        //ArrayList veriler = new ArrayList();
                        //veriler.Add("1");
                        //sonuc2 = gn.db_duzenle(TabloAdlari, "GelenMesaj", veriler, "Mesaj", sonuc);
                    }
                    else TimerSMS.Start();
                    break;
            }

        }






        //programın açılacağı ekranı ayarlayan fonksiyon
        private void setFormLocation(Form form, Screen screen)
        {
            // first method
            Rectangle bounds = screen.WorkingArea;
            form.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            form.StartPosition = FormStartPosition.Manual;

            // second method
            //Point location = screen.Bounds.Location;
            //Size size = screen.Bounds.Size;

            //form.Left = location.X;
            //form.Top = location.Y;
            //form.Width = size.Width;
            //form.Height = size.Height;
        }






























        private async void ie_Git(string adres)
        {

            //bool ie_driver_kapalimiydi = false;
            //string haniymis = "";

            //try
            //{
            //    haniymis = ie_driver.WindowHandles.Count.ToString();
            //}
            //catch (Exception ex)
            //{
            //    string hata = ex.ToString();
            //    gn.LocalLoglaAsync(isimLBL.Text, "IE WebDriver ile şirkete gitmeye çalışırken Handle edilemeyen sayfa", hata);

            //    if (hata.Contains("reachable"))
            //    {
            //        haniymis = "KAPALI";

            //        //chrm_driver.Close();
            //        ie_driver.Quit();
            //        ie_driver.Dispose();
            //        System.Diagnostics.ProcessStartInfo kmt = new System.Diagnostics.ProcessStartInfo();
            //        kmt.CreateNoWindow = true;
            //        kmt.FileName = "cmd.exe";
            //        kmt.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //        kmt.Arguments = @"/c taskkill /f /im IEDriverServer.exe";
            //        System.Diagnostics.Process.Start(kmt);
            //        kmt.Arguments = @"/c taskkill /f /im conhost.exe";
            //        System.Diagnostics.Process.Start(kmt);
            //        //kmt.Arguments = @"/c taskkill /f /im iexplore.exe";
            //        //System.Diagnostics.Process.Start(kmt);
            //        GC.Collect();
            //        await Task.Delay(1000);
            //    }
            //    else
            //    {
            //        if (hata.Contains("Nesne")) haniymis = "YOK";
            //        else Console.Beep();//MessageBox.Show("Diğer Hata: " + hata.ToString());//chngrzdmr
            //    }

            //    ie_driver_kapalimiydi = true;

            try
            {
                InternetExplorerDriverService ie_service = InternetExplorerDriverService.CreateDefaultService();
                ie_service.SuppressInitialDiagnosticInformation = true;
                ie_service.HideCommandPromptWindow = true;
                InternetExplorerOptions ie_opt = new InternetExplorerOptions();

                //Eğer şirket için belirlenen Proxy Server ile aynı yerde değilse
                if (ip != lbl_ip.Text)
                {
                    ie_pr.Kind = ProxyKind.Manual;
                    ie_pr.IsAutoDetect = false;
                    ie_pr.SslProxy = ip + ":" + port;
                    ie_opt.Proxy = ie_pr;
                    ie_opt.UsePerProcessProxy = true;
                    ie_opt.IgnoreZoomLevel = true;
                }
                ie_driver = new InternetExplorerDriver(ie_service, ie_opt);
                ie_driver.Navigate().GoToUrl(adres);
                ie_driver.Manage().Window.Maximize();
                ie_driver.Navigate().GoToUrl(link.ToString());
            }
            catch (Exception ex2)
            {
                string hata = ex2.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Yeni IE Driver oluşturulurken hata", hata);
                //MessageBox.Show("Diğer Hata: " + hata);
            }


            //}

            //if (ie_driver_kapalimiydi) ie_driver.Navigate().GoToUrl(adres);
            //else
            //{
            //    // internet exploerer yeni sekmede açamadğımız için burası iptal.//
            //    /*
            //    if (ie_driver.WindowHandles.Count > 0) ie_driver.SwitchTo().Window(ie_driver.WindowHandles[ie_driver.WindowHandles.Count - 1]);
            //    IJavaScriptExecutor js = (IJavaScriptExecutor)ie_driver;
            //    js.ExecuteScript("window.open(" + adres + ", '_blank').focus();");
            //    ie_driver.SwitchTo().Window(ie_driver.WindowHandles.Last());
            //    */
            //    // internet exploerer yeni sekmede açamadğımız için burası iptal.//

            //    ie_driver.Navigate().GoToUrl(adres);
            //}
            //haniymis = ie_driver.WindowHandles.Count.ToString();
        }


        private void wc_AsyncCompletedEventHandler(object sender, AsyncCompletedEventArgs e)
        {
            tableLayoutPanel1.Enabled = true;
            //this.Enabled = true;
            DurumLBL.Text = "TAMAMLANDI";
            Thread.Sleep(1000);
            DurumLBL.Text = "PAKETLER AÇILIYOR...";

            try
            {
                ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory() + "/chrm.zip", @"C://CMSigorta/chrm/");
            }
            catch (DirectoryNotFoundException ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Timer Karekod içinde WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }
            catch (FileNotFoundException ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Timer Karekod içinde WebDriver ın girdiği URL yi UrlAdres değişkenine atarken", hata);
            }
            finally
            {
                DurumLBL.Text = "HAZIR";
                progressBar.Value = 0;
            }


            if (File.Exists(@"C://CMSigorta/chrm/chrome-win/chrome-cms.exe"))
            {
                chrm_opt.BinaryLocation = @"C://CMSigorta/chrm/chrome-win/chrome-cms.exe";
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            progressBar.Refresh();
            DurumLBL.Text = "PROGRAM İLK KULLANIM İÇİN GEREKLİ EKSİK DOSYALARI İNDİRİYOR, LÜTFEN BEKLEYİNİZ...  %" + e.ProgressPercentage.ToString();
        }





        //Chrome ile açılacak şirket sayfalarını chrome driver ile açan fonksiyon
        private async void Chrm_Git(string adres)
        {
            bool chrm_driver_kapalimiydi;

            if (gn.MySqlBaglanti.Contains("sitemusta"))
            {
                chrm_driver_kapalimiydi = true;
            }
            else chrm_driver_kapalimiydi = false;
            string haniymis = "";

            try //chrm_driver handle yakalamaya çalış
            {
                int adettt = chrm_driver.WindowHandles.Count;
                if (adettt > 0) haniymis = chrm_driver.WindowHandles.Count.ToString();
                else haniymis = "YOK";
            }
            catch (NotFoundException ex)
            {
                string hata = ex.ToString();
                //eğer Nesne hatası varsa daha heniz hiç açılmamış demektir. 
                if (hata.Contains("Nesne"))
                {
                    haniymis = "YOK";
                }
                else MessageBox.Show("Kapalı Driver Hata: " + hata.ToString());
            }
            catch (Exception ex)
            {
                string hata = ex.ToString();
                gn.LocalLoglaAsync(isimLBL.Text, "Chrome WebDriver ile şirkete gitmeye çalışırken Handle edilemeyen sayfa", ".");
                //eğer reachable hatası varsa chrome penceresi açılkdıktan sonra yakalanmış demektir.
                //açık olanları kapatıp yenisini hazırla
                if (hata.Contains("reachable"))
                {
                    haniymis = "KAPALI";

                    //chrm_driver.Close();
                    //chrm_driver.Quit();
                    chrm_driver.Dispose();

                    /*
                    System.Diagnostics.ProcessStartInfo kmt = new System.Diagnostics.ProcessStartInfo();
                    kmt.CreateNoWindow = true;
                    kmt.FileName = "cmd.exe";
                    kmt.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    kmt.Arguments = @"/c taskkill /f /im chromedriver.exe";
                    System.Diagnostics.Process.Start(kmt);
                    kmt.Arguments = @"/c taskkill /f /im conhost.exe";
                    System.Diagnostics.Process.Start(kmt);
                    //kmt.Arguments = @"/c taskkill /f /im chrome-cms.exe";
                    //System.Diagnostics.Process.Start(kmt);
                    //await System.Threading.Tasks.Task.Delay(1000);
                    */
                }
                else
                {
                    //eğer Nesne hatası varsa daha henüz hiç açılmamış demektir. 
                    if (hata.Contains("Nesne")) haniymis = "YOK";
                    else MessageBox.Show("Kapalı Driver Hata: " + hata.ToString());
                }
                chrm_driver_kapalimiydi = true;

            } //chrm_driver handle yakalamaya çalış

            if (chrm_driver_kapalimiydi)
            {

                //hatanın içindeyiz yani driver oluşturulacak 
                try
                {
                    if (ip != lbl_ip.Text)
                    {
                        chrm_pr.Kind = ProxyKind.Manual;
                        chrm_pr.IsAutoDetect = false;
                        chrm_pr.SslProxy = ip + ":" + port;
                        chrm_opt.Proxy = chrm_pr;
                        chrm_opt.AddArguments("ignore-certificate-errors");
                        //if (adres.Contains("neova")) chrm_opt.PageLoadStrategy = PageLoadStrategy.Eager;
                    }
                    chrm_driver = new ChromeDriver(chrm_service, chrm_opt);
                }
                catch (Exception ex)
                {
                    string hata = ex.ToString();
                    gn.LocalLoglaAsync(isimLBL.Text, "Chrome WebDriver kapalıysa IP eşit değilse Proxy ayarlayıp yeni Chrome WebDriver oluştururken", hata);
                    //MessageBox.Show("Driver Oluşturma Diğer Hata: " + hata);
                }
                //chrm_driver.Manage().Timeouts().PageLoad.
                try
                {
                    chrm_driver.Navigate().GoToUrl(adres);
                }
                catch (Exception ex)
                {
                    //hataları belirle CHNGR
                    string hata = ex.ToString();
                    gn.LocalLoglaAsync(isimLBL.Text, "Chrome WebDriver NAVİGATE ile gitmeye çalışırken", hata);
                }
                //new WebDriverWait(chrm_driver, new TimeSpan(0, 0, 250)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            else
            {
                chrm_driver.SwitchTo().Window(chrm_driver.WindowHandles[chrm_driver.WindowHandles.Count - 1]);
                IJavaScriptExecutor js = (IJavaScriptExecutor)chrm_driver;
                js.ExecuteScript("window.open();");
                chrm_driver.SwitchTo().Window(chrm_driver.WindowHandles.Last());
                chrm_driver.Navigate().GoToUrl(adres);
            }

            haniymis = chrm_driver.WindowHandles.Count.ToString();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////
        ///                   PROGRAMI TRAY 'A İNDİR VE ÇIKAR
        /// </summary>////////////////////////////////////////
        private void Trayyy1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            
            //this.BringToFront();
            //Trayyy1.Visible = false;
        }
        /// /////////////////////////
        private void SistemForm_Resize(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Minimized)
            //{
            //    this.Hide();
            //    //Trayyy1.Visible = true;
            //}
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cikisyap();
        }

        private void guna2PictureBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Trayyy1.ShowBalloonTip(1000, "Bekleyiniz...", "Yeniden deneme için 15 saniye daha bekleyiniz.", ToolTipIcon.None);
        }

        /// </summary>////////////////////////////////////////
        ///                   PROGRAMI TRAY 'A İNDİR VE ÇIKAR
        /// </summary>////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////
        ///







        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////
        ///                   axa
        /// </summary>////////////////////////////////////////        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////
        ///                   captcha
        /// </summary>////////////////////////////////////////        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////
        ///                   solver
        /// </summary>////////////////////////////////////////        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////
        ///                   class
        /// </summary>////////////////////////////////////////        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////
        ///                   ve
        /// </summary>////////////////////////////////////////        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////
        ///                   fonksiyonlar
        /// </summary>////////////////////////////////////////
        /// 

        public static ListBox ListBox1 = new ListBox();
        public static ListBox ListBox2 = new ListBox();


        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }


        public static string BulBakalim(Image buluncakimg)
        {
            string karakteradi = "";
            Bitmap araimg1, araimg2;
            int dngsayisi = 0;
            araimg1 = new Bitmap(buluncakimg);
            while (dngsayisi < 53)
            {
                araimg2 = new Bitmap(ListBox1.Items[dngsayisi].ToString());
                //pb20 = araimg2;
                //pb20.Refresh();
                if (Karsilastir(araimg1, araimg2))
                {
                    karakteradi = ListBox1.Items[dngsayisi].ToString();
                    break;
                }
                else
                {
                    //HATA yazmak için  bu şekilde kullanıldı.
                    karakteradi = "c:\\HATA.png";
                }

                dngsayisi++;
            }
            karakteradi = Path.GetFileNameWithoutExtension(karakteradi);
            //MessageBox.Show(dngsayisi.ToString() + " kez döndü");
            return karakteradi;
        }


        public static bool Karsilastir(Image anahtarimg, Image hedefimg)
        {
            try
            {
                string img1_ref, img2_ref;
                int count1 = 0, count2 = 0;
                bool flag = true;
                Bitmap img1 = new Bitmap(anahtarimg);
                Bitmap img2 = new Bitmap(hedefimg);

                if (img1.Width == img2.Width && img1.Height == img2.Height)
                {
                    for (int i = 20; i < img1.Width; i++)
                    {
                        for (int j = 20; j < img1.Height; j++)
                        {
                            img1_ref = img1.GetPixel(i, j).ToString();
                            img2_ref = img2.GetPixel(i, j).ToString();
                            if (img1_ref != img2_ref)
                            {
                                count2++;
                                flag = false;
                                if (count2 > 100)
                                {
                                    break;
                                }

                            }
                            count1++;
                        }
                    }

                    if (flag == false)
                    {
                        if (count2 > 100)
                        {
                            //MessageBox.Show("Sorry, Images are not same , " + count2 + " wrong pixels found");
                            return false;
                        }
                        else
                        {
                            //MessageBox.Show("Resimler çok yakın, " + count1 + " same pixels found  and " + count2 + " wrong pixels found");
                            return true;
                        }
                    }
                    else
                    {
                        //MessageBox.Show(" Images are same , " + count1 + " same pixels found  and " + count2 + " wrong pixels found");
                        return true;
                    }

                }
                else
                {
                    MessageBox.Show("Bu Resim Karşılaştırmaya Uygun Değil!!!");
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }



        private byte[] ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                return imageBytes;
            }
        }


        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9')
                    || (str[i] >= 'A' && str[i] <= 'z'
                        || (str[i] == '.' || str[i] == '_')))
                {
                    sb.Append(str[i]);
                }
            }
            return sb.ToString();
        }


        public string ResimdenBase64e(Image file)
        {
            ImageConverter converter = new ImageConverter();
            byte[] raw = new byte[1];
            Bitmap test = new Bitmap(file);

            raw = (byte[])converter.ConvertTo(test, typeof(byte[]));

            string output = Convert.ToBase64String(raw);
            return output;
        }

        public string OcrSpaceileCoz(Image CaptchaResim, string engineMotor)
        {
            string sonuc = "bosluk";

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 1, 0);


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("K86617846688957"), "apikey"); //Added api key in form data
                //form.Add(new StringContent(getSelectedLanguage()), "language");
                form.Add(new StringContent("eng"), "language");

                form.Add(new StringContent(engineMotor), "ocrengine");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("true"), "istable");

                byte[] imageData = ImageToBase64(CaptchaResim, ImageFormat.Bmp);
                form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");

                HttpResponseMessage response = httpClient.PostAsync("https://api.ocr.space/Parse/Image", form).Result;

                string strContent = response.Content.ReadAsStringAsync().Result;

                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);


                if (ocrResult.OCRExitCode == 1)
                {
                    sonuc = ocrResult.ParsedResults[0].ParsedText;
                }
                else
                {
                    sonuc = "bosluk";
                }
            }
            catch
            {
                //MessageBox.Show(exception.Message);
                sonuc = "bosluk";
            }

            return sonuc;
        }





        public string AxaCaptchaCoz(Image EsasCaptcha)
        {

            string tb1, tb2, tb3, tb4, tb5, tb6, tb7, tb8, tb9, tb10, tb11, tb12, tb13, tb14, tb15, tb16, tb17;
            Bitmap pb1, pb2, pb3, pb4, pb5, pb6, pb7, pb8, pb9, pb10, pb11, pb12, pb13, pb14, pb15, pb16, pb17, pb18, pb19;
            string esasanahtar;
            bool axatekmi = false;
            List<string> keyList = new List<string>();
            List<string> karakterList = new List<string>();
            String[] files;

            pb1 = (Bitmap)EsasCaptcha;



            if (pb1.Width == 328) files = Directory.GetFiles(@"C:\CMSigorta\chrm\karakterler\");
            else
            {
                files = Directory.GetFiles(@"C:\CMSigorta\chrm\karakterler\tek");
                axatekmi = true;
            }

            ListBox1.Items.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                ListBox1.Items.Add(files[i]);
                ListBox2.Items.Add(Path.GetFileNameWithoutExtension(files[i]));
            }



            Bitmap img1;
            esasanahtar = "";
            karakterList.Clear();
            keyList.Clear();

            //önce yazıyı çözelim
            if (axatekmi)
            {
                pb10 = (Bitmap)cropImage(pb1, new Rectangle(0, 167, 368, 27));
                pb10 = new Bitmap(pb10, new Size(736, 54));
            }
            else
            {
                pb10 = (Bitmap)cropImage(pb1, new Rectangle(0, 167, 328, 27));
                pb10 = new Bitmap(pb10, new Size(736, 61));
            }


            //pb10 = Contrastit((Bitmap)pb10, 40);
            //pb10 = QuickRenkDegistir((Bitmap)pb10);
            //pb10 = RemoveNoise((Bitmap)pb10);
            //pb10 = CropWhiteSpace((Bitmap)pb10);
            //pb10 = DikeyYatayTekTemizle((Bitmap)pb10);
            //pb10 = DikeyYatayTekTemizle((Bitmap)pb10);
            //pb10 = DikeyYatayTekTemizle((Bitmap)pb10);
            //pb10 = (Bitmap)FixedSizeTo500(pb10);
            //pb10 = Contrastit((Bitmap)pb10, 40);
            //pb10 = Contrastit((Bitmap)pb10, 40);


            string karakteryazi = "";

            using (var ocrresim = OcrApi.Create())
            {
                //////////////////////////////////////////////////////////////////////////////
                string folderName = @"C:\CMSigorta\cptchaerror\";
                // If directory does not exist, create it
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                //pb10.Save(folderName + DateTime.Now.ToFileTime() + "_" + "axateksifre" + "_1.png", ImageFormat.Png);
                //////////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////////
                pb10.Save(folderName + DateTime.Now.ToFileTime() + "_" + RemoveSpecialCharacters(karakteryazi) + ".png", ImageFormat.Png);
                //////////////////////////////////////////////////////////////////////////////


                ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                karakteryazi = ocrresim.GetTextFromImage((Bitmap)pb10);
            }

            karakteryazi = karakteryazi.Replace(" ", "");
            karakteryazi = karakteryazi.Replace(".", ",");
            karakteryazi = karakteryazi.Replace(";", ",");
            karakteryazi = karakteryazi.Replace(",,", ",");
            karakteryazi = karakteryazi.Replace(",,", ",");
            karakteryazi = karakteryazi.Replace(",,", ",");
            karakteryazi = karakteryazi.Replace("\n", "").Replace(" ", "");
            karakteryazi = karakteryazi.Replace(",,", ",").Replace(",,", ",");

            if (karakteryazi.Substring(0, 1) == ",") { karakteryazi = karakteryazi.Substring(1, karakteryazi.Length - 1); };
            if (karakteryazi.Substring(karakteryazi.Length - 1, 1) == ",") { karakteryazi = karakteryazi.Substring(0, karakteryazi.Length - 1); };
            tb17 = karakteryazi;

            //Trayyy1.ShowBalloonTip(1000, "Axa Anahtar Cümle Tespit edildi", tb17, ToolTipIcon.Info);


            //Trayyy1.ShowBalloonTip(1000, "Axa Anahtar Cümle Tespit edildi", tb17, ToolTipIcon.Info);
            gn.LocalLoglaAsync("AXA", "---", tb17);
            List<string> anahtarLists = tb17.Split(',').ToList();


            //sonra resimleri çözelim tek tek
            if (axatekmi) pb2 = (Bitmap)cropImage(pb1, new Rectangle(3, 3, 83, 73));
            else pb2 = (Bitmap)cropImage(pb1, new Rectangle(3, 3, 73, 73));
            tb2 = BulBakalim(pb2);
            karakterList.Add(tb2);
            pb12 = (Bitmap)cropImage(pb2, new Rectangle(4, 2, 14, 13));
            pb12.SetPixel(13, 11, Color.White);
            pb12.SetPixel(13, 12, Color.White);
            pb12.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb12);
                //karakter = karakter.Replace("l", "").Replace("|", "");
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb12);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb12);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb12);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb1 = karakter;
                keyList.Add(tb1);
            }
            if (axatekmi) pb3 = (Bitmap)cropImage(pb1, new Rectangle(95, 3, 83, 73));
            else pb3 = (Bitmap)cropImage(pb1, new Rectangle(85, 3, 73, 73));
            tb3 = BulBakalim(pb3);
            karakterList.Add(tb3);
            pb13 = (Bitmap)cropImage(pb3, new Rectangle(4, 2, 14, 13));
            pb13.SetPixel(13, 11, Color.White);
            pb13.SetPixel(13, 12, Color.White);
            pb13.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb13);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb13);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb13);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb13);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb4 = karakter;
                keyList.Add(tb4);
            }
            if (axatekmi) pb4 = (Bitmap)cropImage(pb1, new Rectangle(187, 3, 83, 73));
            else pb4 = (Bitmap)cropImage(pb1, new Rectangle(167, 3, 73, 73));
            tb5 = BulBakalim(pb4);
            karakterList.Add(tb5);
            pb14 = (Bitmap)cropImage(pb4, new Rectangle(4, 2, 14, 13));
            pb14.SetPixel(13, 11, Color.White);
            pb14.SetPixel(13, 12, Color.White);
            pb14.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb14);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb14);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb14);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb14);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb6 = karakter;
                keyList.Add(tb6);
            }
            if (axatekmi) pb5 = (Bitmap)cropImage(pb1, new Rectangle(279, 3, 83, 73));
            else pb5 = (Bitmap)cropImage(pb1, new Rectangle(249, 3, 73, 73));
            tb7 = BulBakalim(pb5);
            karakterList.Add(tb7);
            pb15 = (Bitmap)cropImage(pb5, new Rectangle(4, 2, 14, 13));
            pb15.SetPixel(13, 11, Color.White);
            pb15.SetPixel(13, 12, Color.White);
            pb15.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb15);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb15);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb15);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb15);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb8 = karakter;
                keyList.Add(tb8);
            }
            if (axatekmi) pb6 = (Bitmap)cropImage(pb1, new Rectangle(3, 85, 83, 73));
            else pb6 = (Bitmap)cropImage(pb1, new Rectangle(3, 85, 73, 73));
            tb10 = BulBakalim(pb6);
            karakterList.Add(tb10);
            pb16 = (Bitmap)cropImage(pb6, new Rectangle(4, 2, 14, 13));
            pb16.SetPixel(13, 11, Color.White);
            pb16.SetPixel(13, 12, Color.White);
            pb16.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb16);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb16);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb16);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb16);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb9 = karakter;
                keyList.Add(tb9);
            }
            if (axatekmi) pb7 = (Bitmap)cropImage(pb1, new Rectangle(95, 85, 83, 73));
            else pb7 = (Bitmap)cropImage(pb1, new Rectangle(85, 85, 73, 73));
            tb12 = BulBakalim(pb7);
            karakterList.Add(tb12);
            pb17 = (Bitmap)cropImage(pb7, new Rectangle(4, 2, 14, 13));
            pb17.SetPixel(13, 11, Color.White);
            pb17.SetPixel(13, 12, Color.White);
            pb17.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb17);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb17);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb17);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb17);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb11 = karakter;
                keyList.Add(tb11);
            }
            if (axatekmi) pb8 = (Bitmap)cropImage(pb1, new Rectangle(187, 85, 83, 73));
            else pb8 = (Bitmap)cropImage(pb1, new Rectangle(167, 85, 73, 73));
            tb14 = BulBakalim(pb8);
            karakterList.Add(tb14);
            pb18 = (Bitmap)cropImage(pb8, new Rectangle(4, 2, 14, 13));
            pb18.SetPixel(13, 11, Color.White);
            pb18.SetPixel(13, 12, Color.White);
            pb18.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb18);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb18);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb18);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb18);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb13 = karakter;
                keyList.Add(tb13);
            }
            if (axatekmi) pb9 = (Bitmap)cropImage(pb1, new Rectangle(279, 85, 83, 73));
            else pb9 = (Bitmap)cropImage(pb1, new Rectangle(249, 85, 73, 73));
            tb16 = BulBakalim(pb9);
            karakterList.Add(tb16);
            pb19 = (Bitmap)cropImage(pb9, new Rectangle(4, 2, 14, 13));
            pb19.SetPixel(13, 11, Color.White);
            pb19.SetPixel(13, 12, Color.White);
            pb19.SetPixel(13, 0, Color.White);
            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.English);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakter = ocrresim.GetTextFromImage((Bitmap)pb19);
                karakter = karakter.ToUpper();
                karakter = karakter.Replace("\n", "");
                if (karakter.Length > 1) { karakter = karakter.Substring(1, 1); };
                if (karakter == "S")
                {
                    img1 = new Bitmap(pb19);
                    if (img1.GetPixel(3, 7).ToString() == "Color [A=255, R=0, G=116, B=255]")
                    {
                        karakter = "6";
                    }
                }
                if (karakter == "V")
                {
                    img1 = new Bitmap(pb19);
                    if (img1.GetPixel(7, 5).ToString() == "Color [A=255, R=72, G=156, B=255]")
                    {
                        karakter = "Y";
                    }
                }
                if (karakter == "A")
                {
                    img1 = new Bitmap(pb19);
                    if (img1.GetPixel(7, 10).ToString() != "Color [A=255, R=255, G=255, B=255]")
                    {
                        karakter = "3";
                    }
                }
                tb15 = karakter;
                keyList.Add(tb15);
            }


            foreach (string anahtarList in anahtarLists)
            {
                string duzeltilmisanahtar = FindBestMatch(anahtarList, ListBox2.Items.Cast<String>().ToList());
                esasanahtar += keyList[karakterList.IndexOf(duzeltilmisanahtar)];
                Console.WriteLine(esasanahtar);
            }

            if (anahtarLists.Count == esasanahtar.Length) return esasanahtar;
            else return "BOSLUK";

        }


        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }


        public static string FindBestMatch(string stringToCompare, IEnumerable<string> strs)
        {

            //HashSet<string> strCompareHash = stringToCompare.Split(' ').ToHashSet();

            int maxIntersectCount = 100;
            string bestMatch = string.Empty;

            foreach (string str in strs)
            {
                //HashSet<string> strHash = str.Split(' ').ToHashSet();

                //int intersectCount = stringToCompare.Intersect(stringToCompare).Count();
                int intersectCount = Compute(str, stringToCompare);
                //MessageBox.Show(intersectCount.ToString());

                if (intersectCount < maxIntersectCount)
                {
                    maxIntersectCount = intersectCount;
                    bestMatch = str;
                }
            }

            return bestMatch;
        }


        [Obsolete]
        private void guna2PictureBox1_MouseDoubleClick(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.Visible = true;
            sagtikmenusu.Items.Remove(toolStripSeparator1);
            //foreach (ToolStripMenuItem item in sagtikmenusu.Items)
            //{
            //    if ((string)item.Tag != "Çıkış")
            //    {
            //        item.Click -= TraysirketButon_Click;
            //        sagtikmenusu.Items.Remove(item);
            //    }
            //}
            sagtikmenusu.Items.Add(toolStripSeparator1);
            sirketekle();
        }

        private void sagtikmenusu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void sMSŞifrelerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SMSShowerForm frmsms = new SMSShowerForm();
            if (frmsms.IsAccessible)
            {
                frmsms.BringToFront();
            }
            else
            {
                frmsms.Show();
            }

        }



        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            if (ekranno == 1)
            {
                ekranno = 0;
                ScreenButtonX.Text = ekranno.ToString();
            }
            else if (ekranno == 0 && screens.Count() > 1)
            {
                ekranno = 1;
                ScreenButtonX.Text = ekranno.ToString();
            }

            this.Size = screens[ekranno].WorkingArea.Size;

            GorunumDegistir();
            setFormLocation(this, screens[ekranno]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (ekranno == 0)
            {
                ekranno = ekranno + 1;
                ScreenButtonX.Text = ekranno.ToString();
            }
            else if (ekranno == 1 && screens.Count() > 2)
            {
                ekranno = ekranno + 1;
                ScreenButtonX.Text = ekranno.ToString();
            }
            else
            {
                ekranno = 0;
                ScreenButtonX.Text = ekranno.ToString();
            }

            this.Size = screens[ekranno].WorkingArea.Size;

            GorunumDegistir();
            setFormLocation(this, screens[ekranno]);

            if (screens[ekranno].Bounds.Height < 960)
            {
                tableLayoutPanel1.Padding = new Padding(10, 0, 0, 0);
            }


            var columnCount = tableLayoutPanel1.Width / (screens[ekranno].Bounds.Width / 11);
            //var rowCount = tableLayoutPanel1.Height / 180;
            var rowCount = tableLayoutPanel1.Height / (screens[ekranno].Bounds.Height / 6);

            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                tableLayoutPanel1.Controls[i].Width = (tableLayoutPanel1.Width / columnCount) - (columnCount + 3);
                tableLayoutPanel1.Controls[i].Height = (tableLayoutPanel1.Height / rowCount) - (rowCount + 3);
                //tableLayoutPanel1.Controls[i].ImageSize = new Size((screens[ekranno].Bounds.Width / 14), (Screen.PrimaryScreen.Bounds.Height / 10));
                Application.DoEvents();
            }

            this.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            YeniSirketeGitFonk(sirketadi);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Console.Beep(800, 200);
            //Console.Beep(800, 200);
            //Console.Beep(800, 200);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String ankaracaptchasonuc = "";
            IWebElement ankaracaptcharesim = chrm_driver.FindElement(By.XPath("//img[@class='captcha-image']"));
            pictureBox11.Image = GetElementScreenShot(chrm_driver, ankaracaptcharesim);
            pictureBox11.Refresh();
            //string base64bu;
            //base64bu = cptch.ResimdenBase64e(img);
            Captcha cptch = new Captcha();
            //Image captchresim;

            pictureBox11.Image = AnkaraRenkDegistir((Bitmap)pictureBox11.Image);
            pictureBox11.Refresh();
            Application.DoEvents();
            pictureBox11.Image = AnkaraTemizle((Bitmap)pictureBox11.Image);
            pictureBox11.Refresh();
            Application.DoEvents();

            pictureBox11.Image = FixedSizeTo500((Bitmap)pictureBox11.Image);
            pictureBox11.Refresh();
            Application.DoEvents();



            pictureBox11.Image = AnkaraDikey2liTemizle((Bitmap)pictureBox11.Image);
            pictureBox11.Refresh();
            Application.DoEvents();


            //captchresim = FixedSizeTo500(captchresim);

            //ankaracaptchasonuc = cptch.NeovaAritmetik(cptch.NeovaOku(captchresim));
            //ankaracaptchasonuc = cptch.AnkaraOku((Bitmap)captchresim);

            using (var ocrresim = OcrApi.Create())
            {
                ocrresim.Init(Patagames.Ocr.Enums.Languages.Turkish);
                ocrresim.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string karakterx = ocrresim.GetTextFromImage((Bitmap)pictureBox11.Image);
                ankaracaptchasonuc = karakterx;
            }

            if (ankaracaptchasonuc != "")
            {
                if (ankaracaptchasonuc.Contains("HATA"))
                {
                    //goto Etiket_Ankara_LoginDeneme;
                }
                else
                {
                    //Trayyy1.ShowBalloonTip(100, "NEOVA SİGORTA", "Neova Sigorta Captcha çözüldü. " + neovacaptchasonuc, ToolTipIcon.Error);
                    chrm_driver.FindElement(By.XPath("//input[@id='Captcha' and @class='form-control']")).Clear();
                    chrm_driver.FindElement(By.XPath("//input[@id='Captcha' and @class='form-control']")).SendKeys(ankaracaptchasonuc);
                }
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Console.Beep(800, 200);
            //Console.Beep(800, 200);
            //Console.Beep(800, 200);
        }

        private void sirketButon_SizeChanged(object sender, EventArgs e)
        {
            //Console.Beep();
            Guna2TileButton item = (sender as Guna2TileButton);
            item.ImageSize = new Size((screens[ekranno].Bounds.Width / 14), (Screen.PrimaryScreen.Bounds.Height / 10));
        }
    }
}