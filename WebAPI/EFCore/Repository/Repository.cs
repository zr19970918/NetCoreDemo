using EFCore.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EFCore.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private EFDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(IDbContext dbContext)
        {
            _dbContext = (EFDbContext)dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        //public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        //{
        //    if (_dbContext.Database.CurrentTransaction == null)
        //    {
        //        _dbContext.Database.BeginTransactionAsync(isolationLevel);
        //    }
        //}

        public void Commit()
        {
            var transaction = _dbContext.Database.CurrentTransaction;
            if (transaction != null)
            {
                try
                {
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Rollback()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                _dbContext.Database.CurrentTransaction.Rollback();
            }
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }


        public IQueryable<T> Entities
        {
            get { return _dbSet.AsNoTracking(); }
        }

        public IQueryable<T> TrackEntities
        {
            get { return _dbSet; }
        }

        public T Add(T entity, bool isSave = true)
        {
            _dbSet.Add(entity);
            if (isSave)
            {
                SaveChanges();
            }
            return entity;
        }

        public void AddRange(IEnumerable<T> entitys, bool isSave = true)
        {
            _dbSet.AddRange(entitys);
            if (isSave)
            {
                SaveChanges();
            }
        }

        public void Delete(T entity, bool isSave = true)
        {
            _dbSet.Remove(entity);
            if (isSave)
            {
                SaveChanges();
            }
        }

        public void Delete(bool isSave = true, params T[] entitys)
        {
            _dbSet.RemoveRange(entitys);
            if (isSave)
            {
                SaveChanges();
            }
        }

        public void Delete(object id, bool isSave = true)
        {
            _dbSet.Remove(_dbSet.Find(id));
            if (isSave)
            {
                SaveChanges();
            }
        }

        public void Delete(Expression<Func<T, bool>> @where, bool isSave = true)
        {
            T[] entitys = _dbSet.Where<T>(@where).ToArray();
            if (entitys.Length > 0)
            {
                _dbSet.RemoveRange(entitys);
            }
            if (isSave)
            {
                SaveChanges();
            }
        }

        public void Update(T entity, bool isSave = true)
        {
            var entry = _dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }
            if (isSave)
            {
                SaveChanges();
            }
        }

        public void Update(bool isSave = true, params T[] entitys)
        {
            var entry = _dbContext.Entry(entitys);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }
            if (isSave)
            {
                SaveChanges();
            }
        }

        public bool Any(Expression<Func<T, bool>> @where)
        {
            return _dbSet.AsNoTracking().Any(@where);
        }

        public int Count()
        {
            return _dbSet.AsNoTracking().Count();
        }

        public int Count(Expression<Func<T, bool>> @where)
        {
            return _dbSet.AsNoTracking().Count(@where);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> @where)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(@where);
        }

        public T FirstOrDefault<TOrder>(Expression<Func<T, bool>> @where, Expression<Func<T, TOrder>> order, bool isDesc = false)
        {
            if (isDesc)
            {
                return _dbSet.AsNoTracking().OrderByDescending(order).FirstOrDefault(@where);
            }
            else
            {
                return _dbSet.AsNoTracking().OrderBy(order).FirstOrDefault(@where);
            }
        }

        public IQueryable<T> Distinct(Expression<Func<T, bool>> @where)
        {
            return _dbSet.AsNoTracking().Where(@where).Distinct();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> @where)
        {
            return _dbSet.Where(@where);
        }

        public IQueryable<T> Where<TOrder>(Expression<Func<T, bool>> @where, Expression<Func<T, TOrder>> order, bool isDesc = false)
        {
            if (isDesc)
            {
                return _dbSet.Where(@where).OrderByDescending(order);
            }
            else
            {
                return _dbSet.Where(@where).OrderBy(order);
            }
        }

        public IEnumerable<T> Where<TOrder>(Func<T, bool> @where, Func<T, TOrder> order, int pageIndex, int pageSize, out int count, bool isDesc = false)
        {
            count = Count();
            if (isDesc)
            {
                return _dbSet.Where(@where).OrderByDescending(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return _dbSet.Where(@where).OrderBy(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        public IQueryable<T> Where<TOrder>(Expression<Func<T, bool>> @where, Expression<Func<T, TOrder>> order, int pageIndex, int pageSize, out int count, bool isDesc = false)
        {
            count = Count();
            if (isDesc)
            {
                return _dbSet.Where(@where).OrderByDescending(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return _dbSet.Where(@where).OrderBy(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<T> GetAll<TOrder>(Expression<Func<T, TOrder>> order, bool isDesc = false)
        {
            if (isDesc)
            {
                return _dbSet.AsNoTracking().OrderByDescending(order);
            }
            else
            {
                return _dbSet.AsNoTracking().OrderBy(order);
            }
        }

        public T GetById<Ttype>(Ttype id)
        {
            return _dbSet.Find(id);
        }

        public Ttype Max<Ttype>(Expression<Func<T, Ttype>> column)
        {
            if (_dbSet.AsNoTracking().Any())
            {
                return _dbSet.AsNoTracking().Max<T, Ttype>(column);
            }
            return default(Ttype);
        }

        public Ttype Max<Ttype>(Expression<Func<T, Ttype>> column, Expression<Func<T, bool>> @where)
        {
            if (_dbSet.AsNoTracking().Any(@where))
            {
                return _dbSet.AsNoTracking().Where(@where).Max<T, Ttype>(column);
            }
            return default(Ttype);
        }

        public Ttype Min<Ttype>(Expression<Func<T, Ttype>> column)
        {
            if (_dbSet.AsNoTracking().Any())
            {
                return _dbSet.AsNoTracking().Min<T, Ttype>(column);
            }
            return default(Ttype);
        }

        public Ttype Min<Ttype>(Expression<Func<T, Ttype>> column, Expression<Func<T, bool>> @where)
        {
            if (_dbSet.AsNoTracking().Any(@where))
            {
                return _dbSet.AsNoTracking().Where(@where).Min<T, Ttype>(column);
            }
            return default(Ttype);
        }

        public TType Sum<TType>(Expression<Func<T, TType>> selector, Expression<Func<T, bool>> @where) where TType : new()
        {
            object result = 0;

            if (new TType().GetType() == typeof(decimal))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, decimal>>);
            }
            if (new TType().GetType() == typeof(decimal?))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, decimal?>>);
            }
            if (new TType().GetType() == typeof(double))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, double>>);
            }
            if (new TType().GetType() == typeof(double?))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, double?>>);
            }
            if (new TType().GetType() == typeof(float))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, float>>);
            }
            if (new TType().GetType() == typeof(float?))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, float?>>);
            }
            if (new TType().GetType() == typeof(int))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, int>>);
            }
            if (new TType().GetType() == typeof(int?))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, int?>>);
            }
            if (new TType().GetType() == typeof(long))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, long>>);
            }
            if (new TType().GetType() == typeof(long?))
            {
                result = _dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, long?>>);
            }
            return (TType)result;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IEnumerable<T> BulkInsert(IEnumerable<T> entities)
        {
            var bulkInsert = entities as T[] ?? entities.ToArray();
            _dbSet.AddRange(bulkInsert);
            return bulkInsert;
        }

        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Any(predicate);
        }
    }
}
