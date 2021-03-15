using EFCore;
using EFCore.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service.Impl
{
    public class BaseService
    {
        private IRepositoryFactory _repositoryFactory;
        private IDbContext _context;
        public BaseService(IRepositoryFactory repositoryFactory, IDbContext dbcontext)
        {
            this._repositoryFactory = repositoryFactory;
            this._context = dbcontext;
        }
    }
}
