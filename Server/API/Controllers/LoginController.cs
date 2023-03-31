using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.Services;
using PKiK.Shared;
using System;

namespace PKiK.Server.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase {
        private readonly ILoginService loginService;
        public LoginController(ILoginService loginService, IAuthenticationService authService) {
            this.loginService = loginService;
        }

        [HttpGet("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromHeader] string username, [FromHeader] string password) {
            try {
                return Result.Pass(loginService.Login(username, password), "LoginController", "Login");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] UserDTO user) {
            try {
                return Result.Pass(loginService.Register(user), "LoginController", "Register");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }
    }
}
