using JWTProject.API.Services;
using JWTProject.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace JWTProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ResponseModel> Login([FromBody] LoginModel model)
        {
            try
            {
                ResponseModel response = new ResponseModel();

                if (!ModelState.IsValid)
                    return new ResponseModel() { token = "", message = "Invalid credentials", success = false };
                var (status, message) = await _authService.Login(model);
                if (status == 0)
                    return new ResponseModel() { token = "", message = message, success = false }; ;
                return new ResponseModel() { token = message, message = "success", success = true }; ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseModel() { token = "", message = ex.Message, success = false }; ;
            }
        }
        [HttpPost]
        [Route("role")]
        //[Authorize("Admin")]
        public async Task<IActionResult> AddRole(string role)
        {
            try
            {
                if (!role.Equals(UserRoles.Admin) && !role.Equals(UserRoles.User))
                {
                    return BadRequest("Invalid role");
                }
                var (status, message) = await _authService.CreateRole(role.Equals(UserRoles.Admin) ? UserRoles.Admin : UserRoles.User);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(AddRole), role);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("registeration")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _authService.Registeration(model, UserRoles.User);
                //var (status, message) = await _authService.Registeration(model, UserRoles.Admin);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Register), model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}


//user
//{
//    "username":"pooja",
//    "email":"pooja@gmail.com",
//    "password":"Pooja@123",
//    "name":"Pooja Tambe"
//}
//ADmin
//{
//    "username": "admin",
//  "name": "admin user",
//  "email": "admin@gmail.com",
//  "password": "Admin@123"
//}