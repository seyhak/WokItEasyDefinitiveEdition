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
        int idZamówienia, kwotaZamówienia, idObsługi;
        DateTime dataZamówienia;
        bool online;
        string idZamówień;

        public int IdZamówienia { get => idZamówienia; set => idZamówienia = value; }
        public int KwotaZamówienia { get => kwotaZamówienia; set => kwotaZamówienia = value; }
        public int IdObsługi { get => idObsługi; set => idObsługi = value; }
        public DateTime DataZamówienia { get => dataZamówienia; set => dataZamówienia = value; }
        public bool Online { get => online; set => online = value; }
        public string IdZamówień { get => idZamówień; set => idZamówień = value; }

        public static void DopiszZamowienie(double cena, int[] id,string source,int idObslugi)
        {
            string connectionString = source;
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();
            string query1 = "INSERT INTO Zamówienia (DataZamówienia, KwotaZamówienia, IDSM, IDObsługi, Online, Rozliczone) VALUES('";
            query1 += DateTime.Now.ToString();
            query1 += "', '";
            query1 += cena;
            query1 += "', '";
            query1 += id.ToString();
            query1 += "', ";
            query1 += Convert.ToString(id);// tu trzeba bedzie dać ID pracownika z konta klienta który przesłał zamówienie
            query1 += ", ";
            query1 += "True";
            query1 += ", ";
            query1 += "False);";
            OleDbCommand comm = new OleDbCommand(query1, conn);
            OleDbDataAdapter AdapterTab = new OleDbDataAdapter(comm);
            DataSet data1 = new DataSet();
            AdapterTab.Fill(data1, "Zamówienia");
            conn.Close();

        }
        
        public static void DopiszZamowieniaZListyID(List<int> ids,double cena, int idObslugi, string source)
        {
            DopiszZamowienie(cena,ids.ToArray(),source,idObslugi);

        }
    }
}
