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
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Przemek\Desktop\repozytorium\WokItEasy\WokItEasy1.mdb";
        //static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\6\Zespołowe programowanie\WokItEasy\WokItEasy\WokItEasy1.mdb";
        private static Użytkownik obecnieZalogowanyUżytkownik = new Użytkownik();

        internal static Użytkownik ObecnieZalogowanyUżytkownik { get => obecnieZalogowanyUżytkownik; set => obecnieZalogowanyUżytkownik = value; }

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
           
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            ////zaloguj
            // try
            {
                string connString = source;
                    OleDbConnection connection = new OleDbConnection(connString);
                    connection.Open();
                string query = "SELECT * FROM Pracownicy";// + textBox1.Text;
                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                    DataSet data = new DataSet();
                    AdapterTabela.Fill(data,"Pracownicy");
                string wartosc;
                for (int a=0; a < data.Tables["Pracownicy"].Rows.Count; a++)
                {
                    wartosc = data.Tables["Pracownicy"].Rows[a]["Login"].ToString();
                    if (wartosc == textBox1.Text)
                    {
                        string haslo = data.Tables["Pracownicy"].Rows[a]["Hasło"].ToString();
                        if (haslo == textBox2.Text)
                        {
                            //jeśli się udało zaloguj i zapisz w klasie
                            
                            string temp = data.Tables["Pracownicy"].Rows[a][0].ToString();
                            ObecnieZalogowanyUżytkownik.Id = Int16.Parse(temp);

                            temp = data.Tables["Pracownicy"].Rows[a]["Kierownik"].ToString();
                            if (temp == "True")
                                ObecnieZalogowanyUżytkownik.Kierownik = true;
                            else
                                ObecnieZalogowanyUżytkownik.Kierownik = false;

                            ObecnieZalogowanyUżytkownik.Imie= data.Tables["Pracownicy"].Rows[a][1].ToString();
                            ObecnieZalogowanyUżytkownik.Nazwisko = data.Tables["Pracownicy"].Rows[a][2].ToString();
                            ObecnieZalogowanyUżytkownik.Rola = data.Tables["Pracownicy"].Rows[a][4].ToString();
                            label4.Text = ObecnieZalogowanyUżytkownik.Rola +" "+ ObecnieZalogowanyUżytkownik.Imie + " " + ObecnieZalogowanyUżytkownik.Nazwisko;
                            label2.Visible = false;
                            label3.Visible = false;
                            textBox1.Visible = false;
                            textBox2.Visible = false;
                            button6.Visible = false;
                            button1.Visible = true;
                            button2.Visible = true;
                            button3.Visible = true;
                            button4.Visible = true;
                            break;
                        }
                        else
                        {
                            textBox1.Text = "Błędne hasło";
                            textBox2.Text = null;
                        }
                    }
                }
                connection.Close();
            }
            //catch
            //{
            //    textBox1.Text = "Błędne dane";
            //    textBox2.Text = null;
            //}

            //IEnumerable<WokItEasy.WokItEasyDataSet.PracownicyRow> query = wokItEasyDataSet.Pracownicy.Where(LN => LN.Login == textBox1.Text);

            //string query = "SELECT Hasło FROM Pracownicy WHERE Login = " + textBox1.Text;
            //pracownicyTableAdapter.Adapter.
        }
    }
}
