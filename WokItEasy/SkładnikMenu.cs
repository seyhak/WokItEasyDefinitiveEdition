using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WokItEasy
{
    class SkładnikMenu
    {
        enum Rodzaj
        {
            Podstawa,Baza,Sos,Posypka,Inne,Napoje,
        }
        private int idSM;
        private string nazwaSM;
        private string rodzajSM;
        private double cenaSM;
        private DateTime dataDodaniaSM;

        public DateTime DataDodaniaSM { get => dataDodaniaSM; set => dataDodaniaSM = value; }
        public double CenaSM { get => cenaSM; set => cenaSM = value; }
        public string NazwaSM { get => nazwaSM; set => nazwaSM = value; }
        public int IdSM { get => idSM; set => idSM = value; }
        public string RodzajSM { get => rodzajSM; set => rodzajSM = value; }
    }
}
