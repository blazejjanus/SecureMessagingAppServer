using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.Services;
using PKiK.Shared;
using System;

namespace PKiK.Server.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly IUserService userService;
        private readonly IAuthenticationService authService;
        public UserController(IUserService userService, IAuthenticationService authService) {
            this.userService = userService;
            this.authService = authService;
        }

        #region UserManagement
        #region Get
        [HttpGet("GetByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByID([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(userService.GetUser(ID), "UserController", "GetByID");
            }catch(Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("GetByUsername/{Username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByUsername([FromRoute] string Username, [FromHeader] string jwt) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(userService.GetUser(Username), "UserController", "GetByID");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }
        #endregion

        #region Post
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostUser([FromBody] UserDTO user, [FromHeader] string jwt) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(userService.AddUser(user), "UserController", "PostUser");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }

        [HttpPut("Modify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUser([FromBody] UserDTO user, [FromHeader] string jwt) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                if (!authService.IsUser(jwt, user.ID)) {
                    return new StatusCodeResult(403);
                }
                return Result.Pass(userService.ModifyUser(user), "UserController", "UpdateUser");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }
        
        [HttpDelete("Delete/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUser([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(userService.RemoveUser(ID), "UserController", "DeleteUser");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }
        #endregion
        #endregion

        #region SearchUser
        [HttpGet("SearchValue/{value}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchUserByValue([FromRoute] string value, [FromHeader] string jwt, [FromHeader] int Count = 100, [FromHeader] bool ExactMatch = false) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(userService.SearchUser(value, Count,  ExactMatch), "UserController", "SearchValue");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("SearchByKey/{key}/{value}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchUserByValue([FromRoute] string key, [FromRoute] string value, [FromHeader] string jwt, [FromHeader] int Count = 100, [FromHeader] bool ExactMatch = false) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(userService.SearchUser(key, value, Count, ExactMatch), "UserController", "SearchByKey");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }
        #endregion
    }
}