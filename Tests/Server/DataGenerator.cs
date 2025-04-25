namespace PKiK.Tests.Server;

internal static class DataGenerator {
    private static readonly string _usernameChars = "qwertyuiopasdfghjklzxcvbnm";
    internal static string Username(int length = 5) {
        var rng = new Random();
        string result = string.Empty;
        for (int i = 0; i < length; i++) {
            result += _usernameChars.ElementAt(rng.Next(0, _usernameChars.Length));
        }
        return result;
    }

    internal static UserDTO User() {
        string random = Username();
        return new UserDTO() {
            Username = random,
            Name = random,
            Surname = random,
            Password = random
        };
    }

    internal static List<UserDTO> User(int count = 5) {
        var list = new List<UserDTO>();
        for (int i = 0; i < count; i++) {
            list.Add(User());
        }
        return list;
    }
}
