using PKiK.Server.DB.DBO;
using PKiK.Shared;

namespace PKiK.Server.Services {
    public static partial class ObjectMapper {
        public static KeyDBO Key(KeyDTO key) {
            return new KeyDBO {
                ID = key.ID,
                User = User(key.User),
                PublicKey = key.PublicKey,
                Active = key.Active
            };
        }
        public static KeyDTO Key(KeyDBO key) {
            return new KeyDTO {
                ID = key.ID,
                User = User(key.User),
                PublicKey = key.PublicKey,
                Active = key.Active
            };
        }
    }
}
