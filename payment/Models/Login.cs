using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace payment.Models
{
    public class Login
    {
        [Key]
        public int LoginID { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string? username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string? password { get; set; }
    }
}
