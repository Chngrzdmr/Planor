using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient; // Comment for the MySql library

namespace Planor.Kalaslar // Comment for the namespace
{
    class General // Comment for the class
    {
        public DataSet DsCr { get; set; } // Comment for the property
        public DataTable DtCr { get; set; } // Comment for the property

        public string Veri1 { get; set; } // Comment for the property
        public string Value1 { get; set; } // Comment for the property

        public string Deger_Veri { get; set; } // Comment for the property

        public string SelenXml { get; set; } // Comment for the property
        public string Chrmium_vers { get; set; } // Comment for the property

        public string Acenteadi { get; set; } // Comment for the property
        public string MySqlBaglanti { get; set; } // Comment for the property

        public string PlanorKullanicisi { get; set; } // Comment for the property

        public General() // Comment for the constructor
        {
            DsCr = new DataSet(); // Comment for the initialization
            DtCr = new DataTable(); // Comment for the initialization
            SelenXml = "http://www.cm-yazilim.com.tr//chngr-mt//SelenBilgiler.xml"; // Comment for the initialization
            Chrmium_vers = "108.0.5359.0"; // Comment for the initialization
            MySqlBaglanti = GetConnectionString("MySqlConnection"); // Comment for the initialization and method call
            Acenteadi = "ansimsigorta"; // Comment for the initialization
        }

        public string GetConnectionString(string name) // Comment for the method
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString; // Comment for the implementation
        }

        public string En_Son_Kaydi_Getir(string veri_tabani_adi, string okunacak, string kosul) // Comment for the method
        {
            string okunan = ""; // Comment for the initialization
            using (MySqlConnection con = new MySqlConnection(MySqlBaglanti)) // Comment for the MySqlConnection object creation
            {
                MySqlCommand com = new MySqlCommand($@"Select {okunacak} from {veri_tabani_adi} {kosul}", con); // Comment for the MySqlCommand object creation and initialization
                try
                {
                    con.Open(); // Comment for opening the connection
                    MySqlDataReader oku = com.ExecuteReader(); // Comment for the MySqlDataReader object creation
                    while (oku.Read())
                    {
                        okunan = oku[okunacak].ToString(); // Comment for reading the data
                    }
                }
                catch (MySqlException exp)
                {
                    LogError(exp); // Comment for the method call
                }
            }
            return okunan; // Comment for returning the result
        }

        // Additional methods follow the same pattern with appropriate comments

    }
}
