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
        bool workWorkMoneyMade = true;
        Thread thr;
        bool buttonsChanged = true;
        public ObecneZamówienia()
        {
            InitializeComponent();
            SkładnikMenu.Zbuduj();
            thr = new Thread(this.Pokazuj);
            thr.Start();
        }
        private void ObecneZamówienia_MouseClick(object sender, MouseEventArgs e)
        {
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
                        Działaj();
                        buttonsChanged = !buttonsChanged;
                    }
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
                    Button dynamicButton = new Button();
                    dynamicButton.Height = 200;
                    dynamicButton.Width = 200;
                    dynamicButton.Font = new Font("Microsoft Sans Serif", 12);
                    //dynamicButton.BackColor = Color.Red;
                    //dynamicButton.ForeColor = Color.Blue;
                    dynamicButton.Location = new Point(400 + x, 400 + y);
                    dynamicButton.Text = id.ToString() + Environment.NewLine + " " + when.ToString() +Environment.NewLine + what;
                    dynamicButton.Tag = id;
                    dynamicButton.Click += new EventHandler(DynamicButton_Click);
                    Controls.Add(dynamicButton);
                }
                catch
                {
                    MessageBox.Show("Brak pozycji do wyświetlenia");
                }
            }
          
        }
        private void DynamicButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Zamówienie.WykonajZamówienie(Convert.ToInt32(clickedButton.Tag));
            this.Controls.Remove(clickedButton);
            buttonsChanged = true;

        }
        void Działaj()
        {
            int a = 0;
            int Max = 10;
            int x, y;
            x = y = 0;

            foreach (Zamówienie zamówienie in Zamówienie.GetObecneZamówienia())
            {
                //if (Screen.AllScreens.Length > 1)
                //{
                //    oz.Location = Screen.AllScreens[1].WorkingArea.Location;
                //}
                //else
                //{
                //    Screen.PrimaryScreen.Bounds.Width;
                //    Screen.PrimaryScreen.Bounds.Height;
                //    Screen.PrimaryScreen.Bounds.Size;
                //}
                StwórzButton(zamówienie.IdZamówienia, SkładnikMenu.GetNazwyZIdZPrzecinkami(zamówienie.IdZamówień), zamówienie.DataZamówienia, x, y);
                if (a % 6 == 0 && x != 0)
                {
                    y += 125;
                    x = 0;
                }
                x += 125;
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
                label2.Text = DateTime.Now.ToString();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
