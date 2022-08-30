using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class SetPasswordViewModel
    {
        [Required]
        public string setPassword { get; set; }
        [Required]
        [Compare(nameof(setPassword),ErrorMessage ="Passwords Not Matched")]
        public string setPasswordConfirm { get; set; }
    }
}
