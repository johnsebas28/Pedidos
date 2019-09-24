using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersModels.UserPackage
{
    public class User
    {
        public int IdUser { get; set; }
        public int IdPerfil { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Active { get; set; }
    }
}
