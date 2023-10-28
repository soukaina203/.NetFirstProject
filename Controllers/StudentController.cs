using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
        string[] allStudents = new string[] { "anal", "arrow" };
            
            return Ok(allStudents);
        }
    }
}