using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTracking.Models
{
    public class Products
    {
        [Key]
        public Guid id { get; set; }
        public required string name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal price { get; set; }
        public required int stock { get; set; }
    }
}
