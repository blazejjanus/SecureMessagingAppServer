using PKiK.Server.DB.DBO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKiK.Server.DB {
    public class UserDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Username { get; set; }
        [MaxLength(50)]
        public virtual string? Name { get; set; }
        [MaxLength(50)]
        public virtual string? Surname { get; set; }
        [Required]
        public virtual string Password { get; set; }
        public UserDBO() {
            Username = "";
            Password = "";
        }
    }
}
