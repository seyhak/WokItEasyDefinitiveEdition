using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WokItEasy
{
    class Zamówienie
    {
        int idZamówienia, kwotaZamówienia, idObsługi;
        DateTime dataZamówienia;
        bool online;
        string idZamówień;

        public int IdZamówienia { get => idZamówienia; set => idZamówienia = value; }
        public int KwotaZamówienia { get => kwotaZamówienia; set => kwotaZamówienia = value; }
        public int IdObsługi { get => idObsługi; set => idObsługi = value; }
        public DateTime DataZamówienia { get => dataZamówienia; set => dataZamówienia = value; }
        public bool Online { get => online; set => online = value; }
        public string IdZamówień { get => idZamówień; set => idZamówień = value; }
    }
}
