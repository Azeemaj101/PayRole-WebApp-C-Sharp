using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace payment.Models
{
    public class Manager
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int Age { get; set; }

        //job starting date
        [Required]
        public DateTime JSD { get; set; }

        [Required]
        [ForeignKey("log")]
        public int LoginID { get; set; }
        public virtual Login? Log { get; set; }
    }
}
