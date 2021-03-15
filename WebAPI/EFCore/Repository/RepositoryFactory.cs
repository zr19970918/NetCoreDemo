using EFCore.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<T> CreateRepository<T>(IDbContext context) where T : class
        {
            return new Repository<T>(context);
        }
    }
}
