using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.Services;
using PKiK.Shared;

namespace PKiK.Server.API.Controllers;

[ApiController]
[Route("[controller]")]
[AutoValidateAntiforgeryToken]
public class MessageController : ControllerBase {
    private readonly IMessageService _messageService;
    private readonly IAuthenticationService _authService;
    public MessageController(IMessageService messageService, IAuthenticationService authService) {
        this._messageService = messageService;
        this._authService = authService;
    }

    [HttpGet("Get/{ID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetMessage([FromRoute] int ID, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_messageService.Get(ID), "MessageController", "GetMessage");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpGet("GetForUser/{ID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetForUser([FromRoute] int ID, [FromHeader] string jwt, [FromHeader] int Count = 10, [FromHeader] int Index = 0) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_messageService.GetForUser(ID, Count, Index), "MessageController", "GetForUser");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpPost("Send")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult PostMessage([FromBody] MessageDTO message, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            if (!_authService.IsUser(jwt, message.Sender.ID)) {
                return new StatusCodeResult(403);
            }
            return Result.Pass(_messageService.Send(message), "MessageController", "SendMessage");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpPut("SetSeen/{ID}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult SetSeen([FromRoute] int ID, [FromHeader] int UserID, [FromHeader] DateTime Time, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            return Result.Pass(_messageService.SetSeen(ID, UserID, Time), "MessageController", "SetSeen");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }

    [HttpDelete("Delete/{ID}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteMessage([FromRoute] int ID, [FromHeader] string jwt) {
        try {
            if (!_authService.IsValid(jwt)) {
                return new StatusCodeResult(401);
            }
            int userID = _authService.GetUserID(jwt);
            return Result.Pass(_messageService.Delete(ID, userID), "MessageController", "DeleteMessage");
        } catch (Exception exc) {
            Log.Event(exc);
            return new StatusCodeResult(500);
        }
    }
}
