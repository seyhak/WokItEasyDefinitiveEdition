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

        static string source = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.mdb");
        List<SkładnikMenu> listaSM = new List<SkładnikMenu>();
        List<SkładnikMenu> listaPOM = new List<SkładnikMenu>();
        List<int> listaIlePozycjiNaStrone = new List<int>();
        List<Button> listaButtonowNaStronie = new List<Button>();
        int ktoraStronaOgolnie = 0;
        int idStartowe;

        public Form2()
        {
            InitializeComponent();
            //Pokaż();
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        void ZbudujListePozycji()
        {
            try
            {
               listaSM = SkładnikMenu.Zbuduj(source);
            }
            catch
            {
                MessageBox.Show("Błąd odczytywania menu");

            }
        }
        private void Podziel()
        {
            for (int i = 1; i < 12; i++)
            {
                int ile = 0;
                string tekst="";
                switch(i)
                {
                    case 1:
                        {
                            tekst = "Sos";
                            break;
                        }
                    case 2:
                        {
                            tekst = "Posypka";
                            break;
                        }
                    case 3:
                        {
                            tekst = "Podstawa";
                            break;
                        }
                    case 4:
                        {
                            tekst = "Proteina";
                            break;
                        }
                    case 5:
                        {
                            tekst = "Napoje";
                            break;
                        }
                    case 6:
                        {
                            tekst = "Inne";
                            break;
                        }
                    case 7:
                        {
                            tekst = "Zupa";
                            break;
                        }
                    case 8:
                        {
                            tekst = "Piwo";
                            break;
                        }
                    case 9:
                        {
                            tekst = "Wino";
                            break;
                        }
                    case 11:
                        {
                            tekst = "Wódka";
                            break;
                        }
                }
                int ktoraStronaNaLiscie = 0;
                foreach (SkładnikMenu sm in listaSM)
                {
                    
                    if (sm.RodzajSM==tekst)
                    {
                        listaPOM.Add(sm);
                        ile++;

                        if (ile == 36)
                        {
                            listaIlePozycjiNaStrone.Add(ile);
                            ile = 0;
                            ktoraStronaNaLiscie++;
                        }
                    }
                }
                listaIlePozycjiNaStrone.Add(ile);
            }
        }
        private void StwórzButtony()//todo
        {
            try
            {
                int ileButtonow = listaIlePozycjiNaStrone[ktoraStronaOgolnie];//w rzędzie mieści się 6 w kolumnie 6
                int ilePokazac = ileButtonow;
                int x, y;
                x = y = 0;
                //int odKtoregoIDZaczac = ktoraStronaOgolnie * 36;
                idStartowe = 0;
                for (int i = 0; i < ktoraStronaOgolnie; i++)
                {
                    idStartowe += listaIlePozycjiNaStrone[i];
                }
                for (int a = idStartowe; a < idStartowe + ilePokazac; a++)
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
                    dynamicButton.Text = listaPOM[a].NazwaSM;
                    x += 125;

                    dynamicButton.Click += new EventHandler(DynamicButton_Click);
                    listaButtonowNaStronie.Add(dynamicButton);
                    Controls.Add(dynamicButton);
                }
            }

            catch
            {
                if (listaIlePozycjiNaStrone[ktoraStronaOgolnie] == 0)
                    MessageBox.Show("Brak pozycji do wyświetlenia");
            }
        }
        //private void StwórzButtony()//todo
        //{
        //    try
        //    {
        //        int ileButtonow = listaIlePozycjiNaStrone[ktoraStronaOgolnie];//w rzędzie mieści się 6 w kolumnie 6
        //        int ilePokazac = ileButtonow;
        //        int x, y;
        //        x = y = 0;
        //        int odKtoregoIDZaczac = ktoraStronaOgolnie*36;
        //        for (int a = 0; a < ilePokazac; a++)
        //        {
        //            if (a % 6 == 0 && x != 0)
        //            {
        //                y += 125;
        //                x = 0;
        //            }
        //            Button dynamicButton = new Button();
        //            dynamicButton.Height = 120;
        //            dynamicButton.Width = 120;
        //            //dynamicButton.BackColor = Color.Red;
        //            //dynamicButton.ForeColor = Color.Blue;
        //            dynamicButton.Location = new Point(320 + x, 80 + y);
        //            dynamicButton.Text = listaSM[a].NazwaSM ;
        //            x += 125;

        //            dynamicButton.Click += new EventHandler(DynamicButton_Click);
        //            listaButtonowNaStronie.Add(dynamicButton);
        //            Controls.Add(dynamicButton);
        //        }

        //    }
        //    catch
        //    {
        //        if(listaIlePozycjiNaStrone[ktoraStronaOgolnie]==0)
        //            MessageBox.Show("Brak pozycji do wyświetlenia");

        //    }
        //}
        void UsunButtony()
        {
            foreach (Button btn in listaButtonowNaStronie)
            {
                this.Controls.Remove(btn);
                    }
        }
        void CzyZaDuzoPozycji()
        {
            
            if (listaSM.Count > 36)
            {

                button1.Visible = true;
                button2.Visible = true;
            }
        }
        void StworzListeStron()
        {
            int ile = 0;
            int ktoraStronaNaLiscie = 0;
            foreach(SkładnikMenu sm in listaSM)
            {
                ile++;

                if (ile == 36)
                {
                    listaIlePozycjiNaStrone.Add(ile);
                    ile = 0;
                    ktoraStronaNaLiscie++;
                }
            }
            listaIlePozycjiNaStrone.Add(ile);
        }
        void GenerujPrzewijanie()
        {
            StwórzButtony();

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            ZbudujListePozycji();
            CzyZaDuzoPozycji();
            Podziel();
            //StworzListeStron();
            StwórzButtony();
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
            bool online;
            if (button32.Text == "Online")
                online = true;
            else
                online = false;


            List<string> listStr = new List<string>();
            for (int a = 0; a < listBox1.Items.Count; a++)
            {
                listStr.Add(listBox1.GetItemText(listBox1.Items[a]));// ToString();
            }
            Zamówienie.DopiszZamowieniaZListyID(listaSM, listStr, Convert.ToDouble(label2.Text), source, Form1.ObecnieZalogowanyUżytkownik.Id, online, false);
            label2.Text = "0";
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
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //online/gotówka
            if (button32.Text == "Online")
                button32.Text = "Gotówka";
            else
                button32.Text = "Online";
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            //lewo
            if (ktoraStronaOgolnie >0)
            {
                ktoraStronaOgolnie--;
                UsunButtony();
                StwórzButtony();
            }
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {
            //prawo
            if (ktoraStronaOgolnie < listaIlePozycjiNaStrone.Count-1)
            {
                ktoraStronaOgolnie++;
                UsunButtony();
                StwórzButtony();
            }
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
