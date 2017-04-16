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
    public partial class Rozliczenie : Form
    {
        public Rozliczenie()
        {
            InitializeComponent();
        }

        private void Rozliczenie_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
