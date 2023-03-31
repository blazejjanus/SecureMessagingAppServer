namespace PKiK.Shared {
    public class KeyDTO {
        public int ID { get; set; }
        public UserDTO User { get; set; }
        public string PublicKey { get; set; }
        public bool Active { get; set; }

        public KeyDTO() {
            User = new UserDTO();
            PublicKey = "";
            Active = true;
        }
    }
}
