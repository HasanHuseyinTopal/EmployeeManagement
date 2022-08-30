using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class UserUpdateViewModel
    {
        public List<string> roles { get; set; }
        public List<string> claims { get; set; }
        public UserUpdateViewModel()
        {
            roles = new List<string>();
            claims = new List<string>();
        }
        public string userId { get; set; }
        [Required(ErrorMessage = "Email is required value")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email adress")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Email is required value")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email adress")]
        public string userEmail { get; set; }
        public string userCity { get; set; }

    }
}
