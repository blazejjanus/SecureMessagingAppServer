using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.Services;
using PKiK.Shared;

namespace PKiK.Server.API.Controllers;

[ApiController]
[Route("[controller]")]
[AutoValidateAntiforgeryToken]
public class KeyController : ControllerBase {
    private readonly IKeyService _keyService;
    private readonly IAuthenticationService _authService;
    public KeyController(IKeyService keyService, IAuthenticationService authService) {
        this._keyService = keyService;
        this._authService = authService;
    }

    [HttpGet("GetByID/{ID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetKey([FromRoute] int ID, [FromRoute] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_keyService.GetKey(ID), "KeyController", "GetKey");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpGet("GetByUserID/{ID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetKeyForUser([FromRoute] int ID, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_keyService.GetUserKey(ID), "KeyController", "GetUserKey");
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
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult PostKey([FromBody] KeyDTO key, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            if (!_authService.IsUser(jwt, key.User.ID)) {
                return new StatusCodeResult(403);
            }
            return Result.Pass(_keyService.AddKey(key), "KeyController", "PostKey");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpPut("Revoke/{ID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult RevokeKey([FromRoute] int ID, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            int userID = _authService.GetUserID(jwt);
            return Result.Pass(_keyService.RevokeKey(ID, userID), "KeyController", "RevokeKey");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }
}
