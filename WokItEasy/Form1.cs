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
using System.Globalization;

namespace WokItEasy
{
    
    public partial class Form1 : Form
    {
        static List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
        List<TcpListener> l_Listeners = new List<TcpListener>();
        static List<string> l_Zalogowani = new List<string>();
        static string encryptyingCode = "FISH!";
        Thread t_Listen;
        Thread t_Perform;
        public static bool end = true;
        private static Mutex mut = new Mutex();
        private static Mutex mut2 = new Mutex();
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
      
        private static Użytkownik obecnieZalogowanyUżytkownik = new Użytkownik();

        internal static Użytkownik ObecnieZalogowanyUżytkownik { get => obecnieZalogowanyUżytkownik; set => obecnieZalogowanyUżytkownik = value; }

        
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
        private void Listener()
        {
            IPAddress ipAd = IPAddress.Parse("127.0.0.1");//ip serwera
            TcpListener myList = new TcpListener(ipAd, 8001);//ip portu
            myList.Start();
            Socket s;
            while (end)
            {
                if (!end) t_Listen.Abort();
                while (end)
                {
                    if (myList.Pending())
                    {
                        s = myList.AcceptSocket();
                        Performer2 p = new Performer2();
                        p.StartClient(s);
                    }

                }
            }
            myList.Stop();
        }
        public class Performer2
        {
            Socket s;
            public void StartClient(Socket insocket)
            {
                this.s = insocket;
                Thread perfThread = new Thread(Perform);
                perfThread.Start();
            }
            private void Perform()
            {
                try
                {
                    listaSM = SkładnikMenu.Zbuduj(source);
                    //test
                    //s = l_Listeners.First<TcpListener>().AcceptTcpClient();
                    //mut.ReleaseMutex();
                    ASCIIEncoding asen;
                    string str;
                    byte[] b = new byte[256];
                    int k = s.Receive(b);//odczytanie tekstu od klienta
                    string tekst = "";
                    for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                    //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                    switch (tekst)
                    {
                        case "SZ"://pobierz listę zamówień
                            {
                                asen = new ASCIIEncoding();//odpowiedz do klienta
                                //s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
                                s.Send(asen.GetBytes("OK"));
                                UTF8Encoding coderUTF = new UTF8Encoding();
                                List<Zamówienie> listaZM = Zamówienie.ZbudujZamówienia();
                                s.Send(asen.GetBytes(Convert.ToString(listaZM.Count())));
                                foreach (Zamówienie z in listaZM)
                                {
                                    string wyjscie = Zamówienie.ZbudujString(z);
                                    System.Diagnostics.Debug.WriteLine(wyjscie);
                                    s.Send(coderUTF.GetBytes(LengthConverter.Convert(coderUTF.GetByteCount(wyjscie))));//długość,
                                    //Thread.Sleep(1000);
                                    s.Send(coderUTF.GetBytes(wyjscie));//pozycja
                                    //Thread.Sleep(1000);
                                }
                                break;
                            }
                        case "FZ": //wykonanie zamówienia przez klienta
                            {
                                asen = new ASCIIEncoding();//odpowiedz do klienta
                                s.Send(asen.GetBytes("OK"));
                                b = new byte[256];
                                k = s.Receive(b);//odczytanie ilosc w zamowieniu od klienta
                                tekst = "";
                                for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                                Zamówienie.WykonajZamówienie(Convert.ToInt32(tekst),false);
                                break;
                            }
                        //test
                        case "W":
                            {
                                asen = new ASCIIEncoding();//odpowiedz do klienta
                                                           //s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
                                s.Send(asen.GetBytes("OK"));
                                b = new byte[256];
                                k = s.Receive(b);//odczytanie ilosc w zamowieniu od klienta
                                tekst = "";
                                for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                                //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                                string loginDoWylogowania = tekst;
                                int tmp = 0;
                                mut2.WaitOne();
                                foreach (string st in l_Zalogowani)
                                {
                                    if (st == loginDoWylogowania)
                                    {
                                        l_Zalogowani[tmp] = "";
                                        l_Zalogowani.RemoveAt(tmp);
                                    }
                                    
                                    tmp++;
                                }
                                mut2.ReleaseMutex();
                                break;
                            }
                        case "O"://Zamówienia
                            {
                                string order = "";// lista obiektów identyfikowanych przez ID
                                asen = new ASCIIEncoding();//odpowiedz do klienta
                                str = "OK";
                                s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
                                s.Send(asen.GetBytes(str));
                                Thread.Sleep(400);//poczekaj aż klient wykona stringa
                                b = new byte[2560];
                                k = s.Receive(b);//odczytanie id od klienta
                                tekst = "";
                                for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
                                order = tekst;

                                //wczytywanie listy zamówień
                                order = SkładnikMenu.GetNazwyZIdZPrzecinkamiKlient(order);
                                order.TrimEnd(',');
                                string[] split = order.Split(',');

                                int idObsługi = Convert.ToInt32(split[0]);
                                System.Diagnostics.Debug.WriteLine("Zamówienie od: " + split[0]);
                                double kwota = 0;
                                kwota = Double.Parse(split[1], CultureInfo.InvariantCulture);
                                List<string> listIDOrders = new List<string>();
                                for (int v = 2; v < split.Length; v++)
                                {
                                    listIDOrders.Add(split[v]);
                                }
                                System.Diagnostics.Debug.WriteLine("Zamówienie: " + listIDOrders.ToString() + " " + kwota + " " + idObsługi + " " + source);
                                Zamówienie.DopiszZamowieniaZListyNazw(listIDOrders, kwota, idObsługi, source);
                                System.Diagnostics.Debug.WriteLine("Dodano zamówienie");
                                break;
                            }
                        case "M"://Lista dań
                            {
                                string position = "";
                                asen = new ASCIIEncoding();//odpowiedz do klienta
                                UTF8Encoding coderUTF = new UTF8Encoding();
                                //s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
                                str = "OK";
                                s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
                                s.Send(asen.GetBytes(str));

                                //wysłanie ilości dań
                                System.Diagnostics.Debug.WriteLine("Sending Menu...");
                                s.Send(asen.GetBytes(LengthConverter.Convert(listaSM.Count)));//ile pozycji
                                foreach (SkładnikMenu sm in listaSM)
                                {
                                    position = sm.getAlmostXML();
                                    System.Diagnostics.Debug.WriteLine(position);
                                    s.Send(coderUTF.GetBytes(LengthConverter.Convert(coderUTF.GetByteCount(position))));//długość,
                                    //Thread.Sleep(1000);
                                    s.Send(coderUTF.GetBytes(position));//pozycja
                                    //Thread.Sleep(1000);
                                }
                                System.Diagnostics.Debug.WriteLine("Done");
                                break;
                            }
                        case "L":
                            {
                                asen = new ASCIIEncoding();//odpowiedz do klienta
                                                           //str= Szyfrowanie.Encrypt("OK", encryptyingCode);
                                str = "OK";
                                s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
                                Thread.Sleep(300);//inaczej pomija
                                s.Send(asen.GetBytes(str));
                                Thread.Sleep(300);//inaczej pomija
                                b = new byte[256];
                                k = s.Receive(b);//odczytanie tekstu od klienta
                                tekst = "";

                                System.Diagnostics.Debug.WriteLine(k);

                                for (int i = 0; i < k; i++)
                                {
                                    tekst += Convert.ToChar(b[i]);
                                    System.Diagnostics.Debug.Write(Convert.ToChar(b[i]) + " ");
                                }
                                //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                                string[] splited = tekst.Split(' ');

                                System.Diagnostics.Debug.WriteLine("");
                                System.Diagnostics.Debug.WriteLine("Login: " + splited[0]);
                                System.Diagnostics.Debug.WriteLine(" haslo: " + splited[1]);

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

                                        System.Diagnostics.Debug.WriteLine("Poprawny login: " + splited[0]);
                                        string haslo = data.Tables["Pracownicy"].Rows[a]["Hasło"].ToString();
                                        if (haslo == splited[1])
                                        {
                                            System.Diagnostics.Debug.WriteLine("Poprawne hasło: " + splited[1]);
                                            bool free = true;//jeżeli jest wolny na liście
                                            mut2.WaitOne();
                                            foreach (string log in l_Zalogowani)
                                            {
                                                if (log == splited[0])
                                                {
                                                    free = false;
                                                }
                                            }
                                            mut2.ReleaseMutex();
                                            if (free)//jeżeli jest połączony to "C"
                                            {
                                                System.Diagnostics.Debug.WriteLine("Wysyłanie C ");
                                                string ID = data.Tables["Pracownicy"].Rows[a]["IDPracownika"].ToString();
                                                l_Zalogowani.Add(splited[0]);
                                                asen = new ASCIIEncoding();//opwoiedz do klienta
                                                                           //str = Szyfrowanie.Encrypt("C", encryptyingCode);
                                                str = "C";
                                                s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
                                                s.Send(asen.GetBytes(str));
                                                //wysyłamy ID 
                                                str = data.Tables["Pracownicy"].Rows[a]["IDpracownika"].ToString();
                                                System.Diagnostics.Debug.WriteLine("ID : " + str);
                                                s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
                                                s.Send(asen.GetBytes(str));
                                            }

                                        }
                                    }
                                }
                                connection.Close();
                                break;
                            }
                    }
                }
                catch
                {

                }
            }
        }
        //private void Performer(object objParam)
        //{

        //    //zbudowanie listy
        //    listaSM = SkładnikMenu.Zbuduj(source);
        //    Socket s;
        //    if (!end) t_Perform.Abort();
        //    while (end)
        //    {
                
        //        try
        //        {
        //            mut.WaitOne();
        //            if (l_Listeners.Count != 0)
        //            {
        //                //test
        //                //s = l_Listeners.First<TcpListener>().AcceptTcpClient();
        //                mut.ReleaseMutex();
        //                ASCIIEncoding asen;
        //                string str;
        //                byte[] b = new byte[256];
        //                int k = s.Receive(b);//odczytanie tekstu od klienta
        //                string tekst = "";
        //                for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                switch (tekst) {
        //                    case "W"://Wylogowanie
        //                {
        //                    asen = new ASCIIEncoding();//odpowiedz do klienta
        //                    //s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
        //                    s.Send(asen.GetBytes("OK"));
        //                    b = new byte[256];
        //                    k = s.Receive(b);//odczytanie ilosc w zamowieniu od klienta
        //                    tekst = "";
        //                    for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                    //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                    string loginDoWylogowania = tekst;
        //                    int tmp = 0;
        //                    mut2.WaitOne();
        //                    foreach (string st in l_Zalogowani)
        //                    {
        //                        if (st == loginDoWylogowania) l_Zalogowani.RemoveAt(tmp);
        //                        tmp++;
        //                    }
        //                    mut2.ReleaseMutex();
        //                            continue;
        //                }
        //                    case "O"://Zamówienia
        //                {
        //                            string order = "";// lista obiektów identyfikowanych przez ID
        //                            asen = new ASCIIEncoding();//odpowiedz do klienta
        //                            str = "OK";
        //                            s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
        //                            s.Send(asen.GetBytes(str));
        //                            Thread.Sleep(400);//poczekaj aż klient wykona stringa
        //                            b = new byte[2560];
        //                            k = s.Receive(b);//odczytanie id od klienta
        //                            tekst = "";
        //                            for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                            order = tekst;

        //                            //wczytywanie listy zamówień
        //                            string[] split = order.Split('#');
                                    
        //                            int idObsługi = Convert.ToInt32(split[0]);
        //                            System.Diagnostics.Debug.WriteLine("Zamówienie od: " + split[0]);
        //                            double kwota = 0;
        //                            kwota = Double.Parse(split[1], CultureInfo.InvariantCulture);
        //                            List<int> listIDOrders = new List<int>();
        //                            for(int v = 2; v < split.Length; v++)
        //                            {
        //                                listIDOrders.Add(Convert.ToInt32(split[v]));
        //                            }
        //                            System.Diagnostics.Debug.WriteLine("Zamówienie: " + listIDOrders.ToString()+" "+kwota + " " + idObsługi + " " + source);
        //                            Zamówienie.DopiszZamowieniaZListyID(listIDOrders,kwota,idObsługi, source);
        //                            System.Diagnostics.Debug.WriteLine("Dodano zamówienie");
        //                            continue;
        //                        }
        //                    case "M"://Lista dań
        //                        {
        //                            string position = "";
        //                            asen = new ASCIIEncoding();//odpowiedz do klienta
        //                            UTF8Encoding coderUTF = new UTF8Encoding();
        //                            //s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
        //                            str = "OK";
        //                            s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
        //                            s.Send(asen.GetBytes(str));

        //                            //wysłanie ilości dań
        //                            System.Diagnostics.Debug.WriteLine("Sending Menu..."); 
        //                            s.Send(asen.GetBytes(LengthConverter.Convert(listaSM.Count)));//ile pozycji
        //                            foreach(SkładnikMenu sm in listaSM)
        //                            {
        //                                position = sm.getAlmostXML();
        //                                System.Diagnostics.Debug.WriteLine(position);
        //                                s.Send(coderUTF.GetBytes(LengthConverter.Convert(coderUTF.GetByteCount(position))));//długość
        //                                s.Send(coderUTF.GetBytes(position));//pozycja
        //                            }
        //                            System.Diagnostics.Debug.WriteLine("Done");
        //                            continue;
        //                        }
        //                    case "L"://Logowanie
        //                        {
        //                    asen = new ASCIIEncoding();//odpowiedz do klienta
        //                    //str= Szyfrowanie.Encrypt("OK", encryptyingCode);
        //                    str = "OK";
        //                    s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
        //                    s.Send(asen.GetBytes(str));
        //                    Thread.Sleep(300);//inaczej pomija
        //                    b = new byte[256];
        //                    k = s.Receive(b);//odczytanie tekstu od klienta
        //                    tekst = "";

        //                    System.Diagnostics.Debug.WriteLine(k);

        //                    for (int i = 0; i < k; i++)
        //                    { tekst += Convert.ToChar(b[i]);
        //                        System.Diagnostics.Debug.Write(Convert.ToChar(b[i]) + " ");
        //                    }
        //                    //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                    string[] splited = tekst.Split(' ');

        //                    System.Diagnostics.Debug.WriteLine("");
        //                    System.Diagnostics.Debug.WriteLine("Login: " + splited[0]);
        //                    System.Diagnostics.Debug.WriteLine(" haslo: " + splited[1]);

        //                    OleDbConnection connection = new OleDbConnection(source);
        //                    connection.Open();//poszukiwanie loginu i hasla
        //                    string query = "SELECT * FROM Pracownicy";
        //                    OleDbCommand command = new OleDbCommand(query, connection);
        //                    OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
        //                    DataSet data = new DataSet();
        //                    AdapterTabela.Fill(data, "Pracownicy");
        //                    string wartosc;
        //                    string aktywny;
        //                    for (int a = 0; a < data.Tables["Pracownicy"].Rows.Count; a++)
        //                    {
        //                        wartosc = data.Tables["Pracownicy"].Rows[a]["Login"].ToString();
        //                        aktywny = data.Tables["Pracownicy"].Rows[a]["Aktywny"].ToString();

        //                        if (wartosc == splited[0])
        //                        {

        //                            System.Diagnostics.Debug.WriteLine("Poprawny login: " + splited[0]);
        //                            string haslo = data.Tables["Pracownicy"].Rows[a]["Hasło"].ToString();
        //                            if (haslo == splited[1])
        //                            {
        //                                System.Diagnostics.Debug.WriteLine("Poprawne hasło: " + splited[1]);
        //                                bool free = true;//jeżeli jest wolny na liście
        //                                mut2.WaitOne();
        //                                foreach (string log in l_Zalogowani)
        //                                {
        //                                    if (log == splited[0])
        //                                    {
        //                                        free = false;
        //                                    }
        //                                }
        //                                mut2.ReleaseMutex();
        //                                if (free)//jeżeli jest połączony to "C"
        //                                {
        //                                    System.Diagnostics.Debug.WriteLine("Wysyłanie C ");
        //                                    string ID = data.Tables["Pracownicy"].Rows[a]["IDPracownika"].ToString();
        //                                    l_Zalogowani.Add(splited[0]);
        //                                    asen = new ASCIIEncoding();//opwoiedz do klienta
        //                                    //str = Szyfrowanie.Encrypt("C", encryptyingCode);
        //                                    str = "C";
        //                                    s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
        //                                    s.Send(asen.GetBytes(str));
        //                                    //wysyłamy ID 
        //                                    str = data.Tables["Pracownicy"].Rows[a]["IDpracownika"].ToString();
        //                                    System.Diagnostics.Debug.WriteLine("ID : "+str);
        //                                    s.Send(asen.GetBytes(LengthConverter.Convert(str.Length)));//długość słowa
        //                                    s.Send(asen.GetBytes(str));
        //                                }

        //                            }
        //                        }
        //                    }
        //                    connection.Close();continue;
        //                        }
        //                }
        //                s.Close();
        //                mut.WaitOne();
        //                l_Listeners.First<TcpListener>().Stop();
        //                l_Listeners.RemoveAt(0);
        //                mut.ReleaseMutex();
        //            }
        //            else
        //            {
        //                mut.ReleaseMutex();
        //            }
                    
        //        }
        //        catch (Exception e)
        //        {
        //            mut.WaitOne();
        //            l_Listeners.First<TcpListener>().Stop();
        //            l_Listeners.RemoveAt(0);
        //            mut.ReleaseMutex();
        //            Console.WriteLine("Error..... " + e.StackTrace);
        //        }

        //    }
            
        //}
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
            Kategoria.ZbudujStatycznąListęKategorii(source);
            SkładnikMenu.Zbuduj();
            //ParametryWatku parametry = new ParametryWatku();
            //parametry.id = 1;
            //parametry.synchro = WindowsFormsSynchronizationContext.Current.CreateCopy();
            //new Thread(Listener).Start(parametry);
            //new Thread(Performer).Start(parametry);
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

        private void button5_Click(object sender, EventArgs e)//wyjdź
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

        private void button6_Click_1(object sender, EventArgs e)////zaloguj
        {
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



                            //new Thread(Watek).Start(parametry);
                            t_Listen = new Thread(Listener);
                            t_Listen.Start();
                            //t_Perform = new Thread(Performer);
                            //t_Perform.Start(parametry);
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
                            button9.Visible = true;
                            button10.Visible = true;
                            button11.Visible = true;
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

        private void button7_Click(object sender, EventArgs e)//wyloguj
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
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            pictureBox1.Visible = true;
            ObecnieZalogowanyUżytkownik = new Użytkownik();
            l_Zalogowani.Clear();
            end = false;
           // t_Listen.Abort();
            //t_Perform.Abort();
           // MessageBox.Show("Wylogowano");
        }

        private void button8_Click(object sender, EventArgs e)//okno admina
        {
            //dodawanie pracownika i uprawinień
            OknoAdmina oA = new OknoAdmina();
            oA.Show();
        }

        private void button9_Click(object sender, EventArgs e)//obecne zamówienia
        {
            ObecneZamówienia oz = new ObecneZamówienia();
            oz.Show();
            if (Screen.AllScreens.Length > 1)
            {
                Screen[] screens = Screen.AllScreens;
                Rectangle bounds = screens[1].Bounds;
                oz.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                oz.StartPosition = FormStartPosition.Manual;
                oz.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            oz.WindowState = FormWindowState.Maximized;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ObecneZamówienia oz = new ObecneZamówienia(true);
            oz.Show();
            if (Screen.AllScreens.Length > 1)
            {
                Screen[] screens = Screen.AllScreens;
                Rectangle bounds = screens[1].Bounds;
                oz.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                oz.StartPosition = FormStartPosition.Manual;
                oz.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            oz.WindowState = FormWindowState.Maximized;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OdbiórZamówień oz = new OdbiórZamówień();
            oz.Show();
            if (Screen.AllScreens.Length > 1)
            {
                Screen[] screens = Screen.AllScreens;
                Rectangle bounds = screens[1].Bounds;
                oz.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                oz.StartPosition = FormStartPosition.Manual;
                oz.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            oz.WindowState = FormWindowState.Maximized;
        }
    }
}
