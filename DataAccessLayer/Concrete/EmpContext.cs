using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class EmpContext : IdentityDbContext<ApplicationUser>
    {
        public EmpContext(DbContextOptions<EmpContext> dbContextOptions):base(dbContextOptions)
        {

        }

        public DbSet<Employee> employees { get; set; }
    }
}
