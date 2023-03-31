using PKiK.Server.DB;
using PKiK.Shared;
using System;
using System.Text;

namespace PKiK.Server.Services {
    public static class Log {
        public static void Event(string message, EventType type = EventType.INFO) {
            EventDBO evt = new EventDBO();
            evt.Message = message;
            evt.Type = type;
            evt.DateTime = DateTime.Now;
            Display(evt);
            Save(evt);
        }
        public static void Event(Exception exc, string? message=null) {
            EventDBO evt = new EventDBO();
            evt.Type = EventType.ERROR;
            if(message!= null) {
                evt.Message = message + " Exception: " + exc.Message;
            } else {
                evt.Message = exc.Message;
            }
            evt.DateTime = DateTime.Now;
            if (exc.InnerException != null) {
                evt.Inner = exc.InnerException.Message;
                evt.Trace = exc.InnerException.StackTrace;
            } else {
                evt.Inner = null;
                evt.Trace = null;
            }
            Display(evt);
            Save(evt);
        }
        private static void Save(EventDBO evt) {
            using (var context = new DataContext()) {
                context.Events.Add(evt);
            }
        }
        private static void Display(EventDBO evt) {
            StringBuilder sb = new StringBuilder();
            if (evt.DateTime.HasValue) {
                sb.Append(evt.DateTime.Value.ToString("G") + " ");
            }
            sb.Append(evt.Type.ToString() + ": ");
            sb.Append(evt.Message);
            if(evt.Inner != null) {
                sb.Append("\nInner: " + evt.Inner);
            }
            if(evt.Trace != null) {
                sb.Append("\nTrace: " + evt.Trace);
            }
            switch (evt.Type) {
                case EventType.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case EventType.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case EventType.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            Console.WriteLine(sb.ToString());
            Console.ResetColor();
        }
    }
}
