using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PKiK.Server.Services;
using PKiK.Shared;
using System;

namespace PKiK.Server.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class KeyController : ControllerBase {
        private readonly IKeyService keyService;
        private readonly IAuthenticationService authService;
        public KeyController(IKeyService keyService, IAuthenticationService authService) {
            this.keyService = keyService;
            this.authService = authService;
        }

        [HttpGet("GetByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetKey([FromRoute] int ID, [FromRoute] string jwt) {
            try {
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(keyService.GetKey(ID), "KeyController", "GetKey");
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
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(keyService.GetUserKey(ID), "KeyController", "GetUserKey");
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
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                if (!authService.IsUser(jwt, key.User.ID)) {
                    return new StatusCodeResult(403);
                }
                return Result.Pass(keyService.AddKey(key), "KeyController", "PostKey");
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
                if (!authService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                int userID = authService.GetUserID(jwt);
                return Result.Pass(keyService.RevokeKey(ID, userID), "KeyController", "RevokeKey");
            } catch (Exception exc) {
                Log.Event(exc);
                return new StatusCodeResult(500);
            }
        }
    }
}
