using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class RoleUpdateViewModel
    {
        [Required]
        public string roleId { get; set; }
        [Required(ErrorMessage ="Role name is required")]
        public string roleName { get; set; }
        [ValidateNever]
        public List<string> usersInRole { get; set; }
    }
}
