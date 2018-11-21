using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersModels;
using OrdersModels.UserPackage;
using OrdersDAL;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Net;
using Microsoft.Extensions.Options;
using OrdersDAL.Interfaces;
using OrdersAPI.Manager;
using AutoMapper;
using Microsoft.AspNetCore.Cors;

namespace OrdersAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        //private IConfiguration configuration;
        //public UserController(IConfiguration iConfig) {
        //    configuration = iConfig;
        //}
        private readonly IOptions<AppSettings> appSettings;

        public UserController(IOptions<AppSettings> app)
        {
            appSettings = app;
        }

        // GET: api/User
        [HttpGet]
        [EnableCors("MyPolicy")]
        public ActionResult<DataTransfer<User>> Get()
        {
            //UserInsert ui = new UserInsert();
            //ui.address = "Itagui";
            //ui.email = "johnsebas28@hotmail.es";
            //ui.identification = Guid.NewGuid().ToString().Substring(0, 9);
            //ui.isActive = true;
            //ui.lastName = "Palacio -" + Guid.NewGuid().ToString().Substring(0, 9);
            //ui.name = "Sebastián - " + Guid.NewGuid().ToString().Substring(0, 9);
            //ui.nickName = "johnsebas" + Guid.NewGuid().ToString().Substring(0, 9);
            //ui.password = Guid.NewGuid().ToString().Substring(0, 9);
            //ui.phone = "3046533029";

            //string test = Newtonsoft.Json.JsonConvert.SerializeObject(ui);

            //throw new Exception("this is an exception");
            DataTransfer<User> user = new DataTransfer<User>();
           
            UserManager userManager = new UserManager(appSettings);
            var connectionString = appSettings.Value.DNS;
            user = userManager.GetAllUserList();
            var users = Mapper.Map<IEnumerable<UserDto>>(user.lsdata);
            
            

            if (user.code != 0)
            {
                return NotFound();
            }
            return Ok(user);

        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUser")]
        [EnableCors("MyPolicy")]
        public ActionResult<DataTransfer<User>> Get(string id)
        {

            DataTransfer<User> UserResponse = new DataTransfer<User>();
            UserManager userManager = new UserManager(appSettings);
            var connectionString = appSettings.Value.DNS;
            UserResponse = userManager.GetUserById(id);
            if (UserResponse.code != 0)
            {
                return NotFound();
            }
            if (UserResponse.lsdata  == null && UserResponse.data.idUser == null) {
                UserResponse.data = null;
                UserResponse.code = -98;
                UserResponse.message = "Id Not found";
                return NotFound();
            }
            return Ok(UserResponse);
        }

        // POST: api/User
        [HttpPost]
        [EnableCors("MyPolicy")]
        public ActionResult<DataTransfer<User>> Post([FromBody] UserInsert user)
        {
            DataTransfer<User> response = new DataTransfer<User>();
            try
            {
                int errorCode = 0;
                string errorMessage = "OK";
                UserManager userManager = new UserManager(appSettings);
                string IdUser = userManager.InsertUser(user, ref errorCode, ref errorMessage);
                if (errorCode != 0)
                {
                    response.code = errorCode;
                    response.message = errorMessage;
                    return BadRequest(response);
                }
                response.code = errorCode;
                response.message = "OK";
                return CreatedAtRoute("GetUser",new { id= IdUser},response);
            }
            catch (Exception ex)
            {

                response.code = -100;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [EnableCors("MyPolicy")]
        public ActionResult<DataTransfer<User>> Put(string id, [FromBody] UserUpdate user)
        {
            DataTransfer<User> response = new DataTransfer<User>();
            try
            {
                UserManager userManager = new UserManager(appSettings);
                string errorMessage = string.Empty;
                int errorCode = 0;
                userManager.UpdateUser(id, user, ref errorCode, ref errorMessage);
                if (errorCode != 0)
                {
                    response.code = errorCode;
                    response.message = errorMessage;
                    return NotFound(response);
                }
                response.code = errorCode;
                response.message = "OK";
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.code = -100;
                response.message = ex.Message;
                return BadRequest(response);
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [EnableCors("MyPolicy")]
        public ActionResult<DataTransfer<User>> Delete(string id)
        {
            DataTransfer<User> response = new DataTransfer<User>();
            try
            {
                UserManager userManager = new UserManager(appSettings);
                string errorMessage = string.Empty;
                int errorCode = 0;
                userManager.DeleteUser(id, ref errorCode, ref errorMessage);
                if (errorCode != 0)
                {
                    response.code = errorCode;
                    response.message = errorMessage;
                    return NotFound(response);
                }
                response.code = errorCode;
                response.message = "OK";
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.code = -100;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
