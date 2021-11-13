using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_core_api.Models;
using dotnet_core_api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_core_api.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {

        [HttpPost("validate")]
        public IActionResult Validate([FromBody]User user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.email))
                    return BadRequest(Result.CreateResult(true, "Email não pode ser vazio"));
                if (string.IsNullOrWhiteSpace(user.password))
                    return BadRequest(Result.CreateResult(true, "Senha não pode ser vazio"));
                Result result = Result.GetInstance;

                UserBusiness userBusiness = new UserBusiness();
                result = userBusiness.ValidateLogin(user.email, user.password);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(Result.CreateResult(true, "Houve um problema ao realizar o login."));
            }

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]User user)
        {
            if (string.IsNullOrWhiteSpace(user.email))
                return BadRequest(Result.CreateResult(true, "Email não pode ser vazio"));
            if (!Helper.ValidaEmail(user.email))
                return BadRequest(Result.CreateResult(true, "Email no formato errado"));
            if (string.IsNullOrWhiteSpace(user.password))
                return BadRequest(Result.CreateResult(true, "Senha não pode ser vazio"));
            if (string.IsNullOrWhiteSpace(user.name))
                return BadRequest(Result.CreateResult(true, "Nome não pode ser vazio"));
            Result result = Result.GetInstance;

            UserBusiness userBusiness = new UserBusiness();
            result = userBusiness.Register(user.email, user.password.ToString(), user.name);

            return Ok(result);
        }

        [HttpPost("resetpassword")]
        public IActionResult ResetPassword(User user)
        {
            return Ok("");
        }

        [HttpGet("getusers")]
        public IActionResult GetUsers()
        {
            Result result = Result.GetInstance;

            UserBusiness userBusiness = new UserBusiness();
            result = userBusiness.GetUsers();

            return Ok(result);
        }

        [HttpGet("getusergroups")]
        public IActionResult GetUserGroups(string email)
        {
            Result result = Result.GetInstance;

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(Result.CreateResult(true, "Email não pode ser vazio"));
            if (!Helper.ValidaEmail(email))
                return BadRequest(Result.CreateResult(true, "Email no formato errado"));

            UserBusiness userBusiness = new UserBusiness();
            result = userBusiness.GetUserGroups(email);

            return Ok(result);
        }

        [HttpGet("getusertools")]
        public IActionResult GetUserTools(string email)
        {
            Result result = Result.GetInstance;

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(Result.CreateResult(true, "Email não pode ser vazio"));
            if (!Helper.ValidaEmail(email))
                return BadRequest(Result.CreateResult(true, "Email no formato errado"));

            UserBusiness userBusiness = new UserBusiness();
            result = userBusiness.GetUserTools(email);

            return Ok(result);
        }

    }
}