using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace WokItEasy
{
    public partial class ObecneZamówienia : Form
    {
        string godzina;
        bool kuchnia = false;
        bool workWorkMoneyMade = true;
        int screenCount=0;
        Thread thr;
        List<Button> listaBtnów = new List<Button>();
        bool buttonsChanged = true;
        public ObecneZamówienia()
        {
            InitializeComponent();
            if (Screen.AllScreens.Length > 1)
                screenCount = 1;
            this.Location = Screen.AllScreens[screenCount].WorkingArea.Location;
            //this.Location = new Point(0, 0);
            this.Size = Screen.AllScreens[screenCount].WorkingArea.Size;
            SkładnikMenu.Zbuduj();
            thr = new Thread(this.Pokazuj);
            thr.Start();
        }
        public ObecneZamówienia(bool kuchnia)
        {
            InitializeComponent();

            this.kuchnia = kuchnia;
            if (Screen.AllScreens.Length > 1)
                screenCount = 1;
            this.Location = Screen.AllScreens[screenCount].WorkingArea.Location;
            //this.Location = new Point(0, 0);
            this.Size = Screen.AllScreens[screenCount].WorkingArea.Size;
            SkładnikMenu.Zbuduj();
            thr = new Thread(this.Pokazuj);
            thr.Start();
        }
        private void ObecneZamówienia_MouseClick(object sender, MouseEventArgs e)
        {
            workWorkMoneyMade = false;
            thr.Abort();
            this.Close();
        }
        void Pokazuj()
        {
            int framer = 0;
            while (workWorkMoneyMade)
            {
                framer++;
                if (framer % 200 == 0)
                {
                    SetHour(DateTime.Now.ToString());
                    if (buttonsChanged)//wchodzi tylko jeżeli pojawiła się zmiana
                    {
                        buttonsChanged = !buttonsChanged;
                        Remove(listaBtnów);
                        listaBtnów = new List<Button>();
                        Thread.Sleep(100);
                        Działaj();
                    }
                }
            }
        }
        
        void Remove(List<Button> c)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<List<Button>>(Remove), new object[] { c });
                return;
            }
            else
            {
                try
                {
                    foreach (Button b in c)
                    {
                        //listaBtnów.Remove(b);
                        this.Controls.Remove(b);
                    }
                }
                catch
                {
                    //System.Diagnostics.Debug.WriteLine(c + " removed");
                }
            }

        }
        private void StwórzButton(int id, string what, DateTime when,int x,int y)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int,string,DateTime,int,int>(StwórzButton), new object[] { id,what,when,x,y });
                return;
            }
            else
            {
               
                    try
                    {
                        string[] whatSpaces = what.Split(',');
                        Button dynamicButton = new Button();

                        dynamicButton.Width = 200;
                        dynamicButton.Height = this.Size.Width - 105;
                        dynamicButton.Font = new Font("Microsoft Sans Serif", 12);
                        //dynamicButton.BackColor = Color.Red;
                        //dynamicButton.ForeColor = Color.Blue;
                        dynamicButton.Location = new Point(x, y);
                        dynamicButton.Text = id.ToString() + Environment.NewLine + " " + when.ToString() + Environment.NewLine + Environment.NewLine;
                        foreach (string s in whatSpaces)
                        {
                            dynamicButton.Text += s + Environment.NewLine;/* what.Trim(new Char[] { ','});*/
                        }
                        dynamicButton.Tag = id;
                        dynamicButton.TextAlign = ContentAlignment.TopCenter;
                        dynamicButton.Click += new EventHandler(DynamicButton_Click);
                        this.Controls.Add(dynamicButton);
                        listaBtnów.Add(dynamicButton);
                    }
                    catch
                    {
                        //MessageBox.Show("Brak pozycji do wyświetlenia");
                    }
            }
          
        }
        private void DynamicButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Zamówienie.WykonajZamówienie(Convert.ToInt32(clickedButton.Tag),kuchnia);
            buttonsChanged = true;

        }
        void Działaj()
        {
            int a = 0;//ile w rzędzie
            int ileMaxWrzędzie;
            int ileMaxWkolumnie;
            int x, y;
            int maxX, maxY;
            maxX = this.Size.Width;
            maxY = this.Size.Height;
            ileMaxWrzędzie = maxX / 205;
            ileMaxWkolumnie = maxY / 205;
            ileMaxWkolumnie = 1;
            int Max = ileMaxWkolumnie * ileMaxWrzędzie;//maxymalna ilość btn na ekran?
            x = y = 0;
            y = 50;
            //y = maxY;
            foreach (Zamówienie zamówienie in Zamówienie.GetObecneZamówienia(kuchnia))
            {
                if (a >= Max)
                    break;
                string what = "";
                what = SkładnikMenu.GetNazwyZIdZPrzecinkami(zamówienie.IdZamówień, kuchnia);
                if (((what != "")&&kuchnia)||!kuchnia)
                {
                    

                    StwórzButton(zamówienie.IdZamówienia, what, zamówienie.DataZamówienia, x, y);
                    a++;
                    if (a % ileMaxWrzędzie == 0 && x != 0)//jeżeli w rzędzie jest już wystarczająco
                    {
                        y += 205;
                        x = 0;
                    }
                    else
                        x += 205;
                }
            }
            SetCount(Max);
        }
        void SetCount(int M)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(SetCount), new object[] { M });
                return;
            }
            else
            {
                if ((Zamówienie.GetObecneZamówienia(kuchnia).Count - M) > 0) {
                    label1.Text = "+" + (Zamówienie.GetObecneZamówienia(kuchnia).Count - M);
                }
                else
                    label1.Text = "+" +0;
            }

        }
        void SetHour(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetHour), new object[] { text });
                return;
            }
            else
            {
                try
                {
                    label2.Text = DateTime.Now.ToString();
                }
                catch { }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
