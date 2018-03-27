using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace WokItEasy
{
    class SkładnikMenu
    {
        private int idSM;
        private string nazwaSM;
        private string rodzajSM;
        private double cenaSM;
        private DateTime dataDodaniaSM;

        static string zwrocKategorie(string a,string source)
        {
            OleDbConnection connection = new OleDbConnection(source);
            connection.Open();
            string query = "SELECT NazwaKategorii FROM Kategoria WHERE Identyfikator = " + a + ";";

            OleDbCommand command1 = new OleDbCommand(query, connection);
            OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
            DataSet data = new DataSet();
            AdapterTabela1.Fill(data, "Kategoria");
            a = data.Tables["Kategoria"].Rows[0][0].ToString();
            connection.Close();
            return a;
        }
        public static List<SkładnikMenu> Zbuduj(string source)
        {
            try
            {
                List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM SkładnikMenu";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "SkładnikMenu");
                string wartosc;
                for (int a = 0; a < data.Tables["SkładnikMenu"].Rows.Count; a++)
                {
                    wartosc = data.Tables["SkładnikMenu"].Rows[a][2].ToString();
                    SkładnikMenu składnik = new SkładnikMenu();
                    wartosc = zwrocKategorie(Convert.ToInt32(wartosc).ToString(),source);
                    składnik.RodzajSM = wartosc;
                    składnik.NazwaSM = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
                    wartosc = data.Tables["SkładnikMenu"].Rows[a][0].ToString();
                    składnik.IdSM = Int16.Parse(wartosc);
                    wartosc = data.Tables["SkładnikMenu"].Rows[a][3].ToString();
                    składnik.CenaSM = Double.Parse(wartosc);
                    wartosc = data.Tables["SkładnikMenu"].Rows[a][4].ToString();
                    składnik.DataDodaniaSM = DateTime.Parse(wartosc);
                    listaSM.Add(składnik);


                }
                connection.Close();
                return listaSM;
            }
            catch
            {
                return null;
            }
        }
        public DateTime DataDodaniaSM { get => dataDodaniaSM; set => dataDodaniaSM = value; }
        public double CenaSM { get => cenaSM; set => cenaSM = value; }
        public string NazwaSM { get => nazwaSM; set => nazwaSM = value; }
        public int IdSM { get => idSM; set => idSM = value; }
        public string RodzajSM { get => rodzajSM; set => rodzajSM = value; }
        public string getAlmostXML()
        {
            string str = "";
            str += IdSM.ToString();
            str += "#";
            str += NazwaSM.ToString();
            str += "#";
            str += RodzajSM.ToString();
            str += "#";
            str += CenaSM.ToString();
            str += "#";
            str += dataDodaniaSM.ToString();
            return str;
        }
    }
}
