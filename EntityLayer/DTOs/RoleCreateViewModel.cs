using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage ="Role name is required"),Display(Name ="Role Name")]
        public string roleName { get; set; }
    }
}
