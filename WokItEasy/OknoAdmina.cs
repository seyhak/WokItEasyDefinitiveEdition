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
   
    public partial class OknoAdmina : Form
    {
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
        static bool trybDodawania = false;
        public OknoAdmina()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //anuluj
            this.Close();
        }
        void AktywujDezaktywujPracownika(bool aktywować)
        {
            
            try
            {
                OleDbConnection connection = new OleDbConnection(source);
                connection.Open();
                string query = "UPDATE Pracownicy SET Aktywny = "+aktywować.ToString()+" WHERE Login = '" + textBox4.Text + "';";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Pracownicy");
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Błąd zmiany Aktywności");
            }
            DodajDoComboBoxa();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //aktywuj/dezaktywuj Pracownika
            if(button4.Text=="Aktywuj")
                AktywujDezaktywujPracownika(true);
            else
                AktywujDezaktywujPracownika(false);
            OdblokujButtonAktywacji();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dodaj pracownika
            if (trybDodawania)
            {
                DodajPracownika();
            }
            trybDodawania = !trybDodawania;
            AktywacjaDezaktywacjaInterfejsu();
        }
        void AktywacjaDezaktywacjaInterfejsu()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.ReadOnly = !textBox1.ReadOnly;
            textBox2.ReadOnly = !textBox2.ReadOnly;
            textBox3.ReadOnly = !textBox3.ReadOnly;
            textBox4.ReadOnly = !textBox4.ReadOnly;
            textBox5.ReadOnly = !textBox5.ReadOnly;
            checkBox1.Enabled = !checkBox1.Enabled;
            



        }
        void DodajPracownika()
        {
            try
            {
                if (CzyLoginTakiSam(textBox4.Text)&&CzyPusteTextboxy())
                {
                    OleDbConnection connection = new OleDbConnection(source);
                    connection.Open();
                    string query = "INSERT INTO Pracownicy (ImięPracownika,NazwiskoPracownika,Kierownik,Rola,Login,Hasło,Aktywny) VALUES('";
                    query += textBox1.Text;
                    query += "', '";
                    query += textBox2.Text;
                    query += "', ";
                    query += checkBox1.Checked;
                    query += ", '";
                    query += textBox5.Text;
                    query += "', '";
                    query += textBox4.Text;
                    query += "', '";
                    query += textBox3.Text;
                    query += "', ";
                    query += true;
                    query += ")";
                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                    DataSet data = new DataSet();
                    AdapterTabela.Fill(data, "Pracownicy");
                    connection.Close();
                    MessageBox.Show("Dodano: " + textBox1.Text + " " + textBox2.Text + "na stanowisko: " + textBox5.Text);
                    Menu menu = new Menu();
                    menu.Pokaż();
                }
                
            }
            catch
            {
                MessageBox.Show("Błąd dodania");
            }

        }
        private void OknoAdmina_Load(object sender, EventArgs e)
        {

        }
        bool CzyLoginTakiSam(string a)
        {
            
            try
            {
                OleDbConnection connection = new OleDbConnection(source);
                connection.Open();
                string query = "SELECT Login FROM Pracownicy";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Pracownicy");
                string temp;
                for(int n =0; n < data.Tables["Pracownicy"].Rows.Count; n++)
                {
                    temp = data.Tables["Pracownicy"].Rows[n][0].ToString();
                    if (temp == a)
                    {
                        connection.Close();
                        MessageBox.Show("Login zajęty");
                        return false;
                    }
                }
                connection.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("Błąd sprawdzania BD");
                return false;
            }

        }
        bool CzyPusteTextboxy()
        {
            foreach (Control x in this.Controls)
            {
                if (x is TextBox)
                {
                    if (((TextBox)x).Text == String.Empty)
                    {
                        MessageBox.Show("Niewypełnione dane!");
                        return false;
                    }
                }
            }
            return true;    
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                button4.Visible = true;
                if (checkBox1.Checked)
                {
                    button4.Text = "Dezaktywuj";

                }
                else
                {
                    button4.Text = "Aktywuj";
                }
            }
            else
                button4.Visible = false;
        }
        void DodajDoComboBoxa()
        {
            comboBox1.Items.Clear();
            try
            {
                OleDbConnection connection = new OleDbConnection(source);
                connection.Open();
                string query = "SELECT IDPracownika, Rola, ImięPracownika, NazwiskoPracownika, Aktywny FROM Pracownicy";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Pracownicy");
                string temp;
                for (int n = 0; n < data.Tables["Pracownicy"].Rows.Count; n++)
                {
                    if ((!checkBox2.Checked && !(bool)data.Tables["Pracownicy"].Rows[n][4]))
                    {
                        continue;
                    }
                    else
                    {
                        temp = "";
                        temp += data.Tables["Pracownicy"].Rows[n][0].ToString();
                        temp += ":";
                        temp += data.Tables["Pracownicy"].Rows[n][1].ToString();
                        temp += " ";
                        temp += data.Tables["Pracownicy"].Rows[n][2].ToString();
                        temp += " ";
                        temp += data.Tables["Pracownicy"].Rows[n][3].ToString();
                        comboBox1.Items.Add(temp);
                    }
                }
                comboBox1.Sorted = true;
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Błąd sprawdzania BD dla listy rozwijanej");
            }

        }
        private void comboBox1_Click(object sender, EventArgs e)
        {
            
            DodajDoComboBoxa();
        }
        bool UzupełnijTextboxy()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(source);
                connection.Open();
                string query = "SELECT * FROM Pracownicy WHERE IDPracownika = "+ZwróćID();
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Pracownicy");
                textBox1.Text=  data.Tables["Pracownicy"].Rows[0][1].ToString();
                checkBox1.Checked = (bool)data.Tables["Pracownicy"].Rows[0][3];
                textBox2.Text = data.Tables["Pracownicy"].Rows[0][2].ToString();
                textBox3.Text = data.Tables["Pracownicy"].Rows[0][6].ToString();
                textBox4.Text = data.Tables["Pracownicy"].Rows[0][5].ToString();
                textBox5.Text = data.Tables["Pracownicy"].Rows[0][4].ToString();
                connection.Close();
                return (bool)data.Tables["Pracownicy"].Rows[0][7];
            }
            catch
            {
                MessageBox.Show("Błąd sprawdzania BD dla listy rozwijanej");
                return false;
            }

        }
        string ZwróćID()
        {
            string b = "";
            for(int a = 0; a < comboBox1.Text.Length; a++)
            {
                if (comboBox1.Text[a] != ':')
                    b += comboBox1.Text[a];
                else
                    return b;
            }
            return b;

        }
        void OdblokujButtonAktywacji()
        {
            button4.Visible = true;
            if (UzupełnijTextboxy())//aktywny
            {
                button4.Text = "Dezaktywuj";
            }
            else
                button4.Text = "Aktywuj";
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdblokujButtonAktywacji();
        }
    }
}
