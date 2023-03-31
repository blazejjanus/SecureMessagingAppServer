using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PKiK.Server.DB {
    public class RecipientDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [ForeignKey("UserID")]
        public virtual UserDBO User { get; set; }
        [Required]
        public virtual int UserID { get; set; }
        public virtual string Content { get; set; }
        public virtual DateTime? SeenTime { get; set; } //null if not seen

        public RecipientDBO() {
            User = new UserDBO();
            Content = string.Empty;
        }
    }
}
