﻿using Microsoft.Extensions.Options;
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
        private  readonly IOptions<AppSettings> appSettings;

        public UserManager(IOptions<AppSettings> app)
        {
            appSettings = app;
        }
        public object GetAllUser() {
            var connectionString = appSettings.Value.DNS;
            DALManager DAL = new DALManager();
            DAL.openDatabase(connectionString);
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
        public DataTransfer<User> GetAllUserList()
        {
            int CodError = 0;
            string ErrorMessage = "";
            var connectionString = appSettings.Value.DNS;
            DataTransfer<User> ToUser = new DataTransfer<User>();
            DALManager DAL = new DALManager();
            DAL.openDatabase(connectionString);
            ArrayList list = new ArrayList();
            SqlParameter p1 = new SqlParameter();
            p1.DbType = DbType.Int16;
            p1.Direction = ParameterDirection.Input;
            p1.ParameterName = "@Id_User";
            p1.SqlDbType = SqlDbType.Int;
            p1.Value = 1;
            list.Add(p1);

            object ret = DAL.execSP("User_SEL_ALL", ref CodError, ref ErrorMessage);
            if (CodError < 0)
            {
                ToUser.code = CodError;
                ToUser.message = ErrorMessage;
                return ToUser;
            }
            var j = (DataSet)ret;
            int rows = j.Tables[0].Rows.Count;
            List<User> userList = Utils.loadObjects<User>(j.Tables[0]);
            ToUser.lsdata = userList;
            return ToUser;
        }
        public DataTransfer<User> GetUserById(string idUser)
        {
            int CodError = 0;
            string ErrorMessage = "";
            var connectionString = appSettings.Value.DNS;
            DataTransfer<User> ToUser = new DataTransfer<User>();
            DALManager DAL = new DALManager();
            DAL.openDatabase(connectionString);
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
                ToUser.code = CodError;
                ToUser.message = ErrorMessage;
                return ToUser;
            }
            var j = (DataSet)ret;
            int rows = j.Tables[0].Rows.Count;
            User user = Utils.loadObject<User>(j.Tables[0]);
            ToUser.data = user;
            return ToUser;
        }

        public string InsertUser(UserInsert MyUser,ref int errorCode,ref string errorMessage)
        {
            try
            {
                string idUSer = Guid.NewGuid().ToString();
                var connectionString = appSettings.Value.DNS;
                DALManager DAL = new DALManager();
                DAL.openDatabase(connectionString);
                ArrayList Parameters = new ArrayList();
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@UserId", SqlDbType = SqlDbType.NVarChar, Value = idUSer });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Identification", SqlDbType = SqlDbType.NVarChar, Value = MyUser.identification });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Name", SqlDbType = SqlDbType.NVarChar, Value = MyUser.name });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar, Value = MyUser.lastName });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Address", SqlDbType = SqlDbType.NVarChar, Value = MyUser.address });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@NickName", SqlDbType = SqlDbType.NVarChar, Value = MyUser.nickName });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar, Value = MyUser.password });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value = MyUser.email });
                Parameters.Add(new SqlParameter { DbType = DbType.String, Direction = ParameterDirection.Input, ParameterName = "@Phone", SqlDbType = SqlDbType.NVarChar, Value = MyUser.phone });
                Parameters.Add(new SqlParameter { DbType = DbType.Boolean, Direction = ParameterDirection.Input, ParameterName = "@IsActive", SqlDbType = SqlDbType.Bit, Value = MyUser.isActive });

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
                var connectionString = appSettings.Value.DNS;
                DALManager DAL = new DALManager();
                DAL.openDatabase(connectionString);
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
    }
}
