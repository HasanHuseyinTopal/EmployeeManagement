using DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class GenericRepositoryDal<TEntity> : IGenericRepositoryDal<TEntity> where TEntity : class, new()
    {
        EmpContext _context;
        DbSet<TEntity> query;
        public GenericRepositoryDal(EmpContext context)
        {
            _context = context;
            query = _context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
             query.Add(entity);
            
        }

        public void Delete(TEntity entity)
        {
            query.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            query.Update(entity);
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            query.AsNoTracking();
            return filter == null ? query.AsQueryable() : query.Where(filter).AsQueryable();
        }

        public TEntity GetOne(Expression<Func<TEntity, bool>> filter)
        {
            return  query.FirstOrDefault(filter);
        }
    }
}
