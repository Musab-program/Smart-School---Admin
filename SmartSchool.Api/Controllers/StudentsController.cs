using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Main.InterFaces;
using SmartSchool.Main.Dtos;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentService _studentService;

        // The constructor for the controller
        public StudentsController(IUnitOfWork unitOfWork, IStudentService studentService)
        {
            _unitOfWork = unitOfWork;
            _studentService = studentService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentDto dto)
        {
            var result = await _studentService.AddStudent(dto);
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllStudent")]
        public async Task<IActionResult> GetAllStudent()
        {
            var result = await _studentService.GetAllStudents();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await _studentService.GetByIdStudent(id);
            return Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateStudents")]
        public async Task<IActionResult> UpdateStudents([FromBody] StudentDto dto)
        {
            var result = await _studentService.UpdateStudent(dto);
            return Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudent(id);
            return Ok(result);
        }
    }
}
