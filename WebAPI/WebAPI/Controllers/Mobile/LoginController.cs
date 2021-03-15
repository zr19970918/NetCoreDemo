using Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers.Mobile
{
    [Route("api/mobile/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "mobile")]
    public class LoginController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public LoginController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet,Route("GetTest")]
        public string GetTest(string Name)
        {
            return $"欢迎:{Name}光临我的API";
        }

        [HttpGet, Route("GetList")]
        public List<Student> GetList(string Name)
        {
            return _studentService.GetList();
        }
    }
}
