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
    public partial class Historia : Form
    {
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Przemek\Desktop\repozytorium\WokItEasy\WokItEasy1.mdb";
        //static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\6\Zespołowe programowanie\WokItEasy\WokItEasy\WokItEasy1.mdb";
        private void Szczegółowo(string IDSzczegółu)
        {
            string connString = source;
            OleDbConnection connection = new OleDbConnection(connString);
            connection.Open();
            string query = "SELECT * FROM Zamówienia";
            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
            DataSet data = new DataSet();
            AdapterTabela.Fill(data, "Zamówienia");
            int a = Int16.Parse(IDSzczegółu);
            a--;
            //wartosc = data.Tables["Zamówienia"].Rows[a][2].ToString();
            //Zamówienie zamówienie = new Zamówienie();
            //zamówienie.IdZamówienia =Int16.Parse(data.Tables["Zamówienia"].Rows[a][0].ToString());
            //składnik.RodzajSM = wartosc;
            //składnik.NazwaSM = data.Tables["Zamówienia"].Rows[a][1].ToString();
            //wartosc = data.Tables["Zamówienia"].Rows[a][0].ToString();
            //składnik.IdSM = Int16.Parse(wartosc);
            //wartosc = data.Tables["Zamówienia"].Rows[a][3].ToString();
            //składnik.CenaSM = Int16.Parse(wartosc);
            //wartosc = data.Tables["Zamówienia"].Rows[a][4].ToString();
            //składnik.DataDodaniaSM = DateTime.Parse(wartosc);
            label5.Text = IDSzczegółu;
            label6.Text = data.Tables["Zamówienia"].Rows[a][1].ToString();
            label7.Text = data.Tables["Zamówienia"].Rows[a][2].ToString();
            textBox1.Text = data.Tables["Zamówienia"].Rows[a][3].ToString();

            connection.Close();
        }
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
            for (int a = 0; a < data.Tables["Zamówienia"].Rows.Count; a++)
            {
                //wartosc = data.Tables["Zamówienia"].Rows[a][2].ToString();
                //Zamówienie zamówienie = new Zamówienie();
                //zamówienie.IdZamówienia =Int16.Parse(data.Tables["Zamówienia"].Rows[a][0].ToString());
                //składnik.RodzajSM = wartosc;
                //składnik.NazwaSM = data.Tables["Zamówienia"].Rows[a][1].ToString();
                //wartosc = data.Tables["Zamówienia"].Rows[a][0].ToString();
                //składnik.IdSM = Int16.Parse(wartosc);
                //wartosc = data.Tables["Zamówienia"].Rows[a][3].ToString();
                //składnik.CenaSM = Int16.Parse(wartosc);
                //wartosc = data.Tables["Zamówienia"].Rows[a][4].ToString();
                //składnik.DataDodaniaSM = DateTime.Parse(wartosc);

                listBox1.Items.Add(data.Tables["Zamówienia"].Rows[a][0].ToString() + " " + data.Tables["Zamówienia"].Rows[a][1].ToString());
            }
            connection.Close();

        }
        public Historia()
        {
            InitializeComponent();
            Pokaż();
        }

        private void Historia_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //wybór któe chcemy zobaczyć szczegółowo
            string a = ZbudujID(listBox1.SelectedItem.ToString());
            Szczegółowo(a);
        }
        string ZbudujID(string a)
        {
            string b="";
            int x = 0;
            do
            {
                b+=a[x];
                x++;
            } while (a[x] != ' ');
            return b;
        }
       
    }
}
