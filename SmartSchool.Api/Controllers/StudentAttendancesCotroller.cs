using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendancesCotroller : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentAttendanceService _studentAttendance;

        // The constructor for the controller
        public StudentAttendancesCotroller(IUnitOfWork unitOfWork, IStudentAttendanceService studentAttendance)
        {
            _unitOfWork = unitOfWork;
            _studentAttendance = studentAttendance;

        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllStudentAttendance")]
        public async Task<IActionResult> GetAllStudentAttendance()
        {
            var result = await _studentAttendance.GetAllStudentAttendance();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetStudentAttendanceById")]
        public async Task<IActionResult> GetStudentAttendanceById(int id)
        {
            var result = await _studentAttendance.GetByIdStudentAttendance(id);
            return Ok(result);
        }

    }
}
