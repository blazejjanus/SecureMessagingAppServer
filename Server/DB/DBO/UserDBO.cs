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
        public virtual string PasswordHash { get; set; }
        [Required]
        public virtual string PasswordSalt { get; set; }
        public UserDBO() {
            Username = string.Empty;
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
        }
    }
}
