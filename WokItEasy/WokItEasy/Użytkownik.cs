using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WokItEasy
{
    class Użytkownik
    {
        string imie,nazwisko,rola;
        bool kierownik = false;
        int id;
      
        public string Imie { get => imie; set => imie = value; }
        public string Nazwisko { get => nazwisko; set => nazwisko = value; }
        public bool Kierownik { get => kierownik; set => kierownik = value; }
        public int Id { get => id; set => id = value; }
        public string Rola { get => rola; set => rola = value; }
    }
}
