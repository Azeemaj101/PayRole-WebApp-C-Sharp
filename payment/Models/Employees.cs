using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Models
{
    public class Employees
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Range(0, 100, ErrorMessage = "Please enter range(0-100)")]
        [Required]
        public int Age { get; set; }

        [Required]
        public double CNIC { get; set; }
        
        [Required]
        [Range(0, 3000000, ErrorMessage = "Please enter range(0-100)")]
        public double Pay { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Do not enter more than 10 characters")]
        public string? Gender { get; set; }

        [Required]
        public int Category { get; set; }

        //job starting date
        [Required]
        [DisplayName("Job Starting Date")]
        public DateTime JSD { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("attendance")]
        public int AttendanceID { get; set; }
        public virtual Attendance? attendance { get; set; }
        
        [Required]
        [ForeignKey("manager")]
        public int ManagerID { get; set; }
        public virtual Manager? manager { get; set; }
    }
}
