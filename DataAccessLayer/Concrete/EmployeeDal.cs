using DataAccessLayer.Abstract;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class EmployeeDal : GenericRepositoryDal<Employee>, IEmployeeDal
    {
        public EmployeeDal(EmpContext context) : base(context)
        {
        }
    }
}
