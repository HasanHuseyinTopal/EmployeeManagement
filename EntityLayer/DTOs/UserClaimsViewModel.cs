using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            claims = new List<UserClaim>();
        }
        public string userId { get; set; }
        public List<UserClaim> claims { get; set; }
    }
}
