using PKiK.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using PKiK.Server.DB;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PKiK.Server.Services {
    public class MessageService : IMessageService {
        public IActionResult Delete(int ID, int userID) {
            using (var context = new DataContext()) {
                if (context.Messages.Any(x => x.ID == ID)) {
                    var dbo = context.Messages.Single(x => x.ID == ID);
                    if(dbo.Sender.ID != userID) {
                        return new StatusCodeResult(StatusCodes.Status403Forbidden);
                    }
                    context.Messages.Remove(dbo);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult Get(int ID) {
            using (var context = new DataContext()) {
                if (context.Messages.Any(x => x.ID == ID)) {
                    MessageDBO dbo = context.Messages.Single(x => x.ID == ID);
                    return new ObjectResult(ObjectMapper.Message(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetForUser(int UserID, int count, int index) {
            //TODO: Authorize user
            List<MessageDBO> messages;
            var context = new DataContext();
            if (!context.Users.Any(x => x.ID == UserID)) {
                return new ObjectResult("User does not exists!") { StatusCode = StatusCodes.Status404NotFound };
            } else {
                messages = context.Messages.Where(x => x.Sender.ID == UserID || x.Recipients.Any(u => u.User.ID == UserID))
                                                .OrderBy(x => x.SentTime).Skip(index).Take(count).ToList();
                return new ObjectResult(messages) { StatusCode = StatusCodes.Status200OK };
            }
        }

        public IActionResult Send(MessageDTO message) {
            var validation = message.Validate();
            if(!validation.IsSuccess) {
                return validation.GetResult();
            }
            using (var context = new DataContext()) {
                MessageDBO messageDBO = ObjectMapper.Message(message);
                if (context.Users.Any(x => x.ID == messageDBO.Sender.ID)) {
                    messageDBO.Sender = context.Users.Single(x => x.ID == messageDBO.Sender.ID);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                foreach (var recipient in messageDBO.Recipients) {
                    if (context.Users.Any(x => x.ID == recipient.User.ID)) {
                        recipient.User = context.Users.Single(x => x.ID == recipient.User.ID);
                        recipient.SeenTime = null;
                    } else {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }
                }
                context.Messages.Add(messageDBO);
                context.SaveChanges();
                return new StatusCodeResult(StatusCodes.Status200OK);
            }
        }

        public IActionResult SetSeen(int ID, int UserID, DateTime time) {
            using (var context = new DataContext()) {
                if (context.Messages.Any(x => x.ID == ID)) {
                    MessageDBO dbo = context.Messages.Single(x => x.ID == ID);
                    if (dbo.Recipients == null) {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                    foreach (var recipient in dbo.Recipients) {
                        if (recipient.User.ID == UserID) {
                            recipient.SeenTime = time;
                            context.SaveChanges();
                            return new StatusCodeResult(StatusCodes.Status200OK);
                        }
                    }
                    return new ObjectResult("The provided user was not a recipient of provided message!") { StatusCode = StatusCodes.Status409Conflict };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }
    }
}
