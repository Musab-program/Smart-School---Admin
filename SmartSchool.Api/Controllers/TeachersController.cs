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
    public class TeachersController : ControllerBase
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITeacherService _teacherService;

            // The constructor for the controller
            public TeachersController(IUnitOfWork unitOfWork, ITeacherService teacherService)
            {
                _unitOfWork = unitOfWork;
                _teacherService = teacherService;
             }

            // End Point For add Element In This Domin Class
            [HttpPost("AddTeacher")]
            public async Task<IActionResult> AddTeacher([FromBody] TeacherDto dto)
            {
                var result = await _teacherService.AddTeacher(dto);
                return Ok(result);
            }


            // End Point For Get All Elements In This Domin Class
            [HttpGet("GetAllTeacher")]
            public async Task<IActionResult> GetAllTeacher()
            {
                var result = await _teacherService.GetAllTeachers();
                return Ok(result);
            }


            // End Point For Get  Element by id In This Domin Class
            [HttpGet("GetTeacherById")]
            public async Task<IActionResult> GetTeacherById(int id)
            {
                var result = await _teacherService.GetByIdTeacher(id);
                return Ok(result);
            }


            // End Point For update Elements In This Domin Class
            [HttpPut("UpdateTeachers")]
            public async Task<IActionResult> UpdateTeachers([FromBody] TeacherDto dto)
            {
                var result = await _teacherService.UpdateTeacher(dto);
                return Ok(result);
            }


            // End Point For delete Element In This Domin Class
            [HttpDelete("DeleteTeacher")]
            public async Task<IActionResult> DeleteTeacher(int id)
            {
                var result = await _teacherService.DeleteTeacher(id);
                return Ok(result);
            }
        }
}
