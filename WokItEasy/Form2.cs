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
    public partial class Form2 : Form
    {
        
        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Przemek\Desktop\repozytorium\WokItEasy\WokItEasy1.mdb";
        //static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\INFORMATYKA\6\Zespołowe programowanie\WokItEasy\WokItEasy\WokItEasy1.mdb";
        List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
        //void Pokaż()
        //{
        //    string connString = source;
        //    OleDbConnection connection = new OleDbConnection(connString);
        //    connection.Open();
        //    string query = "SELECT * FROM SkładnikMenu";
        //    OleDbCommand command = new OleDbCommand(query, connection);
        //    OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
        //    DataSet data = new DataSet();
        //    AdapterTabela.Fill(data, "SkładnikMenu");
        //    string wartosc;
        //    List<Button> listaBTN = new List<Button>();
        //    //foreach(Control ctrl in this.Controls)
        //    //{
        //    //    if (ctrl is Button)
        //    //    {
        //    //        if(ctrl.Name!="button31" || ctrl.Name != "button32")
        //    //            listaBTN.Add(ctrl);
        //    //    }
        //    //}

        //    for (int a = 0; a < data.Tables["SkładnikMenu"].Rows.Count; a++)
        //    {
        //        wartosc = data.Tables["SkładnikMenu"].Rows[a][2].ToString();
        //        SkładnikMenu składnik = new SkładnikMenu();
        //        składnik.RodzajSM = wartosc;
        //        składnik.NazwaSM = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //        wartosc = data.Tables["SkładnikMenu"].Rows[a][0].ToString();
        //        składnik.IdSM = Int16.Parse(wartosc);
        //        wartosc = data.Tables["SkładnikMenu"].Rows[a][3].ToString();
        //        składnik.CenaSM = double.Parse(wartosc);
        //        wartosc = data.Tables["SkładnikMenu"].Rows[a][4].ToString();
        //        składnik.DataDodaniaSM = DateTime.Parse(wartosc);
        //        listaSM.Add(składnik);


        //        int b = 0;
        //        switch (składnik.RodzajSM)
        //        {
        //            case "Podstawa":
        //                while (listaBTN.Find(x => x.Name.Equals("button" + (b + 1).ToString())).Visible==true)
        //                {
        //                    b++;
        //                }
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 1).ToString())).Visible = true;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 1).ToString())).BackColor = Color.LightGreen;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 1).ToString())).Text = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //                break;
        //            case "Baza":
        //                while (listaBTN.Find(x => x.Name.Equals("button" + (b + 6).ToString())).Visible)
        //                {
        //                    b++;
        //                }
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 6).ToString())).Visible = true;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 6).ToString())).BackColor = Color.LightGreen;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 6).ToString())).Text = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //                break;
        //            case "Sos":
        //                while (listaBTN.Find(x => x.Name.Equals("button" + (b + 11).ToString())).Visible)
        //                {
        //                    b++;
        //                }
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 11).ToString())).Visible = true;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 11).ToString())).BackColor = Color.LightGreen;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 11).ToString())).Text = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //                break;
        //            case "Posypka":
        //                while (listaBTN.Find(x => x.Name.Equals("button" + (b + 16).ToString())).Visible)
        //                {
        //                    b++;
        //                }
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 16).ToString())).Visible = true;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 16).ToString())).BackColor = Color.LightGreen;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 16).ToString())).Text = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //                break;
        //            case "Inne":
        //                while (listaBTN.Find(x => x.Name.Equals("button" + (b + 21).ToString())).Visible)
        //                {
        //                    b++;
        //                }
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 21).ToString())).Visible = true;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 21).ToString())).BackColor = Color.LightGreen;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 21).ToString())).Text = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //                break;
        //            case "Napoje":
        //                while (listaBTN.Find(x => x.Name.Equals("button" + (b + 26).ToString())).Visible)
        //                {
        //                    b++;
        //                }
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 26).ToString())).Visible = true;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 26).ToString())).BackColor = Color.LightGreen;
        //                listaBTN.Find(x => x.Name.Equals("button" + (b + 26).ToString())).Text = data.Tables["SkładnikMenu"].Rows[a][1].ToString();
        //                break;

        //        }
        //    }
        //    connection.Close();

        //}
        public Form2()
        {
            InitializeComponent();
            //Pokaż();
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
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
            return a;
        }
        private void CreateButtons()
        {
            try
            {
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "SELECT * FROM SkładnikMenu";
                string query1 = "SELECT * FROM Kategoria";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbCommand command1 = new OleDbCommand(query1, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                OleDbDataAdapter AdapterTabela1 = new OleDbDataAdapter(command1);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "SkładnikMenu");
                AdapterTabela1.Fill(data, "Kategoria");
                string wartosc;
                for (int a = 0; a < data.Tables["SkładnikMenu"].Rows.Count; a++)
                {
                    wartosc = data.Tables["SkładnikMenu"].Rows[a][2].ToString();
                    SkładnikMenu składnik = new SkładnikMenu();
                    wartosc =  zwrocKategorie(Convert.ToInt32(wartosc).ToString());
                    //data.Tables["Kategoria"].Rows[Convert.ToInt32(wartosc) - 1][1].ToString();
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

                int ileButtonow = listaSM.Count;//w rzędzie mieści się 6 w kolumnie 6
                int ilePokazac = ileButtonow;
                int x, y;
                x = y = 0;
                if (ileButtonow > 36)
                {
                    ilePokazac = 36;
                    button1.Visible = true;
                    button2.Visible = true;
                }
                for (int a = 0; a < ilePokazac; a++)
                {
                    if (a % 6 == 0 && x != 0)
                    {
                        y += 125;
                        x = 0;
                    }
                    Button dynamicButton = new Button();
                    dynamicButton.Height = 120;
                    dynamicButton.Width = 120;
                    //dynamicButton.BackColor = Color.Red;
                    //dynamicButton.ForeColor = Color.Blue;
                    dynamicButton.Location = new Point(320 + x, 80 + y);
                    dynamicButton.Text = listaSM[a].NazwaSM ;
                    x += 125;

                    dynamicButton.Click += new EventHandler(DynamicButton_Click);

                    Controls.Add(dynamicButton);
                }

            }
            catch
            {
                MessageBox.Show("Błąd odczytywania menu");

            }
            //dynamicButton.Name = "DynamicButton";
            //dynamicButton.Font = new Font("Georgia", 16);

            // Add a Button Click Event handler

            //dynamicButton.Click += new EventHandler(DynamicButton_Click);

            // Add Button to the Form. Placement of the Button

            // will be based on the Location and Size of button

            //Controls.Add(dynamicButton);

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            CreateButtons();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(button1.Text);
            Cena(button1.Text);
        }
        private void DynamicButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            listBox1.Items.Add(clickedButton.Text);
            Cena(clickedButton.Text);
            //MessageBox.Show("Dynamic button is clicked");

        }
        private void Cena(string nazwa)
        {
            string cena = label2.Text;
            double a,b;
            a = double.Parse(listaSM.Find(x => x.NazwaSM.Equals(nazwa)).CenaSM.ToString());
            b = double.Parse(cena);
            label2.Text = (a + b).ToString();
        }
        private void CenaMinus(string nazwa)
        {
            string cena = label2.Text;
            double a, b;
            a = double.Parse(listaSM.Find(x => x.NazwaSM.Equals(nazwa)).CenaSM.ToString());
            b = double.Parse(cena);
            label2.Text = (b- a).ToString();
        }
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                CenaMinus(listBox1.SelectedItem.ToString());
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
            catch { }
        }
        
        private void button31_Click(object sender, EventArgs e)
        {
            //Dodaj zamówienie
            try
            {
                string connString = source;
                OleDbConnection connection = new OleDbConnection(connString);
                connection.Open();
                string query = "INSERT INTO Zamówienia (DataZamówienia, KwotaZamówienia, IDSM, IDObsługi, Online, Rozliczone) VALUES('";
                query += DateTime.Now.ToString();
                query += "', '";
                query += Convert.ToDouble(label2.Text);
                query += "', '";
                query += IDSM();
                query += "', ";
                query += Form1.ObecnieZalogowanyUżytkownik.Id.ToString();
                query += ", ";
                if (button32.Text == "Online")
                    query += "True";
                else
                    query += "False";

                query += ", ";
                query += "False);";
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
                DataSet data = new DataSet();
                AdapterTabela.Fill(data, "Zamówienia");
                label2.Text = "0";


                connection.Close();
            }
            catch
            {
                MessageBox.Show("Problem z BD");
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //online/gotówka
            if (button32.Text == "Online")
                button32.Text = "Gotówka";
            else
                button32.Text = "Online";
        }
        string IDSM()//funkcja ma wypisywać co było w zamówieniu
        {
            string x="";
            for(int a=0;a< listBox1.Items.Count; a++)
            {
                x += listBox1.GetItemText(listBox1.Items[a]);// ToString();
                x += ", ";
            }
            for (int a = 0; a < listBox1.Items.Count; a++)//tyle razy bo czasami zostawało?
            {
                listBox1.Items.Remove(listBox1.Items[a]);
            }
            for (int a = 0; a < listBox1.Items.Count; a++)
            {
                listBox1.Items.Remove(listBox1.Items[a]);
            }
            for (int a = 0; a < listBox1.Items.Count; a++)
            {
                listBox1.Items.Remove(listBox1.Items[a]);
            }
            return x;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(button2.Text);
            Cena(button2.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //listBox1.Items.Add(button6.Text);
            //Cena(button6.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //listBox1.Items.Add(button11.Text);
            //Cena(button11.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    listBox1.Items.Add(button7.Text);
        //    Cena(button7.Text);
        //}

        //private void button12_Click(object sender, EventArgs e)
        //{
        //    listBox1.Items.Add(button12.Text);
        //    Cena(button12.Text);
        //}

        //private void button16_Click(object sender, EventArgs e)
        //{
        //    listBox1.Items.Add(button16.Text);
        //    Cena(button16.Text);
        //}
    }
}
