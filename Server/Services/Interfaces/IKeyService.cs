using Microsoft.AspNetCore.Mvc;
using PKiK.Shared;

namespace PKiK.Server.Services {
    public interface IKeyService {
        public IActionResult RevokeKey(int ID, int userID);
        public IActionResult GetUserKey(int UserID);
        public IActionResult AddKey(KeyDTO Key);
        public IActionResult GetKey(int ID);
    }
}
