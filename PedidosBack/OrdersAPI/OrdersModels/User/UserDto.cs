using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersModels.UserPackage
{
    public class UserDto
    {
        public string idUser { get; set; }
        public string identification { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string nickName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public bool isActive { get; set; }
    }
}
