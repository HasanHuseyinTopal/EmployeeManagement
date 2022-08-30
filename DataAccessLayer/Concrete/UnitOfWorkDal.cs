using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class UnitOfWorkDal : IUnitOfWorkDal
    {
        EmpContext _empContext;
        public IEmployeeDal employeeDal { get; set; }

        public UnitOfWorkDal(EmpContext empContext)
        {
            _empContext = empContext;
            employeeDal = new EmployeeDal(_empContext);

        }
        public void Save()
        {
            _empContext.SaveChanges();
        }
    }
}
