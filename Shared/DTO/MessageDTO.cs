using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PKiK.Shared {
    public class MessageDTO {
        public int ID { get; set; }
        public UserDTO Sender { get; set; }
        public List<RecipientDTO> Recipients { get; set; }
        public DateTime SentTime { get; set; }

        public MessageDTO() {
            Sender = new UserDTO();
            Recipients = new List<RecipientDTO>();
        }

        public OperationResult Validate() {
            if(!Recipients.Any(x => x.User.ID == Sender.ID)) { //Check if the sender is one of the recipients
                return new OperationResult(HttpStatusCode.BadRequest, "Sender is not on recipients list!");
            }
            if(Recipients.Count < 2) {
                return new OperationResult(HttpStatusCode.BadRequest, "Recipients list is not valid!");
            }
            return new OperationResult();
        }
    }
}
