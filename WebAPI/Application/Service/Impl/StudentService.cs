using EFCore;
using EFCore.IRepository;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Service.Impl
{
    public class StudentService : BaseService, IStudentService
    {
        private readonly IRepository<Student> _studentServiceRepo;
        public StudentService(IRepositoryFactory repositoryFactory,IDbContext dbContext):base(repositoryFactory, dbContext)
        {
            _studentServiceRepo = repositoryFactory.CreateRepository<Student>(dbContext);
        }
        public List<Student> GetList()
        {

            var list = _studentServiceRepo.GetAll().ToList();
            return list;
        }
    }
}
