using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.Services;
using PKiK.Shared;

namespace PKiK.Server.API.Controllers;

[ApiController]
[Route("[controller]")]
[AutoValidateAntiforgeryToken]
public class UserController : ControllerBase {
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;
    public UserController(IUserService userService, IAuthenticationService authService) {
        this._userService = userService;
        this._authService = authService;
    }

    [HttpGet("GetByID/{ID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetUserByID([FromRoute] int ID, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_userService.GetUser(ID), "UserController", "GetByID");
        } catch (Exception exc) {
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
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_userService.GetUser(Username), "UserController", "GetByID");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult PostUser([FromBody] UserDTO user, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_userService.AddUser(user), "UserController", "PostUser");
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
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            if (!_authService.IsUser(jwt, user.ID)) {
                return new StatusCodeResult(403);
            }
            return Result.Pass(_userService.ModifyUser(user), "UserController", "UpdateUser");
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
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_userService.RemoveUser(ID), "UserController", "DeleteUser");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpGet("SearchValue/{value}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult SearchUserByValue([FromRoute] string value, [FromHeader] string jwt, [FromHeader] int Count = 100, [FromHeader] bool ExactMatch = false) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_userService.SearchUser(value, Count, ExactMatch), "UserController", "SearchValue");
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
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_userService.SearchUser(key, value, Count, ExactMatch), "UserController", "SearchByKey");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }
}