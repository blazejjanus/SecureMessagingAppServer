using PKiK.Shared;
using Microsoft.AspNetCore.Mvc;

namespace PKiK.Server.Services {
    public interface IUserService {
        public IActionResult AddUser(UserDTO user);
        public IActionResult RemoveUser(int ID);
        public IActionResult ModifyUser(UserDTO user);
        public IActionResult GetUser(int ID);
        public IActionResult GetUser(string username);
        public IActionResult SearchUser(string value, int count = 100, bool ExactMatch = false);
        public IActionResult SearchUser(string key, string value, int count = 100, bool ExactMatch = false);
        public IActionResult SearchUserByUsername(string value, int count = 100, bool ExactMatch = false);
        public IActionResult SearchUserByName(string value, int count = 100, bool ExactMatch = false);
        public IActionResult SearchUserBySurname(string value, int count = 100, bool ExactMatch = false);
    }
}
