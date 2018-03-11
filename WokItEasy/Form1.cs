using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;//watek
using System.Threading.Tasks;
using System.Data.OleDb;//access
using System.Windows.Forms;
using System.IO;//txt

namespace WokItEasy
{
    
    public partial class Form1 : Form
    {
        List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
        List<TcpListener> l_Sockets = new List<TcpListener>();
        List<string> l_Zalogowani = new List<string>();
        string encryptyingCode = "FISH!";
        Thread t_Listen;
        Thread t_Perform;
        private bool end = true;
        private static Mutex mut = new Mutex();
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
      
        //static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Przemek\Desktop\repozytorium\WokItEasy\WokItEasy1.mdb";
      
        private static Użytkownik obecnieZalogowanyUżytkownik = new Użytkownik();

        internal static Użytkownik ObecnieZalogowanyUżytkownik { get => obecnieZalogowanyUżytkownik; set => obecnieZalogowanyUżytkownik = value; }

        private void DopiszZamowienie(string produkty, double cena, int id)
        {
            string connectionString = source;
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();
            string query1 = "INSERT INTO Zamówienia (DataZamówienia, KwotaZamówienia, IDSM, IDObsługi, Online, Rozliczone) VALUES('";
            query1 += DateTime.Now.ToString();
            query1 += "', '";
            query1 += cena;
            query1 += "', '";
            query1 += produkty;
            query1 += "', ";
            query1 += Convert.ToString(id);// tu trzeba bedzie dać ID pracownika z konta klienta który przesłał zamówienie
            query1 += ", ";
            query1 += "True";
            query1 += ", ";
            query1 += "False);";
            OleDbCommand comm = new OleDbCommand(query1, conn);
            OleDbDataAdapter AdapterTab = new OleDbDataAdapter(comm);
            DataSet data1 = new DataSet();
            AdapterTab.Fill(data1, "Zamówienia");
            conn.Close();

        }
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

