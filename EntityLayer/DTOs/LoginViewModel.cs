using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EntityLayer.DTOs
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required value")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email adress")]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required, MinLength(6, ErrorMessage = "Password must have more then 6 character"), MaxLength(16, ErrorMessage = "Password must have less then 16 character")]
        public string password { get; set; }
        [ValidateNever]
        public bool rememberMe { get; set; }
        public IList<AuthenticationScheme>? externalLogins { get; set; }
    }
}
