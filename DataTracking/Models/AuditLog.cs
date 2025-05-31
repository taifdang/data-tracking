using System.ComponentModel.DataAnnotations;

namespace DataTracking.Models
{
    public class AuditLog
    {
        [Key]
        public Guid id { get; set; }
        public string session_id { get; set; }
        public long timestamp { get; set; }
        public string action { get; set; }
        public string entity { get; set; }
        public string data { get; set; }
       
    }
}
