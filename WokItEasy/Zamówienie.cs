using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace WokItEasy
{
    class Zamówienie
    {
        int idZamówienia, idObsługi;
        double kwotaZamówienia;
        DateTime dataZamówienia;
        bool online;
        bool rozliczone;
        string idZamówień;
        //static int maxIDZamówienia;
        //static bool maxIDZamówieniaZrobione = false;
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
        public int IdZamówienia { get => idZamówienia; set => idZamówienia = value; }
        public double KwotaZamówienia { get => kwotaZamówienia; set => kwotaZamówienia = value; }
        public int IdObsługi { get => idObsługi; set => idObsługi = value; }
        public DateTime DataZamówienia { get => dataZamówienia; set => dataZamówienia = value; }
        public bool Online { get => online; set => online = value; }
        public string IdZamówień { get => idZamówień; set => idZamówień = value; }
        public bool Rozliczone { get => rozliczone; set => rozliczone = value; }

        private static List<Zamówienie> listObecneZamówienia = new List<Zamówienie>();
        private static List<Zamówienie> listObecneZamówieniaNaKuchni = new List<Zamówienie>();
        private static void BuildObecneZamówienia()
        {
            try
            {
                listObecneZamówienia = new List<Zamówienie>();
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM Zamówienia WHERE Wykonane = false;";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Zamówienia");
                string wartosc;
                for (int a = 0; a < data.Tables["Zamówienia"].Rows.Count; a++)
                {
                    wartosc = data.Tables["Zamówienia"].Rows[a][0].ToString();
                    Zamówienie zamówienie = new Zamówienie();
                    zamówienie.IdZamówienia = Convert.ToInt32(wartosc);
                    wartosc = data.Tables["Zamówienia"].Rows[a][1].ToString();
                    zamówienie.DataZamówienia = DateTime.Parse(wartosc);
                    wartosc = data.Tables["Zamówienia"].Rows[a][3].ToString();
                    zamówienie.IdZamówień = wartosc;
                    listObecneZamówienia.Add(zamówienie);
                }
                connection.Close();
                // return ObecneZamówienia;
            }
            catch
            {
                // null;
            }


        }
        public static List<Zamówienie> GetObecneZamówienia(bool kuchnia)
        {
            if (kuchnia)
            {
                BuildObecneZamówieniaNaKuchni();
                return listObecneZamówieniaNaKuchni;
            }
            else
                BuildObecneZamówienia();

            return listObecneZamówienia;
        }
        private static void BuildObecneZamówieniaNaKuchni()
        {
            try
            {
                listObecneZamówieniaNaKuchni = new List<Zamówienie>();
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM Zamówienia WHERE WykonaneKuchnia = false;";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Zamówienia");
                string wartosc;
                for (int a = 0; a < data.Tables["Zamówienia"].Rows.Count; a++)
                {
                    wartosc = data.Tables["Zamówienia"].Rows[a][0].ToString();
                    Zamówienie zamówienie = new Zamówienie();
                    zamówienie.IdZamówienia = Convert.ToInt32(wartosc);
                    wartosc = data.Tables["Zamówienia"].Rows[a][1].ToString();
                    zamówienie.DataZamówienia = DateTime.Parse(wartosc);
                    wartosc = data.Tables["Zamówienia"].Rows[a][3].ToString();
                    zamówienie.IdZamówień = wartosc;
                    listObecneZamówieniaNaKuchni.Add(zamówienie);
                }
                connection.Close();
                // return ObecneZamówienia;
            }
            catch
            {
                // null;
            }


        }
        public static void WykonajZamówienie(int id,bool kuchnia)
        {
            if (!kuchnia)
            {
                string connectionString = source;
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();
                string query = "UPDATE Zamówienia SET Wykonane = true WHERE Identyfikator = " + id.ToString() + ";";
                System.Diagnostics.Debug.WriteLine(query);
                OleDbCommand comm = new OleDbCommand(query, conn);
                OleDbDataAdapter AdapterTab = new OleDbDataAdapter(comm);
                DataSet data1 = new DataSet();
                AdapterTab.Fill(data1, "Zamówienia");
                conn.Close();
                System.Diagnostics.Debug.WriteLine("Zrealizowano zamówienie o id " + id);
            }
            else
            {
                string connectionString = source;
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();
                string query = "UPDATE Zamówienia SET WykonaneKuchnia = true WHERE Identyfikator = " + id.ToString() + ";";
                System.Diagnostics.Debug.WriteLine(query);
                OleDbCommand comm = new OleDbCommand(query, conn);
                OleDbDataAdapter AdapterTab = new OleDbDataAdapter(comm);
                DataSet data1 = new DataSet();
                AdapterTab.Fill(data1, "Zamówienia");
                conn.Close();
                System.Diagnostics.Debug.WriteLine("Zrealizowano na kuchni zamówienie o id " + id);

            }
        }
        public static void DopiszZamowienie(double cena, string ids,string source,int idObslugi,bool online=true,bool rozliczone=false)
        {
            string connectionString = source;
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();
            string query1 = "INSERT INTO Zamówienia (DataZamówienia, KwotaZamówienia, IDSM, IDObsługi, Online, Rozliczone, Wykonane, WykonaneKuchnia) VALUES('";
            query1 += DateTime.Now.ToString();
            query1 += "', '";
            query1 += cena;
            query1 += "', '";
            query1 += ids;
            query1 += "', ";
            query1 += Convert.ToString(idObslugi);// tu trzeba bedzie dać ID pracownika z konta klienta który przesłał zamówienie
            query1 += ", ";
            query1 += online;
            query1 += ", ";
            query1 += rozliczone;
            query1 += ", false, false);";
            System.Diagnostics.Debug.WriteLine(query1);
            OleDbCommand comm = new OleDbCommand(query1, conn);
            OleDbDataAdapter AdapterTab = new OleDbDataAdapter(comm);
            DataSet data1 = new DataSet();
            AdapterTab.Fill(data1, "Zamówienia");
            conn.Close();
            System.Diagnostics.Debug.WriteLine("Dodano");

        }
        public static void DopiszZamowieniaZListyNazw(List<string> ids, double cena, int idObslugi, string source, bool online = true, bool rozliczone = false)
        {
            string str = "";
            for(int s=0;s<ids.Count;s++)
            {
                str += ids[s] + ", ";
            }
            str.TrimEnd(',');
            DopiszZamowienie(cena, str, source, idObslugi, online, rozliczone);
        }
        public static void DopiszZamowieniaZListyID(List<int> ids,double cena, int idObslugi, string source, bool online = true, bool rozliczone = false)
        {
            DopiszZamowienie(cena,idArr(ids),source,idObslugi,online,rozliczone);

        }
        private static string idArr(List<int> ids)
        {
            string str = "";
            for (int s= 0;s<ids.Count;s++)
            {
                str += ids[s].ToString();
                if (s != ids.Count - 1)
                {
                    str += ", ";
                }

            }
            return str;
        }
        public static void DopiszZamowieniaZListyID(List<SkładnikMenu> listaSM, List<string> lb, double cena, string source, int idObslugi, bool online = true, bool rozliczone = false)
        {
           
            List<int> idsInt = new List<int>();
            foreach (string s in lb)
            {
                idsInt.Add(listaSM.Find(x => x.NazwaSM.Equals(s)).IdSM);
            }
            DopiszZamowieniaZListyID(idsInt, cena, idObslugi, source,online,rozliczone);
        }
    }
}
