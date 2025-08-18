using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherHolidayController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeacherHolidayService _teacherHolidayService;

        // The constructor for the controller 
        public TeacherHolidayController(IUnitOfWork unitOfWork, ITeacherHolidayService teacherHolidayService)
        {
            _unitOfWork = unitOfWork;
            _teacherHolidayService = teacherHolidayService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllTeacherHolidays")]
        public async Task<IActionResult> GetAllTeacherHolidays()
        {
            var result = await _teacherHolidayService.GetAllTeacherHolidays();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetTeacherHolidayById/{id}")]
        public async Task<IActionResult> GetTeacherHolidayById(int id)
        {
            var result = await _teacherHolidayService.GetTeacherHolidayById(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddTeacherHoliday")]
        public async Task<IActionResult> AddTeacherHoliday([FromBody] TeacherHolidayDto dto)
        {
            var result = await _teacherHolidayService.AddTeacherHoliday(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateTeacherHoliday")]
        public async Task<IActionResult> UpdateTeacherHoliday([FromBody] TeacherHolidayDto dto)
        {
            var result = await _teacherHolidayService.UpdateTeacherHoliday(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteTeacherHoliday")]
        public async Task<IActionResult> DeleteTeacherHoliday(int id)
        {
            var result = await _teacherHolidayService.DeleteTeacherHoliday(id);
            return Ok(result);
        }
    }
}
