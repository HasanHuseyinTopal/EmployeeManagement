using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class UserRoleInUserViewModel
    {
        public string roleId { get; set; }
        public string roleName { get; set; }
        public bool isSelected { get; set; }
    }
}
