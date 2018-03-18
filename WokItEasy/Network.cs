using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WokItEasy
{
    class Network
    {
        //public static Connection()
        //{
        //    Socket s;
        //    if (!end) t_Perform.Abort();
        //    while (end)
        //    {

        //        try
        //        {
        //            mut.WaitOne();
        //            if (l_Sockets.Count != 0)
        //            {
        //                //test
        //                s = l_Sockets.First<TcpListener>().AcceptSocket();
        //                mut.ReleaseMutex();
        //                ASCIIEncoding asen;
        //                string str;
        //                byte[] b = new byte[256];
        //                int k = s.Receive(b);//odczytanie tekstu od klienta
        //                string tekst = "";
        //                for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                if (tekst == "W")
        //                {
        //                    asen = new ASCIIEncoding();//odpowiedz do klienta
        //                    s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
        //                    b = new byte[256];
        //                    k = s.Receive(b);//odczytanie ilosc w zamowieniu od klienta
        //                    tekst = "";
        //                    for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                    tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                    string loginDoWylogowania = tekst;
        //                    int tmp = 0;
        //                    mut2.WaitOne();
        //                    foreach (string st in l_Zalogowani)
        //                    {
        //                        if (st == loginDoWylogowania) l_Zalogowani.RemoveAt(tmp);
        //                        tmp++;
        //                    }
        //                    mut2.ReleaseMutex();

        //                }
        //                if (tekst == "O")
        //                {
        //                    string order = "";// lista obiektów identyfikowanych przez ID
        //                    asen = new ASCIIEncoding();//odpowiedz do klienta
        //                    s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
        //                    b = new byte[256];
        //                    k = s.Receive(b);//odczytanie ilosc w zamowieniu od klienta
        //                    tekst = "";
        //                    for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                    tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                    int ilosc = Convert.ToInt32(tekst);
        //                    s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));

        //                    b = new byte[256];
        //                    k = s.Receive(b);//odczytanie id od klienta
        //                    tekst = "";
        //                    for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                    tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                    int idTarget = Convert.ToInt32(tekst);
        //                    s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));

        //                    //wczytywanie listy zamówień
        //                    for (int i = 0; i < ilosc; i++)
        //                    {
        //                        b = new byte[300];
        //                        k = s.Receive(b);//odczytanie tekstu od klienta
        //                        s.Send(asen.GetBytes(Szyfrowanie.Encrypt("OK", encryptyingCode)));
        //                        tekst = "";
        //                        for (int j = 0; j < k; j++) tekst += Convert.ToChar(b[j]);
        //                        tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                        order += tekst + " ";
        //                    }
        //                    string[] split = order.Split(' ');
        //                    ilosc = split.Length - 1;
        //                    //MessageBox.Show(order);
        //                    // tu trzeba opracować kod który bedzie realizował zamówienie
        //                    try
        //                    {
        //                        string connString = source;
        //                        OleDbConnection connection = new OleDbConnection(connString);
        //                        connection.Open();
        //                        string query = "SELECT * FROM SkładnikMenu";
        //                        OleDbCommand command = new OleDbCommand(query, connection);
        //                        OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
        //                        DataSet data = new DataSet();
        //                        AdapterTabela.Fill(data, "SkładnikMenu");
        //                        string wartosc;
        //                        for (int a = 0; a < data.Tables["SkładnikMenu"].Rows.Count; a++)
        //                        {
        //                            wartosc = data.Tables["SkładnikMenu"].Rows[a][2].ToString();
        //                            SkładnikMenu składnik = new SkładnikMenu();
        //                            wartosc = zwrocKategorie(Convert.ToInt32(wartosc).ToString());
        //                            składnik.RodzajSM = wartosc;
        //                            składnik.NazwaSM = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //                            wartosc = data.Tables["SkładnikMenu"].Rows[a][0].ToString();
        //                            składnik.IdSM = Int16.Parse(wartosc);
        //                            wartosc = data.Tables["SkładnikMenu"].Rows[a][3].ToString();
        //                            składnik.CenaSM = Double.Parse(wartosc);
        //                            wartosc = data.Tables["SkładnikMenu"].Rows[a][4].ToString();
        //                            składnik.DataDodaniaSM = DateTime.Parse(wartosc);
        //                            listaSM.Add(składnik);
        //                        }
        //                        connection.Close();

        //                    }
        //                    catch
        //                    {
        //                        MessageBox.Show("Błąd odczytywania menu");

        //                    }
        //                    double cena = 0.0;
        //                    string produkty = "";
        //                    for (int a = 0; a < ilosc; a++)
        //                    {
        //                        foreach (var skladnik in listaSM)
        //                        {
        //                            if (skladnik.IdSM == Convert.ToInt32(split[a]))
        //                            {
        //                                cena += skladnik.CenaSM;
        //                                produkty += skladnik.NazwaSM + ", ";
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    tekst = "";
        //                    DopiszZamowienie(produkty, cena, idTarget);//wpisanie zamówienia do bazy
        //                }
        //                if (tekst == "L")//Logowanie
        //                {
        //                    asen = new ASCIIEncoding();//odpowiedz do klienta
        //                    str = Szyfrowanie.Encrypt("OK", encryptyingCode);
        //                    s.Send(asen.GetBytes(str));
        //                    b = new byte[256];
        //                    k = s.Receive(b);//odczytanie tekstu od klienta
        //                    tekst = "";

        //                    for (int i = 0; i < k; i++) tekst += Convert.ToChar(b[i]);
        //                    tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
        //                    string[] splited = tekst.Split(' ');

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
        //                            string haslo = data.Tables["Pracownicy"].Rows[a]["Hasło"].ToString();
        //                            if (haslo == splited[1])
        //                            {
        //                                bool free = true;
        //                                mut2.WaitOne();
        //                                foreach (string log in l_Zalogowani)
        //                                {
        //                                    if (log == splited[0])
        //                                    {
        //                                        free = false;
        //                                    }
        //                                }
        //                                mut2.ReleaseMutex();
        //                                if (free)
        //                                {
        //                                    string ID = data.Tables["Pracownicy"].Rows[a]["IDPracownika"].ToString();
        //                                    l_Zalogowani.Add(splited[0]);
        //                                    asen = new ASCIIEncoding();//opwoiedz do klienta
        //                                    str = Szyfrowanie.Encrypt("C", encryptyingCode);
        //                                    s.Send(asen.GetBytes(str));

        //                                    str = Szyfrowanie.Encrypt(ID, encryptyingCode);
        //                                    s.Send(asen.GetBytes(str));

        //                                    string filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt");
        //                                    s.SendFile(filename);
        //                                }
        //                                else
        //                                {
        //                                    asen = new ASCIIEncoding();//opwoiedz do klienta
        //                                    str = Szyfrowanie.Encrypt("W", encryptyingCode);
        //                                    s.Send(asen.GetBytes(str));
        //                                }

        //                            }
        //                            else
        //                            {
        //                                asen = new ASCIIEncoding();//opwoiedz do klienta
        //                                str = Szyfrowanie.Encrypt("W", encryptyingCode);
        //                                s.Send(asen.GetBytes(str));
        //                            }
        //                        }
        //                        else if (a == (data.Tables["Pracownicy"].Rows.Count) - 1)
        //                        {
        //                            asen = new ASCIIEncoding();//opwoiedz do klienta
        //                            str = Szyfrowanie.Encrypt("W", encryptyingCode);
        //                            s.Send(asen.GetBytes(str));
        //                        }
        //                    }
        //                    connection.Close();
        //                }
        //                s.Close();
        //                mut.WaitOne();
        //                l_Sockets.First<TcpListener>().Stop();
        //                l_Sockets.RemoveAt(0);
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
        //            l_Sockets.First<TcpListener>().Stop();
        //            l_Sockets.RemoveAt(0);
        //            mut.ReleaseMutex();
        //            Console.WriteLine("Error..... " + e.StackTrace);
        //        }

        //    }
        //}
    }
}
