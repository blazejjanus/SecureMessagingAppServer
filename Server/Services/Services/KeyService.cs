using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.DB;
using PKiK.Server.DB.DBO;
using PKiK.Shared;
using System.Linq;

namespace PKiK.Server.Services.Services {
    public class KeyService : IKeyService {
        public IActionResult AddKey(KeyDTO Key) {
            using (var context = new DataContext()) {
                KeyDBO dbo = ObjectMapper.Key(Key);
                if (context.Keys.Any(x => x.User.ID == Key.User.ID && x.Active == true)) {
                    return new StatusCodeResult(StatusCodes.Status409Conflict); //Cannot have multiple active keys
                }
                if (context.Users.Any(x => x.ID == Key.User.ID)) {
                    dbo.User = context.Users.Single(x => x.ID == Key.User.ID);
                    dbo.Active = true;
                    context.Keys.Add(dbo);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status201Created);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetKey(int ID) {
            using (var context = new DataContext()) {
                if (context.Keys.Any(x => x.ID == ID)) {
                    var dbo = context.Keys.Single(x => x.ID == ID);
                    dbo.User = context.Users.Single(x => x.ID == dbo.User.ID);
                    return new ObjectResult(ObjectMapper.Key(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetUserKey(int UserID) {
            using (var context = new DataContext()) {
                if (context.Keys.Any(x => x.User.ID == UserID && x.Active == true)) {
                    KeyDBO dbo = context.Keys.Single(x => x.User.ID == UserID && x.Active == true);
                    dbo.User = context.Users.Single(x => x.ID == UserID);
                    return new ObjectResult(ObjectMapper.Key(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult RevokeKey(int ID, int userID) {
            using (var context = new DataContext()) {
                if (context.Keys.Any(x => x.ID == ID)) {
                    if (context.Keys.Any(x => x.ID == ID && x.Active == true)) {
                        KeyDBO dbo = context.Keys.Single(x => x.ID == ID && x.Active == true);
                        if(dbo.User.ID != userID) {
                            return new StatusCodeResult(StatusCodes.Status403Forbidden);
                        }
                        dbo.Active = false;
                        context.SaveChanges();
                        return new StatusCodeResult(StatusCodes.Status200OK);
                    } else {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }
    }
}
