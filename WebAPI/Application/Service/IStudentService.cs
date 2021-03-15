using Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public interface IStudentService
    {
        List<Student> GetList();
    }
}
