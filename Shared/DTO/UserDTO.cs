namespace PKiK.Shared {
    public class UserDTO {
        public int ID { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Password { get; set; }

        public UserDTO(string username) {
            Username = username;
            Password = string.Empty;
        }

        public UserDTO() {
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}
