using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKiK.Server.DB.DBO {
    public class KeyDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [ForeignKey("UserID")]
        public virtual UserDBO User { get; set; }
        //TODO: MaxLength
        [Required]
        public string PublicKey { get; set; }
        public bool Active { get; set; }

        public KeyDBO() {
            PublicKey = "";
            User = new UserDBO();
            Active = true;
        }
    }
}
