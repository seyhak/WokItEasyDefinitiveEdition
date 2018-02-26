using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;//access

namespace WokItEasy
{
    public partial class Rozliczenie : Form
    {
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Przemek\Desktop\repozytorium\WokItEasy\WokItEasy1.mdb";
        //static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\6\Zespołowe programowanie\WokItEasy\WokItEasy\WokItEasy1.mdb";
        private List<int> listaNierozliczonych = new List<int>();
        void Pokaż()
        {
            string connString = source;
            OleDbConnection connection = new OleDbConnection(connString);
            connection.Open();
            string query = "SELECT * FROM Zamówienia";
            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
            DataSet data = new DataSet();
            AdapterTabela.Fill(data, "Zamówienia");
            string wartosc;
            for (int a = 0; a < data.Tables["Zamówienia"].Rows.Count; a++)
            {
                wartosc = data.Tables["Zamówienia"].Rows[a]["Rozliczone"].ToString();
                if (wartosc == "False")
                {
                    wartosc = data.Tables["Zamówienia"].Rows[a]["Online"].ToString();
                    string temp;
                    double tempoInt;
                    switch (wartosc)
                    {
                        case "True":
                            temp = label5.Text;
                            tempoInt = double.Parse(temp) + double.Parse(data.Tables["Zamówienia"].Rows[a]["KwotaZamówienia"].ToString());
                            label5.Text = tempoInt.ToString();
                            break;
                        case "False":
                            temp = label6.Text;
                            tempoInt = double.Parse(temp) + double.Parse(data.Tables["Zamówienia"].Rows[a]["KwotaZamówienia"].ToString());
                            label6.Text = tempoInt.ToString();
                            break;
                    }
                }
            }
            double tempo;
            tempo = Double.Parse(label6.Text) + Double.Parse(label7.Text);
            label7.Text = tempo.ToString();
            connection.Close();
        }
        public Rozliczenie()
        {
            InitializeComponent();
            Pokaż();
        }

        private void Rozliczenie_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void Rozlicz_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Rozliczono dnia: " + DateTime.Now.ToString() + ". Całkowity obrót wynosił: " + label7.Text);
            label5.Text = label6.Text = label7.Text = "0";

            string connString = source;
            OleDbConnection connection = new OleDbConnection(connString);
            connection.Open();
            for (int a = 0; a < listaNierozliczonych.Count; a++)
            {
                string query1 = "UPDATE Zamówienia SET Rozliczone = True";
                OleDbCommand command = new OleDbCommand(query1, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Zamówienia");//ustawia co należy
            }
        }
    }
}
