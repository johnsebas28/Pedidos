using OrdersDAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace OrdersDAL
{
    public class ConnectionSQL : IConnection 
    {
        private SqlConnection sqlConnection;
 
        public bool isOpen { get; set; }

        public bool OpenConnection()
        {
            try
            {
                sqlConnection = new SqlConnection("Password=Diosesamor1**;Persist Security Info=True;User ID=sa;Initial Catalog=CustomOrders;Data Source=LAP-JPALACIOS\\JSPV2SQLEXPRESS");
                sqlConnection.Open();
                isOpen = true;
                return true;
            }
            catch (Exception ex)
            {

                //TODO LOG
                sqlConnection.Close();
                sqlConnection.Dispose();
                isOpen = false;
                return false;
                //TODO Clase to respond
            }

        }
        public bool CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                isOpen = false;
                return true;
            }
            catch (Exception)
            {
                //TODO LOG
                sqlConnection.Dispose();
                //TODO Clase to respond
                return false;
            }
            
        }

        public object ExecuteStoreProcedure(string StoreProcedure, ref int CodError, ref string MessageError, ArrayList parameters = null)
        {
            try
            {
                if (isOpen)
                {
                    SqlDataAdapter dataAdapter;
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Connection = sqlConnection;
                    command.CommandText = StoreProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    SqlParameter p1 = new SqlParameter();
                    p1.DbType = DbType.Int16;
                    p1.Direction = ParameterDirection.Output;
                    p1.ParameterName = "@intError";
                    p1.SqlDbType = SqlDbType.Int;

                    command.Parameters.Add(p1);

                    SqlParameter p2 = new SqlParameter();
                    p2.DbType = DbType.String;
                    p2.Direction = ParameterDirection.Output;
                    p2.ParameterName = "@strError";
                    p2.SqlDbType = SqlDbType.VarChar;
                    p2.Size = 300;
                    command.Parameters.Add(p2);

                    dataAdapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds);

                    CodError = (int) command.Parameters["@intError"].Value;
                    MessageError = (string)command.Parameters["@strError"].Value;
                    return ds;
                }
                else {
                    return null;
                }               
            }
            catch (Exception ex)
            {
                //TODO LOG
                CodError = -99;
                MessageError = ex.Message;
                return null;
            }
        }


    }
}
