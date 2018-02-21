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
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\6\Zespołowe programowanie\WokItEasy\WokItEasy\WokItEasy1.mdb";
        private void Pokaż()
        {
            string connString = source;
            OleDbConnection connection = new OleDbConnection(connString);
            connection.Open();
            string query = "SELECT * FROM SkładnikMenu";
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
                składnik.RodzajSM = wartosc;
                składnik.NazwaSM = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
                wartosc = data.Tables["SkładnikMenu"].Rows[a][0].ToString();
                składnik.IdSM = Int16.Parse(wartosc);
                wartosc = data.Tables["SkładnikMenu"].Rows[a][3].ToString();
                składnik.CenaSM = Int16.Parse(wartosc);
                wartosc = data.Tables["SkładnikMenu"].Rows[a][4].ToString();
                składnik.DataDodaniaSM = DateTime.Parse(wartosc);
                listaSM.Add(składnik);
                switch (składnik.RodzajSM)
                {
                    case "Podstawa":
                        listBox1.Items.Add(składnik.NazwaSM + " " + składnik.CenaSM.ToString());
                        break;
                    case "Baza":
                        listBox2.Items.Add(składnik.NazwaSM + " " + składnik.CenaSM.ToString());
                        break;
                    case "Sos":
                        listBox3.Items.Add(składnik.NazwaSM + " " + składnik.CenaSM.ToString());
                        break;
                    case "Posypka":
                        listBox4.Items.Add(składnik.NazwaSM + " " + składnik.CenaSM.ToString());
                        break;
                    case "Inne":
                        listBox5.Items.Add(składnik.NazwaSM + " " + składnik.CenaSM.ToString());
                        break;
                    case "Napoje":
                        listBox6.Items.Add(składnik.NazwaSM + " " + składnik.CenaSM.ToString());
                        break;

                }
            }
            connection.Close();

        }
        public Menu()
        {
            InitializeComponent();
            if (Form1.ObecnieZalogowanyUżytkownik.Kierownik)
            {
                button1.Visible = true;
                //button2.Visible = true;
                //button3.Visible = true;
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
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //edytuj danie 
        }
    }
}
