using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_core_api.Utils
{
    public class Helper
    {
        public static bool ValidaEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, ("(?<user>[^@]+)@(?<host>.+)"));
        }
    }
}
