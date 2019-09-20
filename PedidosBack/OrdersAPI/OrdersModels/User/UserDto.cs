using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersModels.UserPackage
{
    public class UserDto
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int IdPerfil { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }
    }
}
