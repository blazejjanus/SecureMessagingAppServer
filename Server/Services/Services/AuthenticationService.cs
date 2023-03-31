using PKiK.Server.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace PKiK.Server.Services {
    public class AuthenticationService : IAuthenticationService {
        public bool CheckJwtValid(string jwt) {
            JwtSecurityToken token;
            try {
                token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            } catch (Exception) {
                return false;
            }

            return token.ValidTo > DateTime.UtcNow;
        }

        public List<JwtDBO> GetUsersToken(int userID) {
            var result = new List<JwtDBO>();
            using (var context = new DataContext()) {
                if(context.Jwt.Any(x => x.User.ID == userID && x.Active)) {
                    result = context.Jwt.Where(x => x.User.ID == userID && x.Active).ToList();
                }
            }
            return result;
        }

        public int GetUserID(string jwt) {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadToken(jwt) as JwtSecurityToken;
            int id;
            if (int.TryParse(token?.Payload["sub"].ToString(), out id)) {
                return id;
            } else {
                throw new Exception("Malformed token!");
            }
        }

        public bool IsUser(string jwt, int userID) {
            int id = GetUserID(jwt);
            if(id == userID) {
                return true;
            } else {
                return false;
            }
        }

        public bool IsValid(string jwt) {
            if (!CheckJwtValid(jwt)) {
                return false;
            }
            var userTokens = GetUsersToken(GetUserID(jwt));
            if (userTokens == null || userTokens.Count < 1) {
                throw new Exception("JWT user not found!");
            }
            foreach (var userToken in userTokens) {
                if(userToken.JWT == jwt) {
                    return true;
                }
            }
            return false;
        }
    }
}
