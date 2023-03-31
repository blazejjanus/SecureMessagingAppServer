using PKiK.Shared;
using PKiK.Server.DB;
using System.Collections.Generic;

namespace PKiK.Server.Services {
    public static partial class ObjectMapper {
        public static UserDBO User(UserDTO user) {
            return new UserDBO {
                ID = user.ID,
                Username = user.Username,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password
            };
        }

        public static UserDTO User(UserDBO user) {
            return new UserDTO {
                ID = user.ID,
                Username = user.Username,
                Name = user.Name,
                Surname = user.Surname
            };
        }

        public static List<UserDBO> User(List<UserDTO> users) {
            List<UserDBO> result = new List<UserDBO>();
            foreach (UserDTO user in users) {
                result.Add(User(user));
            }
            return result;
        }

        public static List<UserDTO> User(List<UserDBO> users) {
            List<UserDTO> result = new List<UserDTO>();
            foreach (UserDBO user in users) {
                result.Add(User(user));
            }
            return result;
        }
    }
}
