using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_core_api.Models
{
    public class UserGroups
    {
        public int? idUser { get; set; }
        public string nameUser { get; set; }
        public string email { get; set; }
        public int? idGroup { get; set; }
        public string nameGroup { get; set; }
    }
}
