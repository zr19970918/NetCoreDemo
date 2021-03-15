using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.IRepository
{ 
     public interface IRepositoryFactory
    {
        IRepository<T> CreateRepository<T>(IDbContext context) where T : class;
    }
}
