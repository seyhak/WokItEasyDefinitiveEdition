using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WokItEasy
{
    
    public partial class Dodaj : Form
    {
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Przemek\Desktop\repozytorium\WokItEasy\WokItEasy1.mdb";
        //static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\6\Zespołowe programowanie\WokItEasy\WokItEasy\WokItEasy1.mdb";
        string nazwaEdytowana = "";
        string katEdytowana = "";
        bool trybKategorii = false;
        void DodajPozycje()
        {
            try
            {
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "INSERT INTO SkładnikMenu (NazwaSM, RodzajSM, CenaSM, DataDodaniaSM) VALUES('";
                query += textBox1.Text;
                query += "', '";
                query += Numer(comboBox1.Text);
                query += "', '";
                query += textBox2.Text;
                query += "', '";
                query += DateTime.Now.ToString();
                query += "')";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "SkładnikMenu");
                connection.Close();
                MessageBox.Show("Dodano: " + textBox1.Text);
                Menu menu = new Menu();
                menu.Pokaż();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Błąd danych");
            }

        }
        private int Numer(string w)
        {
           
            try
            {
                //wyszukujemy numer kategorii
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query1 = "SELECT Identyfikator FROM Kategoria WHERE NazwaKategorii = '"+w+"';";
                OleDbCommand command1 = new OleDbCommand(query1, connection);
                OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
                DataSet data = new DataSet();
                AdapterTabela1.Fill(data, "Kategoria");
                int b = Convert.ToInt32(data.Tables["Kategoria"].Rows[0][0]);
                connection.Close();
                return b;
            }
            catch
            {
                MessageBox.Show("Błąd połączenia z bazą danych");
                return -1;
            }
        }
        public Dodaj()
        {
            
            InitializeComponent();
            try
            {
                
                //wyszukujemy kategorie i dodajemy do comboboxaSS
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query1 = "SELECT * FROM Kategoria";
                OleDbCommand command1 = new OleDbCommand(query1, connection);
                OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
                DataSet data = new DataSet();
                AdapterTabela1.Fill(data, "Kategoria");
                string wartosc;
                List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
                for (int a = 0; a < data.Tables["Kategoria"].Rows.Count; a++)
                {
                    wartosc = data.Tables["Kategoria"].Rows[a][1].ToString();
                    comboBox1.Items.Add(wartosc);
                }

                connection.Close();
            }
            catch
            {
                MessageBox.Show("Błąd połączenia z bazą danych");
            }
        }
        public Dodaj(string nazwa,string kat, string cena)///Edycja
        {

            InitializeComponent();
            try
            {
                this.Name = "Edytuj";
                button1.BackColor = Color.Yellow;
                nazwaEdytowana = nazwa;
                katEdytowana = kat;
                button1.Text= "Edytuj";
                label4.Text = "Edytuj ";
                textBox1.Text = nazwa;
                textBox2.Text = cena;
                //wyszukujemy kategorie i dodajemy do comboboxaSS
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query1 = "SELECT * FROM Kategoria";
                OleDbCommand command1 = new OleDbCommand(query1, connection);
                OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
                DataSet data = new DataSet();
                AdapterTabela1.Fill(data, "Kategoria");
                string wartosc;
                List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
                int indKat=Convert.ToInt32(kat);//indeks kategorii
                for (int a = 0; a < data.Tables["Kategoria"].Rows.Count; a++)
                {
                    wartosc = data.Tables["Kategoria"].Rows[a][1].ToString();
                    comboBox1.Items.Add(wartosc);
                }
                comboBox1.SelectedIndex = indKat-1;
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Błąd połączenia z bazą danych");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!trybKategorii)
            {
                if (button1.Text == "Edytuj")
                {
                    EdytujPozycje();
                }
                else
                    DodajPozycje();
            }
            else
            {
                if (button1.Text == "Edytuj")
                {
                    EdytujKategorie();
                }
                else
                    DodajKategorie();

            }
        }
        private void EdytujPozycje()
        {
            //edytuj danie
            try
            {
                string whatWhat = nazwaEdytowana;//nazwa pozycji do usunięcia
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

                DodajPozycje();
            }
            catch {
                MessageBox.Show("Błąd edycji");
            }
        }
        private void EdytujKategorie()
        {
            //edytuj kategorie
            try
            {
                string whatWhat = comboBox1.Text;//nr kat do usunięcia
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM Kategoria";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Kategoria");

                query = "UPDATE Kategoria SET NazwaKategorii ='" + textBox1.Text+ "' WHERE NazwaKategorii = '" + whatWhat + "';";
                command = new OleDbCommand(query, connection);
                AdapterTabela = new OleDbDataAdapter(command);
                data = new DataSet();
                AdapterTabela.Fill(data, "Kategoria");

                connection.Close();
                MessageBox.Show("Edytowano: " + textBox1.Text);
                this.Close();
                //DodajKategorie();
            }
            catch
            {
                MessageBox.Show("Błąd edycji");
            }
        }
        void DodajKategorie()
        {
            try
            {
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "INSERT INTO Kategoria (NazwaKategorii,Opis) VALUES('";
                query += textBox1.Text;
                query += "', '";
                query += "')";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Kategoria");
                connection.Close();
                MessageBox.Show("Dodano: " + textBox1.Text);
                Menu menu = new Menu();
                menu.Pokaż();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Błąd danych");
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            trybKategorii = !trybKategorii;
            if (trybKategorii)//jeżeli tryb kategorii to przełącza się na combobox i możemy zmienić nazwę
            {
                button3.Text = "Wył. tryb katergorii";
                label2.Visible = false;
                textBox1.Text = "";
                if (katEdytowana != "")
                {
                    comboBox1.SelectedIndex = Convert.ToInt32(katEdytowana) - 1;
                }
                label3.Visible = false;
                textBox2.Visible = false;
                button4.Visible = true;
            }
            else
            {
                button3.Text = "Tryb katergorii";
                label2.Visible = true;
                label3.Visible = true;
                textBox2.Visible =true;
                button4.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //usun kategorie
            try
            {

                string what = comboBox1.Text;
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM Kategoria";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Kategoria");

                query = "DELETE FROM Kategoria WHERE NazwaKategorii = '" + what + "';";
                command = new OleDbCommand(query, connection);
                AdapterTabela = new OleDbDataAdapter(command);
                data = new DataSet();
                AdapterTabela.Fill(data, "Kategoria");

                connection.Close();
                MessageBox.Show("Usunięto kategorie: " + comboBox1.Text);
                this.Close();
            }
            catch { }
        }
    }
}
