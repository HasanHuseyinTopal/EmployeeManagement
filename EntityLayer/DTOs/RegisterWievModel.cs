using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class RegisterWievModel
    {
        [Required(ErrorMessage = "Email is required value")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email adress")]
        [Display(Name = "Email")]
        public string email { get; set; }


        [Required,MinLength(6,ErrorMessage ="Password must have more then 6 character"),MaxLength(16,ErrorMessage = "Password must have less then 16 character")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(confirmPassword),ErrorMessage = "Passwords Are Not Matched")]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }
        public string? city { get; set; }
    }
}
