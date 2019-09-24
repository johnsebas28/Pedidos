using Microsoft.Extensions.Options;
using OrdersDAL;
using OrdersModels;
using OrdersModels.UserPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersAPI.Manager
{
    public class UserManager
    {

        public object GetAllUser() {
            DALManager DAL = new DALManager();
            DAL.openDatabase();
            ArrayList list = new ArrayList();
            SqlParameter p1 = new SqlParameter();
            p1.DbType = DbType.Int16;
            p1.Direction = ParameterDirection.Input;
            p1.ParameterName = "@Id_User";
            p1.SqlDbType = SqlDbType.Int;
            p1.Value = 1;

            int CodError = 0;
            string ErrorMessage = "";
            list.Add(p1);
            object ret = DAL.execSP("User_SEL_ALL", ref CodError, ref ErrorMessage);
            if (CodError != 0)
            {
                return null;
            }
           
            return ret;
        }
        public List<User> GetAllUserList(ref int CodError, ref string ErrorMessage)
        {
            DALManager DAL = new DALManager();
            DAL.openDatabase();
            ArrayList list = new ArrayList();
            //SqlParameter p1 = new SqlParameter();
            //p1.DbType = DbType.Int16;
            //p1.Direction = ParameterDirection.Input;
            //p1.ParameterName = "@Id_User";
            //p1.SqlDbType = SqlDbType.Int;
            //p1.Value = 1;
            //list.Add(p1);

            object ret = DAL.execSP("User_SEL_ALL", ref CodError, ref ErrorMessage);
            if (CodError < 0)
            {
                return null;
            }
            var j = (DataSet)ret;
            int rows = j.Tables[0].Rows.Count;
            List<User> userList = Utils.loadObjects<User>(j.Tables[0]);
            return userList;
        }
        public User GetUserById(string idUser, ref int CodError, ref string ErrorMessage)
        {
            DALManager DAL = new DALManager();
            DAL.openDatabase();
            ArrayList Parameters = new ArrayList();
            SqlParameter p1 = new SqlParameter();
            p1.DbType = DbType.Int16;
            p1.Direction = ParameterDirection.Input;
            p1.ParameterName = "@IdUser";
            p1.SqlDbType = SqlDbType.VarChar;
            p1.Value = idUser;
            Parameters.Add(p1);

            object ret = DAL.execSP("User_SEL_ALL_By_Id_User", ref CodError, ref ErrorMessage, Parameters);
            if (CodError < 0)
            {
                return null;
            }
            var j = (DataSet)ret;
            int rows = j.Tables[0].Rows.Count;
            User user;
            if (rows > 0)
            {
               user = Utils.loadObject<User>(j.Tables[0]);
            }
            else {
                user = null;
            }
           
            return user;
        }

        public User GetUserByLogin(string login, ref int CodError, ref string ErrorMessage)
        {
            DALManager DAL = new DALManager();
            DAL.openDatabase();
            ArrayList Parameters = new ArrayList();
            SqlParameter p1 = new SqlParameter();
            p1.DbType = DbType.String;
            p1.Direction = ParameterDirection.Input;
            p1.ParameterName = "@Login";
            p1.SqlDbType = SqlDbType.NVarChar;
            p1.Value = login;
            Parameters.Add(p1);

            object ret = DAL.execSP("User_SEL_By_Login", ref CodError, ref ErrorMessage, Parameters);
            if (CodError < 0)
            {
                return null;
            }
            var j = (DataSet)ret;
            int rows = j.Tables[0].Rows.Count;
            User user;
            if (rows > 0)
            {
                user = Utils.loadObject<User>(j.Tables[0]);
            }
            else
            {
                user = null;
            }

            return user;
        }
        public string InsertUser(UserInsert MyUser,ref int errorCode,ref string errorMessage)
        {
            try
            {
                string idUSer = Guid.NewGuid().ToString();
                DALManager DAL = new DALManager();
                DAL.openDatabase();
                ArrayList Parameters = new ArrayList();
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@IdPerfil", SqlDbType = SqlDbType.Int, Value = MyUser.idPerfil });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Identification", SqlDbType = SqlDbType.NVarChar, Value = MyUser.identification });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Name", SqlDbType = SqlDbType.NVarChar, Value = MyUser.name });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar, Value = MyUser.lastName });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Address", SqlDbType = SqlDbType.NVarChar, Value = MyUser.address });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@NickName", SqlDbType = SqlDbType.NVarChar, Value = MyUser.nickName });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar, Value = MyUser.password });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value = MyUser.email });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Phone", SqlDbType = SqlDbType.NVarChar, Value = MyUser.phone });

                object ret = DAL.execSP("User_INS", ref errorCode, ref errorMessage, Parameters);
                if (errorCode != 0)
                {
                    return null;
                }
                return idUSer;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public bool UpdateUser(string IdUser,UserUpdate MyUser, ref int errorCode, ref string errorMessage)
        {
            try
            {
                DALManager DAL = new DALManager();
                DAL.openDatabase();
                ArrayList Parameters = new ArrayList();
                Parameters.Add(new SqlParameter { DbType = DbType.Int32, Direction = ParameterDirection.Input, ParameterName = "@idUser", SqlDbType = SqlDbType.NVarChar, Value = IdUser });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Identification", SqlDbType = SqlDbType.NVarChar, Value = MyUser.identification });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Name", SqlDbType = SqlDbType.NVarChar, Value = MyUser.name });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar, Value = MyUser.lastName });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Address", SqlDbType = SqlDbType.NVarChar, Value = MyUser.address });
                //Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@NickName", SqlDbType = SqlDbType.NVarChar, Value = MyUser.nickName });
                //Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar, Value = MyUser.password });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value = MyUser.email });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Phone", SqlDbType = SqlDbType.NVarChar, Value = MyUser.phone });
                Parameters.Add(new SqlParameter { DbType = DbType.Boolean, Direction = ParameterDirection.Input, ParameterName = "@IsActive", SqlDbType = SqlDbType.Bit, Value = MyUser.isActive });

                object ret = DAL.execSP("User_UPD", ref errorCode, ref errorMessage, Parameters);
                if (errorCode != 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool DeleteUser(string IdUser, ref int errorCode, ref string errorMessage)
        {
            try
            {
                DALManager DAL = new DALManager();
                DAL.openDatabase();
                ArrayList Parameters = new ArrayList();
                Parameters.Add(new SqlParameter { DbType = DbType.Int32, Direction = ParameterDirection.Input, ParameterName = "@idUser", SqlDbType = SqlDbType.NVarChar, Value = IdUser });

                object ret = DAL.execSP("User_DEL", ref errorCode, ref errorMessage, Parameters);
                if (errorCode != 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