            string name =System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.xml");
            dataSet.WriteXml(name);
        }
        private static void FillTable(DataSet dataSet, OleDbConnection conn, string tableName)// Funkcja pomocnicza do konwersji
        {
            StreamWriter sw = new StreamWriter(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt"));
            DataTable dataTable = dataSet.Tables.Add(tableName);
            using (OleDbCommand readRows = new OleDbCommand("SELECT * from " + tableName, conn))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(readRows);
                adapter.Fill(dataTable);
                string text = Convert.ToString(dataTable.Rows.Count)+" ";
                sw.WriteLine(text);
                //MessageBox.Show(Convert.ToString(dataTable.Rows.Count));
                for (int i=0;i<dataTable.Rows.Count;i++)
                {
                   text = dataTable.Rows[i][0].ToString() + " " + dataTable.Rows[i][1].ToString() + " " + dataTable.Rows[i][2].ToString()+" "+ dataTable.Rows[i][3].ToString();
                   sw.WriteLine(text);
                }
                sw.Close();
            }
        }
        class ParametryWatku
        {
            public int id;
            public SynchronizationContext synchro;
        }
        private void Listener(object objParam)
        {
            if (!end) t_Listen.Abort();
            bool next = true;
            while (end)
            {
                if(next)
                {
                    IPAddress ipAd = IPAddress.Parse("127.0.0.1");//ip serwera
                    TcpListener myList = new TcpListener(ipAd, 8001);//ip portu
                    //myList.Start();
                    //Socket s = myList.AcceptSocket();
                    mut.WaitOne();
                    l_Sockets.Add(myList);
                    l_Sockets.First<TcpListener>().Start();
                    mut.ReleaseMutex();
                    next = false;
                }
                mut.WaitOne();
                if(l_Sockets.Count==0|| l_Sockets.Last<TcpListener>().Pending())
                {
                    next = true;
                }
                mut.ReleaseMutex();
            }
        }
        private void Performer(object objParam)
        {
            Socket s;
            if (!end) t_Perform.Abort();
            while (end)
            {
                
                try
                {
                    mut.WaitOne();
                    if (l_Sockets.Count != 0)
                    {
                        //test
                        s = l_Sockets.First<TcpListener>().AcceptSocket();
                        mut.ReleaseMutex();
                        ASCIIEncoding asen;
                        string str;
                        byte[] b = new byte[256];
                        int k = s.Receive(b);//odczytanie tekstu od klienta
                        string tekst = "";
                        for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                        tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                        if (tekst=="W")
                        {
                            asen = new ASCIIEncoding();//odpowiedz do klienta
                            s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
                            b = new byte[256];
                            k = s.Receive(b);//odczytanie ilosc w zamowieniu od klienta
                            tekst = "";
                            for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                            tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                            string loginDoWylogowania = tekst;
                            l_Zalogowani.Remove(loginDoWylogowania);
                        }
                        if (tekst == "O")
                        {
                            string order="";// lista obiektów identyfikowanych przez ID
                            asen = new ASCIIEncoding();//odpowiedz do klienta
                            s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
                            b = new byte[256];
                            k = s.Receive(b);//odczytanie ilosc w zamowieniu od klienta
                            tekst = "";
                            for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                            tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                            int ilosc = Convert.ToInt32(tekst);
                            s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));

                            b = new byte[256];
                            k = s.Receive(b);//odczytanie id od klienta
                            tekst = "";
                            for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                            tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                            int idTarget = Convert.ToInt32(tekst);
                            s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));

                            //wczytywanie listy zamówień
                            for (int i=0;i<ilosc;i++)
                            {
                                b = new byte[300];
                                k = s.Receive(b);//odczytanie tekstu od klienta
                                s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
                                tekst = "";
                                for (int j = 0; j < k; j++) tekst += Convert.ToChar(b[j]);
                                tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                                order += tekst + " ";
                            }
                            string[] split = order.Split(' ');
                            ilosc = split.Length-1;
                            //MessageBox.Show(order);
                            // tu trzeba opracować kod który bedzie realizował zamówienie
                            try
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
                                connection.Close();

                            }
                            catch
                            {
                                MessageBox.Show("Błąd odczytywania menu");

                            }
                            double cena = 0.0;
                            string produkty = "";
                            for(int a=0;a<ilosc;a++)
                            {
                                foreach (var skladnik in listaSM)
                                {
                                    if (skladnik.IdSM == Convert.ToInt32(split[a]))
                                    {
                                        cena += skladnik.CenaSM;
                                        produkty += skladnik.NazwaSM + ", ";
                                        break;
                                    }
                                }
                            }
                            tekst = "";
                            DopiszZamowienie(produkty, cena, idTarget);//wpisanie zamówienia do bazy
                        }
                        if (tekst == "L")//Logowanie
                        {
                            asen = new ASCIIEncoding();//odpowiedz do klienta
                            str= Szyfrowanie.Encrypt("OK", encryptyingCode);
                            s.Send(asen.GetBytes(str));
                            b = new byte[256];
                            k = s.Receive(b);//odczytanie tekstu od klienta
                            tekst = "";
                           
                            for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                            tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                            string[] splited = tekst.Split(' ');

                            OleDbConnection connection = new OleDbConnection(source);
                            connection.Open();//poszukiwanie loginu i hasla
                            string query = "SELECT * FROM Pracownicy";
                            OleDbCommand command = new OleDbCommand(query, connection);
                            OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                            DataSet data = new DataSet();
                            AdapterTabela.Fill(data, "Pracownicy");
                            string wartosc;
                            string aktywny;
                            for (int a = 0; a < data.Tables["Pracownicy"].Rows.Count; a++)
                            {
                                wartosc = data.Tables["Pracownicy"].Rows[a]["Login"].ToString();
                                aktywny = data.Tables["Pracownicy"].Rows[a]["Aktywny"].ToString();

                                if (wartosc == splited[0])
                                {
                                    string haslo = data.Tables["Pracownicy"].Rows[a]["Hasło"].ToString();
                                    if (haslo == splited[1])
                                    {
                                        bool free = true;
                                        foreach (string log in l_Zalogowani)
                                        {
                                            if (log == splited[0])
                                            {
                                                free = false;
                                            }
                                        }
                                        if (free)
                                        {
                                            string ID = data.Tables["Pracownicy"].Rows[a]["IDPracownika"].ToString();
                                            l_Zalogowani.Add(splited[0]);
                                            asen = new ASCIIEncoding();//opwoiedz do klienta
                                            str = Szyfrowanie.Encrypt("C", encryptyingCode);
                                            s.Send(asen.GetBytes(str));

                                            str = Szyfrowanie.Encrypt(ID, encryptyingCode);
                                            s.Send(asen.GetBytes(str));

                                            string filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt");
                                            s.SendFile(filename);
                                        }
                                        else
                                        {
                                            asen = new ASCIIEncoding();//opwoiedz do klienta
                                            str = Szyfrowanie.Encrypt("W", encryptyingCode);
                                            s.Send(asen.GetBytes(str));
                                        }
                                        
                                    }
                                    else
                                    {
                                        asen = new ASCIIEncoding();//opwoiedz do klienta
                                        str = Szyfrowanie.Encrypt("W", encryptyingCode);
                                        s.Send(asen.GetBytes(str));
                                    }
                                }
                                else if(a==(data.Tables["Pracownicy"].Rows.Count)-1)
                                {
                                    asen = new ASCIIEncoding();//opwoiedz do klienta
                                    str = Szyfrowanie.Encrypt("W", encryptyingCode);
                                    s.Send(asen.GetBytes(str));
                                }
                            }
                            connection.Close();
                        }
                        s.Close();
                        mut.WaitOne();
                        l_Sockets.First<TcpListener>().Stop();
                        l_Sockets.RemoveAt(0);
                        mut.ReleaseMutex();
                    }
                    else
                    {
                        mut.ReleaseMutex();
                    }
                    
                }
                catch (Exception e)
                {
                    mut.WaitOne();
                    l_Sockets.First<TcpListener>().Stop();
                    l_Sockets.RemoveAt(0);
                    mut.ReleaseMutex();
                    Console.WriteLine("Error..... " + e.StackTrace);
                }

            }
            
        }
        string zwrocKategorie(string a)
        {
            OleDbConnection connection = new OleDbConnection(source);
            connection.Open();
            string query = "SELECT NazwaKategorii FROM Kategoria WHERE Identyfikator = " + a + ";";

            OleDbCommand command1 = new OleDbCommand(query, connection);
            OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
            DataSet data = new DataSet();
            AdapterTabela1.Fill(data, "Kategoria");
            a = data.Tables["Kategoria"].Rows[0][0].ToString();
            connection.Close();
            return a;
        }

        public Form1()
        {
            InitializeComponent();
            ParametryWatku parametry = new ParametryWatku();
            parametry.id = 1;
            parametry.synchro = WindowsFormsSynchronizationContext.Current.CreateCopy();
            new Thread(Listener).Start(parametry);
            new Thread(Performer).Start(parametry);
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
          
            end = false;
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
                            l_Zalogowani.Add(textBox1.Text);

                            
                            ParametryWatku parametry = new ParametryWatku(); //aktywacja wątków
                            parametry.id = 1;
                            parametry.synchro = WindowsFormsSynchronizationContext.Current.CreateCopy();
                            //new Thread(Watek).Start(parametry);
                            t_Listen= new Thread(Listener);
                            t_Listen.Start(parametry);
                            t_Perform = new Thread(Performer);
                            t_Perform.Start(parametry);
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
            l_Zalogowani.Clear();
            end = true;
           // t_Listen.Abort();
            //t_Perform.Abort();
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
