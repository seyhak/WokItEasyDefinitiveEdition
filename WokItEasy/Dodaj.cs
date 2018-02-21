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
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\6\Zespołowe programowanie\WokItEasy\WokItEasy\WokItEasy1.mdb";

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
                query += comboBox1.Text;
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
                this.Close();
            }
            catch
            {
                MessageBox.Show("Błąd danych");
            }

        }
        public Dodaj()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DodajPozycje();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
