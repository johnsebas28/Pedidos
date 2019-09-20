using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OrdersDAL
{
    public class DALManager
    {
        public Interfaces.IConnection Connection { get; set; }
        public DALManager() {
            Connection = new ConnectionSQL();
        }
        public bool openDatabase() {
            return Connection.OpenConnection();
        }

        public object execSP(string SPName, ref int codError, ref string ErrorMessage, ArrayList arrayParams = null) {
            return Connection.ExecuteStoreProcedure(SPName, ref codError, ref ErrorMessage, arrayParams);
        }
    }
}
