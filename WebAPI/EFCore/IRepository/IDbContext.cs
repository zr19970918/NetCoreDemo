using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore
{
    public interface IDbContext
    {
        int SaveChanges();
        int ExecuteSqlCommand(string sql, params object[] paraObjects);
    }
}
