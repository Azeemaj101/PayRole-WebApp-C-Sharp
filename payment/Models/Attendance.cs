using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, 31, ErrorMessage = "Please enter range(0-31)")]
        public int MonthlyCount { get; set; }
    }
}
