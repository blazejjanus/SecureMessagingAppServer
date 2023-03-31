using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.Services;
using PKiK.Shared;
using System;

namespace PKiK.Server.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase {
        private readonly IMessageService messageService;
        private readonly IAuthenticationService authService;
        public MessageController(IMessageService messageService, IAuthenticationService authService) {
            this.messageService = messageService;
            this.authService = authService;
        }

        [HttpGet("Get/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMessage([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if(!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(messageService.Get(ID), "MessageController", "GetMessage");
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
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(messageService.GetForUser(ID, Count, Index), "MessageController", "GetForUser");
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
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                if(!authService.IsUser(jwt, message.Sender.ID)) {
                    return new StatusCodeResult(403);
                }
                return Result.Pass(messageService.Send(message), "MessageController", "SendMessage");
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
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(messageService.SetSeen(ID, UserID, Time), "MessageController", "SetSeen");
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
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                int userID = authService.GetUserID(jwt);
                return Result.Pass(messageService.Delete(ID, userID), "MessageController", "DeleteMessage");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }
    }
}
