﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace WokItEasy
{
    
    public partial class Dodaj : Form
    {
        static StreamWriter sw = new StreamWriter(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt"));
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
      
        string nazwaEdytowana = "";
        string katEdytowana = "";
        bool trybKategorii = false;
        private void XMLConvert() //Konwersja do XML oraz do txt
        {
            DataSet dataSet = new DataSet();
            OleDbConnection connnection = new OleDbConnection(source);
            using (connnection)
            {
                connnection.Open();
                // Retrieve the schema
                DataTable schemaTable = connnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                // Fill the DataTables.
                foreach (DataRow dataTableRow in schemaTable.Rows)
                {

                    string tableName = dataTableRow["Table_Name"].ToString();
                    // I seem to get an extra table starting with ~. I can't seem to screen it out based on information in schemaTable,
                    // hence this hacky check.
                    if (!tableName.StartsWith("~", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (tableName == "SkładnikMenu")
                        {
                            FillTable(dataSet, connnection, tableName);//Wyciągam teraz tylko składniki
                        }
                    }
                }
                connnection.Close();
            }

            string name = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.xml");
            dataSet.WriteXml(name);
        }
        private static void FillTable(DataSet dataSet, OleDbConnection conn, string tableName)// Funkcja pomocnicza do konwersji
        {
            DataTable dataTable = dataSet.Tables.Add(tableName);
            using (OleDbCommand readRows = new OleDbCommand("SELECT * from " + tableName, conn))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(readRows);
                adapter.Fill(dataTable);
                //MessageBox.Show(Convert.ToString(dataTable.Rows.Count));
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string text = dataTable.Rows[i][0].ToString() + " " + dataTable.Rows[i][1].ToString() + " " + dataTable.Rows[i][2].ToString() + " " + dataTable.Rows[i][3].ToString();
                    sw.WriteLine(text);
                }
                sw.Close();
            }
        }
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
                XMLConvert();
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
                XMLConvert();
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
                XMLConvert();
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

                query = "UPDATE Kategoria SET NazwaKategorii ='" + textBox1.Text + "', Kuchnia = " + checkBox1.Checked.ToString() + " WHERE NazwaKategorii = '" + whatWhat + "';";
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
                string query = "INSERT INTO Kategoria (NazwaKategorii,Opis,Kuchnia) VALUES('";
                query += textBox1.Text;
                query += "', '";
                query += "', "+checkBox1.Checked.ToString();
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
                checkBox1.Visible = true;
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
                checkBox1.Visible = false;
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
