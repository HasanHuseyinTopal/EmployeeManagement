using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IUnitOfWorkDal
    {
        public IEmployeeDal employeeDal { get; set; }
        public void Save();
    }
}
