using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OrdersModels
{
    public class OneDataTransfer<T> where T : class
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }

}
