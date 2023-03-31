using PKiK.Shared;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.DB;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PKiK.Server.Services {
    public class UserService : IUserService {
        public IActionResult AddUser(UserDTO user) {
            using (var context = new DataContext()) {
                if (!context.Users.Any(x => x.Username == user.Username)) {
                    context.Users.Add(ObjectMapper.User(user));
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status201Created);
                } else {
                    return new ObjectResult("User with same username already exists!") { StatusCode = StatusCodes.Status409Conflict };
                }
            }
        }

        public IActionResult GetUser(int ID) {
            using (var context = new DataContext()) {
                if (context.Users.Any(x => x.ID == ID)) {
                    UserDBO dbo = context.Users.Single(x => x.ID == ID);
                    return new ObjectResult(ObjectMapper.User(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetUser(string username) {
            using (var context = new DataContext()) {
                if (context.Users.Any(x => x.Username == username)) {
                    UserDBO dbo = context.Users.Single(x => x.Username == username);
                    return new ObjectResult(ObjectMapper.User(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult ModifyUser(UserDTO user) {
            using (var context = new DataContext()) {
                if (context.Users.Any(x => x.ID == user.ID)) {
                    UserDBO dbo = context.Users.Single(x => x.ID == user.ID);
                    dbo.Username = user.Username;
                    dbo.Name = user.Name;
                    dbo.Surname = user.Surname;
                    dbo.Password = user.Password;
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult RemoveUser(int ID) {
            using (var context = new DataContext()) {
                if (context.Users.Any(x => x.ID == ID)) {
                    UserDBO dbo = context.Users.Single(x => x.ID == ID);
                    var messages = context.Messages.Where(x => x.Sender.ID == ID
                                                          || x.Recipients.Any(r => r.User.ID == ID)).ToList();
                    var placeholder = context.Users.Single(x => x.Username == "removed");
                    foreach (var message in messages) {
                        if (message.Sender.ID == ID) {
                            message.Sender = placeholder;
                        }
                        if (message.Recipients != null) {
                            foreach (var recipient in message.Recipients) {
                                if (recipient.User.ID == ID) {
                                    recipient.User = placeholder;
                                }
                            }
                        }
                    }
                    context.Users.Remove(dbo);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult SearchUser(string value, int count = 100, bool ExactMatch = false) {
            List<UserDBO> dbo = new List<UserDBO>();
            value = value.Trim();
            using (var context = new DataContext()) {
                if (value == "*") {
                    dbo = context.Users.Take(count).ToList();
                } else {
                    if (ExactMatch) {
                        dbo = context.Users.Where(x => x.Username == value || x.Name == value || x.Surname == value).Take(count).ToList();
                    } else {
                        dbo = context.Users.Where(x => (x.Username.Contains(value)) ||
                            (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(value)) ||
                            (!string.IsNullOrEmpty(x.Surname) && x.Surname.Contains(value))).Take(count).ToList();
                    }
                }
                return new ObjectResult(ObjectMapper.User(dbo)) { StatusCode = StatusCodes.Status200OK };
            }
        }

        public IActionResult SearchUser(string key, string value, int count = 100, bool ExactMatch = false) {
            key = key.Trim().ToLower();
            value = value.Trim();
            switch (key) {
                case "username":
                case "uname":
                    return SearchUserByUsername(value, count, ExactMatch);
                case "name":
                case "firstname":
                    return SearchUserByName(value, count, ExactMatch);
                case "surname":
                case "lastname":
                    return SearchUserBySurname(value, count, ExactMatch);
                default:
                    return new ObjectResult("Invalid key provided!") { StatusCode = StatusCodes.Status400BadRequest };
            }
        }

        public IActionResult SearchUserByUsername(string value, int count = 100, bool ExactMatch = false ) {
            value = value.Trim();
            List<UserDBO> dbo = new List<UserDBO>();
            using (var context = new DataContext()) {
                if (value == "*") {
                    dbo = context.Users.Take(count).ToList();
                } else {
                    if (ExactMatch) {
                        dbo = context.Users.Where(x => x.Username == value).Take(count).ToList();
                    } else {
                        dbo = context.Users.Where(x => (x.Username.Contains(value))).Take(count).ToList();
                    }
                }
                return new ObjectResult(ObjectMapper.User(dbo)) { StatusCode = StatusCodes.Status200OK };
            }
        }

        public IActionResult SearchUserByName(string value, int count = 100, bool ExactMatch = false) {
            value = value.Trim();
            List<UserDBO> dbo = new List<UserDBO>();
            using (var context = new DataContext()) {
                if (value == "*") {
                    dbo = context.Users.Take(count).ToList();
                } else {
                    if (ExactMatch) {
                        dbo = context.Users.Where(x => x.Name == value).Take(count).ToList();
                    } else {
                        dbo = context.Users.Where(x => (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(value))).Take(count).ToList();
                    }
                }
                return new ObjectResult(ObjectMapper.User(dbo)) { StatusCode = StatusCodes.Status200OK };
            }
        }

        public IActionResult SearchUserBySurname(string value, int count = 100, bool ExactMatch = false) {
            value = value.Trim();
            List<UserDBO> dbo = new List<UserDBO>();
            using (var context = new DataContext()) {
                if (value == "*") {
                    dbo = context.Users.Take(count).ToList();
                } else {
                    if (ExactMatch) {
                        dbo = context.Users.Where(x => x.Surname == value).Take(count).ToList();
                    } else {
                        dbo = context.Users.Where(x => (!string.IsNullOrEmpty(x.Surname) && x.Surname.Contains(value))).Take(count).ToList();
                    }
                }
                return new ObjectResult(ObjectMapper.User(dbo)) { StatusCode = StatusCodes.Status200OK };
            }
        }
    }
}
