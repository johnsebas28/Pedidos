using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OrdersDAL.Interfaces
{
    public interface IConnection
    {
        bool OpenConnection();
        bool CloseConnection();
        object ExecuteStoreProcedure(string StoreProcedure,ref int CodError, ref string stringMessage, ArrayList parameters);
       
    }
}
