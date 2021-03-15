using Microsoft.EntityFrameworkCore;
using Model.Entity;
using System;

namespace EFCore
{
    public class EFDbContext:DbContext, IDbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options):base(options)
        {

        }

        public DbSet<Student> Student { get; set; }

        public int ExecuteSqlCommand(string sql, params object[] paraObjects)
        {
            throw new NotImplementedException();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges(true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
