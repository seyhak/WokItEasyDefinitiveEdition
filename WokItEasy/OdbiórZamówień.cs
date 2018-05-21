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
    public partial class OdbiórZamówień : Form
    {
        bool workWorkMoneyMade = true;
        int screenCount = 0;
        Thread thr;
        public OdbiórZamówień()
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
        void Pokazuj()
        {
            int framer = 0;
            while (workWorkMoneyMade)
            {
                framer++;
                if (framer % 200 == 0)
                {
                    if (framer%1000000==0)//wchodzi tylko jeżeli pojawiła się zmiana
                    {
                        //Clear();
                        Działaj();
                        framer = 1;
                        Thread.Sleep(100);
                    }
                }
            }
        }
        void Clear(short from, int a)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<short, int>(Clear), new object[] { from, a });
                    return;
                }
                else
                {
                    switch (from)
                    {
                        case 1:

                            listBox1.Items.Remove(a);
                            break;
                        case 2:
                            listBox2.Items.Remove(a);
                            break;
                    }
                }
            }
            catch
            {

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
        
        void Działaj()
        {
            foreach (Zamówienie zamówienie in Zamówienie.GetObecneZamówieniaDoOdebrania())
            {
                if (!zamówienie.Odebrane && zamówienie.Wykonane)//do odbioru
                {
                    if (!listBox2.Items.Contains(zamówienie.IdZamówienia))
                    {
                        Add(2, zamówienie.IdZamówienia);
                        Clear(1, zamówienie.IdZamówienia);
                    }
                }
                if (!zamówienie.Odebrane && !zamówienie.Wykonane)//w trakcie
                {
                    if (!listBox1.Items.Contains(zamówienie.IdZamówienia))
                        Add(1, zamówienie.IdZamówienia);
                }
            }
        }
        void Add(short a,int what)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<short,int>(Add), new object[] { a,what });
                    return;
                }
                else
                {
                    switch (a)
                    {
                        case 1:
                            listBox1.Items.Add(what);
                            break;
                        case 2:
                            listBox2.Items.Add(what);
                            break;
                    }
                }
            }
            catch
            {

            }

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(listBox1.SelectedItem.ToString());
                Zamówienie.PrzekażDoOdebraniaZamówienie(a);
                Clear(1, a);
                Clear(1, a);
                Clear(1, a);
            }
            catch { }
        }

        private void OdbiórZamówień_MouseClick(object sender, MouseEventArgs e)
        {
            workWorkMoneyMade = false;
            thr.Abort();
            this.Close();
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)//usuń zamówienie (odebrane)
        {
            try
            {
                int a = Convert.ToInt32(listBox2.SelectedItem.ToString());
                Zamówienie.OdbrierzZamówienie(a);
                Clear(2, a);
                Clear(2, a);
                Clear(2, a);
            }
            catch { }
        }
    }
}
