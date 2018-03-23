using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WokItEasy
{
    public static class LengthConverter
    {
        public static string Convert(int a)
        {
            string str;
            if (a < 10)
            {
                return "00" + a.ToString();
            }
            else
            {
                if(a<100)
                {
                    return "0" + a.ToString();
                }
                else
                {
                    return a.ToString();
                }
            }
        }
    


    }
    
}
