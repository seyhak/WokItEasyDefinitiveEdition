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
    
    public partial class Menu : Form
    {
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
        string zwrocKategorie(string a)
        {
            OleDbConnection connection = new OleDbConnection(source);
            connection.Open();
            string query = "SELECT NazwaKategorii FROM Kategoria WHERE Identyfikator = " + a + ";";

            OleDbCommand command1 = new OleDbCommand(query, connection);
            OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
            DataSet data = new DataSet();
            AdapterTabela1.Fill(data, "Kategoria");
            a= data.Tables["Kategoria"].Rows[0][0].ToString();
            connection.Close();
            return a;
        }
        public void Pokaż()
        {
            try
            {

                listBox1.Items.Clear();
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM SkładnikMenu";
                //string query1 = "SELECT * FROM Kategoria";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "SkładnikMenu");
                string wartosc;
                List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
                for (int a = 0; a < data.Tables["SkładnikMenu"].Rows.Count; a++)
                {
                    wartosc = data.Tables["SkładnikMenu"].Rows[a][2].ToString();
                    SkładnikMenu składnik = new SkładnikMenu();
                    wartosc = zwrocKategorie(Convert.ToInt32(wartosc).ToString());
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

                listaSM = listaSM.OrderBy(o => o.RodzajSM).ToList();
                foreach (SkładnikMenu składnik in listaSM)
                {
                    string dots = Dots(składnik.NazwaSM + " (" + składnik.RodzajSM + ")", 70);
                    listBox1.Items.Add(składnik.NazwaSM + " (" + składnik.RodzajSM + ")" + dots + składnik.CenaSM.ToString());

                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Błąd odczytywania menu");

            }
        }
        string Dots(string w,int poIluZnakachCena)
        {
            int b = poIluZnakachCena - w.Length;
            w = "";
            for(int a = 0; a < b; a++)
            {
                w += ". ";

            }
            return w;
        }
        public Menu()
        {
            InitializeComponent();
            if (Form1.ObecnieZalogowanyUżytkownik.Kierownik)
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
            }
            Pokaż();
        }

        private void Menu_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dodaj danie
            Dodaj dodaj = new Dodaj();
            dodaj.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //usun danie
            try
            {


                Object sm = listBox1.SelectedItem;
                string what = sm.ToString();//nazwa objektu zaznaczonego w listboxie
                string whatWhat = "";//nazwa obiektu z DB
                for (int a = 0; a < what.Length; a++)
                {
                    if (what[a] == ' ')
                    {
                        break;
                    }
                    else
                        whatWhat += what[a];
                }
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM SkładnikMenu";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "SkładnikMenu");

                query = "DELETE FROM SkładnikMenu WHERE NazwaSM = '" + whatWhat + "';";
                command = new OleDbCommand(query, connection);
                AdapterTabela = new OleDbDataAdapter(command);
                data = new DataSet();
                AdapterTabela.Fill(data, "SkładnikMenu");

                connection.Close();
                Pokaż();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //edytuj danie 
            try
            {
                Object sm = listBox1.SelectedItem;
                string what = sm.ToString();//nazwa objektu zaznaczonego w listboxie

                string whatWhat = "";//nazwa obiektu z DB
                for (int a = 0; a < what.Length; a++)
                {
                    if((what[a] == ' '&& what[a+1] == '('))
                    { 
                            break;
                    }
                    else
                        whatWhat += what[a];
                }

                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query1 = "SELECT * FROM SkładnikMenu WHERE NazwaSM = '" + whatWhat + "';";
                OleDbCommand command1 = new OleDbCommand(query1, connection);
                OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
                DataSet data = new DataSet();
                AdapterTabela1.Fill(data, "SkładnikMenu");

                string[] str = new string[3];
                str[0] = data.Tables["SkładnikMenu"].Rows[0][1].ToString();
                str[1] = data.Tables["SkładnikMenu"].Rows[0][2].ToString();
                str[2] = data.Tables["SkładnikMenu"].Rows[0][3].ToString();
                Dodaj edytuj = new Dodaj(str[0],str[1],str[2]);
                edytuj.Show();
                connection.Close();
                Pokaż();
            }
            catch
            {
               
            }
        }
    }
}
