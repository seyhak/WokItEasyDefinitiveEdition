using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;//access
using System.Windows.Forms;

namespace WokItEasy
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Menu formMenu = new Menu();
            formMenu.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rozliczenie formRozliczenie = new Rozliczenie();
            formRozliczenie.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Historia formHistoria = new Historia();
            formHistoria.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ////zaloguj
           // try
            {
                string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\4\Inżynieria oprogramowania\WokItEasy\WokItEasy1.mdb";
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT Hasło FROM Pracownicy WHERE Login = " + textBox1.Text;
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Pracownicy");
                connection.Close();
            }
            //catch
            {
                textBox1.Text= "Błędne dane";
                textBox2.Text = null;
            }

            //IEnumerable<WokItEasy.WokItEasyDataSet.PracownicyRow> query = wokItEasyDataSet.Pracownicy.Where(LN => LN.Login == textBox1.Text);
            
            //string query = "SELECT Hasło FROM Pracownicy WHERE Login = " + textBox1.Text;
            //pracownicyTableAdapter.Adapter.
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }
    }
}
