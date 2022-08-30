using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string oldPassword { get; set; }
        [Required]
        public string newPassword { get; set; }
        [Compare(nameof(newPassword),ErrorMessage ="Passwords Not Matched")]
        [Required]
        public string newPasswordConfirm { get; set; }
    }
}
