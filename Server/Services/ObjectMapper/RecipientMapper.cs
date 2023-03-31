using PKiK.Server.DB;
using PKiK.Shared;
using System.Collections.Generic;

namespace PKiK.Server.Services {
    public static partial class ObjectMapper {
        public static RecipientDBO Recipient(RecipientDTO recipient) {
            return new RecipientDBO {
                ID = recipient.ID,
                User = User(recipient.User),
                Content = recipient.Content,
                SeenTime = recipient.SeenTime
            };
        }
        public static RecipientDTO Recipient(RecipientDBO recipient) {
            return new RecipientDTO {
                ID = recipient.ID,
                User = User(recipient.User),
                Content = recipient.Content,
                SeenTime = recipient.SeenTime
            };
        }
        public static List<RecipientDBO> Recipient(List<RecipientDTO> recipients) {
            List<RecipientDBO> result = new List<RecipientDBO>();
            foreach (RecipientDTO recipient in recipients) {
                result.Add(Recipient(recipient));
            }
            return result;
        }
        public static List<RecipientDTO> Recipient(List<RecipientDBO> recipients) {
            List<RecipientDTO> result = new List<RecipientDTO>();
            foreach (RecipientDBO recipient in recipients) {
                result.Add(Recipient(recipient));
            }
            return result;
        }
    }
}
