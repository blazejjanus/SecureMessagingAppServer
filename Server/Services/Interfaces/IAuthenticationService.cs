using PKiK.Server.DB;
using System.Collections.Generic;

namespace PKiK.Server.Services {
    public interface IAuthenticationService {
        public bool IsValid(string jwt);
        public bool IsUser(string jwt, int userID);
        public List<JwtDBO> GetUsersToken(int userID);
        public int GetUserID(string jwt);
        public bool CheckJwtValid(string jwt);
    }
}
