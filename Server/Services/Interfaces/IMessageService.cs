using PKiK.Shared;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PKiK.Server.Services {
    public interface IMessageService {
        public IActionResult Send(MessageDTO message);
        public IActionResult Delete(int ID, int userID);
        public IActionResult SetSeen(int ID, int UserID, DateTime time);
        public IActionResult Get(int ID);
        public IActionResult GetForUser(int UserID, int count, int index);
    }
}
