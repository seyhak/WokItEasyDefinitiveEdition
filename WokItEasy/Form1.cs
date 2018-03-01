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
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
      
        //static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Przemek\Desktop\repozytorium\WokItEasy\WokItEasy1.mdb";
      
        private static Użytkownik obecnieZalogowanyUżytkownik = new Użytkownik();

        internal static Użytkownik ObecnieZalogowanyUżytkownik { get => obecnieZalogowanyUżytkownik; set => obecnieZalogowanyUżytkownik = value; }

        private void XMLConvert() //Konwersja do XML
        {
            DataSet dataSet = new DataSet();
            OleDbConnection connnection = new OleDbConnection(source);
            //connnection.Open();
            //string query = "SELECT * FROM Pracownicy";

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
                        FillTable(dataSet, connnection, tableName);
                    }
                }
                connnection.Close();
            }

            string name =System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.xml");
            dataSet.WriteXml(name);
        }

        private static void FillTable(DataSet dataSet, OleDbConnection conn, string tableName)// Funkcja pomocnicza do konwersji
        {
            DataTable dataTable = dataSet.Tables.Add(tableName);
            using (OleDbCommand readRows = new OleDbCommand("SELECT * from " + tableName, conn))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(readRows);
                adapter.Fill(dataTable);
            }
        }

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
                    OleDbConnection connection = new OleDbConnection(source);
                    
                    connection.Open();
                string query = "SELECT * FROM Pracownicy";// + textBox1.Text;
                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                    DataSet data = new DataSet();
                    AdapterTabela.Fill(data,"Pracownicy");
                string wartosc;
                string aktywny;
                for (int a=0; a < data.Tables["Pracownicy"].Rows.Count; a++)
                {
                    wartosc = data.Tables["Pracownicy"].Rows[a]["Login"].ToString();
                    aktywny = data.Tables["Pracownicy"].Rows[a]["Aktywny"].ToString();
                    
                    if (wartosc == textBox1.Text)
                    {
                        string haslo = data.Tables["Pracownicy"].Rows[a]["Hasło"].ToString();
                        if (haslo == textBox2.Text)
                        {
                            //jeśli się udało zaloguj i zapisz w klasie
                            XMLConvert();//Wywołanie konwersji
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
                            pictureBox1.Visible = false;
                            button7.Visible = true;
                            if (ObecnieZalogowanyUżytkownik.Kierownik)
                            {
                                button8.Visible = true;
                            }
                            break;
                        }
                        else
                        {
                            textBox1.Text = "Błędne hasło lub login";
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

        private void button7_Click(object sender, EventArgs e)
        {
            //wyloguj
            button7.Visible = false;
            label2.Visible = true;
            label3.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox1.Text = "";
            textBox2.Text = "";
            button6.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button6.Visible = true;
            button8.Visible = false ;
            pictureBox1.Visible = true;
            ObecnieZalogowanyUżytkownik = new Użytkownik();
            MessageBox.Show("Wylogowano");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //dodawanie pracownika i uprawinień
            OknoAdmina oA = new OknoAdmina();
            oA.Show();
        }
    }
}
