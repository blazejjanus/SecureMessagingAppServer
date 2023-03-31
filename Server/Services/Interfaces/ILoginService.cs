using Microsoft.AspNetCore.Mvc;
using PKiK.Shared;

namespace PKiK.Server.Services {
    public interface ILoginService {
        public IActionResult Register(UserDTO user);
        public IActionResult Login(string username, string password);
        public IActionResult ValidateToken(string token);
        public IActionResult RevokeToken(string token);
    }
}
