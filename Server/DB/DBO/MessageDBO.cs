using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKiK.Server.DB {
    public class MessageDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [ForeignKey("SenderID")]
        public virtual UserDBO Sender { get; set; }
        [ForeignKey("RecipientID")]
        public virtual ICollection<RecipientDBO> Recipients { get; set; }
        public virtual DateTime SentTime { get; set; }

        public MessageDBO() {
            Sender = new UserDBO();
            Recipients = new List<RecipientDBO>();
        }
    }
}
