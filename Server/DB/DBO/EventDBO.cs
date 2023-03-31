using PKiK.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKiK.Server.DB {
    public class EventDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? ID { get; set; }
        public virtual DateTime? DateTime { get; set; }
        public virtual EventType? Type { get; set; }
        [MaxLength(250)]
        public virtual string? Message { get; set; }
        [MaxLength(250)]
        public virtual string? Inner { get; set; }
        [MaxLength(100)]
        public virtual string? Trace { get; set; }
    }
}
