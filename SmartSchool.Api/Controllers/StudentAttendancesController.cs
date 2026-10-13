using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentAttendancesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentAttendanceService _studentAttendance;

        // The constructor for the controller
        public StudentAttendancesController(IUnitOfWork unitOfWork, IStudentAttendanceService studentAttendance)
        {
            _unitOfWork = unitOfWork;
            _studentAttendance = studentAttendance;

        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllStudentAttendance")]
        public async Task<IActionResult> GetAllStudentAttendance()
        {
            var result = await _studentAttendance.GetAllStudentAttendance();
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetStudentAttendanceById")]
        public async Task<IActionResult> GetStudentAttendanceById(int id)
        {
            var result = await _studentAttendance.GetByIdStudentAttendance(id);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

        [HttpPost("AddStudentAttendance")]
        public async Task<IActionResult> AddStudentAttendance([FromBody] StudentAttendanceDto dto)
        {
            var result = await _studentAttendance.AddStudentAttendance(dto);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("UpdateStudentAttendance")]
        public async Task<IActionResult> UpdateStudentAttendance([FromBody] StudentAttendanceDto dto)
        {
            var result = await _studentAttendance.UpdateStudentAttendance(dto);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("DeleteStudentAttendance")]
        public async Task<IActionResult> DeleteStudentAttendance(int id)
        {
            var result = await _studentAttendance.DeleteStudentAttendance(id);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

    }
}
