using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace WokItEasy
{
    class Kategoria
    {
        int IDKategori;
        string nazwaKat;
        bool doKuchni;
        private static List<Kategoria> listaKategorii;
        public int IDKat { get => IDKategori; set => IDKategori = value; }
        public string NazwaKat { get => nazwaKat; set => nazwaKat = value; }
        public bool DoKuchni { get => doKuchni; set => doKuchni = value; }

        public static List<Kategoria> Zbuduj(string sourc)
        {
            try
            {
                List<Kategoria> listaSM = new List<Kategoria>();
                string connString = sourc;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM Kategoria";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Kategoria");
                string wartosc;
                for (int a = 0; a < data.Tables["Kategoria"].Rows.Count; a++)
                {
                    Kategoria kategoria = new Kategoria();
                    wartosc = data.Tables["Kategoria"].Rows[a][0].ToString();
                    kategoria.IDKat = Int16.Parse(wartosc);
                    wartosc = data.Tables["Kategoria"].Rows[a][1].ToString();
                    kategoria.NazwaKat = wartosc;
                    wartosc = data.Tables["Kategoria"].Rows[a][3].ToString();
                    kategoria.doKuchni = Convert.ToBoolean(wartosc);
                    listaSM.Add(kategoria);
                }
                connection.Close();
                listaKategorii = listaSM;
                return listaSM;
            }
            catch
            {
                return null;
            }
        }
        public static void ZbudujStatycznąListęKategorii(string sourc)
        {
            listaKategorii = new List<Kategoria>();
            try
            {
                string connString = sourc;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM Kategoria";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Kategoria");
                string wartosc;
                for (int a = 0; a < data.Tables["Kategoria"].Rows.Count; a++)
                {
                    Kategoria kategoria = new Kategoria();
                    wartosc = data.Tables["Kategoria"].Rows[a][0].ToString();
                    kategoria.IDKat = Int16.Parse(wartosc);
                    wartosc = data.Tables["Kategoria"].Rows[a][1].ToString();
                    kategoria.NazwaKat = wartosc;
                    wartosc = data.Tables["Kategoria"].Rows[a][3].ToString();
                    kategoria.doKuchni = Convert.ToBoolean(wartosc);
                    listaKategorii.Add(kategoria);
                }
                connection.Close();
            }
            catch
            {
            }
        }
        public static bool CzyNależyDoKuchni(string nazwaKategorii)
        {
            foreach(Kategoria k in Kategoria.listaKategorii)
            {
                if (k.NazwaKat == nazwaKategorii)
                {
                    return k.DoKuchni;

                }

            }
            return false;
        }
    }
}
