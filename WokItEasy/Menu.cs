using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WokItEasy
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dodaj danie
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //usun danie
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //edytuj danie 
        }
    }
}
