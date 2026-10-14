using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //https;//localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: https:/localhost:portnumber/api/students
        //when you point to the above URL the control come to the below action method and executes the code
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "John", "Jane", "Mark", "Emily", "David" };
            return Ok(studentNames);
            //here the returnining names back to the swaggers
            //this  creates tje okay result object that produces and empty status with 200 okay responses

        }
    }
}
