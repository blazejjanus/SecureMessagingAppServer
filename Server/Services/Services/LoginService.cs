using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKiK.Server.DB;
using PKiK.Server.Services.Utils;
using PKiK.Server.Shared;
using PKiK.Shared;
using PKIK.Services.Utils;
using System.Linq;

namespace PKiK.Server.Services
{
    public class LoginService : ILoginService {
        private readonly IAuthenticationService _authService;
        private readonly Config _config;

        public LoginService(IAuthenticationService authService, Config config) {
            _authService = authService;
            _config = config;
        }

        public IActionResult Register(UserDTO user) {
            ObjectResult validation = ValidateUser(user);
            if (validation.StatusCode == 200) {
                using (var context = new DataContext()) {
                    if (context.Users.Any(x => x.Username == user.Username)) {
                        return new ObjectResult("User with same username already registered!") { StatusCode = StatusCodes.Status409Conflict };
                    }
                    var dbo = ObjectMapper.User(user);
                    //Generate hash
                    using (var hashingHelper = new HashingHelper(_config)) {
                        var result = hashingHelper.HashPassword(user.Password);
                        dbo.PasswordHash = result.Hash;
                        dbo.PasswordSalt = result.Salt;
                    }
                    context.Add(dbo);
                    context.SaveChanges();
                    var token = TokenGenerator.UserToken(dbo);
                    var jwtDBO = new JwtDBO() {
                        User = dbo,
                        JWT = token,
                        Active = true
                    };
                    context.Jwt.Add(jwtDBO);
                    context.SaveChanges();
                    return new ObjectResult(token) { StatusCode = StatusCodes.Status200OK };
                }
            } else {
                return validation;
            }
        }

        public IActionResult Login(string username, string password) {
            using (var context = new DataContext()) {
                if (context.Users.Any(x => x.Username == username)) {
                    UserDBO user = context.Users.Single(x => x.Username == username);
                    var passwordHash = new HashingHelper(_config).HashPassword(password.Trim(), user.PasswordSalt);
                    if (user.PasswordHash == passwordHash) {
                        string? token = null;
                        if (context.Jwt.Any(x => x.User.ID == user.ID)) {
                            var userTokens = context.Jwt.Where(x => x.User.ID == user.ID).ToList();
                            foreach (var userToken in userTokens) {
                                if (!_authService.CheckJwtValid(userToken.JWT)) {
                                    userToken.Active = false; //Deactivate expired token
                                } else {
                                    token = userToken.JWT;
                                    break;
                                }
                            }
                        }
                        if (token == null) {
                            token = TokenGenerator.UserToken(user);
                        }
                        return new ObjectResult(token) { StatusCode = StatusCodes.Status200OK };
                    } else {
                        return new ObjectResult("Wrong username or password!") { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult RevokeToken(string token) {
            //TODO: Revoke Token
            throw new System.NotImplementedException();
        }

        public IActionResult ValidateToken(string token) {
            //TODO: Check if token is valid
            throw new System.NotImplementedException();
        }

        private ObjectResult ValidateUser(UserDTO user) {
            if(user.Username == null || user.Username.Length < 4) {
                return new ObjectResult("Username not valid.") 
                                        { StatusCode = StatusCodes.Status406NotAcceptable };
            }
            if(!ValidateString(user.Name ?? "")) {
                return new ObjectResult("Name contains forbiden character.")
                                        { StatusCode = StatusCodes.Status406NotAcceptable };
            }
            if(!ValidateString(user.Surname ?? "")) {
                return new ObjectResult("Surname contains forbiden character.") 
                                        { StatusCode = StatusCodes.Status406NotAcceptable };
            }
            return new ObjectResult("") { StatusCode = 200 };
        }

        private bool ValidateString(string str) {
            foreach(char c in str) {
                if (!char.IsLetter(c)) { return false; }
            }
            return true;
        }
    }
}
