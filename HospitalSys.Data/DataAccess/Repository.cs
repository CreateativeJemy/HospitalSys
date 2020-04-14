using HospitalSys.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalSys.Data.DataAccess
{
    public class Repository<TEntity> where TEntity : class
    {
        public DbSet<TEntity> dbSet;
        private readonly ApplicationDbContext dbcontext;

        public Repository(ApplicationDbContext context)
        {
            dbcontext = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return dbcontext.Set<TEntity>();
        }

        public virtual void Delete(TEntity entity)
        {
            dbcontext.Set<TEntity>().Remove(entity);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return dbcontext.Set<TEntity>().Add(entity).Entity;
        }

        public virtual void Update(TEntity entity)
        {
            dbcontext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            dbcontext.Set<TEntity>().RemoveRange(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            dbcontext.Set<TEntity>().AddRange(entity);
            dbcontext.SaveChanges();
        }
        public virtual void UpdateRange(IEnumerable<TEntity> entity)
        {
            dbcontext.Set<TEntity>().UpdateRange(entity);
        }
    }

}
