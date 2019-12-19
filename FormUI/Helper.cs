using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormUI
{
    public static class Helper
    {
        public static string Conectare()
        {
            return ConfigurationManager.ConnectionStrings["Proiect"].ConnectionString;
        }
    }
}
