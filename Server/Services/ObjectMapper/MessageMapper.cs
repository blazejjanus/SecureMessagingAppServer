using PKiK.Shared;
using PKiK.Server.DB;
using System.Collections.Generic;
using System.Linq;

namespace PKiK.Server.Services {
    public static partial class ObjectMapper {
        public static MessageDBO Message(MessageDTO message) {
            return new MessageDBO {
                Sender = User(message.Sender),
                SentTime = message.SentTime,
                Recipients = Recipient(message.Recipients.ToList()),
            };
        }

        public static MessageDTO Message(MessageDBO message) {
            return new MessageDTO {
                Sender = User(message.Sender),
                SentTime = message.SentTime,
                Recipients = Recipient(message.Recipients.ToList()),
            };
        }

        public static List<MessageDBO> Message(List<MessageDTO> messages) {
            List<MessageDBO> result = new List<MessageDBO>();
            foreach (MessageDTO message in messages) {
                result.Add(Message(message));
            }
            return result;
        }

        public static List<MessageDTO> Message(List<MessageDBO> messages) {
            List<MessageDTO> result = new List<MessageDTO>();
            foreach (MessageDBO message in messages) {
                result.Add(Message(message));
            }
            return result;
        }
    }
}
