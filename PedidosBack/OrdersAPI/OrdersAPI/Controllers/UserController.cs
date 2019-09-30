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
using OrdersModels.User;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace OrdersAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {

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
            List<User> userList = new List<User>();
            DataTransfer<User> ret = new DataTransfer<User>();
            //Startup.Configuration.
            UserManager userManager = new UserManager();
            int CodError = 0;
            string ErrorMessage = string.Empty;
            userList = userManager.GetAllUserList(ref CodError, ref ErrorMessage);
            var users = Mapper.Map<IEnumerable<UserDto>>(userList);

            if (CodError != 0)
            {
                return NotFound();
            }

            ret.code = 0;
            ret.message = "OK";
            ret.lsdata = userList;
            return Ok(ret);

        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "getuser")]
        [EnableCors("MyPolicy")]
        public ActionResult<OneDataTransfer<User>> Get(string id)
        {
            OneDataTransfer<User> ret = new OneDataTransfer<User>();
            User user = new User();
            UserManager userManager = new UserManager();
            int CodError = 0;
            string ErrorMessage = string.Empty;
            user = userManager.GetUserById(id, ref CodError, ref ErrorMessage);
            if (CodError != 0)
            {
                ret.code = CodError;
                ret.message = ErrorMessage;
                return NotFound(ret);
            }
            if (user == null && user.IdUser == 0)
            {
                ret.data = null;
                ret.code = -98;
                ret.message = "Id Not found";
                return NotFound(ret);
            }
            ret.data = user;
            ret.code = 0;
            ret.message = "OK";
            return Ok(ret);
        }

        // POST: api/User
        [Authorize]
        [HttpPost(Name = "insert")]
        [Route("[action]")]
        [ActionName("insert")]
        [EnableCors("MyPolicy")]
        public ActionResult<OneDataTransfer<User>> Post([FromBody] UserInsert user)
        {
            OneDataTransfer<User> response = new OneDataTransfer<User>();
            try
            {
                int errorCode = 0;
                string errorMessage = "OK";
                UserManager userManager = new UserManager();

                //encrypt password
                SecurityRSA rSA = new SecurityRSA();
                string pubKey = rSA.GeneratePublicKey();
                string encryptedPassword = rSA.Encrypt(pubKey, user.password);
                user.password = encryptedPassword;

                string IdUser = userManager.InsertUser(user, ref errorCode, ref errorMessage);
                if (errorCode != 0)
                {
                    response.code = errorCode;
                    response.message = errorMessage;
                    return BadRequest(response);
                }
                response.code = errorCode;
                response.message = "OK";
                return CreatedAtRoute("getuser", new { id = IdUser }, response);
            }
            catch (Exception ex)
            {

                response.code = -100;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }


        //POST: api/UserLogin
        [HttpPost(Name = "user-login")]
        [ActionName("user-login")]
        [EnableCors("MyPolicy")]
        public ActionResult<OneDataTransfer<object>> userLogin([FromBody] UserLogin userLogin)
        {
            OneDataTransfer<object> response = new OneDataTransfer<object>();
            try
            {
                int errorCode = 0;
                string errorMessage = "OK";
                UserManager userManager = new UserManager();
                User user = userManager.GetUserByLogin(userLogin.UserName, ref errorCode, ref errorMessage);

                SecurityRSA rSA = new SecurityRSA();
                string pubKey = rSA.GeneratePublicKey();
                string decryptedPass = rSA.Decrypt(user.Password);
                if (decryptedPass == userLogin.Password)
                {
                    //Get JWT 
                    var claim = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, user.NickName)
                    };
                    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Aquivaunallaveconlaquequieroencriptar"));
                    int expiryInMinutes = 5; //Minutes to expired

                    var token = new JwtSecurityToken(
                        issuer: "http://www.ordersjsp.com.co",
                        audience: "http://www.ordersjsp.com.co",
                        expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                        signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                    );

                    response.data = new {
                        token=  new JwtSecurityTokenHandler().WriteToken(token),
                        expiration= token.ValidTo
                    };
                    response.code = errorCode;
                    response.message = "OK";
                    return Ok(response);
                }
                else
                {
                    response.code = errorCode;
                    response.message = errorMessage;
                    return Unauthorized();
                }

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
        public ActionResult<OneDataTransfer<User>> Put(string id, [FromBody] UserUpdate user)
        {
            OneDataTransfer<User> response = new OneDataTransfer<User>();
            try
            {
                UserManager userManager = new UserManager();
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

        [HttpPatch("{id}")]
        [EnableCors("MyPolicy")]
        public ActionResult<OneDataTransfer<User>> put(string id)
        {
            return null;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [EnableCors("MyPolicy")]
        public ActionResult<OneDataTransfer<User>> Delete(string id)
        {
            string errorMessage = string.Empty;
            int errorCode = 0;
            OneDataTransfer<User> response = new OneDataTransfer<User>();
            try
            {
                UserManager userManager = new UserManager();
                User userExists = userManager.GetUserById(id, ref errorCode, ref errorMessage);
                if (errorCode != 0)
                {
                    response.data = null;
                    response.code = errorCode;
                    response.message = "TODO: Error";
                    return NotFound();
                }
                if (userExists == null)
                {
                    response.data = null;
                    response.code = errorCode;
                    response.message = "User Not Found";
                    return NotFound(response);
                }

                userManager.DeleteUser(id, ref errorCode, ref errorMessage);
                if (errorCode != 0)
                {
                    response.code = errorCode;
                    response.message = errorMessage;
                    return NotFound(response);
                }
                response.code = errorCode;
                response.message = "OK";
                return NoContent();

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
