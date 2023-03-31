using System;
using System.Collections.Generic;

namespace PKiK.Shared {
    public class RecipientDTO {
        public int ID { get; set; }
        public UserDTO User { get; set; }
        public string Content { get; set; }
        public DateTime? SeenTime { get; set; } //null if not seen

        public RecipientDTO() {
            User = new UserDTO();
            SeenTime = null;
            Content = string.Empty;
        }
    }
}
