﻿using System;
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
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
            //this.Location = new Point(0, 0);
            this.Size = Screen.AllScreens[1].WorkingArea.Size;
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
                        foreach(Control c in this.Controls)
                        {
                            if(c is Button)
                            {
                                this.Controls.Remove(c);
                            }

                        }
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
                    dynamicButton.Location = new Point(x, y);
                    dynamicButton.Text = id.ToString() + Environment.NewLine + " " + when.ToString() +Environment.NewLine + Environment.NewLine + Environment.NewLine + what.Trim(new Char[] { ','});
                    dynamicButton.Tag = id;
                    dynamicButton.TextAlign = ContentAlignment.TopCenter;
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
            int a = 0;//ile w rzędzie
            int ileMaxWrzędzie;
            int Max = 10;//maxymalna ilość btn na ekran?
            int x, y;
            int maxX, maxY;
            maxX = this.Size.Width;
            maxY = this.Size.Height;
            ileMaxWrzędzie = maxX / 205;
            x = y = 0;
            y = 100;
            //y = maxY;
            foreach (Zamówienie zamówienie in Zamówienie.GetObecneZamówienia())
            {
              
                StwórzButton(zamówienie.IdZamówienia, SkładnikMenu.GetNazwyZIdZPrzecinkami(zamówienie.IdZamówień), zamówienie.DataZamówienia, x, y);
                if (a % ileMaxWrzędzie == 0 && x != 0)
                {
                    y += 205;
                    x = 0;
                }
                else
                    x += 205;
                a++;
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
